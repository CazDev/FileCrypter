using System;
using System.Collections.Generic;
using System.IO;

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

        private static void Main(string[] args)
        {
            //args = new[]
            //{
            //    "C:\\Users\\tavvi\\Desktop\\123"
            //};

            if (args.Length == 0)
            {
                Console.WriteLine("Drop file/folder on exe");
                Console.ReadKey();
                Environment.Exit(0);
            }

            string password = GetPassword();

            ConsoleKeyInfo key = GetKey();

            Console.Clear();

            ColorWriter.Write("Getting files...", ConsoleColor.White);
            Path[] pathes = GetArgs(args);

            if (key.Key == ConsoleKey.E)
            {
                crypter.Encrypt(pathes, password);
            }
            else
            {
                crypter.Decrypt(pathes, password);
            }

            ColorWriter.Write("\nDone", ConsoleColor.White);
            Console.ReadKey();
        }

        private static Path[] GetArgs(string[] pathes)
        {
            List<Path> args = new List<Path>();
            for (int i = 0; i < pathes.Length; i++)
            {
                try
                {
                    FileAttributes attr = File.GetAttributes(pathes[i]);
                    if (attr.HasFlag(FileAttributes.Directory))
                    {//if dir
                        args.AddRange(GetArgs(Directory.GetFiles(pathes[i])));
                        args.AddRange(GetArgs(Directory.GetDirectories(pathes[i])));
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

        private static ConsoleKeyInfo GetKey()
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
        static string GetPassword()
        {
            string pass;
            do
            {
                Console.Write("Enter password: ");
                pass = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(pass));
            return pass;
        }
    }
}
