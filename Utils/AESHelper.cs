using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class AESHelper
    {
        public static string AesEncrypt(string str,string key)
        {
            byte[] StrBytes = Encoding.UTF8.GetBytes(str);
            Aes a = GetAesTool(key);
            ICryptoTransform cTransform = a.CreateEncryptor();
            byte[] Result = cTransform.TransformFinalBlock(StrBytes, 0, StrBytes.Length);
            return Convert.ToBase64String(Result);
        }

        public static string AesDecrypt(string str,string key)
        {
            byte[] StrBytes = Encoding.UTF8.GetBytes(str);
            Aes a = GetAesTool(key);
            ICryptoTransform cTransform = a.CreateDecryptor();
            byte[] Result = cTransform.TransformFinalBlock(StrBytes, 0, StrBytes.Length);
            return Convert.ToBase64String(Result);
        }
        private static Aes GetAesTool(string key)
        {
            byte[] KeyBytes = GetAESKey(key);
            Aes a = Aes.Create();
            a.Key = KeyBytes;
            a.Mode = CipherMode.ECB;
            a.Padding = PaddingMode.PKCS7;
            return a;
        }
        private static byte[] GetAESKey(string Key)
        {
            byte[] KeyByte = Encoding.UTF8.GetBytes(Key);
            byte[] newArray = new byte[16];
            if (KeyByte.Length < 16)
            {
                for (int i = 0; i < newArray.Length; i++)
                {
                    if (i > KeyByte.Length - 1)
                    {
                        newArray[i] = 0;
                    }
                    else
                    {
                        newArray[i] = KeyByte[i];
                    }
                }
                return newArray;
            }
            else
            {
                return KeyByte;
            }
        }
    }
}
