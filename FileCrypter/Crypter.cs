using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileCrypter
{
    class Crypter
    {
        public List<Task> tasks = new List<Task>();
        public int TasksCounter { get; private set; } = 0;
        public bool Encrypt(Path path)
        {
            if (path.cryptStatus == CryptStatus.NotCrypted)
            {
                try
                {
                    ColorWriter.Write($"\nEncrypting {path.FileName}", ConsoleColor.Green);

                    Console.Title = "Reading...";

                    byte[] text = File.ReadAllBytes(path.path);

                    Console.Title = "30%";

                    byte[] finalText = CryptBytes(text);

                    Console.Title = "60%";

                    int was = finalText.Length;
                    finalText = Compress(finalText);

                    Console.Title = "70%";

                    int now = finalText.Length;
                    int temp = was - now;
                    double temp1 = Convert.ToDouble(temp) / Convert.ToDouble(was);
                    double percent = temp1 * 100;

                    File.WriteAllBytes(path.path, finalText);

                    Console.Title = "100%";

                    ColorWriter.Write(String.Format(" -{0:##}%", percent), ConsoleColor.White);

                    File.Move(path.path, $"{path.path}.crr");
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public bool Decrypt(Path path)
        {
            if (path.cryptStatus == CryptStatus.Crypted)
            {
                try
                {
                    ColorWriter.Write($"\nDecrypting {path.FileName}", ConsoleColor.Green);

                    byte[] text = File.ReadAllBytes(path.path);

                    Console.Title = "30%";

                    text = Decompress(text);

                    Console.Title = "40%";

                    byte[] finalText = DecryptBytes(text);

                    Console.Title = "70%";

                    File.WriteAllBytes(path.path, finalText);

                    File.Move(path.path, $"{path.path.Substring(0, path.path.Length - ".crr".Length)}");

                    Console.Title = "100%";
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }
        public byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
        public static byte[] DecryptBytes(byte[] arr)
        {
            byte[] result = ByteReverse(arr);

            result = DecBytes(arr);

            return result;
        }
        public static byte[] CryptBytes(byte[] arr)
        {
            byte[] result = ByteReverse(arr);

            result = IncBytes(arr);
            return result;
        }
        public static byte[] ByteReverse(byte[] arr)
        {
            return arr.Reverse().ToArray();
        }
        public static byte[] DecBytes(byte[] arr)
        {
            byte[] result = new byte[arr.Length];
            int counter = 0;
            foreach (var num in arr)
            {
                if (num == 0)
                {
                    result[counter] = 255;
                }
                else
                {
                    byte temp = Convert.ToByte(num - 1);
                    result[counter] = temp;
                }
                counter++;
            }
            return result.ToArray();
        }
        public static byte[] IncBytes(byte[] arr)
        {
            byte[] result = new byte[arr.Length];
            int counter = 0;
            foreach (var num in arr)
            {
                if (num == 255)
                {
                    result[counter] = 0;
                }
                else
                {
                    byte temp = Convert.ToByte(num + 1);
                    result[counter] = temp;
                }
                counter++;
            }
            return result.ToArray();
        }
    }
}
