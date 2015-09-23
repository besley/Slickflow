(function ($) {
    // register namespace
    $.extend(true, window, {
        "Slick": {
            "CheckboxSelectColumn": CheckboxSelectColumn,
            "DropDownListColumn": DropDownListColumn
        }
    });

    //#region 复选框列
    function CheckboxSelectColumn(options) {
        var _grid;
        var _self = this;
        var _handler = new Slick.EventHandler();
        var _selectedRowsLookup = {};
        var _lastSelectedRows = [];
        var _preCheckBox = "chkbox_li00gong59_";
        var _defaults = {
            columnId: "_checkbox_selector",
            cssClass: null,
            toolTip: "选择/取消选择",
            width: 30
        };

        var _options = $.extend(true, {}, _defaults, options);

        function init(grid) {
            _grid = grid;
            _handler
                .subscribe(_grid.onClick, handleClick)
                .subscribe(_grid.onHeaderClick, handleHeaderClick)
                .subscribe(_grid.onSelectedRowsChanged, handleSelectedRowsChanged)
                .subscribe(_grid.onKeyDown, handleKeyDown);
        }

        function destroy() {
            _handler.unsubscribeAll();
        }

        function handleKeyDown(e, args) {
            if (e.which == 32) {
                if (_grid.getColumns()[args.cell].id === _options.columnId) {
                    // if editing, try to commit
                    if (!_grid.getEditorLock().isActive() || _grid.getEditorLock().commitCurrentEdit()) {
                        toggleRowSelection(args.row);
                    }
                    e.preventDefault();
                    e.stopImmediatePropagation();
                }
            }
        }

        //复选框列单击事件
        function handleClick(e, args) {
            // clicking on a row select checkbox
            if (_grid.getColumns()[args.cell].id === _options.columnId && $(e.target).is(":checkbox")) {
                // if editing, try to commit
                if (_grid.getEditorLock().isActive() && !_grid.getEditorLock().commitCurrentEdit()) {
                    e.preventDefault();
                    e.stopImmediatePropagation();
                    return;
                }

                //标识行记录的复选框是否选中的状态, 添加了额外的标识属性;
                _grid.getDataItem([args.row])["checked"] = e.target.checked;

                toggleRowSelection(args.row);
                e.stopPropagation();
                e.stopImmediatePropagation();
                _self.onCheckBoxClicked.notify({ "row": args.row, "checked": e.target.checked});
            }
        }

        function toggleRowSelection(row) {
            if (_selectedRowsLookup[row]) {
                _grid.setSelectedRows($.grep(_grid.getSelectedRows(), function (n) {
                    return n != row
                }));
            } else {
                _grid.setSelectedRows(_grid.getSelectedRows().concat(row));
            }
        }

        //复选框列标题栏单击事件
        function handleHeaderClick(e, args) {
            if (args.column.id == _options.columnId && $(e.target).is(":checkbox")) {
                // if editing, try to commit
                if (_grid.getEditorLock().isActive() && !_grid.getEditorLock().commitCurrentEdit()) {
                    e.preventDefault();
                    e.stopImmediatePropagation();
                    return;
                }

                if ($(e.target).is(":checked")) {
                    var rows = [];
                    for (var i = 0; i < _grid.getDataLength() ; i++) {
                        rows.push(i);
                    }
                    _grid.setSelectedRows(rows);

                    //checkbox 置为选中状态
                    $("input:checkbox.slick-checkbox-firstColumn").attr("checked", "checked");
                } else {
                    _grid.setSelectedRows([]);

                    //checkbox 置为未选中
                    $("input:checkbox.slick-checkbox-firstColumn").removeAttr("checked");
                }
                e.stopPropagation();
                e.stopImmediatePropagation();
            }
        }

        //Grid 选定行改变事件
        function handleSelectedRowsChanged(e, args) {
            var selectedRows = _grid.getSelectedRows();
            if (selectedRows.length == 1) {
                var rowId = args.rows[0];
                if ($.inArray(rowId, _lastSelectedRows) == -1) {
                    //当前选中行未在之前选定行的范围内，所以先取消之前选中记录，再选取当前记录。
                    var chkboxId = _preCheckBox + rowId;
                    $("input:checkbox.slick-checkbox-firstColumn").removeAttr("checked");
                    $("#" + chkboxId).attr("checked", "checked");
                }
            }
            //保存选取行记录的数组
            _lastSelectedRows = selectedRows;
        }

        function getColumnDefinition() {
            return {
                id: _options.columnId,
                name: "<input type='checkbox'>",
                toolTip: _options.toolTip,
                field: "sel",
                width: _options.width,
                resizable: false,
                sortable: false,
                cssClass: _options.cssClass,
                formatter: checkboxSelectionFormatter
            };
        }

        function checkboxSelectionFormatter(row, cell, value, columnDef, dataContext) {
            if (dataContext) {
                var chkboxId = _preCheckBox + row;
                var input = "<input type='checkbox' class='slick-checkbox-firstColumn' id='" + chkboxId + "'";

                //判断行记录的选中状态，重新加载checkbox
                var item = dataContext;
                if (item.checked == true) {
                    input = input + " checked='checked'>";
                }
                else {
                    input = input + ">";
                }
                return input;
            }
            return null;
        }

        $.extend(this, {
            "init": init,
            "destroy": destroy,
            "onCheckBoxClicked": new Slick.Event(),
            "getColumnDefinition": getColumnDefinition
        });
    }
    //#endregion

    //#region 下拉框列
    function DropDownListColumn(options) {
        var _grid;
        var _self = this;
        var _handler = new Slick.EventHandler();
        var _selectedRowsLoop = {};
        var _default = {
            columnId: options.id,
            width: 60
        };

        var _options = $.extend(true, {}, _default, options);

        function init(grid) {
            _grid = grid;
        }

        function destroy() {
            _handler.unsubscribeAll();
        }

        function getColumnDefinition() {
            return {
                id: _options.columnId,
                name: _options.name,
                field: _options.field,
                fieldType: _options.fieldType,
                hasFilter: _options.hasFilter,
                dataSource: _options.dataSource,
                width: _options.width,
                resizable: false,
                sortable: false,
                formatter: dropdownlistFormatter
            };
        }

        function dropdownlistFormatter(row, cell, value, columnDef, dataContext) {
            if (dataContext) {
                var dataSource = _options.dataSource;
                var option_str = "";
                var preSelect = "<SELECT tabIndex='0' id='slt_" + columnDef.field + "' class='editor-select'><OPTION value='-1'></OPTION>";
                var endSelect = "</SELECT>";
                for (var i = 0, len = dataSource.length; i < len; i++) {
                    if (dataContext[columnDef.field] == dataSource[i].ID) {
                        option_str += "<OPTION value='" + dataSource[i].ID + "' selected='true'>" + dataSource[i].Text + "</OPTION>";
                    }
                    else {
                        option_str += "<OPTION value='" + dataSource[i].ID + "'>" + dataSource[i].Text + "</OPTION>";
                    }
                }
                var list = preSelect + option_str + endSelect;
                return list;
            }
        }

        $.extend(this, {
            "init": init,
            "destroy": destroy,
            "getColumnDefinition": getColumnDefinition
        });
    }
    //#endregion

})(jQuery);