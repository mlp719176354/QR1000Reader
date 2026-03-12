using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QR1000Reader
{
    public partial class Form1 : Form
    {
        // WebSocket 客户端
        private SinosecuWebSocketClient? _webSocketClient;
        private string _webSocketUri = "ws://127.0.0.1:90/echo";

        // 状态标志
        private bool m_bIsConnected = false;
        private bool m_bManualInput = false;
        private bool m_bIsReading = false;

        // 定时器用于自动检测
        private System.Windows.Forms.Timer autoTimer;

        // 存储识别结果
        private Dictionary<string, string>? _currentCardData;
        private string? _currentImageBase64;

        public Form1()
        {
            InitializeComponent();
            InitializeApp();
            InitializeTimer();
            InitializeInputHandlers();

            // 设置窗体启动时最大化
            this.WindowState = FormWindowState.Maximized;
            
            // 调整 DataGridView 大小以适应窗口
            this.Resize += Form1_Resize;
            AdjustDataGridViewSize();
        }

        private void InitializeTimer()
        {
            autoTimer = new System.Windows.Forms.Timer();
            autoTimer.Interval = 500; // 500ms 检测一次
            autoTimer.Tick += AutoTimer_Tick;
            autoTimer.Start();
        }

        /// <summary>
        /// 初始化输入框事件处理
        /// </summary>
        private void InitializeInputHandlers()
        {
            // 票号输入框按 Enter 键触发保存
            txtTicketNumber.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave_Click(this, EventArgs.Empty);
                    e.SuppressKeyPress = true;
                }
            };

            // 航班时间输入框只允许数字
            txtFlightHour.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
            txtFlightMinute.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // 航班时间输入框自动跳转
            txtFlightHour.TextChanged += (s, e) =>
            {
                if (txtFlightHour.Text.Length >= 2)
                {
                    txtFlightMinute.Focus();
                }
            };
        }

        private void InitializeApp()
        {
            // 初始化配置
            string configPath = Path.Combine(Application.StartupPath, "config.yaml");
            ConfigHelper.Load(configPath);

            // 初始化数据库
            string dbPath = Path.Combine(Application.StartupPath, "PassengerData.db");
            DatabaseHelper.Initialize(dbPath);

            // 从配置读取 WebSocket URI
            _webSocketUri = ConfigHelper.GetWebSocketUri() ?? "ws://127.0.0.1:90/echo";

            // 填充下拉框
            FillComboBoxes();

            // 设置默认日期为今天
            dtpFlightDate.Value = DateTime.Today;

            // 设置 UserID
            txtUserID.Text = "1001";

            // 加载今天的记录
            LoadData();

            // 连接 WebSocket
            ConnectWebSocket();
        }

        private void FillComboBoxes()
        {
            // 港口 - 添加空选项
            var ports = ConfigHelper.GetPorts();
            var portsWithEmpty = new List<PortInfo>();
            portsWithEmpty.Add(new PortInfo { Code = "", Name = "" });
            portsWithEmpty.AddRange(ports);
            
            cmbDeparturePort.DataSource = new BindingSource(portsWithEmpty, null);
            cmbDeparturePort.DisplayMember = "Name";
            cmbDeparturePort.ValueMember = "Code";
            cmbDeparturePort.SelectedIndex = 0; // 默认为空

            cmbArrivalPort.DataSource = new BindingSource(portsWithEmpty, null);
            cmbArrivalPort.DisplayMember = "Name";
            cmbArrivalPort.ValueMember = "Code";
            cmbArrivalPort.SelectedIndex = 0; // 默认为空

            // 证件类型 - 添加"其他"选项
            var docTypes = ConfigHelper.GetDocumentTypes();
            docTypes.Add(new DocumentTypeInfo { Code = "OTHER", Name = "其他" });
            cmbDocumentType.DataSource = new BindingSource(docTypes, null);
            cmbDocumentType.DisplayMember = "Name";
            cmbDocumentType.ValueMember = "Code";

            // 航班时间输入框初始化为空白
            txtFlightHour.Text = "";
            txtFlightMinute.Text = "";
        }

        private async void ConnectWebSocket()
        {
            try
            {
                _webSocketClient = new SinosecuWebSocketClient(_webSocketUri);

                // 订阅事件
                _webSocketClient.ConnectionStateChanged += WebSocketClient_ConnectionStateChanged;
                _webSocketClient.CardDataReceived += WebSocketClient_CardDataReceived;
                _webSocketClient.DeviceStatusChanged += WebSocketClient_DeviceStatusChanged;

                // 连接
                await _webSocketClient.ConnectAsync();

                txtRecognizedText.AppendText($"正在连接 WebSocket: {_webSocketUri}\r\n");
            }
            catch (Exception ex)
            {
                lblDeviceStatus.Text = $"WebSocket 连接失败：{ex.Message}";
                lblDeviceStatus.ForeColor = Color.Red;
                txtRecognizedText.AppendText($"WebSocket 连接失败：{ex.Message}\r\n");
            }
        }

        private void WebSocketClient_ConnectionStateChanged(object? sender, ConnectionStateChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => WebSocketClient_ConnectionStateChanged(sender, e)));
                return;
            }

            m_bIsConnected = e.IsConnected;

            if (e.IsConnected)
            {
                lblDeviceStatus.Text = "设备已连接 (WebSocket)";
                lblDeviceStatus.ForeColor = Color.Green;
                txtRecognizedText.AppendText("WebSocket 连接成功，系统初始化完成\r\n");
                txtRecognizedText.AppendText("请将证件放在读卡器上，检测到证件后会自动读取...\r\n\r\n");

                // 连接成功后设置参数
                InitializeWebSocketSettings();
            }
            else
            {
                lblDeviceStatus.Text = "设备未连接";
                lblDeviceStatus.ForeColor = Color.Red;
                txtRecognizedText.AppendText($"WebSocket 连接断开：{e.Reason}\r\n");
            }
        }

        private async void InitializeWebSocketSettings()
        {
            if (_webSocketClient == null || !_webSocketClient.IsConnected) return;

            try
            {
                // 设置默认参数
                await _webSocketClient.SetDefaultSettingsAsync();

                // 启用版面识别
                await _webSocketClient.SetReadVizAsync(true);

                // 启用证件检测通知（用于自动检测）
                await _webSocketClient.SetCardDetectNotifyAsync(true);

                // 启用回调模式
                await _webSocketClient.SetCallbackModeAsync(true);

                txtRecognizedText.AppendText("WebSocket 参数设置完成\r\n");
            }
            catch (Exception ex)
            {
                txtRecognizedText.AppendText($"设置参数失败：{ex.Message}\r\n");
            }
        }

        private void WebSocketClient_CardDataReceived(object? sender, CardDataEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => WebSocketClient_CardDataReceived(sender, e)));
                return;
            }

            // 处理卡片数据
            txtRecognizedText.AppendText($"收到数据：{e.Command} - {e.Operand}\r\n");

            if (e.Param.HasValue && e.Param.Value.ValueKind == System.Text.Json.JsonValueKind.Object)
            {
                _currentCardData = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var param = e.Param.Value;

                // 遍历 JSON 对象，提取字段
                foreach (var prop in param.EnumerateObject())
                {
                    string value = prop.Value.ToString();
                    string propName = prop.Name.Trim();

                    // 检查是否是图像数据（参考 Sinosecu.html 的图像类型）
                    if (prop.Name.Equals("Image", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("ImageData", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("Base64", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("Photo", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("White", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("IR", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("UV", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("OcrHead", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("ChipHead", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("SidHead", StringComparison.OrdinalIgnoreCase))
                    {
                        if (value.Length > 1000) // 确认是 Base64 图像
                        {
                            _currentImageBase64 = value;
                            txtRecognizedText.AppendText($"[图像数据 - {propName}]: {value.Length} 字节\r\n");
                            
                            // 显示图像到 pictureBox
                            DisplayImageFromBase64(value);
                        }
                    }
                    else
                    {
                        _currentCardData[propName] = value;
                        txtRecognizedText.AppendText($"{propName}: {value}\r\n");
                    }
                }

                // 自动填充字段
                if (_currentCardData.Count > 0)
                {
                    txtRecognizedText.AppendText("\r\n=== 开始自动填充字段 ===\r\n");
                    AutoFillFields(_currentCardData);
                    txtRecognizedText.AppendText("=== 自动填充完成 ===\r\n");
                    
                    // 打印 CARD_NAME 信息
                    if (_currentCardData.ContainsKey("CARD_NAME") && !string.IsNullOrEmpty(_currentCardData["CARD_NAME"]))
                    {
                        txtRecognizedText.AppendText($"CARD_NAME: {_currentCardData["CARD_NAME"]}\r\n\r\n");
                    }
                    else
                    {
                        txtRecognizedText.AppendText("\r\n");
                    }
                }
            }
            else if (e.Param.HasValue && e.Param.Value.ValueKind == System.Text.Json.JsonValueKind.String)
            {
                // 处理字符串类型的参数（可能是 Base64 图像）
                string paramValue = e.Param.Value.ToString();
                if (paramValue.Length > 1000) // 可能是 Base64 图像
                {
                    _currentImageBase64 = paramValue;
                    txtRecognizedText.AppendText("收到图像数据\r\n");
                    
                    // 显示图像到 pictureBox
                    DisplayImageFromBase64(paramValue);
                }
            }
        }

        /// <summary>
        /// 从 Base64 字符串显示图像到 pictureBox
        /// </summary>
        private void DisplayImageFromBase64(string base64String)
        {
            try
            {
                // 移除可能存在的头部（如 "data:image/jpeg;base64,"）
                string base64Data = base64String;
                if (base64String.Contains(","))
                {
                    base64Data = base64String.Substring(base64String.IndexOf(",") + 1);
                }

                byte[] imageBytes = Convert.FromBase64String(base64Data);
                using (var ms = new MemoryStream(imageBytes))
                {
                    // 先释放旧图像
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                        pictureBox.Image = null;
                    }

                    // 创建新图像
                    Image img = Image.FromStream(ms);
                    pictureBox.Image = img;
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    
                    txtRecognizedText.AppendText($"图像已显示：{img.Width}x{img.Height}\r\n");
                }
            }
            catch (Exception ex)
            {
                txtRecognizedText.AppendText($"图像显示失败：{ex.Message}\r\n");
                txtRecognizedText.AppendText($"Base64 长度：{base64String.Length}\r\n");
            }
        }

        private void WebSocketClient_DeviceStatusChanged(object? sender, DeviceStatusEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => WebSocketClient_DeviceStatusChanged(sender, e)));
                return;
            }

            txtRecognizedText.AppendText($"设备状态：{e.Status} - {e.Description}\r\n");

            // 如果设备状态表示有证件，触发读取
            if (m_bIsConnected && !m_bIsReading && rbtnReaderInput.Checked)
            {
                // 根据状态判断是否有证件
                if (e.Status.Contains("Ready") || e.Status.Contains("DocumentDetected"))
                {
                    m_bIsReading = true;
                    ReadCardAsync();
                }
            }
        }

        /// <summary>
        /// 自动定时器检测证件
        /// </summary>
        private void AutoTimer_Tick(object? sender, EventArgs e)
        {
            if (m_bIsReading) return; // 正在读取中
            if (!rbtnReaderInput.Checked) return; // 手动输入模式
            if (!m_bIsConnected) return;

            // WebSocket 模式下，由设备状态通知触发读取
            // 定时器仅用于重连检测
            if (_webSocketClient != null && !_webSocketClient.IsConnected)
            {
                // 尝试重连
                ConnectWebSocket();
            }
        }

        /// <summary>
        /// 读取证件 - WebSocket 方式
        /// </summary>
        private async void ReadCardAsync()
        {
            if (_webSocketClient == null || !_webSocketClient.IsConnected)
            {
                txtRecognizedText.AppendText("WebSocket 未连接，无法读卡\r\n");
                m_bIsReading = false;
                return;
            }

            try
            {
                // 重置数据
                _currentCardData = null;
                _currentImageBase64 = null;
                txtRecognizedText.Clear();

                txtRecognizedText.AppendText("开始读取证件...\r\n");

                // 发送扫描文档命令
                await _webSocketClient.ScanDocumentAsync();

                // 等待数据接收（通过事件处理）
                // 设置超时
                var timeout = Task.Delay(10000);
                var waitTask = WaitForCardData();

                if (await Task.WhenAny(waitTask, timeout) == waitTask)
                {
                    // 成功接收数据
                    if (_currentCardData != null && _currentCardData.Count > 0)
                    {
                        txtRecognizedText.AppendText("\r\n=== 读取成功 ===\r\n");

                        // 获取图像
                        await GetImageAsync();

                        // 自动跳到票务字段
                        txtTicketNumber.Focus();
                    }
                    else
                    {
                        txtRecognizedText.AppendText("读卡失败：未收到数据\r\n");
                        cmbDocumentType.SelectedValue = "OTHER";
                    }
                }
                else
                {
                    txtRecognizedText.AppendText("读卡失败：超时\r\n");
                    cmbDocumentType.SelectedValue = "OTHER";
                }
            }
            catch (Exception ex)
            {
                txtRecognizedText.AppendText($"读卡错误：{ex.Message}\r\n");
            }
            finally
            {
                m_bIsReading = false;
            }
        }

        /// <summary>
        /// 等待卡片数据
        /// </summary>
        private async Task WaitForCardData()
        {
            while (_currentCardData == null && m_bIsReading)
            {
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 获取图像
        /// </summary>
        private async Task GetImageAsync()
        {
            if (_webSocketClient == null || !_webSocketClient.IsConnected) return;

            try
            {
                await _webSocketClient.GetImageAsync();

                // 图像通过 CardDataReceived 事件处理
                if (!string.IsNullOrEmpty(_currentImageBase64))
                {
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(_currentImageBase64);
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            pictureBox.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        txtRecognizedText.AppendText($"图像加载失败：{ex.Message}\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                txtRecognizedText.AppendText($"获取图像失败：{ex.Message}\r\n");
            }
        }

        private void AutoFillFields(Dictionary<string, string> fields)
        {
            txtRecognizedText.AppendText("开始填充旅客信息...\r\n");

            // 1. 首先尝试从 MRZ 解析数据
            var mrzResult = MrzParser.ParseFromFields(fields);
            
            // 2. 旅客姓名 - 优先使用 MRZ 解析结果
            string name = "";
            string mrzName = "";
            
            // 使用 MRZ 解析的姓名
            if (mrzResult.IsValid && !string.IsNullOrEmpty(mrzResult.FullName))
            {
                mrzName = mrzResult.FullName;
                txtRecognizedText.AppendText($"MRZ 识别到姓名：{mrzName}\r\n");
            }
            
            // 优先使用中文姓名
            if (fields.ContainsKey("中文姓名") && !string.IsNullOrEmpty(fields["中文姓名"]))
            {
                name = fields["中文姓名"].Trim();
                txtRecognizedText.AppendText($"识别到中文姓名：{name}\r\n");
            }
            // 否则使用英文姓名
            else if (fields.ContainsKey("英文姓名") && !string.IsNullOrEmpty(fields["英文姓名"]))
            {
                name = fields["英文姓名"].Trim();
                txtRecognizedText.AppendText($"识别到英文姓名：{name}\r\n");
            }
            // 使用 MRZ 姓名（如果 VIZ 姓名为空）
            else if (!string.IsNullOrEmpty(mrzName))
            {
                name = mrzName;
                txtRecognizedText.AppendText($"使用 MRZ 姓名：{name}\r\n");
            }
            // 使用 Surname + GivenNames 组合
            else if (fields.ContainsKey("Surname") && fields.ContainsKey("GivenNames"))
            {
                name = fields["Surname"].Trim() + " " + fields["GivenNames"].Trim();
                txtRecognizedText.AppendText($"识别到英文姓名：{name}\r\n");
            }
            // 使用拼音姓名
            else if (fields.ContainsKey("拼音姓名") && !string.IsNullOrEmpty(fields["拼音姓名"]))
            {
                name = fields["拼音姓名"].Trim();
                txtRecognizedText.AppendText($"识别到拼音姓名：{name}\r\n");
            }
            // 使用 Name 字段
            else if (fields.ContainsKey("Name") && !string.IsNullOrEmpty(fields["Name"]))
            {
                name = fields["Name"].Trim();
                txtRecognizedText.AppendText($"识别到姓名：{name}\r\n");
            }

            if (!string.IsNullOrEmpty(name))
            {
                txtPassengerName.Text = name;
                txtRecognizedText.AppendText($"已填充姓名：{name}\r\n");
            }

            // 3. 证件号码 - 优先使用 MRZ 解析结果
            string number = "";
            string mrzDocNumber = "";
            
            // 使用 MRZ 解析的证件号码
            if (mrzResult.IsValid && !string.IsNullOrEmpty(mrzResult.DocumentNumber))
            {
                mrzDocNumber = mrzResult.DocumentNumber;
                txtRecognizedText.AppendText($"MRZ 识别到证件号码：{mrzDocNumber}\r\n");
            }

            // 优先使用证件号码
            if (fields.ContainsKey("证件号码") && !string.IsNullOrEmpty(fields["证件号码"]))
            {
                number = fields["证件号码"].Trim();
                txtRecognizedText.AppendText($"识别到证件号码：{number}\r\n");
            }
            // 使用身份号码（澳门身份证）
            else if (fields.ContainsKey("身份号码") && !string.IsNullOrEmpty(fields["身份号码"]))
            {
                number = fields["身份号码"].Trim();
                txtRecognizedText.AppendText($"识别到身份号码：{number}\r\n");
            }
            // 使用身份证号码
            else if (fields.ContainsKey("身份证件号码") && !string.IsNullOrEmpty(fields["身份证件号码"]))
            {
                number = fields["身份证件号码"].Trim();
                txtRecognizedText.AppendText($"识别到身份证件号码：{number}\r\n");
            }
            // 使用身份证号码（带括号）
            else if (fields.ContainsKey("身份證號碼") && !string.IsNullOrEmpty(fields["身份證號碼"]))
            {
                number = fields["身份證號碼"].Trim();
                txtRecognizedText.AppendText($"识别到身份證號碼：{number}\r\n");
            }
            // 使用护照号码
            else if (fields.ContainsKey("护照号码") && !string.IsNullOrEmpty(fields["护照号码"]))
            {
                number = fields["护照号码"].Trim();
                txtRecognizedText.AppendText($"识别到护照号码：{number}\r\n");
            }
            // 使用 MRZ 证件号码（如果 VIZ 号码为空）
            else if (!string.IsNullOrEmpty(mrzDocNumber))
            {
                number = mrzDocNumber;
                txtRecognizedText.AppendText($"使用 MRZ 证件号码：{number}\r\n");
            }
            // 使用 Number 字段
            else if (fields.ContainsKey("Number") && !string.IsNullOrEmpty(fields["Number"]))
            {
                number = fields["Number"].Trim();
                txtRecognizedText.AppendText($"识别到证件号码：{number}\r\n");
            }
            // 使用 DocumentNo 字段
            else if (fields.ContainsKey("DocumentNo") && !string.IsNullOrEmpty(fields["DocumentNo"]))
            {
                number = fields["DocumentNo"].Trim();
                txtRecognizedText.AppendText($"识别到证件号码：{number}\r\n");
            }

            if (!string.IsNullOrEmpty(number))
            {
                // 清理号码中的特殊字符（如括号）
                number = System.Text.RegularExpressions.Regex.Replace(number, @"[()（）]", "");
                txtDocumentNumber.Text = number;
                txtRecognizedText.AppendText($"已填充证件号码：{number}\r\n");
            }

            // 4. 检查是否有必填字段为空
            bool hasMissingFields = string.IsNullOrEmpty(name) || string.IsNullOrEmpty(number);
            
            if (hasMissingFields)
            {
                txtRecognizedText.AppendText("\r\n=== 警告：部分字段无法识别 ===\r\n");
                if (string.IsNullOrEmpty(name))
                {
                    txtRecognizedText.AppendText("警告：旅客姓名为空\r\n");
                }
                if (string.IsNullOrEmpty(number))
                {
                    txtRecognizedText.AppendText("警告：证件号码为空\r\n");
                }
                txtRecognizedText.AppendText("请手动输入或检查证件放置是否正确\r\n");
                
                // 弹出提示
                MessageBox.Show("部分必填字段无法自动识别，请手动输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 证件类型识别
            IdentifyDocumentType(fields);

            // 国家/地区识别（Passport 类型）
            string nationality = "";
            if (fields.ContainsKey("持证人国籍代码") && !string.IsNullOrEmpty(fields["持证人国籍代码"]))
            {
                nationality = fields["持证人国籍代码"].Trim();
            }
            else if (fields.ContainsKey("国籍") && !string.IsNullOrEmpty(fields["国籍"]))
            {
                nationality = fields["国籍"].Trim();
            }
            else if (fields.ContainsKey("Nationality") && !string.IsNullOrEmpty(fields["Nationality"]))
            {
                nationality = fields["Nationality"].Trim();
            }
            else if (fields.ContainsKey("签发国代码") && !string.IsNullOrEmpty(fields["签发国代码"]))
            {
                nationality = fields["签发国代码"].Trim();
            }
            else if (mrzResult.IsValid && !string.IsNullOrEmpty(mrzResult.Nationality))
            {
                nationality = mrzResult.Nationality;
                txtRecognizedText.AppendText($"MRZ 识别到国籍：{nationality}\r\n");
            }

            if (!string.IsNullOrEmpty(nationality))
            {
                var countries = ConfigHelper.GetPopularCountries();
                var country = countries.Find(c => c.Code == nationality);
                if (country == null)
                {
                    country = countries.Find(c => c.Name.Contains(nationality) || nationality.Contains(c.Code));
                }

                if (country != null)
                {
                    txtRecognizedText.AppendText($"识别国家：{country.Name} ({country.Code})\r\n");
                }
            }

            // 填充完成后，将焦点设置到票号输入框
            txtTicketNumber.Focus();
        }

        /// <summary>
        /// 获取字段值，支持多个可能的字段名
        /// </summary>
        private string GetFieldValue(Dictionary<string, string> fields, params string[] possibleNames)
        {
            foreach (var name in possibleNames)
            {
                if (fields.ContainsKey(name) && !string.IsNullOrEmpty(fields[name]))
                {
                    return fields[name];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据识别结果自动设置证件类型
        /// </summary>
        private void IdentifyDocumentType(Dictionary<string, string> fields)
        {
            // 根据字段判断证件类型
            bool hasEEPFields = fields.ContainsKey("签发机关") ||
                               fields.ContainsKey("有效期") && (fields.ContainsKey("往") || fields.ContainsKey("港澳"));
            bool hasPassportFields = fields.ContainsKey("持证人国籍代码") ||
                                    fields.ContainsKey("国籍") ||
                                    fields.ContainsKey("Date of birth") ||
                                    fields.ContainsKey("Dateofbirth") ||
                                    fields.ContainsKey("P") ||
                                    (fields.ContainsKey("证件类型") && fields["证件类型"] == "P");
            bool hasHKIDFields = (fields.ContainsKey("中文姓名") || fields.ContainsKey("姓名")) &&
                                (fields.ContainsKey("身份证") || fields.ContainsKey("ID Card") || fields.ContainsKey("IDCard"));
            bool hasMOIDFields = fields.ContainsKey("澳門") ||
                                fields.ContainsKey("澳门") ||
                                (fields.ContainsKey("證件名稱") && fields["證件名稱"].Contains("澳門")) ||
                                (fields.ContainsKey("证件名称") && fields["证件名称"].Contains("澳门")) ||
                                (fields.ContainsKey("身份證號碼") && fields.ContainsKey("拼音姓名")) ||
                                (fields.ContainsKey("身份证号码") && fields.ContainsKey("拼音姓名")) ||
                                (fields.ContainsKey("身份号码") && fields.ContainsKey("英文名")) ||
                                (fields.ContainsKey("签发国代码") && fields["签发国代码"] == "MAC");
            bool hasCRFields = fields.ContainsKey("港澳居民来往内地通行证") ||
                              (fields.ContainsKey("CARD_NAME") && fields["CARD_NAME"].Contains("港澳居民"));
            bool hasXGZFields = hasCRFields && (fields.ContainsKey("本证有效期至") || fields.ContainsKey("身份证件号码"));
            bool hasTBPFields = fields.ContainsKey("台湾居民来往大陆通行证") ||
                               (fields.ContainsKey("CARD_NAME") && fields["CARD_NAME"].Contains("台湾居民来往大陆通行证"));
            bool hasGATFields = fields.ContainsKey("往来港澳通行证") ||
                               (fields.ContainsKey("CARD_NAME") && fields["CARD_NAME"].Contains("往来港澳通行证"));

            if (hasTBPFields)
            {
                cmbDocumentType.SelectedValue = "TBP";
                txtRecognizedText.AppendText("证件类型：台胞证\r\n");
            }
            else if (hasGATFields)
            {
                cmbDocumentType.SelectedValue = "EEP";
                txtRecognizedText.AppendText("证件类型：往来港澳通行证\r\n");
            }
            else if (hasXGZFields)
            {
                cmbDocumentType.SelectedValue = "XGZ";
                txtRecognizedText.AppendText("证件类型：回乡证\r\n");
            }
            else if (hasCRFields)
            {
                cmbDocumentType.SelectedValue = "EEP";
                txtRecognizedText.AppendText("证件类型：往来港澳通行证（CRT）\r\n");
            }
            else if (hasMOIDFields)
            {
                cmbDocumentType.SelectedValue = "MOID";
                txtRecognizedText.AppendText("证件类型：澳门身份证\r\n");
            }
            else if (hasPassportFields)
            {
                cmbDocumentType.SelectedValue = "PASSPORT";
                txtRecognizedText.AppendText("证件类型：外国护照\r\n");
            }
            else if (hasHKIDFields)
            {
                cmbDocumentType.SelectedValue = "HKID";
                txtRecognizedText.AppendText("证件类型：香港身份证\r\n");
            }
            else
            {
                cmbDocumentType.SelectedValue = "OTHER";
                txtRecognizedText.AppendText("证件类型：其他（无法自动识别）\r\n");
            }
        }

        private void btnReadCard_Click(object sender, EventArgs e)
        {
            if (m_bIsReading)
            {
                MessageBox.Show("正在读取中，请稍候...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!m_bIsConnected)
            {
                MessageBox.Show("WebSocket 未连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_bIsReading = true;
            try
            {
                ReadCardAsync();
            }
            catch (Exception ex)
            {
                txtRecognizedText.AppendText($"读卡错误：{ex.Message}\r\n");
                m_bIsReading = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. 验证出发港
            if (cmbDeparturePort.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(txtDepartureCode.Text))
            {
                MessageBox.Show("请选择出发港！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDeparturePort.Focus();
                return;
            }

            // 2. 验证到达港
            if (cmbArrivalPort.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(txtArrivalCode.Text))
            {
                MessageBox.Show("请选择到达港！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbArrivalPort.Focus();
                return;
            }

            // 3. 验证航班时间
            if (string.IsNullOrWhiteSpace(txtFlightHour.Text) || string.IsNullOrWhiteSpace(txtFlightMinute.Text))
            {
                MessageBox.Show("请填写航班时间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFlightHour.Focus();
                return;
            }

            // 4. 验证证件类型
            if (cmbDocumentType.SelectedValue == null)
            {
                MessageBox.Show("请选择证件类型！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDocumentType.Focus();
                return;
            }

            // 5. 验证旅客姓名和证件号码是否为空
            if (string.IsNullOrWhiteSpace(txtPassengerName.Text))
            {
                MessageBox.Show("旅客姓名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassengerName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDocumentNumber.Text))
            {
                MessageBox.Show("证件号码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumentNumber.Focus();
                return;
            }

            // 6. 检查是否与上一条记录重复
            var lastRecord = DatabaseHelper.GetLastRecord();
            if (lastRecord != null)
            {
                bool isSameName = string.Equals(txtPassengerName.Text.Trim(), lastRecord.PassengerName?.Trim(), StringComparison.OrdinalIgnoreCase);
                bool isSameDocNumber = string.Equals(txtDocumentNumber.Text.Trim(), lastRecord.DocumentNumber?.Trim(), StringComparison.OrdinalIgnoreCase);

                if (isSameName && isSameDocNumber)
                {
                    MessageBox.Show($"旅客姓名和证件号码与上一条记录相同，重复保存！\r\n\r\n上一条记录：{lastRecord.PassengerName} - {lastRecord.DocumentNumber}", 
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 解析证件有效期
            DateTime? expiryDate = null;
            string documentStatus = "";
            
            // 从识别文本中解析"本证有效期至：YYYY-MM-DD"
            if (_currentCardData != null && _currentCardData.ContainsKey("本证有效期至"))
            {
                string expiryStr = _currentCardData["本证有效期至"];
                if (DateTime.TryParse(expiryStr, out DateTime parsedDate))
                {
                    expiryDate = parsedDate;
                    if (parsedDate < DateTime.Today)
                    {
                        documentStatus = "已过期";
                    }
                    else
                    {
                        documentStatus = "有效";
                    }
                }
            }

            var record = new PassengerRecord
            {
                DeparturePortCode = txtDepartureCode.Text,
                DeparturePortName = cmbDeparturePort.Text,
                ArrivalPortCode = txtArrivalCode.Text,
                ArrivalPortName = cmbArrivalPort.Text,
                FlightDate = dtpFlightDate.Value,
                FlightTime = $"{txtFlightHour.Text}:{txtFlightMinute.Text}",
                DocumentType = cmbDocumentType.Text,
                DocumentNumber = txtDocumentNumber.Text,
                PassengerName = txtPassengerName.Text,
                TicketNumber = txtTicketNumber.Text,
                RecognizedText = txtRecognizedText.Text,
                DocumentStatus = documentStatus,
                DocumentExpiryDate = expiryDate
            };

            DatabaseHelper.Insert(record);

            LoadData();

            // 只清空票号，其他字段保留
            txtTicketNumber.Clear();
            txtTicketNumber.Focus();
        }

        private void ClearInputFields()
        {
            txtDocumentNumber.Clear();
            txtPassengerName.Clear();
            txtTicketNumber.Clear();
            txtRecognizedText.Clear();
            pictureBox.Image = null;
            cmbDocumentType.SelectedValue = "OTHER";
            txtDepartureCode.Clear();
            txtArrivalCode.Clear();
            txtFlightHour.Text = "";
            txtFlightMinute.Text = "";
            _currentCardData = null;
            _currentImageBase64 = null;
        }

        private void LoadData()
        {
            var records = DatabaseHelper.GetTodayRecords();
            dataGridView.Rows.Clear();

            foreach (var record in records)
            {
                int rowIndex = dataGridView.Rows.Add(
                    record.Id,
                    record.DeparturePortCode,
                    record.DeparturePortName,
                    record.ArrivalPortCode,
                    record.ArrivalPortName,
                    record.FlightDate.ToString("yyyy-MM-dd"),
                    record.FlightTime,
                    record.DocumentType,
                    record.DocumentNumber,
                    record.PassengerName,
                    record.TicketNumber,
                    record.DocumentStatus,
                    record.CreatedTime.ToString("HH:mm:ss")
                );

                // 判断证件状态（回乡证有效期）
                if (record.DocumentType == "XGZ" || record.DocumentType == "EEP")
                {
                    if (record.DocumentExpiryDate.HasValue)
                    {
                        if (record.DocumentExpiryDate.Value < DateTime.Today)
                        {
                            dataGridView.Rows[rowIndex].Cells["证件状态"].Style.ForeColor = System.Drawing.Color.Red;
                            dataGridView.Rows[rowIndex].Cells["证件状态"].Value = "已过期";
                        }
                        else
                        {
                            dataGridView.Rows[rowIndex].Cells["证件状态"].Style.ForeColor = System.Drawing.Color.Green;
                            dataGridView.Rows[rowIndex].Cells["证件状态"].Value = "有效";
                        }
                    }
                }
            }

            // 滚动到最后一行（最新插入的数据）
            if (dataGridView.Rows.Count > 0)
            {
                // 确保最后一行完全可见
                dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.Rows.Count - 1;
                dataGridView.CurrentCell = dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[0];
                
                // 强制刷新显示
                dataGridView.Refresh();
            }
        }

        private void btnClearSingle_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                if (MessageBox.Show("确定要清除这条记录吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value);
                    DatabaseHelper.DeleteById(id);
                    LoadData();
                }
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清除所有记录吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DatabaseHelper.DeleteAll();
                LoadData();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel 文件|*.xlsx";
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HHmm");
            sfd.FileName = $"出境旅客信息登記表{today}_{time}.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var records = DatabaseHelper.GetTodayRecords();
                ExcelHelper.ExportToExcel(records, sfd.FileName);
            }
        }

        private void btnReloadData_Click(object sender, EventArgs e)
        {
            LoadData();
            txtRecognizedText.AppendText("已从 LiteDB 读取今天的记录\r\n");
        }

        private void cmbDeparturePort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDeparturePort.SelectedValue != null)
            {
                txtDepartureCode.Text = cmbDeparturePort.SelectedValue.ToString();
            }
        }

        private void cmbArrivalPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbArrivalPort.SelectedValue != null)
            {
                txtArrivalCode.Text = cmbArrivalPort.SelectedValue.ToString();
            }
        }

        private void rbtnManualInput_CheckedChanged(object sender, EventArgs e)
        {
            m_bManualInput = rbtnManualInput.Checked;
        }

        /// <summary>
        /// 调整 DataGridView 大小以适应窗口
        /// </summary>
        private void Form1_Resize(object? sender, EventArgs e)
        {
            AdjustDataGridViewSize();
        }

        private void AdjustDataGridViewSize()
        {
            // 获取任务栏高度（使用 Screen 类）
            int taskbarHeight = Screen.PrimaryScreen.WorkingArea.Bottom - Screen.PrimaryScreen.Bounds.Bottom;
            taskbarHeight = Math.Abs(taskbarHeight); // 任务栏高度为正值
            
            // 计算可用高度：从顶部 370 开始，底部留出任务栏空间和版本文本空间
            int bottomMargin = 50 + taskbarHeight; // 底部边距 + 任务栏高度
            int dataGridViewHeight = this.ClientSize.Height - 370 - bottomMargin;
            if (dataGridViewHeight < 200) dataGridViewHeight = 200;

            // DataGridView 宽度为窗口宽度减去左右边距
            int dataGridViewWidth = this.ClientSize.Width - 24;
            if (dataGridViewWidth < 800) dataGridViewWidth = 800;

            dataGridView.Size = new Size(dataGridViewWidth, dataGridViewHeight);
            
            // 设置版本文本位置（在 DataGridView 下方）
            lblVersion.Location = new Point(12, dataGridView.Bottom + 10);

            // 调整最后一列的宽度以填充剩余空间
            if (dataGridView.Columns.Count > 0)
            {
                int fixedWidth = 0;
                for (int i = 0; i < dataGridView.Columns.Count - 1; i++)
                {
                    fixedWidth += dataGridView.Columns[i].Width;
                }
                int lastColumnWidth = dataGridViewWidth - fixedWidth - dataGridView.RowHeadersWidth - 5;
                if (lastColumnWidth > 50)
                {
                    dataGridView.Columns[dataGridView.Columns.Count - 1].Width = lastColumnWidth;
                }
            }
        }

        private void btnToggleAutoMode_Click(object sender, EventArgs e)
        {
            if (!m_bIsConnected)
            {
                MessageBox.Show("WebSocket 未连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 切换自动模式
            if (autoTimer.Enabled)
            {
                // 关闭自动模式
                autoTimer.Stop();
                btnToggleAutoMode.Text = "打开自动模式";
                rbtnReaderInput.Checked = false;
                rbtnManualInput.Checked = true;
                txtRecognizedText.AppendText("自动读卡模式已关闭\r\n");
            }
            else
            {
                // 打开自动模式
                autoTimer.Start();
                btnToggleAutoMode.Text = "关闭自动模式";
                rbtnReaderInput.Checked = true;
                rbtnManualInput.Checked = false;
                txtRecognizedText.AppendText("自动读卡模式已打开\r\n");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            autoTimer?.Stop();
            autoTimer?.Dispose();

            // 断开 WebSocket 连接
            if (_webSocketClient != null)
            {
                try
                {
                    _webSocketClient.DisconnectAsync().Wait(3000);
                    _webSocketClient.Dispose();
                }
                catch { }
            }

            DatabaseHelper.Close();

            // 强制退出应用程序
            Application.Exit();
            System.Environment.Exit(0);

            base.OnFormClosing(e);
        }
    }
}
