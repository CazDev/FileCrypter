using FileCrypter.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;

namespace FileCrypter.Tests
{
    [TestClass()]
    public class CrypterTests
    {
        private readonly Crypter crypter = new Crypter();
        private readonly Random rnd = new Random();
        private readonly string TempPath = $"{DriveInfo.GetDrives()[0].Name}Users\\{Environment.UserName}\\AppData\\Local\\Temp\\";

        [TestMethod()]
        public void EncryptDecryptTest()
        {
            PathName pathName = CreatePathName();
            string password = Guid.NewGuid().ToString();

            string MD5First = CalculateMD5(pathName.path);

            crypter.Encrypt(new[] { pathName }, password);

            string MD5Crypted = CalculateMD5(pathName.path + ".crr");

            crypter.Decrypt(new[] { new PathName(pathName.path + ".crr", CryptStatus.Crypted) }, password);

            string MD5NotCrypted = CalculateMD5(pathName.path);

            Assert.AreEqual(MD5First, MD5NotCrypted);

            File.Delete(pathName.path);
        }
        private PathName CreatePathName()
        {
            PathName pathName;
            while (true)
            {
                string pathToFile = $"{TempPath}{rnd.Next()}.debug";
                if (!File.Exists(pathToFile))
                {
                    File.WriteAllBytes(pathToFile, Guid.NewGuid().ToByteArray());
                    pathName = new PathName(pathToFile, CryptStatus.NotCrypted);
                    return pathName;
                }
            }
        }

        private string CalculateMD5(string filename)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        [TestMethod()]
        public void EncryptBytesTest()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            string password = Guid.NewGuid().ToString();

            byte[] encrypted = crypter.EncryptBytes(bytes, password);
            byte[] decrypted = crypter.DecryptBytes(encrypted, password);

            Assert.AreEqual(bytes.Length, decrypted.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(bytes[i], decrypted[i]);
            }
        }
        [TestMethod()]
        public void DecryptBytesTest()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            string password = Guid.NewGuid().ToString();

            byte[] encrypted = crypter.EncryptBytes(bytes, password);
            byte[] decrypted = crypter.DecryptBytes(encrypted, password);

            Assert.AreEqual(bytes.Length, decrypted.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(bytes[i], decrypted[i]);
            }
        }
    }
}