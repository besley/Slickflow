using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// 随机序列生成器
    /// https://www.c-sharpcorner.com/article/generating-random-number-and-string-in-C-Sharp/
    /// </summary>
    public class RandomSequenceGenerator
    {
        private readonly Random random = new Random();

        /// <summary>
        /// 获取随机数字
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int GetRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public string GetRandomString(int size, bool lowerCase = false)
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
        /// 获取字符串和数字的混合序列
        /// </summary>
        /// <returns></returns>
        public string GetRandomSequece()
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
