////*****************ajaxHelper.js
/***
 * HttpPost保存数据
 * @url 
 * @data json数据
 * @fn回调方法
 */
function doHttpClientGet(url, fn) {
    $.getJSON(url, fn);
}

function ajaxPost(url, data, fn) {
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
function ajaxPostSyn(url, data, _async, fn) {
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
function ajaxGet(url, data, fn) {
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
function ajaxGetSyn(url, data,_async, fn) {
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
function ajaxGetSynNoCache(url, data, _async, fn) {
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
function ajaxGetNoCache(url, data, fn) {
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
//function ajaxPostAny(url, data, fn) {

//    $.ajax({
//        url: url,
//        type: 'POST',
//        data: data,
//        dataType: 'json',
//        contentType: 'application/x-www-form-urlencoded',
//        success: fn
//    });
//}
//function ajaxPostA(url, data, fn) {
//    $.ajax({
//        url: url,
//        type: 'POST',
//        data: data,
//        dataType: 'json',
//        contentType: 'application/x-www-form-urlencoded',
//        success: fn
//    });
//}
function ajaxUpload(url, data, fn) {
    $.ajax({
        url: url,
        type: 'POST',
        data: data,
        dataType: 'json',
        contentType: 'multipart/form-data',
        success: fn
    });
}
function strToJson(str) {
    var json = eval('(' + str + ')');
    return json;
}

//字符串类型转换为Boolean类型
function parseBool(val) {
    if ((typeof val === "string" && (val.toLowerCase() === 'true' || val.toLowerCase() === 'yes')) || val === 1)
        return true;
    else if ((typeof val === "string" && (val.toLowerCase() === 'false' || val.toLowerCase() === 'no')) || val === 0)
        return false;

    return null;
}

//判断是否是数字类型，如果是返回true, 否则返回false
function isNumber(o) {
    return !isNaN(o - 0) && o !== null && o !== "" && o !== false;
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

//替换字符串中包含的全部匹配字串
String.prototype.replaceAll = function (stringToFind, stringToReplace) {
    var temp = this;
    var index = temp.indexOf(stringToFind);
    while (index != -1) {
        temp = temp.replace(stringToFind, stringToReplace);
        index = temp.indexOf(stringToFind);
    }
    return temp;
}

//添加cookie
function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    var c_domain = '.localhost';
    document.cookie = c_name + "=" + c_value + ";path=/" + ";domain=" + c_domain;
}

//删除cookie
function removeCookie(c_name) {
    //当设置过期时间为负数时，会清除cookie
    setCookie(c_name, '', -7);
}

function checkCookie() {
    var username = getCookie("username");
    if (username != null && username != "") {
        alert("Welcome again " + username);
    }
    else {
        username = prompt("Please enter your name:", "");
        if (username != null && username != "") {
            setCookie("username", username, 365);
        }
    }
}

//获取Url地址中的查询串的值
$.extend({
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getUrlVar: function (name) {
        return $.getUrlVars()[name];
    }
});

//function CreateXmlHttp() {
//    var xhrobj = false;
//    try {
//        xhrobj = new ActiveXObject("Msxml2.XMLHTTP"); //ie msxml3.0+
//    }
//    catch (e) {
//        try {
//            xhrobj = new ActiveXObject("Microsoft.XMLHTTP"); //ie msxml 2.6
//        } catch (e2) {
//            xhrobj = false;
//        }
//    }
//    if (!xhrobj && typeof XMLHttpRequest != 'undefined') { //firefox opera 8.0 safari
//        xhrobj = new XMLHttpRequest();
//    }
//    return xhrobj;
//}

//var xhr = CreateXmlHttp();
////window.onload = function () {
////    Get();
////}
//function Get() {
//    //1、设置请求方式、目标、是否异步
//    //1.1 Get方式
//    xhr.open("GET", "GetAreasByAjax.ashx?isAjax=1", true);
//    //===============如果是Post方式，请按下面的进行设置====================
//    //1.2 Post方式,如果是Post方式，还需要其他一些设置
//    xhr.open("POST", "GetAreasByAjax.aspx", true);
//    //1.2.1设置HTTP的输出内容类型为：application/x-www-form-urlencoded
//    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
//    //1.2.2设置浏览器不使用缓存，服务器不从缓存中找，重新执行代码，而且服务器返回给浏览器的时候，告诉浏览器也不要保存缓存。
//    xhr.setRequestHeader("If-Modified-Since", "0");

//    //2、设置回调函数
//    xhr.onreadystatechange = wacthing;  //wacthing是方法名

//    //3、发送请求
//    xhr.send(null); //GET方式
//    xhr.send("isAjax=1&na=123"); //POST方式
//}
//function ajaxPost(url, data, fn) {
//    xhr.open("POST", url, true);
//    xhr.setRequestHeader("Content-Type", "application/json;charset=utf-8");
//    xhr.setRequestHeader("If-Modified-Since", "0");
//    xhr.onreadystatechange =
//        ajax_callback(fn,xhr.responseText, xhr.status);

   
//    xhr.send(data);
//    //$.ajax({
//    //    url: url,
//    //    type: 'POST',
//    //    data: data,
//    //    dataType: 'json',
//    //    contentType: 'application/json;charset=utf-8',
//    //    success: fn
//    //});
//}
//  //回调函数
//function ajax_callback(fn) {
//    fn;
//    if (xhr.readyState == 4) {
//        if (xhr.status == 200) {
//            var res = fn;// xhr.reponseText; //获得服务器返回的字符串
//            alert(res)
//        }
//    }
//}