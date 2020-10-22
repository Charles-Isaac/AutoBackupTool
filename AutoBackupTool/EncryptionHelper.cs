using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackupTool
{
    public class EncryptionHelper
    {
        private static byte[] GenerateRandomSeed()
        {
            byte[] data = new byte[16];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // Fill buffer.
                rng.GetBytes(data);
            }
            return data;
        }
        public static byte[] EncryptAES(byte[] bytesToEncrypt, string password)
        {
            byte[] ivSeed = GenerateRandomSeed();

            var rfc = new Rfc2898DeriveBytes(password, ivSeed);
            byte[] Key = rfc.GetBytes(16);
            byte[] IV = rfc.GetBytes(16);

            byte[] encrypted;
            using (MemoryStream mstream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mstream, aesProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    }
                }
                encrypted = mstream.ToArray();
            }

            var messageLengthAs32Bits = Convert.ToInt32(bytesToEncrypt.Length);
            var messageLength = BitConverter.GetBytes(messageLengthAs32Bits);

            encrypted = encrypted.Prepend(ivSeed);
            encrypted = encrypted.Prepend(messageLength);

            return encrypted;
        }

        public static uint CompressAndEncryptAES(Stream fileToEncrypt, string SaveFolder, string outputGuid, string password)
        {
            uint CRC = 0;
            byte[] ivSeed = GenerateRandomSeed();

            var rfc = new Rfc2898DeriveBytes(password, ivSeed);
            byte[] Key = rfc.GetBytes(16);
            byte[] IV = rfc.GetBytes(16);
            string outputFile = $"{SaveFolder}\\{outputGuid}.proto";
            using (FileStream cryptFile = new FileStream(outputFile, FileMode.Create))
            {
                var messageLengthAs32Bits = Convert.ToInt32(fileToEncrypt.Length);
                var messageLength = BitConverter.GetBytes(messageLengthAs32Bits);

                cryptFile.Write(messageLength, 0, messageLength.Length);
                cryptFile.Write(ivSeed, 0, ivSeed.Length);

                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                using (CryptoStream cryptoStream = new CryptoStream(cryptFile, aesProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                using (var compressor = new DeflateStream(cryptoStream, CompressionMode.Compress))
                {
                    byte[] buff = new byte[4096];
                    int read = 0;
                    while ((read = fileToEncrypt.Read(buff, 0, buff.Length)) != 0)
                    {
                        compressor.Write(buff, 0, read);
                        CRC = Crc32C.Crc32CAlgorithm.Append(CRC, buff, 0, read);
                    }
                }
            }

            return CRC;
        }

        public static void DecompressAndDecryptAES(Stream fileToDecrypt, string outputFilePath, string password)
        {
            byte[] ReadBuff = new byte[4096];
            if (fileToDecrypt.Read(ReadBuff, 0, 20) != 20)
            {
                throw new Exception("file too small");
            }


            //var Crc = BitConverter.ToUInt32(ReadBuff, 0);
            var length = BitConverter.ToInt32(ReadBuff.Take(4).ToArray(), 0);
            var rfc = new Rfc2898DeriveBytes(password, ReadBuff.Skip(4).Take(16).ToArray());


            byte[] Key = rfc.GetBytes(16);
            byte[] IV = rfc.GetBytes(16);
            Directory.CreateDirectory(outputFilePath.Substring(0, outputFilePath.LastIndexOf('\\')));
            using (FileStream outputFile = new FileStream(outputFilePath, FileMode.Create))
            {
                if (length > 0)
                {
                    using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider() { Padding = PaddingMode.None })
                    using (CryptoStream cryptoStream = new CryptoStream(fileToDecrypt, aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                    using (DeflateStream decompressor = new DeflateStream(cryptoStream, CompressionMode.Decompress))
                    {
                        int read = 0;
                        while ((read = decompressor.Read(ReadBuff, 0, ReadBuff.Length)) != 0)
                        {
                            outputFile.Write(ReadBuff, 0, read);
                        }
                    }
                }
            }
        }


        public static byte[] DecryptAES(byte[] bytesToDecrypt, string password)
        {
            (byte[] messageLengthAs32Bits, byte[] bytesWithIv) = bytesToDecrypt.Shift(4); // get the message length
            (byte[] ivSeed, byte[] encrypted) = bytesWithIv.Shift(16);                    // get the initialization vector

            var length = BitConverter.ToInt32(messageLengthAs32Bits, 0);

            var rfc = new Rfc2898DeriveBytes(password, ivSeed);
            byte[] Key = rfc.GetBytes(16);
            byte[] IV = rfc.GetBytes(16);

            using (MemoryStream mStream = new MemoryStream(encrypted))
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider() { Padding = PaddingMode.None })
            using (CryptoStream cryptoStream = new CryptoStream(mStream, aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
            {
                cryptoStream.Read(encrypted, 0, length);
                return mStream.ToArray().Take(length).ToArray();
            }
        }
    }
}
