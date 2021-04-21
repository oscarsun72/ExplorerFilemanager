using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExplorerFilemanager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        DirectoryInfo di;
        private void Form1_Load(object sender, EventArgs e)
        {
            listFolders(); listFiles();
            comboBox1.DataSource = new List<string> { "move to" };
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            listFolders();
        }

        void listFiles()
        {
            di = new DirectoryInfo(textBox1.Text);
            listBox1.DataSource = di.GetFiles();
        }
        void listFolders()
        {
            di = new DirectoryInfo(textBox2.Text);
            //            listBox2.DataSource = di.GetDirectories();            
            List<DirectoryInfo> dList = new List<DirectoryInfo>();
            foreach (DirectoryInfo item in di.GetDirectories())
            {
                if (item.Name.IndexOf("System Volume Information") == -1)
                    dList.Add(item);
            }
            listBox2.DataSource = dList;

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

        private void listBox1RefQuery()
        {
            listFiles();
        }

        void moveFile()        {            
            if (listBox1.SelectedItems.Count == 0 || listBox2.SelectedItems.Count == 0) return;
            int idx = listBox1.SelectedIndex;Point p = listBox1.AutoScrollOffset;
             FileInfo fi = listBox1.SelectedItem as FileInfo;
            DirectoryInfo di = listBox2.SelectedItem as DirectoryInfo;
            string moveToFileFullname = di.FullName + "\\" +
                 listBox1.SelectedItem.ToString();
            if (File.Exists(moveToFileFullname))
            {
                DialogResult dr = MessageBox.Show("檔案已存在，是否取代原檔案？\r\n" +
                    "取消作業請按「取消」，\r\n重新命名移動過去的檔，請按「否」。", "注意：", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (dr)
                {
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.Yes:
                        File.Delete(moveToFileFullname);
                        File.Move(fi.FullName, moveToFileFullname);
                        break;
                    case DialogResult.No:
                        int i = 0;
                        string ext = fi.Extension;
                        string movefileName =
                            moveToFileFullname.Substring(0, moveToFileFullname.IndexOf(ext));
                        do
                        {
                            moveToFileFullname = movefileName +"("
                                   + (i++.ToString() + ")"+ ext);
                        } while (File.Exists(moveToFileFullname));
                        File.Move(fi.FullName, moveToFileFullname);
                        break;
                    default:
                        break;
                }


            }
            else
                File.Move(fi.FullName, moveToFileFullname);
            listBox1RefQuery();
            if (idx < listBox1.Items.Count)
                listBox1.SelectedIndex = idx;
            listBox1.AutoScrollOffset = p;
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
            Process.Start(fileorDir);
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
            if (e.Button == MouseButtons.Right)
            {
                FileInfo fi = listBox1.SelectedItem as FileInfo;
                rightKeyMouse(fi.FullName);
            }
        }

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DirectoryInfo di = listBox2.SelectedItem as DirectoryInfo;
                rightKeyMouse(di.FullName);
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

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)                
                if (e.KeyChar == 3)//char.Parse("c")) 
                //Clipboard.SetData()
                Clipboard.SetDataObject(listBox1.SelectedItem);                

        }
    }
}
