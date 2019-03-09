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
        /// Use args as pathes to files & folders
        /// </summary>
        /// <param name="path">Arguments</param>
        /// <param name="password">pass</param>
        public void StartUsingPathes(string[] path)
        {
            string password = GetPassword();
            ColorWriter.Write("Getting files...\n", ConsoleColor.White);
            Model.PathName[] pathes = GetPathNamesFromPathes(path);

            ConsoleKeyInfo key = GetKeyEorD();

            if (key.Key == ConsoleKey.E)
            {
                crypter.Encrypt(pathes, password);
            }
            else if (key.Key == ConsoleKey.D)
            {
                crypter.Decrypt(pathes, password);
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
        private PathName[] GetPathNamesFromPathes(string[] pathes)
        {
            List<PathName> args = new List<PathName>();
            for (int i = 0; i < pathes.Length; i++)
            {
                try
                {
                    FileAttributes attr = File.GetAttributes(pathes[i]);
                    if (attr.HasFlag(FileAttributes.Directory))
                    {//if dir
                        args.AddRange(GetPathNamesFromPathes(Directory.GetFiles(pathes[i])));
                        args.AddRange(GetPathNamesFromPathes(Directory.GetDirectories(pathes[i])));
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
    }
}
