var jshelper;
if (!jshelper) jshelper = {};

(function () {
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

    //should use namespce to avoid conflict or something else...
    ////验证
    //function validateFloat(val) {//验证整、小数
    //    var patten = /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/;
    //    return patten.test(val);
    //}

    //function validateNum(val) {//验证整数
    //    var patten = /^-?\d+$/;
    //    return patten.test(val);
    //}

    //function validateNumOrLetter(val) {//只能输入数字和字母
    //    var patten = /^[A-Za-z0-9]+$/;
    //    return patten.test(val);
    //}

    //function validateNull(val) {//验证空
    //    return val.replace(/\s+/g, "").length == 0;
    //}

    //function validateDate(val) {//验证时间2010-10-10 //var patten = /^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/;
    //    var patten = /^(?:(?!0000)[0-9]{4}\/(?:(?:0[1-9]|1[0-2])\/(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])\/(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)$/;
    //    if (val == null || val == "") return true;
    //    var t = patten.test(val);
    //    if (!t) {
    //        var reg = /^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/;
    //        return reg.test(val);
    //    }
    //    return t;
    //}

    //function validateCH(val) {//验证汉字
    //    var patten = /^[\u4e00-\u9fa5],{0,}$/;
    //    return patten.test(val);
    //}

    //function validateEmail(val) {//验证Email
    //    var patten = /^(([A-Za-z0-9\-]+_+)|([A-Za-z0-9\-]+\-+)|([A-Za-z0-9\-]+\.+)|([A-Za-z0-9\-]+\++))*[A-Za-z0-9_\-]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$/;
    //    return patten.test(val);
    //}

    //function getQueryString(name) {
    //    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    //    var r = window.location.search.substr(1).match(reg);
    //    if (r != null) return unescape(r[2]); return null;
    //}

    ////替换字符串中包含的全部匹配字串
    //String.prototype.replaceAll = function (stringToFind, stringToReplace) {
    //    var temp = this;
    //    var index = temp.indexOf(stringToFind);
    //    while (index != -1) {
    //        temp = temp.replace(stringToFind, stringToReplace);
    //        index = temp.indexOf(stringToFind);
    //    }
    //    return temp;
    //}

    ////自定义lTrim()方法去除字串左侧杂质 
    //String.prototype.ltrim = function (Useless) {
    //    var regex = eval("/^" + Useless + "*/g"); return this.replace(regex, "");
    //}
    ////自定义rTrim()方法去除字串右侧杂质 
    //String.prototype.rtrim = function (Useless) {
    //    var regex = eval("/" + Useless + "*$/g"); return this.replace(regex, "");
    //}

    //Array.prototype.max = function (id) {
    //    var arr = [];
    //    if (!id)
    //        return Math.max.apply({}, this)
    //    else {
    //        for (var iii = 0; iii < this.length; iii++) {
    //            if (this[iii][id] != null)
    //                arr.push(this[iii][id]);
    //        }
    //        if (arr.length > 0)
    //            return Math.max.apply({}, arr);
    //        else
    //            return 0;
    //    }
    //}

    //Array.prototype.min = function (id) {
    //    var arr = [];
    //    if (!id)
    //        return Math.min.apply({}, this)
    //    else {
    //        for (var iii = 0; iii < this.length; iii++) {
    //            if (this[iii][id] != null)
    //                arr.push(this[iii][id]);
    //        }
    //        if (arr.length > 0)
    //            return Math.min.apply({}, arr);
    //        else
    //            return 0;
    //    }
    //}

    //Array.prototype.indexOf = function (substr, start) {
    //    if (!start)
    //        return;

    //    var ta, rt, d = '\0';
    //    if (start != null) { ta = this.slice(start); rt = start; } else { ta = this; rt = 0; }
    //    var str = d + ta.join(d) + d, t = str.indexOf(d + substr + d);
    //    if (t == -1) return -1; rt += str.slice(0, t).replace(/[^\0]/g, '').length;
    //    return rt;
    //}

    //Array.prototype.lastIndexOf = function (substr, start) {
    //    var ta, rt, d = '\0';
    //    if (start != null) { ta = this.slice(start); rt = start; } else { ta = this; rt = 0; }
    //    ta = ta.reverse(); var str = d + ta.join(d) + d, t = str.indexOf(d + substr + d);
    //    if (t == -1) return -1; rt += str.slice(t).replace(/[^\0]/g, '').length - 2;
    //    return rt;
    //}
    //Array.prototype.replace = function (reg, rpby) {
    //    var ta = this.slice(0), d = '\0';
    //    var str = ta.join(d); str = str.replace(reg, rpby);
    //    return str.split(d);
    //}

    //Array.prototype.remove = function (from, to) {
    //    var rest = this.slice((to || from) + 1 || this.length);
    //    this.length = from < 0 ? this.length + from : from;
    //    return this.push.apply(this, rest);
    //};

    //Array.prototype.search = function (reg) {
    //    var ta = this.slice(0), d = '\0', str = d + ta.join(d) + d, regstr = reg.toString();
    //    reg = new RegExp(regstr.replace(/\/((.|\n)+)\/.*/g, '\\0$1\\0'), regstr.slice(regstr.lastIndexOf('/') + 1));
    //    t = str.search(reg);
    //    if (t == -1)
    //        return -1;
    //    return str.slice(0, t).replace(/[^\0]/g, '').length;
    //}

    //// Original function by Alien51
    //Array.prototype.unique = function () {
    //    var arrVal = this;
    //    var uniqueArr = [];
    //    for (var i = arrVal.length; i--;) {
    //        var val = arrVal[i];
    //        if ($.inArray(val, uniqueArr) === -1) {
    //            uniqueArr.unshift(val);
    //        }
    //    }
    //    return uniqueArr;
    //}

    //Array.prototype.addUnique = function (val) {
    //    var arrVal = this;
    //    if ($.inArray(val, arrVal) === -1) {
    //        arrVal.push(val);
    //    }
    //}

    //function Int(value) {
    //    var _t = parseInt(value);
    //    if (isNaN(_t)) return 0; else return _t;
    //}
    //function Float(value) {
    //    var _t = parseFloat(value);
    //    if (isNaN(_t)) return 0; else return _t;
    //}
    //function Round(v, e) {
    //    var t = 1;
    //    for (; e > 0; t *= 10, e--);
    //    for (; e < 0; t /= 10, e++);
    //    return Math.round(v * t) / t;
    //}
    //function DateFormat(cellval) {
    //    try {
    //        var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    //        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    //        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    //        return date.getFullYear() + "-" + month + "-" + currentDate;
    //    } catch (e) {
    //        return "";
    //    }
    //}
    ///**
    // * 时间对象的格式化
    // */
    //Date.prototype.format = function (format) {
    //    /*
    //     * format="yyyy-MM-dd hh:mm:ss";
    //     */
    //    var o = {
    //        "M+": this.getMonth() + 1,
    //        "d+": this.getDate(),
    //        "h+": this.getHours(),
    //        "m+": this.getMinutes(),
    //        "s+": this.getSeconds(),
    //        "q+": Math.floor((this.getMonth() + 3) / 3),
    //        "S": this.getMilliseconds()
    //    }
    //    if (/(y+)/.test(format)) {
    //        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
    //            - RegExp.$1.length));
    //    }
    //    for (var k in o) {
    //        if (new RegExp("(" + k + ")").test(format)) {
    //            format = format.replace(RegExp.$1, RegExp.$1.length == 1
    //                ? o[k]
    //                : ("00" + o[k]).substr(("" + o[k]).length));
    //        }
    //    }
    //    return format;
    //}

    ////{"date":"3","day":"2","hours":"0","minutes":"0","month":"0","seconds":"0","time":"1325520000000","timezoneOffset":"-480","year":"112"}
    //function toDate(obj) {
    //    var date = new Date();
    //    date.setTime(obj.time);
    //    date.setHours(obj.hours);
    //    date.setMinutes(obj.minutes);
    //    date.setSeconds(obj.seconds);
    //    return date.format("yyyy-MM-dd hh:mm:ss");
    //}
    ////只要把DateTime值传递给ConvertJSONDateToJSDateObject就可以返回Date。/Date(1366106684184)/
    //function JsonDateToObj(jsondate) {
    //    var date = new Date(parseInt(jsondate.replace("/Date(", "").replace(")/", ""), 10));
    //    return date;
    //}
    //function getDate(jsondate) {
    //    var date = JsonDateToObj(jsondate);
    //    var year = date.getFullYear();
    //    var month = date.getMonth() + 1;
    //    var day = date.getDate();
    //    return year + "-" + month + "-" + day;
    //}
    ////如果想返回yyyy-MM-dd HH:mm:SS格式
    //function getDateTime(jsondate) {
    //    var date = JsonDateToObj(jsondate);
    //    var year = date.getFullYear();
    //    var month = date.getMonth() + 1;
    //    var day = date.getDate();
    //    var hh = date.getHours();
    //    var mm = date.getMinutes();
    //    var ss = date.getSeconds();
    //    return year + "-" + month + "-" + day + " " + hh + ":" + mm + ":" + ss;
    //}

    jshelper.getRandomInt = function(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    //获取函数方法名
    jshelper.getFnName = function(fn) {
        return (fn.toString().match(/function (.+?)\(/) || [, ''])[1];
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
})()

var Sys;
if (!Sys) Sys = {};
Sys = (function () {
    var tt = 7;
    var ie, firefox, chrome, opera, safari;
    var ua = navigator.userAgent.toLowerCase();
    var s;
    (s = ua.match(/msie ([\d.]+)/)) ? ie = s[1] :
    (s = ua.match(/firefox\/([\d.]+)/)) ? firefox = s[1] :
    (s = ua.match(/chrome\/([\d.]+)/)) ? chrome = s[1] :
    (s = ua.match(/opera.([\d.]+)/)) ? opera = s[1] :
    (s = ua.match(/version\/([\d.]+).*safari/)) ? safari = s[1] : 0;

    function Load(args) {
        parent.dialogFrame(args);
    }
    function Load2(args) {
        parent.dialogFrame2(args);
    }
    function showDiv(args) {
        parent.dialogDiv(args);
    }
    function alert(msg) {
        parent.showMessage(msg);
    }
    function aa() {
        alert("Sys.aa");
    }
    //this.aa = aa;
    return { Load: Load, Load2: Load2, showDiv: showDiv, alert: alert, aa: aa, ie: ie, firefox: firefox, chrome: chrome, opera: opera, safari: safari };
})();


