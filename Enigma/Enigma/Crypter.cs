using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Enigma
{
    class Crypter
    {
        private SymmetricAlgorithm _encriptionAlgorithm;

        public Crypter(string algorithm)
        {
            switch (algorithm)
            {
                case "aes":
                    _encriptionAlgorithm = new AesCryptoServiceProvider();
                    break;
                case "des":
                    _encriptionAlgorithm = new DESCryptoServiceProvider();
                    break;
                case "rc2":
                    _encriptionAlgorithm = new RC2CryptoServiceProvider();
                    break;
                case "rijndael":
                    _encriptionAlgorithm = new RijndaelManaged();
                    break;
                default:
                    throw new ArgumentException("Algorithm not supported.");
            }
        }

        public void Encrypt(string inputFileName, string outputFileName)
        {
            string outputDir = new FileInfo(outputFileName).Directory.FullName;
            StringBuilder keyFileNameBuilder = new StringBuilder(outputDir).Append("\\file.key.txt");
            string keyFileName = keyFileNameBuilder.ToString();

            _encriptionAlgorithm.GenerateIV();
            _encriptionAlgorithm.GenerateKey();

            using (FileStream keyFileStream = new FileStream(keyFileName, FileMode.Create))
            {
                StreamWriter keyWriter = new StreamWriter(keyFileStream);

                keyWriter.WriteLine(Convert.ToBase64String(_encriptionAlgorithm.IV));
                keyWriter.WriteLine(Convert.ToBase64String(_encriptionAlgorithm.Key));

                keyWriter.Flush();

                keyWriter.Close();
            }

            using (ICryptoTransform cryptoTransform = _encriptionAlgorithm.CreateEncryptor())
            {
                ApplyCryptoTransform(cryptoTransform, inputFileName, outputFileName);   
            }
        }

        public void Decrypt(string inputFileName, string keyFileName, string outputFileName)
        {

            using (FileStream keyFileStream = new FileStream(keyFileName, FileMode.Open))
            {
                StreamReader keyReader = new StreamReader(keyFileStream);

                string iv = keyReader.ReadLine();
                string key = keyReader.ReadLine();

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
                    throw new ArgumentException("Error in key file.");

                _encriptionAlgorithm.IV = Convert.FromBase64String(iv);
                _encriptionAlgorithm.Key = Convert.FromBase64String(key);
            }

            using (ICryptoTransform cryptoTransform = _encriptionAlgorithm.CreateDecryptor())
            {
                ApplyCryptoTransform(cryptoTransform, inputFileName, outputFileName);
            }
        }

        private void ApplyCryptoTransform(ICryptoTransform cryptoTransform, string inputFileName, string outputFileName)
        {
            using (FileStream outputFileStream = new FileStream(outputFileName, FileMode.Create))
            {
                using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    using (FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open))
                    {
                        inputFileStream.CopyTo(cryptoStream);
                    }

                    cryptoStream.Flush();
                }
            }
        }
    }
}
