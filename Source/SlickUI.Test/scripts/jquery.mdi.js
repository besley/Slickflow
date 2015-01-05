/*

 */
jQuery.extend({	
	MdiDefaults: { 
	    mdiTab: 'mdiTab',	    
	    mdiBody: 'mdiBody',
	    mdiIds: [],
	    mdiPages: [],
	    nextId: 1,
		mdiTabWidth: 0,
		activedTabId: ''
	    },
	    
	InitMdi: function(o){ 
		jQuery.MdiDefaults = jQuery.extend({ },jQuery.MdiDefaults,o);	
		jQuery('#' + o.mdiTab).addClass('mdiTabs');
		jQuery('#' + o.mdiBody).addClass('mdiBodys');
	},
	
	getWidthDiff : function(tabs){
			if(tabs.height()>30){
				return -1;
			}
			var maxWidth = tabs.width()-32;
			var sumWidth = 0;
			tabs.children("span").each(function(index){
					if( this.style.display!="none" ){
						sumWidth+=this.clientWidth;
					}
				});

			return maxWidth - sumWidth;
		},
	  
	 getShowCount : function(tabs){
			var sumWidth = 0;
			tabs.children("span").each(function(index){
					if( this.style.display!="none" ){
						sumWidth+=1;
					}
				});

			return sumWidth;
		},
	

		showTabss : function( tabs ){
			jQuery.showLeftTabs( tabs);
			jQuery.showRightTabs( tabs );
		},


		showRightTabs : function( tabs ){
			var thehide = "";
			var lastShow = "";
			tabs.children("span")
				.each(function(i){
						if( this.style.display!="none" ){
							lastShow = this; //.return false;
						}
						
						if( lastShow!="" && this.style.display=="none" ){
							thehide = this;
							return false;
						}
				});
			if(thehide!=""){
				thehide.style.display="";
				if( getWidthDiff()<0 ){
					thehide.style.display="none";
				}else{
					jQuery.showRightTabs(tabs);
				}
			}
			jQuery.setLeftRightNarrow( tabs );
		},

		showLeftTabs : function( tabs ){
			var thehide = "";
			tabs.children("span")
				.each(function(i){
						if( this.style.display!="none" ){
							return false;
						}else{
							thehide = this;
						}
				});
			if(thehide!=""){
				thehide.style.display="";
				if( getWidthDiff()<0 ){
					leftLastHide.style.display="none";
				}else{
					jQuery.showLeftTabs(tabs);
				}
			}
			jQuery.setLeftRightNarrow( tabs );
		},

		showOneRightTab : function( tabs ){
			var findShow = false;
			tabs.children("span")
				.each(function(i){
						if( this.style.display!="none" ){
							findShow = true;
						}else if(findShow==true){
							this.style.display="";
							return false;
						}
				});
			jQuery.hideLeftTabs(tabs );
		},

		showOneLeftTab : function(tabs){
			var leftLastHide = "";
			tabs.children("span")
				.each(function(i){
						if( this.style.display=="none" ){
							leftLastHide = this;
						}else{
							return false;
						}
				});
			if(leftLastHide==""){
				return false;
			}

			leftLastHide.style.display="";

			jQuery.hideRightTabs(tabs);
		},

		hideRightTabs : function( tabs ){
			while( jQuery.getShowCount(tabs)>1 && jQuery.getWidthDiff(tabs)<0 ){
				var lastShow = "";
				tabs.children("span")
					.each(function(){
						if( this.style.display!="none" ){
							lastShow = this;
						}
				});
				if(lastShow!=""){
					lastShow.style.display="none";
				}
			}
			jQuery.setLeftRightNarrow(tabs);
		},

		hideLeftTabs : function(tabs){
			while( jQuery.getShowCount(tabs)>1 && jQuery.getWidthDiff(tabs)<0 ){
				tabs.children("span")
					.each(function(){
						if( this.style.display!="none" ){
							this.style.display="none";
							return false;
						}
				});
			}
			jQuery.setLeftRightNarrow(tabs);
		},

		findAndShowTab : function(tabs,tabId){
			var tab="";
			var onShowing = false;
			tabs.children("span")
					.each(function(){
						if( this.id==tabId){
							tab=this;
							if( this.style.display!="none" ){
								onShowing = true;
							}
							return false;
						}
				});
			if(onShowing || tab==""){
				return true;
			}

			var inLeft = false;

			tabs.children("span")
					.each(function(){
						if( this.style.display!="none" ){
							return false;
						}
						if( this.id==tab.id){
							inLeft = true;
							return false;
						}
				});

			while( tab.style.display=="none" ){
				if( inLeft ){
					jQuery.showOneLeftTab(tabs);
				}else{
					jQuery.showOneRightTab(tabs);
				}
			}
		},

	setLeftRightNarrow : function(tabs){
			var objXX = tabs.children("span");

			var xObj = jQuery('#leftButton');
			var yObj = jQuery('#rightButton');
			
			//xObj.click(function(){ showOneLeftTab();});
			//yObj.click(function(){ showOneRightTab();});
			
			if(objXX.length==0 || objXX[0].style.display!="none" ){
			    xObj.css("background", "url(/Common/Content/default/arrowleft.gif)"); //hide();
			}else{
			    xObj.css("background", "url(/Common/Content/default/arrowleft_light.gif)"); //show();
			}

			if(objXX.length==0 || objXX[objXX.length-1].style.display!="none" ){
			    yObj.css("background", "url(/Common/Content/default/arrowright.gif)"); //hide();
			}else{
			    yObj.css("background", "url(/Common/Content/default/arrowright_light.gif)"); //??show();
			}
		},

	ShowChildWidow: function(url, title,id){
		var o = jQuery.MdiDefaults;		
		o.nextId++;
		o.mdiIds.push(id);
		o.mdiPages.push("body"+id);
		var regS = new RegExp("[ ]","gi");    //replace ' '
		var idfromtitle = title.replace(regS, "");
		idfromtitle = id;
		var tabs = jQuery('#' + o.mdiTab);   //the tab container
		var bodys = jQuery('#' + o.mdiBody); //the body container
		var tabId = 'tab' + idfromtitle;           //current tab id
		var bodyId = 'body' + idfromtitle;         //current body id

		//active specified tab		
		var active = function(t, b, index){
			if(t.css("display")=="none"){
				return false;
			}

		    jQuery('span.MdiTabActived', tabs).removeClass("MdiTabActived");//.find('div.mdiDivClose').hide();
		    t.addClass("MdiTabActived");//.find('div.mdiDivClose').show(); //show the close button image
		    
		    jQuery('#mdiBody').children("div").hide(); //hide the visible frames,and show the current frame
		    b.show();

			//activedTabId = t.id;
		}
	    
		//close specified tab
		var close = function (t, b) {
		    var _ii = $("#mdiTab").children("span").index($(t));
		    if (_ii <= 0) _ii = 0; else _ii--;
		    gc(b);//±ÜÃâiframeÄÚ´æÐ¹Â©
		    t.remove();
		    b.remove();
		    //alert(o.mdiPages.length);
		    //remove from the array list
		    for(i=0; i<o.mdiPages.length; i++)
		    {  
		        if (t[0].id.substring(3, t[0].id.length) == o.mdiIds[i])
		        { o.mdiPages.splice(i, 1); o.mdiIds.splice(i, 1); }
		    }
		    //active the first tab page after closed    
		    //active(jQuery('#' + o.mdiIds[0], tabs), jQuery('#' + o.mdiIds[0], bodys), o.mdiIds[0]);
		    active(jQuery('#tab' + o.mdiIds[_ii]), jQuery('#body' + o.mdiIds[_ii]), o.mdiIds[_ii]);
			jQuery.showTabss(tabs); //fixWindow(t,b,index );
		}

		var gc = function clearIframe() {
		    //var frame = id;
		    //frame[0].src = 'about:blank';
		    //frame[0].contentWindow.document.write('');//Çå¿ÕiframeµÄÄÚÈÝ
		    //frame[0].contentWindow.close();//±ÜÃâiframeÄÚ´æÐ¹Â©
		    //frame.remove();//É¾³ýiframe
		    //if ($.browser.msie) {
		    //    CollectGarbage();
		    //    setTimeout("CollectGarbage();", 1);
		    //}
		    if (Sys.ie) {
		        CollectGarbage();
		        setTimeout("CollectGarbage()", 1);
		    }
		    //Ïú»Ù±äÁ¿
		    if (GLOBAL[id]) {
		        try{
		            GLOBAL[id].GC();
		            GLOBAL[id] = null;
		            delete GLOBAL[id];
		        }
		        catch (e) {
		            window.console.log(e.message);
		        }
		    }
		}

				
		//if exists then return		
		if(tabs.find("#" + tabId).size()>0){
			jQuery.findAndShowTab(tabs,tabId);

			active(tabs.find("#" + tabId), bodys.find("#" + bodyId)); 
			return;
		}
		
	    tabs.append('<span id="' + tabId + '" class="MdiTabNormal" ><div class="mdiDivLoading"></div><span style="float:left;z-index:6;"> '+title+'</span><div class="mdiDivClose"></div></span>');
	    var tab = jQuery('#' + tabId, tabs);
	    tab.addClass("MdiTabActived");  //add a class to the current tab
	   
	    $("#mdiBody").append("<div id='" + bodyId + "'></div>");
	    $("#" + bodyId).css({ position: "absolute", margin: "0px", right: "0px", bottom: "0px", left: "0", top: "0", "z-index": "0", display: "block", visibility: " visible" });
	    //height: $("#mdiBody").css("height"), width: $("#mdiBody").css("width"),
	    
	    $("#" + bodyId).load(url , function () {
	        if (GLOBAL[id])
	            //$("#" + bodyId).layout({ applyDefaultStyles: true, maskContents: true, onresize_end: GLOBAL[id].resizeGrid });
                
	        tab.find('div.mdiDivLoading').hide();

	        //var dd = new Date()
	        //GLOBAL.test.dd2 = dd.getTime();
	        //alert(GLOBAL.test.dd2 - GLOBAL.test.dd);

	        //o.framebody.push({ url: url, html: $("#" + bodyId) });
	        //if (o.framebody.length > 8) { o.framebody.splice(0, 1); }
	    });

	    //bodys.append('<iframe id="'+ bodyId +'" name="'+bodyId+'" width="100%" height="100%" border="1" frameborder="0" marginwidth="1" marginheight="1" src="'+ url +'"></iframe>');
        var body = jQuery('#' + bodyId, bodys);
        
        //event
	    tab.click(function () {
	        var _index = $("#mdiTab").children("span").index(tab);
	        active(tab, body, tab.attr('id').substring(tab.attr('id').indexOf('b') + 1));
	        if (GLOBAL[id].resizeGrid) {
	            try{
	                GLOBAL[id].resizeGrid();}
	            catch (e) { window.console.log(e.message);}
	        }
	    })
            .dblclick(function(){ close(tab, body);} )
            .mouseover(function(){ jQuery(this).addClass("MdiTabOver")})
            .mouseout(function(){ jQuery(this).removeClass("MdiTabOver")})
            .find("div.mdiDivClose").click(function(){ close(tab, body);})
                    .mouseover(function(){ jQuery(this).css('background-position','bottom')})
                    .mouseout(function(){ jQuery(this).css('background-position','top')});	    
	    
	    
        body.load(function(){ tab.find('div.mdiDivLoading').hide(); });
	    
        active(tab, body, tab.attr('id').substring(tab.attr('id').indexOf('b')+1));

		//fixWindow(t,b,index );
		if(jQuery.getWidthDiff(tabs)<0){
			jQuery.hideLeftTabs(tabs);
		}
	}
});

