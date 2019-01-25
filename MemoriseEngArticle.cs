using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication26
{
    //====================================================================================
    //====================================================================================
    //====================================================================================
    //====================================================================================
    //====================================================================================

    public partial class Form1 : Form
    {
        //====================================================================================
        const string ROOT_PATH = @"C:\MemoriseEngArgicle\";
        const string FILE_LOAD_TEMP = @"Temp.txt";
        const string FILE_INPUT_TEMP = @".InputTemp.txt";
        const string FILE_SOURCE = @".Source.txt";
        const string FILE_SOUND = @".wav";
        int _nOri_tbSourceLength = 0;
        //----------------------------------------------------------------
        TextAnalyser _taSource = new TextAnalyser();
        TextAnalyser _taInput = new TextAnalyser();
        TextAnalyser _taShowChar = new TextAnalyser();
        //----------------------------------------------------------------
        [DllImport("winmm.dll")]
        private static extern int mciSendString(string MciComando, string MciRetorno, int MciRetornoLeng, int CallBack);
        //====================================================================================
        public void SaveCurrentArticle()
        {
            try
            {
                File.WriteAllText(ROOT_PATH + _sName + FILE_SOURCE, tbSource.Text);
                File.WriteAllText(ROOT_PATH + _sName + FILE_INPUT_TEMP, tbInput.Text);
                File.WriteAllText(ROOT_PATH + FILE_LOAD_TEMP, _sName);
                //sound file already saved if has
            }
            catch
            {
            }
        }
        //----------------------------------------------------------------
        public void DeleteCurrentArticle()
        {
            try
            {
                File.Delete(ROOT_PATH + _sName + FILE_SOURCE);
                File.Delete(ROOT_PATH + _sName + FILE_INPUT_TEMP);
                File.Delete(ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND);
            }
            catch
            {
            }
        }
        //----------------------------------------------------------------
        private string _sName = "";
        public void sCurrentArticle(string value, bool bIsDeleteCurrent)
        {
            DirectoryInfo dir = new DirectoryInfo(ROOT_PATH);

            if (bIsDeleteCurrent)
            {
                if (comboBox1.Items.Count == 1)
                    return;

                DeleteCurrentArticle();

                foreach (var file in dir.GetFiles("*.txt"))
                {
                    if (file.Name.Contains(FILE_SOURCE))
                    {
                        value = file.Name.Replace(FILE_SOURCE, "");
                        break;
                    }
                }

            }
            else if (_sName.Length > 0)
            {
                SaveCurrentArticle();
            }

            tbInput.Clear();
            tbSyncInput.Clear();
            tbSource.Clear();
            tbShowChar.Clear();


            /////-----------------------------------------------------------
            /////-----------------------------------------------------------
            /////-----------------------------------------------------------
            _sName = value;//File.ReadAllText(ROOT_PATH + FILE_LOAD_TEMP);
            /////-----------------------------------------------------------
            /////-----------------------------------------------------------
            /////-----------------------------------------------------------


            File.WriteAllText(ROOT_PATH + FILE_LOAD_TEMP, _sName);


            if (File.Exists(ROOT_PATH + _sName + FILE_SOURCE))
            {
            }
            else if (_sName == "ExampleArticle")
            {
                File.WriteAllText(ROOT_PATH + _sName + FILE_SOURCE, "This is an example article for parcticing. Try to keep typing words that you seem from this box to the top-right box. Now you seem the word 'article' is shown in red, and the next word is 'for', so you should hit 'space' then type 'for' to finish the next word. And the dot mark means any single char, the semicolon mark means any continuous chars, the left-top and right-botton box are hints. You can explore other buttons by yourself, enjoy it.");
                File.WriteAllText(ROOT_PATH + _sName + FILE_INPUT_TEMP, "t i a e.....e a;e");
            }
            else
            {
                File.WriteAllText(ROOT_PATH + _sName + FILE_SOURCE, "");
            }
            tbSource.Text = File.ReadAllText(ROOT_PATH + _sName + FILE_SOURCE);
            sourceChanged(this, null);



            if (File.Exists(ROOT_PATH + _sName + FILE_INPUT_TEMP))
                tbInput.Text = File.ReadAllText(ROOT_PATH + _sName + FILE_INPUT_TEMP);


            comboBox1.Items.Clear();
            foreach (var file in dir.GetFiles("*.txt"))
                if (file.Name.Contains(FILE_SOURCE))
                    comboBox1.Items.Add(file.Name.Replace(FILE_SOURCE, ""));

            comboBox1.SelectedItem = _sName;
            return;
        }
        //====================================================================================
        public Form1()
        {
            InitializeComponent2();
            if (!File.Exists(ROOT_PATH + FILE_LOAD_TEMP))
            {
                Directory.CreateDirectory(ROOT_PATH);
                sCurrentArticle("ExampleArticle", false);
            }
            else
            {
                sCurrentArticle(File.ReadAllText(ROOT_PATH + FILE_LOAD_TEMP), false);
            }
        }
        //----------------------------------------------------------------
        public void ByteArrayToFile(string fileName, byte[] btArray)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(btArray, 0, btArray.Length);
            }
        }
        //----------------------------------------------------------------
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveCurrentArticle();
        }
        //----------------------------------------------------------------
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.T))
            {
                this.TopMost = !this.TopMost;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        //----------------------------------------------------------------
        public void ReadTempFileAndRefreshForm()
        {
            _sName = File.ReadAllText(ROOT_PATH + FILE_LOAD_TEMP);

            tbInput.Clear();
            tbSyncInput.Clear();
            tbSource.Clear();
            tbShowChar.Clear();

            string sTemp = File.ReadAllText(ROOT_PATH + _sName + FILE_SOURCE);
            tbSource.Text = sTemp;
            sourceChanged(this, null);

            if (!File.Exists(ROOT_PATH + _sName + FILE_INPUT_TEMP))
            {
                File.Create(ROOT_PATH + _sName + FILE_INPUT_TEMP);
            }
            try
            {
                sTemp = File.ReadAllText(ROOT_PATH + _sName + FILE_INPUT_TEMP);
            }
            catch
            {
                sTemp = "";
            }
            tbInput.Text = sTemp;
            inputChanged(this, null);

            tbInput.SelectionStart = tbInput.TextLength;
            tbInput.ScrollToCaret();
            this.ActiveControl = tbInput;

            DirectoryInfo dir = new DirectoryInfo(ROOT_PATH);

            string sFinal = "";
            comboBox1.Items.Clear();
            foreach (var file in dir.GetFiles("*.txt"))
            {
                if (file.Name.Contains(FILE_SOURCE))
                {
                    sFinal = file.Name.Replace(FILE_SOURCE, "");
                    comboBox1.Items.Add(sFinal);
                }
            }
            comboBox1.SelectedItem = _sName;
        }
        //====================================================================================
        private void Btn_Save_And_Play_Click(object sender, EventArgs e)
        {
            if (tbShowChar.BackColor == Color.Green)
            {
                tbSource.BackColor = Color.Black;
                tbSyncInput.BackColor = Color.Black;
                tbInput.BackColor = Color.Black;
                tbShowChar.BackColor = Color.Black;

                System.Threading.Thread.Sleep(500);
                mciSendString("pause Som", null, 0, 0);
                mciSendString("save Som " + ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND, null, 0, 0);
                mciSendString("close Som", null, 0, 0);
            }

            mciSendString("play " + ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND, null, 0, 0);
        }
        //----------------------------------------------------------------
        private void Btn_Loop_Play_Click(object sender, EventArgs e)
        {
            if (File.Exists(ROOT_PATH + _sName + FILE_SOUND))
                return;

            if (tbShowChar.BackColor == Color.Green)
            {
                tbSource.BackColor = Color.Black;
                tbSyncInput.BackColor = Color.Black;
                tbInput.BackColor = Color.Black;
                tbShowChar.BackColor = Color.Black;

                System.Threading.Thread.Sleep(500);
                mciSendString("pause Som", null, 0, 0);
                mciSendString("save Som " + ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND, null, 0, 0);
                mciSendString("close Som", null, 0, 0);
            }

            mciSendString(string.Format("open \"{0}\" type mpegvideo alias media", ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND), null, 0, 0);
            mciSendString("play media repeat", null, 0, 0);
        }
        //----------------------------------------------------------------
        private void Btn_Record_Click(object sender, EventArgs e)
        {
            tbSource.BackColor = Color.Green;
            tbSyncInput.BackColor = Color.Green;
            tbInput.BackColor = Color.Green;
            tbShowChar.BackColor = Color.Green;

            mciSendString("open new type waveaudio alias Som", null, 0, 0);
            mciSendString("record Som", null, 0, 0);
        }
        //----------------------------------------------------------------
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            if (tbShowChar.BackColor == Color.Green)
            {
                tbSource.BackColor = Color.Black;
                tbSyncInput.BackColor = Color.Black;
                tbInput.BackColor = Color.Black;
                tbShowChar.BackColor = Color.Black;

                System.Threading.Thread.Sleep(500);
                mciSendString("pause Som", null, 0, 0);
                mciSendString("save Som " + ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND, null, 0, 0);
                mciSendString("close Som", null, 0, 0);
            }
            else
            {
                mciSendString("close all", null, 0, 0);
            }
        }
        //----------------------------------------------------------------
        private void Btn_Pause_Click(object sender, EventArgs e)
        {
            mciSendString("pause " + ROOT_PATH + _sName.Replace(" ", "_") + FILE_SOUND, null, 0, 0);
        }
        //====================================================================================
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        //----------------------------------------------------------------
        private void btnShowCharClick(object sender, EventArgs e)
        {
            int length;
            try
            {
                Button btnn = (Button)sender;
                length = Convert.ToInt32(btnn.Text);
            }
            catch
            {
                length = 1;
            }

            _taShowChar = Clone(_taSource);  //deep clone!!!!    public static T Clone<T>(T source)  need [Serializable]
            //taShowChar = taSource;         //swallow clone...

            tbShowChar.Clear();
            for (int i = 0; i < _taSource._nCount; i++)
            {
                if (_taSource._list_nSingleWordLength[i] >= length)
                {
                    _taShowChar._list_sSingleWord[i] = _taSource._list_sSingleWord[i].Substring(0, length);
                    for (int j = 0; j < _taSource._list_nSingleWordLength[i] - length; j++)
                        _taShowChar._list_sSingleWord[i] += "_";
                }
                else
                {
                    _taShowChar._list_sSingleWord[i] = _taSource._list_sSingleWord[i].Substring(0, 1);
                    for (int j = 0; j < _taSource._list_nSingleWordLength[i] - 1; j++)
                        _taShowChar._list_sSingleWord[i] += "_";
                }
            }

            tbShowChar.Text = _taShowChar.Merge();
            _taShowChar.Analysis(tbShowChar.Text, @"([a-zA-Z0-9_]+)");//for erase unnecessary space and newline in source
        }
        //====================================================================================
        private void showCharSelectionChanged(object sender, EventArgs e)
        {
            string sLine = tbShowChar.Text.Substring(0, tbShowChar.SelectionStart);
            int spacenumber = sLine.Split(' ').Length - 1;
            try
            {
                HighlightTextWithIndex(tbSource, _taSource._list_nSingleWordFirstIndex[spacenumber], Color.Red);
            }
            catch
            {
            }
        }
        //----------------------------------------------------------------
        private void sourceChanged(object sender, EventArgs e)
        {
            if (_nOri_tbSourceLength == tbSource.TextLength)
                return;
            _taSource.Analysis(tbSource.Text, @"([a-zA-Z0-9\'\-]+)");
            _taSource.AnalyzeSingleMarkForSource(tbSource.Text);
            _nOri_tbSourceLength = tbSource.TextLength;

            btnShowCharClick(this, null);
        }
        //----------------------------------------------------------------
        private void inputChanged(object sender, EventArgs e)
        {
            if (tbInput.TextLength == 0)
                return;

            _taInput.Analysis(tbInput.Text, @"([a-zA-Z0-9;\.\'\-]+)");

            if (_taInput._nCount > _taSource._nCount)
                return;

            string sLastInput = _taInput._list_sSingleWord[_taInput._nCount - 1];
            string sLastSource = _taSource._list_sSingleWord[_taInput._nCount - 1];

            sLastInput = sLastInput.ToLower();
            sLastSource = sLastSource.ToLower();

            char sLastChar = sLastInput[sLastInput.Length - 1];
            sLastInput = sLastInput.Replace(";", ".+");

            Match m1 = Regex.Match(sLastSource, sLastInput, RegexOptions.RightToLeft);
            string sM1 = m1.ToString();
            Match m2 = Regex.Match(sLastSource, sLastInput + ".+", RegexOptions.RightToLeft);
            string sM2 = m2.ToString();

            try
            {
                if (sLastInput.Length == 1 && sLastSource[0] == sLastInput[0])
                    tbInput.BackColor = Color.Black;
                else if (sLastChar == ';')
                    tbInput.BackColor = Color.Brown;
                else if (sM1 == sLastSource)
                    tbInput.BackColor = Color.Black;
                else if (sM2 == sLastSource)
                    tbInput.BackColor = Color.Brown;
                else
                    SendKeys.Send("{BACKSPACE}");
            }
            catch { }


            CoverTextBox.BringToFront();

            #region tbSource react
            try
            {
                tbSource.Select(0, tbSource.TextLength);
                tbSource.SelectionColor = Color.White;
                tbSource.Select(_taSource._list_nSingleWordFirstIndex[_taInput._nCount - 1], _taSource._list_nSingleWordLength[_taInput._nCount - 1]);
                tbSource.SelectionColor = Color.Red;

                if (_taInput._nCount - 1 > 90)
                    tbSource.Select(_taShowChar._list_nSingleWordFirstIndex[_taInput._nCount - 1] - 90, 1);
                else
                    tbSource.Select(1, 1);

                tbSource.ScrollToCaret();
            }
            catch
            {

            }
            #endregion

            #region tbSyncInput react
            try
            {
                tbSyncInput.Text = tbSource.Text.Substring(0, _taSource._list_nSingleWordFirstIndex[_taInput._nCount] + 1) + "\n";
                HighlightTextWithoutResetColor(tbSyncInput, ".", Color.Red);
                HighlightTextWithoutResetColor(tbSyncInput, ",", Color.Red);
                HighlightTextWithoutResetColor(tbSyncInput, "?", Color.Red);
                HighlightTextWithoutResetColor(tbSyncInput, ";", Color.Red);
                HighlightTextWithoutResetColor(tbSyncInput, "!", Color.Red);
            }
            catch
            {
            }
            #endregion

            CoverTextBox.SendToBack();

            #region tbShowChar react
            this.tbShowChar.SelectionChanged -= new System.EventHandler(this.showCharSelectionChanged);
            try
            {
                tbShowChar.Select(0, tbShowChar.TextLength);
                tbShowChar.SelectionColor = Color.White;

                int test1 = _taShowChar._list_nSingleWordFirstIndex[_taInput._nCount - 1];
                int test2 = _taShowChar._list_nSingleWordLength[_taInput._nCount - 1];
                tbShowChar.Select(test1, test2);
                tbShowChar.SelectionColor = Color.Red;

                if (_taInput._nCount - 1 > 90)
                    tbShowChar.Select(_taShowChar._list_nSingleWordFirstIndex[_taInput._nCount] - 90, 1);
                else
                    tbShowChar.Select(1, 1);

                tbShowChar.ScrollToCaret();
                tbShowChar.ScrollToCaret();
            }
            catch
            {
            }
            this.tbShowChar.SelectionChanged += new System.EventHandler(this.showCharSelectionChanged);
            #endregion
        }
        //----------------------------------------------------------------
        public void SyncInputMouseUp(object sender, MouseEventArgs e)
        {
            RichTextBox richTextBox1 = sender as RichTextBox;
            if (richTextBox1.SelectedText.Length > 4)
            {
                HighlightTextAfterResetColor(tbSource, richTextBox1.SelectedText.ToLower(), Color.Red);
            }
        }
        //====================================================================================
        public void HighlightTextAfterResetColor(RichTextBox rtb, string sWord, Color color)
        {
            rtb.Select(0, rtb.TextLength);
            rtb.SelectionColor = Color.White;
            try
            {
                int nStart = rtb.SelectionStart, nStartIndex = 0, index;

                while ((index = rtb.Text.ToLower().IndexOf(sWord, nStartIndex)) != -1)
                {
                    rtb.Select(index, sWord.Length);
                    rtb.SelectionColor = color;

                    nStartIndex = index + sWord.Length;

                    rtb.ScrollToCaret();
                }

                rtb.SelectionStart = nStart;
                rtb.SelectionLength = 0;
                rtb.SelectionColor = Color.White;
            }
            catch
            {
            }
        }
        //----------------------------------------------------------------
        public void HighlightTextWithoutResetColor(RichTextBox rtb, string sWord, Color color)
        {
            try
            {
                int nStart = rtb.SelectionStart, nStartIndex = 0, index;

                while ((index = rtb.Text.ToLower().IndexOf(sWord, nStartIndex)) != -1)
                {
                    rtb.Select(index, sWord.Length);
                    rtb.SelectionColor = color;

                    nStartIndex = index + sWord.Length;

                    rtb.ScrollToCaret();
                }

                rtb.SelectionStart = nStart;
                rtb.SelectionLength = 0;
                rtb.SelectionColor = Color.White;

            }
            catch
            {
            }
        }
        //----------------------------------------------------------------
        public void HighlightTextWithIndex(RichTextBox rtb, int nIndex, Color color)
        {
            rtb.Select(0, rtb.TextLength);
            rtb.SelectionColor = Color.White;
            try
            {
                int nStart = rtb.SelectionStart, nStartIndex = 0, index;
                if ((index = nIndex) != -1 && nIndex != 0)
                {
                    rtb.Select(index, 1);
                    rtb.SelectionColor = color;

                    nStartIndex = index + 1;

                    rtb.ScrollToCaret();
                }

                rtb.SelectionStart = nStart;
                rtb.SelectionLength = 0;
                rtb.SelectionColor = Color.White;
            }
            catch
            {
            }
        }
        //====================================================================================
        private void New_Click(object sender, EventArgs e)
        {
            string temp = "";
            ShowInputDialog(ref temp);
            sCurrentArticle(temp, false);
        }
        //----------------------------------------------------------------
        private void Combobox_SelectChanged(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndexChanged -= new System.EventHandler(this.Combobox_SelectChanged);

            sCurrentArticle(comboBox1.SelectedItem.ToString(), false);

            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.Combobox_SelectChanged);

        }
        //----------------------------------------------------------------        
        private void Delete_Click(object sender, EventArgs e)
        {
            sCurrentArticle("", true);
        }
        //----------------------------------------------------------------
        private static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Name";

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
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
    //====================================================================================
    //====================================================================================
    //====================================================================================
    //====================================================================================

    [Serializable]
    public class TextAnalyser
    {
        //----------------------------------------------------------------
        static MatchCollection _s_M1;
        static public string _sTemp;
        public int _nCount = 0;
        public List<string> _list_sSingleWord = new List<string>();
        public List<int> _list_nSingleWordFirstIndex = new List<int>();
        public List<int> _list_nSingleWordLength = new List<int>();
        //----------------------------------------------------------------
        static public List<string> _list_sSourceSingleMark = new List<string>();
        //----------------------------------------------------------------
        public void AnalyzeSingleMarkForSource(string sAllText)
        {
            _list_sSourceSingleMark.Clear();
            for (int i = 0; i < _s_M1.Count; i++)
            {
                if (i == _s_M1.Count - 1)
                {
                    _list_sSourceSingleMark.Add("");
                    break;
                }
                string sTemp = "";
                int j = -1;
                while (true)
                {
                    j++;
                    int nIndex = _list_nSingleWordFirstIndex[i] + _list_nSingleWordLength[i] + j;

                    string temp = Convert.ToString(sAllText[nIndex]);
                    if ((temp == " " || temp == "\n") && j == 0)
                    {
                        _list_sSourceSingleMark.Add("");
                        break;
                    }
                    else if (temp == " " || temp == "\n")
                    {
                        _list_sSourceSingleMark.Add(sTemp);
                        break;
                    }
                    else
                    {
                        sTemp += temp;
                    }
                }
            }
        }
        //----------------------------------------------------------------
        public void Analysis(string sAllText, string sPattern)
        {
            _list_sSingleWord.Clear();
            _list_nSingleWordFirstIndex.Clear();
            _list_nSingleWordLength.Clear();

            _s_M1 = Regex.Matches(sAllText, sPattern);
            for (int i = 0; i < _s_M1.Count; i++)
            {
                _sTemp = _s_M1[i].ToString();
                _list_sSingleWord.Add(_sTemp);
                _list_nSingleWordFirstIndex.Add(_s_M1[i].Index);
                _list_nSingleWordLength.Add(_sTemp.Length);
            }
            _nCount = _s_M1.Count;
        }
        //----------------------------------------------------------------
        public string Merge()
        {
            _sTemp = "";
            for (int i = 0; i < _nCount; i++)
            {
                _sTemp += _list_sSingleWord[i] + _list_sSourceSingleMark[i] + " ";
            }
            return _sTemp;
        }
        //----------------------------------------------------------------
    }

    //====================================================================================
    //====================================================================================
    //====================================================================================
    //====================================================================================
    //====================================================================================

    partial class Form1
    {
        private void InitializeComponent2()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tbSyncInput = new System.Windows.Forms.RichTextBox();
            this.tbSource = new System.Windows.Forms.RichTextBox();
            this.btnChar3 = new System.Windows.Forms.Button();
            this.btnChar2 = new System.Windows.Forms.Button();
            this.btnChar1 = new System.Windows.Forms.Button();
            this.tbShowChar = new System.Windows.Forms.RichTextBox();
            this.tbInput = new System.Windows.Forms.RichTextBox();
            this.Btn_Stop = new System.Windows.Forms.Button();
            this.Btn_Pause = new System.Windows.Forms.Button();
            this.Btn_Save_and_Play = new System.Windows.Forms.Button();
            this.Btn_Record = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.CoverTextBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(588, 1);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 48;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.Combobox_SelectChanged);
            // 
            // tbSyncInput
            // 
            this.tbSyncInput.BackColor = System.Drawing.SystemColors.InfoText;
            this.tbSyncInput.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold);
            this.tbSyncInput.ForeColor = System.Drawing.SystemColors.Window;
            this.tbSyncInput.Location = new System.Drawing.Point(23, 23);
            this.tbSyncInput.Name = "tbSyncInput";
            this.tbSyncInput.ReadOnly = true;
            this.tbSyncInput.Size = new System.Drawing.Size(466, 332);
            this.tbSyncInput.TabIndex = 45;
            this.tbSyncInput.Text = "";
            this.tbSyncInput.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SyncInputMouseUp);
            // 
            // tbSource
            // 
            this.tbSource.BackColor = System.Drawing.SystemColors.InfoText;
            this.tbSource.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSource.ForeColor = System.Drawing.SystemColors.Window;
            this.tbSource.Location = new System.Drawing.Point(23, 369);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(454, 380);
            this.tbSource.TabIndex = 44;
            this.tbSource.Text = "";
            this.tbSource.TextChanged += new System.EventHandler(this.sourceChanged);
            // 
            // btnChar3
            // 
            this.btnChar3.Location = new System.Drawing.Point(885, 515);
            this.btnChar3.Name = "btnChar3";
            this.btnChar3.Size = new System.Drawing.Size(35, 18);
            this.btnChar3.TabIndex = 43;
            this.btnChar3.Text = "3";
            this.btnChar3.UseVisualStyleBackColor = true;
            this.btnChar3.Click += new System.EventHandler(this.btnShowCharClick);
            // 
            // btnChar2
            // 
            this.btnChar2.Location = new System.Drawing.Point(885, 488);
            this.btnChar2.Name = "btnChar2";
            this.btnChar2.Size = new System.Drawing.Size(35, 21);
            this.btnChar2.TabIndex = 42;
            this.btnChar2.Text = "2";
            this.btnChar2.UseVisualStyleBackColor = true;
            this.btnChar2.Click += new System.EventHandler(this.btnShowCharClick);
            // 
            // btnChar1
            // 
            this.btnChar1.Location = new System.Drawing.Point(885, 462);
            this.btnChar1.Name = "btnChar1";
            this.btnChar1.Size = new System.Drawing.Size(36, 20);
            this.btnChar1.TabIndex = 40;
            this.btnChar1.Text = "1";
            this.btnChar1.UseVisualStyleBackColor = true;
            this.btnChar1.Click += new System.EventHandler(this.btnShowCharClick);
            // 
            // tbShowChar
            // 
            this.tbShowChar.BackColor = System.Drawing.SystemColors.InfoText;
            this.tbShowChar.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.tbShowChar.ForeColor = System.Drawing.SystemColors.Window;
            this.tbShowChar.Location = new System.Drawing.Point(495, 419);
            this.tbShowChar.Name = "tbShowChar";
            this.tbShowChar.ReadOnly = true;
            this.tbShowChar.Size = new System.Drawing.Size(387, 330);
            this.tbShowChar.TabIndex = 38;
            this.tbShowChar.Text = "";
            this.tbShowChar.SelectionChanged += new System.EventHandler(this.showCharSelectionChanged);
            // 
            // tbInput
            // 
            this.tbInput.BackColor = System.Drawing.SystemColors.InfoText;
            this.tbInput.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold);
            this.tbInput.ForeColor = System.Drawing.SystemColors.Window;
            this.tbInput.Location = new System.Drawing.Point(495, 23);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(425, 332);
            this.tbInput.TabIndex = 37;
            this.tbInput.Text = "";
            this.tbInput.TextChanged += new System.EventHandler(this.inputChanged);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.Location = new System.Drawing.Point(742, 361);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(70, 52);
            this.Btn_Stop.TabIndex = 36;
            this.Btn_Stop.Text = "Stop Play/Record";
            this.Btn_Stop.UseVisualStyleBackColor = true;
            this.Btn_Stop.Click += new System.EventHandler(this.Btn_Stop_Click);
            // 
            // Btn_Pause
            // 
            this.Btn_Pause.Location = new System.Drawing.Point(667, 360);
            this.Btn_Pause.Name = "Btn_Pause";
            this.Btn_Pause.Size = new System.Drawing.Size(69, 52);
            this.Btn_Pause.TabIndex = 35;
            this.Btn_Pause.Text = "Pause";
            this.Btn_Pause.UseVisualStyleBackColor = true;
            this.Btn_Pause.Click += new System.EventHandler(this.Btn_Pause_Click);
            // 
            // Btn_Save_and_Play
            // 
            this.Btn_Save_and_Play.Location = new System.Drawing.Point(591, 360);
            this.Btn_Save_and_Play.Name = "Btn_Save_and_Play";
            this.Btn_Save_and_Play.Size = new System.Drawing.Size(70, 53);
            this.Btn_Save_and_Play.TabIndex = 34;
            this.Btn_Save_and_Play.Text = "Save/Play";
            this.Btn_Save_and_Play.UseVisualStyleBackColor = true;
            this.Btn_Save_and_Play.Click += new System.EventHandler(this.Btn_Save_And_Play_Click);
            // 
            // Btn_Record
            // 
            this.Btn_Record.Location = new System.Drawing.Point(504, 360);
            this.Btn_Record.Name = "Btn_Record";
            this.Btn_Record.Size = new System.Drawing.Size(81, 52);
            this.Btn_Record.TabIndex = 33;
            this.Btn_Record.Text = "Record";
            this.Btn_Record.UseVisualStyleBackColor = true;
            this.Btn_Record.Click += new System.EventHandler(this.Btn_Record_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(715, -3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 24);
            this.button1.TabIndex = 49;
            this.button1.Text = "New";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.New_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(831, -3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 24);
            this.button2.TabIndex = 50;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Delete_Click);
            // 
            // CoverTextBox
            // 
            this.CoverTextBox.BackColor = System.Drawing.Color.Black;
            this.CoverTextBox.Location = new System.Drawing.Point(12, -3);
            this.CoverTextBox.Multiline = true;
            this.CoverTextBox.Name = "CoverTextBox";
            this.CoverTextBox.Size = new System.Drawing.Size(486, 366);
            this.CoverTextBox.TabIndex = 51;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("PMingLiU", 7F);
            this.button3.Location = new System.Drawing.Point(615, 360);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(46, 21);
            this.button3.TabIndex = 52;
            this.button3.Text = "Loop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Btn_Loop_Play_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(933, 763);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.tbSyncInput);
            this.Controls.Add(this.tbSource);
            this.Controls.Add(this.btnChar3);
            this.Controls.Add(this.btnChar2);
            this.Controls.Add(this.btnChar1);
            this.Controls.Add(this.tbShowChar);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.Btn_Stop);
            this.Controls.Add(this.Btn_Pause);
            this.Controls.Add(this.Btn_Save_and_Play);
            this.Controls.Add(this.Btn_Record);
            this.Controls.Add(this.CoverTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RichTextBox tbSyncInput;
        private System.Windows.Forms.RichTextBox tbSource;
        private System.Windows.Forms.Button btnChar3;
        private System.Windows.Forms.Button btnChar2;
        private System.Windows.Forms.Button btnChar1;
        private System.Windows.Forms.RichTextBox tbShowChar;
        private System.Windows.Forms.RichTextBox tbInput;
        private System.Windows.Forms.Button Btn_Stop;
        private System.Windows.Forms.Button Btn_Pause;
        private System.Windows.Forms.Button Btn_Save_and_Play;
        private System.Windows.Forms.Button Btn_Record;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox CoverTextBox;
        private System.Windows.Forms.Button button3;
    }

    //====================================================================================
    //====================================================================================
    //====================================================================================
    //====================================================================================
    //====================================================================================
}
