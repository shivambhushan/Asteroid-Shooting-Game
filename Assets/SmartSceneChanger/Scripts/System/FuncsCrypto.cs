using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Static Functions for Crypto
    /// </summary>
    public partial class Funcs
    {

        static int SaltSize = 8;
        static int RMBlockSize = 128;
        static int RMKeySize = 128;
        static CipherMode RMCipherMode = CipherMode.CBC;
        static PaddingMode RMPaddingMode = PaddingMode.PKCS7;

        static int Iterations = 1000;
        static int PasswordLength = 32;

#if UNITY_EDITOR

        /// <summary>
        /// Is crypto version deprecated
        /// </summary>
        /// <returns>deprecated</returns>
        // -----------------------------------------------------------------------------------------------------------
        public static bool IsCryptoVersionDeprecated(CryptoVersion cryptoVersion)
        {
            return (cryptoVersion == CryptoVersion.Ver1_depricated);
        }

#endif

        /// <summary>
        /// Encrypt text data
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="password">password</param>
        /// <returns>encrypted binary data</returns>
        // -----------------------------------------------------------------------------------------------------------
        public static byte[] EncryptTextData2(string text, string password)
        {
            return EncryptBinaryData2(UTF8Encoding.UTF8.GetBytes(text), password);
        }

        /// <summary>
        /// Encrypt binary data
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="password">password</param>
        /// <returns>encrypted binary data</returns>
        // -----------------------------------------------------------------------------------------------------------
        public static byte[] EncryptBinaryData2(byte[] data, string password)
        {

            List<byte> ret = new List<byte>();

            // ---------------------

            using (RijndaelManaged rm = new RijndaelManaged())
            {

                byte[] salt = new byte[SaltSize];

                // -----------------------

                // salt
                {
                    new RNGCryptoServiceProvider().GetBytes(salt);
                }

                // aes
                {

                    rm.BlockSize = RMBlockSize;
                    rm.KeySize = RMKeySize;
                    rm.Mode = RMCipherMode;
                    rm.Padding = RMPaddingMode;

                    rm.Key = (new Rfc2898DeriveBytes(password, salt, Iterations)).GetBytes(PasswordLength);

                }

                // ICryptoTransform
                {

                    using (ICryptoTransform encryptor = rm.CreateEncryptor(rm.Key, rm.IV))
                    {

                        ret.AddRange(salt);
                        ret.AddRange(rm.IV);
                        ret.AddRange(encryptor.TransformFinalBlock(data, 0, data.Length));

                    }

                }

            }

            return ret.ToArray();

        }

        /// <summary>
        /// Decrypt text data
        /// </summary>
        /// <param name="data">encrypted data</param>
        /// <param name="password">password</param>
        /// <returns>decrypted text</returns>
        // -----------------------------------------------------------------------------------------------------------
        public static string DecryptBinaryDataToText2(byte[] data, string password)
        {

            try
            {
                byte[] bytes = DecryptBinaryData2(data, password);
                return UTF8Encoding.UTF8.GetString(bytes);
            }

            catch (Exception e)
            {
                Debug.LogError("Error in Func.DecryptBinaryDataToText2 : " + e.Message);
            }

            return "";

        }

        /// <summary>
        /// Decrypt binary data
        /// </summary>
        /// <param name="data">encrypted data</param>
        /// <param name="password">password</param>
        /// <returns>decrypted binary data</returns>
        // -----------------------------------------------------------------------------------------------------------
        public static byte[] DecryptBinaryData2(byte[] data, string password)
        {

            int ivLength = RMBlockSize / 8;

            // ----------------------------

            if (data.Length < SaltSize + ivLength)
            {
                Debug.LogError("Invalid data in Func.DecryptBinaryData2");
                return new byte[0];
            }

            // ----------------------------

            byte[] salt = null;
            byte[] iv = null;
            byte[] trimmedData = null;

            byte[] ret = null;

            // ----------------------------

            // trim
            {

                List<byte> temp = new List<byte>(data);

                // salt
                {
                    salt = temp.GetRange(0, SaltSize).ToArray();
                    temp.RemoveRange(0, SaltSize);
                }

                // iv
                {
                    iv = temp.GetRange(0, ivLength).ToArray();
                    temp.RemoveRange(0, ivLength);
                }

                // trimmedData
                {
                    trimmedData = temp.ToArray();
                }

            }

            // ----------------------------

            try
            {

                using (RijndaelManaged rm = new RijndaelManaged())
                {

                    //rm.BlockSize = RMBlockSize;
                    //rm.KeySize = RMKeySize;
                    rm.Mode = RMCipherMode;
                    rm.Padding = RMPaddingMode;

                    rm.Key = (new Rfc2898DeriveBytes(password, salt, Iterations)).GetBytes(PasswordLength);

                    rm.IV = iv;

                    using (ICryptoTransform decryptor = rm.CreateDecryptor())
                    {
                        ret = decryptor.TransformFinalBlock(trimmedData, 0, trimmedData.Length);
                    }

                }

            }

#if UNITY_EDITOR

            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

#else

            catch
            {

            }

#endif
            return ret;

        }

        /// <summary>
        /// Create encrypted string
        /// </summary>
        /// <param name="str">data</param>
        /// <param name="password">password</param>
        /// <returns>encrypted string</returns>
        // -----------------------------------------------------------------------------------------------------------
        public static string CreateEncryptedText(string str, string password)
        {

            if(string.IsNullOrEmpty(str))
            {
                return "";
            }

            // ----------------

            byte[] temp = EncryptTextData2(str, password);

            return (temp != null) ? Convert.ToBase64String(temp) : "";

        }

        /// <summary>
        /// Resume encrypted string
        /// </summary>
        /// <param name="encryptedStr">data</param>
        /// <param name="password">password</param>
        /// <returns>decrypted string</returns>
        // -----------------------------------------------------------------------------------------------------------
        public static string ResumeEncryptedText(string encryptedStr, string password)
        {

            if (string.IsNullOrEmpty(encryptedStr))
            {
                return "";
            }

            // ----------------

            byte[] temp = Convert.FromBase64String(encryptedStr);

            return (temp != null) ? DecryptBinaryDataToText2(temp, password) : "";

        }

    }

}
