using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExplorerFilemanager
{/*FileInfo
  * https://jjnnykimo.pixnet.net/blog/post/26817575
  System.IO底下的類別：
Directory	提供類別方法來建立、移動、複製資料夾......等功能
DirectoryInfo	一個資料夾就是一個DirectoryInfo物件
File	提供類別方法來建立、移動、複製檔案......等功能
FileInfo	一個檔案就是一個FileInfo物件
StreamReader	使用位元組串流來讀取文字檔
StreamWriter	使用位元組串流來寫入文字檔
FileStream	建立檔案串流，可以用來處理二進制檔
  */
    public class fileOps
    {
        List<FileInfo> fis;
        FileInfo f;
        public fileOps(List<FileInfo> fInfos)
        {
            fis = fInfos;
        }
        public fileOps(FileInfo fInfo)
        {
            f = fInfo;
        }

        public fileOps(string fileFullname)
        {
            f = new FileInfo(fileFullname);
        }

        public void moveFiles2DirControl(DirectoryInfo di,
            ListBox listBox, Form1 frm)
        {
            string moveToFileFullname; ListBox.SelectedIndexCollection idc = listBox.SelectedIndices;//Point p;//記下清單中選取的位置
            int idx = idc[idc.Count - 1];
            foreach (FileInfo fi in fis)
            {
                //Point p = listBox1.AutoScrollOffset;
                try
                {
                    moveToFileFullname = di.FullName + "\\" + fi.ToString();
                    if (moveToFileFullname == fi.FullName)continue;//避免同一檔案的誤刪（移動時須先刪除目的檔案才移動來源檔）
                    if (File.Exists(moveToFileFullname))
                    {
                        FileInfo fiNew= new FileInfo(moveToFileFullname);
                        DialogResult dr;
                        if (fiNew.Length==fi.Length&&fiNew.LastWriteTime==fi.LastWriteTime)
                        {
                            dr = DialogResult.Yes;
                        }
                        else
                        dr = MessageBox.Show("檔案已存在，是否取代原檔案？\r\n" +
                            "檔名： " + fi.Name+ "\r\n\r\n"+
                            "來源檔日期： " + fi.LastWriteTime.ToString() + "\r\n\r\n" +
                            "目的檔日期： " + fiNew.LastWriteTime.ToString()+ "\r\n\r\n" +
                            "來源檔大小： " +(fi.Length / 1000).ToString() + "KB" + "\r\n\r\n" +
                            "目的檔大小： " + (fiNew.Length/1000)+ "KB" + "\r\n\r\n" +
                            "取消作業請按「取消」，\r\n重新命名移動過去的檔，請按「否」。", "注意：", 
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2);
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
                                    moveToFileFullname = movefileName + "("
                                           + (i++.ToString() + ")" + ext);
                                } while (File.Exists(moveToFileFullname));
                                File.Move(fi.FullName, moveToFileFullname);
                                break;
                            default:
                                break;
                        }


                    }
                    else
                        File.Move(fi.FullName, moveToFileFullname);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally {; }
            }
            frm.listBox1RefQuery();
            listBox.SelectionMode = SelectionMode.One;
            if (idx + 10 < listBox.Items.Count)
                listBox.SelectedIndex = idx + 10;
            else
                listBox.SelectedIndex = listBox.Items.Count - 1;
            if (idx < listBox.Items.Count)
                listBox.SelectedIndex = idx;
            listBox.SelectionMode = SelectionMode.MultiExtended;
            //listBox.AutoScrollOffset = p;

        }

        public void deleteFiles(DirectoryInfo di,
            ListBox listBox, Form1 frm)
        { }
    }
}
