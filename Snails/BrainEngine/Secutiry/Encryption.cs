using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
#if WINDOWS
using System.Security.Cryptography;
#endif

namespace TwoBrainsGames.BrainEngine.Secutiry
{
    public class Encryption
    {

        /// <summary>
        /// 
        /// </summary>
        public static byte[] Encrypt(Stream stream, string key)
        {
            byte[] streamBytes = new byte[stream.Length];
            int bytesRead = 0;
            int len;
            while ((len = stream.Read(streamBytes, bytesRead, (int)Math.Min((long)1024, (long)(stream.Length - bytesRead)))) > 0)
            {
                bytesRead += len;
            }
            return Encryption.Encrypt(streamBytes, key);
        }

        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra security</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] toEncrypt, string key)
        {
#if FALSE
            byte[] keyArray;

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);
            tdes.Clear();
            return resultArray;
#else
            return toEncrypt;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static byte[] Decrypt(Stream cryptedScream, string key)
        {
            MemoryStream tempStream = new MemoryStream();
            int len;
            byte[] buffer = new byte[1024];
            while ((len = cryptedScream.Read(buffer, 0, 1024)) > 0)
            {
                tempStream.Write(buffer, 0, len);
            }
            tempStream.Seek(0, SeekOrigin.Begin);
            byte[] streamBytes = new byte[tempStream.Length];
            int bytesRead = 0;
            while ((len = tempStream.Read(streamBytes, bytesRead, (int)Math.Min((long)1024, (long)(tempStream.Length - bytesRead)))) > 0)
            {
                bytesRead += len;
            }
#if FALSE
            return Encryption.Decrypt(streamBytes, key);
#else
            return streamBytes;
#endif
        }

        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] cryptedData, string key)
        {
#if FALSE
            byte[] keyArray;

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(cryptedData, 0, cryptedData.Length);

            tdes.Clear();
            return resultArray;
#else
            return cryptedData;
#endif
        }
    }
}
