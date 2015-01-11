using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Slickflow.Engine.Business.Entity;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using System.Reflection;
namespace Slickflow.MvcDemo.Models
{
    public class LeaveViewModel
    {
        [Required(ErrorMessage = "请选择请假类型")]
        [Display(Name = "请假类型")]
        public int LeaveType { get; set; }

        [Display(Name = "请假天数")]
        [Required(ErrorMessage = "请输入请假天数")]
        //验证缺失,可自行补上
        public int Days { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        [Required(ErrorMessage = "开始时间格式错误/为空")]
        [DataType(DataType.DateTime)]
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        [Required(ErrorMessage = "结束时间格式错误/为空")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        /// <summary>
        ///流程GUID
        /// </summary>
        public string ProcessGUID { get; set; }
    }
    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    //public class DateCompare : ValidationAttribute
    //{

    //    public DateCompare(string otherProperty)
    //    {
    //        if (otherProperty == null)
    //        {
    //            throw new ArgumentNullException("otherProperty");
    //        }
    //        this.OtherProperty = otherProperty;
    //    }


    //    public override string FormatErrorMessage(string name)
    //    {
    //        object[] args = new object[] { name, this.OtherPropertyDisplayName ?? this.OtherProperty };
    //        return string.Format(CultureInfo.CurrentCulture, base.ErrorMessageString, args);
    //    }

    //    private static string GetDisplayNameForProperty(Type containerType, string propertyName)
    //    {
    //        PropertyDescriptor descriptor2 = GetTypeDescriptor(containerType).GetProperties().Find(propertyName, true);
    //        if (descriptor2 == null)
    //        {
    //            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "未知错误", new object[] { containerType.FullName, propertyName }));
    //        }
    //        IEnumerable<Attribute> source = descriptor2.Attributes.Cast<Attribute>();
    //        DisplayAttribute attribute = source.OfType<DisplayAttribute>().FirstOrDefault<DisplayAttribute>();
    //        if (attribute != null)
    //        {
    //            return attribute.GetName();
    //        }
    //        DisplayNameAttribute attribute2 = source.OfType<DisplayNameAttribute>().FirstOrDefault<DisplayNameAttribute>();
    //        if (attribute2 != null)
    //        {
    //            return attribute2.DisplayName;
    //        }
    //        return propertyName;
    //    }

    //    private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
    //    {
    //        return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
    //    }


    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        PropertyInfo property = validationContext.ObjectType.GetProperty(this.OtherProperty);
    //        if (property == null)
    //        {
    //            return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "未知错误", new object[] { this.OtherProperty }));
    //        }
    //        object objB = property.GetValue(validationContext.ObjectInstance, null);
    //        //作比较
    //        if (Convert.ToDateTime(value) < Convert.ToDateTime(objB))
    //        {
    //            return null;
    //        }

    //        if (this.OtherPropertyDisplayName == null)
    //        {
    //            this.OtherPropertyDisplayName = GetDisplayNameForProperty(validationContext.ObjectType, this.OtherProperty);
    //        }

    //        return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
    //    }
    //    public string OtherProperty
    //    {
    //        get;
    //        set;
    //    }
    //    public string OtherPropertyDisplayName
    //    {
    //        get;
    //        set;
    //    }
    //    public override bool RequiresValidationContext
    //    {
    //        get
    //        {
    //            return true;
    //        }
    //    }
    //}
}