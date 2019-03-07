using FileCrypter.Controller;
using System;

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FileCrypter
{
    public class Program
    {
        static CryptController controller = new CryptController();
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                controller.StartWithoutArgs();
            }
            else
            {
                controller.StartUsingArgs(args);
            }

            ColorWriter.Write("\nDone", ConsoleColor.White);

            controller.Update();
            Console.ReadKey();
        }
    }
}
