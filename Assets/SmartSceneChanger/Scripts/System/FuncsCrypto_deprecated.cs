using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Static Functions for Crypto (deprecated)
    /// </summary>
    public partial class Funcs
    {


        /// <summary>
        /// Encrypt text data, password length must be 16, 24, or 32
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="value">password</param>
        /// <returns>encrypted binary data</returns>
        [Obsolete("This function is old, please use EncryptTextData2 instead.", false)]
        public static byte[] EncryptTextData(string text, string value = "12345678901234567890123456789012")
        {
            return EncryptBinaryData(UTF8Encoding.UTF8.GetBytes(text), value);
        }

        /// <summary>
        /// Decrypt text data, password length must be 16, 24, or 32
        /// </summary>
        /// <param name="data">encrypted data</param>
        /// <param name="value">password</param>
        /// <returns>decrypted text</returns>
        [Obsolete("This function is old, please use DecryptBinaryDataToText2 instead.", false)]
        public static string DecryptBinaryDataToText(byte[] data, string value = "12345678901234567890123456789012")
        {

            try
            {
                byte[] bytes = DecryptBinaryData(data, value);
                return UTF8Encoding.UTF8.GetString(bytes);
            }

            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return "";

        }

        /// <summary>
        /// Encrypt binary data, password length must be 16, 24, or 32
        /// </summary>
        /// <param name="data">binary data</param>
        /// <param name="value">password</param>
        /// <returns>encrypted binary data</returns>
        [Obsolete("This function is old, please use EncryptBinaryData2 instead.", false)]
        public static byte[] EncryptBinaryData(byte[] data, string value = "12345678901234567890123456789012")
        {

#if UNITY_EDITOR

            if (value == "12345678901234567890123456789012")
            {
                Debug.LogWarning("You should not use default password " + value);
            }

#endif

            try
            {

                RijndaelManaged rm = new RijndaelManaged();
                rm.Key = ASCIIEncoding.UTF8.GetBytes(value);
                rm.Padding = PaddingMode.PKCS7;
                rm.Mode = CipherMode.ECB;

                ICryptoTransform encryptor = rm.CreateEncryptor();
                return encryptor.TransformFinalBlock(data, 0, data.Length);

            }

            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return new byte[] { };

        }

        /// <summary>
        /// Decrypt binary data, password length must be 16, 24, or 32
        /// </summary>
        /// <param name="data">encrypted binary data</param>
        /// <param name="value">password</param>
        /// <returns>decrypted binary data</returns>
        [Obsolete("This function is old, please use DecryptBinaryData2 instead.", false)]
        public static byte[] DecryptBinaryData(byte[] data, string value = "12345678901234567890123456789012")
        {

            try
            {

                RijndaelManaged rm = new RijndaelManaged();
                rm.Key = ASCIIEncoding.UTF8.GetBytes(value);
                rm.Padding = PaddingMode.PKCS7;
                rm.Mode = CipherMode.ECB;

                ICryptoTransform decryptor = rm.CreateDecryptor();
                return decryptor.TransformFinalBlock(data, 0, data.Length);

            }

            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return new byte[] { };

        }

    }

}
