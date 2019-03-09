using FileCrypter.Controller;
using System;
using System.Windows.Forms;

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
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Multiselect = true
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    controller.StartUsingPathes(ofd.FileNames);
                }
            }
            else
            {
                controller.StartUsingPathes(args);
            }

            ColorWriter.Write("\nDone", ConsoleColor.White);

            Console.ReadKey();
        }
    }
}
