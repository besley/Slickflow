/*------------------------------------------|
| 可输入可选下拉框                        　|
|-------------------------------------------|
| 源码引自：http://blog.csdn.net/cxzhq2002　|
| 修 改 BY: jiangzhengjun  2008-12-16       |
| 新增功能：支持模糊定位、支持上下箭选择、  |
| 支持注释层功能、支持按回车键从下拉框中  　|
| 选择选项                              　　|
|------------------------------------------*/

//下拉框选项所对应的层的名字
var SELECT_DIV="SELECT_DIV_";

//注释层的名字
var NODE_DIV="NODE_DIV_";
var textObject;

//焦点是否在选择层上:初始时为false,表示默认不在选择层上
//主要防止鼠标点击选择项时，文本框会失去焦点，这样选择层就会跟着隐藏，此时还未
//让点击的选择项选中并赋值到文本框中去。此时可以设置鼠标在选择层上时cursorInSelectDivObj=ture
//这时点击时不会立即隐藏选择层，等选中后再设置cursorInSelectDivObj=false，此时就可以隐藏选择层了
var cursorInSelectDivObj=false;

//是否是ie浏览器
var ie=(document.getElementById && document.all);

//全局的注释数组
var noteArr = new Array();


//以防名字已存在，循环取名，先判断是否存在"Textselectshow_Div"的对象，
//如果存在，则重新取名为"Textselectshow_Div1"，如果"Textselectshow_Div1"
//还是存在，则取名为"Textselectshow_Div2"，依次类推:"Textselectshow_Div..."
for(var i=0;document.getElementById(SELECT_DIV)!=null;i++){	
	var tmpNm = SELECT_DIV + i;
	//如果存在同名的，则以重新取名为 Textselectshow_Div + i ，如果Textselectshow_Div + i
	//存在，则循环取名为 Textselectshow_Div + i + 1，直到不重名为止。如果存在，则赋值为本身再循环
	SELECT_DIV=(document.getElementById(tmpNm)==null)?tmpNm:SELECT_DIV;
}

//以同样的命名方式为注释层取名
for(var i=0;document.getElementById(NODE_DIV)!=null;i++){
	var tmpNm = NODE_DIV + i;
	NODE_DIV=(document.getElementById(tmpNm)==null)?tmpNm:NODE_DIV;
}

//为隐藏下拉框创建一个对应的层，并且刚初始化时为隐藏的
document.write ('<div id="' + SELECT_DIV + '" style="position: absolute;'
								+ 'cursor: default;border: 1px solid #B2B2B2;background-color: #fff;display: none;"></div>')
//同样方式创建一个注释层
document.write ('<div id="' + NODE_DIV + '"  style="position: absolute;'
								+ 'cursor: default;border: 1px solid #B2B2B2;background-color:#ffffd9;display: none;'
								+ 'overflow-x:auto;word-wrap:break-word"></div>')

// 获取某对象的坐标
function getObjPosition(Obj){
	try{		
		var sumTop=0;
		var sumLeft=0;
		
		while(Obj!=window.document.body){
			sumTop+=Obj.offsetTop;
			sumLeft+=Obj.offsetLeft;	
			Obj=Obj.offsetParent;
		}
		return {left:sumLeft,top:sumTop};
	}catch(e){alert(e);}
}

//处理Div中的选项/* 某个选项，文本框的ID */
function optionDivOnmouseover(optionDivObj,textId){
	//文本框
	var textObj=document.getElementById(textId);

	//optionDivObj.parentNode为某select option选项所对应层的父对象，即SELECT_DIV层
	//得到select下拉框所有option选项所对应的层
	var objChilddiv=optionDivObj.parentNode.getElementsByTagName("div");
	
	//清空所有选项层的样式，即去掉原来背景为蓝色的选项层的样式
	for(var i=0; i < objChilddiv.length; i++){
		objChilddiv[i].style.cssText='';
	}
	
	//使本身选项层的背景为蓝色，字为白色，模拟选中样式
	optionDivObj.style.cssText = 'background-color: #479ff3;color: #ffffff;'
 	
 	var noteDivObj =document.getElementById(NODE_DIV);
	var selectDivObj =document.getElementById(SELECT_DIV);
	
	///////设置注释层中的选项及位置
 	setNoteDivObj(textObj,optionDivObj,selectDivObj,noteDivObj);	
 
	//点击某个选项层时
	optionDivObj.onclick=function(){

		//点击选项后选择层后要隐藏，即要设置成失去焦点状态
		cursorInSelectDivObj=false;
		
		//把选中的某选项层的内容赋值为文本框
		if(ie){
			textObj.value=optionDivObj.outerText;  	
		}
		else
		{
			textObj.value=optionDivObj.textContent;
		}
		
		//var noteDivObj =document.getElementById(NODE_DIV);
		
		//点击某个选项层时，对应的注释层也要隐藏
		noteDivObj.style.display='none';		

		//点击过后使文本框获取焦点
		textObj.focus();

		////////调用文本框失去焦点触发的方法
		textObjectBlur(selectDivObj,noteDivObj);		
	}
}

/**
* textObj:文本框
* seleObj:下拉框
* noteArr:noteArr 注釋數組，沒有可以不传或为null或空数组
*/
function showSelect(textObj,seleObj,arrNote){	
	textObject = textObj;
	
	//保存全局注释，供其他方法使用
	noteArr = arrNote;
	var selectDivObj =document.getElementById(SELECT_DIV);
	var noteDivObj =document.getElementById(NODE_DIV);
	var seleObj =document.getElementById(seleObj);
	

	///////鼠标移出下拉框层时
	selectDivObj.onmouseout=function(){		
		//当鼠标移出选择层时，设置选择层为失去焦点状态	
		cursorInSelectDivObj=false;
		
		//当鼠标移出选择层时，让文本框获取焦点
		textObj.focus();
	}
	
	///////文本框失去焦点时
	textObj.onblur=function(){
		textObjectBlur(selectDivObj,noteDivObj);		
	}
	
	///////鼠标经过下拉框层时
	selectDivObj.onmouseover=function(){
		//当鼠标移进选择层时，设置选择层为获得焦点状态	
		cursorInSelectDivObj=true;
	}
		
	///////文本框点击时
	textObj.onclick=function(){
 		//设置下拉框对应层中的选项及位置
		setSelectDivObj(textObj,selectDivObj,seleObj);

		//设置注释层中的选项及位置
		setNoteDivObj(textObj,null,selectDivObj,noteDivObj);
		
		//自动匹配与模糊定位
		autoMatch(textObj,selectDivObj);	
 	}
 	
 	///////文本框上输入时
 	textObj.onkeyup=function(){
 		//如果按的是Tab键时直接退出
 		if(event.keyCode==9){
 			return false;
 		}
 		
 		if(event.keyCode==13){
 			enter(textObj,selectDivObj,noteDivObj);	
 			return false;
 		}
 	
		//设置下拉框对应层中的选项及位置
		setSelectDivObj(textObj,selectDivObj,seleObj);
		
		//如果按了上下键时
 		if(event.keyCode == 38 || event.keyCode == 40 ){
 			var selectedOptionDiv = downOrUp(textObj,selectDivObj,noteDivObj,seleObj); 		
 			//设置注释层中的选项及位置
			setNoteDivObj(textObj,selectedOptionDiv,selectDivObj,noteDivObj);		
 		}else{
 			//设置注释层中的选项及位置
			setNoteDivObj(textObj,null,selectDivObj,noteDivObj);		
 		} 	
 		
 		//自动匹配与模糊定位
		autoMatch(textObj,selectDivObj);
 	}
}

// 隐藏遮挡ID为objID的对象（层）的所有select
function hiddenOverSelects(objID){
	var sels = document.getElementsByTagName('select'); 
	for (var i = 0; i < sels.length; i++){
		if (obj1OverObj2(document.getElementById(objID), sels[i])){
			sels[i].style.visibility = 'hidden'; 	
		}else{
			sels[i].style.visibility = 'visible';	
		}		
	}
}

//判断obj1是否遮挡了obj2
function obj1OverObj2(obj1, obj2){
	var pos1 = getObjPosition(obj1) 
	var pos2 = getObjPosition(obj2) 
	var result = true; 
	var obj1Left = pos1.left - window.document.body.scrollLeft; 
	var obj1Top = pos1.top - window.document.body.scrollTop; 
	var obj1Right = obj1Left + obj1.offsetWidth; 
	var obj1Bottom = obj1Top + obj1.offsetHeight;
	var obj2Left = pos2.left - window.document.body.scrollLeft; 
	var obj2Top = pos2.top - window.document.body.scrollTop; 
	var obj2Right = obj2Left + obj2.offsetWidth; 
	var obj2Bottom = obj2Top + obj2.offsetHeight;
	
	if (obj1Right <= obj2Left || obj1Bottom <= obj2Top || 
	obj1Left >= obj2Right || obj1Top >= obj2Bottom) 
	result = false; 
	return result; 
}


//文本框失去焦点时调用的方法
function textObjectBlur(selectDivObj,noteDivObj){
		//如果点击了某个选项后，已设置选择层为失去焦点状态，此时选择层可以隐藏了
		if(!cursorInSelectDivObj){
			selectDivObj.style.display='none';
		}
		
		if(ie){
			//恢复所有已被隐藏的下拉框
			hiddenOverSelects(selectDivObj.id);	
			if(noteDivObj.style.display=='inline'){
				noteDivObj.style.display=selectDivObj.style.display;
			}
			
			if(selectDivObj.style.display=='none'){
				noteDivObj.style.display='none';
			}
		}	
}

//设置下拉框对应层中的选项及层位置
function setSelectDivObj(textObj,selectDivObj,seleObj){
	//如果已显示，则直接退出
	if(selectDivObj.style.display=='inline'){
		return false;
	}

	//如果文本框的id为空时，则要命名
	for(var i=0;textObj.id=='';i++){
		var tmpNm = "textSelect" + i;
		textObj.id = (document.getElementById(tmpNm)==null)?tmpNm:'';
	}

	selectDivObj.style.left = getObjPosition(textObj).left;
	selectDivObj.style.top = getObjPosition(textObj).top + textObj.offsetHeight;
	selectDivObj.style.width = textObj.offsetWidth;
	selectDivObj.style.height = '';
	selectDivObj.style.overflowY = '';
	selectDivObj.innerHTML='' 
  
	//读取select的项目放到Div里。
	for(var x = 0; x<seleObj.options.length; x++){
		selectDivObj.innerHTML+="<div onmouseover=\"optionDivOnmouseover(this,'" 
		+ textObj.id 
		+ "')\" style='width:100%;white-space: nowrap;cursor: default;'>"
		+seleObj.options[x].text+"</div>";
	}
	
 	//调整Div高度，过度显示滚动条
	if(x > 8){
		selectDivObj.style.height=13 * 8;
		selectDivObj.style.overflowY='auto';
	}else{
		selectDivObj.style.height=15 * x;
		selectDivObj.style.overflowY='auto';
	}
 
	selectDivObj.style.display='inline';
	if(ie){
		hiddenOverSelects(selectDivObj.id);
	}
}

//设置下拉框对应层中的选项及层位置
function setNoteDivObj(textObj,optionDivObj,selectDivObj,noteDivObj){

	 //如果需要显示对应键的注释时
	if(noteArr != null && noteArr != undefined){
			
			//获取下拉框所对应的层的宽度与左边距
			var regStr = new RegExp("(([0-9]+)px)");		
			selectDivObj.style.width.match(regStr);
			var width=RegExp.$2;
			regStr = new RegExp("(([0-9]+)px)");
			selectDivObj.style.left.match(regStr);
			var left= RegExp.$2;
			//设置注释层的位置与大小
			noteDivObj.style.left=parseInt(width) +parseInt(left);
			noteDivObj.style.top=selectDivObj.style.top;			
			noteDivObj.style.width=width*1;
			noteDivObj.style.height=selectDivObj.style.height;
		
			var i = 0;			

			//如果找到对应的注释，则显示注释层
			for(i = 0;i < noteArr.length;i++){
				if(optionDivObj==null && textObj.value == noteArr[i][0]){
					noteDivObj.innerText=noteArr[i][1];
					noteDivObj.style.display="inline";
					break;
				}else if(optionDivObj !=undefined 
					&& optionDivObj !=null 
					&& optionDivObj.outerText == noteArr[i][0]){
					noteDivObj.innerText=noteArr[i][1];
					noteDivObj.style.display="inline";
					break;
				}
			}
			
			if(i==noteArr.length){				
				noteDivObj.innerText='';
				noteDivObj.style.display="none";
			}
	
	}
}

//自动匹配选项中符合文本框中输入的值的记录
function autoMatch(textObj,selectDivObj){
 	if(textObj.value==''){
  	return null;
  }
  
  if(event.keyCode == 38 || event.keyCode == 40 ){
  	return null;
  } 

	return autoMatch_(textObj,selectDivObj);
}

//String.fromCharCode
function autoMatch_(textObj,selectDivObj){
  var objChilddiv=selectDivObj.getElementsByTagName("div");  
  var arr = new Array();
  //清除所有层的样式
  for(var x=0;x<objChilddiv.length;x++){
  	objChilddiv[x].style.cssText='';
  }
	

	var selectOptionDivObj = null;
	var textValueReg = replaceReg(textObj.value);
	
	for(var x=0;x<objChilddiv.length;x++){
		var strChilddiv=(ie)?objChilddiv[x].outerText:textObj.textContent;
		var regRegExp = new RegExp('^'+textValueReg);	
		//alert('^'+textValueReg + "   " + strChilddiv + "   " + textObj.value);
		
		//先模糊匹配
		if(regRegExp.test(strChilddiv)){
			//让模糊匹配到的选项上移
			selectDivObj.scrollTop=objChilddiv[x].offsetHeight*x;		
			
			//再精确匹配，且让精确匹配的成选中状态
			if(strChilddiv==textObj.value){
				arr[0]=objChilddiv[x];
				arr[1]=x;
				objChilddiv[x].style.cssText = 'background-color: #479ff3;color: #ffffff;';
				break;
			}else{
				objChilddiv[x].style.cssText='';
			}
			break;
		}else{
			objChilddiv[x].style.cssText='';
		}
		textObj.focus();
	}

	return arr;
}


//上下翻
function downOrUp(textObj,selectDivObj,noteDivObj,seleObj){

	//得到select下拉框所有option选项所对应的层
	var objChilddiv=selectDivObj.getElementsByTagName("div");
	
	if(objChilddiv.length == 0){
		return null;
	}
	var selectedOptionDiv;
	
	var hig = 0;
	if(event.keyCode==38){
		selectedOptionDiv = objChilddiv[objChilddiv.length -1];	
		hig = objChilddiv.length -1;
	}else if(event.keyCode==40){
		selectedOptionDiv = objChilddiv[0];
		hig = 0;
	}else{
		selectedOptionDiv = objChilddiv[0];
	}
	var i=0;
	//清空所有选项层的样式，即去掉原来背景为蓝色的选项层的样式
	for(i=0; i < objChilddiv.length; i++){		
	    if (objChilddiv[i].style.backgroundColor == '#479ff3') {
			if(event.keyCode==38){
				if(i != 0){
					selectedOptionDiv = objChilddiv[i - 1];
					hig = i - 1;
				}else{
					selectedOptionDiv = objChilddiv[objChilddiv.length - 1];
					hig = objChilddiv.length -1;
				}				
			}else if(event.keyCode==40){
				if(i != (objChilddiv.length -1)){
					selectedOptionDiv = objChilddiv[i + 1];
					hig = i + 1;
				}else{
					selectedOptionDiv = objChilddiv[0];
					hig = 0;
				}				
			}			
			objChilddiv[i].style.cssText='';			
			break;
		}
	}
	
	//解决用上下键弹出下拉列表时，让列表中与文本框相同值的选项选中
	if(i==objChilddiv.length){
		
		//自动匹配与模糊定位
		var selectOption = autoMatch_(textObj,selectDivObj);
		
		if(selectOption.length != 0){
			//设置注释层中的选项及位置
			setNoteDivObj(textObj,selectOption,selectDivObj,noteDivObj);
			selectedOptionDiv = selectOption[0];
			hig = selectOption[1];
		}
	}
	
	selectDivObj.scrollTop=selectedOptionDiv.offsetHeight*hig;

	//使本身选项层的背景为蓝色，字为白色，模拟选中样式
	selectedOptionDiv.style.cssText = 'background-color: #479ff3;color: #ffffff;'

 	textObj.focus();
 
 	return selectedOptionDiv;
}


//回车
function enter(textObj,selectDivObj,noteDivObj){
	if(selectDivObj.style.display=='none'){
		return false;
	}	
	
	//得到select下拉框所有option选项所对应的层
	var objChilddiv=selectDivObj.getElementsByTagName("div");
	
	if(objChilddiv.length == 0){
		return false;
	}
	var selectedOptionDiv;
	
		
	//清空所有选项层的样式，即去掉原来背景为蓝色的选项层的样式
	for(var i=0; i < objChilddiv.length; i++){		
	    if (objChilddiv[i].style.backgroundColor == '#479ff3') {
			textObj.value=(ie)?objChilddiv[i].outerText:objChilddiv[i].textContent;
			//回车时相当于点击了某个选项，此时设置选择层为失去焦点状态
			//再调用文本框失去焦点方法textObjectBlur让选择层隐藏
			cursorInSelectDivObj=false;
			textObjectBlur(selectDivObj,noteDivObj);
			break;
		}
	}
}

var regChars = new Array();
regChars[0]=new Array();
regChars[0][0]="$";
regChars[0][1]="\\$";
regChars[1]=new Array();
regChars[1][0]="(";
regChars[1][1]="\\(";
regChars[2]=new Array();
regChars[2][0]=")";
regChars[2][1]="\\)";
regChars[3]=new Array();
regChars[3][0]="*";
regChars[3][1]="\\*";
regChars[4]=new Array();
regChars[4][0]="+";
regChars[4][1]="\\+";
regChars[5]=new Array();
regChars[5][0]=".";
regChars[5][1]="\\.";
regChars[6]=new Array();
regChars[6][0]="[";
regChars[6][1]="\\[";
regChars[7]=new Array();
regChars[7][0]="?";
regChars[7][1]="\\?";
regChars[8]=new Array();
regChars[8][0]="]";
regChars[8][1]="\\]";
regChars[9]=new Array();
regChars[9][0]="^";
regChars[9][1]="\\^";
regChars[10]=new Array();
regChars[10][0]="|";
regChars[10][1]="\\|";
regChars[11]=new Array();
regChars[11][0]="{";
regChars[11][1]="\\{";
regChars[12]=new Array();
regChars[12][0]="}";
regChars[12][1]="\\}";
regChars[13]=new Array();
regChars[13][0]="\\";
regChars[13][1]="\\\\";

//代换正则表达式中特殊字符
function replaceReg(str){
		
		//$()*+.[?]^|}{\
		var regStr =/[$()*+.\[?\]^|}{\\]/g;
		
		if(!str.match(regStr)){
			return str;
		}

		var regArr =/./g;
		var valueArr = str.match(regArr);
		var tempStr = "";

		for(var i = 0 ; i < valueArr.length; i++){			
			regStr =/[$()*+.\[?\]^|}{\\]/g;
				
			if(valueArr[i].match(regStr)){			
				valueArr[i] = findByKey(valueArr[i])[1];
			}
			
			tempStr = tempStr + valueArr[i];
		}

		return tempStr;
}

//查询正则特殊字符要替换字符串
function findByKey(key){
	var i = 0;
	for(var i = 0; i < regChars.length; i++){
		if(regChars[i][0]==key){
			return regChars[i];
		}
	}
	if(i == regChars.length ){
		return null;
	}
}

//在最后一个输入元素上按回车键时自动提交
function keydownOnSelectInput(){

	if(event.srcElement.type == undefined){
		return;
	}	
	var type = event.srcElement.type.toLowerCase();
	if(event.keyCode!=13 
	|| type=='button' 
	|| type=='submit' 
	|| type=='reset' 
	|| type=='textarea' 
	|| type==''){
		return;
	}
	
	var noteDivObj =document.getElementById(NODE_DIV);
	var selectDivObj =document.getElementById(SELECT_DIV);
	
	if(event.srcElement.nextSibling != null 
	&& event.srcElement.nextSibling.type=='select-one'
	&& selectDivObj.style.display=='inline'){
		
		var objChilddiv=selectDivObj.getElementsByTagName("div");
		var i=0;
		for(i=0; i < objChilddiv.length; i++){		
		    if (objChilddiv[i].style.backgroundColor == '#479ff3') {
				break;
			}
		}
		
		//在可选可输入文本框上按回车时，如果下拉列表中没有选中项，则直接跳到下一输入元素
		if(i == objChilddiv.length){
			cursorInSelectDivObj=false;
			textObjectBlur(selectDivObj,noteDivObj);
			event.keyCode=9;
		}else{
			event.returnValue=false;
		}
		return;
	}
	
		
	var srcForm = event.srcElement.form;

	if(srcForm == undefined || srcForm == null){
			return ;
	}
	
	
	var srcForm = event.srcElement.form;
	var srcElementNext = null;

	var allElems = srcForm.elements;
	for(var i = 0; i < allElems.length; i++){
		if(event.srcElement == allElems[i]){
			if(!isLastElem(allElems,i+1)){					
					event.keyCode=9;
					break;
			}else {				
				if(event.srcElement.type=='select-one'){
					var subButton = findSubmitButton(allElems,i);
					if(subButton !=null){
						subButton.click();
					}				
				}				
			}
		}
	}

}

//查找提交按钮
function findSubmitButton(allElems,index){
	for(var i = index; i < allElems.length; i++){
			if(allElems[i].type=='submit'){
				return  allElems[i];
			}
	}
	
	return null;
}

//判断是否是最后一个元素
function isLastElem(allElems,index){
	
	if(index >=allElems.length || allElems[index].type=="submit" ){		
		return true;
	}
	
	for(var i = index; i < allElems.length; i++){
		var tempObj = allElems[i];
		while(tempObj != window.document.body){
			//如果该元素未隐藏，则判断父元素是否隐藏
			if(tempObj.style.display != 'none'){
				tempObj=tempObj.parentElement;
			}else{
				//如果输入元素隐藏，则递归查找其他输入元素是否隐藏
				return isLastElem(allElems,i+1);	
			}			
		}
		
		//如果某输入元素本身未隐藏，且其父也未隐藏，则不是最后一输入元素
		if(tempObj == window.document.body){
			return false;
		}
	}		
}


//自动绑定按键事件
window.document.onkeydown = keydownOnSelectInput;
window.onresize=function(){
		if(textObject){
			textObject.blur();
		}
}
