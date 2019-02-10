using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FileCrypter
{
    internal enum CryptStatus
    {
        Crypted,
        NotCrypted
    }

    internal struct Path
    {
        public string path;
        public string FileName;
        public CryptStatus cryptStatus;

        public Path(string path, CryptStatus cryptStatus)
        {
            this.path = path;
            this.cryptStatus = cryptStatus;
            string[] temp = path.Split('\\');
            FileName = temp[temp.Length - 1];
        }
    }

    internal class Program
    {
        private static readonly Crypter crypter = new Crypter();
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                StartWithoutArgs(GetPassword());
            }
            else
            {
                StartUsingArgs(args, GetPassword());
            }
            ColorWriter.Write("\nDone", ConsoleColor.White);

            Console.ReadKey();
        }
        /// <summary>
        /// Show openFileDialog, get files and encrypt or decrypt
        /// </summary>
        /// <param name="password"></param>
        private static void StartWithoutArgs(string password)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ReadKeyAndEncryptOrDecrypt(GetPathesFromPathes(ofd.FileNames), password);
            }
        }
        /// <summary>
        /// Use args as pathes to files & folders
        /// </summary>
        /// <param name="args">Arguments</param>
        /// <param name="password">pass</param>
        private static void StartUsingArgs(string[] args, string password)
        {
            ColorWriter.Write("Getting files...\n", ConsoleColor.White);
            Path[] pathes = GetPathesFromPathes(args);

            ReadKeyAndEncryptOrDecrypt(pathes, password);
        }
        #region subMethods-StartUsingArgs
        private static ConsoleKeyInfo GetKeyEorD()
        {
            ColorWriter.Write("Perss \"D\" to decrypt, \"E\" to encrypt", ConsoleColor.White);
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.E || key.Key == ConsoleKey.D)
                {
                    break;
                }
            } while (true);
            return key;
        }

        private static string GetPassword()
        {
            string pass;
            string conf;
            do
            {
                Console.Write("Enter password: ");
                pass = Console.ReadLine();

                Console.Write("Confirm password: ");
                conf = Console.ReadLine();

                Console.Clear();
            } while (pass != conf);
            return pass;
        }
        #endregion
        private static Path[] GetPathesFromPathes(string[] pathes)
        {
            List<Path> args = new List<Path>();
            for (int i = 0; i < pathes.Length; i++)
            {
                try
                {
                    FileAttributes attr = File.GetAttributes(pathes[i]);
                    if (attr.HasFlag(FileAttributes.Directory))
                    {//if dir
                        args.AddRange(GetPathesFromPathes(Directory.GetFiles(pathes[i])));
                        args.AddRange(GetPathesFromPathes(Directory.GetDirectories(pathes[i])));
                    }
                    else
                    {// if file
                        CryptStatus cryptStatus;
                        if (pathes[i].EndsWith(".crr") || pathes[i].EndsWith(".crr\\"))
                        {
                            cryptStatus = CryptStatus.Crypted;
                        }
                        else
                        {
                            cryptStatus = CryptStatus.NotCrypted;
                        }

                        args.Add(new Path(pathes[i], cryptStatus));
                    }
                }
                catch { }
            }
            return args.ToArray();
        }
        private static void ReadKeyAndEncryptOrDecrypt(Path[] pathes, string password)
        {
            ConsoleKeyInfo key = GetKeyEorD();

            if (key.Key == ConsoleKey.E)
            {
                crypter.Encrypt(pathes, password);
            }
            else
            {
                crypter.Decrypt(pathes, password);
            }
        }
    }
}
