using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        void listFiles()
        {
            di = new DirectoryInfo(textBox1.Text);
            listBox1.DataSource = di.GetFiles();
        }
        void listFolders()
        {
            di = new DirectoryInfo(textBox2.Text);
            listBox2.DataSource = di.GetDirectories();

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            moveFile();
            listBox1RefQuery();
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            moveFile();
            listBox1RefQuery();

        }

        private void listBox1RefQuery()
        {
            listFiles();
        }

        void moveFile()
        {
            if (listBox1.SelectedItems.Count == 0 || listBox2.SelectedItems.Count == 0) return;
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
                            moveToFileFullname = movefileName
                                   + (i++.ToString() + ext);
                        } while (File.Exists(moveToFileFullname));
                        File.Move(fi.FullName, moveToFileFullname);
                        break;
                    default:
                        break;
                }


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
    }
}
