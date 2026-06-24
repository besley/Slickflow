using System;
using System.Text.RegularExpressions;

namespace Slickflow.Module.External.Utility
{
    /// <summary>
    /// Extracts contact info (name, mobile, telephone, wechat, email) from customer reply text using regex.
    /// </summary>
    public static class ContactExtractorUtility
    {
        // Chinese mobile: 11 digits, 1[3-9]xxxxxxxxx (with optional keyword prefix)
        private static readonly Regex MobileRegex = new Regex(
            @"(?:手机|手机号|电话|联系方式|手机号码)[：:\s]*(1[3-9]\d{9})|(1[3-9]\d{9})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Landline: area code-number or digits only
        private static readonly Regex TelephoneRegex = new Regex(
            @"(?:电话|座机|固话)[：:\s]*(\d{3,4}[-\s]?\d{7,8}|\d{7,8})|0\d{2,3}[-\s]?\d{7,8}",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // WeChat: keyword + ID, or common wechat id format (alphanumeric, 6-20 chars)
        private static readonly Regex WechatRegex = new Regex(
            @"(?:微信|微信号|wechat)[：:\s]*([a-zA-Z][a-zA-Z0-9_\-]{5,19}|[a-zA-Z0-9_\-]{6,20})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Email
        private static readonly Regex EmailRegex = new Regex(
            @"(?:邮箱|邮件|email)[：:\s]*([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+)|[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Name/salutation: e.g. "称呼/姓名/我叫" + name
        private static readonly Regex NameRegex = new Regex(
            @"(?:称呼|姓名|名字|我是|我叫|我姓)[：:\s]*([^\s,，。.；;!！?？\r\n]{2,20})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Salutation: e.g. "王先生", "陈女士", "李小姐" (surname + 先生/女士/小姐); maps to customer name field
        private static readonly Regex SalutationRegex = new Regex(
            @"([\u4e00-\u9fa5]{1,2})(先生|女士|小姐)\b",
            RegexOptions.Compiled);

        /// <summary>
        /// Extracts 11-digit mobile number from text (prefers number after "mobile" keyword).
        /// </summary>
        public static string ExtractMobile(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            var m = MobileRegex.Match(text);
            if (!m.Success) return null;
            if (m.Groups[1].Success && !string.IsNullOrWhiteSpace(m.Groups[1].Value))
                return m.Groups[1].Value.Trim();
            if (m.Groups[2].Success && !string.IsNullOrWhiteSpace(m.Groups[2].Value))
                return m.Groups[2].Value.Trim();
            var digits = Regex.Replace(m.Value, @"\D", "");
            return digits.Length >= 11 ? digits.Substring(digits.Length - 11, 11) : null;
        }

        /// <summary>
        /// Extracts landline telephone from text.
        /// </summary>
        public static string ExtractTelephone(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            var m = TelephoneRegex.Match(text);
            if (!m.Success) return null;
            if (m.Groups.Count > 1 && !string.IsNullOrWhiteSpace(m.Groups[1].Value))
                return m.Groups[1].Value.Trim();
            return m.Value.Trim();
        }

        /// <summary>
        /// Extracts WeChat ID from text.
        /// </summary>
        public static string ExtractWechat(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            var m = WechatRegex.Match(text);
            if (!m.Success) return null;
            return m.Groups.Count > 1 ? m.Groups[1].Value.Trim() : m.Value.Trim();
        }

        /// <summary>
        /// Extracts email address from text.
        /// </summary>
        public static string ExtractEmail(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            var m = EmailRegex.Match(text);
            if (!m.Success) return null;
            if (m.Groups.Count > 1 && !string.IsNullOrWhiteSpace(m.Groups[1].Value))
                return m.Groups[1].Value.Trim();
            return m.Value.Trim();
        }

        /// <summary>
        /// Extracts customer name/salutation from text.
        /// Tries name after keyword (e.g. "I am"/"name"/"call me") first; then salutation pattern (surname + Mr./Ms./Miss in Chinese) for customer name field.
        /// </summary>
        public static string ExtractName(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            var m = NameRegex.Match(text);
            if (m.Success && m.Groups.Count > 1 && !string.IsNullOrWhiteSpace(m.Groups[1].Value))
                return m.Groups[1].Value.Trim();
            var sm = SalutationRegex.Match(text);
            if (sm.Success && sm.Groups.Count >= 3)
                return (sm.Groups[1].Value + sm.Groups[2].Value).Trim();
            return null;
        }

        /// <summary>
        /// Extracts all contact fields from customer reply text in one call.
        /// </summary>
        /// <param name="userMessage">Process variable user_message (customer reply content).</param>
        /// <returns>Name, mobile, telephone, wechat, email; unmatched fields are null.</returns>
        public static ContactInfo ExtractAll(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return new ContactInfo();

            return new ContactInfo
            {
                Name = ExtractName(userMessage),
                Mobile = ExtractMobile(userMessage),
                Telephone = ExtractTelephone(userMessage),
                Wechat = ExtractWechat(userMessage),
                Email = ExtractEmail(userMessage)
            };
        }
    }

    /// <summary>
    /// Customer contact info (result of extraction from user_message).
    /// </summary>
    public class ContactInfo
    {
        /// <summary>Customer name/salutation; maps to biz_customer.name.</summary>
        public string Name { get; set; }
        /// <summary>Mobile number; maps to biz_customer.phone_number (used with or instead of Telephone).</summary>
        public string Mobile { get; set; }
        /// <summary>Landline telephone.</summary>
        public string Telephone { get; set; }
        /// <summary>WeChat ID.</summary>
        public string Wechat { get; set; }
        /// <summary>Email address.</summary>
        public string Email { get; set; }
    }
}
