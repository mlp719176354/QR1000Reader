namespace QR1000Reader
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label lblDeparturePort;
        private System.Windows.Forms.ComboBox cmbDeparturePort;
        private System.Windows.Forms.TextBox txtDepartureCode;
        private System.Windows.Forms.Label lblArrivalPort;
        private System.Windows.Forms.ComboBox cmbArrivalPort;
        private System.Windows.Forms.TextBox txtArrivalCode;
        private System.Windows.Forms.Label lblFlightDate;
        private System.Windows.Forms.DateTimePicker dtpFlightDate;
        private System.Windows.Forms.Label lblFlightTime;
        private System.Windows.Forms.TextBox txtFlightHour;
        private System.Windows.Forms.Label lblFlightTimeSeparator;
        private System.Windows.Forms.TextBox txtFlightMinute;
        private System.Windows.Forms.Label lblDocumentType;
        private System.Windows.Forms.ComboBox cmbDocumentType;
        private System.Windows.Forms.Label lblDocumentNumber;
        private System.Windows.Forms.TextBox txtDocumentNumber;
        private System.Windows.Forms.Label lblPassengerName;
        private System.Windows.Forms.TextBox txtPassengerName;
        private System.Windows.Forms.Label lblTicketNumber;
        private System.Windows.Forms.TextBox txtTicketNumber;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.GroupBox grpReadCard;
        private System.Windows.Forms.GroupBox grpButtons;
        private System.Windows.Forms.Button btnReadCard;
        private System.Windows.Forms.RadioButton rbtnManualInput;
        private System.Windows.Forms.RadioButton rbtnReaderInput;
        private System.Windows.Forms.Label lblRecognizedText;
        private System.Windows.Forms.TextBox txtRecognizedText;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClearSingle;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label lblDeviceStatus;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnToggleAutoMode;

        private void InitializeComponent()
        {
            this.lblUserID = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.lblDeparturePort = new System.Windows.Forms.Label();
            this.cmbDeparturePort = new System.Windows.Forms.ComboBox();
            this.txtDepartureCode = new System.Windows.Forms.TextBox();
            this.lblArrivalPort = new System.Windows.Forms.Label();
            this.cmbArrivalPort = new System.Windows.Forms.ComboBox();
            this.txtArrivalCode = new System.Windows.Forms.TextBox();
            this.lblFlightDate = new System.Windows.Forms.Label();
            this.dtpFlightDate = new System.Windows.Forms.DateTimePicker();
            this.lblFlightTime = new System.Windows.Forms.Label();
            this.txtFlightHour = new System.Windows.Forms.TextBox();
            this.lblFlightTimeSeparator = new System.Windows.Forms.Label();
            this.txtFlightMinute = new System.Windows.Forms.TextBox();
            this.lblDocumentType = new System.Windows.Forms.Label();
            this.cmbDocumentType = new System.Windows.Forms.ComboBox();
            this.lblDocumentNumber = new System.Windows.Forms.Label();
            this.txtDocumentNumber = new System.Windows.Forms.TextBox();
            this.lblPassengerName = new System.Windows.Forms.Label();
            this.txtPassengerName = new System.Windows.Forms.TextBox();
            this.lblTicketNumber = new System.Windows.Forms.Label();
            this.txtTicketNumber = new System.Windows.Forms.TextBox();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.grpReadCard = new System.Windows.Forms.GroupBox();
            this.grpButtons = new System.Windows.Forms.GroupBox();
            this.btnToggleAutoMode = new System.Windows.Forms.Button();
            this.btnReadCard = new System.Windows.Forms.Button();
            this.rbtnManualInput = new System.Windows.Forms.RadioButton();
            this.rbtnReaderInput = new System.Windows.Forms.RadioButton();
            this.lblRecognizedText = new System.Windows.Forms.Label();
            this.txtRecognizedText = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClearSingle = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.lblDeviceStatus = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.grpInput.SuspendLayout();
            this.grpReadCard.SuspendLayout();
            this.grpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            //
            // lblUserID
            //
            this.lblUserID.AutoSize = true;
            this.lblUserID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserID.Location = new System.Drawing.Point(12, 12);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(89, 20);
            this.lblUserID.Text = "User ID:";
            //
            // txtUserID
            //
            this.txtUserID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserID.Location = new System.Drawing.Point(100, 10);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(120, 26);
            this.txtUserID.TabIndex = 0;
            //
            // lblDeparturePort
            //
            this.lblDeparturePort.AutoSize = true;
            this.lblDeparturePort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeparturePort.Location = new System.Drawing.Point(12, 45);
            this.lblDeparturePort.Name = "lblDeparturePort";
            this.lblDeparturePort.Size = new System.Drawing.Size(93, 20);
            this.lblDeparturePort.Text = "始发港*:";
            //
            // cmbDeparturePort
            //
            this.cmbDeparturePort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeparturePort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDeparturePort.Location = new System.Drawing.Point(100, 42);
            this.cmbDeparturePort.Name = "cmbDeparturePort";
            this.cmbDeparturePort.Size = new System.Drawing.Size(150, 28);
            this.cmbDeparturePort.TabIndex = 1;
            this.cmbDeparturePort.SelectedIndexChanged += new System.EventHandler(this.cmbDeparturePort_SelectedIndexChanged);
            //
            // txtDepartureCode
            //
            this.txtDepartureCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDepartureCode.Location = new System.Drawing.Point(260, 42);
            this.txtDepartureCode.Name = "txtDepartureCode";
            this.txtDepartureCode.ReadOnly = true;
            this.txtDepartureCode.Size = new System.Drawing.Size(60, 26);
            this.txtDepartureCode.TabIndex = 2;
            //
            // lblArrivalPort
            //
            this.lblArrivalPort.AutoSize = true;
            this.lblArrivalPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArrivalPort.Location = new System.Drawing.Point(15, 85);
            this.lblArrivalPort.Name = "lblArrivalPort";
            this.lblArrivalPort.Size = new System.Drawing.Size(93, 20);
            this.lblArrivalPort.Text = "到达港*:";
            //
            // cmbArrivalPort
            //
            this.cmbArrivalPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArrivalPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbArrivalPort.Location = new System.Drawing.Point(115, 82);
            this.cmbArrivalPort.Name = "cmbArrivalPort";
            this.cmbArrivalPort.Size = new System.Drawing.Size(150, 28);
            this.cmbArrivalPort.TabIndex = 3;
            this.cmbArrivalPort.SelectedIndexChanged += new System.EventHandler(this.cmbArrivalPort_SelectedIndexChanged);
            //
            // txtArrivalCode
            //
            this.txtArrivalCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArrivalCode.Location = new System.Drawing.Point(275, 82);
            this.txtArrivalCode.Name = "txtArrivalCode";
            this.txtArrivalCode.ReadOnly = true;
            this.txtArrivalCode.Size = new System.Drawing.Size(70, 26);
            this.txtArrivalCode.TabIndex = 4;
            //
            // lblFlightDate
            //
            this.lblFlightDate.AutoSize = true;
            this.lblFlightDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlightDate.Location = new System.Drawing.Point(15, 120);
            this.lblFlightDate.Name = "lblFlightDate";
            this.lblFlightDate.Size = new System.Drawing.Size(93, 20);
            this.lblFlightDate.Text = "航班日期:";
            //
            // dtpFlightDate
            //
            this.dtpFlightDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFlightDate.Location = new System.Drawing.Point(115, 117);
            this.dtpFlightDate.Name = "dtpFlightDate";
            this.dtpFlightDate.Size = new System.Drawing.Size(150, 26);
            this.dtpFlightDate.TabIndex = 5;
            //
            // lblFlightTime
            //
            this.lblFlightTime.AutoSize = true;
            this.lblFlightTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlightTime.Location = new System.Drawing.Point(15, 155);
            this.lblFlightTime.Name = "lblFlightTime";
            this.lblFlightTime.Size = new System.Drawing.Size(97, 20);
            this.lblFlightTime.Text = "航班时间*:";
            //
            // txtFlightHour
            //
            this.txtFlightHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFlightHour.Location = new System.Drawing.Point(115, 152);
            this.txtFlightHour.Name = "txtFlightHour";
            this.txtFlightHour.Size = new System.Drawing.Size(50, 26);
            this.txtFlightHour.TabIndex = 6;
            this.txtFlightHour.MaxLength = 2;
            //
            // lblFlightTimeSeparator
            //
            this.lblFlightTimeSeparator.AutoSize = true;
            this.lblFlightTimeSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlightTimeSeparator.Location = new System.Drawing.Point(168, 155);
            this.lblFlightTimeSeparator.Name = "lblFlightTimeSeparator";
            this.lblFlightTimeSeparator.Size = new System.Drawing.Size(15, 20);
            this.lblFlightTimeSeparator.Text = ":";
            //
            // txtFlightMinute
            //
            this.txtFlightMinute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFlightMinute.Location = new System.Drawing.Point(185, 152);
            this.txtFlightMinute.Name = "txtFlightMinute";
            this.txtFlightMinute.Size = new System.Drawing.Size(50, 26);
            this.txtFlightMinute.TabIndex = 7;
            this.txtFlightMinute.MaxLength = 2;
            //
            // lblDocumentType
            //
            this.lblDocumentType.AutoSize = true;
            this.lblDocumentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentType.Location = new System.Drawing.Point(15, 190);
            this.lblDocumentType.Name = "lblDocumentType";
            this.lblDocumentType.Size = new System.Drawing.Size(97, 20);
            this.lblDocumentType.Text = "证件类型*:";
            //
            // cmbDocumentType
            //
            this.cmbDocumentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocumentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDocumentType.Location = new System.Drawing.Point(115, 187);
            this.cmbDocumentType.Name = "cmbDocumentType";
            this.cmbDocumentType.Size = new System.Drawing.Size(210, 28);
            this.cmbDocumentType.TabIndex = 8;
            //
            // lblDocumentNumber
            //
            this.lblDocumentNumber.AutoSize = true;
            this.lblDocumentNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentNumber.Location = new System.Drawing.Point(15, 225);
            this.lblDocumentNumber.Name = "lblDocumentNumber";
            this.lblDocumentNumber.Size = new System.Drawing.Size(97, 20);
            this.lblDocumentNumber.Text = "证件号码:";
            //
            // txtDocumentNumber
            //
            this.txtDocumentNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentNumber.Location = new System.Drawing.Point(115, 222);
            this.txtDocumentNumber.Name = "txtDocumentNumber";
            this.txtDocumentNumber.Size = new System.Drawing.Size(210, 26);
            this.txtDocumentNumber.TabIndex = 9;
            //
            // lblPassengerName
            //
            this.lblPassengerName.AutoSize = true;
            this.lblPassengerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassengerName.Location = new System.Drawing.Point(15, 260);
            this.lblPassengerName.Name = "lblPassengerName";
            this.lblPassengerName.Size = new System.Drawing.Size(97, 20);
            this.lblPassengerName.Text = "旅客姓名:";
            //
            // txtPassengerName
            //
            this.txtPassengerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassengerName.Location = new System.Drawing.Point(115, 257);
            this.txtPassengerName.Name = "txtPassengerName";
            this.txtPassengerName.Size = new System.Drawing.Size(210, 26);
            this.txtPassengerName.TabIndex = 10;
            //
            // lblTicketNumber
            //
            this.lblTicketNumber.AutoSize = true;
            this.lblTicketNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketNumber.Location = new System.Drawing.Point(15, 295);
            this.lblTicketNumber.Name = "lblTicketNumber";
            this.lblTicketNumber.Size = new System.Drawing.Size(69, 20);
            this.lblTicketNumber.Text = "票号:";
            //
            // txtTicketNumber
            //
            this.txtTicketNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTicketNumber.Location = new System.Drawing.Point(115, 292);
            this.txtTicketNumber.Name = "txtTicketNumber";
            this.txtTicketNumber.Size = new System.Drawing.Size(210, 26);
            this.txtTicketNumber.TabIndex = 11;
            //
            // grpInput
            //
            this.grpInput.Controls.Add(this.lblDeparturePort);
            this.grpInput.Controls.Add(this.cmbDeparturePort);
            this.grpInput.Controls.Add(this.txtDepartureCode);
            this.grpInput.Controls.Add(this.lblArrivalPort);
            this.grpInput.Controls.Add(this.cmbArrivalPort);
            this.grpInput.Controls.Add(this.txtArrivalCode);
            this.grpInput.Controls.Add(this.lblFlightDate);
            this.grpInput.Controls.Add(this.dtpFlightDate);
            this.grpInput.Controls.Add(this.lblFlightTime);
            this.grpInput.Controls.Add(this.txtFlightHour);
            this.grpInput.Controls.Add(this.lblFlightTimeSeparator);
            this.grpInput.Controls.Add(this.txtFlightMinute);
            this.grpInput.Controls.Add(this.lblDocumentType);
            this.grpInput.Controls.Add(this.cmbDocumentType);
            this.grpInput.Controls.Add(this.lblDocumentNumber);
            this.grpInput.Controls.Add(this.txtDocumentNumber);
            this.grpInput.Controls.Add(this.lblPassengerName);
            this.grpInput.Controls.Add(this.txtPassengerName);
            this.grpInput.Controls.Add(this.lblTicketNumber);
            this.grpInput.Controls.Add(this.txtTicketNumber);
            this.grpInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpInput.Location = new System.Drawing.Point(12, 40);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(360, 280);
            this.grpInput.TabIndex = 1;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "旅客信息";
            //
            // grpReadCard
            //
            this.grpReadCard.Controls.Add(this.btnToggleAutoMode);
            this.grpReadCard.Controls.Add(this.btnReadCard);
            this.grpReadCard.Controls.Add(this.rbtnManualInput);
            this.grpReadCard.Controls.Add(this.rbtnReaderInput);
            this.grpReadCard.Controls.Add(this.lblRecognizedText);
            this.grpReadCard.Controls.Add(this.txtRecognizedText);
            this.grpReadCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpReadCard.Location = new System.Drawing.Point(385, 40);
            this.grpReadCard.Name = "grpReadCard";
            this.grpReadCard.Size = new System.Drawing.Size(560, 280);
            this.grpReadCard.TabIndex = 2;
            this.grpReadCard.TabStop = false;
            this.grpReadCard.Text = "读卡器";
            //
            // grpButtons
            //
            this.grpButtons.Controls.Add(this.btnExport);
            this.grpButtons.Controls.Add(this.btnClearAll);
            this.grpButtons.Controls.Add(this.btnClearSingle);
            this.grpButtons.Controls.Add(this.btnSave);
            this.grpButtons.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpButtons.Location = new System.Drawing.Point(960, 40);
            this.grpButtons.Name = "grpButtons";
            this.grpButtons.Size = new System.Drawing.Size(130, 280);
            this.grpButtons.TabIndex = 3;
            this.grpButtons.TabStop = false;
            this.grpButtons.Text = "操作";
            //
            // btnReadCard
            //
            this.btnReadCard.Location = new System.Drawing.Point(10, 20);
            this.btnReadCard.Name = "btnReadCard";
            this.btnReadCard.Size = new System.Drawing.Size(100, 30);
            this.btnReadCard.TabIndex = 0;
            this.btnReadCard.Text = "读取证件";
            this.btnReadCard.UseVisualStyleBackColor = true;
            this.btnReadCard.Click += new System.EventHandler(this.btnReadCard_Click);
            //
            // btnToggleAutoMode
            //
            this.btnToggleAutoMode.Location = new System.Drawing.Point(120, 20);
            this.btnToggleAutoMode.Name = "btnToggleAutoMode";
            this.btnToggleAutoMode.Size = new System.Drawing.Size(100, 30);
            this.btnToggleAutoMode.TabIndex = 1;
            this.btnToggleAutoMode.Text = "关闭自动模式";
            this.btnToggleAutoMode.UseVisualStyleBackColor = true;
            this.btnToggleAutoMode.Click += new System.EventHandler(this.btnToggleAutoMode_Click);
            this.btnToggleAutoMode.Visible = false;
            //
            // rbtnManualInput
            //
            this.rbtnManualInput.AutoSize = true;
            this.rbtnManualInput.Location = new System.Drawing.Point(230, 25);
            this.rbtnManualInput.Name = "rbtnManualInput";
            this.rbtnManualInput.Size = new System.Drawing.Size(71, 16);
            this.rbtnManualInput.TabIndex = 2;
            this.rbtnManualInput.Text = "手动输入";
            this.rbtnManualInput.UseVisualStyleBackColor = true;
            this.rbtnManualInput.CheckedChanged += new System.EventHandler(this.rbtnManualInput_CheckedChanged);
            this.rbtnManualInput.Visible = false;
            //
            // rbtnReaderInput
            //
            this.rbtnReaderInput.AutoSize = true;
            this.rbtnReaderInput.Checked = true;
            this.rbtnReaderInput.Location = new System.Drawing.Point(320, 25);
            this.rbtnReaderInput.Name = "rbtnReaderInput";
            this.rbtnReaderInput.Size = new System.Drawing.Size(120, 16);
            this.rbtnReaderInput.TabIndex = 3;
            this.rbtnReaderInput.TabStop = true;
            this.rbtnReaderInput.Text = "自动读卡模式";
            this.rbtnReaderInput.UseVisualStyleBackColor = true;
            this.rbtnReaderInput.CheckedChanged += new System.EventHandler(this.rbtnManualInput_CheckedChanged);
            this.rbtnReaderInput.Visible = false;
            //
            // lblRecognizedText
            //
            this.lblRecognizedText.AutoSize = true;
            this.lblRecognizedText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecognizedText.Location = new System.Drawing.Point(15, 25);
            this.lblRecognizedText.Name = "lblRecognizedText";
            this.lblRecognizedText.Size = new System.Drawing.Size(97, 20);
            this.lblRecognizedText.Text = "识别文字:";
            //
            // txtRecognizedText
            //
            this.txtRecognizedText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecognizedText.Location = new System.Drawing.Point(15, 50);
            this.txtRecognizedText.Multiline = true;
            this.txtRecognizedText.Name = "txtRecognizedText";
            this.txtRecognizedText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRecognizedText.Size = new System.Drawing.Size(530, 210);
            this.txtRecognizedText.TabIndex = 3;
            //
            // btnSave
            //
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(15, 40);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 45);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "确定";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnClearSingle
            //
            this.btnClearSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSingle.Location = new System.Drawing.Point(15, 90);
            this.btnClearSingle.Name = "btnClearSingle";
            this.btnClearSingle.Size = new System.Drawing.Size(100, 45);
            this.btnClearSingle.TabIndex = 4;
            this.btnClearSingle.Text = "清除单条";
            this.btnClearSingle.UseVisualStyleBackColor = true;
            this.btnClearSingle.Click += new System.EventHandler(this.btnClearSingle_Click);
            //
            // btnClearAll
            //
            this.btnClearAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearAll.Location = new System.Drawing.Point(15, 140);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(100, 45);
            this.btnClearAll.TabIndex = 5;
            this.btnClearAll.Text = "清除全部";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            //
            // btnExport
            //
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(15, 190);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 45);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "导出 Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            //
            // dataGridView
            //
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataGridView.ColumnHeadersHeight = 32;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "序号", Width = 45, Frozen = true },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "始发港代码", Width = 75 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "始发港名称", Width = 100 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "到达港代码", Width = 75 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "到达港名称", Width = 100 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "航班日期", Width = 110 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "航班时间", Width = 85 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "证件类型", Width = 120 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "证件号码", Width = 170 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "旅客姓名", Width = 130 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "票号", Width = 130 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "证件状态", Width = 85 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "创建时间", Width = 100 }
            });
            this.dataGridView.Location = new System.Drawing.Point(12, 330);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 30;
            this.dataGridView.RowTemplate.Height = 30;
            this.dataGridView.Size = new System.Drawing.Size(1330, 360);
            this.dataGridView.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView.TabIndex = 7;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dataGridView.EnableHeadersVisualStyles = false;
            this.dataGridView.ColumnHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.AllowUserToOrderColumns = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            //
            // lblVersion
            //
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(12, 390);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(120, 20);
            this.lblVersion.TabIndex = 8;
            this.lblVersion.Text = "版本：v2026.03.16";
            this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            //
            // lblDeviceStatus
            //
            this.lblDeviceStatus.AutoSize = true;
            this.lblDeviceStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeviceStatus.Location = new System.Drawing.Point(650, 15);
            this.lblDeviceStatus.Name = "lblDeviceStatus";
            this.lblDeviceStatus.Size = new System.Drawing.Size(97, 20);
            this.lblDeviceStatus.Text = "设备状态:";
            this.lblDeviceStatus.ForeColor = System.Drawing.Color.Red;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 420);
            this.Controls.Add(this.lblUserID);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.grpReadCard);
            this.Controls.Add(this.grpButtons);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.lblDeviceStatus);
            this.Controls.Add(this.lblVersion);
            this.Name = "Form1";
            this.Text = "护照阅读器";
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.grpReadCard.ResumeLayout(false);
            this.grpReadCard.PerformLayout();
            this.grpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
