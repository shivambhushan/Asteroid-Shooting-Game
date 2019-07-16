using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Static Functions
    /// </summary>
    public partial class Funcs
    {

        /// <summary>
        /// Temp StringBuilder
        /// </summary>
        static StringBuilder TempStringBuilder = new StringBuilder();

        /// <summary>
        /// DeepCopy
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="obj">obj</param>
        /// <returns>result</returns>
        public static T DeepCopy<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Copy stream
        /// </summary>
        /// <param name="src">src</param>
        /// <param name="dest">dest</param>
        /// <param name="length">read length</param>
        static void CopyTo(Stream src, Stream dest, int length = 4096)
        {
            byte[] bytes = new byte[length];

            int cnt = 0;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        /// <summary>
        /// Zip text
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>zipped data</returns>
        public static byte[] Zip(string text)
        {
            return Zip(ASCIIEncoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// Zip data
        /// </summary>
        /// <param name="bytes">data</param>
        /// <returns>zipped data</returns>
        public static byte[] Zip(byte[] bytes)
        {

            MemoryStream mso = null;

            using (MemoryStream msi = new MemoryStream(bytes))
            {
                using (mso = new MemoryStream())
                {
                    using (GZipStream gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        CopyTo(msi, gs);
                    }
                }
            }

            return mso.ToArray();

        }

        /// <summary>
        /// Unzip data and return text
        /// </summary>
        /// <param name="bytes">zipped data</param>
        /// <returns>unzipped text</returns>
        public static string UnzipToText(byte[] bytes)
        {
            return UTF8Encoding.UTF8.GetString(Unzip(bytes));
        }

        /// <summary>
        /// Unzip data
        /// </summary>
        /// <param name="bytes">zipped data</param>
        /// <returns>unzipped data</returns>
        public static byte[] Unzip(byte[] bytes)
        {

            MemoryStream mso = null;

            using (MemoryStream msi = new MemoryStream(bytes))
            {
                using (mso = new MemoryStream())
                {
                    using (GZipStream gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        CopyTo(gs, mso);
                    }
                }
            }

            return mso.ToArray();

        }

        /// <summary>
        /// Create hierachy path
        /// </summary>
        /// <param name="trans">Transform</param>
        /// <returns>path</returns>
        // -------------------------------------------------------------------------------------------
        public static string CreateHierarchyPath(Transform trans)
        {

            if (!trans)
            {
                return "";
            }

            // --------------------------

            TempStringBuilder.Length = 0;

            TempStringBuilder.Append("/" + trans.name);

            Transform temp = trans.parent;

            while (temp != null)
            {

                TempStringBuilder.Insert(0, "/" + temp.name);

                temp = temp.parent;
            }

            return TempStringBuilder.ToString();

        }

    }

}
