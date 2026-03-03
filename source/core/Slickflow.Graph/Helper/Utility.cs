using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Graph.Helper
{
    /// <summary>
    /// Utility Class
    /// </summary>
    public class Utility
    {
        private static Random random = new Random();

        /// <summary>
        /// Get Random String 
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>String</returns>
        public static string GetRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Get random internal
        /// 获得随机整数
        /// </summary>
        /// <returns></returns>
        public static int GetRandomInt()
        {
            Random r = new Random();
            int value = r.Next(1000, 9999);
            return value;
        }

        /// <summary>
        /// 首字母变大写
        /// First Letter to Upper
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
