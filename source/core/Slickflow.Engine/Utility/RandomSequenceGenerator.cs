using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// Random sequence generator
    /// 随机序列生成器
    /// https://www.c-sharpcorner.com/article/generating-random-number-and-string-in-C-Sharp/
    /// </summary>
    public class RandomSequenceGenerator
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Get Random Number
        /// 获取随机数字
        /// </summary>
        public static int GetRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Get Random Int 4
        /// 获得随机整数，默认4位整数
        /// </summary>
        public static int GetRandomInt4()
        {
            Random r = new Random();
            int value = r.Next(1000, 9999);
            return value;
        }

        /// <summary>
        /// Get Random String
        /// </summary>
        public static string GetRandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(1024);
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        /// <summary>
        /// Get Random Sequence
        /// </summary>
        public static string GetRandomSequece()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(GetRandomString(3, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(GetRandomNumber(100, 999));

            // 2-Letters upper case  
            passwordBuilder.Append(GetRandomString(2));
            return passwordBuilder.ToString();
        }
    }
}
