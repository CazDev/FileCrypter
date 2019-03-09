using System;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

namespace FileCrypter.Model
{
    public class Updater
    {
        private const string DownloadLink = "https://github.com/tavvi1337/FileCrypter/raw/master/FileCrypter/bin/Debug/FileCrypter.exe";

        private string ComputeMD5Checksum(string path)
        {
            using (FileStream fs = System.IO.File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                return result;
            }
        }
    }
}
