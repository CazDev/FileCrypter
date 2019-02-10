using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCrypter
{
    class ProgressBar
    {
        int cursorLeft;
        int cursorTop;
        string Name;

        public ProgressBar(string Name)
        {
            this.cursorLeft = Console.CursorLeft;
            this.cursorTop = Console.CursorTop;

            this.Name = Name;
        }

        public void ReportValue(int progress)
        {
            int tempCursorLeft = Console.CursorLeft;
            int tempCursorTop = Console.CursorTop;

            progress = progress / 4;

            string sProgress = RepeatForLoop('*', progress);


            string text = String.Format("{0} [{1,-25}] {2,3}%", Name, sProgress, progress * 4);

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(text);

            Console.SetCursorPosition(tempCursorLeft, tempCursorTop);
        }

        public void End()
        {
            Console.WriteLine();
        }

        string RepeatForLoop(char c, int n, int max = 25)
        {
            string result = "";

            for (var i = 0; i < n; i++)
            {
                result += c;
            }

            return result;
        }
    }
}
