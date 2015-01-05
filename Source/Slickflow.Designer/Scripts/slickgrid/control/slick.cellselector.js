(function ($) {
    $.extend(true, window, {
        "Slick": {
            "Cell": {
                "SelectionModel": SelectionModel,
                "CopyManager": CopyManager,
                "RangeToExcel": RangeToExcel,
                "RangeDecorator": RangeDecorator,
                "RangeSelector": RangeSelector,
                "AutoTooltips": AutoTooltips
            }
        }
    });

    //#region 选择模型
    function SelectionModel(options) {
        var _grid;
        var _canvas;
        var _ranges = [];
        var _self = this;
        var _selector = new Slick.Cell.RangeSelector({
            "selectionCss": {
                "border": "2px solid black"
            }
        });

        var _options;
        var _defaults = {
            selectActiveCell: true
        };
        var _data = options.data;
        var _columns = options.columns;

        function init(grid) {
            _options = $.extend(true, {}, _defaults, options);
            _grid = grid;
            _canvas = _grid.getCanvasNode();
            _grid.onActiveCellChanged.subscribe(handleActiveCellChange);
            grid.registerPlugin(_selector);
            _selector.onCellRangeSelected.subscribe(handleCellRangeSelected);
            _selector.onBeforeCellRangeSelected.subscribe(handleBeforeCellRangeSelected);
        }

        function destroy() {
            _grid.onActiveCellChanged.unsubscribe(handleActiveCellChange);
            _selector.onCellRangeSelected.unsubscribe(handleCellRangeSelected);
            _selector.onBeforeCellRangeSelected.unsubscribe(handleBeforeCellRangeSelected);
            _grid.unregisterPlugin(_selector);
        }

        function removeInvalidRange(ranges) {
            var result = [];
            for (var i = 0; i < ranges.length; i++) {
                var r = ranges[i];
                if (_grid.canCellBeSelected(r.fromRow, r.fromCell) && _grid.canCellBeSelected(r.toRow, r.toCell)) {
                    result.push(r);
                }
            }
            return result;
        }

        function setSelectedRanges(ranges) {
            _ranges = removeInvalidRange(ranges);
        }

        function getSelectedRanges() {
            return _ranges;
        }

        function handleBeforeCellRangeSelected(e, args) {
            if (_grid.getEditorLock().isActive()) {
                e.stopPropagation();
                return false;
            }
        }

        function handleCellRangeSelected(e, args) {
            setSelectedRanges([args.range]);
            getSumOnSelectedRangesChanged(args.range);
            _self.onSelectedRangesChanged.notify(_ranges);
        }

        function handleActiveCellChange(e, args) {
            if (_options.selectActiveCell) {
                setSelectedRanges([new Slick.Range(args.row, args.cell)]);
            }
        }

        //选定范围改变时，求和
        function getSumOnSelectedRangesChanged(range) {
            var fromRow = range.fromRow;
            var fromCell = range.fromCell;
            var toRow = range.toRow;
            var toCell = range.toCell;
            var sum = 0;

            var node;
            for (var i = 0; i <= toRow - fromRow; i++) {
                for (var j = 0; j <= toCell - fromCell; j++) {
                    node = _grid.getCellNode(fromRow + i, fromCell + j);
                    val = node.text();
                    sum = sum + Number.tryParseFloat(val, 0);
                }
            }

            //显示合计
            var node = _grid.getCellNode(toRow, toCell);
            setTimeout(function () {
                $(node).attr("title", "合计：" + sum);
            }, 1200);
        }

        $.extend(this, {
            "getSelectedRanges": getSelectedRanges,
            "setSelectedRanges": setSelectedRanges,
            "init": init,
            "destroy": destroy,

            "onSelectedRangesChanged": new Slick.Event()
        });
    }
    //#endregion

    //#region 复制管理器
    function CopyManager(args) {
        var _grid;
        var _self = this;
	    var _activeCell;
        var _copiedRanges;
        var _data = args.data;
        var _dataview = args.dataview;
        var _columns = args.columns;

        //菜单项显示明细
        var $menu;
        var menuItems = [
            { action: "note", bindType: "click", text: "批注", iconImage: "/Common/Content/default/js/slickgrid/images/sticky_note_plus.png" },
            { action: "copy", bindType: "click", text: "复制", iconImage: "/Common/Content/default/js/slickgrid/images/copy.png", newHorLine: true },
            { action: "paste", bindType: "click", text: "粘贴", iconImage: "/Common/Content/default/js/slickgrid/images/paste.png" },
            { action: "cancel", bindType: "click", text: "清除", newHorLine: true }
        ];

        function init(grid) {
            _grid = grid;
            _grid.onKeyDown.subscribe(handleKeyDown);
            _grid.onContextMenu.subscribe(handleCellContextMenu);
			_grid.onClick.subscribe(handleGridClick);

            $menu = $("<div class='slick-column-menu' style='display:none;position:absolute;z-index:20;'></div>")
                .appendTo(document.body);

            prepareCellMenuItem();
        }

        function destroy() {
            _grid.onKeyDown.unsubscribe(handleKeyDown);
            _grid.onContextMenu.unsubscribe(handleCellContextMenu);
            _grid.onClick.unsubscribe(handleGridClick);
            $menu.remove();
            $menu = null;
        }

	    function handleKeyDown(e, args) {
	        if (_grid.getEditorLock().isActive()) {
	            return;
	        }

		    if (e.which == $.ui.keyCode.ESCAPE) {
		        cancelCellRanges(e);
		    }
            
		    if (e.which == 67 && (e.ctrlKey || e.metaKey)) {
		        copyCellRanges(e);
		    }

		    if (e.which == 86 && (e.ctrlKey || e.metaKey)) {
		        pasteCellRanges(e);
		    }
	    }

        //复制单元格范围
        function copyCellRanges(e) {
            var ranges = _grid.getSelectionModel().getSelectedRanges();
            if (ranges.length != 0) {
                e.preventDefault();
                _copiedRanges = ranges;
                markCopySelection(ranges);
                _self.onCopyCells.notify({ ranges: ranges });
            }
        }

        //标记复制范围
        function markCopySelection(ranges) {
            var hash = {};
            for (var i = 0; i < ranges.length; i++) {
                for (var j = ranges[i].fromRow; j <= ranges[i].toRow; j++) {
                    hash[j] = {};
                    for (var k = ranges[i].fromCell; k <= ranges[i].toCell; k++) {
                        hash[j][_columns[k].id] = "copied";
                    }
                }
            }
            _grid.setCellCssStyles("copy-manager", hash);
        }

        //粘贴单元格范围
        function pasteCellRanges(e) {
            if (_copiedRanges) {
                e.preventDefault();
                clearCopySelection();
                var ranges = _grid.getSelectionModel().getSelectedRanges();

                if (ranges.length == 0) {
                    var cell = _grid.getCellFromEvent(e);
                    if (cell.row >= _data.length) {
                        //新增行粘贴记录
                        var range = new Slick.Range(cell.row, cell.cell, cell.row, cell.cell);
                        ranges.push(range);
                    }
                }
                //调用系统复制粘贴方法
                doPasteCellsContent({ from: _copiedRanges, to: ranges });

                //调用外部注册事件
                _self.onPasteCells.notify({ from: _copiedRanges, to: ranges });
                _copiedRanges = null;
            }
        }

        //粘贴操作
        function doPasteCellsContent(args) {
            if (args.from.length !== 1 || args.to.length !== 1) {
                throw "this implementation only supports single range copy and paste operations."
            }

            var val;
            var from = args.from[0];
            var to = args.to[0];

            for (var i = 0; i <= from.toRow - from.fromRow; i++) {
                for (var j = 0; j <= from.toCell - from.fromCell; j++) {
                    val = _data[from.fromRow + i][_columns[from.fromCell + j].field];

                    //判断是否是新生成记录行
                    if ((to.fromRow + i) >= _data.length) {
                        _data.push({});
                        _grid.updateRowCount();
                    }
                    _data[to.fromRow + i][_columns[to.fromCell + j].field] = val;
                    _grid.invalidateRow(to.fromRow + i);
                }
            }
            _grid.invalidate();
            _grid.render();
            _dataview.refresh();
        }

        //取消单元格范围
        function cancelCellRanges(e) {
            if (_copiedRanges) {
                e.preventDefault();
                _self.onCopyCancelled.notify({ ranges: _copiedRanges });
                _copiedRanges = null;
                clearCopySelection();
            }
        }

        function clearCopySelection() {
            _grid.removeCellCssStyles("copy-manager");
        }

        //生成菜单栏目项
        function prepareCellMenuItem() {
            for (var i = 0; i < menuItems.length; i++) {
                var item = menuItems[i];

                //line break
                if (item.newHorLine) {
                    $("<hr />").appendTo($menu);
                }

                //item content
                var $li = $("<div class='slick-column-menuitem' id='divPopMenuItem1001'></div>")
                    .attr("title", item.text)
                    .appendTo($menu);

                //icon
                var $icon = $("<div class='slick-column-menuicon'></div>")
                    .appendTo($li);
                if (item.iconImage) {
                    $icon.css("background-image", "url(" + item.iconImage + ")")
                }

                //right popmenu
                if (item.rightPopMenu) {
                    var $popMenu = $("<div class='slick-column-menupopicon' style='float:right'></div>")
                        .appendTo($li);
                    $popMenu.css("background-image", "url(/Common/Content/default/js/slickgrid/images/arrow-right.gif)");
                }

                //span
                var $span = $("<span class='slick-column-menucontent'></span>")
                    .text(item.text)
                    .appendTo($li);

                //bind event
                $li.bind("click", item, handleColumnMenuItemClick);
            }
        }

        //弹出右键菜单
        function handleCellContextMenu(e, args) {
            e.preventDefault();

	    _activeCell = { row: args.row, cell: args.cell };
            $menu
                .css("top", e.pageY - 5)
                .css("left", e.pageX - 10)
                .fadeIn(150);
        }

        //右键菜单项上的命令
        function handleColumnMenuItemClick(e, args) {
            e.preventDefault();
            e.stopPropagation();

	    var item = e.data;
	    if (item.bindType == "click") {
	        if (item.action == "note") {
	            _self.onNoteCell.notify({ cell: _activeCell });
	        }
	        else if (item.action == "copy") {
	            copyCellRanges(e);
	        }
	        else if (item.action == "paste") {
	            pasteCellRanges(e);
	        }
	        else if (item.action == "cancel") {
	            cancelCellRanges(e);
	        }
	    }
	    hideMenu();
	}

        //隐藏右键菜单
        function hideMenu() {
            if ($menu) {
                $menu.hide();
            }
        }

		function handleGridClick(e) {
		    hideMenu();
		}

		$.extend(this, {
			"init": init,
			"destroy": destroy,
			"clearCopySelection": clearCopySelection,

            "onNoteCell": new Slick.Event(),
            "onCopyCells": new Slick.Event(),
            "onCopyCancelled": new Slick.Event(),
            "onPasteCells": new Slick.Event()
        });
    }
    //#endregion

    //#region 导出Excel
    function RangeToExcel(options) {
        var options = jQuery.extend({
            separator: ',',
            header: [],
            delivery: 'popup'
        }, options);

        var csvData = [];
        var headerArr = [];
        var el = this;

        var numCols = options.header.length;
        var headerRow = [];
        var numArray = [];
        var count = 0;

        function init() {
            if (numCols > 0) {
                for (var i = 0; i < numCols; i++) {
                    headerRow[headerRow.length] = options.header[i];
                }
            } else {
                var tmpColumns = options.columns;
                for (var key in tmpColumns) {
                    var obj = tmpColumns[key];
                    for (var prop in obj) {
                        if (prop == "name") {
                            if (obj[prop] != "<input type='checkbox'>") {
                                headerRow[headerRow.length] = obj[prop];
                            }
                            else {
                                headerRow[headerRow.length] = " ";
                            }
                        }

                        if (prop == "id") {
                            numArray[obj[prop]] = count++;
                        }
                    }
                }
            }

            row2CSV(headerRow);

            for (var key in options.dataview) {
                var row = options.dataview[key];
                var tmpRow = [];

                for (var name in numArray) {
                    tmpRow[numArray[name]] = row[name];
                }
                row2CSV(tmpRow);
            }

            var mydata = csvData.join('\n');
            if (options.delivery == 'popup') {
                return popup(mydata);
            }
            else {
                return mydata;
            }
        }

        function row2CSV(tmpRow) {
            var tmp = tmpRow.join('');
            var mystr = '';
            if (tmpRow.length > 0 && tmp != '') {
                mystr = tmpRow.join(options.separator);
            }

            csvData[csvData.length] = mystr;
        }

        function popup(data) {
            if ($("#exportform") == null || $("#exportform").length == 0) {
                $("body").append('<form id="exportform" action="/Common/Common/Export" method="post"></form>');
            }

            $hidden = $("<input type='hidden' id='exportdata' name='exportdata'/>")
                .appendTo($("#exportform"));

            $hidden.val(data);
        }

        function exportExcel() {
            $("#exportform").submit().remove();
        }

        init();

        $.extend(this, {
            "exportExcel": exportExcel
        });
    }
    //#endregion

    //#region 范围装饰器
    function RangeDecorator(grid, options) {
        var _elem;
        var _defaults = {
            selectionCss: {
                "zIndex": "9999",
                "border": "2px dashed red"
            }
        };

        options = $.extend(true, {}, _defaults, options);

        function show(range) {
            if (!_elem) {
                _elem = $("<div></div>", { css: options.selectionCss })
                    .css("position", "absolute")
                    .appendTo(grid.getCanvasNode());
            }

            var from = grid.getCellNodeBox(range.fromRow, range.fromCell);
            var to = grid.getCellNodeBox(range.toRow, range.toCell);

            _elem.css({
                top: from.top - 1,
                left: from.left - 1,
                height: to.bottom - from.top - 2,
                width: to.right - from.left - 2
            });

            return _elem;
        }

        function hide() {
            if (_elem) {
                _elem.remove();
                _elem = null;
            }
        }

        $.extend(this, {
            "show": show,
            "hide": hide
        });
    }
    //#endregion

    //#region 范围选择器
    function RangeSelector(options) {
        var _grid;
        var _canvas;
        var _dragging;
        var _decorator;
        var _self = this;
        var _handler = new Slick.EventHandler();
        var _defaults = {
            selectionCss: {
                "border": "2px dashed blue"
            }
        };

        function init(grid) {
            options = $.extend(true, {}, _defaults, options);
            _decorator = new Slick.Cell.RangeDecorator(grid, options);
            _grid = grid;
            _canvas = _grid.getCanvasNode();
            _handler
                .subscribe(_grid.onDragInit, handleDragInit)
                .subscribe(_grid.onDragStart, handleDragStart)
                .subscribe(_grid.onDrag, handleDrag)
                .subscribe(_grid.onDragEnd, handleDragEnd);
        }

        function destroy() {
            _handler.unsubscribeAll();
        }

        function handleDragInit(e, dd) {
            e.stopImmediatePropagation();
        }

        function handleDragStart(e, dd) {
            var cell = _grid.getCellFromEvent(e);
            if (_self.onBeforeCellRangeSelected.notify(cell) !== false) {
                if (_grid.canCellBeSelected(cell.row, cell.cell)) {
                    _dragging = true;
                    e.stopImmediatePropagation();
                }
            }

            if (!_dragging) {
                return;
            }

            var start = _grid.getCellFromPoint(
                dd.startX - $(_canvas).offset().left,
                dd.startY - $(_canvas).offset().top);

            dd.range = { start: start, end: {} };
            return _decorator.show(new Slick.Range(start.row, start.cell));
        }

        function handleDrag(e, dd) {
            if (!_dragging) {
                return;
            }
            e.stopImmediatePropagation();

            var end = _grid.getCellFromPoint(
                e.pageX - $(_canvas).offset().left,
                e.pageY - $(_canvas).offset().top);

            if (!_grid.canCellBeSelected(end.row, end.cell)) {
                return;
            }

            dd.range.end = end;
            _decorator.show(new Slick.Range(dd.range.start.row, dd.range.start.cell, end.row, end.cell));
        }

        function handleDragEnd(e, dd) {
            if (!_dragging) {
                return;
            }

            _dragging = false;
            e.stopImmediatePropagation();

            _decorator.hide();

            _self.onCellRangeSelected.notify({
                range: new Slick.Range(
                    dd.range.start.row,
                    dd.range.start.cell,
                    dd.range.end.row,
                    dd.range.end.cell
                )
            });
        }

        function trigger(evt, args, e) {
            e = e || new Slick.EventData();
            args = args || {};
            args.grid = self;
            return evt.notify(args, e, self);
        }

        $.extend(this, {
            "init": init,
            "destroy": destroy,
            "onBeforeCellRangeSelected": new Slick.Event(),
            "onCellRangeSelected": new Slick.Event(),
            "onCellTested": new Slick.Event()
        });
    }
    //#endregion

    //#region 单元格提示信息
    function AutoTooltips(options) {
        var _grid;
        var _self = this;
        var _defaults = {
            maxToolTipLength: null
        };

        function init(grid) {
            options = $.extend(true, {}, _defaults, options);
            _grid = grid;
            _grid.onMouseEnter.subscribe(handleMouseEnter);
            _grid.onMouseLeave.subscribe(handleMouseLeave);
        }

        function destroy() {
            _grid.onMouseEnter.unsubscribe(handleMouseEnter);
            _grid.onMouseLeave.unsubscribe(handleMouseLeave);
        }

	    function handleMouseEnter(e, args) {
	        var cell = _grid.getCellFromEvent(e);
	        if (cell) {
	            var $node = $(_grid.getCellNode(cell.row, cell.cell));
	            var text;
	            if ($node.innerWidth() < $node[0].scrollWidth) {
	                text = $.trim($node.text());
	                if (options.maxToolTipLength && text.length > options.maxToolTipLength) {
	                    text = text.substr(0, options.maxToolTipLength - 3) + "...";
	                }
	            } else {
	                text = "";
	            }
	            $node.attr("title", text);
	        }
	    }

        function handleMouseLeave(e, args) {
            var cell = _grid.getCellFromEvent(e);
            if (cell) {
                var node = _grid.getCellNode(cell.row, cell.cell);
                $(node).attr("title", "");
            }
        }

        $.extend(this, {
            "init": init,
            "destroy": destroy
        });
    }
    //#endregion

})(jQuery);

