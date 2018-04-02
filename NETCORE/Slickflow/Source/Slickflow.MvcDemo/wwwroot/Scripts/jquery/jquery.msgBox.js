/*
jQuery.msgBox plugin
Version: 0.1.1 (trying to follow http://semver.org/)
Code repository: https://github.com/dotCtor/jQuery.msgBox

Copyright (c) 2011-2013 Halil İbrahim Kalyoncu and Oliver Kopp
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/

// users may change this variable to fit their needs
var msgBoxImagePath = "content/jquery-ui/images/";

jQuery.msgBox = msg;
function msg (options) {
    var isShown = false;
    var typeOfValue = typeof options;
    var defaults = {
        content: (typeOfValue == "string" ? options : "Message"),
        title: "Warning",
        type: "alert",
        autoClose: false,
        timeOut: 0,
        modal: false,
        showButtons: true,
        buttons: [{ value: "Ok"}],
        inputs: [{ type: "text", name:"userName", header: "User Name" }, { type: "password",name:"password", header: "Password"}],
        success: function (result) { },
        beforeShow: function () { },
        afterShow: function () { },
        beforeClose: function () { },
        afterClose: function () { },
        opacity: 0.1
    };
    options = typeOfValue == "string" ? defaults : options;
    if (options.type != null) {
        switch (options.type) {
            case "alert":
                options.title = options.title == null ? "Warning" : options.title;
                break;
            case "info":
                options.title = options.title == null ? "Information" : options.title;
                break;
            case "error":
                options.title = options.title == null ? "Error" : options.title;
                break;
            case "confirm":
                options.title = options.title == null ? "Confirmation" : options.title;
                options.buttons = options.buttons == null ? [{ value: "Yes" }, { value: "No" }, { value: "Cancel"}] : options.buttons;
                break;
            case "prompt":
                options.title = options.title == null ? "Log In" : options.title;
                options.buttons = options.buttons == null ? [{ value: "Login" }, { value: "Cancel"}] : options.buttons;
                break;
            default:
                image = "alert.png";
        }
    }
    options.timeOut = options.timeOut == null ? (options.content == null ? 500 : options.content.length * 70) : options.timeOut;
    options = $.extend({}, defaults, options);
    if (options.autoClose) {
        setTimeout(hide, options.timeOut);
    }
    var image = "";
    switch (options.type) {
        case "alert":
            image = "alert.png";
            break;
        case "info":
            image = "info.png";
            break;
        case "error":
            image = "error.png";
            break;
        case "confirm":
            image = "confirm.png";
            break;
        default:
            image = "alert.png";
    }
    
    var divId = "msgBox" + new Date().getTime();
    
    /* i was testing with ($.browser.msie  && parseInt($.browser.version, 10) === 7) but $.browser.msie is not working with jQuery 1.9.0 :S. Alternative method: */
    if ( navigator.userAgent.match(/msie 7/i) !== null) { var divMsgBoxContentClass = "msgBoxContentIEOld"; } else { var divMsgBoxContentClass = "msgBoxContent";}
    
    var divMsgBoxId = divId; 
    var divMsgBoxContentId = divId+"Content"; 
    var divMsgBoxImageId = divId+"Image";
    var divMsgBoxButtonsId = divId+"Buttons";
    var divMsgBoxBackGroundId = divId+"BackGround";
	var firstButtonId = divId+"FirstButton";
    
    var buttons = "";
	var isFirstButton = true;
    $(options.buttons).each(function (index, button) {
        var add = "";
        if (isFirstButton) {
            add = ' id="' + firstButtonId + '"';
            isFirstButton = false;
        }
        buttons += "<input class=\"msgButton\" type=\"button\" name=\"" + button.value + "\" value=\"" + button.value + "\"" + add + "/>";
    });

    var inputs = "";
    $(options.inputs).each(function (index, input) {
        var type = input.type;
        if (type=="checkbox" || type =="radiobutton") {
            inputs += "<div class=\"msgInput\">" +
            "<input type=\"" + input.type + "\" name=\"" + input.name + "\" "+(input.checked == null ? "" : "checked ='"+input.checked+"'")+" value=\"" + (typeof input.value == "undefined" ? "" : input.value) + "\" />" +
            "<text>"+input.header +"</text>"+
            "</div>";
        }
        else {
            inputs += "<div class=\"msgInput\">" +
            "<span class=\"msgInputHeader\">" + input.header + "</span>" +
            "<input type=\"" + input.type + "\" name=\"" + input.name + "\" value=\"" + (typeof input.value == "undefined" ? "" : input.value) + "\" "+
            (typeof input.size!==undefined?" size='"+input.size+"' ":"")+
            (typeof input.maxlength!==undefined?" maxlength='"+input.maxlength+"' ":"")+
            " />" +
            "</div>";
        }
    });

    var divBackGround = "<div id=\"" + divMsgBoxBackGroundId + "\" class=\"msgBoxBackGround\"></div>";
    var divTitle = "<div class=\"msgBoxTitle\">" + options.title + "</div>";
    var divContainer = "<div class=\"msgBoxContainer\"><div id=\"" + divMsgBoxImageId + "\" class=\"msgBoxImage\"><img src=\"" + msgBoxImagePath + image + "\"/></div><div id=\"" + divMsgBoxContentId + "\" class=\"" + divMsgBoxContentClass + "\"><p><span>" + options.content + "</span></p></div></div>";
    var divButtons = "<div id=\"" + divMsgBoxButtonsId + "\" class=\"msgBoxButtons\">" + buttons + "</div>";
    var divInputs = "<div class=\"msgBoxInputs\">" + inputs + "</div>";

    var divMsgBox; 
    var divMsgBoxContent; 
    var divMsgBoxImage;
    var divMsgBoxButtons;
    var divMsgBoxBackGround;
    
    if (options.type == "prompt") {
        $("body").append(divBackGround + "<div id=\"" + divMsgBoxId + "\" class=\"msgBox\">" + divTitle + "<div>" + divContainer + (options.showButtons ? divButtons + "</div>" : "</div>") + "</div>");
        divMsgBox = $("#"+divMsgBoxId); 
        divMsgBoxContent = $("#"+divMsgBoxContentId); 
        divMsgBoxImage = $("#"+divMsgBoxImageId);
        divMsgBoxButtons = $("#"+divMsgBoxButtonsId);
        divMsgBoxBackGround = $("#"+divMsgBoxBackGroundId);

        divMsgBoxImage.remove();
        divMsgBoxButtons.css({"text-align":"center","margin-top":"5px"});
        divMsgBoxContent.css({"width":"100%","height":"100%"});
        divMsgBoxContent.html(divInputs);
    }
    else {
        $("body").append(divBackGround + "<div id=\"" + divMsgBoxId + "\" class=\"msgBox\">" + divTitle + "<div>" + divContainer + (options.showButtons ? divButtons + "</div>" : "</div>") + "</div>");
        divMsgBox= $("#"+divMsgBoxId); 
        divMsgBoxContent = $("#"+divMsgBoxContentId); 
        divMsgBoxImage = $("#"+divMsgBoxImageId);
        divMsgBoxButtons = $("#"+divMsgBoxButtonsId);
        divMsgBoxBackGround = $("#"+divMsgBoxBackGroundId);
    }
    divMsgBoxContent.height('auto');

    var width = divMsgBox.width();
    var height = divMsgBox.height();
    var windowHeight = $(window).height();
    var windowWidth = $(window).width();

    var top = windowHeight / 2 - height / 2;
    var left = windowWidth / 2 - width / 2;

    show();

    function show() {
        if (isShown) {
            return;
        }
        divMsgBox.css({ opacity: 0, top: top - 50, left: left });
        divMsgBox.css({ zIndex: 9999});
        divMsgBox.css("background-image", "url('"+msgBoxImagePath+"msgBoxBackGround.png')");
        divMsgBoxBackGround.css({ opacity: options.opacity });
        options.beforeShow();
        divMsgBoxBackGround.css({ "width": $(document).width(), "height": getDocHeight() });
        $(divMsgBoxId+","+divMsgBoxBackGroundId).fadeIn(0);
        divMsgBox.animate({ opacity: 1, "top": top, "left": left }, 200);
        setTimeout(options.afterShow, 200);
        $("#" + firstButtonId).focus();
        isShown = true;
        $(window).bind("resize", function (e) {
            var width = divMsgBox.width();
            var height = divMsgBox.height();
            var windowHeight = $(window).height();
            var windowWidth = $(window).width();

            var top = windowHeight / 2 - height / 2;
            var left = windowWidth / 2 - width / 2;

            divMsgBox.css({ "top": top, "left": left });
            divMsgBoxBackGround.css({"width": "100%", "height": "100%"});
        });
    }

    function hide() {
        if (!isShown) {
            return;
        }
        options.beforeClose();
        divMsgBox.animate({ opacity: 0, "top": top - 50, "left": left }, 200);
        divMsgBoxBackGround.fadeOut(300);
        setTimeout(function () { divMsgBox.remove(); divMsgBoxBackGround.remove(); }, 300);
        setTimeout(options.afterClose, 300);
        $(window).unbind("resize");
        isShown = false;
    }

    function getDocHeight() {
        var D = document;
        return Math.max(
        Math.max(D.body.scrollHeight, D.documentElement.scrollHeight),
        Math.max(D.body.offsetHeight, D.documentElement.offsetHeight),
        Math.max(D.body.clientHeight, D.documentElement.clientHeight));
    }

    function getFocus() {
    	divMsgBox.fadeOut(200).fadeIn(200);
    }

    $("input.msgButton").click(function (e) {
        e.preventDefault();
        var value = $(this).val();
        if (options.type != "prompt") {
            options.success(value);
        }
        else {
            var inputValues = [];
            $("div.msgInput input").each(function (index, domEle) {
                var name = $(this).attr("name");
                var value = $(this).val();
                var type = $(this).attr("type");
                if (type == "checkbox" || type == "radiobutton") {
                    inputValues.push({ name: name, value: value,checked: $(this).attr("checked")});
                }
                else {
                    inputValues.push({ name: name, value: value });
                }
            });
            options.success(value,inputValues);
        }
        hide();
    });

    divMsgBoxBackGround.click(function (e) {
        if ( options.modal )
            return;
        if (!options.showButtons || (options.showButtons && options.buttons.length<2) || options.autoClose) {
            hide();
        }
        else {
            getFocus();
        }
    });
};
