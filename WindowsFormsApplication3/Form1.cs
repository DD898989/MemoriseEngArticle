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
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace WindowsFormsApplication3
{
    //====================================================================================
    public partial class Form1 : Form
    {
        //====================================================================================

        string ROOT_PATH_PARENT =
           Directory.GetParent(
           Directory.GetParent(
           Directory.GetParent(
           Directory.GetParent(
               System.Reflection.Assembly.GetEntryAssembly().Location
               ).FullName
               ).FullName
               ).FullName
               ).FullName
            +
            @"\";

        string ROOT_PATH =
           Directory.GetParent(
           Directory.GetParent(
           Directory.GetParent(
           Directory.GetParent(
               System.Reflection.Assembly.GetEntryAssembly().Location
               ).FullName
               ).FullName
               ).FullName
               ).FullName
            +
            @"\文章檔案\";
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
            File.WriteAllText(ROOT_PATH_PARENT + FILE_LOAD_TEMP, value);
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

                if (!File.Exists(ROOT_PATH + name + FILE_SOURCE)) //bIsDelete=true會進去
                {
                    if (過去.Count > 0)
                    {
                        name = 過去.Pop();
                    }
                    else if (未來.Count > 0)
                    {
                        name = 未來.Pop();
                    }
                    else
                    {
                        foreach (var file in dir.GetFiles("*" + FILE_SOURCE))
                        {
                            name = file.Name.Replace(FILE_SOURCE, "");
                            break;
                        }
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

                File.WriteAllText(ROOT_PATH_PARENT + FILE_LOAD_TEMP, _sName);

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

        public static string CurrentFolder = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + @"\";
        public static string path_有音標處理過不用重新parse = CurrentFolder + @"字典資料\有音標處理過不用重新parse.txt";
        //path_國中1200
        public static string path_國中1200 = CurrentFolder + @"字典資料\國中1200單字.txt";

        //重要全部8800單字
        public static string path_重要全部8800單字 = CurrentFolder + @"字典資料\重要全部8800單字.txt";

        public static Dictionary<string, string> hash總字典 = new Dictionary<string, string>();
        public static Dictionary<string, 不規則> 不規則_正常是key = new Dictionary<string, 不規則>();
        public static Dictionary<string, 不規則> 不規則_其他是key = new Dictionary<string, 不規則>();

        public static string path_不規則 = CurrentFolder + @"字典資料\不規則.txt";

        public class 不規則
        {
            public string 正常;
            public List<string> 過去s;
            public List<string> 過分s;
            public List<string> 複數s;
            public 不規則(string 正常, List<string> 過去s, List<string> 過分s, List<string> 複數s)
            {
                this.正常 = 正常;
                this.過去s = 過去s;
                this.過分s = 過分s;
                this.複數s = 複數s;
            }


            public override string ToString()
            {
                var rtn = "正常:  " + 正常 + Environment.NewLine;

                if (過去s != null)
                {
                    foreach (var i in 過去s)
                    {
                        rtn += ("過去:  " + i + Environment.NewLine);
                    }
                }

                if (過分s != null)
                {
                    foreach (var i in 過分s)
                    {
                        rtn += ("過分:  " + i + Environment.NewLine);
                    }
                }

                if (複數s != null)
                {
                    foreach (var i in 複數s)
                    {
                        rtn += ("複數:  " + i + Environment.NewLine);
                    }
                }
                return
                    rtn;
            }

        }
        public Form1()
        {
            InitializeComponent();
            if (!File.Exists(ROOT_PATH_PARENT + FILE_LOAD_TEMP))
            {
                File.WriteAllText(ROOT_PATH_PARENT + FILE_LOAD_TEMP, "");
            }
            else
            {
                sCurrentArticle(File.ReadAllText(ROOT_PATH_PARENT + FILE_LOAD_TEMP));
            }



            {

                var list_不規則 = new List<string>(File.ReadAllLines(path_不規則));
                foreach (var item in list_不規則)
                {
                    var item2 = item;
                    item2 = item2.Replace("【動詞】", "");
                    item2 = item2.Replace("【名詞】", "");
                    item2 = item2.Replace("【過去式】", "拉");
                    item2 = item2.Replace("【過去分詞】", "拉");
                    item2 = item2.Replace("【複數】", "拉");

                    var item3 = item2.Split('拉');
                    if (item3.Length == 3)
                    {
                        var item4 = item3[0];
                        var item5 = item3[1].Split(',').ToList();
                        var item6 = item3[2].Split(',').ToList();
                        不規則 test = new 不規則(item4, item5, item6, null);
                        if (不規則_正常是key.ContainsKey(item4))
                        {
                            throw new Exception("GGGGGG");
                        }
                        else
                        {
                            不規則_正常是key.Add(item4, test);
                        }

                        foreach (var i in item5)
                        {
                            if (不規則_其他是key.ContainsKey(i))
                            {
                                throw new Exception("GGGGGG");
                            }
                        }
                        foreach (var i in item6)
                        {
                            if (不規則_其他是key.ContainsKey(i))
                            {
                                throw new Exception("GGGGGG");
                            }
                        }
                        foreach (var i in item5)
                        {
                            不規則_其他是key[i] = test;
                        }
                        foreach (var i in item6)
                        {
                            不規則_其他是key[i] = test;
                        }

                    }
                    else
                    {
                        var item4 = item3[0];
                        var item5 = item3[1].Split(',').ToList();
                        不規則 test = new 不規則(item4, null, null, item5);
                        if (不規則_正常是key.ContainsKey(item4))
                        {
                            throw new Exception("GGGGGG");
                        }
                        else
                        {
                            不規則_正常是key.Add(item4, test);
                        }
                        foreach (var i in item5)
                        {
                            if (不規則_其他是key.ContainsKey(i))
                            {
                                throw new Exception("GGGGGG");
                            }
                        }

                        foreach (var i in item5)
                        {
                            不規則_其他是key[i] = test;
                        }
                    }
                }


            }

            {
                var list_有音標處理過 = new List<string>(File.ReadAllLines(path_國中1200));
                foreach (var aa in list_有音標處理過)
                {
                    var sdf = aa.Split('\t').ToList<string>();
                    國中單字們.Add((sdf[0], sdf[1]));
                }
            }

            
            {
                var list_有音標處理過 = new List<string>(File.ReadAllLines(path_重要全部8800單字));
                var asfdadf = new HashSet<string>();
                foreach (var aa in list_有音標處理過)
                {
                    MatchCollection M1 = Regex.Matches(aa, @"^([A-Za-z]+)@\(([a-z]+)\)(.+)\t等級(\d)$");

                    
                    if(M1.Count != 1)
                    {
                        asfdadf.Add(aa);
                    }
                    else
                    {
                        GroupCollection groups = M1[0].Groups;
                        var 單字1 = groups[1].Value;
                        var 磁性1 = (WordType)Enum.Parse(typeof(WordType), groups[2].Value); 
                        var 解釋1 = groups[3].Value;
                        int 等級1 = int.Parse( groups[4].Value);
                        八千單字.Add((單字1, 磁性1), new Tuple<string , int >(解釋1, 等級1));

                    }

                }
            }


            {

                var list_有音標處理過 = new List<string>(File.ReadAllLines(path_有音標處理過不用重新parse));


                ENGtoCHinfo etc = null;


                for (int i = 0; i < list_有音標處理過.Count; i++)
                {
                    if (i % 15000 == 0)
                    {
                        Console.WriteLine(((double)i / (double)list_有音標處理過.Count * (double)100) + "%");
                    }

                    if (i % 2 == 0)
                    {
                        List<string> item = list_有音標處理過[i].Split('\t').ToList<string>();

                        etc = new ENGtoCHinfo(item[0], item[1]);
                        etc.音標搜尋用 = item[2];
                    }
                    else
                    {
                        List<string> item = list_有音標處理過[i].Split('\t').ToList<string>();
                        string 詞性 = "";
                        for (int j = 0; j < item.Count; j++)
                        {
                            if (j % 2 == 0)
                            {
                                詞性 = item[j];
                            }
                            else
                            {
                                var 中文 = item[j];

                                etc.list解釋.Add(new chInfo(中文, (WordType)Enum.Parse(typeof(WordType), 詞性)));
                            }
                        }
                        list總字典.Add(etc);
                    }

                }
            }


            foreach (var item in list總字典)
            {
                if (item.音標.Length > 0)
                {
                    if (!hash總字典.ContainsKey(item.單字))
                    {
                        hash總字典.Add(item.單字, item.音標);
                    }
                }
            }



            var fsefs = new HashSet<WordType>();
            foreach (var asd in 八千單字)
            {
                fsefs.Add(asd.Key.磁性);
            }


            foreach (var x in list總字典)
            {

                foreach(var y in x.list解釋)
                {
                    foreach(var 暫時 in fsefs)
                    {
                        if (
                            八千單字.TryGetValue
                                (
                                   (x.單字, 暫時)
                                        ,
                                   out Tuple<string, int> tuple
                                )
                            )
                        {
                            var 解釋2 = tuple.Item1;
                            var 等級2 = tuple.Item2;

                            x.八千_有嗎 = true;
                            x.八千_通用解釋_累加 += (解釋2 + " ");
                            x.八千_等級 = 等級2;
                        }
                    }
                }
            }



            //var asdf = new Dictionary<string, string>();
            //var asdf2 = new Dictionary<string, string>();
            //foreach (var asd in 八千單字)
            //{
            //    asdf[asd.Key.單字] = "拉拉拉";
            //}

            //foreach (var x in list總字典)
            //{
            //    if (asdf.ContainsKey(x.單字))
            //    {
            //        if (!x.八千_有嗎)
            //        {
            //            asdf2.Add(x.單字, "顆顆");
            //        }
            //    }
            //}



            button1_Click_字典(this, null);
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
        public static DateTime caps = DateTime.Now;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.CapsLock)
            {
                var old = caps;
                caps = DateTime.Now;
                if (DateTime.Now - old < TimeSpan.FromMilliseconds(300))
                {
                    button1_Click_字典(this, null);
                }

            }
            else if (keyData == (Keys.Alt | Keys.Right))
            {
                if (rtb_0_Input.Visible)
                {
                    button1_Click_下篇(this, null);
                }
                else if (考單字_tb_目標單字.Visible)
                {
                    button1_Click_下篇2(this, null);
                }
            }
            else if (keyData == (Keys.Alt | Keys.Left))
            {
                if (rtb_0_Input.Visible)
                {
                    button1_Click_上篇(this, null);
                }
            }
            else if (keyData == (Keys.Control | Keys.T))
            {
                this.TopMost = !this.TopMost;
            }
            else if (keyData == Keys.Escape)
            {
                this.Close();
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            return true;
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

            List<string> excludes1 = new List<string>()
            {
                "this",
                "that",
                "the",
                "they",
            };

            List<char> excludes2 = new List<char>()
            {
                //下面會出問題
                //',',
                //'.',
                //'\'',
                //'-'',
            };

            for (int k = 0; k < _taSource.count; k++)
            {
                int len = 1;
                if (_taSource.WordLen(k) >= Len)
                    len = Len;


                if (excludes1.Contains(_taSource.Word(k).ToLower()))
                {
                    merge += _taSource.Word(k);
                }
                else
                {
                    var output = "";
                    output += _taSource.Word(k).Substring(0, len);
                    output += new String('_', _taSource.WordLen(k) - len);

                    foreach (var ex in excludes2)
                    {
                        if (_taSource.Word(k).Contains(ex))
                        {
                            var idx = _taSource.Word(k).IndexOf(ex);
                            output = new System.Text.StringBuilder(output).Remove(idx, 1).Insert(idx, ex).ToString();
                        }
                    }

                    merge += output;
                }

                merge += TextAnalyser.s_lsSourceMark[k];
                if (k != _taSource.count - 1)
                    merge += " ";
            }

            rtb_0_ShowChar.Text = merge;
            OriginDashString = merge;
            _taShowChar = new TextAnalyser(rtb_0_ShowChar.Text, @"([a-zA-Z0-9_]+)"); //for erase unnecessary space and newline in source
        }
        public static string OriginDashString = "";
        //====================================================================================
        private void rtb_0_ShowChar_MouseUp(object sender, EventArgs e)
        {
            string sLine = rtb_0_ShowChar.Text.Substring(0, rtb_0_ShowChar.SelectionStart);
            int spaceNum = sLine.Split(' ').Length - 1;
            int idx = _taSource.WordIdx(spaceNum);
            HighlightText(rtb_0_Source, idx, 1, Color.Red, true);
            顯示音標(rtb_0_Source.Text, idx);
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
            string lastTwo = "";
            try
            {
                lastTwo = rtb_0_Input.Text.Substring(rtb_0_Input.Text.Length - 2, 2);
            }
            catch
            {

            }

            bool 有正規 = sLastInputRegex.Contains(".");


            bool FALSE = false;
            bool 這個字結束了 = false;
            if (
                lastTwo == (sLastSource[0].ToString() + " ")
                ||
                lastTwo == ("; ")
                )
            {
                這個字結束了 = true;
            }

            if (
                lastTwo == ("; ")
                )
            {
                有正規 = false;
            }



            //開始
            if (sLastInput == sLastSource[0].ToString())
            {

                if (FALSE)
                    rtb_0_Input.BackColor = Color.Black;
            }
            //最後
            else if (sLastSource == Regex.Match(sLastSource, sLastInputRegex/*   */, RegexOptions.RightToLeft).ToString())
            {
                if (sLastCharInTB == ' ')
                {
                    這個字結束了 = true;
                }
                if (FALSE)
                {
                    if (sLastCharInTB == ' ')
                        rtb_0_Input.BackColor = Color.Black;
                    else
                        rtb_0_Input.BackColor = Color.Brown;
                }
            }
            //其他
            else if (sLastSource == Regex.Match(sLastSource, sLastInputRegex + ".+", RegexOptions.RightToLeft).ToString() && sLastCharInTB != ' ')
            {
                if (FALSE)
                    rtb_0_Input.BackColor = Color.Brown;
            }
            //錯誤
            else
            {
                this.rtb_0_Input.TextChanged -= this.rtb_0_Input_TextChanged;
                try//for start the process
                {
                    this.BackColor = Color.Red;
                    Application.DoEvents();
                    SendKeys.Send("{BACKSPACE}");
                    this.BackColor = Color.Black;
                }
                catch
                {

                }
                this.rtb_0_Input.TextChanged += this.rtb_0_Input_TextChanged;
            }



            int idx;
            int len;

            idx = _taSource.WordIdx(_taInput.count - 1);
            len = _taSource.WordLen(_taInput.count - 1);
            HighlightText(rtb_0_Source, idx, len, Color.Red, true);

            if (!這個字結束了)
            {
                顯示音標(rtb_0_Source.Text, idx, false);
            }
            else
            {
                try
                {
                    顯示音標(rtb_0_Source.Text, _taSource.WordIdx(_taInput.count), false);
                }
                catch
                {

                }
            }


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

            if (!有正規)
            {
                try
                {
                    var text = "";

                    if (這個字結束了)
                    {

                        text =
                            OriginDashString.Substring(0, idx) +
                            sLastSource.Substring(0, sLastSource.Length) +
                            OriginDashString.Substring(idx + len, OriginDashString.Length - idx - len);

                    }
                    else
                    {
                        text =
                            OriginDashString.Substring(0, idx) +
                            sLastSource.Substring(0, sLastInput.Length) +
                            string.Concat(Enumerable.Repeat("_", len - sLastInput.Length)) +
                            OriginDashString.Substring(idx + len, OriginDashString.Length - idx - len);

                    }

                    rtb_0_ShowChar.Text = text;
                }
                catch { }
            }

            HighlightText(rtb_0_ShowChar, idx, len, Color.Red, true, Color.Gray);
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
        private void HighlightText(RichTextBox rtb, int nIndex, int nLen, Color color, bool bResetColor, Color? resetColor = null)
        {
            resetColor = resetColor ?? Color.White;

            if (bResetColor)
            {
                rtb.Select(0, rtb.TextLength);
                rtb.SelectionColor = (Color)resetColor;
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

        Stack<string> 過去 = new Stack<string>();
        Stack<string> 未來 = new Stack<string>();
        private void button1_Click_上篇(object sender, EventArgs e)
        {

            if (過去.Count > 0)
            {
                未來.Push(cbx_2_File.SelectedItem.ToString());
                cbx_2_File.SelectedItem = 過去.Pop();
            }


            考句子聚焦();
        }

        private void button1_Click_下篇(object sender, EventArgs e)
        {
            過去.Push(cbx_2_File.SelectedItem.ToString());

            if (未來.Count > 0)
            {
                cbx_2_File.SelectedItem = 未來.Pop();
            }
            else
            {
                cbx_2_File.SelectedIndex = new Random().Next(cbx_2_File.Items.Count);
            }

            考句子聚焦();
        }


        public void 清空()
        {
            現在目標2 = null;
            解釋拉 = null;
            考單字_lb_一樣的.Items.Clear();
        }

        public void 給值(ENGtoCHinfo 目標2,string 解釋, List<ENGtoCHinfo> 一樣念法)
        {
            現在目標2 = 目標2;
            解釋拉 = 解釋;
            if (false)
            {
                考單字_tb_目標解釋.Text = 解釋拉;
            }
            else
            {
                考單字_tb_目標解釋.Text = "";
            }

            foreach(var i in 一樣念法)
            {
                if (i.八千_有嗎)
                {
                    考單字_lb_一樣的.Items.Add(i.單字+" ~"+i.八千_等級);
                }
                else
                {
                    考單字_lb_一樣的.Items.Add(i.單字);
                }
            }


            is填單字 = (new Random().NextDouble() >= 0.0);


            this.考單字_tb_目標單字.BackColor = Color.Black;
            this.考單字_tb_目標音標.BackColor = Color.Black;

            考單字_tb_目標單字.TextChanged -= 考單字_tb_目標單字_TextChanged;
            考單字_tb_目標音標.TextChanged -= 考單字_tb_目標音標_TextChanged;

            if (!is填單字)
            {
                this.考單字_tb_目標單字.Text = 目標2.單字;
                this.考單字_tb_目標音標.Text = "";
                this.考單字_tb_目標音標.Select();
            }
            else
            {
                this.考單字_tb_目標音標.Text = 目標2.音標;
                this.考單字_tb_目標單字.Text = "";
                this.考單字_tb_目標單字.Select();
            }

            考單字_tb_目標單字.TextChanged += 考單字_tb_目標單字_TextChanged;
            考單字_tb_目標音標.TextChanged += 考單字_tb_目標音標_TextChanged;


        }

        public static ENGtoCHinfo 現在目標2 = null;
        public string 解釋拉 = null;
        public bool is填單字 = false;
        private void button1_Click_下篇2(object sender, EventArgs e)
        {
            清空();

            {
                int randomNumber = new Random().Next(國中單字們.Count);
                var 目標1 = 國中單字們[randomNumber];
                string 單字 = 目標1.a;
                string 解釋 = 目標1.b;
                try
                {
                    ENGtoCHinfo 目標2 = list總字典.Where(a => a.單字 == 單字 && a.音標.Length > 0).First();
                    var 一樣念法 = list總字典.Where(a => a.單字 != 單字 && a.音標 == 目標2.音標).ToList();

                    給值(目標2, 解釋, 一樣念法);
                }
                catch
                {
                }
                finally
                {
                    國中單字們.RemoveAt(randomNumber);
                }

            }


        }


        public void 顯示音標(string allArticel, int cursorPosition, bool showWord = true)
        {

            allArticel = " " + allArticel + " ";//讓開頭跟結尾是 !Char.IsLetterOrDigit
            var list = new List<int> { };
            for (int i = 0; i < allArticel.Length; i++)
            {
                if (!Char.IsLetterOrDigit(allArticel[i]))
                {
                    list.Add(i);
                }
            }

            try
            {
                var jj = 0;
                foreach (var ii in list)
                {
                    if (ii > cursorPosition)
                    {
                        int length = (ii - 1) - (jj);
                        var target原來可能的大寫 = allArticel.Substring(jj + 1, length);
                        var target = target原來可能的大寫.ToLower();

                        var target2 = "";

                        if (!hash總字典.ContainsKey(target))
                        {
                            {
                                if (hash總字典.ContainsKey(target原來可能的大寫))
                                {
                                    target2 = target原來可能的大寫;
                                    goto THISOK;
                                }
                            }


                            {
                                if (不規則_其他是key.ContainsKey(target))
                                {
                                    target2 = 不規則_其他是key[target].正常;
                                    goto THISOK;
                                }
                            }



                            {
                                target2 = RemoveFromEnd(target, "ing", out bool has) + "e";
                                if (has && hash總字典.ContainsKey(target2))
                                {
                                    goto THISOK;
                                }
                            }

                            {
                                target2 = RemoveFromEnd(target, "ing", out bool has);

                                if (has)
                                {
                                    if (hash總字典.ContainsKey(target2))
                                    {
                                        goto THISOK;
                                    }
                                    else if (target2.Length >= 2 && target2[target2.Length - 1] == target2[target2.Length - 2])
                                    {
                                        target2 = target2.Remove(target2.Length - 1);
                                        if (has && hash總字典.ContainsKey(target2))
                                        {
                                            goto THISOK;
                                        }
                                    }
                                }
                            }


                            {
                                target2 = RemoveFromEnd(target, "ed", out bool has);
                                if (has && target2.Length >= 2 && target2[target2.Length - 1] == target2[target2.Length - 2])
                                {
                                    target2 = target2.Remove(target2.Length - 1);
                                    if (has && hash總字典.ContainsKey(target2))
                                    {
                                        goto THISOK;
                                    }
                                }
                            }

                            {
                                target2 = RemoveFromEnd(target, "ied", out bool has) + "y";
                                if (has && hash總字典.ContainsKey(target2))
                                {
                                    goto THISOK;
                                }
                            }




                            {
                                target2 = RemoveFromEnd(target, "ies", out bool has) + "y";
                                if (has && hash總字典.ContainsKey(target2))
                                {
                                    goto THISOK;
                                }
                            }


                            {
                                target2 = RemoveFromEnd(target, "ed", out bool has) + "e";
                                if (has && hash總字典.ContainsKey(target2))
                                {
                                    goto THISOK;
                                }
                            }

                            {
                                target2 = RemoveFromEnd(target, "ed", out bool has);
                                if (has && hash總字典.ContainsKey(target2))
                                {
                                    goto THISOK;
                                }
                            }



                            //先去s
                            {
                                target2 = RemoveFromEnd(target, "s", out bool has);
                                if (has && hash總字典.ContainsKey(target2))
                                {
                                    goto THISOK;
                                }
                            }


                            //再去es
                            {
                                target2 = RemoveFromEnd(target, "es", out bool has);
                                if (has && hash總字典.ContainsKey(target2))
                                {
                                    goto THISOK;
                                }
                            }


                            goto ALSOOK;
                        }
                        else
                        {
                            goto ALSOOK;
                        }

                    THISOK:
                        target = target2;
                    ALSOOK:

                        if (hash總字典.ContainsKey(target))
                        {
                            真實 = target;
                            if (showWord)
                            {
                                textBox1.Text = target;
                            }
                            else
                            {
                                textBox1.Text = "";
                            }
                            textBox2.Text = hash總字典[target];
                        }
                        else
                        {
                            textBox1.Text = "";
                            textBox2.Text = "";
                        }




                        break;
                    }
                    jj = ii;
                }
            }
            catch
            {

            }
        }

        public string 真實 = "";

        private void TextBox3_MouseClick(object sender, MouseEventArgs e)
        {
            var textbox = (RichTextBox)sender;
            int cursorPosition = textbox.SelectionStart;
            string allArticel = textbox.Text;

            顯示音標(allArticel, cursorPosition);
        }

        public static string RemoveFromEnd(string s, string suffix, out bool has)
        {
            if (s.EndsWith(suffix))
            {
                has = true;
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                has = false;
                return s;
            }
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_字典(object sender, EventArgs e)
        {
            if (!this.rtb_0_Input.Visible && !this.考單字_tb_目標單字.Visible)
            {
                foreach (Control c in this.Controls)
                {
                    if (c.Name.Contains("查單字"))
                    {
                        c.Visible = false;
                    }
                    else
                    {
                        if (c.Name.Contains("考單字"))
                        {
                            if (checkBox1.Checked)
                            {
                                c.Visible = true;
                            }
                            else
                            {
                                c.Visible = false;
                            }
                        }
                        else
                        {
                            if (checkBox1.Checked)
                            {
                                c.Visible = false;
                            }
                            else
                            {
                                c.Visible = true;
                            }
                        }
                    }
                }

                if (!checkBox1.Checked)
                {
                    考句子聚焦();
                }
                else
                {
                    考單字聚焦();
                }
            }
            else
            {
                foreach (Control c in this.Controls)
                {
                    if (c.Name.Contains("查單字"))
                    {
                        c.Visible = true;
                    }
                    else
                    {
                        c.Visible = false;
                    }
                }


                if (!checkBox1.Checked)
                {
                    if (真實.Length > 0)
                    {
                        查單字_tb_輸入英文.Text = 真實;
                    }
                    else
                    {
                        查單字_tb_輸入英文.Text = "";
                    }
                }
                else
                {
                    if (現在目標2?.音標?.Length > 0)
                    {
                        查單字_tb_輸入英文.Text = 現在目標2.單字;
                    }
                    else
                    {
                        查單字_tb_輸入英文.Text = "";
                    }
                }


                查單字_tb_輸入英文.Select();
                查單字_tb_輸入英文.SelectionStart = 0;
                查單字_tb_輸入英文.SelectionLength = 查單字_tb_輸入英文.Text.Length;
            }

            btn_mode.Visible = true;
            checkBox1.Visible = true;
        }

        public void 考句子聚焦()
        {
            rtb_0_Input.Select();
            rtb_0_Input.SelectionStart = rtb_0_Input.Text.Length;
            rtb_0_Input.SelectionLength = 0;
        }

        public void 考單字聚焦()
        {
            if (!is填單字)
            {
                考單字_tb_目標音標.Select();
                考單字_tb_目標音標.SelectionStart = 考單字_tb_目標音標.Text.Length;
                考單字_tb_目標音標.SelectionLength = 0;
            }
            else
            {
                考單字_tb_目標單字.Select();
                考單字_tb_目標單字.SelectionStart = 考單字_tb_目標單字.Text.Length;
                考單字_tb_目標單字.SelectionLength = 0;
            }
        }

        public static List<ENGtoCHinfo> list總字典 = new List<ENGtoCHinfo>();

        public static List<(string a, string b)> 國中單字們 = new List<(string a, string b)>();
        
        public static Dictionary<(string 單字, WordType 磁性),
            Tuple<string , int >
            > 
            八千單字 = new Dictionary<(string 單字, WordType 磁性),
            Tuple<string , int >
                >();


        public enum WordType//建議照字數多寡排列 ex: comb>adj>vi>n
        {
            auxil,
            comb,
            conj,
            intj,//can't use "int" because it's a C# keyword
            prep,
            pref,
            pred,
            pron,
            adj,
            adv,
            art,
            aux,
            vbl,
            suf,
            xx, //沒有類形
            vi,
            vt,
            pl,
            n,
            a,
            v,
        }

        public struct chInfo
        {
            public string 中文;
            public WordType 詞性;

            public chInfo(string 中文, WordType 詞性)
            {
                this.中文 = 中文;
                this.詞性 = 詞性;
            }
            public override string ToString()
            {
                return 詞性 + "  " + 中文;
            }
        }

        public class ENGtoCHinfo
        {
            public string 單字;
            public string 音標;
            public string 音標搜尋用;
            public List<chInfo> list解釋 = new List<chInfo>();


            public bool 八千_有嗎 = false;
            public string 八千_通用解釋_累加 = "";
            public int 八千_等級 = -1;


            public static List<(string a, string b)> 音標對應們2 = new List<(string a, string b)>
            {
            //下面取代要照順序
            ("ɑɪ", "哀"),
            ("ɑʊ", "凹"),
            ("dʒ", "舉"),//"+"是regex保留字無法用
            ("ʒ", "舉"),//"+"是regex保留字無法用
               ("tʃ", "區"),
               ("ɔɪ", "唷"),
               ("ʌ", "阿"),//"^"是regex保留字無法用 
               ("﹐", ""),
               ("ˋ", ""),
               (":", ":"),
               ("d", "d"),
               ("r", "r"),
               ("i", "i"),
               ("n", "n"),
               ("o", "o"),
               ("k", "k"),
               ("g", "g"),
               ("s", "s"),
               ("z", "z"),
               ("w", "w"),
               ("j", "j"),
               ("t", "t"),
               ("m", "m"),
               ("l", "l"),
               ("p", "p"),
               ("e", "e"),
               ("b", "b"),
               ("h", "h"),
               ("f", "f"),
               ("u", "u"),
               ("v", "v"),
               ("!", "!"),
               ("ɛ", "3"),
               ("θ", "4"),
               ("ə", "痾"),
               ("æ", "@"),
               ("ʊ", "5"),
               ("ɔ", "喔"),
               ("ɑ", "a"),
               ("ɪ", "1"),
               ("ʃ", "需"),
               ("ɚ", "2"),
               ("ɝ", "2"),
               ("ŋ", "7"),
               ("ð", "6"),
               ("ṇ", "ㄥ")
        };


            public ENGtoCHinfo(string en, string kk, string kk搜尋用)
            {
                this.單字 = en;
                this.音標 = kk;
                this.音標搜尋用 = kk搜尋用;
            }

            public ENGtoCHinfo(string en, string kk)
            {
                this.單字 = en;
                this.音標 = kk;
                string temp1 = kk;
                string temp2 = kk;

                if (kk.Length > 0)
                {
                    foreach (var temp in 音標對應們2)
                    {
                        temp1 = temp1.Replace(temp.a, temp.b);

                        temp2 = temp2.Replace(temp.a, "");
                    }
                }

                if (temp2 != "")
                    throw new Exception("音標無法解析");
                else
                    this.音標搜尋用 = temp1;
            }

            public override string ToString()
            {
                if (this.八千_有嗎)
                {
                    return 八千_等級+"~~ " +單字 + " [" + 音標 + "]";
                }
                else
                {
                    return "     "+單字 + " [" + 音標 + "]";
                }
                
            }
        }

        private void 查單字_tb_輸入英文_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text.EndsWith(" "))
            {
                tb.Text = tb.Text.Remove(tb.Text.Length - 1);
                tb.SelectionStart = tb.Text.Length;
                tb.SelectionLength = 0;
            }

            if (查單字_checkBox2.Checked)
            {

                Loop(
                    sender,
                    list總字典.Where(x => x.八千_有嗎 == true).ToList(),
                    x => ((ENGtoCHinfo)x).單字,
                    this.查單字_lb_顯示英文
                    );
            }
            else
            {

            Loop(
                sender,
                list總字典,
                x => ((ENGtoCHinfo)x).單字,
                this.查單字_lb_顯示英文
                );
            }

        }
        //====================================================================================

        async void Loop<T>(object sender, IEnumerable<T> list, Func<T, string> member, ListBox listBoxUpdate)
        {
            string text = ((TextBox)sender).Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            List<object> keyList = new List<object>();


            await Task.Run(() =>
            {
                Thread.Sleep(800);

                if (text != ((TextBox)sender).Text)
                {
                    return;
                }
                else
                {
                    foreach (var i in list)
                    {
                        try
                        {
                            var test = member(i);
                            var Match = Regex.Match(member(i), text, RegexOptions.IgnoreCase);
                            if (Match.Success)
                            {
                                if (keyList.Count > 100)
                                {
                                    break;
                                }
                                keyList.Add(i);

                                var index = Match.Index;//這要怎麼加到list
                            }
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
            });

            UpdateListBox(listBoxUpdate, keyList);
        }

        private void UpdateListBox(ListBox listBox, List<object> objs, bool selectFirst = true)
        {
            listBox.Items.Clear();

            listBox.Items.AddRange(objs.ToArray());

            if (selectFirst && objs.Count > 0)
                listBox.SelectedIndex = 0;
        }

        private void 查單字_lb_顯示英文_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var temp = ((ENGtoCHinfo)((ListBox)sender).SelectedItem);
                查單字_tb_顯示音標.Text = (@"^" + temp.音標搜尋用 + @"$");
                List<object> objectList = temp.list解釋.Cast<object>().ToList();

                if (不規則_正常是key.ContainsKey(temp.單字))
                {
                    查單字_tb_顯示變化.Text = 不規則_正常是key[temp.單字].ToString();
                }
                else if (不規則_其他是key.ContainsKey(temp.單字))
                {
                    查單字_tb_顯示變化.Text = 不規則_其他是key[temp.單字].ToString();
                }
                else
                {
                    查單字_tb_顯示變化.Text = "";
                }



                UpdateListBox(查單字_lb_顯示中文, objectList, false);
            }
            catch
            {
            }
        }



        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void 查單字_tb_輸入音標_TextChanged(object sender, EventArgs e)
        {
            if (查單字_checkBox2.Checked)
            {
                Loop(
                    sender,
                    list總字典.Where(x=>x.八千_有嗎==true).ToList(),
                    x => ((ENGtoCHinfo)x).音標搜尋用,
                    this.查單字_lb_顯示英文
                    );
            }
            else
            {
                Loop(
                    sender,
                    list總字典,
                    x => ((ENGtoCHinfo)x).音標搜尋用,
                    this.查單字_lb_顯示英文
                    );
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void 查單字_test_button1_Click(object sender, EventArgs e)
        {
            var sdfs = new Dictionary<string,
                Dictionary<char,
                List<ENGtoCHinfo
                >>>();

            foreach (var i in list總字典)
            {
                if (i.音標搜尋用.Length == 3)
                {
                    string key1 = i.音標搜尋用[0].ToString() + i.音標搜尋用[2].ToString();
                    if (!sdfs.ContainsKey(key1))
                    {
                        sdfs.Add(key1, new Dictionary<char, List<ENGtoCHinfo>>());
                    }

                    var key2 = i.音標搜尋用[1];
                    if (!sdfs[key1].ContainsKey(key2))
                    {
                        sdfs[key1].Add(key2, new List<ENGtoCHinfo>());
                    }

                    sdfs[key1][key2].Add(i);
                }
            }

            var sdfsd = "";
            foreach (var i in sdfs)
            {
                foreach (var j in i.Value)
                {
                    foreach (var k in j.Value)
                    {
                        sdfsd += i.Key; sdfsd += "\t";
                        sdfsd += j.Key; sdfsd += "\t";
                        sdfsd += k.單字; sdfsd += "\t";
                        sdfsd += k.音標; sdfsd += "\t";
                        sdfsd += k.音標搜尋用; sdfsd += "\t";
                        sdfsd += string.Join(",", k.list解釋); sdfsd += "\t";
                        sdfsd += Environment.NewLine;
                    }
                }
            }

            try
            {
                File.Delete("拉拉拉.txt");
            }
            catch { }

            File.WriteAllText("拉拉拉.txt", sdfsd, System.Text.Encoding.UTF8);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void 考單字_tb_目標單字_TextChanged(object sender, EventArgs e)
        {

            改題目(sender, true);
        }

        private void 考單字_tb_目標音標_TextChanged(object sender, EventArgs e)
        {
            改題目(sender, false);
        }

        public void 改題目(object sender, bool 是考單字)
        {
            this.考單字_tb_目標單字.BackColor = Color.Black;
            this.考單字_tb_目標音標.BackColor = Color.Black;
            var tb = (TextBox)sender;
            string 填答 = tb.Text;


            if (現在目標2 == null || 解釋拉 == null || 解釋拉 == "")
            {
                tb.BackColor = Color.DarkGray;
                return;
            }


            if (是考單字)
            {
                if (填答 == 現在目標2.單字 + " ")
                {
                    button1_Click_下篇2(this, null);
                    tb.BackColor = Color.Black;
                }
                else if (填答 == 現在目標2.單字)
                {
                    this.考單字_tb_目標解釋.Text = 解釋拉;
                    tb.BackColor = Color.Green;
                }
                else
                {
                    tb.BackColor = Color.Black;
                }
            }
            else
            {
                if (填答 == 現在目標2.音標搜尋用 + " ")
                {
                    button1_Click_下篇2(this, null);
                    tb.BackColor = Color.Black;
                }
                else if (填答 == 現在目標2.音標搜尋用)
                {
                    this.考單字_tb_目標解釋.Text = 解釋拉;
                    tb.BackColor = Color.Green;
                }
                else
                {
                    tb.BackColor = Color.Black;
                }
            }
        }
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


    //====================================================================================
}
