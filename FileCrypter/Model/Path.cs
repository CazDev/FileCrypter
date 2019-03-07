using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCrypter.Model
{
    public struct PathName
    {
        public string path;
        public string FileName;
        public CryptStatus cryptStatus;

        public PathName(string path, CryptStatus cryptStatus)
        {
            this.path = path;
            this.cryptStatus = cryptStatus;
            string[] temp = path.Split('\\');
            FileName = temp[temp.Length - 1];
        }
    }
}
