/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。

The Slickflow Designer project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

var kresource = (function () {
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
        mxLanguage = lang;
        if (mxClient) mxClient.language = lang;
        window.localStorage.setItem(kresource.MX_RESOURCE_LOCAL_LANGUAGE, lang);
    }

    kresource.localize = function () {
        var lang = kresource.getLang();
        jshelper.ajaxGet('Resources/' + lang + ".json", null,
            function (json) {
                kresource.mxCurrentLanguageJSON = json;
                $(".lang").each(function (o) {
                    var key = $(this).attr("as");
                    $(this).text(json[key]);
                });
            });

        jshelper.ajaxGet('Language/SetLang/' + lang, null,
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