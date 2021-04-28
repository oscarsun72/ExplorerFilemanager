using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;


namespace ExplorerFilemanager
{
    public static class ClipBoardPlus
    {
        public static void CopyFile(string fileFullname)
        {//單一檔案複製
            //StringCollection A =
            //        new StringCollection();
            //A.Add(fileFullname);//此與下等式，VisualStudio建議simple化
            StringCollection A =new StringCollection{fileFullname};
            Clipboard.SetFileDropList(A);
        }
        public static void CopyFilesDirs(string[] Fullnames)
        {//多個檔案複製（或資料夾亦可）
            StringCollection A = new StringCollection();
            A.AddRange(Fullnames);
            Clipboard.SetFileDropList(A);
        }

        public static void CopyDirectories(string[] dirs)
        {            
            StringCollection A = new StringCollection();
            A.AddRange(dirs);
            Clipboard.SetFileDropList(A);
        }


        //https://docs.microsoft.com/zh-tw/dotnet/desktop/winforms/advanced/how-to-add-data-to-the-clipboard?view=netframeworkdesktop-4.8
        // Demonstrates SetFileDropList, ContainsFileDroList, and GetFileDropList
        public static System.Collections.Specialized.StringCollection
            SwapClipboardFileDropList(
            System.Collections.Specialized.StringCollection replacementList)
        {
            System.Collections.Specialized.StringCollection returnList = null;
            if (Clipboard.ContainsFileDropList())
            {
                returnList = Clipboard.GetFileDropList();
                Clipboard.SetFileDropList(replacementList);
            }
            return returnList;
        }

    }
}