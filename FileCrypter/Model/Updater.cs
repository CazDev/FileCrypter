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
        private const string DownloadLink = "https://github.com/tavvi1337/FileCrypter/raw/master/FileCrypter/bin/Release/FileCrypter.exe";

        public void Update()
        {
            bool needUpdate = NeedUpdate();

            if (needUpdate)
            {
                WebClient wc = new WebClient();
                wc.Headers["User-Agent"] = "Mozilla/4.0";
                wc.Encoding = Encoding.UTF8;

                string fileName;
                if (File.Exists("FileCrypter_newVersion.exe"))
                {
                    for (int i = 1; ; i++)
                    {
                        if (!File.Exists($"FileCrypter_newVersion{i}.exe"))
                        {
                            fileName = $"FileCrypter_newVersion{i}.exe";
                            break;
                        }
                    }
                }
                else
                    fileName = "FileCrypter_newVersion.exe";

                wc.DownloadFile(DownloadLink, fileName);

                MessageBox.Show("Upated", "Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(fileName, "new_ver");

                Environment.Exit(0);
            }
            MessageBox.Show("You use latest version");
        }

        private bool NeedUpdate()
        {
            string exeMD5 = ComputeMD5Checksum(Assembly.GetExecutingAssembly().Location);
            string latestVerMD5 = GetLatestVersionMD5();

            return exeMD5 != latestVerMD5;
        }

        private string GetLatestVersionMD5()
        {
            WebClient wc = new WebClient();
            wc.Headers["User-Agent"] = "Mozilla/4.0";
            wc.Encoding = Encoding.UTF8;

            wc.DownloadFile(DownloadLink, "temp.exe");

            string MD5 = ComputeMD5Checksum("temp.exe");

            File.Delete("temp.exe");

            return MD5;
        }

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
