using Microsoft.AspNetCore.Mvc;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Utility;
using Slickflow.WebUtility;

namespace sfdapi.Controllers
{
    /// <summary>
    /// Language controller
    /// 界面语言控制器
    /// </summary>
    public class LanguageController : Controller
    {
        #region Langauge Set
        /// <summary>
        /// set language
        /// </summary>
        [HttpGet]
        public ResponseResult SetLang(string id)
        {
            var result = ResponseResult.Default();
            try
            {
                LangTypeEnum lang = EnumHelper.ParseEnum<LangTypeEnum>(id);
                LocalizeHelper.SetLang(lang);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("languagecontroller.setlang.error", ex.Message));
            }
            return result;
        }
        #endregion
    }
}