/*******************************弹出框dialog***********/
(function ($) {
    var z_index = 1000, width, height;
    var cache = [];
    $.fn.dialog2 = function (args) {
        z_index++;
        var settings = {
            width: 200, height: 200, title: "对话框", buttons: [], modal: true, openUrl: null, newzIndex: z_index, okbutton: true, cancelbutton: true, id: z_index, N: $(this).selector,
            okButtonClick: null, cancelButtonClick: null,
        };

        $.extend(settings, args);

        init(settings);
    };

    $.fn.confirm = function (args) {
        z_index++;
        var settings = {
            width: 280, height: 150, title: "确认对话框", buttons: [], modal: true, openUrl: null, newzIndex: z_index, okbutton: true, cancelbutton: true, id: z_index,
            okButtonClick: null, cancelButtonClick: null,
        };

        $.extend(settings, args);
        settings.html = '<p><span style="float: left; margin: 0 7px 50px 0;" class="ui-icon ui-icon-circle-check"></span><b>' + args.html + '</b></p>';
        init(settings);
    };

    $.fn.alert = function (args) {
        z_index++;
        var settings = {
            width: 280, height: 150, title: "提示", buttons: [], modal: true, openUrl: null, newzIndex: z_index, okbutton: true, id: z_index,
            okButtonClick: null, cancelButtonClick: null,
        };

        $.extend(settings, args);
        settings.html = '<p><span style="float: left; margin: 0 7px 50px 0;" class="ui-icon ui-icon-circle-check"></span><b>' + args.html + '</b></p>';
        init(settings);
    };

    var init = function (settings) {
        var $dialog, $head, $body, $bottompanel, $bottomset, $button, $mengban;
        var top, left;
        var _f_cache = false;
        width = $(window).width();
        height = $(window).height();
        settings.height = settings.height + 30;
        if (settings.width < width) left = (width - settings.width) / 2; else left = 0;
        if (settings.height < height) top = (height - settings.height) / 2; else top = 0;
        var id = settings.newzIndex;

        $mengban = $('<div class="ui-widget-overlay" style="width: ' + width + 'px; height: ' + height + 'px; z-index: ' + (z_index - 1) + ';"></div>');
        $dialog = $('<div id="ui-dialog' + id + '" class="ui-dialog ui-widget-content ui-draggable ui-corner-all" style="outline: 0px none; z-index: ' + settings.newzIndex + '; position: absolute; height: ' + settings.height + 'px; width: ' + settings.width + 'px; top: ' + top + 'px; left: ' + left + 'px; display: block;" tabindex="-1"></div>')
        .appendTo($("body"));
        $head = $('<div id="ui-dialog-header' + id + '" class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix"><span class="ui-dialog-title">' + settings.title + '</span><a href="#" class="ui-dialog-titlebar-close ui-corner-all"><span class="ui-icon ui-icon-closethick">close</span></a></div>')
        .appendTo($dialog);

        if (settings.openUrl) {
            for (var i = 0; i < cache.length; i++) {
                if (cache[i].url == settings.openUrl) {
                    cache[i].body.appendTo($dialog); _f_cache = true; break;
                }
            }
            if (!_f_cache)
            $body = $('<div id="dialogframe" class="ui-dialog-content ui-widget-content" style="width: auto; min-height: 0px; height: ' + (settings.height - 70) + 'px;" scrolltop="0" scrollleft="0"><div>正在载入中……</div></div>').appendTo($dialog)
                .load(settings.openUrl, function () {
                    if (cache.length < 12)
                        cache.push({ url: settings.openUrl, body: $body });
                    else {
                        cache.splice(0, 1);
                        if (GLOBAL[settings.N] && GLOBAL[settings.N].GC) GLOBAL[settings.N].GC();
                    }
                });           
        }
        else {
            $body = $('<div id="dialogframe" class="ui-dialog-content ui-widget-content" style="width: auto; min-height: 0px; height: ' + (settings.height - 70) + 'px;" scrolltop="0" scrollleft="0"></div>').append(settings.html).appendTo($dialog);          
        }
        initButtonPanel(id);

        if ($(".ui-widget-overlay").length == 0)
            $mengban.appendTo($("body"));

        function initButtonPanel(id) {
            $bottompanel = $('<div class="ui-dialog-buttonpane ui-widget-content ui-helper-clearfix"></div>').appendTo($dialog);
            $bottomset = $('<div class="ui-dialog-buttonset"></div>').appendTo($bottompanel);
            if (settings.buttons) {         
                for (var i = 0; i < settings.buttons.length; i++) {
                    $('<button type="button" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false"><span class="ui-button-text">' + settings.buttons[i].name + '</span></button>').appendTo($bottomset)
                    .bind("click", function () {
                        var _index = $bottomset.children("button").index(this);
                        if (settings.buttons[_index].onClick) {
                            if (settings.buttons[_index].onClick() != false) close(id);
                        }
                        else
                            close(settings);
                    });
                }
            }
            if (settings.okbutton) {
                $('<button type="button" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false"><span class="ui-button-text">确定</span></button>').appendTo($bottomset)
                .bind("click", function () {
                    if (settings.okButtonClick) {
                        if (settings.okButtonClick() != false) close(id);
                    }
                    else
                        close(settings);
                });
            }
            if (settings.cancelbutton)
                $('<button type="button" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false"><span class="ui-button-text">取消</span></button>').appendTo($bottomset)
              .bind("click", function () {
                  if (settings.cancelButtonClick) {
                      if (settings.cancelButtonClick() != false) close(id);
                  }
                  else
                      close(id);
              });

        }
        bindEvent(settings);
  
    }

    function bindEvent(settings) {
        var x0, x1, y0, y1, _top, _left, stop = false;
        $("#ui-dialog-header" + settings.id + " .ui-icon-closethick").click(function () {
            close(settings);
        });

        $("#ui-dialog-header" + settings.id).mousedown(function (e) {
            x0 = e.clientX, y0 = e.clientY;
            _top = $("#ui-dialog" + settings.id)[0].offsetTop, _left = $("#ui-dialog" + settings.id)[0].offsetLeft;

            this.setCapture ? (this.setCapture(), this.onmousemove = function (ev) { mouseMove(ev || event) }, this.onmouseup = mouseUp)
            : ($(document).bind("mousemove", mouseMove).bind("mouseup", mouseUp));

            function mouseMove(e) {
                x1 = e.clientX, y1 = e.clientY;
                if (x0 && y0) {
                    $("#ui-dialog" + settings.id).css({ top: _top + y1 - y0, left: _left + x1 - x0 });
                    if (_top + y1 - y0 <= 0) $("#ui-dialog" + settings.id).css({ top: 0 });
                    else if (_top + y1 - y0 + settings.height  >= height) $("#ui-dialog" + settings.id).css({ top: height - settings.height + 30 });
                    if (_left + x1 - x0 <= 0) $("#ui-dialog" + settings.id).css({ left: 0 });
                    else if (_left + x1 - x0 + settings.width  >= width) $("#ui-dialog" + settings.id).css({ left: width - settings.width });
                    //if (_top + y1 - y0 >= 0 && _top + y1 - y0 + settings.height <= height) $("#ui-dialog" + settings.id).css({ top: _top + y1 - y0 });
                    //if (_left + x1 - x0 >= 0 && _left + x1 - x0 + settings.width <= width) $("#ui-dialog" + settings.id).css({ left: _left + x1 - x0 });
                }
            }
            function mouseUp() {
                this.releaseCapture ? (this.releaseCapture(), this.onmousemove = this.onmouseup = null) :
                ($(document).unbind("mousemove", mouseMove).unbind("mouseup", mouseUp));
            }
        });

        if ($(".ui-dialog").length <= 1) {
            $(window).resize(function () {
                $mengban.css({ width: $(window).width(), height: $(window).height() });              
            });
        }
    }

    function delayClose(settings) {
        GLOBAL[settings.N].Init();
        $("#ui-dialog" + settings.id).remove();
    }

    function close(settings) {
        if ($(".ui-dialog").length <= 1) $(".ui-widget-overlay").remove();  
        if (GLOBAL[settings.N] && GLOBAL[settings.N].Init) GLOBAL[settings.N].Init();
        $("#ui-dialog" + settings.id).remove();
        $(window).unbind("resize");
    }


    function closeAll(id) {
        $(".ui-widget-overlay").remove();
        $(".ui-dialog").remove();
        $(window).unbind("resize");
    }

})(jQuery);

