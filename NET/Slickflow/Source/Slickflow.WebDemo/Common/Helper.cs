using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Slickflow.WebDemo.Common
{
    public class Helper
    {
        /// <summary>
        /// 转换字符串为整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ConverToInt32(string value)
        {
            int v;
            if (!int.TryParse(value, out v))
            {
                v = 0;
            }
            return v;
        }

        /// <summary>
        /// 转换字符串为decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ConverToDecimal(string value)
        {
            decimal v;
            if (!decimal.TryParse(value, out v))
            {
                v = 0;
            }
            return v;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defaultValue">缺省值</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime ConvertToDateTime(string value, DateTime defaultValue)
        {
            if (!string.IsNullOrEmpty(value))
            {
                DateTime dateTime;
                if (DateTime.TryParse(value, out dateTime))
                    return dateTime;
            }
            return defaultValue;
        }

        /// <summary>
        /// 绑定DropDownList
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="dt"></param>
        /// <param name="textField"></param>
        /// <param name="valueField"></param>
        public static void BindDropDownList(DropDownList ddl, DataTable dt, string textField, string valueField)
        {
            BindDropDownList(ddl, dt, textField, valueField, false, "", "");
        }

        /// <summary>
        /// 绑定DropDownList
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="dt"></param>
        /// <param name="textField"></param>
        /// <param name="valueField"></param>
        public static void BindDropDownList(DropDownList ddl, DataTable dt, string textField, string valueField, bool isInsertDefaultItem, string defaultItemText, string defaultItemValue)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            if (isInsertDefaultItem)
                ddl.Items.Insert(0, new ListItem(defaultItemText, defaultItemValue));
        }
    }
}