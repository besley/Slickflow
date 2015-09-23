(function ($) {
    // register namespace
    $.extend(true, window, {
        "Slick": {
            "Plugins": {
                "HeaderMenu": HeaderMenu,
                "HeaderButtons": HeaderButtons
            }
        }
    });

    //#region 列标题菜单
    function HeaderMenu(options) {
        var grid;
        var columnsInfo;
        var self = this;
        var handler = new Slick.EventHandler();
        var defaults = {
            buttonCssClass: null,
            buttonImage: "/Common/Content/default/js/slickgrid/images/down.gif"
        };

        var $menu;
        var $activeHeaderColumn;
        var dataview = options.dataview;

        var specialDefaultItem = {
            "BLANK": "hz16-ligongwangtr2013-filter-keywords-1002",
            "NONBLANK": "hz16-ligongwangtr2013-filter-keywords-1001",
            "ALL": "hz16-ligongwangtr2013-filter-keywords-1000"
        };

        //filterColumnMenuViewModel 
        //DATA STRUCTER:
        //[
        // {"column": "UnitPrice", "rawData": [10,20,1000], "checked": [10, 20], "expression": "UnitPrice == 10 || UnitPrice == 20"},
        // {"column": "ProductName", "rawData": ['Book','Phone'], "checked": ['Book'], "expression": "ProductName == 'Book'"}
        //]
        var filterColumnMenuViewModel = [];

        var defaultFilterItems = [
            {
                ID: specialDefaultItem.BLANK,
                Text: "(空值)",
                itemValueType: "blank",
                checked: "false"
            },
            {
                ID: specialDefaultItem.NONBLANK,
                Text: "(非空值)",
                itemValueType: "non-blank",
                checked: "false"
            },
            {
                ID: specialDefaultItem.ALL,
                Text: "(全部)",
                itemValueType: "all",
                checked: "false"
            }
        ];

        //列下拉菜单初始化
        function init(gridCtrl) {
            options = $.extend(true, {}, defaults, options);
            grid = gridCtrl;
            handler
                .subscribe(grid.onHeaderCellRendered, handleHeaderCellRendered)
                .subscribe(grid.onBeforeHeaderCellDestroy, handleBeforeHeaderCellDestroy);

            columnsInfo = grid.getColumns();
            prepareHeaderMenu(columnsInfo);
            grid.setColumns(columnsInfo);

            $(document.body).bind("mousedown", handleBodyMouseDown);
            grid.onClick.subscribe(handleGridClick);
        }

        //列标题头的默认项
        function prepareHeaderMenu(columns) {
            for (var i = 0; i < columns.length; i++) {
                var showHeaderMenu = columns[i].hasFilter;
                if (showHeaderMenu) {
                    columns[i].header = {
                        headerMenu: {
                            items: []
                        }
                    };
                }
            }
        }

        function handleHeaderCellRendered(e, args) {
            var column = args.column;
            var menu = column.header && column.header.headerMenu;

            if (menu) {
                var $el = $("<div></div>")
                    .addClass("slick-header-menubutton")
                    .data("column", column)
                    .data("headerMenu", menu);

                if (options.buttonCssClass) {
                    $el.addClass(options.buttonCssClass);
                }

                if (options.buttonImage) {
                    $el.css("background-image", "url(" + options.buttonImage + ")");
                }

                if (menu.tooltip) {
                    $el.attr("title", menul.tooltip);
                }

                $el
                    .bind("click", showMenu)
                    .appendTo(args.node);
            }
        }

        function handleBeforeHeaderCellDestroy(e, args) {
            var column = args.column;

            if (column.header && column.header.headerMenu) {
                $(args.node).find(".slick-header-menubutton").remove();
            }
        }

        //显示菜单
        function showMenu(e) {
            var $menuButton = $(this);
            var column = $menuButton.data("column");
            var menuModel = getMenuModelByColumnField(column.field);

            //构造菜单对象
            $menu = $("<div class='slick-header-menu' style='overflow-y: auto;'></div>")
                .appendTo(document.body);

            //重复显示
            if (menuModel) {
                renderMenuItem(menuModel.rawData, column);
                showMenuItemPosition($menuButton);
                return;
            }

            //填充菜单缺省内容
            var menuData = [];
            fillHeaderMenuContent(menuData, column);
            renderMenuItem(menuData, column);
            showMenuItemPosition($menuButton);

            //构造模型对象
            var innerModel = {};
            innerModel["column"] = column.field;
            innerModel["rawData"] = menuData;

            filterColumnMenuViewModel.push(innerModel);
        }

        //根据列名称获取列菜单数组
        function getMenuModelByColumnField(columnField) {
            var menuArray = jQuery.grep(filterColumnMenuViewModel, function (x) {
                return x.column == columnField;
            });

            var menuModel;
            if (menuArray && menuArray.length == 1) {
                menuModel = menuArray[0];
            }
            return menuModel;
        }

        //响应标题栏下拉菜单的预显示事件
        function fillHeaderMenuContent(headerMenu, column) {
            //填充3个默认菜单项
            var clonedArray = $.map(defaultFilterItems, function (obj) {
                return $.extend(true, {}, obj);
            });
            headerMenu.push.apply(headerMenu, clonedArray);

            if (column.fieldType == "dropdownlist") {
                //如果是外挂下拉框类型
                var dataSource = column.dataSource;
                for (var i = 0; i < dataSource.length; i++) {
                    headerMenu.push({
                        ID: dataSource[i].ID,
                        Text: dataSource[i].Text,
                        itemValueType: "dataItem"
                    });
                }
            }
            else {
                //如果是原始数组字段
                var txtView = "";
                var items = dataview.getItems();
                var filterItems = unique(items, column.field);
                for (var i = 0; i < filterItems.length; i++) {
                    if (column.fieldType == "boolean")
                        txtView = (parseBool(filterItems[i]) == true ? "是" : "否");
                    else
                        txtView = filterItems[i];

                    headerMenu.push({
                        ID: i,
                        Text: txtView,
                        Value: filterItems[i],
                        itemValueType: "dataItem"
                    });
                }
            }
        }

        //列出该字段枚举取值
        function renderMenuItem(menuData, column) {
            //先清除内容
            $menu.empty();

            //显示菜单项内容
            var item, checked;
            var $li, $check;

            var length = menuData.length;
            for (var i = 0; i < length; i++) {
                item = menuData[i];

                if (item.Text == "null")
                    continue;

                $li = $("<div class='slick-header-menuitem'></div>")
                    .data("itemValueType", item.itemValueType || '')
                    .data("column", column)
                    .data("item", item)
                    .bind("click", handleMenuItemClick)
                    .appendTo($menu);

                if (item.disabled) {
                    $li.addClass("slick-header-menuitem-disabled");
                }

                if (item.tooltip) {
                    $li.attr("title", item.tooltip);
                }

                //数据项和非数据项的显示
                //非数据项显示
                var filterType;
                if (item.itemValueType != "dataItem") {
                    filterType = 'single';
                }
                else {
                    filterType = 'multiple';
                }

                //绑定复选框事件
                checked = item.checked;
                if (checked == true){
                    $check = $("<input type='checkbox' filterType='" + filterType
                        + "' belongs='" + column.field
                        + "' checked='" + "checked"
                        + "' style='margin-right:5px;'/>");
                }
                else {
                    $check = $("<input type='checkbox' filterType='" + filterType
                        + "' belongs='" + column.field
                        + "' style='margin-right:5px;'/>");
                }

                $check.bind("click", handleMenuItemChecked)
                    .appendTo($li);

                //添加文本内容
                $("<span class='slick-header-menucontent'></span>")
                    .text(item.Text)
                    .appendTo($li);
            }

            var height = length * 15;
            $menu.css('height', height + "px");
        }

        //复选框事件
        function handleMenuItemChecked(e) {
            //字段
            var field = $(e.currentTarget).attr("belongs");
            //菜单数值项
            var menuText = $(this).parent().data("item").Text;
            var rawMenuData = getMenuModelByColumnField(field).rawData;
            var rawItems = jQuery.grep(rawMenuData, function (x) {
                return x.Text == menuText;
            });

            //记忆功能，第二次弹出菜单项，默认选中
            var rawItem = rawItems[0];
            if (e.currentTarget.checked == true) {
                rawItem.checked = true;
            }
            else {
                rawItem.checked = false;
            }
        }

        //按位置显示菜单项
        function showMenuItemPosition(menuButton) {
            $menu
                   .css("top", menuButton.offset().top + menuButton.height())
                   .css("left", menuButton.offset().left);

            $activeHeaderColumn = menuButton.closest(".slick-header-column");
            $activeHeaderColumn
                .addClass("slick-header-column-active");

            $menu.show();
        }

        //根据指定的属性，判断对象类型的数组元素的唯一性
        function unique(arr, property) {
            var i,
                len = arr.length,
                out = [],
                obj = {};

            for (i = 0; i < len; i++) {
                obj[arr[i][property]] = 0;
            }
            for (i in obj) {
                out.push(i);
            }
            return out;
        };

        //响应菜单项的单击事件
        function handleMenuItemClick(e) {
            var valueType = $(this).data("itemValueType");
            var columnInfo = $(this).data("column");
            
            var expression;

            var item = $(this).data("item");
            if (item.disabled) {
                return;
            }

            //window.console.log("filter checked data items...");
            var complexExpression;
            if (valueType != undefined) {
                var fieldName = "[" + String(columnInfo.field) + "]";
                //数据项
                var chkbox;
                var chkboxes = $("div.slick-header-menu :checkbox[belongs='" + columnInfo.field + "']");
                for (var i = 0; i < chkboxes.length; i++) {
                    chkbox = chkboxes[i];
                    if (chkbox.checked) {
                        item = $(chkbox).parent().data("item");
                        if ($(chkbox).attr("filterType") == "single") {
                            //特殊3项的处理：空、非空和全部
                            if (item.ID == specialDefaultItem.ALL)
                                expression = "1 == 1";
                            else if (item.ID == specialDefaultItem.NONBLANK)
                                expression = fieldName + "!= 'null'";
                            else if (item.ID == specialDefaultItem.BLANK)
                                expression = fieldName + "== 'null'";

                            if (complexExpression == undefined) {
                                complexExpression = expression;
                            }
                            else {
                                complexExpression = complexExpression + " || ";
                                complexExpression = complexExpression + expression;
                            }
                        }
                        else if ($(chkbox).attr("filterType") == "multiple") {
                            //数据项处理
                            if (complexExpression == undefined) {
                                //首次
                                complexExpression = getCheckedExpression(columnInfo, fieldName, item, chkbox);
                            }
                            else {
                                //多条件
                                complexExpression = complexExpression + " || ";
                                complexExpression = complexExpression + getCheckedExpression(columnInfo, fieldName, item, chkbox);
                            }
                        }
                    }
                }           

                //没有任何条件时，该列取值全部显示
                if (complexExpression == undefined)
                    complexExpression = " 1==1 ";

                //构造表达式并过滤
                buildFilterExpression(columnInfo.field, complexExpression);
                e.stopPropagation();

                //trigger onClickCommand event here
            }
        }

        //构造表达式，先给本次菜单项赋予表达式，然后计算表达式树，最后执行过滤
        function buildFilterExpression(columnField, expression) {
            var menuModel = getMenuModelByColumnField(columnField);
            menuModel.expression = expression;

            //遍历每一列的表达式
            var finalExpression = "";
            for (var i = 0; i < filterColumnMenuViewModel.length; i++) {
                var menuItem = filterColumnMenuViewModel[i];
                if (finalExpression && menuItem.expression)
                    finalExpression += " && ";
                finalExpression = finalExpression + "(" + menuItem.expression + ")";
            }
            //window.console.log(finalExpression);
            alert(finalExpression);
            filterArgs = {
                "field": columnsInfo,
                "fieldType": "all",
                "filterType": "complex",
                "colRangeType": "multiplecolumn",
                "expression": finalExpression
            };
            dataview.setFilterArgs(filterArgs);
            dataview.refresh();
        }

        function getCheckedExpression(columnInfo, fieldName, item, chkbox) {
            //可以多选，多个筛选条件
            var expression;
            if (columnInfo.fieldType == "dropdownlist")
                expression = fieldName + "==" + item.ID;
            else if (columnInfo.fieldType == "string")
                expression = fieldName + "=='" + item.Text + "'";   //严格匹配
            else if (columnInfo.fieldType == "number")
                expression = fieldName + "==" + item.Text;
            else if (columnInfo.fieldType == "boolean")
                expression = fieldName + "==" + parseBool(item.Value);
            else if (columnInfo.fieldType == "datetime")
                expression = fieldName + "==" + Date.parse(item.Text);
            else
                throw new Error("未知的数据类型或处理方式!");

            return expression;
        }

        function destroy() {
            handler.unsubscribeAll();
            $(document.body).unbind("mousedown", handleBodyMouseDown);
        }

        function handleBodyMouseDown(e) {
            if ($menu && $menu[0] != e.target && !$.contains($menu[0], e.target)) {
                hideMenu();
            }
        }

        function hideMenu() {
            if ($menu) {
                $menu.hide();
                $menu.remove();
                $activeHeaderColumn
                    .removeClass("slick-header-column-active");
            }
        }

        function handleGridClick() {
            hideMenu();
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
            "onClickCommand": new Slick.Event()
        });
    }
    //#endregion

    //#region 列标题按钮
    /***
  * A plugin to add custom buttons to column headers.
  *
  * USAGE:
  *
  * Add the plugin .js & .css files and register it with the grid.
  *
  * To specify a custom button in a column header, extend the column definition like so:
  *
  *   var columns = [
  *     {
  *       id: 'myColumn',
  *       name: 'My column',
  *
  *       // This is the relevant part
  *       header: {
  *          buttons: [
  *              {
  *                // button options
  *              },
  *              {
  *                // button options
  *              }
  *          ]
  *       }
  *     }
  *   ];
  *
  * Available button options:
  *    cssClass:     CSS class to add to the button.
  *    image:        Relative button image path.
  *    tooltip:      Button tooltip.
  *    showOnHover:  Only show the button on hover.
  *    handler:      Button click handler.
  *    command:      A command identifier to be passed to the onCommand event handlers.
  *
  * The plugin exposes the following events:
  *    onCommand:    Fired on button click for buttons with 'command' specified.
  *        Event args:
  *            grid:     Reference to the grid.
  *            column:   Column definition.
  *            command:  Button command identified.
  *            button:   Button options.  Note that you can change the button options in your
  *                      event handler, and the column header will be automatically updated to
  *                      reflect them.  This is useful if you want to implement something like a
  *                      toggle button.
  *
  *
  * @param options {Object} Options:
  *    buttonCssClass:   a CSS class to use for buttons (default 'slick-header-button')
  * @class Slick.Plugins.HeaderButtons
  * @constructor
  */
    function HeaderButtons(options) {
        var _grid;
        var _self = this;
        var _handler = new Slick.EventHandler();
        var _defaults = {
            buttonCssClass: "slick-header-button"
        };


        function init(grid) {
            options = $.extend(true, {}, _defaults, options);
            _grid = grid;
            _handler
              .subscribe(_grid.onHeaderCellRendered, handleHeaderCellRendered)
              .subscribe(_grid.onBeforeHeaderCellDestroy, handleBeforeHeaderCellDestroy);

            // Force the grid to re-render the header now that the events are hooked up.
            _grid.setColumns(_grid.getColumns());
        }


        function destroy() {
            _handler.unsubscribeAll();
        }


        function handleHeaderCellRendered(e, args) {
            var column = args.column;

            if (column.header && column.header.buttons) {
                // Append buttons in reverse order since they are floated to the right.
                var i = column.header.buttons.length;
                while (i--) {
                    var button = column.header.buttons[i];
                    var btn = $("<div></div>")
                      .addClass(options.buttonCssClass)
                      .data("column", column)
                      .data("button", button);

                    if (button.showOnHover) {
                        btn.addClass("slick-header-button-hidden");
                    }

                    if (button.image) {
                        btn.css("backgroundImage", "url(" + button.image + ")");
                    }

                    if (button.cssClass) {
                        btn.addClass(button.cssClass);
                    }

                    if (button.tooltip) {
                        btn.attr("title", button.tooltip);
                    }

                    if (button.command) {
                        btn.data("command", button.command);
                    }

                    if (button.handler) {
                        btn.bind("click", button.handler);
                    }

                    btn
                      .bind("click", handleButtonClick)
                      .appendTo(args.node);
                }
            }
        }


        function handleBeforeHeaderCellDestroy(e, args) {
            var column = args.column;

            if (column.header && column.header.buttons) {
                // Removing buttons via jQuery will also clean up any event handlers and data.
                // NOTE: If you attach event handlers directly or using a different framework,
                //       you must also clean them up here to avoid memory leaks.
                $(args.node).find("." + options.buttonCssClass).remove();
            }
        }


        function handleButtonClick(e) {
            var command = $(this).data("command");
            var columnDef = $(this).data("column");
            var button = $(this).data("button");

            if (command != null) {
                _self.onCommand.notify({
                    "grid": _grid,
                    "column": columnDef,
                    "command": command,
                    "button": button
                }, e, _self);

                // Update the header in case the user updated the button definition in the handler.
                _grid.updateColumnHeader(columnDef.id);
            }

            // Stop propagation so that it doesn't register as a header click event.
            e.preventDefault();
            e.stopPropagation();
        }

        $.extend(this, {
            "init": init,
            "destroy": destroy,

            "onCommand": new Slick.Event()
        });
    }
    //#endregion

})(jQuery);