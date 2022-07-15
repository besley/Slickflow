using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// MD5 签名的帮助类
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// 计算文件内容的MD5值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ComputeMD5(Stream stream)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                stream.Seek(0, SeekOrigin.Begin);
                byte[] hashBytesNew = md5.ComputeHash(stream);
                stream.Seek(0, SeekOrigin.Begin);

                // make a hex string of the hash for display or whatever
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytesNew)
                {
                    sb.Append(b.ToString("x2").ToLower());
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 计算文本类型的MD5值
        /// </summary>
        /// <param name="sDataIn"></param>
        /// <returns></returns>
        public static string GetMD5(string sDataIn)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }  
    }
}
