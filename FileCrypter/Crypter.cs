using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace FileCrypter
{
    internal class Crypter
    {
        /// <summary>
        /// Encryping & compressing files & folders 
        /// </summary>
        /// <param name="pathes">pathes to folders & files</param>
        /// <param name="password">password to encrypt</param>
        public void Encrypt(Path[] pathes, string password)
        {
            for (int i = 0; i < pathes.Length; i++)
            {
                EncryptFile(pathes[i], password);
                Console.Title = $"{i + 1}/{pathes.Length}";
            }
        }
        /// <summary>
        /// Decryping & dempressing files & folders 
        /// </summary>
        /// <param name="pathes">pathes to folders & files</param>
        /// <param name="password">password to encrypt</param>
        public void Decrypt(Path[] pathes, string password)
        {
            for (int i = 0; i < pathes.Length; i++)
            {
                DecryptFile(pathes[i], password);
                Console.Title = $"{i + 1}/{pathes.Length}";
            }
        }


        private bool EncryptFile(Path path, string password)
        {
            if (path.cryptStatus == CryptStatus.NotCrypted)
            {
                try
                {
                    ColorWriter.Write($"\nEncrypting {path.FileName}", ConsoleColor.Green);

                    Console.Title = "Reading...";

                    byte[] bytes = File.ReadAllBytes(path.path);

                    Console.Title = "30%";

                    byte[] finalText = EncryptBytes(bytes, password);

                    Console.Title = "60%";

                    finalText = Compress(finalText);

                    Console.Title = "70%";

                    File.WriteAllBytes(path.path, finalText);

                    Console.Title = "100%";

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
        private bool DecryptFile(Path path, string password)
        {
            if (path.cryptStatus == CryptStatus.Crypted)
            {
                try
                {
                    ColorWriter.Write($"\nDecrypting {path.FileName}", ConsoleColor.Green);

                    byte[] bytes = File.ReadAllBytes(path.path);

                    Console.Title = "30%";

                    bytes = Decompress(bytes);

                    Console.Title = "40%";

                    byte[] finalText = DecryptBytes(bytes, password);

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
        private byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }
        private byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
        private static SymmetricAlgorithm GetAlgorithm(string password)
        {
            Rijndael algorithm = Rijndael.Create();
            Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(password, new byte[] {
        0x53,0x6f,0x64,0x69,0x75,0x6d,0x20,             // salty goodness
        0x43,0x68,0x6c,0x6f,0x72,0x69,0x64,0x65
    });
            algorithm.Padding = PaddingMode.ISO10126;
            algorithm.Key = rdb.GetBytes(32);
            algorithm.IV = rdb.GetBytes(16);
            return algorithm;
        }
        /// <summary>
        /// Encrypts a string with a given password.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="password">The password.</param>
        private static byte[] EncryptBytes(byte[] clearBytes, string password)
        {
            SymmetricAlgorithm algorithm = GetAlgorithm(password);
            ICryptoTransform encryptor = algorithm.CreateEncryptor();
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();
                return ms.ToArray();
            }
        }
        /// <summary>
        /// Decrypts a string using a given password.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="password">The password.</param>
        private static byte[] DecryptBytes(byte[] cipherBytes, string password)
        {
            SymmetricAlgorithm algorithm = GetAlgorithm(password);
            ICryptoTransform decryptor = algorithm.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.Close();
                return ms.ToArray();
            }
        }
    }
}
