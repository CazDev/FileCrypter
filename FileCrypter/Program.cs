using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCrypter
{
    enum CryptStatus
    {
        Crypted,
        NotCrypted
    }
    struct Path
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
    class Program
    {
        static Crypter crypter = new Crypter();
        static void Main(string[] args)
        {
            //args = new[]
            //{
            //    "E:\\123\\Progs"
            //};

            if(args.Length == 0)
            {
                Console.WriteLine("Drop file/folder on exe");
                Console.ReadKey();
                Environment.Exit(0);
            }

            ColorWriter.Write("Perss \"D\" to decrypt, \"E\" to encrypt", ConsoleColor.White);
            ConsoleKeyInfo key;
            do
            {
                 key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.E || key.Key == ConsoleKey.D)
                    break;
            } while (true);

            Console.Clear();
            ColorWriter.Write("Getting files...", ConsoleColor.White);
            Path[] pathes = GetArgs(args);

            for (int i = 0; i < pathes.Length; i++)
            {
                if (key.Key == ConsoleKey.E && pathes[i].cryptStatus == CryptStatus.NotCrypted)
                {
                    crypter.Encrypt(pathes[i]);
                }
                else if (key.Key == ConsoleKey.D && pathes[i].cryptStatus == CryptStatus.Crypted)
                {
                    crypter.Decrypt(pathes[i]);
                }
                Console.Title = $"{i+1}/{pathes.Length}";
            }
            ColorWriter.Write("\nDone", ConsoleColor.White);
            Console.ReadKey();
        }

        public static Path[] GetArgs(string[] pathes)
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
                            cryptStatus = CryptStatus.Crypted;
                        else
                            cryptStatus = CryptStatus.NotCrypted;
                        args.Add(new Path(pathes[i], cryptStatus));
                    }
                }
                catch { }
            }
            return args.ToArray();
        }
    }
}
