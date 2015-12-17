namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encryptorCRC8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.apatorScreen = new System.Windows.Forms.RichTextBox();
            this.apatorBtn = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.infoBtn = new System.Windows.Forms.Button();
            this.lastRecBtn = new System.Windows.Forms.Button();
            this.byIdBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.reqUd2_btn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.addressDecCmb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.portSettingsApplyBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.portCmbBox = new System.Windows.Forms.ComboBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.addressCmbBox = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.readCurBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.readDailyBtn = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.apatorBtn)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(993, 29);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(63, 25);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.closeToolStripMenuItem.Text = "Выход";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolToolStripMenuItem
            // 
            this.toolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encryptorCRC8ToolStripMenuItem});
            this.toolToolStripMenuItem.Name = "toolToolStripMenuItem";
            this.toolToolStripMenuItem.Size = new System.Drawing.Size(126, 25);
            this.toolToolStripMenuItem.Text = "Инструменты";
            // 
            // encryptorCRC8ToolStripMenuItem
            // 
            this.encryptorCRC8ToolStripMenuItem.Name = "encryptorCRC8ToolStripMenuItem";
            this.encryptorCRC8ToolStripMenuItem.Size = new System.Drawing.Size(223, 26);
            this.encryptorCRC8ToolStripMenuItem.Text = "Шифратор и CRC8";
            this.encryptorCRC8ToolStripMenuItem.Click += new System.EventHandler(this.encryptorCRC8ToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(123, 25);
            this.aboutToolStripMenuItem.Text = "О программе";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(567, 538);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // apatorScreen
            // 
            this.apatorScreen.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.apatorScreen.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.apatorScreen.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.apatorScreen.Location = new System.Drawing.Point(158, 253);
            this.apatorScreen.Name = "apatorScreen";
            this.apatorScreen.ReadOnly = true;
            this.apatorScreen.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.apatorScreen.Size = new System.Drawing.Size(229, 125);
            this.apatorScreen.TabIndex = 25;
            this.apatorScreen.Text = "Приветствие!";
            // 
            // apatorBtn
            // 
            this.apatorBtn.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.apatorBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("apatorBtn.BackgroundImage")));
            this.apatorBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.apatorBtn.Image = ((System.Drawing.Image)(resources.GetObject("apatorBtn.Image")));
            this.apatorBtn.Location = new System.Drawing.Point(261, 424);
            this.apatorBtn.Name = "apatorBtn";
            this.apatorBtn.Size = new System.Drawing.Size(24, 22);
            this.apatorBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.apatorBtn.TabIndex = 26;
            this.apatorBtn.TabStop = false;
            this.apatorBtn.Click += new System.EventHandler(this.apatorBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.infoBtn);
            this.groupBox1.Controls.Add(this.lastRecBtn);
            this.groupBox1.Controls.Add(this.byIdBtn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Location = new System.Drawing.Point(581, 386);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(399, 71);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Архив записей (РТ протокол)";
            // 
            // infoBtn
            // 
            this.infoBtn.Location = new System.Drawing.Point(299, 25);
            this.infoBtn.Name = "infoBtn";
            this.infoBtn.Size = new System.Drawing.Size(81, 31);
            this.infoBtn.TabIndex = 24;
            this.infoBtn.Text = "Инфо";
            this.infoBtn.UseVisualStyleBackColor = true;
            this.infoBtn.Click += new System.EventHandler(this.infoBtn_Click);
            // 
            // lastRecBtn
            // 
            this.lastRecBtn.Location = new System.Drawing.Point(181, 26);
            this.lastRecBtn.Name = "lastRecBtn";
            this.lastRecBtn.Size = new System.Drawing.Size(112, 31);
            this.lastRecBtn.TabIndex = 23;
            this.lastRecBtn.Text = "Последняя";
            this.lastRecBtn.UseVisualStyleBackColor = true;
            this.lastRecBtn.Click += new System.EventHandler(this.lastRecBtn_Click);
            // 
            // byIdBtn
            // 
            this.byIdBtn.Location = new System.Drawing.Point(92, 27);
            this.byIdBtn.Name = "byIdBtn";
            this.byIdBtn.Size = new System.Drawing.Size(83, 30);
            this.byIdBtn.TabIndex = 22;
            this.byIdBtn.Text = "По ID";
            this.byIdBtn.UseVisualStyleBackColor = true;
            this.byIdBtn.Click += new System.EventHandler(this.byIdBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 21;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(12, 29);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(74, 26);
            this.numericUpDown1.TabIndex = 20;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.reqUd2_btn);
            this.groupBox2.Location = new System.Drawing.Point(581, 308);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(399, 72);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MBUS";
            // 
            // reqUd2_btn
            // 
            this.reqUd2_btn.Location = new System.Drawing.Point(12, 25);
            this.reqUd2_btn.Name = "reqUd2_btn";
            this.reqUd2_btn.Size = new System.Drawing.Size(115, 33);
            this.reqUd2_btn.TabIndex = 10;
            this.reqUd2_btn.Text = "REQ_UD2";
            this.reqUd2_btn.UseVisualStyleBackColor = true;
            this.reqUd2_btn.Click += new System.EventHandler(this.reqUd2_btn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.addressDecCmb);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.portSettingsApplyBtn);
            this.groupBox3.Controls.Add(this.panel2);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.radioButton2);
            this.groupBox3.Controls.Add(this.radioButton1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.addressCmbBox);
            this.groupBox3.Location = new System.Drawing.Point(581, 32);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(399, 270);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметры соединения";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(248, 205);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 20);
            this.label7.TabIndex = 38;
            this.label7.Text = "dec";
            // 
            // addressDecCmb
            // 
            this.addressDecCmb.FormattingEnabled = true;
            this.addressDecCmb.Location = new System.Drawing.Point(233, 228);
            this.addressDecCmb.Name = "addressDecCmb";
            this.addressDecCmb.Size = new System.Drawing.Size(85, 28);
            this.addressDecCmb.TabIndex = 37;
            this.addressDecCmb.TextChanged += new System.EventHandler(this.addressDecCmb_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(161, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 20);
            this.label6.TabIndex = 36;
            this.label6.Text = "hex";
            // 
            // portSettingsApplyBtn
            // 
            this.portSettingsApplyBtn.Location = new System.Drawing.Point(327, 224);
            this.portSettingsApplyBtn.Name = "portSettingsApplyBtn";
            this.portSettingsApplyBtn.Size = new System.Drawing.Size(53, 34);
            this.portSettingsApplyBtn.TabIndex = 35;
            this.portSettingsApplyBtn.Text = "OK";
            this.portSettingsApplyBtn.UseVisualStyleBackColor = true;
            this.portSettingsApplyBtn.Click += new System.EventHandler(this.portSettingsApplyBtn_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(35, 144);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(345, 50);
            this.panel2.TabIndex = 34;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(246, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(92, 26);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "5002";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(218, 26);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "172.40.40.240";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.numericUpDown4);
            this.panel1.Controls.Add(this.portCmbBox);
            this.panel1.Location = new System.Drawing.Point(37, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 50);
            this.panel1.TabIndex = 33;
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(244, 11);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(92, 26);
            this.numericUpDown4.TabIndex = 31;
            this.toolTip1.SetToolTip(this.numericUpDown4, "Задержка чтения (Read Timeout)");
            this.numericUpDown4.Value = new decimal(new int[] {
            1600,
            0,
            0,
            0});
            // 
            // portCmbBox
            // 
            this.portCmbBox.FormattingEnabled = true;
            this.portCmbBox.Location = new System.Drawing.Point(13, 10);
            this.portCmbBox.Name = "portCmbBox";
            this.portCmbBox.Size = new System.Drawing.Size(124, 28);
            this.portCmbBox.TabIndex = 30;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(10, 114);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(82, 24);
            this.radioButton2.TabIndex = 32;
            this.radioButton2.Tag = "rb_tcp";
            this.radioButton2.Text = "TCP/IP";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.rb_portselect_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 25);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(69, 24);
            this.radioButton1.TabIndex = 31;
            this.radioButton1.Tag = "rb_com";
            this.radioButton1.Text = "COM";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.rb_portselect_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 231);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 20);
            this.label3.TabIndex = 30;
            this.label3.Text = "Сетевой адрес:";
            // 
            // addressCmbBox
            // 
            this.addressCmbBox.FormattingEnabled = true;
            this.addressCmbBox.Location = new System.Drawing.Point(142, 228);
            this.addressCmbBox.Name = "addressCmbBox";
            this.addressCmbBox.Size = new System.Drawing.Size(85, 28);
            this.addressCmbBox.TabIndex = 29;
            this.addressCmbBox.TextChanged += new System.EventHandler(this.addressCmbBox_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.readCurBtn);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.numericUpDown3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.numericUpDown2);
            this.groupBox4.Controls.Add(this.readDailyBtn);
            this.groupBox4.Controls.Add(this.dateTimePicker1);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(581, 463);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(399, 107);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Тест функций СО";
            // 
            // readCurBtn
            // 
            this.readCurBtn.Location = new System.Drawing.Point(263, 63);
            this.readCurBtn.Name = "readCurBtn";
            this.readCurBtn.Size = new System.Drawing.Size(111, 27);
            this.readCurBtn.TabIndex = 28;
            this.readCurBtn.Text = "Текущие";
            this.readCurBtn.UseVisualStyleBackColor = true;
            this.readCurBtn.Click += new System.EventHandler(this.readCurBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(191, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 20);
            this.label5.TabIndex = 27;
            this.label5.Text = "Канал:";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(262, 25);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(76, 26);
            this.numericUpDown3.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 25;
            this.label4.Text = "Адрес:";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(79, 25);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(76, 26);
            this.numericUpDown2.TabIndex = 24;
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // readDailyBtn
            // 
            this.readDailyBtn.Location = new System.Drawing.Point(146, 63);
            this.readDailyBtn.Name = "readDailyBtn";
            this.readDailyBtn.Size = new System.Drawing.Size(111, 27);
            this.readDailyBtn.TabIndex = 23;
            this.readDailyBtn.Text = "Суточные";
            this.readDailyBtn.UseVisualStyleBackColor = true;
            this.readDailyBtn.Click += new System.EventHandler(this.readDailyBtn_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(16, 64);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(124, 26);
            this.dateTimePicker1.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 582);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.apatorBtn);
            this.Controls.Add(this.apatorScreen);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ElfApator - тестовая утилита";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.apatorBtn)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem encryptorCRC8ToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.RichTextBox apatorScreen;
        private System.Windows.Forms.PictureBox apatorBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button lastRecBtn;
        private System.Windows.Forms.Button byIdBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button reqUd2_btn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox addressCmbBox;
        private System.Windows.Forms.Button portSettingsApplyBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox portCmbBox;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button infoBtn;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button readDailyBtn;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button readCurBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox addressDecCmb;
        private System.Windows.Forms.Label label6;
    }
}

