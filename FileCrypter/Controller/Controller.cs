using FileCrypter.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FileCrypter.Controller
{
    public class CryptController
    {
        private readonly Crypter crypter = new Crypter();
        private readonly Updater updater = new Updater();

        /// <summary>
        /// Show openFileDialog, get files and encrypt or decrypt
        /// </summary>
        /// <param name="password"></param>
        public void StartWithoutArgs()
        {
            string password = GetPassword();
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
        public void StartUsingArgs(string[] args)
        {
            if (args[0] == "new_ver")
            {
                MessageBox.Show("Updated");
                List<string> new_args = args.OfType<string>().ToList();
                new_args.RemoveAt(0);
                args = new_args.ToArray();
            }


            string password = GetPassword();
            ColorWriter.Write("Getting files...\n", ConsoleColor.White);
            Model.PathName[] pathes = GetPathesFromPathes(args);

            ReadKeyAndEncryptOrDecrypt(pathes, password);
        }
        public void Update()
        {
            if (MessageBox.Show("Job done! Do you want to check updates?", "Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information) == DialogResult.Yes)// ask if user want to check update
            {
                updater.Update();
            }
        }


        #region subMethods-StartUsingArgs
        private ConsoleKeyInfo GetKeyEorD()
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

        private string GetPassword()
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
        private PathName[] GetPathesFromPathes(string[] pathes)
        {
            List<PathName> args = new List<PathName>();
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

                        args.Add(new PathName(pathes[i], cryptStatus));
                    }
                }
                catch { }
            }
            return args.ToArray();
        }
        private void ReadKeyAndEncryptOrDecrypt(PathName[] pathes, string password)
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
