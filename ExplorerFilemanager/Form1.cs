using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ExplorerFilemanager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        DirectoryInfo di; FileInfo[] fArray;
        private void Form1_Load(object sender, EventArgs e)
        {
            listFolders(); listFiles();
            comboBox1.DataSource = new List<string> { "move to" };
            textBox2.Text = @"G:\!!!!!@@分類檔案●@@!!!!!";
            //textBox3.Text = "篩選！開頭";
            # region 測試用
            //textBox2.Text = @"X:\temp";
            //textBox1.Text = @"X:\temp\新增資料夾";
            #endregion
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            listFolders();
        }

        void listFiles()
        {
            string fdrPath = textBox1.Text;
            if (!Directory.Exists(fdrPath)) return;
            di = new DirectoryInfo(fdrPath);
            fArray = di.GetFiles();
            listBox1.DataSource = fArray;
        }
        void listFolders()
        {
            string fdrPath = textBox2.Text;
            if (!Directory.Exists(fdrPath)) return;
            di = new DirectoryInfo(fdrPath);
            //            listBox2.DataSource = di.GetDirectories();            
            List<DirectoryInfo> dList = new List<DirectoryInfo>();
            foreach (DirectoryInfo item in di.GetDirectories())
            {
                if (item.Name.IndexOf("System Volume Information") == -1)
                    dList.Add(item);
            }
            listBox2.DataSource = dList;//.Sort();

            /*
            //https://dotblogs.com.tw/marcus116/2011/07/10/31423
            DirectoryInfo di0 = di.GetDirectories()[0];
            DirectoryInfo[] listbox2DataSource=di.GetDirectories()
                .Where(val => val != di0).ToArray();//寫成「.Where<DirectoryInfo>」也無效
            listBox2.DataSource = listbox2DataSource;
            //移除無效，再研究 : C# 從陣列中取掉一個元素
            */
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            moveFile();
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            moveFile();
        }

        internal void listBox1RefQuery()
        {
            listFiles();
        }
        internal void listBox2RefQuery()
        {
            listFolders();
        }

        void moveFile()
        {
            if (listBox1.SelectedItems.Count == 0 || listBox2.SelectedItems.Count == 0) return;
            ListBox.SelectedObjectCollection selected = listBox1.SelectedItems;
            List<FileInfo> fis = new List<FileInfo>();
            foreach (object item in selected)
            {
                fis.Add((FileInfo)item);
            }
            DirectoryInfo di = listBox2.SelectedItem as DirectoryInfo;
            try
            {
                fileOps fMove = new fileOps(fis);
                fMove.moveFiles2DirControl(di, listBox1, this);
            }
            catch (Exception)
            {

                throw;
            }
        }

        void dialogBoxWarning(string warningMsg)
        {
            //DialogResult dr = MessageBox.Show("檔案已存在，是否取代原檔案？", "注意：", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            DialogResult dr = MessageBox.Show(warningMsg, "注意：", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            switch (dr)
            {
                case DialogResult.Cancel:
                    //return;
                    break;
                case DialogResult.Yes:

                    break;
                case DialogResult.No:
                    break;
                default:
                    break;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listFiles();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.F5:
                    if (textBox4.Text != "") extensionFilter();
                    else listFiles();
                    listFolders();
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {//https://docs.microsoft.com/zh-tw/dotnet/desktop/winforms/input-keyboard/how-to-check-modifier-key?view=netdesktop-5.0
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                //MessageBox.Show("KeyPress " + Keys.Shift);
                if (e.KeyChar == 23) this.Close();//w=23
        }

        void rightKeyMouse(string fileorDir)
        {
            if (File.Exists(fileorDir) || Directory.Exists(fileorDir))
            {
                try
                {
                    Process.Start(fileorDir);
                }
                catch
                {
                    MessageBox.Show("此連結或檔案無效。", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                rightKeyMouse(textBox1.Text);
            }
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                rightKeyMouse(textBox2.Text);
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (listBox1.SelectedItems.Count > 0)
                    {
                        FileInfo fi = listBox1.SelectedItem as FileInfo;
                        rightKeyMouse(fi.FullName);
                    }
                    break;
                case MouseButtons.Left:
                    ////再研究：拖放清單中的項目 https://zh.stackoom.com/question/2KGOV/%E5%9C%A8%E5%B0%8D%E8%B1%A1%E5%88%97%E8%A1%A8%E6%A1%86%E4%B8%8A%E6%8B%96%E6%94%BE
                    //listBox1.DoDragDrop(listBox1.SelectedItems, DragDropEffects.Copy);
                    break;
            }

        }

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listBox2.SelectedItems.Count > 0)
                {
                    DirectoryInfo di = listBox2.SelectedItem as DirectoryInfo;
                    rightKeyMouse(di.FullName);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            textBox1.Text = Clipboard.GetText();
        }

        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            textBox2.Text = Clipboard.GetText();
        }

        private bool doNotEntered = false;//https://bit.ly/3esQfGM
        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {//https://bit.ly/3esQfGM  https://bit.ly/3aunpVd
            if (doNotEntered)//不讓按鍵干擾操作
            { e.Handled = true; doNotEntered = false; }//使用完畢要還原
            //if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            //    if (e.KeyChar == 3)//char.Parse("c")) 複製檔案 
            //    {
            //        /* 
            //         * 1.System.Collections.Specialized.StringCollection scss = new System.Collections.Specialized.StringCollection();
            //            scss.Add(((FileInfo)listBox1.SelectedItem).FullName);
            //            Clipboard.SetData(DataFormats.FileDrop, scss);//這也不行
            //        * 2. Clipboard.SetData(DataFormats.FileDrop,(FileInfo)listBox1.SelectedItem);
            //        * 3. Clipboard.SetDataObject((FileInfo)listBox1.SelectedItem);//此法不行，下面才行
            //        */
            //        ClipBoardPlus.CopyFiles(((FileInfo)listBox1.SelectedItem).FullName);
            //        return;
            //    }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys == Keys.Control || ModifierKeys == Keys.Shift ||
                ModifierKeys == Keys.Alt) doNotEntered = true;
            else doNotEntered = false;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    moveFile();//執行多重檔案移動
                    break;
                case Keys.Oem3://Esc鍵下的「`」鍵
                    moveFile();//執行多重檔案移動
                    break;
                case Keys.F5:
                    listBox1RefQuery();
                    break;
                case Keys.Delete:
                    if (listBox1.SelectedItems.Count > 0)
                    {
                        if (ModifierKeys == Keys.Shift) { }
                        else
                        {
                            if (MessageBox.Show("確定刪除？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.
                                Warning) != DialogResult.OK)
                                return;
                        }
                        int idx = listBox1.SelectedIndices[0];
                        foreach (FileInfo item in listBox1.SelectedItems)
                        {
                            File.Delete(item.FullName);
                        }
                        listBox1RefQuery();
                        listBox1.SelectionMode = SelectionMode.One;
                        if (idx + 10 < listBox1.Items.Count)
                            listBox1.SelectedIndex = idx + 10;
                        else
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        if (idx < listBox1.Items.Count)
                            listBox1.SelectedIndex = idx;
                        listBox1.SelectionMode = SelectionMode.MultiExtended;
                    }

                    break;

                default:
                    break;
            }
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.KeyCode == Keys.C) //複製檔案到剪貼簿準備手動貼上
                {
                    if (listBox1.Items.Count > 0)
                    {
                        //ClipBoardPlus.CopyFile(((FileInfo)listBox1.SelectedItem).FullName);
                        int i = 0;
                        string[] listboxselected = new string[listBox1.SelectedItems.Count];
                        switch (listBox1.Items[0].GetType().Name)
                        {
                            case "FileInfo":
                                foreach (FileInfo item in listBox1.SelectedItems)
                                    listboxselected[i++] = item.FullName;
                                break;
                            case "DirectoryInfo":
                                foreach (DirectoryInfo item in listBox1.SelectedItems)
                                    listboxselected[i++] = item.FullName;
                                break;
                            default:
                                break;
                        }
                        ClipBoardPlus.CopyFilesDirs(listboxselected);
                    }
                }
            }


            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.KeyCode == Keys.A) //複製檔案到剪貼簿準備手動貼上
                {
                    if (listBox1.Items.Count > 0)
                    {
                        for (int i = 0; i < listBox1.Items.Count; i++)
                        {
                            listBox1.SetSelected(i, true);
                        }
                    }
                }
            }
        }

        private void listBox2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    listBox2RefQuery();
                    break;
                case Keys.Oem3://Esc鍵下的「`」鍵
                    moveFile();//執行多重檔案移動
                    break;
                default:
                    break;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            float flSize = (float)numericUpDown1.Value;
            using (Font fnt1 = new Font(listBox1.Font.FontFamily, flSize),
                        fnt2 = new Font(listBox2.Font.FontFamily, flSize))
            {
                listBox1.Font = fnt1;
                listBox2.Font = fnt2;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "篩選！開頭")
            {
                string fdrPath = textBox2.Text;
                if (!Directory.Exists(fdrPath)) return;
                di = new DirectoryInfo(fdrPath);
                List<DirectoryInfo> dList = new List<DirectoryInfo>();
                foreach (DirectoryInfo item in di.GetDirectories())
                {
                    if (item.Name.IndexOf("System Volume Information") == -1 &&
                        item.Name.StartsWith("！"))
                        dList.Add(item);
                }
                listBox2.DataSource = dList;//.Sort();
            }
            else
                listFolders();
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {//https://wellbay.cc/thread-976171.htm
            if (listBox1.Items.Count == 0)
            {
                List<DirectoryInfo> dis = new List<DirectoryInfo>();
                string[] files = (string[])e.Data.GetData(
                    DataFormats.FileDrop);
                foreach (string file in files)
                {
                    //if (Path.GetExtension(file) == ".txt")  //判斷檔案型別，只接受txt檔案
                    //{
                    //    textBox1.Text += file + "\r\n";
                    //}
                    DirectoryInfo di = new DirectoryInfo(file);
                    dis.Add(di);
                }
                listBox1.DataSource = dis;
            }
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {//https://wellbay.cc/thread-976171.htm
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void listBox1_DragLeave(object sender, EventArgs e)
        {// 這是拖放進入後又出範圍時之leave也 20210428
        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = ""; listFiles();
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    extensionFilter();//副檔名篩選
                    break;
                default:
                    break;
            }
        }

        private void extensionFilter()
        {//副檔名篩選
            string fn; string textBox4Text = textBox4.Text;
            if (textBox4Text == "") { listFiles(); return; }
            if (fArray == null) return;
            //Regex rx = new Regex("[a-zA-Z]"); if (!rx.IsMatch(textBox4Text)) return;
            if (!new Regex("[a-zA-Z]").IsMatch(textBox4Text)) return;
            List<FileInfo> fList = new List<FileInfo>();
            foreach (FileInfo item in fArray)
            {
                fn = item.Name;
                int extStart = fn.LastIndexOf(".") + 1;
                //不分大小寫比對字串
                if (string.Equals(fn.Substring(extStart, fn.Length - extStart), textBox4Text, StringComparison.OrdinalIgnoreCase))
                    fList.Add(item);
            }
            listBox1.DataSource = fList;

        }
        private void fileNameFilter()
        {//檔名篩選
            string textBox5Text = textBox5.Text;
            if (textBox5Text == "") { listFiles(); return; }
            if (fArray == null) return;
            List<FileInfo> fList = new List<FileInfo>();
            foreach (FileInfo item in fArray)
            {
                //不分大小寫比對字串
                if (item.Name.IndexOf(textBox5Text) > -1)//, StringComparison.OrdinalIgnoreCase))
                    fList.Add(item);
            }
            listBox1.DataSource = fList;

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {//檔名篩選

        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    fileNameFilter();//檔名篩選
                    break;
                default:
                    break;
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] sc = (string[])e.Data.GetData(DataFormats.FileDrop);
            //是資料夾才處理
            if (!Directory.Exists(sc[0])) return;
            TextBox txbox = (TextBox)sender;
            txbox.Text = sc[0];
        }

        private void textBox1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void listBox2_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void listBox2_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;

            int idx;
            if (listBox2.Items.Count > 0)
            {
                idx = listBox2.IndexFromPoint(new
                    Point(e.X - (listBox2.Left + this.Left),
                    e.Y - (listBox2.Top + this.Top + (Top - listBox2.Top))));
                if (idx > -1)
                    listBox2.SelectedIndex = idx;
            }
        }

        private void listBox2_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
