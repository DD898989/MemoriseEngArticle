using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication3
{
    //====================================================================================
    public partial class Form1 : Form
    {
        //====================================================================================
        const string ROOT_PATH = @"D:\MemoriseEngArgicle\";
        const string FILE_LOAD_TEMP = @"Temp.txt";
        const string FILE_INPUT_TEMP = @".InputTemp.txt";
        const string FILE_SOURCE = @".Source.txt";
        const string FILE_SOUND = @".wav";
        //----------------------------------------------------------------
        TextAnalyser _taSource;
        TextAnalyser _taInput;
        TextAnalyser _taShowChar;
        //----------------------------------------------------------------
        [DllImport("winmm.dll")]
        private static extern int mciSendString(string MciComando, string MciRetorno, int MciRetornoLeng, int CallBack);
        //====================================================================================
        private void SaveArticle(string value)
        {
            File.WriteAllText(ROOT_PATH + value + FILE_SOURCE, rtb_0_Source.Text);
            File.WriteAllText(ROOT_PATH + value + FILE_INPUT_TEMP, rtb_0_Input.Text);
            File.WriteAllText(ROOT_PATH + FILE_LOAD_TEMP, value);
            //sound file already saved if has
        }
        //----------------------------------------------------------------
        private void DeleteArticle(string value)
        {
            if (File.Exists(ROOT_PATH + value + FILE_SOURCE))
                File.Delete(ROOT_PATH + value + FILE_SOURCE);
            if (File.Exists(ROOT_PATH + value + FILE_INPUT_TEMP))
                File.Delete(ROOT_PATH + value + FILE_INPUT_TEMP);
            if (File.Exists(ROOT_PATH + value.Replace(" ", "_") + FILE_SOUND))
                File.Delete(ROOT_PATH + value.Replace(" ", "_") + FILE_SOUND);
        }
        //----------------------------------------------------------------
        private string _sName = "";
        private void sCurrentArticle(string name, bool bIsDelete = false)
        {
            Unload_Event();
            {
                DirectoryInfo dir = new DirectoryInfo(ROOT_PATH);

                if (bIsDelete)
                    DeleteArticle(name);
                else if (_sName.Length > 0)
                    SaveArticle(_sName);

                rtb_0_Input.Clear();
                rtb_0_SyncInput.Clear();
                rtb_0_Source.Clear();
                rtb_0_ShowChar.Clear();

                if (!File.Exists(ROOT_PATH + name + FILE_SOURCE))
                {
                    foreach (var file in dir.GetFiles("*" + FILE_SOURCE))
                    {
                        name = file.Name.Replace(FILE_SOURCE, "");
                        break;
                    }

                    if (!File.Exists(ROOT_PATH + name + FILE_SOURCE))
                    {
                        name = "ExampleArticle";
                        File.WriteAllText(ROOT_PATH + name + FILE_SOURCE,
                            "This is an example article for parcticing. Try to keep typing words that you seem from this box to the top-right box. Now you seem the word [ article ] is shown in red, and the next word is [ for ], so you should hit [ space ] then type [ for ] to finish the next word. And the dot mark means any single char, the semicolon mark means any continuous chars, the left-top and right-botton box are hints. You can explore other buttons by yourself, enjoy it.");
                        File.WriteAllText(ROOT_PATH + name + FILE_INPUT_TEMP, "t i a e.....e a;e");
                    }
                }

                _sName = name;

                File.WriteAllText(ROOT_PATH + FILE_LOAD_TEMP, _sName);

                rtb_0_Source.Text = File.ReadAllText(ROOT_PATH + _sName + FILE_SOURCE);
                rtb_0_Source_TextChanged(this, null);

                if (File.Exists(ROOT_PATH + _sName + FILE_INPUT_TEMP))
                    rtb_0_Input.Text = File.ReadAllText(ROOT_PATH + _sName + FILE_INPUT_TEMP);
                rtb_0_Input_TextChanged(this, null);
                rtb_0_Input.SelectionStart = rtb_0_Input.TextLength;
                rtb_0_Input.ScrollToCaret();

                cbx_2_File.Items.Clear();
                foreach (var file in dir.GetFiles("*" + FILE_SOURCE))
                    cbx_2_File.Items.Add(file.Name.Replace(FILE_SOURCE, ""));

                cbx_2_File.SelectedItem = _sName;
            }
            Load_Event();
        }
        //====================================================================================
        public Form1()
        {
            Initialize_Component();
            if (!File.Exists(ROOT_PATH + FILE_LOAD_TEMP))
            {
                Directory.CreateDirectory(ROOT_PATH);
                sCurrentArticle("");
            }
            else
            {
                sCurrentArticle(File.ReadAllText(ROOT_PATH + FILE_LOAD_TEMP));
            }
        }
        //----------------------------------------------------------------
        private void Load_Event()
        {
            this.FormClosing += Form1_FormClosing;

            this.rtb_0_SyncInput.MouseUp += this.MouseUp_To_Highlight_Source;
            //this.rtb_0_Source.MouseUp += this.MouseUp_To_Highlight_Source;
            this.rtb_0_ShowChar.MouseUp += this.rtb_0_ShowChar_MouseUp;

            this.rtb_0_Source.TextChanged += this.rtb_0_Source_TextChanged;
            this.rtb_0_Input.TextChanged += this.rtb_0_Input_TextChanged;

            this.btn_1_ShowChar1.Click += this.btn_1_ShowChar_Click;
            this.btn_1_ShowChar2.Click += this.btn_1_ShowChar_Click;
            this.btn_1_ShowChar3.Click += this.btn_1_ShowChar_Click;

            this.cbx_2_File.SelectedIndexChanged += this.cbx_2_File_SelectedIndexChanged;
            this.btn_2_New.Click += this.btn_2_New_Click;
            this.btn_2_Delete.Click += this.btn_2_Delete_Click;

            this.btn_3_Stop.Click += this.btn_3_Stop_Click;
            this.btn_3_SaveAndPlay.Click += this.btn_3_SaveAndPlay_Click;
            this.btn_3_Record.Click += this.btn_3_Record_Click;
            this.btn_3_LoopPlay.Click += this.btn_3_LoopPlay_Click;
        }
        //----------------------------------------------------------------
        private void Unload_Event()
        {
            this.FormClosing -= Form1_FormClosing;

            this.rtb_0_SyncInput.MouseUp -= this.MouseUp_To_Highlight_Source;
            this.rtb_0_Source.MouseUp -= this.MouseUp_To_Highlight_Source;
            this.rtb_0_ShowChar.MouseUp -= this.rtb_0_ShowChar_MouseUp;

            this.rtb_0_Source.TextChanged -= this.rtb_0_Source_TextChanged;
            this.rtb_0_Input.TextChanged -= this.rtb_0_Input_TextChanged;

            this.btn_1_ShowChar1.Click -= this.btn_1_ShowChar_Click;
            this.btn_1_ShowChar2.Click -= this.btn_1_ShowChar_Click;
            this.btn_1_ShowChar3.Click -= this.btn_1_ShowChar_Click;

            this.cbx_2_File.SelectedIndexChanged -= this.cbx_2_File_SelectedIndexChanged;
            this.btn_2_New.Click -= this.btn_2_New_Click;
            this.btn_2_Delete.Click -= this.btn_2_Delete_Click;

            this.btn_3_Stop.Click -= this.btn_3_Stop_Click;
            this.btn_3_SaveAndPlay.Click -= this.btn_3_SaveAndPlay_Click;
            this.btn_3_Record.Click -= this.btn_3_Record_Click;
            this.btn_3_LoopPlay.Click -= this.btn_3_LoopPlay_Click;
        }
        //----------------------------------------------------------------
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveArticle(_sName);
        }
        //----------------------------------------------------------------
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.T))
            {
                this.TopMost = !this.TopMost;
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        //====================================================================================
        void StopSoundAction(Color clr)
        {
            rtb_0_Source.BackColor = clr;
            rtb_0_SyncInput.BackColor = clr;
            rtb_0_Input.BackColor = clr;
            rtb_0_ShowChar.BackColor = clr;

            Thread.Sleep(500);
            mciSendString("pause Som", null, 0, 0);
            mciSendString("save Som " + ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND, null, 0, 0);
            mciSendString("close Som", null, 0, 0);
            mciSendString("close all", null, 0, 0);
        }
        //----------------------------------------------------------------
        private void btn_3_SaveAndPlay_Click(object sender, EventArgs e)
        {
            StopSoundAction(Color.Black);
            mciSendString("play " + ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND, null, 0, 0);
        }
        //----------------------------------------------------------------
        private void btn_3_LoopPlay_Click(object sender, EventArgs e)
        {
            StopSoundAction(Color.Black);
            mciSendString(string.Format("open \"{0}\" type mpegvideo alias media", ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND), null, 0, 0);
            mciSendString("play media repeat", null, 0, 0);
        }
        //----------------------------------------------------------------
        private void btn_3_Record_Click(object sender, EventArgs e)
        {
            StopSoundAction(Color.Green);
            mciSendString("open new type waveaudio alias Som", null, 0, 0);
            mciSendString("record Som", null, 0, 0);
        }
        //----------------------------------------------------------------
        private void btn_3_Stop_Click(object sender, EventArgs e)
        {
            StopSoundAction(Color.Black);
        }
        //====================================================================================
        private void btn_1_ShowChar_Click(object sender, EventArgs e)
        {
            int Len;
            Button btn = sender as Button;
            if (btn != null)
                Len = Convert.ToInt32(btn.Text);//hit btn
            else
                Len = 1;//open program

            rtb_0_ShowChar.Clear();
            string merge = "";
            for (int k = 0; k < _taSource.count; k++)
            {
                int len = 1;
                if (_taSource.WordLen(k) >= Len)
                    len = Len;

                merge += _taSource.Word(k).Substring(0, len);
                merge += new String('_', _taSource.WordLen(k) - len);
                merge += TextAnalyser.s_lsSourceMark[k];
                if (k != _taSource.count - 1)
                    merge += " ";
            }

            rtb_0_ShowChar.Text = merge;
            _taShowChar = new TextAnalyser(rtb_0_ShowChar.Text, @"([a-zA-Z0-9_]+)"); //for erase unnecessary space and newline in source
        }
        //====================================================================================
        private void rtb_0_ShowChar_MouseUp(object sender, EventArgs e)
        {
            string sLine = rtb_0_ShowChar.Text.Substring(0, rtb_0_ShowChar.SelectionStart);
            int spaceNum = sLine.Split(' ').Length - 1;
            int idx = _taSource.WordIdx(spaceNum);
            HighlightText(rtb_0_Source, idx, 1, Color.Red, true);
            TextBoxMoveTo(rtb_0_Source, idx);
        }
        //----------------------------------------------------------------
        int _oriHash = 0;
        private void rtb_0_Source_TextChanged(object sender, EventArgs e)
        {
            int newHash = rtb_0_Source.Text.GetHashCode();

            if (_oriHash == newHash)
                return;
            else
                _oriHash = newHash;

            _taSource = new TextAnalyser(rtb_0_Source.Text, @"([a-zA-Z0-9\'\-]+)");
            _taSource.AnalyzeSingleMarkForShowChar(rtb_0_Source.Text);

            btn_1_ShowChar_Click(this, null);
        }
        //----------------------------------------------------------------
        static int spaceCount = 0;
        private void rtb_0_Input_TextChanged(object sender, EventArgs e)
        {
            if (rtb_0_Input.TextLength == 0)
                return;

            _taInput = new TextAnalyser(rtb_0_Input.Text, @"([a-zA-Z0-9;\.\'\-]+)");

            if (_taInput.count > _taSource.count || _taInput.count < 1)
                return;

            string sLastInput = _taInput.Word(_taInput.count - 1).ToLower();
            string sLastInputRegex = sLastInput.Replace(";", ".+");
            string sLastSource = _taSource.Word(_taInput.count - 1).ToLower();
            char sLastChar = sLastInput[sLastInput.Length - 1];
            char sLastCharInTB = char.ToLower(rtb_0_Input.Text[rtb_0_Input.Text.Length - 1]);


            if (sLastCharInTB == ' ')
                spaceCount++;

            if (sLastInput == sLastSource[0].ToString())
            {
                rtb_0_Input.BackColor = Color.Black;
                spaceCount = 0;
            }
            else if (sLastSource == Regex.Match(sLastSource, sLastInputRegex/*   */, RegexOptions.RightToLeft).ToString())
            {
                if (sLastCharInTB == ' ')
                    rtb_0_Input.BackColor = Color.Black;
                else
                    rtb_0_Input.BackColor = Color.Brown;

                spaceCount = 0;
            }
            else if (sLastSource == Regex.Match(sLastSource, sLastInputRegex + ".+", RegexOptions.RightToLeft).ToString() && sLastCharInTB != ' ')
            {
                rtb_0_Input.BackColor = Color.Brown;
            }
            else
            {
                Console.WriteLine(spaceCount);
                if (spaceCount >= 2)
                {
                    spaceCount = 0;
                }
                else
                {
                    this.rtb_0_Input.TextChanged -= this.rtb_0_Input_TextChanged;
                    try//for start the process
                    {
                        SendKeys.Send("{BACKSPACE}");
                    }
                    catch
                    { 
                    
                    }
                    this.rtb_0_Input.TextChanged += this.rtb_0_Input_TextChanged;
                }
            }



            int idx;
            int len;

            idx = _taSource.WordIdx(_taInput.count - 1);
            len = _taSource.WordLen(_taInput.count - 1);
            HighlightText(rtb_0_Source, idx, len, Color.Red, true);
            TextBoxMoveTo(rtb_0_Source, idx);



            if (_taSource.count > _taInput.count)
            {
                idx = _taSource.WordIdx(_taInput.count) + 1;
                rtb_0_SyncInput.Text = rtb_0_Source.Text.Substring(0, idx) + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n";
            }

            rtb_0_SyncInput.SelectionStart = rtb_0_SyncInput.TextLength;
            rtb_0_SyncInput.ScrollToCaret();
            rtb_0_SyncInput.ScrollToCaret();//weird
            //HighlightText(rtb_0_SyncInput, ".", Color.Red, false);
            //HighlightText(rtb_0_SyncInput, ",", Color.Red, false);
            //HighlightText(rtb_0_SyncInput, "?", Color.Red, false);
            //HighlightText(rtb_0_SyncInput, ";", Color.Red, false);
            //HighlightText(rtb_0_SyncInput, "!", Color.Red, false);


            idx = _taShowChar.WordIdx(_taInput.count - 1);
            len = _taShowChar.WordLen(_taInput.count - 1);
            HighlightText(rtb_0_ShowChar, idx, len, Color.Red, true);
            TextBoxMoveTo(rtb_0_ShowChar, idx);
        }
        //----------------------------------------------------------------
        private void MouseUp_To_Highlight_Source(object sender, MouseEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            if (rtb.SelectedText.Length > 0)
            {
                HighlightText(rtb_0_Source, rtb.SelectedText, Color.Red, true);
            }
        }
        //----------------------------------------------------------------
        private void TextBoxMoveTo(RichTextBox rtb, int idx)
        {
            if (idx > 140)
                rtb.SelectionStart = idx - 140;
            else
                rtb.SelectionStart = 0;

            rtb.ScrollToCaret();
            rtb.ScrollToCaret();
        }
        //====================================================================================
        private void HighlightText(RichTextBox rtb, string sWord, Color color, bool bResetColor)
        {
            if (bResetColor)
            {
                rtb.Select(0, rtb.TextLength);
                rtb.SelectionColor = Color.White;
            }

            int Len = sWord.Length;
            int nStart = rtb.SelectionStart, nStartIndex = 0, index;

            while ((index = rtb.Text.ToLower().IndexOf(sWord, nStartIndex)) != -1)
            {
                rtb.Select(index, Len);
                rtb.SelectionColor = color;

                nStartIndex = index + sWord.Length;

                //rtb.ScrollToCaret();
            }
        }
        //----------------------------------------------------------------
        private void HighlightText(RichTextBox rtb, int nIndex, int nLen, Color color, bool bResetColor)
        {
            if (bResetColor)
            {
                rtb.Select(0, rtb.TextLength);
                rtb.SelectionColor = Color.White;
            }

            int nStart = rtb.SelectionStart, index;
            if ((index = nIndex) != -1 && nIndex != 0)
            {
                rtb.Select(index, nLen);
                rtb.SelectionColor = color;
                //rtb.ScrollToCaret();
            }
        }
        //====================================================================================
        private void btn_2_New_Click(object sender, EventArgs e)
        {
            string temp = "";
            ShowInputDialog(ref temp);
            File.WriteAllText(ROOT_PATH + temp + FILE_SOURCE, "");
            sCurrentArticle(temp);
        }
        //----------------------------------------------------------------
        private void cbx_2_File_SelectedIndexChanged(object sender, EventArgs e)
        {
            sCurrentArticle(cbx_2_File.SelectedItem.ToString());
        }
        //----------------------------------------------------------------        
        private void btn_2_Delete_Click(object sender, EventArgs e)
        {
            sCurrentArticle(cbx_2_File.Text, true);
        }
        //----------------------------------------------------------------
        private static DialogResult ShowInputDialog(ref string input)
        {
            Size size = new Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Name";

            TextBox textBox = new TextBox();
            textBox.Size = new Size(size.Width - 10, 23);
            textBox.Location = new Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
        //====================================================================================
    }

    //====================================================================================
    class TextAnalyser
    {
        //----------------------------------------------------------------
        private int _nCount;
        private List<string> _lsWord = new List<string>();
        private List<int> _lsWordIdx = new List<int>();
        private List<int> _lsWordLen = new List<int>();
        //----------------------------------------------------------------
        static public List<string> s_lsSourceMark = new List<string>();
        //----------------------------------------------------------------
        public int count
        {
            get { return this._nCount; }
        }
        //----------------------------------------------------------------
        public string Word(int n)
        {
            return this._lsWord[n];
        }
        //----------------------------------------------------------------
        public int WordIdx(int n)
        {
            return this._lsWordIdx[n];
        }
        //----------------------------------------------------------------
        public int WordLen(int n)
        {
            return this._lsWordLen[n];
        }
        //----------------------------------------------------------------
        public TextAnalyser(string text, string pattern)
        {
            MatchCollection M1 = Regex.Matches(text, pattern);
            string temp;
            for (int i = 0; i < M1.Count; i++)
            {
                temp = M1[i].ToString();
                _lsWord.Add(temp);
                _lsWordIdx.Add(M1[i].Index);
                _lsWordLen.Add(temp.Length);
            }
            _nCount = M1.Count;
        }
        //----------------------------------------------------------------
        public void AnalyzeSingleMarkForShowChar(string text)
        {
            s_lsSourceMark.Clear();
            for (int i = 0; i < _nCount; i++)
            {
                if (i == _nCount - 1)
                    s_lsSourceMark.Add("");
                else
                {
                    char ch = text[WordIdx(i) + WordLen(i)];
                    if (ch != ' ')
                        s_lsSourceMark.Add(ch.ToString());
                    else
                        s_lsSourceMark.Add("");
                }
            }
        }
        //----------------------------------------------------------------
    }
    //====================================================================================

    partial class Form1
    {
        private void Initialize_Component()
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
            this.SuspendLayout();
            // 
            // cbx_2_File
            // 
            this.cbx_2_File.Location = new System.Drawing.Point(588, 1);
            this.cbx_2_File.Name = "cbx_2_File";
            this.cbx_2_File.Size = new System.Drawing.Size(121, 20);


            this.rtb_0_ShowChar.Name = "rtb_0_ShowChar";
            this.rtb_0_Input.Name = "rtb_0_Input";
            this.rtb_0_Source.Name = "rtb_0_Source";
            this.rtb_0_SyncInput.Name = "rtb_0_SyncInput";

            this.rtb_0_Input.Size = new System.Drawing.Size(425, 332);
            this.rtb_0_ShowChar.Size = new System.Drawing.Size(387, 330);
            this.rtb_0_SyncInput.Size = new System.Drawing.Size(466, 332);
            this.rtb_0_Source.Size = new System.Drawing.Size(454, 380);

            this.rtb_0_ShowChar.Location = new System.Drawing.Point(495, 419);
            this.rtb_0_Source.Location = new System.Drawing.Point(23, 369);
            this.rtb_0_Input.Location = new System.Drawing.Point(495, 23);
            this.rtb_0_SyncInput.Location = new System.Drawing.Point(23, 23);

            this.rtb_0_ShowChar.BackColor = System.Drawing.SystemColors.InfoText;
            this.rtb_0_Input.BackColor = System.Drawing.SystemColors.InfoText;
            this.rtb_0_SyncInput.BackColor = System.Drawing.SystemColors.InfoText;
            this.rtb_0_Source.BackColor = System.Drawing.SystemColors.InfoText;

            this.rtb_0_ShowChar.ForeColor = System.Drawing.SystemColors.Window;
            this.rtb_0_Input.ForeColor = System.Drawing.SystemColors.Window;
            this.rtb_0_SyncInput.ForeColor = System.Drawing.SystemColors.Window;
            this.rtb_0_Source.ForeColor = System.Drawing.SystemColors.Window;

            this.rtb_0_ShowChar.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.rtb_0_Input.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold);
            this.rtb_0_SyncInput.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold);
            this.rtb_0_Source.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.rtb_0_ShowChar.ReadOnly = true;
            this.rtb_0_SyncInput.ReadOnly = true;


            this.btn_1_ShowChar3.Location = new System.Drawing.Point(885, 515);
            this.btn_1_ShowChar3.Name = "btn_1_ShowChar3";
            this.btn_1_ShowChar3.Size = new System.Drawing.Size(35, 18);
            this.btn_1_ShowChar3.Text = "3";
            this.btn_1_ShowChar3.UseVisualStyleBackColor = true;
            this.btn_1_ShowChar2.Location = new System.Drawing.Point(885, 488);
            this.btn_1_ShowChar2.Name = "btn_1_ShowChar2";
            this.btn_1_ShowChar2.Size = new System.Drawing.Size(35, 21);
            this.btn_1_ShowChar2.Text = "2";
            this.btn_1_ShowChar2.UseVisualStyleBackColor = true;
            this.btn_1_ShowChar1.Location = new System.Drawing.Point(885, 462);
            this.btn_1_ShowChar1.Name = "btn_1_ShowChar1";
            this.btn_1_ShowChar1.Size = new System.Drawing.Size(36, 20);
            this.btn_1_ShowChar1.Text = "1";
            this.btn_1_ShowChar1.UseVisualStyleBackColor = true;



            // 
            // btn_3_Stop
            // 
            this.btn_3_Stop.Location = new System.Drawing.Point(742, 361);
            this.btn_3_Stop.Name = "btn_3_Stop";
            this.btn_3_Stop.Size = new System.Drawing.Size(70, 52);

            this.btn_3_Stop.Text = "Stop Play/Record";
            this.btn_3_Stop.UseVisualStyleBackColor = true;
            // 
            // btn_3_Pause
            // 
            this.btn_3_Pause.Location = new System.Drawing.Point(667, 360);
            this.btn_3_Pause.Name = "btn_3_Pause";
            this.btn_3_Pause.Size = new System.Drawing.Size(69, 52);

            this.btn_3_Pause.Text = "Pause";
            this.btn_3_Pause.UseVisualStyleBackColor = true;
            // 
            // Btn_Save_and_Play
            // 
            this.btn_3_SaveAndPlay.Location = new System.Drawing.Point(591, 360);
            this.btn_3_SaveAndPlay.Name = "Btn_Save_and_Play";
            this.btn_3_SaveAndPlay.Size = new System.Drawing.Size(70, 53);

            this.btn_3_SaveAndPlay.Text = "Save/Play";
            this.btn_3_SaveAndPlay.UseVisualStyleBackColor = true;
            // 
            // btn_3_Record
            // 
            this.btn_3_Record.Location = new System.Drawing.Point(504, 360);
            this.btn_3_Record.Name = "btn_3_Record";
            this.btn_3_Record.Size = new System.Drawing.Size(81, 52);

            this.btn_3_Record.Text = "Record";
            this.btn_3_Record.UseVisualStyleBackColor = true;
            // 
            // btn_2_New
            // 
            this.btn_2_New.Location = new System.Drawing.Point(715, -3);
            this.btn_2_New.Name = "btn_2_New";
            this.btn_2_New.Size = new System.Drawing.Size(110, 24);

            this.btn_2_New.Text = "New";
            this.btn_2_New.UseVisualStyleBackColor = true;
            // 
            // btn_2_Delete
            // 
            this.btn_2_Delete.Location = new System.Drawing.Point(831, -3);
            this.btn_2_Delete.Name = "btn_2_Delete";
            this.btn_2_Delete.Size = new System.Drawing.Size(110, 24);

            this.btn_2_Delete.Text = "Delete";
            this.btn_2_Delete.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.btn_3_LoopPlay.Location = new System.Drawing.Point(615, 360);
            this.btn_3_LoopPlay.Name = "button3";
            this.btn_3_LoopPlay.Size = new System.Drawing.Size(46, 21);

            this.btn_3_LoopPlay.Font = new System.Drawing.Font("PMingLiU", 7F);
            this.btn_3_LoopPlay.Text = "Loop";
            this.btn_3_LoopPlay.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(933, 763);
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
            this.Name = "Form1";
            this.Text = "Form1";
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
    }

    //====================================================================================
}
