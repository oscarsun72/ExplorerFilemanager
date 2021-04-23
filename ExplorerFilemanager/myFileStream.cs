using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerFilemanager
{
    public class myFileStream : FileStream
    {
        public myFileStream(string path, FileMode mode) : base(path, mode)
        {
        }
    }
}
