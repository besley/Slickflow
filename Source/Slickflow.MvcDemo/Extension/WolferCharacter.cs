using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Slickflow.MvcDemo.Extension
{
    public class SurName
    {
        public int ID { get; set; }
        //复姓
        public string Name { get; set; }

        public SurName(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
    public class SurNameList : List<SurName>
    {
        private string[] input = {"欧阳","太史","端木","上官","司马",
                           "东方","独孤","南宫","万俟","闻人",
                           "夏侯","诸葛","尉迟","公羊","赫连",
                           "澹台","皇甫","宗政","濮阳","公冶",
                           "太叔","申屠","公孙","慕容","仲孙",
                           "钟离","长孙","宇文","司徒","鲜于",
                           "司空","闾丘","子车","亓官","司寇",
                           "巫马","公西","颛孙","壤驷","公良",
                           "宰父","谷梁","拓跋","夹谷","轩辕",
                           "令狐","段干","百里","呼延","东郭",
                           "漆雕", "乐正", "南门","羊舌","微生",
                           "公户","公玉","公仪","梁丘","公仲",
                           "公上","公门","公山","公坚","左丘",
                           "公伯","西门","公祖","第五","公乘",
                           "贯丘","公皙","南荣","东里","东宫",
                           "仲长","子书","子桑","即墨","达奚",
                           "褚师","吴铭" };
        public SurNameList()
        {
            new SurNameList(input);
        }
        public SurNameList(string[] collection)
        {
            this.input = collection;
        }
    }
    public static class WolferCharacter
    {
        public static string ToPetName(this string realName)
        {
            //传入字符串的长度
            int lengthCharacter = realName.Length;

            switch (lengthCharacter)
            {
                case 1:
                    return realName;
                case 2:
                    //截取字符串取第一个值
                    return realName.Substring(0, 1);
                case 3:
                    string realNameTwo = realName.Substring(0, 2);
                    if (IsExistCompoundSurname(realName))
                        return realNameTwo;
                    else
                        return realName.Substring(0, 1);
                default:
                    return realName.Substring(0, 2);
            }


        }


        public static bool IsExistCompoundSurname(string username)
        {
            bool isExist = false;
            if (username.Length == 2)
            {
                SurNameList surNameList = new SurNameList();
                Action<SurName> listAll = delegate (SurName sur)
                   {
                       if (sur.Name == username)
                       {
                           isExist = true;
                           return;
                       }
                   };
            }
            else
            {
                return isExist = false; ;
            }
            return isExist;
        }



    }
}