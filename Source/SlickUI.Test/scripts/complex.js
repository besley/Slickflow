/**
 *	debugData
 *
 *	Pass me a data structure {} and I'll output all the key/value pairs - recursively
 *
 *	@example var HTML = debugData( oElem.style, "Element.style", { keys: "top,left,width,height", recurse: true, sort: true, display: true, returnHTML: true });	
 *
 *	@param Object	o_Data   A JSON-style data structure
 *	@param String	s_Title  Title for dialog (optional)
 *	@param Hash		options  Pass additional options in a hash
 */
//function debugData (o_Data, s_Title, options) {
//	options = options || {};
//	var
//		str=s_Title || 'DATA'
//	//	maintain backward compatibility with OLD 'recurseData' param
//	,	recurse=(typeof options=='boolean' ? options : options.recurse !==false)
//	,	keys=(options.keys?','+options.keys+',':false)
//	,	display=options.display !==false
//	,	html=options.returnHTML !==false
//	,	sort=options.sort !==false
//	,	D=[], i=0 // Array to hold data, i=counter
//	,	hasSubKeys = false
//	,	k, t, skip, x	// loop vars
//	;
//	if (o_Data.jquery) {
//		str=(s_Title ? s_Title+'\n':'')+'jQuery Collection ('+ o_Data.length +')\n    context="'+ o_Data.context +'"';
//	}
//	else if (o_Data.tagName && typeof o_Data.style == 'object') {
//		str=(s_Title ? s_Title+'\n':'')+o_Data.tagName;
//		var id = o_Data.id, cls=o_Data.className, src=o_Data.src, hrf=o_Data.href;
//		if (id)  str+='\n    id="'+		id+'"';
//		if (cls) str+='\n    class="'+	cls+'"';
//		if (src) str+='\n    src="'+	src+'"';
//		if (hrf) str+='\n    href="'+	hrf+'"';
//	}
//	else {
//		parse(o_Data,''); // recursive parsing
//		if (sort && !hasSubKeys) D.sort(); // sort by keyName - but NOT if has subKeys!
//		str+='\n***'+'****************************'.substr(0,str.length);
//		str+='\n'+ D.join('\n'); // add line-breaks
//	}
//	if (display) alert(str); // display data
//	if (html) str=str.replace(/\n/g, ' <br>').replace(/  /g, ' &nbsp;'); // format as HTML
//	return str;
//	function parse ( data, prefix ) {
//		if (typeof prefix=='undefined') prefix='';
//		try {
//			$.each( data, function (key, val) {
//				k = prefix+key+':  ';
//				skip = (keys && keys.indexOf(','+key+',') == -1);
//				if (typeof val=='function') { // FUNCTION
//					if (!skip) D[i++] = k +'function()';
//				}
//				else if (typeof val=='string') { // STRING
//					if (!skip) D[i++] = k +'"'+ val +'"';
//				}
//				else if (typeof val !='object') { // NUMBER or BOOLEAN
//					if (!skip) D[i++] = k + val;
//				}
//				else if (isArray(val)) { // ARRAY
//					if (!skip) D[i++] = k +'[ '+ val.toString() +' ]'; // output delimited array
//				}
//				else if (val.jquery) {
//					if (!skip) D[i++] = k +'jQuery ('+ val.length +') context="'+ val.context +'"';
//				}
//				else if (val.tagName && typeof val.style == 'object') {
//					var id = val.id, cls=val.className, src=val.src, hrf=val.href;
//					if (skip) D[i++] = k +' '+
//						id  ? 'id="'+	id+'"' :
//						src ? 'src="'+	src+'"' :
//						hrf ? 'href="'+	hrf+'"' :
//						cls ? 'class="'+cls+'"' :
//						'';
//				}
//				else { // Object or JSON
//					if (!recurse || !hasKeys(val)) { // show an empty hash
//						if (!skip) D[i++] = k +'{ }';
//					}
//					else { // recurse into JSON hash - indent output
//						D[i++] = k +'{';
//						parse( val, prefix+'    '); // RECURSE
//						D[i++] = prefix +'}';
//					}
//				}
//			});
//		} catch (e) {}
//		function isArray(o) {
//			return (o && typeof o==='object' && !o.propertyIsEnumerable('length') && typeof o.length==='number');
//		}
//		function hasKeys(o) {
//			var c=0;
//			for (x in o) c++;
//			if (!hasSubKeys) hasSubKeys = !!c;
//			return !!c;
//		}
//	}
//};
/**
* showOptions
*
* Pass a layout-options object, and the pane/key you want to display
*/
//function showOptions (o_Settings, key) {
//	var data = o_Settings.options;
//	$.each(key.split("."), function() {
//		data = data[this]; // resurse through multiple levels
//	});
//	debugData( data, 'options.'+key );
//}
///**
//* showState
//*
//* Pass a layout-options object, and the pane/key you want to display
//*/
//function showState (o_Settings, key) {
//	debugData( o_Settings.state[key], 'state.'+key );
//}
///**
//* createInnerLayout
//*/
var GLOBAL = {};
/**
 * 命名空间函数
 * @author ww
 */
GLOBAL.namespace = function (str) {
    var arr = str.split('.'), o = GLOBAL;
    for (i = (arr[0] == 'GLOBAL') ? 1 : 0; i < arr.length; i++) {
        o[arr[i]] = o[arr[i]] || {};
        o = o[arr[i]];
    }
}

function createInnerLayout () {
	// innerLayout is INSIDE the center-pane of the outerLayout
	//debugData( layoutSettings_Inner );
	innerLayout = $( outerLayout.options.center.paneSelector ).layout( layoutSettings_Inner );
	// hide 'Create Inner Layout' commands and show the list of testing commands
	$('#createInner').hide();
	$('#createInner2').hide();
	$('#innerCommands').show();
}

function bindResizeH(div1,div,div2, e) {
    //objstyle = obj.style, x = y = 0; y2 = 0;
    var obj = document.getElementById(div);
    var obj1 = document.getElementById(div1);
    var obj2 = document.getElementById(div2);
    var _divs = $("#" + div2).nextAll();
    var x = y = 0;
    y1 = e.clientY - obj1.clientHeight;
    y2 = e.clientY + obj2.clientHeight;
    obj.setCapture ? (obj.setCapture(),//捕捉焦点                 
        obj.onmousemove = function (ev) {//设置事件 
            mouseMove(ev || event)
        },
        obj.onmouseup = mouseUp
    ) : (
        $(document).bind("mousemove", mouseMove).bind("mouseup", mouseUp)
    )
    e.preventDefault() //防止默认事件发生        
    //移动事件 
    function mouseMove(e) {

        if (e.clientY - y1 > 10 && y2 - e.clientY > 0) {
            obj1.style.height = e.clientY - y1 + 'px';
            obj2.style.height = y2 - e.clientY + 'px';
            for (i = 0; i < _divs.length - 1;i++)
            {
                $(_divs[i]).height(y2 - e.clientY);
            }
        }
    }
    //停止事件 
    function mouseUp() {
        obj.releaseCapture ? (
        //释放焦点 
            obj.releaseCapture(),
        //移除事件 
            obj.onmousemove = obj.onmouseup = null
        ) : (
            //卸载事件 
            $(document).unbind("mousemove", mouseMove).unbind("mouseup", mouseUp)
        )
    }
}
function bindResizeH2(div1, div, div2, e) {
    //objstyle = obj.style, x = y = 0; y2 = 0;
    var obj = document.getElementById(div);
    var obj1 = document.getElementById(div1);
    var obj2 = document.getElementById(div2);
    var _divs = $("#" + div2).children("div");
    var x = y = 0,_fix=23;
    y1 = e.clientY - obj1.clientHeight;
    y2 = e.clientY + obj2.clientHeight;
    obj.setCapture ? (obj.setCapture(),//捕捉焦点                 
        obj.onmousemove = function (ev) {//设置事件 
            mouseMove(ev || event)
        },
        obj.onmouseup = mouseUp
    ) : (
        $(document).bind("mousemove", mouseMove).bind("mouseup", mouseUp)
    )
    e.preventDefault() //防止默认事件发生        
    //移动事件 
    function mouseMove(e) {

        if (e.clientY - y1 > 10 && y2 - e.clientY > 0) {
            obj1.style.height = e.clientY - y1 + 'px';
            obj2.style.height = y2 - e.clientY  + 'px';
            for (i = 0; i < _divs.length - 1; i++) {
                $(_divs[i]).height(y2 - e.clientY - _fix);
            }
        }
    }
    //停止事件 
    function mouseUp() {
        obj.releaseCapture ? (//obj.releaseCapture(),obj.onmousemove = obj.onmouseup = null) : (
        //释放焦点 
            obj.releaseCapture(),
        //移除事件 
            obj.onmousemove = obj.onmouseup = null
        ) : (
            //卸载事件 
            $(document).unbind("mousemove", mouseMove).unbind("mouseup", mouseUp)
        )
    }
}
function bindResizeHS(div1, div, div2, e) {
    var obj = document.getElementById(div);
    var obj1 = document.getElementById(div1);
    var obj2 = document.getElementById(div2);
    //var _divs = $("#" + div2).children("div");
    var x = y = 0, _fix = 23;
    y1 = e.clientY - obj1.clientHeight;
    y2 = e.clientY + obj2.clientHeight;
    obj.setCapture ? (obj.setCapture(),//捕捉焦点                 
        obj.onmousemove = function (ev) {//设置事件 
            mouseMove(ev || event)
        },
        obj.onmouseup = mouseUp
    ) : (
        $(document).bind("mousemove", mouseMove).bind("mouseup", mouseUp)
    )
    e.preventDefault() //防止默认事件发生        
    //移动事件 
    function mouseMove(e) {
        if (e.clientY - y1 > 10 && y2 - e.clientY > 0) {
            obj1.style.height = e.clientY - y1 + 'px';
            obj2.style.height = y2 - e.clientY + 'px';
        }
    }
    //停止事件 
    function mouseUp() {
        obj.releaseCapture ? (//obj.releaseCapture(),obj.onmousemove = obj.onmouseup = null) : (
        //释放焦点 
            obj.releaseCapture(),
        //移除事件 
            obj.onmousemove = obj.onmouseup = null
        ) : (
            //卸载事件 
            $(document).unbind("mousemove", mouseMove).unbind("mouseup", mouseUp)
        )
    }
}
function resizeTopB(div1, div, div2, e,callback,fix) {
    var obj = document.getElementById(div);
    var obj1 = document.getElementById(div1);
    var obj2 = document.getElementById(div2);

    var pagey = e.pageY;
    var h1 = obj1.clientHeight;
    var h2 = obj2.clientHeight;
    var _fix=26;

    obj.setCapture ? (obj.setCapture(),//捕捉焦点                 
        obj.onmousemove = function (ev) {//设置事件 
            mouseMove(ev || event)
        },
        obj.onmouseup = mouseUp
    ) : (
        $(document).bind("mousemove", mouseMove).bind("mouseup", mouseUp)
    )
    e.preventDefault() //防止默认事件发生        
    //移动事件 
    if (fix != undefined)
        _fix = fix;

    function mouseMove(ev) {
        if (ev.clientY > pagey - h1 && ev.clientY < pagey + h2-55) {
            var offset = pagey - ev.clientY;
            obj1.style.height = h1 - offset+ 'px'; //上下边框 内外边距
            obj2.style.top = h1 - offset+_fix + $(obj).outerHeight() + 'px';    //分隔条和它的边距
        }
    }
    //停止事件 
    function mouseUp() {
        obj.releaseCapture ? (//obj.releaseCapture(),obj.onmousemove = obj.onmouseup = null) : (
        //释放焦点 
            obj.releaseCapture(),
        //移除事件 
            obj.onmousemove = obj.onmouseup = null
        ) : (
            //卸载事件 
            $(document).unbind("mousemove", mouseMove).unbind("mouseup", mouseUp)
        )
        callback();
    }
}
function resizeLeftToR(div1, div, div2, e,callback) {
    var obj = document.getElementById(div);
    var obj1 = document.getElementById(div1);
    var obj2 = document.getElementById(div2);

    var x1 = x2 = 0;
    x1 = e.clientX - obj1.clientWidth;
    x2 = e.clientX + obj2.clientWidth;
    obj.setCapture ? (obj.setCapture(),//捕捉焦点                 
        obj.onmousemove = function (ev) {//设置事件 
            mouseMove(ev || event)
        },
        obj.onmouseup = mouseUp
    ) : (
        $(document).bind("mousemove", mouseMove).bind("mouseup", mouseUp)
    )
    e.preventDefault() //防止默认事件发生        
    //移动事件 
    function mouseMove(e) {

        if (e.clientX - x1 > 10 && x2 - e.clientX > 0) {
            obj.style.left = e.clientX-x1+'px';
            obj1.style.width = e.clientX - x1 + 'px';
            obj2.style.left = e.clientX - x1+$(obj).outerWidth()+ 'px';
        }
    }
    //停止事件 
    function mouseUp() {

        obj.releaseCapture ? (//obj.releaseCapture(),obj.onmousemove = obj.onmouseup = null) : (
        //释放焦点 
            obj.releaseCapture(),
        //移除事件 
            obj.onmousemove = obj.onmouseup = null
        ) : (
            //卸载事件 
            $(document).unbind("mousemove", mouseMove).unbind("mouseup", mouseUp)
        )
        callback();
    }
}
function resizeTbcenter(div1, div, div2, e,callback) {
    var obj = document.getElementById(div);
    var obj1 = document.getElementById(div1);
    var obj2 = document.getElementById(div2);

    var y1 = y2 = 0;

    y1 = e.clientY - obj1.clientHeight;
    y2 = e.clientY + obj2.clientHeight;
    obj.setCapture ? (obj.setCapture(),//捕捉焦点                 
        obj.onmousemove = function (ev) {//设置事件 
            mouseMove(ev || event)
        },
        obj.onmouseup = mouseUp
    ) : (
        $(document).bind("mousemove", mouseMove).bind("mouseup", mouseUp)
    )
    e.preventDefault() //防止默认事件发生        
    //移动事件 
    function mouseMove(e) {
        if (e.clientY - y1 > 10 && y2 - e.clientY > 0) {
            obj.style.bottom = y2 - e.clientY + 'px';
            obj1.style.bottom = y2 - e.clientY +$(obj).outerHeight()+ 'px'; //上下边框 内外边距
            obj2.style.height = y2 - e.clientY + 'px';    //分隔条和它的边距
        }
    }
    //停止事件 
    function mouseUp() {
        obj.releaseCapture ? (//obj.releaseCapture(),obj.onmousemove = obj.onmouseup = null) : (
        //释放焦点 
            obj.releaseCapture(),
        //移除事件 
            obj.onmousemove = obj.onmouseup = null
        ) : (
            //卸载事件 
            $(document).unbind("mousemove", mouseMove).unbind("mouseup", mouseUp)
        )
        callback();
    }
}

function countDiv(div1, div2) {
    var obj1 = document.getElementById(div1);
    var obj2 = document.getElementById(div2);
    obj2.style.height = $(".ui-layout-center").height() - $("#" + div1).height() - 65 + 'px';
}
function countDivThree(div1, div2, div3) {
    var obj1 = document.getElementById(div1);
    var obj2 = document.getElementById(div2);
    var obj3 = document.getElementById(div3);
    obj3.style.height = $(".ui-layout-center").height() - $("#" + div1).height() - $("#" + div2).height() - 100 + 'px';
}
//计算查询grid
function countLeftDiv(args) {
    var init = [{ id: "id", fix: 0}, { id: "id2", fix: 5 }];
    var obj;
    var me = "#";
    for (i = 0; i < args.length; i++) {
        obj = document.getElementById(args[i].id);  
        //if (args[i].clear) {
        //    $(me + args[i].id).css("height", "none");
        //    //obj.style.height = "none";
        //}
        //else
            obj.style.height = $(".ui-layout-center").height() - args[i].fix + 'px';
    }
}

//#region 下拉框
//焦点是否在选择层上:初始时为false,表示默认不在选择层上
//主要防止鼠标点击选择项时，文本框会失去焦点，这样选择层就会跟着隐藏，此时还未
//让点击的选择项选中并赋值到文本框中去。此时可以设置鼠标在选择层上时cursorInSelectDivObj=ture
//这时点击时不会立即隐藏选择层，等选中后再设置cursorInSelectDivObj=false，此时就可以隐藏选择层了
var cursorInSelectDivObj = false, textselselectvalue = -1, seljsonData, regxData;
//var textObj, selObj;
function showtextSelect(textID, selID, jsonData, btn) {//jsonData=[{ ID: 1, txt: "aaa", des: "奇趣地3" },]
    var textObj, selObj, btnObj;
    textObj = document.getElementById(textID);
    selObj = document.getElementById(selID);
    if (btn != undefined) {
        //btnObj = document.getElementById(btn);
        //btnObj.onclick = function () {
            DrawDiv(textObj, selID, jsonData);
            textObj.focus();
        //}
    }
    seljsonData = jsonData, regxData = jsonData;
    textObj.onclick = function () {
        DrawDiv(textObj, selID, jsonData);
    }
    //if (btn)
    //    btn.onclick = function () {
    //        DrawDiv(textObj, selID, jsonData);
    //    }
    textObj.onblur = function () {
        textonblur(textObj, selID);
    }
    textObj.onkeyup = function (e) {
        textonkeyup(e || event, textObj, selID);
    }
}
function DrawDiv(textObj, selID, data) {
    $("#_divmast").remove();
    $("#_divdesc").remove();
    var _div = "", _divdesc = "";
    for (i = 0; i < data.length; i++) {
        _div += "<div id='_divsel" + data[i].ID + "' value='" + data[i].ID + "' desc='" + data[i].des + "' >" + data[i].txt + "</div>";//onmouseover=divselonmouseover(this)
    }
    if (data[0] && data[0].des)
    _divdesc = '<div id="_divdesc" style="position: absolute;cursor: default;border: 1px solid #B2B2B2;background-color:#ffffd9;width:150px;height:120px;overflow-x:auto;word-wrap:break-word;z-index:999;"></div>';

    if (data.length > 0) {
        if (data.length>35)
            _div = "<div id='_divmast' style='position: absolute;cursor: default;border: 1px solid #B2B2B2;background-color: #fff;z-index:999;overflow:auto;height:300px;'>" + _div + "</div>";
        else
            _div = "<div id='_divmast' style='position: absolute;cursor: default;border: 1px solid #B2B2B2;background-color: #fff;z-index:999;overflow:auto;'>" + _div + "</div>";
        $("body").append(_div + _divdesc);

        var _divmastObj = document.getElementById("_divmast");
        //var el = textObj.parentNode, _t = 0, _l = 0;
        //while (el) {
        //    _t += el.offsetTop;
        //    _l += el.offsetLeft;
        //    el = el.offsetParent;
        //}
        //_divmastObj.style.top = _t + textObj.parentNode.offsetHeight + "px";   //textObj高度 
        //_divmastObj.style.left = _l + "px";//textObj左边距
        //_divmastObj.style.width = textObj.parentNode.clientWidth + "px";
        //if (data[0]&&data[0].des) {
        //    var _divdescObj = document.getElementById("_divdesc"); 
        //    _divdescObj.style.top = _divmastObj.style.top + "px";
        //    _divdescObj.style.left = textObj.offsetLeft + textObj.scrollWidth + "px";
        //}
        var viewHeight = $(window).height();
        var scrollTop = $(window).scrollTop();
        var textHeight = $(textObj).outerHeight();
        var offset = $(textObj).offset();
        var spaceUp = offset.top - scrollTop;
        var spaceDown = viewHeight - (spaceUp + textHeight);
        var verticalMax = spaceUp > spaceDown ? spaceUp : spaceDown;
        offset.top = offset.top + textHeight;
        _divmastObj.style.width = textObj.parentNode.clientWidth + "px";
        // Double for IE bug
        $(_divmastObj).offset(offset);
        $(_divmastObj).offset(offset);
        //添加事件
        ///////鼠标经过下拉框层时
        $("#_divmast").bind("mouseover", function () {
            //当鼠标移进选择层时，设置选择层为获得焦点状态	
            cursorInSelectDivObj = true;
        });
        //鼠标移出下拉框层时
        $("#_divmast").bind("mouseout", function () {
            //当鼠标移出选择层时，设置选择层为失去焦点状态	
            cursorInSelectDivObj = false;

            //当鼠标移出选择层时，让文本框获取焦点
            textObj.focus();
        });  
        $("#_divmast div").bind("mouseover", function () {
            divselonmouseover(this);
        });
        $("#_divmast div").bind("click", function () {
            selObj = document.getElementById(selID);
            divselonclick(textObj, this, selObj);

            //点击选项后选择层后要隐藏，即要设置成失去焦点状态
            cursorInSelectDivObj = false;
            //点击过后使文本框获取焦点
            textObj.focus();
            //调用文本框失去焦点触发的方法
            textonblur(textObj, selID);
        });
    }
}

function divselonmouseover(divselObj) {
    //当鼠标移进选择层时，设置选择层为获得焦点状态	
    cursorInSelectDivObj = true;
    var _divmastObj = document.getElementById("_divmast");
    $("#_divmast div").removeAttr("style");
    //使本身选项层的背景为蓝色，字为白色，模拟选中样式
    divselObj.style.cssText = 'background-color: #479ff3;color: #ffffff;'
    textselselectvalue = $(divselObj).attr("value");
    $("#_divdesc").text($(divselObj).attr("desc"));
}
function divselonclick(textObj, divselObj, selObj) {
    if (divselObj) {
        _divselecttxt = $(divselObj).text();
        $(textObj).val(_divselecttxt);
        $(selObj).val(textselselectvalue);//下拉框定位
    }
}
///////文本框输入时
function textonkeyup(e, textObj, selID) {
    switch (e.keyCode) {
        case 9://Tab键
            textObj.focus();
            textonblur(textObj, selID);
            break;
        case 13://回车
            textenter(textObj);
            textObj.focus();
            textonblur(textObj, selID);
            break;
        case 38://上键
            selObj = document.getElementById(selID);
            textselectUp(textObj, regxData, selObj);
            break;
        case 40://下键
            selObj = document.getElementById(selID);
            textselectDown(textObj, regxData, selObj);
            break;
        default:
            textautoMatch(textObj);
            break;
    }
}
function textenter(textObj) {

}

function textselectUp(textObj, data, selObj) {
    var _index = -1;
    for (i = 0; i < data.length; i++) {
        if (textselselectvalue == data[i].ID) {
            _index = i; $("#_divsel" + textselselectvalue).removeAttr("style"); break;
        }
    }
    if (_index > 0) {
        var _divselObj = document.getElementById("_divsel" + textselselectvalue);
        var _divselprevObj = document.getElementById("_divsel" + data[i - 1].ID);
        //使本身选项层的背景为蓝色，字为白色，模拟选中样式
        _divselprevObj.style.cssText = 'background-color: #479ff3;color: #ffffff;'
        textselselectvalue = $(_divselprevObj).attr("value");
        $("#_divdesc").text($(_divselprevObj).attr("desc"));
        $(textObj).val($(_divselprevObj).text());
        //setTimeout(function () { textselselectvalue = data[_index - 1].ID; textselectUp(textObj, data); }, 300);
    }
    else if (_index == 0) {
        var _divselprevObj = document.getElementById("_divsel" + data[data.length - 1].ID);
        //使本身选项层的背景为蓝色，字为白色，模拟选中样式
        _divselprevObj.style.cssText = 'background-color: #479ff3;color: #ffffff;'
        textselselectvalue = $(_divselprevObj).attr("value");
        $("#_divdesc").text($(_divselprevObj).attr("desc"));
        $(textObj).val($(_divselprevObj).text());
    }
    else {
        var _divselprevObj = document.getElementById("_divsel" + data[0].ID);
        //使本身选项层的背景为蓝色，字为白色，模拟选中样式
        _divselprevObj.style.cssText = 'background-color: #479ff3;color: #ffffff;'
        textselselectvalue = $(_divselprevObj).attr("value");
        $("#_divdesc").text($(_divselprevObj).attr("desc"));
        $(textObj).val($(_divselprevObj).text());
    }
    $(selObj).val(textselselectvalue);//下拉框定位
}
function textselectDown(textObj, data, selObj) {
    var _index = -1;
    for (i = 0; i < data.length; i++) {
        if (textselselectvalue == data[i].ID) {
            _index = i; $("#_divsel" + textselselectvalue).removeAttr("style"); break;
        }
    }
    if (_index < 0 && data[0]) {
        var _divselnextObj = document.getElementById("_divsel" + data[0].ID);
        //使本身选项层的背景为蓝色，字为白色，模拟选中样式
        _divselnextObj.style.cssText = 'background-color: #479ff3;color: #ffffff;'
        textselselectvalue = $(_divselnextObj).attr("value");
        $("#_divdesc").text($(_divselnextObj).attr("desc"));
        $(textObj).val($(_divselnextObj).text());
    }
    else if (_index >= 0 && _index < data.length - 1) {
        var _divselObj = document.getElementById("_divsel" + textselselectvalue);
        var _divselnextObj = document.getElementById("_divsel" + data[i + 1].ID);
        //使本身选项层的背景为蓝色，字为白色，模拟选中样式
        _divselnextObj.style.cssText = 'background-color: #479ff3;color: #ffffff;'
        textselselectvalue = $(_divselnextObj).attr("value");
        $("#_divdesc").text($(_divselnextObj).attr("desc"));
        $(textObj).val($(_divselnextObj).text());
    }
    else if (_index = data.length - 1) {
        var _divselnextObj = document.getElementById("_divsel" + data[data.length - 1].ID);
        //使本身选项层的背景为蓝色，字为白色，模拟选中样式
        _divselnextObj.style.cssText = 'background-color: #479ff3;color: #ffffff;'
        textselselectvalue = $(_divselnextObj).attr("value");
        $("#_divdesc").text($(_divselnextObj).attr("desc"));
        $(textObj).val($(_divselnextObj).text());
    }
    $("#_divmast").scrollTop = 20;
    $(selObj).val(textselselectvalue);//下拉框定位
}
function textautoMatch(textObj) {
    var textValueReg = replaceReg(textObj.value), thisData = [], _index = -1;
    for (var i = 0; i < seljsonData.length; i++) {
        var regRegExp = new RegExp('^' + textValueReg);
        //先模糊匹配
        if (regRegExp.test(seljsonData[i].txt)) {
            thisData.push(seljsonData[i]);
        }
    }
    DrawDiv(textObj, null, thisData);
    regxData = thisData;
    textObj.focus();
    return { data: thisData, selectIndex: _index };
}
//代换正则表达式中特殊字符
function replaceReg(str) {
    //$()*+.[?]^|}{\
    var regStr = /[$()*+.\[?\]^|}{\\]/g;

    if (!str.match(regStr)) {
        return str;
    }

    var regArr = /./g;
    var valueArr = str.match(regArr);
    var tempStr = "";

    for (var i = 0 ; i < valueArr.length; i++) {
        regStr = /[$()*+.\[?\]^|}{\\]/g;

        if (valueArr[i].match(regStr)) {
            valueArr[i] = findByKey(valueArr[i])[1];
        }

        tempStr = tempStr + valueArr[i];
    }
    return tempStr;
}
//查询正则特殊字符要替换字符串
function findByKey(key) {
    var i = 0;
    for (var i = 0; i < regChars.length; i++) {
        if (regChars[i][0] == key) {
            return regChars[i];
        }
    }
    if (i == regChars.length) {
        return null;
    }
}
function textonblur(textObj, selID) {
    //如果点击了某个选项后，已设置选择层为失去焦点状态，此时选择层可以隐藏了
    if (!cursorInSelectDivObj) {
        $("#_divmast").remove();
        $("#_divdesc").remove();
        var jq = "#" + selID + " option";
        $(jq).each(function (i) {
            if ($(this).text() == $(textObj).val()) { $("#" + selID).get(0).selectedIndex = i; return false; }
        })
        //$(jq).attr("selected", true);
        textselselectvalue = -1;
    }
}
//#endregion

//#region Tabs
(function ($) {
    var _s = "#";
    $.fn.clearDivHeight = function (args) {
        $(this).children("div").css("height");
    }

    $.fn.tabsInit = function (args,callback) {
        var me = this.attr("id");
        var _ul = $(this).children("ul");
        var _div = $(this).children("div");
        //for (i = 0; i < _div.length; i++) {
        //    tabArgs._scoll.push({ h: 0, v: 0 });
        //}

        switch (args) {
            case "b":
                //tabArgs._bodyTopDivHeight = document.getElementById("topDivedit").clientHeight  + 20 + 20;
                //tabArgs._bodyBottomDivHeight = $(".ui-layout-center").height();// document.getElementById(ui-layout-center).clientHeight;
                $(_ul[0]).addClass("tabs-bottom-head");
                $(_ul[0].children[0]).addClass("current");
                break;
            case "l":
                $(_ul[0]).addClass("tabs-left-head");
                $(_ul[0].children[0]).addClass("current");
                var len = $(_ul[0]).children("li").length;
                if (len > 7) {
                    $(_ul[0]).width("45px");
                    $(_ul[0]).children("li:odd").css("clear", "left");
                    $(_ul[0]).children("li:gt(1):even").css("margin-top", "-50px");
                }
                break;
            case "r":
                $(_ul[0]).addClass("tabs-right-head");
                $(_ul[0].children[0]).addClass("current");
                break;
            case "refresh":
                $(_ul[0]).addClass("tabs-right-head");
                break;
            case "t":
                //tabArgs._bodyTopDivHeight = document.getElementById("topDivedit").clientHeight + 20;// document.getElementById(me).clientHeight;
                $(_ul[0]).addClass("tabs-top-head");
                $(_ul[0].children[0]).addClass("current");
                break;
            default:
                $(_ul[0]).addClass("tabs-top-head");
                $(_ul[0].children[0]).addClass("current");
                break;
        }

        for (i = 0; i < _div.length; i++) {
            switch (args) {
                case "b":
                    //$(_div[i]).height(tabArgs._bodyBottomDivHeight - tabArgs._headHeight - tabArgs._bodyTopDivHeight);//("height", tabArgs._bodyBottomDivHeight - tabArgs._headHeight);
                    break;
                case "l":
                    $(_div[i]).css("height", tabArgs._bodyBottomDivHeight - tabArgs._headHeight);
                    break;
                case "r":
                    $(_div[i]).css("height", tabArgs._bodyBottomDivHeight - tabArgs._headHeight);
                    break;
                case "refresh":
                    $(_div[i]).css("height", tabArgs._bodyBottomDivHeight - tabArgs._headHeight);
                    break;
                case "left":
            
                    break;
                case "t":
                    //$(_div[i]).css("height", tabArgs._bodyTopDivHeight - tabArgs._headHeight);
                    break;
            }

            if (i != 0) {
                $(_div[i]).css("display", "none");
                //$(_div[i]).show().siblings().hide();'left'
            }
        }
        $(_ul[0].children).click(function () {
            index = $(_ul[0].children).index(this);
            for (i = 0; i < _div.length; i++) {
                if (i == index) {
                    $(_div[i]).css("display", "block");
                    $(_ul[0].children[i]).addClass("current");
                    if (callback != undefined)
                        callback();
                    //$("#" + me).parent()[0].scrollTop = 0;//tabArgs._scoll[i].v;
                }
                else {
                    $(_div[i]).css("display", "none");
                    $(_ul[0].children[i]).removeClass("current");
                    //tabArgs._scoll[i].v = $("#" + me).parent()[0].scrollTop;
                }
            }
        }
      )
    }
    $.fn.setActive = function (index) {
        var me = this.attr("id");
        var _ul = $(this).children("ul");
        var _div = $(this).children("div");
        for (i = 0; i < _div.length; i++) {       
            if (i == index) {
                $(_div[i]).css("display", "block");
                $(_ul[0].children[i]).addClass("current");               
            }
            else {
                $(_div[i]).css("display", "none");
                $(_ul[0].children[i]).removeClass("current");
            }
        }
        $("#" + me).parent()[0].scrollTop = 0;
    }
    $.fn.getActive = function () {
        var me = this.attr("id");
        var _ul = $(this).children("ul");
        var _div = $(this).children("div");
        for (i = 0; i < _div.length; i++) {
            if ($(_div[i]).css("display") == "block") {             
                return i;
            }
        }
        return null;
    }
    $.fn.displayTabs = function (t) {
        if (t.length > 0) {
            for (i = 0; i < t.length; i++) {
                $(this).children("ul").children("li").eq(t[i]).css("display", "block");
            }
        }
    }
    $.fn.disabledTabs = function (t) {
        $(this).children("ul").children("li").attr("disabled", false);
        if (t.length > 0) {
            for (i = 0; i < t.length; i++) {
                $(this).children("ul").children("li").eq(t[i]).attr("disabled", true);
            }
        }
    }
    $.fn.tabs_container = function (args) {
        var me = this.attr("id");
        var _ul = $(this).children("ul");
        var _div = $(this).children("div");

        tabArgs._bodyBottomDivHeight = document.getElementById(me).clientHeight;
        $(_ul[0]).addClass("tabs-bottom-head");
        $(_ul[0].children[0]).addClass("current");

        //var _divHeight = document.getElementById(me).clientHeight;// $(this).height();

        for (i = 0; i < _div.length; i++) {
            $(_div[i]).height(tabArgs._bodyBottomDivHeight - tabArgs._headHeight);
        }
        if (i != 0) {
            $(_div[i]).css("display", "none");
        }
    };
    $.fn.tabs_add = function (t,args) {
        switch (t) {
            case "l":
                break;
            case "b":
                //for (i = 0; i < tabContain.length; i++) {
                //    if (tabContain[i].id == args.id) {
                //        break;
                //    }
                //}
                //if (i < tabContain.length) {
                //    $(this).tabs_active(i);
                //}
                //else {
                //    tabContain.push({ index: i, id: args.id });
                var label = args.txt,
                      id = args.id,
                      li = $("<li><span><a href='#" + id + "'>" + label + "</a> <span class='tabs-span-close'>移除</span></span></li>"),
                  tabContentHtml = args.txt;

                //var tabsPage = $($(this).children("div"));
                $(this).children("div").last().after("<div id='" + id + "'><p>" + tabContentHtml + "</p></div>");
                $(this).find(".tabs-bottom-head").append(li);
                $(this).tabs_show('refresh');
                $(this).tabs_active(tabContain.length - 1);
                break;
            default:

                break;
        }
    }

    $.fn.tabs_remove = function (t, args) {
        switch (t) {
            case "l":
                break;
            case "b":
                for (i = 0; i < tabContain.length; i++) {
                    if (tabContain[i].id == args.id) {
                        break;
                    }
                }
                if (i < tabContain.length) {        
                    tabContain.pop(i);        
                    $(this).children("div").eq(i).remove();
                    $(this).children("ul").find("li:eq("+i+")").remove();           
                    if (tabContain.length > 0) {
                        if (i > 0 && i < tabContain.length)
                            $(this).tabs_active(i);
                        else if(i == tabContain.length)
                            $(this).tabs_active(i-1);
                    }
                }
                break;
            default:

                break;
        }
    }

    $.fn.mengbanDiv = function (args) {
        var me = this.attr("id");
        var _h = $(this).innerHeight(),
            _w = $(this).innerWidth(),
            _l=document.getElementById(me).offsetLeft-1,
            _t = document.getElementById(me).offsetTop-1;
        if (args) {//true
            if ($(this).children("div .menbanDiv").length == 0) {
                //$(this).children("div .menbanDiv").remove();
                $(this).append('<div class="menbanDiv" style="width:' + _w + 'px; height: ' + _h + 'px;left:' + _l + 'px;top:' + _t + 'px;"></div>');
            }
        }
        else {
            $(this).children("div .menbanDiv").remove();
        }
    }

    $.fn.checDivInput = function (args) {
        $(this).find("input:text").each(function (i) {
            $(this).change(function () {
                //var _reqed = $(this).attr("required");
                var _ck = $(this).attr("check");
                //if (Sys.ie) {
                    switch (_ck) {
                        case "num":
                            if (!validateFloat($(this).val())) {
                                $(this).addClass("Error");
                                alert("请输入数字");
                                $(this).val("");
                            }
                            else
                                $(this).removeClass("Error");
                            //return false;
                            break;
                        case "date":
                            if (!validateDate($(this).val())) {
                                $(this).addClass("Error");
                                alert("请输入正确日期如;2013/08/09");
                                $(this).val("");
                            }
                            else
                                $(this).removeClass("Error");
                            //return false;
                            break;
                    }
                //}
                //else {
                //    switch (_ck) {
                //        case "num":
                //            if (!validateFloat($(this).val())) {
                //                alert("请输入数字");
                //                $(this).val("");
                //            }
                //            return false;
                //            break;
                //        case "date":
                //            if (!validateDate($(this).val())) {
                //                alert("请输入正确日期如;2013/08/09");
                //                $(this).val("");
                //            }
                //            return false;
                //            break;

                //    }
                //}
            });
        })
    }
    $.fn.RegDropDownData = function (args) {
        $(this).empty();
        //$(this).append("<option value=''>--维护--</option>");
        $(this).append("<option value=''></option>");
        for (i = 0; i < args.length; i++) {
            $(this).append("<option value='" + args[i].ID + "'>" + args[i].txt + "</option>");
        }
        //$(this).get(0).selectedIndex = -1;
    }
}
)(jQuery)

var tabArgs = function () {
    return  {
        _headHeight: 23,
        _bodyTopDivHeight: 200,
        _bodyBottomDivHeight: 200, _scoll: []
    }

}(jQuery)
//#endregion

//消息框
var Msg ;
if (!Msg) Msg = {};
Msg.show = (function () {
    function show(msg) {
        parent.showMessage(msg);
    }
    return show;
})();

//#region 系统扩展
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
// 如果已经定义了命名空间对象
var Dot;                // 创建Dot命名空间
if (!Dot) Dot = {};
Dot.students = {};    // student命名空间已经定义
(function (students) {
    // ..... 这里省略了代码 ......
    var Subject;
    var i;
    function Grade(args) { alert(args+this.i) };
    // 将公共API导到上面定义的命名空间中
    this.Subject = Subject;
    students.Grade = Grade;
    // 这里也不需要返回值
})(Dot.students);
var GG = {};
GG = (new function () {
    var Subject;
    function Grade(args) { alert(args)};
    // 将API导到this对象中
    this.Subject = Subject;
    this.Grade = Grade;
    // 注意，这里没有返回值
}());    // 括号写在里面。这里是创建新实例，new后面应紧跟构造函数的调用而不是表达式
//#endregion

//获取Cookie
function getCookie(str) {
    var tmp, reg = new RegExp("(^| )" + str + "=([^;]*)(;|$)", "gi");
    if (tmp = reg.exec(document.cookie)) return (tmp[2]);
    return null;
}
//验证
function validateFloat(val) {//验证整、小数
    var patten = /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/;
    return patten.test(val);
}
function validateNum(val) {//验证整数
    var patten = /^-?\d+$/;
    return patten.test(val);
}
function validateNumOrLetter(val) {//只能输入数字和字母
    var patten = /^[A-Za-z0-9]+$/;
    return patten.test(val);
}
function validateNull(val) {//验证空
    return val.replace(/\s+/g, "").length == 0;
}
function validateDate(val) {//验证时间2010-10-10 //var patten = /^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/;
    var patten = /^(?:(?!0000)[0-9]{4}\/(?:(?:0[1-9]|1[0-2])\/(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])\/(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)$/; 
    if (val == null || val == "") return true;
    var t = patten.test(val);
    if (!t) {
        var reg= /^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/;
        return reg.test(val);
    }
    return t;
}
function validateCH(val) {//验证汉字
    var patten = /^[\u4e00-\u9fa5],{0,}$/;
    return patten.test(val);
}
function validateEmail(val) {//验证Email
    var patten = /^(([A-Za-z0-9\-]+_+)|([A-Za-z0-9\-]+\-+)|([A-Za-z0-9\-]+\.+)|([A-Za-z0-9\-]+\++))*[A-Za-z0-9_\-]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$/;
    return patten.test(val);
}
function getQueryString(name)
{
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
//alert(GetQueryString("参数名3"));
//去空格
//String.prototype.trim = function () {
//    return this.replace(/(^\s*)|(\s*$)/g, "");
//}
String.prototype.trim = function (Useless) {          //eval函数转换字符串形式的表达式
    if (Useless == null) Useless = " ";
    var regex = eval("/^" + Useless + "*|" + Useless + "*$/g"); return this.replace(regex, "");
}
//自定义lTrim()方法去除字串左侧杂质 
String.prototype.ltrim = function (Useless) {
    var regex = eval("/^" + Useless + "*/g"); return this.replace(regex, "");
}
//自定义rTrim()方法去除字串右侧杂质 
String.prototype.rtrim = function (Useless) {    
    var regex = eval("/" + Useless + "*$/g"); return this.replace(regex, "");
}
//String.prototype.ltrim=function(){
//    return this.replace(/(^\s*)/g,"");
//}
//String.prototype.rtrim=function(){
//    return this.replace(/(\s*$)/g,"");
//}
Array.prototype.max = function (id) {
    var arr = [];
    if (!id)
        return Math.max.apply({}, this)
    else {
        for (var iii = 0; iii < this.length; iii++) {
            if (this[iii][id] != null)
                arr.push(this[iii][id]);
        }
        if (arr.length > 0)
            return Math.max.apply({}, arr);
        else
            return 0;
    }
}
Array.prototype.min = function (id) {
    var arr = [];
    if(!id)
        return Math.min.apply({}, this)
    else {        
        for (var iii = 0; iii < this.length; iii++) {
            if (this[iii][id] != null)
                arr.push(this[iii][id]);
        }
        if (arr.length > 0)
            return Math.min.apply({}, arr);
        else
            return 0;
    }
}
Array.prototype.indexOf = function (substr, start) {
    if (!start)
        return;

    var ta, rt, d = '\0';
    if (start != null) { ta = this.slice(start); rt = start; } else { ta = this; rt = 0; }
    var str = d + ta.join(d) + d, t = str.indexOf(d + substr + d);
    if (t == -1) return -1; rt += str.slice(0, t).replace(/[^\0]/g, '').length;
    return rt;
}

Array.prototype.lastIndexOf = function (substr, start) {
    var ta, rt, d = '\0';
    if (start != null) { ta = this.slice(start); rt = start; } else { ta = this; rt = 0; }
    ta = ta.reverse(); var str = d + ta.join(d) + d, t = str.indexOf(d + substr + d);
    if (t == -1) return -1; rt += str.slice(t).replace(/[^\0]/g, '').length - 2;
    return rt;
}
Array.prototype.replace = function (reg, rpby) {
    var ta = this.slice(0), d = '\0';
    var str = ta.join(d); str = str.replace(reg, rpby);
    return str.split(d);
}
Array.prototype.search = function (reg) {
    var ta = this.slice(0), d = '\0', str = d + ta.join(d) + d, regstr = reg.toString();
    reg = new RegExp(regstr.replace(/\/((.|\n)+)\/.*/g, '\\0$1\\0'), regstr.slice(regstr.lastIndexOf('/') + 1));
    t = str.search(reg);
    if (t == -1)
        return -1;
    return str.slice(0, t).replace(/[^\0]/g, '').length;
}

// Original function by Alien51
Array.prototype.unique = function () {
    var arrVal = this;
    var uniqueArr = [];
    for (var i = arrVal.length; i--;) {
        var val = arrVal[i];
        if ($.inArray(val, uniqueArr) === -1) {
            uniqueArr.unshift(val);
        }
    }
    return uniqueArr;
}

Array.prototype.addUnique = function (val) {
    var arrVal = this;
    if ($.inArray(val, arrVal) === -1) {
        arrVal.push(val);
    }
}

function Int(value) {
    var _t = parseInt(value);
    if (isNaN(_t)) return 0; else return _t;
}
function Float(value) {
    var _t = parseFloat(value);
    if (isNaN(_t)) return 0; else return _t;
}
function Round(v, e) {
    var t = 1;
    for (; e > 0; t *= 10, e--);
    for (; e < 0; t /= 10, e++);
    return Math.round(v * t) / t;
}
function DateFormat(cellval) {
    try {
        var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + currentDate;
    } catch (e) {
        return "";
    }
}
/**
 * 时间对象的格式化
 */
Date.prototype.format = function(format) {
 /*
  * format="yyyy-MM-dd hh:mm:ss";
  */
 var o = {
  "M+" : this.getMonth() + 1,
  "d+" : this.getDate(),
  "h+" : this.getHours(),
  "m+" : this.getMinutes(),
  "s+" : this.getSeconds(),
  "q+" : Math.floor((this.getMonth() + 3) / 3),
  "S" : this.getMilliseconds()
 }
 if (/(y+)/.test(format)) {
  format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
      - RegExp.$1.length));
 }
 for (var k in o) {
  if (new RegExp("(" + k + ")").test(format)) {
   format = format.replace(RegExp.$1, RegExp.$1.length == 1
       ? o[k]
       : ("00" + o[k]).substr(("" + o[k]).length));
  }
 }
 return format;
} 
//{"date":"3","day":"2","hours":"0","minutes":"0","month":"0","seconds":"0","time":"1325520000000","timezoneOffset":"-480","year":"112"}
function toDate(obj){
 var date = new Date();
 date.setTime(obj.time);
 date.setHours(obj.hours);
 date.setMinutes(obj.minutes);
 date.setSeconds(obj.seconds);
 return date.format("yyyy-MM-dd hh:mm:ss");  
}
//只要把DateTime值传递给ConvertJSONDateToJSDateObject就可以返回Date。/Date(1366106684184)/
function JsonDateToObj(jsondate) {
    var date = new Date(parseInt(jsondate.replace("/Date(", "").replace(")/", ""), 10));
    return date;
} 
function getDate(jsondate) {
    var date = JsonDateToObj(jsondate);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    return year + "-" + month + "-" + day ;
}
//如果想返回yyyy-MM-dd HH:mm:SS格式
function getDateTime(jsondate) {
    var date = JsonDateToObj(jsondate);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hh = date.getHours();
    var mm = date.getMinutes();
    var ss = date.getSeconds();
    return year + "-" + month + "-" + day + " " + hh + ":" + mm + ":" + ss;
} 

//#region ajaxHelper.js
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
function ajaxGetSyn(url, data, _async, fn) {
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
//#endregion

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
function saveGridStyle()
{
    saveGridStyleA(ColumnDt);
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

//获取函数方法名
function getFnName(fn) {
    return (fn.toString().match(/function (.+?)\(/) || [, ''])[1];
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
function savegridColumns(columnsDt,selfcolumns, _grid, isGetcolun)
{
    var newcolumnsSearch;
    var columns = _grid.getVisibleColumns();
    var clonedArray = $.map(columns, function (obj) {
        return obj;
    });
    var clonedArray = setColumn(columns);
    var _columnsDt = {
        ID: null, tablename: columnsDt.tablename, reatableName: columnsDt.reatableName, usercode: LoginModel.logid, username: LoginModel.u_name, Columns: JSON.stringify(clonedArray)
    }
    if (_columnsDt != undefined) {
        ajaxPostSyn('/WebAPI/api/Common/SaveGridStyle/',
            JSON.stringify(_columnsDt),false,
            function (result) {
                if (result != null) {
                    getgridStyle(_columnsDt,selfcolumns, isGetcolun);
                    alert("界面保存成功！");
                }
            });

    }
    return newcolumnsSearch;
}
function cleargridStyle(columnsDt,selfcolumns)
{
    var newcolumnsSearch;
    var _columnsDt = {
        ID: null, tablename: columnsDt.tablename, reatableName: columnsDt.reatableName, usercode: LoginModel.logid, username: LoginModel.u_name
    }
    if (columnsDt != undefined) {
        ajaxPostSyn('/WebAPI/api/Common/ClearGridStyle/' + _columnsDt, JSON.stringify(_columnsDt),false,
        function (result) {
            newcolumnsSearch = selfcolumns;
        });
    }
    return newcolumnsSearch;
}
function getgridStyle(columnsDt, columns, isBool) {
    var newcolumnsSearch;
    var columnsdt = {
        ID: null, tablename: columnsDt.tablename, reatableName: columnsDt.reatableName, usercode: LoginModel.logid, username: LoginModel.u_name
    }
    var cloned2Array = $.map(columns, function (obj) {
        return obj;
    });
    ajaxPostSyn('/WebAPI/api/Common/GetGridStyle/' + columnsdt, JSON.stringify(columnsdt),false,
        function (result) {
            if (result != "0") {
                var newcoluSearch = strToJson(result);               
                newcolumnsSearch = getColum(newcoluSearch, cloned2Array);
            } else {
                newcolumnsSearch = cloned2Array;
            }
            if (isBool) {             
                GetClumns(columnsdt.tablename + "," + columnsdt.reatableName + "," + LoginModel.logid, newcolumnsSearch);
            }
        });
    return newcolumnsSearch;
}
function setColumn(column) {
    var columnsData = [];
    for (var i = 0; i < column.length; i++) {
        var columnN = { id: column[i].id, width: column[i].width };
        columnsData.push(columnN);
    }
    return columnsData;
}
function getColum(newcolumnsSearch,colu)
{
    var newColumns = [];
    for (var i = 0; i < newcolumnsSearch.length; i++)
    {
        for (var j = 0; j < colu.length; j++)
        {
            if (newcolumnsSearch[i].id == colu[j].id)
            {
                newColumns.push(colu[j]);
                newColumns[i].width = newcolumnsSearch[i].width;
            }
        }
    }
    return newColumns;
}
//function setColumn(column) {
//    for (var i = 0; i < column.length; i++) {
//        if (column[i].editor == "DropDownList") {         
//            column[i].editor = column[i].editor.toString();
//        } else if (column[i].editor != undefined && column[i].editor != "DropDownList") {
//            var fne = column[i].editor.toString().match(/function\s+([^\s\(]+)/);
//            column[i].editor = fne[1];
//        }
//        if (column[i].formatter != undefined) {
//            var fnf = column[i].formatter.toString().match(/function\s+([^\s\(]+)/);
//            column[i].formatter = fnf[1];
//        }
//        if (column[i].linkAction != undefined)
//        {
//            var fnl = column[i].linkAction.toString().match(/function\s+([^\s\(]+)/);
//            column[i].linkAction = fnl[1];
//        }
//    }
//    return column;
//}
//function getColum(newcolumnsSearch, fn2)
//    {
//        for (var i = 0; i < newcolumnsSearch.length; i++) {
//            if (newcolumnsSearch[i].formatter == "RowNumberFormatter") {
//                newcolumnsSearch[i].formatter = Slick.Formatters.RowNumber;
//            } else if (newcolumnsSearch[i].formatter == "CheckBoxVisibleFormatter") {
//                newcolumnsSearch[i].formatter = Slick.Formatters.CheckBoxVisible;
//            } else if (newcolumnsSearch[i].formatter == "hyperLinkInTabFormatter") {
//                newcolumnsSearch[i].formatter = Slick.Formatters.HyperLinkInTab;
//            }
//            if (newcolumnsSearch[i].editor == "CheckboxReferedEditor") {
//                newcolumnsSearch[i].editor = Slick.Editors.CheckboxRefered;
//            } else if (newcolumnsSearch[i].editor == "RowNumberSelector") {
//                newcolumnsSearch[i].editor = Slick.Editors.RowNumberSelector;
//            } else if (newcolumnsSearch[i].editor == "DecimalEditor") {
//                newcolumnsSearch[i].editor = Slick.Editors.Decimal;
//            } else if (newcolumnsSearch[i].editor == "TextButtonEditor") {
//                newcolumnsSearch[i].editor = Slick.Editors.TextButton;
//            } else if (newcolumnsSearch[i].editor == "DateEditor") {
//                newcolumnsSearch[i].editor = Slick.Editors.Date;
//            } else if (newcolumnsSearch[i].editor == "TextEditor") {
//                newcolumnsSearch[i].editor = Slick.Editors.Text;
//            } else if (newcolumnsSearch[i].editor == "DropDownList") {
//                newcolumnsSearch[i].editor = Slick.Editors.DropDownList;//DropDownList(args)
//            }
//            if (newcolumnsSearch[i].linkAction == "getActionName"&&fn2!=undefined)
//            {
//                newcolumnsSearch[i].linkAction = fn2;
//            }           
//        }
//    }//
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

    //#region contextmenu.js
    (function ($) {
        var menu, shadow, trigger, content, hash, currentTarget;
        var defaults = {
            menuStyle: {
                listStyle: 'none',
                padding: '1px',
                margin: '0px',
                backgroundColor: '#fff',
                border: '1px solid #999',
                width: '100px'
            },
            itemStyle: {
                margin: '0px',
                color: '#000',
                display: 'block',
                cursor: 'default',
                padding: '3px',
                border: '1px solid #fff',
                backgroundColor: 'transparent'
            },
            itemHoverStyle: {
                border: '1px solid #0a246a',
                backgroundColor: '#b6bdd2'
            },
            eventPosX: 'pageX',
            eventPosY: 'pageY',
            shadow: true,
            onContextMenu: null,
            onShowMenu: null
        };
        $.fn.contextMenu = function (id, options) {
            if (!menu) {
                menu = $('<div id="jqContextMenu"></div>').hide().css({
                    position: 'absolute',
                    zIndex: '500'
                }).appendTo('body').bind('click',
                function (e) {
                    e.stopPropagation()
                })
            }
            if (!shadow) {
                shadow = $('<div></div>').css({
                    backgroundColor: '#000',
                    position: 'absolute',
                    opacity: 0.2,
                    zIndex: 499
                }).appendTo('body').hide()
            }
            hash = hash || [];
            hash.push({
                id: id,
                menuStyle: $.extend({},
                defaults.menuStyle, options.menuStyle || {}),
                itemStyle: $.extend({},
                defaults.itemStyle, options.itemStyle || {}),
                itemHoverStyle: $.extend({},
                defaults.itemHoverStyle, options.itemHoverStyle || {}),
                bindings: options.bindings || {},
                shadow: options.shadow || options.shadow === false ? options.shadow : defaults.shadow,
                onContextMenu: options.onContextMenu || defaults.onContextMenu,
                onShowMenu: options.onShowMenu || defaults.onShowMenu,
                eventPosX: options.eventPosX || defaults.eventPosX,
                eventPosY: options.eventPosY || defaults.eventPosY
            });
            var index = hash.length - 1;
            $(this).bind('contextmenu',
            function (e) {
                var bShowContext = (!!hash[index].onContextMenu) ? hash[index].onContextMenu(e) : true;
                if (bShowContext) display(index, this, e, options);
                return false
            });
            return this
        };
        function display(index, trigger, e, options) {
            var cur = hash[index];
            content = $('#' + cur.id).find('ul:first').clone(true);
            content.css(cur.menuStyle).find('li').css(cur.itemStyle).hover(function () {
                $(this).css(cur.itemHoverStyle)
            },
            function () {
                $(this).css(cur.itemStyle)
            }).find('img').css({
                verticalAlign: 'middle',
                paddingRight: '2px'
            });
            menu.html(content);
            if (!!cur.onShowMenu) menu = cur.onShowMenu(e, menu);
            $.each(cur.bindings,
            function (id, func) {
                $('#' + id, menu).bind('click',
                function (e) {
                    hide();
                    func(trigger, currentTarget)
                })
            });
            menu.css({
                'left': e[cur.eventPosX],
                'top': e[cur.eventPosY]
            }).show();
            if (cur.shadow) shadow.css({
                width: menu.width(),
                height: menu.height(),
                left: e.pageX + 2,
                top: e.pageY + 2
            }).show();
            $(document).one('click', hide)
        }
        function hide() {
            menu.hide();
            shadow.hide()
        }
        $.contextMenu = {
            defaults: function (userDefaults) {
                $.each(userDefaults,
                function (i, val) {
                    if (typeof val == 'object' && defaults[i]) {
                        $.extend(defaults[i], val)
                    } else defaults[i] = val
                })
            }
        }
    })(jQuery);
    $(function () {
        $('div.contextMenu').hide()
    });
    //#endregion

    //#region jquery.wk.toolbar-1.0.0.js
    (function ($) {
        $.fn.swapClass = function (c1, c2) {
            return this.removeClass(c1).addClass(c2);
        }
        $.fn.switchClass = function (c1, c2) {
            if (this.hasClass(c1)) {
                return this.swapClass(c1, c2);
            }
            else {
                return this.swapClass(c2, c1);
            }
        }
        $.fn.dtoolbar = function (settings) {
            var dfop = {
                items: null
            }
            $.extend(dfop, settings);
            var me = $(this);
            me.addClass("gTlBar");
            if (dfop.items) {
                for (i = 0; i < dfop.items.length; i++) {
                    me.append(addtoolbar(dfop.items[i], i));
                }
            }
            function addtoolbar(item, numi) {
                var t = {
                    checkbox: false,
                    text: null,
                    ico: null,
                    handler: false,
                    disabled: false
                }
                $.extend(t, item);
                var toolbarEntity = this;
                if (t.text == '-') {
                    //还没想到
                    //TODO
                } else {
                    var bttbar = $("<div></div>");
                    if (t.id == null)
                        bttbar.attr("id", "bt_" + numi);
                    else
                        bttbar.attr("id", "bt_" + t.id);
                    bttbar.addClass('tlbtn2');
                    bttbar.attr({
                        checkbox: t.checkbox,
                        checked: 'false',
                        numberid: numi
                    })
                    bttbar.append('<div class="btnLe"></div>');
                    if (t.id != null) {
                        switch (t.ico) {
                            case "dropdownlist":
                                var se = '<select class="btnselect" id="' + t.id + '">';
                                if (t.text)
                                    bttbar.append('<span class="btnTxt">' + t.text + '</span>');
                                for (j = 0; j < item.list.length; j++) {
                                    se += '<option value="' + item.list[j].value + '">' + item.list[j].txt + '</option>';
                                }
                                bttbar.append(se + '</select>');
                                break;
                            case "textbox":
                                if (t.text)
                                    bttbar.append('<span class="btnTxt">' + t.text + '</span>');
                                bttbar.append('<input type="text" class="btntextbox" id="' + t.id + '"/>');
                                break;
                            case "upload":
                                bttbar.append("<input type='file' name='upload' id='" + t.id + "' />");
                                break;
                            default:
                                bttbar.append('<b class="icoBtn ' + t.ico + '"></b><span class="btnTxt">' + t.text + '</span>');
                                break;
                        }
                    }
                    else
                        bttbar.append('<span class="btnTxt">' + t.text + '</span>');
                    bttbar.append('<div class="btnRi"></div>');
                    if (t.handler) {
                        if (t.ico != "dropdownlist") {
                            bttbar.unbind('click');
                            bttbar.bind('click', t.handler);                         
                        }
                        //else $("#dropdownlist1").change(t.handler);
                    }
                    if (t.disabled) {
                        bttbar.attr("disabled", "disabled");
                        bttbar.addClass('btndisabled');
                        bttbar.unbind('click');                      
                    }
                    bttbar.bind('mouseover', function () {
                        var isdisabled = (bttbar.attr("disabled") == 'disabled') ? true : false;
                        if (isdisabled) {

                        }
                        else {
                            if ($(this).attr("checked") == 'false' || $(this).attr("checked") == false) {
                                $(this).addClass('on');
                                $(this).siblings().removeClass('on');
                            }
                            $(this).bind('mouseout', function () {
                                $(this).removeClass('click');
                                if ($(this).attr("checked") == 'false' || $(this).attr("checked") == false) {
                                    $(this).removeClass('on');
                                }
                                $(this).unbind('mouseout');
                            });
                        }
                    });
                    bttbar.bind('mousedown', function () {
                        var isdisabled = (bttbar.attr("disabled") == 'disabled') ? true : false;
                        if (isdisabled) {
                        }
                        else {
                            $(this).removeClass('on');
                            $(this).addClass('click');
                            $(this).bind('mouseup', function () {
                                $(this).removeClass('click');
                                $(this).addClass('on');
                                $(this).siblings().removeClass('on');
                                $(this).unbind('mouseup');
                            });
                            if ($(this).attr("checkbox") == 'true') {
                                if ($(this).attr("checked") == 'true') {
                                    $(this).attr("checked", 'false');
                                    $(this).removeClass('on');
                                }
                                else {
                                    $(this).attr("checked", 'true');
                                    $(this).addClass('on');
                                    $(this).siblings().removeClass('on');
                                }
                            }
                        }
                    });
                    return bttbar;
                }
            };
            me[0].t = {
                setEnable: function (id, enable) {
                    var bttbar = $("#bt_" + id);
                    if (bttbar.length <= 0) return;
                    var numid = bttbar.attr("numberid");
                    var t = dfop.items[numid];
                    if (!enable) {
                        bttbar.attr("disabled", "disabled");
                        bttbar.addClass('btndisabled');
                        bttbar.unbind('click');
                        //bttbar.data("click", false);
                    } else {
                        bttbar.removeAttr("disabled");
                        bttbar.removeClass('btndisabled');
                        if (t.handler) {            
                            //if (bttbar.data("events") && !bttbar.data("events")["click"])
                            bttbar.unbind('click');
                                bttbar.bind('click', t.handler);
                        }
                    }
                }
            };
            me[0].All = {
                setEnable: function (obj, enable) {
                    var _obj = obj.children("div");
                    for (i = 0; i < _obj.length; i++) {
                        var bttbar = $("#" + _obj[i].id);
                        if (bttbar.length <= 0) return;
                        var numid = bttbar.attr("numberid");
                        var t = dfop.items[numid];
                        if (!enable) {
                            bttbar.attr("disabled", "disabled");
                            bttbar.addClass('btndisabled');
                            bttbar.unbind('click');
                        } else {
                            bttbar.removeAttr("disabled");
                            bttbar.removeClass('btndisabled');             
                            if (t.handler) {
                                //if (bttbar.data("events")&& !bttbar.data("events")["click"])
                                bttbar.unbind('click');
                                    bttbar.bind('click', t.handler);
                            }
                        }
                    }
                }
            };
            return me;
        }
        //将某一个按钮设为可用和不可用
        $.fn.setbtEnable = function (id, enable) {
            if (this[0].t) {                
                return this[0].t.setEnable(id, enable);
            }
            return null;
        }
        //将按钮设为可用和不可用
        $.fn.setAllEnable = function (enable) {
            if (this[0] != null && this[0].All) {
                return this[0].All.setEnable(this, enable);
            }
            return null;
        }
    })(jQuery);
//#endregion

    //#region jquery.caret.js 
    (function ($) {
        $.fn.caret = function (pos) {
            var target = this[0];
            var isContentEditable = target.contentEditable === 'true';
            //get
            if (arguments.length == 0) {
                //HTML5
                if (window.getSelection) {
                    //contenteditable
                    if (isContentEditable) {
                        target.focus();
                        var range1 = window.getSelection().getRangeAt(0),
                            range2 = range1.cloneRange();
                        range2.selectNodeContents(target);
                        range2.setEnd(range1.endContainer, range1.endOffset);
                        return range2.toString().length;
                    }
                    //textarea
                    return target.selectionStart;
                }
                //IE<9
                if (document.selection) {
                    target.focus();
                    //contenteditable
                    if (isContentEditable) {
                        var range1 = document.selection.createRange(),
                            range2 = document.body.createTextRange();
                        range2.moveToElementText(target);
                        range2.setEndPoint('EndToEnd', range1);
                        return range2.text.length;
                    }
                    //textarea
                    var pos = 0,
                        range = target.createTextRange(),
                        range2 = document.selection.createRange().duplicate(),
                        bookmark = range2.getBookmark();
                    range.moveToBookmark(bookmark);
                    while (range.moveStart('character', -1) !== 0) pos++;
                    return pos;
                }
                //not supported
                return 0;
            }
            //set
            if (pos == -1)
                pos = this[isContentEditable ? 'text' : 'val']().length;
            //HTML5
            if (window.getSelection) {
                //contenteditable
                if (isContentEditable) {
                    target.focus();
                    window.getSelection().collapse(target.firstChild, pos);
                }
                    //textarea
                else
                    target.setSelectionRange(pos, pos);
            }
                //IE<9
            else if (document.body.createTextRange) {
                var range = document.body.createTextRange();
                range.moveToElementText(target);
                range.moveStart('character', pos);
                range.collapse(true);
                range.select();
            }
            if (!isContentEditable)
                target.focus();
            return pos;
        }
    })(jQuery)
//#endregion

    //#region log ui error
    function logError(e, msg) {
        var trace = printStackTrace({ e: e });
        var message = "页面错误: " + msg + "\n\n 详细描述: " + e.message + "\n\n stace trace: " + trace;
        var msg = { id: message };

        ajaxPost('/Common/Common/LogError/', JSON.stringify(msg));
    }

    //#region stacktrace.js
// Domain Public by Eric Wendelin http://eriwen.com/ (2008)
//                  Luke Smith http://lucassmith.name/ (2008)
//                  Loic Dachary <loic@dachary.org> (2008)
//                  Johan Euphrosine <proppy@aminche.com> (2008)
//                  Oyvind Sean Kinsey http://kinsey.no/blog (2010)
//                  Victor Homyakov <victor-homyakov@users.sourceforge.net> (2010)
/*global module, exports, define, ActiveXObject*/
(function (global, factory) {
    if (typeof exports === 'object') {
        // Node
        module.exports = factory();
    } else if (typeof define === 'function' && define.amd) {
        // AMD
        define(factory);
    } else {
        // Browser globals
        global.printStackTrace = factory();
    }
}(this, function () {
    /**
     * Main function giving a function stack trace with a forced or passed in Error
     *
     * @cfg {Error} e The error to create a stacktrace from (optional)
     * @cfg {Boolean} guess If we should try to resolve the names of anonymous functions
     * @return {Array} of Strings with functions, lines, files, and arguments where possible
     */
    function printStackTrace(options) {
        options = options || { guess: true };
        var ex = options.e || null, guess = !!options.guess;
        var p = new printStackTrace.implementation(), result = p.run(ex);
        return (guess) ? p.guessAnonymousFunctions(result) : result;
    }

    printStackTrace.implementation = function () {
    };

    printStackTrace.implementation.prototype = {
        /**
         * @param {Error} ex The error to create a stacktrace from (optional)
         * @param {String} mode Forced mode (optional, mostly for unit tests)
         */
        run: function (ex, mode) {
            ex = ex || this.createException();
            // examine exception properties w/o 
            //for (var prop in ex) {alert("Ex['" + prop + "']=" + ex[prop]);}
            mode = mode || this.mode(ex);
            if (mode === 'other') {
                return this.other(arguments.callee);
            } else {
                return this[mode](ex);
            }
        },

        createException: function () {
            try {
                this.undef();
            } catch (e) {
                return e;
            }
        },

        /**
         * Mode could differ for different exception, e.g.
         * exceptions in Chrome may or may not have arguments or stack.
         *
         * @return {String} mode of operation for the exception
         */
        mode: function (e) {
            if (e['arguments'] && e.stack) {
                return 'chrome';
            } else if (e.stack && e.sourceURL) {
                return 'safari';
            } else if (e.stack && e.number) {
                return 'ie';
            } else if (e.stack && e.fileName) {
                return 'firefox';
            } else if (e.message && e['opera#sourceloc']) {
                // e.message.indexOf("Backtrace:") > -1 -> opera9
                // 'opera#sourceloc' in e -> opera9, opera10a
                // !e.stacktrace -> opera9
                if (!e.stacktrace) {
                    return 'opera9'; // use e.message
                }
                if (e.message.indexOf('\n') > -1 && e.message.split('\n').length > e.stacktrace.split('\n').length) {
                    // e.message may have more stack entries than e.stacktrace
                    return 'opera9'; // use e.message
                }
                return 'opera10a'; // use e.stacktrace
            } else if (e.message && e.stack && e.stacktrace) {
                // e.stacktrace && e.stack -> opera10b
                if (e.stacktrace.indexOf("called from line") < 0) {
                    return 'opera10b'; // use e.stacktrace, format differs from 'opera10a'
                }
                // e.stacktrace && e.stack -> opera11
                return 'opera11'; // use e.stacktrace, format differs from 'opera10a', 'opera10b'
            } else if (e.stack && !e.fileName) {
                // Chrome 27 does not have e.arguments as earlier versions,
                // but still does not have e.fileName as Firefox
                return 'chrome';
            }
            return 'other';
        },

        /**
         * Given a context, function name, and callback function, overwrite it so that it calls
         * printStackTrace() first with a callback and then runs the rest of the body.
         *
         * @param {Object} context of execution (e.g. window)
         * @param {String} functionName to instrument
         * @param {Function} callback function to call with a stack trace on invocation
         */
        instrumentFunction: function (context, functionName, callback) {
            context = context || window;
            var original = context[functionName];
            context[functionName] = function instrumented() {
                callback.call(this, printStackTrace().slice(4));
                return context[functionName]._instrumented.apply(this, arguments);
            };
            context[functionName]._instrumented = original;
        },

        /**
         * Given a context and function name of a function that has been
         * instrumented, revert the function to it's original (non-instrumented)
         * state.
         *
         * @param {Object} context of execution (e.g. window)
         * @param {String} functionName to de-instrument
         */
        deinstrumentFunction: function (context, functionName) {
            if (context[functionName].constructor === Function &&
                context[functionName]._instrumented &&
                context[functionName]._instrumented.constructor === Function) {
                context[functionName] = context[functionName]._instrumented;
            }
        },

        /**
         * Given an Error object, return a formatted Array based on Chrome's stack string.
         *
         * @param e - Error object to inspect
         * @return Array<String> of function calls, files and line numbers
         */
        chrome: function (e) {
            return (e.stack + '\n')
                .replace(/^\s+(at eval )?at\s+/gm, '') // remove 'at' and indentation
                .replace(/^([^\(]+?)([\n$])/gm, '{anonymous}() ($1)$2')
                .replace(/^Object.<anonymous>\s*\(([^\)]+)\)/gm, '{anonymous}() ($1)')
                .replace(/^(.+) \((.+)\)$/gm, '$1@$2')
                .split('\n')
                .slice(1, -1);
        },

        /**
         * Given an Error object, return a formatted Array based on Safari's stack string.
         *
         * @param e - Error object to inspect
         * @return Array<String> of function calls, files and line numbers
         */
        safari: function (e) {
            return e.stack.replace(/\[native code\]\n/m, '')
                .replace(/^(?=\w+Error\:).*$\n/m, '')
                .replace(/^@/gm, '{anonymous}()@')
                .split('\n');
        },

        /**
         * Given an Error object, return a formatted Array based on IE's stack string.
         *
         * @param e - Error object to inspect
         * @return Array<String> of function calls, files and line numbers
         */
        ie: function (e) {
            return e.stack
                .replace(/^\s*at\s+(.*)$/gm, '$1')
                .replace(/^Anonymous function\s+/gm, '{anonymous}() ')
                .replace(/^(.+)\s+\((.+)\)$/gm, '$1@$2')
                .split('\n')
                .slice(1);
        },

        /**
         * Given an Error object, return a formatted Array based on Firefox's stack string.
         *
         * @param e - Error object to inspect
         * @return Array<String> of function calls, files and line numbers
         */
        firefox: function (e) {
            return e.stack.replace(/(?:\n@:0)?\s+$/m, '')
                .replace(/^(?:\((\S*)\))?@/gm, '{anonymous}($1)@')
                .split('\n');
        },

        opera11: function (e) {
            var ANON = '{anonymous}', lineRE = /^.*line (\d+), column (\d+)(?: in (.+))? in (\S+):$/;
            var lines = e.stacktrace.split('\n'), result = [];

            for (var i = 0, len = lines.length; i < len; i += 2) {
                var match = lineRE.exec(lines[i]);
                if (match) {
                    var location = match[4] + ':' + match[1] + ':' + match[2];
                    var fnName = match[3] || "global code";
                    fnName = fnName.replace(/<anonymous function: (\S+)>/, "$1").replace(/<anonymous function>/, ANON);
                    result.push(fnName + '@' + location + ' -- ' + lines[i + 1].replace(/^\s+/, ''));
                }
            }

            return result;
        },

        opera10b: function (e) {
            // "<anonymous function: run>([arguments not available])@file://localhost/G:/js/stacktrace.js:27\n" +
            // "printStackTrace([arguments not available])@file://localhost/G:/js/stacktrace.js:18\n" +
            // "@file://localhost/G:/js/test/functional/testcase1.html:15"
            var lineRE = /^(.*)@(.+):(\d+)$/;
            var lines = e.stacktrace.split('\n'), result = [];

            for (var i = 0, len = lines.length; i < len; i++) {
                var match = lineRE.exec(lines[i]);
                if (match) {
                    var fnName = match[1] ? (match[1] + '()') : "global code";
                    result.push(fnName + '@' + match[2] + ':' + match[3]);
                }
            }

            return result;
        },

        /**
         * Given an Error object, return a formatted Array based on Opera 10's stacktrace string.
         *
         * @param e - Error object to inspect
         * @return Array<String> of function calls, files and line numbers
         */
        opera10a: function (e) {
            // "  Line 27 of linked script file://localhost/G:/js/stacktrace.js\n"
            // "  Line 11 of inline#1 script in file://localhost/G:/js/test/functional/testcase1.html: In function foo\n"
            var ANON = '{anonymous}', lineRE = /Line (\d+).*script (?:in )?(\S+)(?:: In function (\S+))?$/i;
            var lines = e.stacktrace.split('\n'), result = [];

            for (var i = 0, len = lines.length; i < len; i += 2) {
                var match = lineRE.exec(lines[i]);
                if (match) {
                    var fnName = match[3] || ANON;
                    result.push(fnName + '()@' + match[2] + ':' + match[1] + ' -- ' + lines[i + 1].replace(/^\s+/, ''));
                }
            }

            return result;
        },

        // Opera 7.x-9.2x only!
        opera9: function (e) {
            // "  Line 43 of linked script file://localhost/G:/js/stacktrace.js\n"
            // "  Line 7 of inline#1 script in file://localhost/G:/js/test/functional/testcase1.html\n"
            var ANON = '{anonymous}', lineRE = /Line (\d+).*script (?:in )?(\S+)/i;
            var lines = e.message.split('\n'), result = [];

            for (var i = 2, len = lines.length; i < len; i += 2) {
                var match = lineRE.exec(lines[i]);
                if (match) {
                    result.push(ANON + '()@' + match[2] + ':' + match[1] + ' -- ' + lines[i + 1].replace(/^\s+/, ''));
                }
            }

            return result;
        },

        // Safari 5-, IE 9-, and others
        other: function (curr) {
            var ANON = '{anonymous}', fnRE = /function\s*([\w\-$]+)?\s*\(/i, stack = [], fn, args, maxStackSize = 10;
            while (curr && curr['arguments'] && stack.length < maxStackSize) {
                fn = fnRE.test(curr.toString()) ? RegExp.$1 || ANON : ANON;
                args = Array.prototype.slice.call(curr['arguments'] || []);
                stack[stack.length] = fn + '(' + this.stringifyArguments(args) + ')';
                curr = curr.caller;
            }
            return stack;
        },

        /**
         * Given arguments array as a String, substituting type names for non-string types.
         *
         * @param {Arguments,Array} args
         * @return {String} stringified arguments
         */
        stringifyArguments: function (args) {
            var result = [];
            var slice = Array.prototype.slice;
            for (var i = 0; i < args.length; ++i) {
                var arg = args[i];
                if (arg === undefined) {
                    result[i] = 'undefined';
                } else if (arg === null) {
                    result[i] = 'null';
                } else if (arg.constructor) {
                    if (arg.constructor === Array) {
                        if (arg.length < 3) {
                            result[i] = '[' + this.stringifyArguments(arg) + ']';
                        } else {
                            result[i] = '[' + this.stringifyArguments(slice.call(arg, 0, 1)) + '...' + this.stringifyArguments(slice.call(arg, -1)) + ']';
                        }
                    } else if (arg.constructor === Object) {
                        result[i] = '#object';
                    } else if (arg.constructor === Function) {
                        result[i] = '#function';
                    } else if (arg.constructor === String) {
                        result[i] = '"' + arg + '"';
                    } else if (arg.constructor === Number) {
                        result[i] = arg;
                    }
                }
            }
            return result.join(',');
        },

        sourceCache: {},

        /**
         * @return the text from a given URL
         */
        ajax: function (url) {
            var req = this.createXMLHTTPObject();
            if (req) {
                try {
                    req.open('GET', url, false);
                    //req.overrideMimeType('text/plain');
                    //req.overrideMimeType('text/javascript');
                    req.send(null);
                    //return req.status == 200 ? req.responseText : '';
                    return req.responseText;
                } catch (e) {
                }
            }
            return '';
        },

        /**
         * Try XHR methods in order and store XHR factory.
         *
         * @return <Function> XHR function or equivalent
         */
        createXMLHTTPObject: function () {
            var xmlhttp, XMLHttpFactories = [
                function () {
                    return new XMLHttpRequest();
                }, function () {
                    return new ActiveXObject('Msxml2.XMLHTTP');
                }, function () {
                    return new ActiveXObject('Msxml3.XMLHTTP');
                }, function () {
                    return new ActiveXObject('Microsoft.XMLHTTP');
                }
            ];
            for (var i = 0; i < XMLHttpFactories.length; i++) {
                try {
                    xmlhttp = XMLHttpFactories[i]();
                    // Use memoization to cache the factory
                    this.createXMLHTTPObject = XMLHttpFactories[i];
                    return xmlhttp;
                } catch (e) {
                }
            }
        },

        /**
         * Given a URL, check if it is in the same domain (so we can get the source
         * via Ajax).
         *
         * @param url <String> source url
         * @return <Boolean> False if we need a cross-domain request
         */
        isSameDomain: function (url) {
            return typeof location !== "undefined" && url.indexOf(location.hostname) !== -1; // location may not be defined, e.g. when running from nodejs.
        },

        /**
         * Get source code from given URL if in the same domain.
         *
         * @param url <String> JS source URL
         * @return <Array> Array of source code lines
         */
        getSource: function (url) {
            // TODO reuse source from script tags?
            if (!(url in this.sourceCache)) {
                this.sourceCache[url] = this.ajax(url).split('\n');
            }
            return this.sourceCache[url];
        },

        guessAnonymousFunctions: function (stack) {
            for (var i = 0; i < stack.length; ++i) {
                var reStack = /\{anonymous\}\(.*\)@(.*)/,
                    reRef = /^(.*?)(?::(\d+))(?::(\d+))?(?: -- .+)?$/,
                    frame = stack[i], ref = reStack.exec(frame);

                if (ref) {
                    var m = reRef.exec(ref[1]);
                    if (m) { // If falsey, we did not get any file/line information
                        var file = m[1], lineno = m[2], charno = m[3] || 0;
                        if (file && this.isSameDomain(file) && lineno) {
                            var functionName = this.guessAnonymousFunction(file, lineno, charno);
                            stack[i] = frame.replace('{anonymous}', functionName);
                        }
                    }
                }
            }
            return stack;
        },

        guessAnonymousFunction: function (url, lineNo, charNo) {
            var ret;
            try {
                ret = this.findFunctionName(this.getSource(url), lineNo);
            } catch (e) {
                ret = 'getSource failed with url: ' + url + ', exception: ' + e.toString();
            }
            return ret;
        },

        findFunctionName: function (source, lineNo) {
            // FIXME findFunctionName fails for compressed source
            // (more than one function on the same line)
            // function {name}({args}) m[1]=name m[2]=args
            var reFunctionDeclaration = /function\s+([^(]*?)\s*\(([^)]*)\)/;
            // {name} = function ({args}) TODO args capture
            // /['"]?([0-9A-Za-z_]+)['"]?\s*[:=]\s*function(?:[^(]*)/
            var reFunctionExpression = /['"]?([$_A-Za-z][$_A-Za-z0-9]*)['"]?\s*[:=]\s*function\b/;
            // {name} = eval()
            var reFunctionEvaluation = /['"]?([$_A-Za-z][$_A-Za-z0-9]*)['"]?\s*[:=]\s*(?:eval|new Function)\b/;
            // Walk backwards in the source lines until we find
            // the line which matches one of the patterns above
            var code = "", line, maxLines = Math.min(lineNo, 20), m, commentPos;
            for (var i = 0; i < maxLines; ++i) {
                // lineNo is 1-based, source[] is 0-based
                line = source[lineNo - i - 1];
                commentPos = line.indexOf('//');
                if (commentPos >= 0) {
                    line = line.substr(0, commentPos);
                }
                // TODO check other types of comments? Commented code may lead to false positive
                if (line) {
                    code = line + code;
                    m = reFunctionExpression.exec(code);
                    if (m && m[1]) {
                        return m[1];
                    }
                    m = reFunctionDeclaration.exec(code);
                    if (m && m[1]) {
                        //return m[1] + "(" + (m[2] || "") + ")";
                        return m[1];
                    }
                    m = reFunctionEvaluation.exec(code);
                    if (m && m[1]) {
                        return m[1];
                    }
                }
            }
            return '(?)';
        }
    };

    return printStackTrace;
}));
//#endregion
//#endregion

    //#region firefox report print button
    var FFReport;
    if (!FFReport) FFReport = {};
    FFReport = (function () {
        function $_create(elem, tag, target) { return addElem(elem, target, tag) }
        function $_add(elem, target) { return addElem(elem, target) }
        function $_GB() { return GetBrowser(); }

        function GetBrowser() {
            if (Sys.ie)
                return 'IE';
            else
                return 'FF';
        }

        function addElem(elem, target, tag) {
            if (typeof elem === 'string') {
                var el = document.getElementById(elem);
                if (!el) {

                    el = document.createElement(tag);

                    el.id = elem;
                }
                elem = el;
            }

            if (target) {

                var dest;
                if (typeof target === 'string')
                    dest = document.getElementById(target);
                else
                    dest = target;


                dest.appendChild(elem);

            }
            return elem;
        }

        function $_insert(printpage) {
            var contents = $("#" + printpage).html();

            if ($("#printDiv").length == 0) {
                var printDiv = null;

                printDiv = document.createElement('div');
                printDiv.setAttribute('id', 'printDiv');
                printDiv.setAttribute('class', 'printable');
                $(printDiv).appendTo('body');
            }

            $("#printDiv").html(contents);
            window.print();
            $("#printDiv").remove();

            return false;
        }

        return {
            "$_GB": $_GB,
            "$_add": $_add,
            "$_create": $_create,
            "$_insert": $_insert
        };
    })();
    //#endregion
