namespace WindowsFormsApplication3
{
    partial class Form1
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cbx_2_File = new System.Windows.Forms.ComboBox();
            this.rtb_0_SyncInput = new System.Windows.Forms.RichTextBox();
            this.rtb_0_Source = new System.Windows.Forms.RichTextBox();
            this.btn_1_ShowChar3 = new System.Windows.Forms.Button();
            this.btn_1_ShowChar2 = new System.Windows.Forms.Button();
            this.btn_1_ShowChar1 = new System.Windows.Forms.Button();
            this.rtb_0_ShowChar = new System.Windows.Forms.RichTextBox();
            this.rtb_0_Input = new System.Windows.Forms.RichTextBox();
            this.btn_3_Stop = new System.Windows.Forms.Button();
            this.btn_3_Pause = new System.Windows.Forms.Button();
            this.btn_3_SaveAndPlay = new System.Windows.Forms.Button();
            this.btn_3_Record = new System.Windows.Forms.Button();
            this.btn_2_New = new System.Windows.Forms.Button();
            this.btn_2_Delete = new System.Windows.Forms.Button();
            this.btn_3_LoopPlay = new System.Windows.Forms.Button();
            this.查單字_lb_顯示英文 = new System.Windows.Forms.ListBox();
            this.查單字_lb_顯示中文 = new System.Windows.Forms.ListBox();
            this.查單字_tb_輸入英文 = new System.Windows.Forms.TextBox();
            this.查單字_tb_顯示音標 = new System.Windows.Forms.TextBox();
            this.查單字_tb_輸入音標 = new System.Windows.Forms.TextBox();
            this.查單字_tb_顯示變化 = new System.Windows.Forms.TextBox();
            this.btn_mode = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.查單字_test_button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.考單字_tb_目標單字 = new System.Windows.Forms.TextBox();
            this.考單字_tb_目標音標 = new System.Windows.Forms.TextBox();
            this.考單字_tb_目標解釋 = new System.Windows.Forms.TextBox();
            this.考單字_lb_一樣的 = new System.Windows.Forms.ListBox();
            this.考單字_label1 = new System.Windows.Forms.Label();
            this.查單字_checkBox2 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbx_2_File
            // 
            this.cbx_2_File.Location = new System.Drawing.Point(588, 1);
            this.cbx_2_File.Name = "cbx_2_File";
            this.cbx_2_File.Size = new System.Drawing.Size(121, 20);
            this.cbx_2_File.TabIndex = 3;
            // 
            // rtb_0_SyncInput
            // 
            this.rtb_0_SyncInput.BackColor = System.Drawing.SystemColors.InfoText;
            this.rtb_0_SyncInput.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold);
            this.rtb_0_SyncInput.ForeColor = System.Drawing.SystemColors.Window;
            this.rtb_0_SyncInput.Location = new System.Drawing.Point(23, 417);
            this.rtb_0_SyncInput.Name = "rtb_0_SyncInput";
            this.rtb_0_SyncInput.ReadOnly = true;
            this.rtb_0_SyncInput.Size = new System.Drawing.Size(466, 332);
            this.rtb_0_SyncInput.TabIndex = 4;
            this.rtb_0_SyncInput.Text = "";
            // 
            // rtb_0_Source
            // 
            this.rtb_0_Source.BackColor = System.Drawing.SystemColors.InfoText;
            this.rtb_0_Source.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_0_Source.ForeColor = System.Drawing.SystemColors.Window;
            this.rtb_0_Source.Location = new System.Drawing.Point(23, 28);
            this.rtb_0_Source.Name = "rtb_0_Source";
            this.rtb_0_Source.Size = new System.Drawing.Size(454, 380);
            this.rtb_0_Source.TabIndex = 5;
            this.rtb_0_Source.Text = "";
            this.rtb_0_Source.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TextBox3_MouseClick);
            // 
            // btn_1_ShowChar3
            // 
            this.btn_1_ShowChar3.Location = new System.Drawing.Point(885, 515);
            this.btn_1_ShowChar3.Name = "btn_1_ShowChar3";
            this.btn_1_ShowChar3.Size = new System.Drawing.Size(35, 18);
            this.btn_1_ShowChar3.TabIndex = 6;
            this.btn_1_ShowChar3.Text = "3";
            this.btn_1_ShowChar3.UseVisualStyleBackColor = true;
            // 
            // btn_1_ShowChar2
            // 
            this.btn_1_ShowChar2.Location = new System.Drawing.Point(885, 488);
            this.btn_1_ShowChar2.Name = "btn_1_ShowChar2";
            this.btn_1_ShowChar2.Size = new System.Drawing.Size(35, 21);
            this.btn_1_ShowChar2.TabIndex = 7;
            this.btn_1_ShowChar2.Text = "2";
            this.btn_1_ShowChar2.UseVisualStyleBackColor = true;
            // 
            // btn_1_ShowChar1
            // 
            this.btn_1_ShowChar1.Location = new System.Drawing.Point(885, 462);
            this.btn_1_ShowChar1.Name = "btn_1_ShowChar1";
            this.btn_1_ShowChar1.Size = new System.Drawing.Size(36, 20);
            this.btn_1_ShowChar1.TabIndex = 8;
            this.btn_1_ShowChar1.Text = "1";
            this.btn_1_ShowChar1.UseVisualStyleBackColor = true;
            // 
            // rtb_0_ShowChar
            // 
            this.rtb_0_ShowChar.BackColor = System.Drawing.SystemColors.InfoText;
            this.rtb_0_ShowChar.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_0_ShowChar.ForeColor = System.Drawing.Color.DarkGray;
            this.rtb_0_ShowChar.Location = new System.Drawing.Point(495, 446);
            this.rtb_0_ShowChar.Name = "rtb_0_ShowChar";
            this.rtb_0_ShowChar.ReadOnly = true;
            this.rtb_0_ShowChar.Size = new System.Drawing.Size(387, 303);
            this.rtb_0_ShowChar.TabIndex = 9;
            this.rtb_0_ShowChar.Text = "";
            // 
            // rtb_0_Input
            // 
            this.rtb_0_Input.BackColor = System.Drawing.SystemColors.InfoText;
            this.rtb_0_Input.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold);
            this.rtb_0_Input.ForeColor = System.Drawing.SystemColors.Window;
            this.rtb_0_Input.Location = new System.Drawing.Point(495, 23);
            this.rtb_0_Input.Name = "rtb_0_Input";
            this.rtb_0_Input.Size = new System.Drawing.Size(425, 332);
            this.rtb_0_Input.TabIndex = 10;
            this.rtb_0_Input.Text = "";
            // 
            // btn_3_Stop
            // 
            this.btn_3_Stop.Location = new System.Drawing.Point(742, 361);
            this.btn_3_Stop.Name = "btn_3_Stop";
            this.btn_3_Stop.Size = new System.Drawing.Size(70, 52);
            this.btn_3_Stop.TabIndex = 11;
            this.btn_3_Stop.Text = "Stop Play/Record";
            this.btn_3_Stop.UseVisualStyleBackColor = true;
            // 
            // btn_3_Pause
            // 
            this.btn_3_Pause.Location = new System.Drawing.Point(667, 360);
            this.btn_3_Pause.Name = "btn_3_Pause";
            this.btn_3_Pause.Size = new System.Drawing.Size(69, 52);
            this.btn_3_Pause.TabIndex = 12;
            this.btn_3_Pause.Text = "Pause";
            this.btn_3_Pause.UseVisualStyleBackColor = true;
            // 
            // btn_3_SaveAndPlay
            // 
            this.btn_3_SaveAndPlay.Location = new System.Drawing.Point(591, 360);
            this.btn_3_SaveAndPlay.Name = "btn_3_SaveAndPlay";
            this.btn_3_SaveAndPlay.Size = new System.Drawing.Size(70, 53);
            this.btn_3_SaveAndPlay.TabIndex = 13;
            this.btn_3_SaveAndPlay.Text = "Save/Play";
            this.btn_3_SaveAndPlay.UseVisualStyleBackColor = true;
            // 
            // btn_3_Record
            // 
            this.btn_3_Record.Location = new System.Drawing.Point(504, 360);
            this.btn_3_Record.Name = "btn_3_Record";
            this.btn_3_Record.Size = new System.Drawing.Size(81, 52);
            this.btn_3_Record.TabIndex = 14;
            this.btn_3_Record.Text = "Record";
            this.btn_3_Record.UseVisualStyleBackColor = true;
            // 
            // btn_2_New
            // 
            this.btn_2_New.Location = new System.Drawing.Point(715, -3);
            this.btn_2_New.Name = "btn_2_New";
            this.btn_2_New.Size = new System.Drawing.Size(54, 24);
            this.btn_2_New.TabIndex = 2;
            this.btn_2_New.Text = "New";
            this.btn_2_New.UseVisualStyleBackColor = true;
            // 
            // btn_2_Delete
            // 
            this.btn_2_Delete.Location = new System.Drawing.Point(775, -3);
            this.btn_2_Delete.Name = "btn_2_Delete";
            this.btn_2_Delete.Size = new System.Drawing.Size(50, 24);
            this.btn_2_Delete.TabIndex = 1;
            this.btn_2_Delete.Text = "Delete";
            this.btn_2_Delete.UseVisualStyleBackColor = true;
            // 
            // btn_3_LoopPlay
            // 
            this.btn_3_LoopPlay.Font = new System.Drawing.Font("PMingLiU", 7F);
            this.btn_3_LoopPlay.Location = new System.Drawing.Point(615, 360);
            this.btn_3_LoopPlay.Name = "btn_3_LoopPlay";
            this.btn_3_LoopPlay.Size = new System.Drawing.Size(46, 21);
            this.btn_3_LoopPlay.TabIndex = 0;
            this.btn_3_LoopPlay.Text = "Loop";
            this.btn_3_LoopPlay.UseVisualStyleBackColor = true;
            // 
            // 查單字_lb_顯示英文
            // 
            this.查單字_lb_顯示英文.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.查單字_lb_顯示英文.Font = new System.Drawing.Font("PMingLiU", 17F);
            this.查單字_lb_顯示英文.ForeColor = System.Drawing.SystemColors.Info;
            this.查單字_lb_顯示英文.FormattingEnabled = true;
            this.查單字_lb_顯示英文.ItemHeight = 23;
            this.查單字_lb_顯示英文.Location = new System.Drawing.Point(398, 28);
            this.查單字_lb_顯示英文.Name = "查單字_lb_顯示英文";
            this.查單字_lb_顯示英文.Size = new System.Drawing.Size(427, 188);
            this.查單字_lb_顯示英文.TabIndex = 27;
            this.查單字_lb_顯示英文.SelectedIndexChanged += new System.EventHandler(this.查單字_lb_顯示英文_SelectedIndexChanged);
            // 
            // 查單字_lb_顯示中文
            // 
            this.查單字_lb_顯示中文.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.查單字_lb_顯示中文.Font = new System.Drawing.Font("PMingLiU", 17F);
            this.查單字_lb_顯示中文.ForeColor = System.Drawing.SystemColors.Info;
            this.查單字_lb_顯示中文.FormattingEnabled = true;
            this.查單字_lb_顯示中文.ItemHeight = 23;
            this.查單字_lb_顯示中文.Location = new System.Drawing.Point(398, 258);
            this.查單字_lb_顯示中文.Name = "查單字_lb_顯示中文";
            this.查單字_lb_顯示中文.Size = new System.Drawing.Size(427, 188);
            this.查單字_lb_顯示中文.TabIndex = 28;
            // 
            // 查單字_tb_輸入英文
            // 
            this.查單字_tb_輸入英文.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.查單字_tb_輸入英文.Font = new System.Drawing.Font("PMingLiU", 14F);
            this.查單字_tb_輸入英文.ForeColor = System.Drawing.SystemColors.Info;
            this.查單字_tb_輸入英文.Location = new System.Drawing.Point(37, 28);
            this.查單字_tb_輸入英文.Name = "查單字_tb_輸入英文";
            this.查單字_tb_輸入英文.Size = new System.Drawing.Size(188, 30);
            this.查單字_tb_輸入英文.TabIndex = 29;
            this.查單字_tb_輸入英文.TextChanged += new System.EventHandler(this.查單字_tb_輸入英文_TextChanged);
            // 
            // 查單字_tb_顯示音標
            // 
            this.查單字_tb_顯示音標.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.查單字_tb_顯示音標.Font = new System.Drawing.Font("PMingLiU", 14F);
            this.查單字_tb_顯示音標.ForeColor = System.Drawing.SystemColors.Info;
            this.查單字_tb_顯示音標.Location = new System.Drawing.Point(398, 222);
            this.查單字_tb_顯示音標.Name = "查單字_tb_顯示音標";
            this.查單字_tb_顯示音標.Size = new System.Drawing.Size(188, 30);
            this.查單字_tb_顯示音標.TabIndex = 30;
            // 
            // 查單字_tb_輸入音標
            // 
            this.查單字_tb_輸入音標.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.查單字_tb_輸入音標.Font = new System.Drawing.Font("PMingLiU", 14F);
            this.查單字_tb_輸入音標.ForeColor = System.Drawing.SystemColors.Info;
            this.查單字_tb_輸入音標.Location = new System.Drawing.Point(37, 149);
            this.查單字_tb_輸入音標.Name = "查單字_tb_輸入音標";
            this.查單字_tb_輸入音標.Size = new System.Drawing.Size(188, 30);
            this.查單字_tb_輸入音標.TabIndex = 31;
            this.查單字_tb_輸入音標.TextChanged += new System.EventHandler(this.查單字_tb_輸入音標_TextChanged);
            // 
            // 查單字_tb_顯示變化
            // 
            this.查單字_tb_顯示變化.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.查單字_tb_顯示變化.Font = new System.Drawing.Font("PMingLiU", 14F);
            this.查單字_tb_顯示變化.ForeColor = System.Drawing.SystemColors.Info;
            this.查單字_tb_顯示變化.Location = new System.Drawing.Point(37, 249);
            this.查單字_tb_顯示變化.Multiline = true;
            this.查單字_tb_顯示變化.Name = "查單字_tb_顯示變化";
            this.查單字_tb_顯示變化.Size = new System.Drawing.Size(188, 173);
            this.查單字_tb_顯示變化.TabIndex = 32;
            // 
            // btn_mode
            // 
            this.btn_mode.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btn_mode.Location = new System.Drawing.Point(831, 368);
            this.btn_mode.Name = "btn_mode";
            this.btn_mode.Size = new System.Drawing.Size(89, 44);
            this.btn_mode.TabIndex = 15;
            this.btn_mode.Text = "caps*2 = 字典   alt+<> = 換篇";
            this.btn_mode.UseVisualStyleBackColor = false;
            this.btn_mode.Click += new System.EventHandler(this.button1_Click_字典);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBox1.Font = new System.Drawing.Font("PMingLiU", 18F);
            this.textBox1.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox1.Location = new System.Drawing.Point(156, 309);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(222, 36);
            this.textBox1.TabIndex = 33;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBox2.Font = new System.Drawing.Font("PMingLiU", 18F);
            this.textBox2.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox2.Location = new System.Drawing.Point(156, 351);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(222, 36);
            this.textBox2.TabIndex = 34;
            // 
            // 查單字_test_button1
            // 
            this.查單字_test_button1.Location = new System.Drawing.Point(885, 736);
            this.查單字_test_button1.Name = "查單字_test_button1";
            this.查單字_test_button1.Size = new System.Drawing.Size(50, 24);
            this.查單字_test_button1.TabIndex = 35;
            this.查單字_test_button1.Text = "Test";
            this.查單字_test_button1.UseVisualStyleBackColor = true;
            this.查單字_test_button1.Click += new System.EventHandler(this.查單字_test_button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.checkBox1.Location = new System.Drawing.Point(831, 418);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(99, 16);
            this.checkBox1.TabIndex = 36;
            this.checkBox1.Text = "考句子/考單字";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // 考單字_tb_目標單字
            // 
            this.考單字_tb_目標單字.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.考單字_tb_目標單字.Font = new System.Drawing.Font("PMingLiU", 26F);
            this.考單字_tb_目標單字.ForeColor = System.Drawing.SystemColors.Info;
            this.考單字_tb_目標單字.Location = new System.Drawing.Point(70, 445);
            this.考單字_tb_目標單字.Name = "考單字_tb_目標單字";
            this.考單字_tb_目標單字.Size = new System.Drawing.Size(290, 49);
            this.考單字_tb_目標單字.TabIndex = 37;
            this.考單字_tb_目標單字.TextChanged += new System.EventHandler(this.考單字_tb_目標單字_TextChanged);
            // 
            // 考單字_tb_目標音標
            // 
            this.考單字_tb_目標音標.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.考單字_tb_目標音標.Font = new System.Drawing.Font("PMingLiU", 26F);
            this.考單字_tb_目標音標.ForeColor = System.Drawing.SystemColors.Info;
            this.考單字_tb_目標音標.Location = new System.Drawing.Point(398, 445);
            this.考單字_tb_目標音標.Name = "考單字_tb_目標音標";
            this.考單字_tb_目標音標.Size = new System.Drawing.Size(325, 49);
            this.考單字_tb_目標音標.TabIndex = 38;
            this.考單字_tb_目標音標.TextChanged += new System.EventHandler(this.考單字_tb_目標音標_TextChanged);
            // 
            // 考單字_tb_目標解釋
            // 
            this.考單字_tb_目標解釋.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.考單字_tb_目標解釋.Font = new System.Drawing.Font("PMingLiU", 26F);
            this.考單字_tb_目標解釋.ForeColor = System.Drawing.SystemColors.Info;
            this.考單字_tb_目標解釋.Location = new System.Drawing.Point(267, 373);
            this.考單字_tb_目標解釋.Name = "考單字_tb_目標解釋";
            this.考單字_tb_目標解釋.Size = new System.Drawing.Size(222, 49);
            this.考單字_tb_目標解釋.TabIndex = 39;
            // 
            // 考單字_lb_一樣的
            // 
            this.考單字_lb_一樣的.BackColor = System.Drawing.SystemColors.MenuText;
            this.考單字_lb_一樣的.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.考單字_lb_一樣的.ForeColor = System.Drawing.SystemColors.Window;
            this.考單字_lb_一樣的.FormattingEnabled = true;
            this.考單字_lb_一樣的.ItemHeight = 24;
            this.考單字_lb_一樣的.Location = new System.Drawing.Point(108, 80);
            this.考單字_lb_一樣的.Name = "考單字_lb_一樣的";
            this.考單字_lb_一樣的.Size = new System.Drawing.Size(193, 172);
            this.考單字_lb_一樣的.TabIndex = 40;
            // 
            // 考單字_label1
            // 
            this.考單字_label1.AutoSize = true;
            this.考單字_label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.考單字_label1.Location = new System.Drawing.Point(118, 62);
            this.考單字_label1.Name = "考單字_label1";
            this.考單字_label1.Size = new System.Drawing.Size(89, 12);
            this.考單字_label1.TabIndex = 41;
            this.考單字_label1.Text = "一樣念法的單字";
            // 
            // 查單字_checkBox2
            // 
            this.查單字_checkBox2.AutoSize = true;
            this.查單字_checkBox2.ForeColor = System.Drawing.Color.White;
            this.查單字_checkBox2.Location = new System.Drawing.Point(831, 127);
            this.查單字_checkBox2.Name = "查單字_checkBox2";
            this.查單字_checkBox2.Size = new System.Drawing.Size(84, 16);
            this.查單字_checkBox2.TabIndex = 42;
            this.查單字_checkBox2.Text = "僅顯示常用";
            this.查單字_checkBox2.UseVisualStyleBackColor = true;
            this.查單字_checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(933, 763);
            this.Controls.Add(this.查單字_checkBox2);
            this.Controls.Add(this.考單字_label1);
            this.Controls.Add(this.考單字_lb_一樣的);
            this.Controls.Add(this.考單字_tb_目標解釋);
            this.Controls.Add(this.考單字_tb_目標音標);
            this.Controls.Add(this.考單字_tb_目標單字);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.查單字_test_button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.查單字_tb_顯示變化);
            this.Controls.Add(this.查單字_tb_輸入音標);
            this.Controls.Add(this.查單字_tb_顯示音標);
            this.Controls.Add(this.查單字_tb_輸入英文);
            this.Controls.Add(this.查單字_lb_顯示中文);
            this.Controls.Add(this.查單字_lb_顯示英文);
            this.Controls.Add(this.btn_mode);
            this.Controls.Add(this.btn_3_LoopPlay);
            this.Controls.Add(this.btn_2_Delete);
            this.Controls.Add(this.btn_2_New);
            this.Controls.Add(this.cbx_2_File);
            this.Controls.Add(this.rtb_0_SyncInput);
            this.Controls.Add(this.rtb_0_Source);
            this.Controls.Add(this.btn_1_ShowChar3);
            this.Controls.Add(this.btn_1_ShowChar2);
            this.Controls.Add(this.btn_1_ShowChar1);
            this.Controls.Add(this.rtb_0_ShowChar);
            this.Controls.Add(this.rtb_0_Input);
            this.Controls.Add(this.btn_3_Stop);
            this.Controls.Add(this.btn_3_Pause);
            this.Controls.Add(this.btn_3_SaveAndPlay);
            this.Controls.Add(this.btn_3_Record);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox cbx_2_File;
        private System.Windows.Forms.RichTextBox rtb_0_SyncInput;
        private System.Windows.Forms.RichTextBox rtb_0_Source;
        private System.Windows.Forms.Button btn_1_ShowChar3;
        private System.Windows.Forms.Button btn_1_ShowChar2;
        private System.Windows.Forms.Button btn_1_ShowChar1;
        private System.Windows.Forms.RichTextBox rtb_0_ShowChar;
        private System.Windows.Forms.RichTextBox rtb_0_Input;
        private System.Windows.Forms.Button btn_2_New;
        private System.Windows.Forms.Button btn_2_Delete;
        private System.Windows.Forms.Button btn_3_LoopPlay;
        private System.Windows.Forms.Button btn_3_Stop;
        private System.Windows.Forms.Button btn_3_Pause;
        private System.Windows.Forms.Button btn_3_SaveAndPlay;
        private System.Windows.Forms.Button btn_3_Record;

        #endregion
        private System.Windows.Forms.ListBox 查單字_lb_顯示英文;
        private System.Windows.Forms.ListBox 查單字_lb_顯示中文;
        private System.Windows.Forms.TextBox 查單字_tb_輸入英文;
        private System.Windows.Forms.TextBox 查單字_tb_顯示音標;
        private System.Windows.Forms.TextBox 查單字_tb_輸入音標;
        private System.Windows.Forms.TextBox 查單字_tb_顯示變化;
        private System.Windows.Forms.Button btn_mode;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button 查單字_test_button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox 考單字_tb_目標單字;
        private System.Windows.Forms.TextBox 考單字_tb_目標音標;
        private System.Windows.Forms.TextBox 考單字_tb_目標解釋;
        private System.Windows.Forms.ListBox 考單字_lb_一樣的;
        private System.Windows.Forms.Label 考單字_label1;
        private System.Windows.Forms.CheckBox 查單字_checkBox2;
    }
}

