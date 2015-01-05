(function($) {
	$.fn.swapClass = function(c1, c2) {
		return this.removeClass(c1).addClass(c2);
	}
	$.fn.switchClass = function(c1, c2) {
		if (this.hasClass(c1)) {
			return this.swapClass(c1, c2);
		}
		else {
			return this.swapClass(c2, c1);
		}
	}
	$.fn.dtoolbar = function(settings) {
		var dfop ={
			items:null
		}
		$.extend(dfop, settings);
		var me = $(this);
		me.addClass("gTlBar");
		if (dfop.items) {
			for (i = 0; i < dfop.items.length; i++) {
				me.append(addtoolbar(dfop.items[i],i));
			}
		}
		function addtoolbar(item,numi) {
			var t = {
				checkbox:false,
				text:null,
				ico :null,
				handler:false,
				disabled:false
			}
			$.extend(t, item);
			var toolbarEntity = this;
			if (t.text == '-') {
			//还没想到
			//TODO
			} else {
				var bttbar = $("<div></div>");
				if(t.id==null)
					bttbar.attr("id","bt_"+numi);
				else
					bttbar.attr("id","bt_"+t.id);
				bttbar.addClass('tlbtn2');
				bttbar.attr({
					checkbox:t.checkbox,
					checked:'false',
					numberid:numi
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
				    if (t.ico != "dropdownlist") bttbar.bind('click', t.handler);
				    //else $("#dropdownlist1").change(t.handler);
				}
				if (t.disabled) {
					bttbar.attr("disabled", "disabled");
					bttbar.addClass('btndisabled');
					bttbar.unbind('click');
				}
				bttbar.bind('mouseover', function(){
					var isdisabled = (bttbar.attr("disabled")=='disabled')?true:false;
					if (isdisabled) {
					
					}
					else {
						if ($(this).attr("checked") == 'false' || $(this).attr("checked") == false) {
							$(this).addClass('on');
						}
						$(this).bind('mouseout', function(){
							$(this).removeClass('click');
							if ($(this).attr("checked") == 'false' || $(this).attr("checked") == false) {
								$(this).removeClass('on');
							}
							$(this).unbind('mouseout');
						});
					}
				});
				bttbar.bind('mousedown', function(){
					var isdisabled = (bttbar.attr("disabled")=='disabled')?true:false;
					if (isdisabled) {
					}
					else {
						$(this).removeClass('on');
						$(this).addClass('click');
						$(this).bind('mouseup', function(){
							$(this).removeClass('click');
							$(this).addClass('on');
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
							}
						}
					}
				});
				return bttbar;
			}
		};
		me[0].t ={
			setEnable:function (id,enable){
				var bttbar = $("#bt_"+id);
				if(bttbar.length<=0) return;
				var numid = bttbar.attr("numberid");
				var t=dfop.items[numid];
				if(!enable){
					bttbar.attr("disabled", "disabled");
					bttbar.addClass('btndisabled');
					bttbar.unbind('click');
				}else{
					bttbar.removeAttr("disabled");
					bttbar.removeClass('btndisabled');
					if (t.handler) {
					    if (bttbar.data("events")&&!bttbar.data("events")["click"])
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
		                    if (bttbar.data("events")&&!bttbar.data("events")["click"])
		                        bttbar.bind('click', t.handler);		                                
		                }
		            }
		        }
		    }
		};
		return me;
	}
	//将某一个按钮设为可用和不可用
	$.fn.setbtEnable=function(id,enable){
		if (this[0].t) {
			return this[0].t.setEnable(id,enable);
		}
		return null;
	}
    //将按钮设为可用和不可用
	$.fn.setAllEnable = function (enable) {
	    if (this[0]!=null&&this[0].All) {
	        return this[0].All.setEnable(this,enable);
	    }
	    return null;
	}
})(jQuery);