const jshelper = (function () {
    function jshelper() {

    }

    //#region ajax 方法
    /***
     * HttpPost保存数据
     * @url 
     * @data json数据
     * @fn回调方法
     */
    jshelper.ajaxGet = function (url, data, fn) {
        $.ajax({
            url: url,
            type: 'GET',
            data: data,
            cache: false,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: fn
        });
    }

    jshelper.ajaxPost = function(url, data, fn) {
        $.ajax({
            url: url,
            type: 'POST',
            data: data,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: fn
        });
    }

    /***
     * HttpPost同步保存数据
     * @url 
     * @data json数据
     * @fn回调方法
     */
    jshelper.ajaxPostSyn = function(url, data, _async, fn) {
        $.ajax({
            url: url,
            type: 'POST',
            async: _async,//false同步
            data: data,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: fn
        });
    }

    jshelper.ajaxGetSyn = function(url, data, _async, fn) {
        $.ajax({
            url: url,
            type: 'GET',
            async: _async,//false
            data: data,
            cache: false,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: fn
        });
    }

    jshelper.ajaxGetSynNoCache = function(url, data, _async, fn) {
        $.ajax({
            url: url,
            type: 'GET',
            async: _async,//false
            data: data,
            cache: false,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: fn
        });
    }

    jshelper.ajaxGetNoCache = function(url, data, fn) {
        $.ajax({
            url: url,
            type: 'GET',
            data: data,
            cache: false,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: fn
        });
    }

    jshelper.ajaxUpload = function(url, data, fn) {
        $.ajax({
            url: url,
            type: 'POST',
            data: data,
            dataType: 'json',
            contentType: 'multipart/form-data',
            success: fn
        });
    }

    jshelper.strToJson = function(str) {
        var json = eval('(' + str + ')');
        return json;
    }
    //#endregion

    //字符串类型转换为Boolean类型
    jshelper.parseBool = function(val) {
        if ((typeof val === "string" && (val.toLowerCase() === 'true' || val.toLowerCase() === 'yes')) || val === 1)
            return true;
        else if ((typeof val === "string" && (val.toLowerCase() === 'false' || val.toLowerCase() === 'no')) || val === 0)
            return false;

        return null;
    }

    //判断是否是数字类型，如果是返回true, 否则返回false
    jshelper.isNumber = function(o) {
        return !isNaN(o - 0) && o !== null && o !== "" && o !== false;
    }

    jshelper.formatDate = function (timestamp) {
        var date = new Date(timestamp);
        var curr_date = date.getDate();
        var curr_month = date.getMonth();
        curr_month++;
        var curr_year = date.getFullYear();
        result = curr_year + "/" + curr_month + "/" + curr_date;
        return result;
    }

    jshelper.formatISODate = function (dateStringInRange) {
        var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s*$/,
        date = new Date(NaN), month,
        parts = isoExp.exec(dateStringInRange);

        if (parts) {
            month = +parts[2];
            date.setFullYear(parts[1], month - 1, parts[3]);
            if (month != date.getMonth() + 1) {
                date.setTime(NaN);
            }
        }
        return date;
    }

    jshelper.getSepeicalNum = function (n, base) {
        n = Number(n);
        if (n === 0 || !!(n && !(n % base))) {
            return n;
        } else {
            return (Math.floor(n / base) + 1) * base;
        }
    }

    //字符串转换为Integer
    Number.tryParseInt = function (str, defaultValue) {
        if (isNumber(str) == true) {
            return parseInt(str);
        }

        var retValue = defaultValue;
        if (str != null) {
            if (str.length > 0) {
                if (!isNaN(str)) {
                    retValue = parseInt(str);
                }
            }
        }
        return retValue;
    }

    //字符串转换为Float
    Number.tryParseFloat = function (str, defaultValue) {
        if (isNumber(str) == true) {
            return parseFloat(str);
        }

        var retValue = defaultValue;
        if (str != null) {
            if (str.length > 0) {
                if (!isNaN(str)) {
                    retValue = parseFloat(str);
                }
            }
        }
        return retValue;
    }

    jshelper.getRandomInt = function(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    //获取函数方法名
    jshelper.getFnName = function(fn) {
        return (fn.toString().match(/function (.+?)\(/) || [, ''])[1];
    }


    jshelper.getRandomString = function(len) {
        len = len || 32;
        var t = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
            a = t.length,
            n = "";
        for (i = 0; i < len; i++) n += t.charAt(Math.floor(Math.random() * a));
        return n
    }

    jshelper.getUUID = function () {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
        });
        return uuid;
    };

    jshelper.getTrueCoords = function (evt, svgRoot, trueCoords) {
        var newScale = svgRoot.currentScale;
        var translation = svgRoot.currentTranslate;

        trueCoords.x = (evt.clientX - translation.x) / newScale;
        trueCoords.y = (evt.clientY - translation.y) / newScale;

        return trueCoords;
    }

    //#region text selection function
    jshelper.isTextSelected = function (select_field) {
        //check txt length, if it is null
        var len = $(select_field).val().length;
        if (len == 0) {
            return true;
        }

        var selected = false;
        //IE  
        if (document.selection) {
            var sel = document.selection.createRange();
            if (sel.text.length > 0) {
                selected = true;
            }
        }
            //FF  
        else if (select_field.selectionStart || select_field.selectionStart == '0') {
            var startP = select_field.selectionStart;
            var endP = select_field.selectionEnd;
            if (startP != endP) {
                selected = true;
            }
        }
        return selected;
    }
    //#endregion

    var entityMap = {
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': '&quot;',
        "'": '&#39;',
        "/": '&#x2F;'
    };

    jshelper.escapeHtml = function(string) {
        return String(string).replace(/[&<>"'\/]/g, function (s) {
            return entityMap[s];
        });
    }

    jshelper.unescapeHTML = function(string) {
        return $("<div/>").html(string).text();
    }


    jshelper.replaceHTMLTags = function (data) {
        data = data.replace('&lt;', '<', 'gm')
                   .replace('&gt;', '>', 'gm');
        return data;
    }

    jshelper.getUrlParameter = function getUrlParameter(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
        return false;
    };

    return jshelper
})()

window.jshelper = jshelper;

export default jshelper
