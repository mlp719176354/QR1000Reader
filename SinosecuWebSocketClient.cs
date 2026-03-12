using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace QR1000Reader
{
    #region 事件参数类

    /// <summary>
    /// 连接状态改变事件参数
    /// </summary>
    public class ConnectionStateChangedEventArgs : EventArgs
    {
        public bool IsConnected { get; set; }
        public string? Reason { get; set; }
    }

    /// <summary>
    /// 卡片数据事件参数
    /// </summary>
    public class CardDataEventArgs : EventArgs
    {
        public string Command { get; set; } = string.Empty;
        public string Operand { get; set; } = string.Empty;
        public JsonElement? Param { get; set; }
        public string? RawJson { get; set; }
    }

    /// <summary>
    /// 设备状态事件参数
    /// </summary>
    public class DeviceStatusEventArgs : EventArgs
    {
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    /// <summary>
    /// 错误消息事件参数
    /// </summary>
    public class ErrorMessageEventArgs : EventArgs
    {
        public string Command { get; set; } = string.Empty;
        public string Operand { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string? RawJson { get; set; }
    }

    #endregion

    #region 请求/响应模型类

    /// <summary>
    /// WebSocket 请求消息
    /// </summary>
    public class WebSocketRequest
    {
        public string Type { get; set; } = "Request";
        public string Command { get; set; } = string.Empty;
        public string Operand { get; set; } = string.Empty;
        public object? Param { get; set; }
    }

    /// <summary>
    /// WebSocket 响应消息
    /// </summary>
    public class WebSocketResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string Operand { get; set; } = string.Empty;
        public string? Succeeded { get; set; }
        public string? Result { get; set; }
        public JsonElement? Param { get; set; }
    }

    #endregion

    /// <summary>
    /// 中安护照阅读器 WebSocket 客户端
    /// 基于 Sinosecu Passport Reader SDK WebSocket-JSON 协议
    /// </summary>
    public class SinosecuWebSocketClient : IDisposable
    {
        private ClientWebSocket? _webSocket;
        private readonly string _serverUri;
        private CancellationTokenSource? _receiveCts;
        private Task? _receiveTask;
        private bool _isDisposed;
        private bool _isConnecting;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// 连接状态改变事件
        /// </summary>
        public event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;

        /// <summary>
        /// 卡片数据接收事件（用于接收 Notify 类型的消息）
        /// </summary>
        public event EventHandler<CardDataEventArgs>? CardDataReceived;

        /// <summary>
        /// 设备状态改变事件
        /// </summary>
        public event EventHandler<DeviceStatusEventArgs>? DeviceStatusChanged;

        /// <summary>
        /// 错误消息事件
        /// </summary>
        public event EventHandler<ErrorMessageEventArgs>? ErrorMessageReceived;

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected => _webSocket?.State == WebSocketState.Open;

        /// <summary>
        /// WebSocket 连接状态
        /// </summary>
        public WebSocketState ConnectionState => _webSocket?.State ?? WebSocketState.None;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverUri">WebSocket 服务器地址，默认为 ws://127.0.0.1:90/echo</param>
        public SinosecuWebSocketClient(string serverUri = "ws://127.0.0.1:90/echo")
        {
            _serverUri = serverUri;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };
        }

        /// <summary>
        /// 连接到 WebSocket 服务
        /// </summary>
        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            if (_isConnecting)
            {
                throw new InvalidOperationException("连接正在进行中...");
            }

            if (IsConnected)
            {
                return;
            }

            _isConnecting = true;
            try
            {
                _webSocket?.Dispose();
                _webSocket = new ClientWebSocket();

                await _webSocket.ConnectAsync(new Uri(_serverUri), cancellationToken);

                _receiveCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                _receiveTask = ReceiveLoopAsync(_receiveCts.Token);

                OnConnectionStateChanged(true, "连接成功");
            }
            finally
            {
                _isConnecting = false;
            }
        }

        /// <summary>
        /// 断开 WebSocket 连接
        /// </summary>
        public async Task DisconnectAsync()
        {
            if (_webSocket == null || !IsConnected)
            {
                return;
            }

            try
            {
                _receiveCts?.Cancel();

                if (_receiveTask != null)
                {
                    try
                    {
                        await _receiveTask;
                    }
                    catch (OperationCanceledException)
                    {
                        // 预期内的取消异常
                    }
                }

                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "正常关闭", CancellationToken.None);
            }
            catch (Exception)
            {
                // 忽略关闭异常
            }
            finally
            {
                _webSocket?.Dispose();
                _webSocket = null;
                _receiveCts?.Dispose();
                _receiveCts = null;
                _receiveTask = null;

                OnConnectionStateChanged(false, "已断开连接");
            }
        }

        /// <summary>
        /// 发送 JSON 消息
        /// </summary>
        public async Task<WebSocketResponse?> SendAsync(WebSocketRequest request, CancellationToken cancellationToken = default)
        {
            if (!IsConnected || _webSocket == null)
            {
                throw new InvalidOperationException("未连接到 WebSocket 服务");
            }

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var bytes = Encoding.UTF8.GetBytes(json);

            await _webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                cancellationToken);

            return null; // 响应通过 ReceiveLoop 异步处理
        }

        /// <summary>
        /// 发送并等待响应
        /// </summary>
        public async Task<WebSocketResponse?> SendAndWaitAsync(WebSocketRequest request, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            if (!IsConnected || _webSocket == null)
            {
                throw new InvalidOperationException("未连接到 WebSocket 服务");
            }

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var bytes = Encoding.UTF8.GetBytes(json);

            await _webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                cancellationToken);

            // 等待响应（简单实现，实际可能需要更复杂的请求 - 响应匹配机制）
            var receiveCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            receiveCts.CancelAfter(timeout);

            try
            {
                var buffer = new byte[4096];
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), receiveCts.Token);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var responseJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    return JsonSerializer.Deserialize<WebSocketResponse>(responseJson, _jsonOptions);
                }
            }
            catch (OperationCanceledException)
            {
                // 超时
            }

            return null;
        }

        /// <summary>
        /// 获取设备状态
        /// 发送：{"Type":"Request","Command":"Get","Operand":"DeviceStatus"}
        /// </summary>
        public async Task GetDeviceStatusAsync(CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "Get",
                Operand = "DeviceStatus"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 设置默认参数
        /// 发送：{"Type":"Request","Command":"Set","Operand":"DefaultSettings"}
        /// </summary>
        public async Task SetDefaultSettingsAsync(CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "Set",
                Operand = "DefaultSettings"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 设置是否读取芯片
        /// 发送：{"Type":"Request","Command":"Set","Operand":"ReadChip","Param":"Y"或"N"}
        /// </summary>
        /// <param name="enable">是否启用读取芯片</param>
        public async Task SetReadChipAsync(bool enable, CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "Set",
                Operand = "ReadChip",
                Param = enable ? "Y" : "N"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 设置是否识别版面
        /// 发送：{"Type":"Request","Command":"Set","Operand":"ReadViz","Param":"Y"或"N"}
        /// </summary>
        /// <param name="enable">是否启用识别版面</param>
        public async Task SetReadVizAsync(bool enable, CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "Set",
                Operand = "ReadViz",
                Param = enable ? "Y" : "N"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 手动触发识别
        /// 发送：{"Type":"Request","Command":"ManualTrigger"}
        /// </summary>
        public async Task ManualTriggerAsync(CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "ManualTrigger"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 获取图像
        /// 发送：{"Type":"Request","Command":"Get","Operand":"Image"}
        /// </summary>
        public async Task GetImageAsync(CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "Get",
                Operand = "Image"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 扫描文档
        /// 发送：{"Type":"Request","Command":"ScanDocument"}
        /// </summary>
        public async Task ScanDocumentAsync(CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "ScanDocument"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 设置二代证只读芯片
        /// 发送：{"Type":"Request","Command":"Set","Operand":"ReadSidChip","Param":"Y"或"N"}
        /// </summary>
        /// <param name="enable">是否只读芯片</param>
        public async Task SetReadSidChipAsync(bool enable, CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "Set",
                Operand = "ReadSidChip",
                Param = enable ? "Y" : "N"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 启用回调模式
        /// 发送：{"Type":"Request","Command":"Set","Operand":"CallbackMode","Param":"Y"或"N"}
        /// </summary>
        /// <param name="enable">是否启用回调模式</param>
        public async Task SetCallbackModeAsync(bool enable, CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "Set",
                Operand = "CallbackMode",
                Param = enable ? "Y" : "N"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 启用证件检测通知
        /// 发送：{"Type":"Request","Command":"Set","Operand":"CardDetectNotify","Param":"Y"或"N"}
        /// </summary>
        /// <param name="enable">是否启用证件检测通知</param>
        public async Task SetCardDetectNotifyAsync(bool enable, CancellationToken cancellationToken = default)
        {
            var request = new WebSocketRequest
            {
                Command = "Set",
                Operand = "CardDetectNotify",
                Param = enable ? "Y" : "N"
            };
            await SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 接收消息循环
        /// </summary>
        private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
        {
            var buffer = new byte[8192];

            try
            {
                while (!cancellationToken.IsCancellationRequested && IsConnected)
                {
                    var result = await _webSocket!.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await DisconnectAsync();
                        break;
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        await ProcessMessageAsync(json);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 正常取消
            }
            catch (WebSocketException)
            {
                // 连接断开
                await DisconnectAsync();
            }
            catch (Exception ex)
            {
                OnConnectionStateChanged(false, $"错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        private async Task ProcessMessageAsync(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("Type", out var typeProp))
                {
                    return;
                }

                var type = typeProp.GetString();
                var command = root.TryGetProperty("Command", out var cmdProp) ? cmdProp.GetString() : string.Empty;
                var operand = root.TryGetProperty("Operand", out var opProp) ? opProp.GetString() : string.Empty;

                switch (type)
                {
                    case "Notify":
                        await HandleNotifyAsync(command, operand, root, json);
                        break;

                    case "Reply":
                        await HandleReplyAsync(command, operand, root, json);
                        break;
                }
            }
            catch (JsonException)
            {
                // JSON 解析失败，忽略
            }
        }

        /// <summary>
        /// 处理 Notify 类型消息
        /// </summary>
        private async Task HandleNotifyAsync(string? command, string? operand, JsonElement root, string rawJson)
        {
            var cardDataArgs = new CardDataEventArgs
            {
                Command = command ?? string.Empty,
                Operand = operand ?? string.Empty,
                RawJson = rawJson
            };

            if (root.TryGetProperty("Param", out var paramProp))
            {
                cardDataArgs.Param = paramProp;
            }

            CardDataReceived?.Invoke(this, cardDataArgs);

            // 特殊处理：设备状态通知
            if (string.Equals(operand, "DeviceStatus", StringComparison.OrdinalIgnoreCase))
            {
                var status = paramProp.TryGetProperty("Status", out var s) ? s.GetString() : string.Empty;
                var description = paramProp.TryGetProperty("Description", out var d) ? d.GetString() : null;

                var deviceStatusArgs = new DeviceStatusEventArgs
                {
                    Status = status ?? string.Empty,
                    Description = description
                };
                DeviceStatusChanged?.Invoke(this, deviceStatusArgs);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理 Reply 类型消息
        /// </summary>
        private async Task HandleReplyAsync(string? command, string? operand, JsonElement root, string rawJson)
        {
            var succeeded = root.TryGetProperty("Succeeded", out var sucProp) ? sucProp.GetString() : string.Empty;
            var result = root.TryGetProperty("Result", out var resProp) ? resProp.GetString() : string.Empty;

            // 如果操作失败，触发错误事件
            if (string.Equals(succeeded, "N", StringComparison.OrdinalIgnoreCase))
            {
                var errorArgs = new ErrorMessageEventArgs
                {
                    Command = command ?? string.Empty,
                    Operand = operand ?? string.Empty,
                    Result = result ?? string.Empty,
                    RawJson = rawJson
                };
                ErrorMessageReceived?.Invoke(this, errorArgs);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 触发连接状态改变事件
        /// </summary>
        private void OnConnectionStateChanged(bool isConnected, string? reason)
        {
            ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs
            {
                IsConnected = isConnected,
                Reason = reason
            });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            _receiveCts?.Cancel();
            _webSocket?.Dispose();
            _receiveCts?.Dispose();
        }
    }
}
