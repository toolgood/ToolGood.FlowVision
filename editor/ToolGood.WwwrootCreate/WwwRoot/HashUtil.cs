using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// HASH
    /// </summary>
    class HashUtil
    {
        #region MD5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string GetMd5String(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(buffer);
            md5.Dispose();
            return BitConverter.ToString(retVal).Replace("-", "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetMd5String(string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            var buffer = System.Text.Encoding.UTF8.GetBytes(text);
            return GetMd5String(buffer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] GetMd5Bytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(buffer);
            md5.Dispose();
            return retVal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] GetMd5Bytes(string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            var buffer = System.Text.Encoding.UTF8.GetBytes(text);
            return GetMd5Bytes(buffer);
        }
        #endregion
    }
}
