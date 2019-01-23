using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCrypter
{
    static class ColorWriter
    {
        public static void Write(string s, ConsoleColor cc)
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = cc;
            Console.Write(s);
            Console.ForegroundColor = temp;
        }
    }
}
