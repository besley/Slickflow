/**Jgrid.js
*
*/
(function ($) {
    // Slick.Grid
    $.extend(true, window, { Jgrid: { Grid: SlickGrid }});

    function SlickGrid(container, data, columns, options) {
        // settings
        var defaults = {          
            rowHeight: 25,
            defaultColumnWidth: 80,
            enableAddRow: false,       
            editable: false,
            autoEdit: true,
            selectionModel: "Row",
            editorLock: Jgrid.GlobalEditorLock,
            //multiSelection:true,
            //enableCellNavigation: true,
            //enableColumnReorder: true,
            //asyncEditorLoading: false,
            //asyncEditorLoadDelay: 100,
            //forceFitColumns: false,
            //enableAsyncPostRender: false,
            //asyncPostRenderDelay: 50,
            //autoHeight: false,              
            //headerRowHeight: 25,
            //showTopPanel: false,
            //topPanelHeight: 25,
            //formatterFactory: null,
            //editorFactory: null,    
            //multiSelect: true,
            //enableTextSelectionOnCells: false,            
            //fullWidthRows: false,
            multiColumnSort: false   
        };

        var columnDefaults = {
            name: "",
            resizable: true,
            sortable: false,
            width: 80,
            rerenderOnResize: false,
            headerCssClass: null,
            defaultSortAsc: true
        };

        // private
        var initialized = false;    
        var data = data;
        var $container, _containerHeight, _width;
        var uid = Math.round(1000000 * Math.random());
        var _grid = "jgrid" + uid;
        var _canves = "canves" + uid;
        var self = this;    
        var $headerScroller;
        var $headers, $headersL, $headersR;
        var $style;
        var _canvasWidth, _canvasLWidth, $headerRowScroller, $headerRowSpacer;
        var $canvas, $canvasL, $canvasR, $cell;
        var $row, $rowL, $rowR;
        var headCache = [], headersRCache = [], canvasLCache = [];
        var x, y,preX,preY;//行，列
        var currentEditor = null;
        var editController;      
        var activeRow, activeCell,active;
        var _fixpad = 4, _fixbod = 1, _fixcanvas = 4, _rowHeight = 25, offset=1;

        // Initialization
        function init() {
            $.extend(defaults, options);
            columnsById = {};
            for (var i = 0; i < columns.length; i++) {
                var m = columns[i] = $.extend({}, columnDefaults, columns[i]);
                columnsById[m.id] = i;
            }

            editController = {
                isEdited: null,
                cell:null,
                cancelEdit: cancelEdit,
                commitCurrentEdit: commitCurrentEdit
            };

            $container = $(container);
            $(container).empty();
            $container.removeClass(_grid);
            //$(container).removeAttr("style");
            if ($container.length < 1) {
                throw new Error("SlickGrid requires a valid container, " + container + " does not exist in the DOM.");
            }
            $container.addClass(_grid);

            getCanvasSize();
            //_containerHeight = $container.height() == null ? 300 : $container.height();
            //_width = $container.width();// >= _canvasWidth ? $container.width() : _canvasWidth;
            $container.css({ height: _containerHeight + 'px', overflow: 'hidden', outline: '0px none', position: 'relative' });//width: _width + 'px',

            if (defaults.frozenColumn) {
                $headersL = $("<div class='ui-state-default slick-header slick-header-left' style='width:" + _canvasLWidth + "px;float:left;position:absolute;z-index:100'></div>").appendTo($container);
                $headersR = $("<div class='ui-state-default slick-header slick-header-right' style='width:" + (_canvasWidth - _canvasLWidth) + "px;left:" + _canvasLWidth + "px;overflow:hidden;'></div>").appendTo($container);
                var cc=0;
                for (var i = 0; i < columns.length; i++) {
                    if (columns[i].visible != null && !columns[i].visible) continue;
                    if (cc < defaults.frozenColumn) {                   
                        $("<div id='" + _grid + columns[i].id + "' class='ui-state-default slick-header-column2 ' style='width:" + columns[i].width + "px; height: 16px;' title=''>"
                            + "<span class='slick-column-name '>" + columns[i].name + "</span><div class='slick-resizable-handle2'></div>"
                           + "</div>").appendTo($headersL);
                        cc++;
                    }
                    else {
                        headersRCache.push($("<div id='" + _grid + columns[i].id + "' class='ui-state-default slick-header-column2 ' style='width:" + columns[i].width + "px; height: 16px;' title=''>"
                          + "<span class='slick-column-name '>" + columns[i].name + "</span><div class='slick-resizable-handle2'></div>"
                         + "</div>"));
                        headersRCache[headersRCache.length-1].appendTo($headersR);
                    }
                }
                if (data.length > 0)
                    createCssRules();

                var el = $container, _h = 0, _l = 0;
                while (el[0]) {
                    _h=el[0].offsetWidth;
                    if (_h != null && _h > 0)
                        break;
                    el = el.offsetParent();
                }
                 
                $canvasL = $("<div tabindex='0' id='" + _canves + "L' class='slick-pane slick-pane-top slick-pane-left' style='height: " + (_containerHeight - 30) + "px;overflow-x:scroll;overflow-y:hidden;width:" + _canvasLWidth + "px;'><div style='width:" + _canvasLWidth + "px;height:100%;'></div></div>").appendTo($container);
                $canvasR = $("<div tabindex='1' id='" + _canves + "R' class='slick-pane slick-pane-top slick-pane-right' style='height: " + (_containerHeight - 30) + "px;overflow:auto;left:" + _canvasLWidth + "px;width:" + (_h - _canvasLWidth) + "px;'><div style='width:" + (_canvasWidth - _canvasLWidth) + "px;height:100%;left:" + _canvasLWidth + "px;'></div></div>").appendTo($container);
                if (data.length == 0) $canvasL.css("overflow-x", "hidden");
            }
            else {
                $headers = $("<div class='ui-state-default slick-header slick-header-left' style='width:" + _canvasWidth + "px;'></div>").appendTo($container);               
                for (var i = 0; i < columns.length; i++) {
                    if (columns[i].visible != null && !columns[i].visible) continue;                   
                    headCache.push($("<div id='" + _grid + columns[i].id + "' class='ui-state-default slick-header-column2 ' style='width:" + columns[i].width + "px; height: 16px;' title=''>"
                        + "<span class='slick-column-name '>" + columns[i].name + "</span><div class='slick-resizable-handle2'></div>"
                       + "</div>"))
                    headCache[headCache.length - 1].appendTo($headers);
                }

                if (data.length > 0)
                    createCssRules();

                $canvas = $("<div tabindex='0' id='" + _canves + "' class='slick-pane slick-pane-top slick-pane-left' style='height: " + (_containerHeight - 30) + "px;overflow:auto;'><div style='width:" + _canvasWidth + "px;height:100%;'></div></div>").appendTo($container);            
            }
           
            for (var i = 0; i < data.length; i++) {
                drawRow(data[i], i);
            }

            bindEvent();
        }

        function drawRow(row, _index) {
            var txt = "", m, cc = 0;
            if (defaults.frozenColumn) {
                if (_index % 2 == 0) {
                    $rowL = $("<div class='ui-widget-content slick-row2 even " + "rowL" + uid + _index + "'></div>").appendTo($canvasL);
                    $rowR = $("<div class='ui-widget-content slick-row2 even " + "rowR" + uid + _index + "'></div>").appendTo($canvasR);
                }
                else {
                    $rowL = $("<div class='ui-widget-content slick-row2 odd2 " + "rowL" + uid + _index + "'></div>").appendTo($canvasL);
                    $rowR = $("<div class='ui-widget-content slick-row2 odd2 " + "rowR" + uid + _index + "'></div>").appendTo($canvasR);
                }            
                for (var i = 0; i < columns.length; i++) {
                    if (columns[i].visible != null && !columns[i].visible) continue;
                    m = columns[i];
                    if (m.fieldType) txt = _index + 1;
                    else {
                        if (row[m.id] != null) txt = row[m.id]; else txt = "";
                    }
                    if (cc < defaults.frozenColumn)             
                        $("<div cc='" + _index + "r" + cc + "' class='slick-cell2 " + "cell" + uid + i + "'>" + txt + "</div>").appendTo($rowL);                
                    else
                        $("<div cc='" + _index + "r" + cc + "' class='slick-cell2 " + "cell" + uid + i + "'>" + txt + "</div>").appendTo($rowR);
                    cc++;
                }
                canvasLCache.push($rowL);
            }
            else {
                if (_index % 2 == 0)
                    $row = $("<div class='ui-widget-content slick-row2 even " + "row" + uid + _index + "'></div>").appendTo($canvas);
                else
                    $row = $("<div class='ui-widget-content slick-row2 odd2 " + "row" + uid + _index + "'></div>").appendTo($canvas);
                for (var i = 0; i < columns.length; i++) {
                    if (columns[i].visible != null && !columns[i].visible) continue;
                    m = columns[i];
                    if (m.fieldType) txt = _index + 1;
                    else {
                        if (row[m.id] != null) txt = row[m.id]; else txt = "";
                    }
                    $("<div cc='" + _index + "r" + cc + "' class='slick-cell2 " + "cell" + uid + i + "'>" + txt + "</div>").appendTo($row);
                }
            }
        }
        function updateRow(_index,row) {
            var cc = 0;
            if (row == null) row = data[_index];
            if (defaults.frozenColumn) {
                var $currowL = $canvasL.children("div").eq(_index).empty();
                var $currowR = $canvasR.children("div").eq(_index).empty();
                for (var i = 0; i < columns.length; i++) {
                    if (columns[i].visible != null && !columns[i].visible) continue;
                    m = columns[i];
                    if (m.fieldType) txt = _index + 1;
                    else {
                        if (row[m.id] != null) txt = row[m.id]; else txt = "";
                    }
                    if (cc < defaults.frozenColumn)
                        $("<div cc='" + _index + "r" + cc + "' class='slick-cell2 " + "cell" + uid + i + "'>" + txt + "</div>").appendTo($currowL);
                    else
                        $("<div cc='" + _index + "r" + cc + "' class='slick-cell2 " + "cell" + uid + i + "'>" + txt + "</div>").appendTo($currowR);
                    cc++;
                }
                applyCss(editController.cell, "L");
                applyCss(editController.cell, "R");
            }
            else {
                var $currow = $canvas.children("div").eq(_index).empty();
                for (var i = 0; i < columns.length; i++) {
                    if (columns[i].visible != null && !columns[i].visible) continue;
                    m = columns[i];
                    if (m.fieldType) txt = _index + 1;
                    else {
                        if (row[m.id] != null) txt = row[m.id]; else txt = "";
                    }
                    $("<div cc='" + _index + "r" + cc + "' class='slick-cell2 " + "cell" + uid + i + "'>" + txt + "</div>").appendTo($currow);
                }
                applyCss(editController.cell);
            }
            setCss();
        }

        function createCssRules(rowTop, rowWidth, cellWidth, cellHeight) {
            var rules = [], cc = 0;
            if ($style == null)
                $style = $("<style type='text/css' rel='stylesheet' />").appendTo($("head"));
            $style.empty();
            if (defaults.frozenColumn) {
                for (var i = 0; i < data.length; i++) {
                    rules.push(".rowL" + uid + i + " {top:" + i * _rowHeight + "px;width:" + _canvasLWidth + "px;}");
                    rules.push(".rowR" + uid + i + " {top:" + i * _rowHeight + "px;width:" + (_canvasWidth - _canvasLWidth) + "px;}");
                }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    rules.push(".row" + uid + i + " {top:" + i * _rowHeight + "px;width:" + _canvasWidth + "px;}");
                }
            }

            for (var i = 0; i < columns.length; i++)
                rules.push(".cell" + uid + i + " { width:" + (columns[i].width + _fixpad + _fixbod) + "px;height:" + 20 + "px;}");

            if ($style[0].styleSheet) { // IE
                $style[0].styleSheet.cssText = rules.join(" ");
            } else {
                $style[0].appendChild(document.createTextNode(rules.join(" ")));
            }
        }

        function applyCss(obj,args) {   
            if (args=="L") { 
                $canvasL.children("div").find(".selected").removeClass('selected');
                $canvasL.find(".active").removeClass('active');
                $canvasR.children("div").find(".selected").removeClass('selected');
                $canvasR.find(".active").removeClass('active');            

                $canvasR.children("div").eq(x).children("div").addClass('selected');
                $canvasL.children("div").eq(x).children("div").addClass('selected');
                $canvasL.children("div").eq(x).children("div").eq(y - defaults.frozenColumn).addClass('active');
            }
            else if (args == "R") {
                $canvasR.children("div").find(".selected").removeClass('selected');
                $canvasR.find(".active").removeClass('active');
                $canvasL.children("div").find(".selected").removeClass('selected');
                $canvasL.find(".active").removeClass('active');

                $canvasL.children("div").eq(x).children("div").addClass('selected');
                $canvasR.children("div").eq(x).children("div").addClass('selected');
                $canvasR.children("div").eq(x).children("div").eq(y - defaults.frozenColumn).addClass('active');      
            }
            else {
                $canvas.children("div").find(".selected").removeClass('selected');
                $canvas.children("div").find(".active").removeClass('active');
       
                $canvas.children("div").eq(x).children("div").addClass('selected');
                $canvas.children("div").eq(x).children("div").eq(y).addClass('active');
            }
        }
        function setCss() {
            if (defaults.frozenColumn) {
                $canvasL.children("div").eq(x).children("div").addClass('selected');
                $canvasR.children("div").eq(x).children("div").addClass('selected');
                if (y < defaults.frozenColumn)
                    $canvasL.children("div").eq(x).children("div").eq(y).addClass('active');
                else
                    $canvasR.children("div").eq(x).children("div").eq(y - defaults.frozenColumn).addClass('active');
            }
            else {

            }
            $container.find(".slick-pane").children("div").eq(x).addClass('selected');
        }

        function render() {
            if (defaults.frozenColumn) {
                $canvasL.empty();
                $canvasR.empty();
                canvasLCache = [];
                if (data.length == 0) {
                    $("<div style='width:" + _canvasLWidth + "px;height:100%;'></div>").appendTo($canvasL);
                    $("<div style='width:" + (_canvasWidth - _canvasLWidth) + "px;height:100%;left:" + _canvasLWidth + "px;'></div>").appendTo($canvasR);
                }
                else
                    $canvasL.css("overflow-x", "scroll");
                createCssRules();

                for (var i = 0; i < data.length; i++) {
                    drawRow(data[i], i);
                }
            }
            else {
                $canvas.empty();
                if (data.length == 0)
                    $("<div style='width:" + _canvasWidth + "px;height:100%;'></div>").appendTo($canvas);
                else
                    $canvasL.css("overflow-x", "scroll");
                createCssRules();

                for (var i = 0; i < data.length; i++) {
                    drawRow(data[i], i);
                }
            }
        }
        function setData(_data) {
            data = _data;
        }
        function bindEvent() {
            if (defaults.frozenColumn) {
                $canvasL.bind("keydown", handleKeyDown)
                .bind("click", handleLClick)           

                $canvasR.bind("keydown", handleKeyDown)
               .bind("click", handleRClick)
                //.bind("mouseleave", handleMouseLeave);        

                //$container.bind("mouseleave", handleMouseLeave); //mouseleave

                $canvasR.scroll(function () {
                    $headersR.css("left", _canvasLWidth - $(this)[0].scrollLeft + "px");               
                    //$headersR[0].scrollLeft = $(this)[0].scrollLeft;                
                    $canvasL[0].scrollTop = $(this)[0].scrollTop;                   
                });
            }
            else {               
                $canvas.bind("keydown", handleKeyDown)
                .bind("click", handleClick)
                .bind("mouseleave", handleMouseLeave);

                $canvas.scroll(function () {
                    $headers.css("left",  - $(this)[0].scrollLeft + "px");                 
                });
            }

            $(window).resize(function () {
                resizeCanvas();
            });
        }

        function handleKeyDown(e) {
            //trigger(self.onKeyDown, { row: activeRow, cell: activeCell }, e);
            //var handled = e.isImmediatePropagationStopped();
            //if (!handled) {
            //    if (!e.shiftKey && !e.altKey && !e.ctrlKey) {
            //        if (e.which == 27) {
            //            if (!getEditorLock().isActive()) {
            //                return; // no editing mode to cancel, allow bubbling and default processing (exit without cancelling the event)
            //            }
            //            cancelEditAndSetFocus();
            //        } else if (e.which == 37) {
            //            handled = navigateLeft();
            //        } else if (e.which == 39) {
            //            handled = navigateRight();
            //        } else if (e.which == 38) {
            //            handled = navigateUp();
            //        } else if (e.which == 40) {
            //            handled = navigateDown();
            //        } else if (e.which == 9) {
            //            handled = navigateNext();
            //        } else if (e.which == 13) {
            //            if (options.editable) {
            //                if (currentEditor) {
            //                    // adding new row
            //                    if (activeRow === getDataLength()) {
            //                        navigateDown();
            //                    } else {
            //                        commitEditAndSetFocus();
            //                    }
            //                } else {
            //                    if (getEditorLock().commitCurrentEdit()) {
            //                        makeActiveCellEditable();
            //                    }
            //                }
            //            }
            //            handled = true;
            //        }
            //    } else if (e.which == 9 && e.shiftKey && !e.ctrlKey && !e.altKey) {
            //        handled = navigatePrev();
            //    }
            //}

            //if (handled) {
            //    // the event has been handled so don't let parent element (bubbling/propagation) or browser (default) handle it
            //    e.stopPropagation();
            //    e.preventDefault();
            //    try {
            //        e.originalEvent.keyCode = 0; // prevent default behaviour for special keys in IE browsers (F3, F5, etc.)
            //    }
            //    // ignore exceptions - setting the original event's keycode throws access denied exception for "Ctrl"
            //    // (hitting control key only, nothing else), "Shift" (maybe others)
            //    catch (error) {
            //    }
            //}
        }

        function handleLClick(e) {
            $cell = $(e.target).closest(".slick-cell2", $canvasL);
            handles(e,"L");
        }
        function handleRClick(e) {
            $cell = $(e.target).closest(".slick-cell2", $canvasR);          
            handles(e,"R");
        }

        function handleClick(e) {
            $cell = $(e.target).closest(".slick-cell2", $canvas);
            handles(e);        
            //trigger(self.onClick, { row: cell.row, cell: cell.cell }, e);
            //if (e.isImmediatePropagationStopped()) {
            //    return;
            //}

            //if ((activeCell != cell.cell || activeRow != cell.row) && canCellBeActive(cell.row, cell.cell)) {
            //    if (!getEditorLock().isActive() || getEditorLock().commitCurrentEdit()) {
            //        scrollRowIntoView(cell.row, false);
            //        setActiveCellInternal(getCellNode(cell.row, cell.cell), (cell.row === getDataLength()) || options.autoEdit);
            //    }
            //}

        }
        function handleMouseLeave(e) {         
            if (active != null ) {
                if (!getEditItem().cancel()) {
                    var value;
                    $("#txt_cancelmemo").val($("#txt_cancelmemo").val() + "cancel;");
                    if (getEditItem().isValueChanged()) {
                        value = getEditItem().getValue();                   
                        var _cellData = { x: x, y: y, clumn: getClumnName(y), value: value, item: data[x] };//getCellDataFromRow();
                        data[preX][getClumnName(preY)] = value;
                        trigger(self.onCellChange, _cellData, e);
                    }
              
                    if (editController.cell == null) return;
                            
                    //if (defaults.frozenColumn) {
                    //    if (y < defaults.frozenColumn)
                    //        $canvasL.children("div").eq(x).children("div").eq(y).html(value);
                    //    else
                    //        $canvasR.children("div").eq(x).children("div").eq(y - defaults.frozenColumn).html(value);
                    //}
                    //else {
                    //    $canvas.children("div").eq(x).children("div").eq(y).html(value);
                    //}          
                    getCell().html(value);
                    editController.cell = null;
                    editController.isEdited = null;
                    active = null;
                    return null;
                }
            }         
        }

        function handles(e, pos) {
            active = true;
            handle = e;
            //$("#txt_cancelmemo").val($("#txt_cancelmemo").val() + "handle = e;");
            if (!$cell.length) {              
                if (editController.cell != null) {
                    if (getEditItem().isValueChanged()) {
                        commitCurrentEdit();
                            var _cellData = getCellDataFromRow();
                            trigger(self.onCellChange, _cellData, e);
                            return;
                    }
                }
                commitCurrentEdit();
                active = null;
                return null;
            }

            var cell = getCellFromEvent(e);            
            applyCss($cell, pos);

            if (editController.cell != null) {
                //var _value = getEditItem().serializeValue();//editController.cell.find("input").val();
                if (getEditItem().isValueChanged()) {
                    if (cancelEdit()) {
                        var _cellData = getCellDataFromPreCache();
                        trigger(self.onCellChange, _cellData, e);
                    }
                }
            }
         
            //if (!cancelEdit()) {
            //    $("#txt_cancelmemo").val($("#txt_cancelmemo").val() + "if (!cancelEdit());");
            //    return;
            //}
            if (!makeActiveCellEditable()) return;

            var cellData = getCellData($cell);

            trigger(self.onClick, cellData, e);

            if (x != preX) {
                trigger(self.onSelectedRowsChanged, cellData, e);
            }
            if (x != preX || y != preY)
            { preX = x; preY = y; }
            //修正
            if (pos == "L")
                $cell = $(e.target).closest(".slick-cell2", $canvasL);
            else if (pos == "R")
                $cell = $(e.target).closest(".slick-cell2", $canvasR);
            else
                $cell = $(e.target).closest(".slick-cell2", $canvas);

            setEdit($cell);
            
        }

        function destroy() {

        }

        function handleblur() {
            if (editController.cell == null) return;
            var _value = getEditItem().serializeValue();
            editController.cell.empty();
            editController.cell.html(_value);         
            data[preX][getClumnName(preY)] = _value;
            editController.cell = null;
            editController.isEdited = null;
        } 
        function editCallback(e) {
            handleMouseLeave(e);
        }
        function resizeCanvas() {
            //alert("reSize");
        }

        function getCanvasSize() {
            _canvasWidth = 0, _canvasLWidth = 0, cc = 0;
            var _fix = _fixpad + _fixbod + _fixpad;
            if (defaults.frozenColumn) {
                for (var i = 0; i < columns.length; i++) {
                    if (columns[i].visible != null && !columns[i].visible) continue;
                    if (cc < defaults.frozenColumn) {
                        _canvasLWidth += columns[i].width + _fix;
                        cc++;
                    }
                    _canvasWidth += columns[i].width + _fix;
                }
            }
            else {
                for (var i = 0; i < columns.length; i++) {
                    if (columns[i].visible != null && !columns[i].visible) continue;
                    _canvasWidth += columns[i].width + _fix;
                }
            }
            var el = $container, _h = 0, _l = 0;
            while (el[0]) {
                _h = el[0].offsetHeight;
                if (_h != null && _h > 0)
                    break;
                el = el.offsetParent();
            }
            _containerHeight = _h;
        }

        function trigger(evt, args, e) {
            e = e || new Jgrid.EventData();
            args = args || {};
            args.grid = self;
            return evt.notify(args, e, self);
        }

        function registerPlugin() {

        }
        function getActClumnItem() {
            var _c = getClumnName(y);
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].id == _c)
                    return columns[i];
            }
            return null;
        }
        function getEditItem() {
            var _c = getClumnName(preY);
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].id == _c)
                    return columns[i];
            }
            return null;
        }

        function getCellData(obj) {
            return { x: x, y: y, clumn: getClumnName(y), value: data[x][getClumnName(y)], item: data[x]};
        }
        function getCellDataFromRow() {
            return { x: x, y: y, clumn: getClumnName(y), value: data[x][getClumnName(y)], item: data[x] };
        }
        function  getCellDataFromPreCache() {
            return { x: preX, y: preY, clumn: getClumnName(preY), value: data[preX][getClumnName(preY)], item: data[preX] };
        }
        function setEdit(obj) {
            var actclumnObj = getActClumnItem(),_cell;
            if (actclumnObj.editor) {
                //验证等
                cancelEdit();
                //_cell = getCell();
                editController.cell = $cell;
                editController.isEdited = true;
                $("#txt_cancelmemo").val($("#txt_cancelmemo").val() + "setEdit;");
                $cell.empty();
                actclumnObj.editor({ container: $cell, column: actclumnObj, item: data[x], grid: self, x: x, callback: editCallback });
                if (data[x][getClumnName(y)] != null)
                    actclumnObj.loadValue(data[x]);
            }
            else {
                cancelEdit();
            }
        }
        function getCell() {
            if (defaults.frozenColumn) {
                if (y < defaults.frozenColumn)
                    return $canvasL.children("div").eq(x).children("div").eq(y);
                else
                    return $canvasR.children("div").eq(x).children("div").eq(y - defaults.frozenColumn);
            }
            else {
                return $canvas.children("div").eq(x).children("div").eq(y);
            }
        }
        function cancelEdit() {          
            if (editController.cell == null) {
                $("#txt_cancelmemo").val($("#txt_cancelmemo").val() + "isEdited;");
                //if (preX != null && (x != preX || y != preY))//单元格切换
                    return true;
                //else
                //    return false;
            }
            if (preX != null && (x != preX || y != preY)) {//单元格切换则关闭之前 
                $("#txt_cancelmemo").val($("#txt_cancelmemo").val() + "preX;");
                handleblur();
                return true;
            }
            return false;
        }

        function commitCurrentEdit() {
            handleblur();
        }

        function makeActiveCellEditable() {
            if (trigger(self.onBeforeEditCell, { x: x, y: y, clumn: getClumnName(y), item: data[x] }) === false) {               
                return false;
            }
            return true;
        }

        function getCellFromEvent(e) {
            var _cc = $cell.attr("cc").split('r');
            x = parseInt(_cc[0]);// $cell.parent().parent().find(".slick-row2").index($cell.parent());
            y = parseInt(_cc[1]);// $cell.parent().find(".slick-cell2").index($cell);

            if (x == null || y == null) {
                return null;
            } else {
                return {
                    x: x,
                    y: y
                };
            }
        }

        function getClumnName(_y) {
            var _cid = $container.find(".slick-header").children()[_y].id;
            return _cid.substr(_grid.length, _cid.length - _grid.length + 1);
        }

    //    //function commitCurrentEdit() {
    //    //    var item = getDataItem(activeRow);
    //    //    var column = columns[activeCell];
    //    //    if (currentEditor) {
    //    //        if (currentEditor.isValueChanged()) {
    //    //            var validationResults = currentEditor.validate();
    //    //            if (validationResults.valid) {
    //    //                if (activeRow < getDataLength()) {
    //    //                    var editCommand = {
    //    //                        row: activeRow,
    //    //                        cell: activeCell,
    //    //                        editor: currentEditor,
    //    //                        serializedValue: currentEditor.serializeValue(),
    //    //                        prevSerializedValue: serializedEditorValue,
    //    //                        execute: function () {
    //    //                            this.editor.applyValue(item, this.serializedValue);
    //    //                            updateRow(this.row);
    //    //                        },
    //    //                        undo: function () {
    //    //                            this.editor.applyValue(item, this.prevSerializedValue);
    //    //                            updateRow(this.row);
    //    //                        }
    //    //                    };
    //    //                    if (options.editCommandHandler) {
    //    //                        makeActiveCellNormal();
    //    //                        options.editCommandHandler(item, column, editCommand);
    //    //                    } else {
    //    //                        editCommand.execute();
    //    //                        makeActiveCellNormal();
    //    //                    }
    //    //                    trigger(self.onCellChange, {
    //    //                        row: activeRow,
    //    //                        cell: activeCell,
    //    //                        item: item
    //    //                    });
    //    //                } else {
    //    //                    var newItem = {};
    //    //                    currentEditor.applyValue(newItem, currentEditor.serializeValue());
    //    //                    makeActiveCellNormal();
    //    //                    trigger(self.onAddNewRow, { item: newItem, column: column });
    //    //                }
    //    //                // check whether the lock has been re-acquired by event handlers
    //    //                return !getEditorLock().isActive();
    //    //            } else {
    //    //                // TODO: remove and put in onValidationError handlers in examples
    //    //                $(activeCellNode).addClass("invalid");
    //    //                $(activeCellNode).stop(true, true).effect("highlight", { color: "red" }, 300);
    //    //                trigger(self.onValidationError, {
    //    //                    editor: currentEditor,
    //    //                    cellNode: activeCellNode,
    //    //                    validationResults: validationResults,
    //    //                    row: activeRow,
    //    //                    cell: activeCell,
    //    //                    column: column
    //    //                });
    //    //                currentEditor.focus();
    //    //                return false;
    //    //            }
    //    //        }
    //    //        makeActiveCellNormal();
    //    //    }
    //    //    return true;
    //    //}
    //    //function cancelCurrentEdit() {
    //    //    makeActiveCellNormal();
    //    //    return true;
    //    //}

    //    //**************************************************getData**************/
        function getData(index) {
            return data[index];
        }
        function getSelectedRow() {
            return data[x];
        }
        function getSelectedRows() {
            return data[x];
        } 
   
        //**************************************************更新元素**************/   

        //******************************** Public API
        $.extend(this, {
            Version: "1.0",
            //*******************事件***************/
            onCellChange: new Jgrid.Event(),
            OnCellClick: new Jgrid.Event(),
            onClick: new Jgrid.Event(),
            onSelectedRowsChanged: new Jgrid.Event(),
            onCellChange: new Jgrid.Event(),
            onDblClick: new Jgrid.Event(),
            onKeyDown: new Jgrid.Event(),
            onBeforeEditCell: new Jgrid.Event(),

            //*******************方法***************/
            getData:getData,
            getSelectedRow: getSelectedRow,
            getSelectedRows: getSelectedRows,
            setData: setData,
            //getClumnName:
            //*******************渲染***************/
            updateRow:updateRow,
            render: render,
            resizeCanvas: resizeCanvas
        });

        init();
    }
})(jQuery);

//$(function () {
//    var grid2 = new Jgrid.Grid("#ddd", _data, columns, options);

//    grid2.OnCellClick.subscribe(function (e, args) {
//        var row = grid2.getSelectedRow();
//        var tt = grid2.getData(1);
//        //_data.push({ ID: 77 });
//        //tt = grid2.getData(2);
//        //grid2.render();
//    });

//});