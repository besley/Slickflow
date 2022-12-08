import jshelper from '../script/jshelper.js'

const kresource = (function () {
    function kresource() {
       
    }

    kresource.mxCurrentLanguageJSON = null;
    kresource.MX_RESOURCE_LOCAL_LANGUAGE = "mx-resource-local-lang";
    kresource.IsIE = navigator.userAgent.indexOf('MSIE') >= 0;

    kresource.getLang = function () {
        //get user enviroment language
        var localLang = window.localStorage.getItem(kresource.MX_RESOURCE_LOCAL_LANGUAGE);
        if (localLang === null || localLang === "") {
            var lan = (kresource.IsIE) ? navigator.userLanguage : navigator.language;
            if (lan.substring(0, 2) !== "zh")
                window.localStorage.setItem(kresource.MX_RESOURCE_LOCAL_LANGUAGE, "en");
            else
                window.localStorage.setItem(kresource.MX_RESOURCE_LOCAL_LANGUAGE, "zh");
            localLang = window.localStorage.getItem(kresource.MX_RESOURCE_LOCAL_LANGUAGE);
        }
        return localLang;
    }

    kresource.setLang = function (lang) {
        window.localStorage.setItem(kresource.MX_RESOURCE_LOCAL_LANGUAGE, lang);
    }

    kresource.localize = function (path) {
        var lang = kresource.getLang();
        var fileName = 'resources/' + lang + ".json";
        if (path) fileName = path + fileName;

        jshelper.ajaxGet(fileName, null,
            function (json) {
                kresource.mxCurrentLanguageJSON = json;
                $(".lang").each(function (o) {
                    var key = $(this).attr("as");
                    $(this).text(json[key]);
                });
            });

        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Language/SetLang/' + lang, null,
            function (result) {
                var status = result.Status;
            }
        );
    }

    kresource.getItem = function (key) {
        var json = kresource.mxCurrentLanguageJSON;
        var text = json[key];
        return text;
    }
    
    return kresource;
})()

export default kresource;