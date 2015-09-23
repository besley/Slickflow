(function ($) {
    //column picker
    $.extend(true, window, {
        "Slick": {
            "Controls": {
                "Pager": SlickGridPager,
                "PagerSvr": SlickGridPagerSvr,
                "ColumnPicker": SlickColumnPicker,
                "filterDialog": SlickFilterDialog
            }
        }
    });

    //#region 客户端分页控件
    function SlickGridPager(dataView, grid, $container) {
        var $status;

        function init() {
            dataView.onPagingInfoChanged.subscribe(function (e, pagingInfo) {
                updatePager(pagingInfo);
            });

            constructPagerUI();
            updatePager(dataView.getPagingInfo());
        }

        function getNavState() {
            var cannotLeaveEditMode = !Slick.GlobalEditorLock.commitCurrentEdit();
            var pagingInfo = dataView.getPagingInfo();
            var lastPage = pagingInfo.totalPages - 1;

            return {
                canGotoFirst: !cannotLeaveEditMode && pagingInfo.pageSize != 0 && pagingInfo.pageNum > 0,
                canGotoLast: !cannotLeaveEditMode && pagingInfo.pageSize != 0 && pagingInfo.pageNum != lastPage,
                canGotoPrev: !cannotLeaveEditMode && pagingInfo.pageSize != 0 && pagingInfo.pageNum > 0,
                canGotoNext: !cannotLeaveEditMode && pagingInfo.pageSize != 0 && pagingInfo.pageNum < lastPage,
                pagingInfo: pagingInfo
            }
        }

        function setPageSize(n) {
            dataView.setRefreshHints({
                isFilterUnchanged: true
            });
            dataView.setPagingOptions({ pageSize: n });
        }

        function gotoFirst() {
            if (getNavState().canGotoFirst) {
                dataView.setPagingOptions({ pageNum: 0 });
            }
        }

        function gotoLast() {
            var state = getNavState();
            if (state.canGotoLast) {
                dataView.setPagingOptions({ pageNum: state.pagingInfo.totalPages - 1 });
            }
        }

        function gotoPrev() {
            var state = getNavState();
            if (state.canGotoPrev) {
                dataView.setPagingOptions({ pageNum: state.pagingInfo.pageNum - 1 });
            }
        }

        function gotoNext() {
            var state = getNavState();
            if (state.canGotoNext) {
                dataView.setPagingOptions({ pageNum: state.pagingInfo.pageNum + 1 });
            }
        }

        function constructPagerUI() {
            $container.empty();

            var $nav = $("<span class='slick-pager-nav' />").appendTo($container);
            var $settings = $("<span class='slick-pager-settings' />").appendTo($container);
            $status = $("<span class='slick-pager-status' />").appendTo($container);

            $settings
                .append("<span class='slick-pager-settings-expanded' style='display:none'>Show: <a data=0>All</a><a data='-1'>Auto</a><a data=25>25</a><a data=50>50</a><a data=100>100</a></span>");

            $settings.find("a[data]").click(function (e) {
                var pagesize = $(e.target).attr("data");
                if (pagesize != undefined) {
                    if (pagesize == -1) {
                        var vp = grid.getViewport();
                        setPageSize(vp.bottom - vp.top);
                    } else {
                        setPageSize(parseInt(pagesize));
                    }
                }
            });

            var icon_prefix = "<span class='ui-state-default ui-corner-all ui-icon-container'><span class='ui-icon ";
            var icon_suffix = "' /></span>";

            $(icon_prefix + "ui-icon-lightbulb" + icon_suffix)
                .click(function () {
                    $(".slick-pager-settings-expanded").toggle()
                })
                .appendTo($settings);

            $(icon_prefix + "ui-icon-seek-first" + icon_suffix)
                .click(gotoFirst)
                .appendTo($nav);

            $(icon_prefix + "ui-icon-seek-prev" + icon_suffix)
                .click(gotoPrev)
                .appendTo($nav);

            $(icon_prefix + "ui-icon-seek-next" + icon_suffix)
                .click(gotoNext)
                .appendTo($nav);

            $(icon_prefix + "ui-icon-seek-end" + icon_suffix)
                .click(gotoLast)
                .appendTo($nav);

            $container.find(".ui-icon-container")
                .hover(function () {
                    $(this).toggleClass("ui-state-hover");
                });

            $container.children().wrapAll("<div class='slick-pager' />");
        }


        function updatePager(pagingInfo) {
            var state = getNavState();

            $container.find(".slick-pager-nav span").removeClass("ui-state-disabled");
            if (!state.canGotoFirst) {
                $container.find(".ui-icon-seek-first").addClass("ui-state-disabled");
            }
            if (!state.canGotoLast) {
                $container.find(".ui-icon-seek-end").addClass("ui-state-disabled");
            }
            if (!state.canGotoNext) {
                $container.find(".ui-icon-seek-next").addClass("ui-state-disabled");
            }
            if (!state.canGotoPrev) {
                $container.find(".ui-icon-seek-prev").addClass("ui-state-disabled");
            }

            if (pagingInfo.pageSize == 0) {
                $status.text("Showing all " + pagingInfo.totalRows + " rows");
            } else {
                $status.text("Showing page " + (pagingInfo.pageNum + 1) + " of " + pagingInfo.totalPages);
            }
        }

        init();
    }
    //#endregion

    //#region 服务端分页控件
    function SlickGridPagerSvr(dataView, grid, pageInfo, $container, options) {
        var $status;
        var self = this;
        var gridId = grid.getGridId();
        var txtGotoPageNumId = gridId + "_" + "txtGotoPageNum";
        var txtPageRowsNumId = gridId + "_" + "txtPageRowsNum";
        var lblTotalPagesCountId = gridId + "_" + "lblTotalPagesCount";
        var txtGotoPageNum, txtPageRowsNum, lblTotalPagesCount;

        function init() {
            constructPagerUI();
            updatePager();
        }

        function getNavState() {
            var cannotLeaveEditMode = !Slick.GlobalEditorLock.commitCurrentEdit();
            var lastPage = pageInfo.totalPagesCount - 1;

            return {
                canGotoFirst: !cannotLeaveEditMode && pageInfo.pageSize != 0 && pageInfo.pageNum > 0,
                canGotoLast: !cannotLeaveEditMode && pageInfo.pageSize != 0 && pageInfo.pageNum != lastPage,
                canGotoPrev: !cannotLeaveEditMode && pageInfo.pageSize != 0 && pageInfo.pageNum > 0,
                canGotoNext: !cannotLeaveEditMode && pageInfo.pageSize != 0 && pageInfo.pageNum < lastPage,
                pagingInfo: pageInfo
            }
        }

        function gotoFirst() {
            var state = getNavState();
            if (state.canGotoFirst) {
                var pSize = state.pagingInfo.pageSize;
                trigger(self.onDataPaged, { pageNum: 0, pageSize: pSize });
            }
        }

        function gotoNext() {
            var state = getNavState();
            if (state.canGotoNext) {
                var pNum = state.pagingInfo.pageNum + 1;
                var pSize = state.pagingInfo.pageSize;
                trigger(self.onDataPaged, { pageNum: pNum, pageSize: pSize });
            }
        }

        function gotoPrev() {
            var state = getNavState();
            if (state.canGotoPrev) {
                var pNum = state.pagingInfo.pageNum - 1;
                var pSize = state.pagingInfo.pageSize;
                trigger(self.onDataPaged, { pageNum: pNum, pageSize: pSize });
            }
        }

        function gotoLast() {
            var state = getNavState();
            if (state.canGotoLast) {
                var pNum = state.pagingInfo.totalPagesCount - 1;
                var pSize = state.pagingInfo.pageSize;
                trigger(self.onDataPaged, { pageNum: pNum, pageSize: pSize });
            }
        }

        function gotoPageNum(e) {
            if (txtGotoPageNum.val() != txtGotoPageNum.val().replace(/[^0-9\.]/g, '')) {
                var value = txtGotoPageNum.val().replace(/[^0-9\.]/g, '');
                txtGotoPageNum.text(value);
            }

            //enter key is pressed
            if (e.keyCode == 13) {
                var pageIndex = txtGotoPageNum.val();
                pageIndex = parseInt(pageIndex) - 1;
                if (pageIndex != undefined) {
                    if ((pageIndex >= 0) && (pageIndex <= pageInfo.totalPagesCount)) {
                        trigger(self.onDataPaged, { pageNum: pageIndex, pageSize: pageInfo.pageSize });
                    }
                }
            }
        }

        function constructPagerUI() {
            $container.empty();

            var $nav = $("<span class='slick-pager-nav'/>").appendTo($container);
            var $settings = $("<span class='slick-pager-settings'/>").appendTo($container);
            $status = $("<span class='slick-pager-status slick-pager-rows-number'/>").appendTo($container);

            $settings
                .append("<span class='slick-pager-settings-expanded' style='display:none'>每页显示: <input id='"
                + txtPageRowsNumId + "' type='text' style='width:20px;'><a data=100>100</a><a data=200>200</a><a data=500>500</a></span>");

            //get jquery element
            txtPageRowsNum = $("#" + txtPageRowsNumId);

            $settings.find("a[data]").click(function (e) {
                var pagesize = $(e.target).attr("data");
                if (pagesize != undefined) {
                    txtPageRowsNum.val("");
                    if (pagesize > 0) {
                        trigger(self.onDataPaged, { pageNum: 0, pageSize: pagesize });
                    }
                }
            });

            //输入自定义的每页显示的记录数
            txtPageRowsNum.keyup(function (e) {
                if (this.value != this.value.replace(/[^0-9\.]/g, '')) {
                    this.value = this.value.replace(/[^0-9\.]/g, '');
                }

                //when enter key is pressed
                if (e.keyCode == 13) {
                    var pagesize = this.value;
                    if (pagesize != undefined) {
                        if (pagesize > 0) {
                            trigger(self.onDataPaged, { pageNum: 0, pageSize: pagesize });
                        }
                    }
                }
            });

            var icon_prefix = "<span class='ui-state-default ui-corner-all ui-icon-container'><span style='display:inline-block;' class='ui-icon ";
            var icon_suffix = "' /></span>";

            $(icon_prefix + "ui-icon-lightbulb" + icon_suffix)
                .click(function () {
                    $(".slick-pager-settings-expanded").toggle();
                    if (options.isCompact && options.isCompact == true) {
                        $(".slick-pager-rows-number").toggle();
                    }
                })
                .appendTo($settings);

            $(icon_prefix + "ui-icon-seek-first" + icon_suffix)
                .click(gotoFirst)
                .appendTo($nav);

            $(icon_prefix + "ui-icon-seek-prev" + icon_suffix)
                .click(gotoPrev)
                .appendTo($nav);

            var pageGoto_prefix = "<span class='slick-pager-status' style='vertical-align:top'><input id='"
                + txtGotoPageNumId + "' type='text' style='width:15px;'/>/<label id='"
                + lblTotalPagesCountId + "'></label>";
            var pageGoto_suffix = "</span>";

            $(pageGoto_prefix + pageGoto_suffix)
                .keyup(gotoPageNum)
                .appendTo($nav);

            //get juqery element
            txtGotoPageNum = $("#" + txtGotoPageNumId);
            lblTotalPagesCount = $("#" + lblTotalPagesCountId);

            $(icon_prefix + "ui-icon-seek-next" + icon_suffix)
                .click(gotoNext)
                .appendTo($nav);

            $(icon_prefix + "ui-icon-seek-end" + icon_suffix)
                .click(gotoLast)
                .appendTo($nav);

            $container.find(".ui-icon-container")
                .hover(function () {
                    $(this).toggleClass("ui-state-hover");
                });

            $container.children().wrapAll("<div class='slick-pager'/>");
        }

        function updatePager() {
            var state = getNavState();

            $container.find(".slick-pager-nav span").removeClass("ui-state-disabled");
            if (!state.canGotoFirst) {
                $container.find(".ui-icon-seek-first").addClass("ui-state-disabled");
            }
            if (!state.canGotoLast) {
                $container.find(".ui-icon-seek-end").addClass("ui-state-disabled");
            }
            if (!state.canGotoNext) {
                $container.find(".ui-icon-seek-next").addClass("ui-state-disabled");
            }
            if (!state.canGotoPrev) {
                $container.find(".ui-icon-seek-prev").addClass("ui-state-disabled");
            }

            var rowsText;
            if (pageInfo.totalPagesCount != 0) {
                var pNum = parseInt(pageInfo.pageNum);
                
                txtGotoPageNum.val(pNum + 1);
                lblTotalPagesCount.text(pageInfo.totalPagesCount);
            }
            $status.text("每页: " + pageInfo.pageSize + " 共: " + pageInfo.totalRowsCount + " 条");
        }

        function trigger(evt, args, e) {
            e = e || new Slick.EventData();
            args = args || {};
            args.grid = self;
            return evt.notify(args, e, self);
        }

        init();

        $.extend(this, {
            "onGoFirst": new Slick.Event(),
            "onGoNext": new Slick.Event(),
            "onGoPrev": new Slick.Event(),
            "onGoLast": new Slick.Event(),
            "onDataPaged": new Slick.Event()
        });
    }
    //#endregion

    //#region 列选择器
    function SlickColumnPicker(columns, grid, dataview, options) {
        var $menu;
        var $popMenu;
        var _pickedColumnDef;
        var self = this;
        var simpleColDef = Slick.Data.SlickColumnDefSimplify(columns);

        var defaults = {
            fadeSpeed: 150
        };

        //菜单项显示明细
        var menuItems = [
            { action: "ascSort", bindType: "click", text: "升序", iconImage: "/Common/Content/default/js/slickgrid/images/sort_ascend.png" },
            { action: "descSort", bindType: "click", text: "降序", iconImage: "/Common/Content/default/js/slickgrid/images/sort_descend.png" },
            { action: "defaultSort", bindType: "click", text: "清除排序" },
            { action: "showColumns", bindType: "mouseover", text: "显示列", newHorLine: true, rightPopMenu: true, iconImage: "/Common/Content/default/js/slickgrid/images/column-chooser.gif" },
            { action: "resizeColumns", bindType: "mouseover", text: "列宽度", rightPopMenu: true, iconImage: "/Common/Content/default/js/slickgrid/images/column-format.png" },
            { action: "filterCurrentCol", bindType: "mouseover", text: "过滤(当前列)", newHorLine: true, rightPopMenu: true, iconImage: "/Common/Content/default/js/slickgrid/images/filter-single.png" },
            { action: "filterCompositor", bindType: "dialog", text: "过滤器(多列)", iconImage: "/Common/Content/default/js/slickgrid/images/filter-multiple.gif" },
            { action: "filterColumnsClear", bindType: "click", text: "清除过滤" }
        ];

        //控件初始化
        function init() {
            //右键菜单在slickgrid.cs的onHeaderContextMenu里的contextmenu
            grid.onHeaderContextMenu.subscribe(initHeaderContextMenu);
            grid.onClick.subscribe(handleGridClick);
            options = $.extend({}, defaults, options);

            $menu = $("<div class='slick-column-menu' style='display:none;position:absolute;z-index:20;'></div>")
                   .appendTo(document.body);

            // Hide the menu on outside click.
            $(document.body).bind("mousedown", handleBodyMouseDown);
        }

        //初始化菜单
        function initHeaderContextMenu(e, args) {
            e.preventDefault();

            //var columnDef = $(this).data("column");
            _pickedColumnDef = args.column;
            if (!$menu) {
                $menu = $("<div class='slick-column-menu'></div>")
                    .appendTo(document.body);
            }
            $menu.empty();

            //初始化右键菜单项
            prepareColumnMenuItem();

            $menu
                .css("top", e.pageY - 5)
                .css("left", e.pageX - 10)
                .fadeIn(options.fadeSpeed);
        }

        //#region 样式展现部分
        //生成菜单栏目项
        function prepareColumnMenuItem() {
            for (var i = 0; i < menuItems.length; i++) {
                var item = menuItems[i];

                //line break
                if (item && item.newHorLine) {
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
                $li
                    .bind("mouseover", item, handleColumnMenuItemMouseover)
                    .bind("click", item, handleColumnMenuItemClick);
            }
        }

        var columnCheckboxes = [];

        //鼠标移过菜单项时，展开二级复选项窗口
        function handleColumnMenuItemMouseover(e) {
            e.preventDefault();
            e.stopPropagation();

            var item = e.data;
            if (item.bindType != "mouseover") {
                //如果是单击和dialog菜单项，则隐藏popmenu
                hidePopMenu();
                return;
            }

            //初始化PopMenu
            if (!$popMenu) {
                $popMenu = $("<span class='slick-columnpicker' style='display:none;position:absolute;z-index:20;overflow-y: auto;' />")
                    .appendTo(document.body);
            }
            $popMenu.empty();

            if (item.action == "resizeColumns") {
                $li = $("<li />")
                    .bind("click", refreshColumnStyle)
                    .appendTo($popMenu);

                $input = $("<input type='checkbox' />")
                    .data("option", "autoresize");
                    

                $("<label />")
                    .text("自动列宽")
                    .prepend($input)
                    .appendTo($li);
                if (grid.getOptions().forceFitColumns) {
                    $input.attr("checked", "checked");
                }
                //#region 同步列宽目前没有用到
                //$li = $("<li />").appendTo($popMenu);
                //$input = $("<input type='checkbox' />")
                //    .data("option", "syncresize")
                //    .bind("click", refreshColumnStyle);

                //$("<label />")
                //    .text("同步列宽")
                //    .prepend($input)
                //    .appendTo($li);

                //if (grid.getOptions().syncColumnCellResize) {
                //    $input.attr("checked", "checked");
                //}
                //#endregion
            }
            else if (item.action == "showColumns") {
                columnCheckboxes = [];
                var length = columns.length;
                for (var i = 0; i < length; i++) {
                    $li = $("<li />")
                        .appendTo($popMenu);

                    var chkboxId = "chk_" + Math.ceil(Math.random() * 1000000);
                    $input = $("<input type='checkbox' id='" + chkboxId + "' />")
                        .bind("click", refreshColumnStyle)
                        .appendTo($li);

                    if (columns[i].id){
                        $input.data("column-id", columns[i].id);
                    }

                    if (grid.getColumnIndex(columns[i].id) != null) {
                        $input.attr("checked", "checked");
                    }
                       
                    columnCheckboxes.push($input);

                    var title = columns[i].name;
                    if (columns[i].id == "_checkbox_selector")
                        title = "复选框";

                    $("<label />")
                        .text(title)
                        .prepend($input)
                        .appendTo($li);
                }
                var height = length * 15;
                $popMenu.css('height', height + 'px');
            }
            else if (item.action == "filterCurrentCol") {
                var fieldType = _pickedColumnDef.fieldType;
                if (fieldType == "string") {
                    $input = $("<input type='textbox' style='height:20px' />")
                        .data("column-id", _pickedColumnDef.id)
                        .bind("keyup", { "columnInfo": _pickedColumnDef, "operator": "contain" }, handleFilterColumnKeyup)
                        .appendTo($popMenu);
                }
                else if (fieldType == "number") {
                    appendNumberInput($popMenu, "greater", ">");
                    appendNumberInput($popMenu, "equal", "=");
                    appendNumberInput($popMenu, "less", "<");
                }
                else if (fieldType == "datetime") {
                    appendDatetimeInput($popMenu, "greater", ">");
                    appendDatetimeInput($popMenu, "equal", "=");
                    appendDatetimeInput($popMenu, "less", "<");
                }
                else if (fieldType == "boolean") {
                    appendBooleanInput($popMenu, "equal", "=");
                }
                else if (fieldType == "dropdownlist") {
                    appendDropdownlistInput($popMenu, "greater", ">");
                    appendDropdownlistInput($popMenu, "equal", "=");
                    appendDropdownlistInput($popMenu, "less", "<");
                }
            }

            $popMenu
                .css("top", e.pageY - 20)
                .css("left", $menu.offset().left + 80)
                .fadeIn(options.fadeSpeed);
        }

        //重新生成列样式
        function refreshColumnStyle(e) {
            if ($(e.target).data("option") == "autoresize") {
                if (e.target.checked) {
                    grid.setOptions({ forceFitColumns: true });
                    grid.autosizeColumns();
                } else {
                    grid.setOptions({ forceFitColumns: false });
                }
                return false;
            }

            if ($(e.target).data("option") == "syncresize") {
                if (e.target.checked) {
                    grid.setOptions({ syncColumnCellResize: true });
                } else {
                    grid.setOptions({ syncColumnCellResize: false });
                }
                return false;
            }

            if ($(e.target).is(":checkbox")) {
                var visibleColumns = [];
                $.each(columnCheckboxes, function (i, e) {
                    if ($(this).is(":checked")) {
                        visibleColumns.push(columns[i]);
                    }
                });

                if (!visibleColumns.length) {
                    $(e.target).attr("checked", "checked");
                    return false;
                }

                grid.setColumns(visibleColumns);
            }
        }

        //处理菜单隐藏
        function handleBodyMouseDown(e) {
            if ($popMenu
                && ($popMenu[0] != e.target)
                && (!$.contains($popMenu[0], e.target))
                && (e.target.className.indexOf("ui-state-default") == -1)) {
                hidePopMenu();
            }

            if ($menu && $menu[0] != e.target && !$popMenu && !$.contains($menu[0], e.target)) {
                hideMenu();
            }
        }

        function handleGridClick(e) {
            hidePopMenu();
            hideMenu();
        }

        //隐藏主菜单
        function hideMenu() {
            if ($menu) {
                $menu.remove();
                $menu = null;
            }
        }

        //隐藏二级菜单
        function hidePopMenu() {
            if ($popMenu) {
                $popMenu.remove();
                $popMenu = null;
            }
        }
        //#endregion

        //#region 数据处理部分
        //单击菜单项，直接触发外部事件
        function handleColumnMenuItemClick(e) {
            e.preventDefault();
            e.stopPropagation();

            var item = e.data;
            if (item.bindType == "click") {
                if (item.action == "ascSort") {
                    trigger(self.onDataColumnSort, { field: _pickedColumnDef.field, sort: "asc" });
                }
                else if (item.action == "descSort") {
                    trigger(self.onDataColumnSort, { field: _pickedColumnDef.field, sort: "desc" });
                }
                else if (item.action == "defaultSort") {
                    trigger(self.onDataColumnSort, { field: _pickedColumnDef.field, sort: "default" });
                }
                else if (item.action == "filterColumnsClear") {
                    clearSingleColumnFilter(_pickedColumnDef.field, _pickedColumnDef.fieldType);
                }
            }
            else if (item.bindType == "dialog") {
                //将列信息临时写入cookie，弹出对话框
                var colInfo = JSON.stringify(simpleColDef);
                setCookie("colDefEPProduct", colInfo, 1);

                var dialogContainerId = "divFilterDialog_" + grid.getGridId();
                var divFilterContainer = $("<div id='" + dialogContainerId + "' />").appendTo("body");
                var filterHtmlPage = divFilterContainer.load("/Common/Content/default/js/slickgrid/control/slick.filterdialog.html",
                    function () {
                        var dialogOptions = {
                            title: '多列过滤',
                            width: 450,
                            height: 500,
                            position: 'center',
                            modal: false,
                            resizable: true,
                            autoOpen: false,
                            close: function (event, ui) {
                                $(this).dialog("destroy");
                                $(this).remove();
                            }
                        };

                        filterHtmlPage
                            .dialog(dialogOptions)
                            .dialog('open');
                });
            }
        }

        //数字类型字段大于、等于和小于的输入框
        function appendNumberInput(popMenu, title, operator) {
            $li = $("<li />").appendTo($popMenu);
            $label = $("<label />")
                .text(operator)
                .appendTo($li);

            $input = $("<input type=textbox' style='height:20px' />")
                .data("column-id", _pickedColumnDef.id + "_" + title)
                .bind("keyup", { "columnInfo": _pickedColumnDef, "operator": operator }, handleFilterColumnKeyup)
                .prependTo($label)
                .appendTo($li);
        }

        //响应输入条件的键盘事件
        function handleFilterColumnKeyup(e, args) {
            if (e.keyCode == 13) {
                var filterValue = this.value;

                handleSingleColumnFilter(e.data.columnInfo.field, e.data.columnInfo.fieldType, e.data.operator, filterValue);
            }
        }

        //日期类型字段大于、等于和小于的输入框
        function appendDatetimeInput(popMenu, title, operator) {
            $li = $("<li />").appendTo($popMenu);
            $label = $("<label />")
                .text(operator)
                .appendTo($li);

            $datetime = $("<input type='text' style='height:20px' />")
                .data("datetime-id", _pickedColumnDef.id + "_" + title)
                .bind("change", { "columnInfo": _pickedColumnDef, "operator": operator }, handleFilterColumnDatetime)
                .prependTo($label)
                .appendTo($li)
                .datepicker();
        }

        //响应日期输入框的change事件
        function handleFilterColumnDatetime(e) {
            var filterValue = this.value;

            handleSingleColumnFilter(e.data.columnInfo.field, e.data.columnInfo.fieldType, e.data.operator, filterValue);

            hidePopMenu();
            hideMenu();
        }

        //处理YesOrNo类型的字段过滤
        function appendBooleanInput(popMenu, title, operator) {
            $radio1 = $("<input type='radio' name='filterradio' value='1' />")
                .bind("change", { "columnInfo": _pickedColumnDef, "operator": "=" }, handleFilterRadio)
                .appendTo($popMenu);
            $label1 = $("<label />")
                .text("是")
                .prependTo($radio1)
                .appendTo($popMenu);

            $radio2 = $("<input type='radio' name='filterradio' value='0' />")
                .bind("change", { "columnInfo": _pickedColumnDef, "operator": "=" }, handleFilterRadio)
                .prependTo($radio1)
                .appendTo($popMenu);
            $label2 = $("<label />")
                .text("否")
                .prependTo($radio1)
                .appendTo($popMenu);
        }

        //响应RadioButton的change事件
        function handleFilterRadio(e) {
            var filterValue = this.value == 1 ? 'true' : 'false';

            handleSingleColumnFilter(e.data.columnInfo.field, e.data.columnInfo.fieldType, e.data.operator, filterValue);

            hidePopMenu();
            hideMenu();
        }

        //处理下拉框类型的字段过滤
        function appendDropdownlistInput(popMenu, title, operator) {
            $li = $("<li />").appendTo($popMenu);
            $label = $("<label />")
                .text(operator)
                .appendTo($li);

            var a = _pickedColumnDef;
            var dataSource = _pickedColumnDef.dataSource;
            var option_str = "";
            var preSelect = "<SELECT tabIndex='0' id='slt_" + _pickedColumnDef.field + "' class='editor-select' style='width:80px;height:20px;margin-left:5px'><OPTION value='-1'>...</OPTION>";
            var endSelect = "</SELECT>";
            for (var i = 0, len = dataSource.length; i < len; i++) {
                option_str += "<OPTION value='" + dataSource[i].ID + "'>" + dataSource[i].Text + "</OPTION>";
            }
            var list = preSelect + option_str + endSelect;

            $(list).prependTo($label)
                .bind("change", { "columnInfo": _pickedColumnDef, "operator": operator }, handleFilterColumnDropdownlist)
                .appendTo($li);
        }

        //响应dropdownlist过滤器上的过滤事件
        function handleFilterColumnDropdownlist(e) {
            var filterValue = this.value;

            handleSingleColumnFilter(e.data.columnInfo.field, e.data.columnInfo.fieldType, e.data.operator, filterValue);

            hidePopMenu();
            hideMenu();
        }

        //处理单列过滤
        function handleSingleColumnFilter(field, fieldType, operator, filterValue) {
            var expression = generateExpression(field, fieldType, operator, filterValue);

            var filterArgs = {
                "field": field,
                "fieldType": fieldType,
                "filterType": "single",
                "expression": expression
            };

            dataview.setFilterArgs(filterArgs);
            dataview.refresh();
        }

        //生成表达式
        function generateExpression(fieldName, fieldType, operator, filterValue) {
            var expression;
            fieldName = "[" + fieldName + "]";
            operator = operator == "=" ? "==" : operator;
            if (fieldType == "string")
                expression = fieldName + ".indexOf(\'" + filterValue + "\')!=-1";
            else if (fieldType == "number") {
                if (!filterValue)
                    filterValue = 0;
                expression = fieldName + operator + filterValue;
            }
            else if (fieldType == "boolean")
                expression = fieldName + operator + parseBool(filterValue);
            else if (fieldType == "datetime")
                expression = fieldName + operator + Date.parse(filterValue);
            else if (fieldType == "dropdownlist")
                expression = fieldName + operator + filterValue;
            else
                throw new Error("未知的数据类型: " + fieldType);

            return expression;
        }

        //清除过滤条件的操作
        function clearSingleColumnFilter(field, fieldType) {
            var filterArgs = {
                "field": field,
                "fieldType": fieldType,
                "filterType": "clear",
                "expression": ''
            };

            dataview.setFilterArgs(filterArgs);
            dataview.refresh();
        }
        //#endregion

        function trigger(evt, args, e) {
            e = e || new Slick.EventData();
            args = args || {};
            args.grid = self;
            return evt.notify(args, e, self);
        }

        init();

        $.extend(this, {
            "onDataColumnSort": new Slick.Event()
        });
    }
    //#endregion

    //#region 过滤器对话框
    function SlickFilterDialog() {
        var $logicMenu;
        var $fieldNameMenu;
        var $operatorMenu;

        var $currentLogicLink;
        var $currentFieldNameLink;
        var $currentOperatorLink;

        var _divLogicExpressionPrefix = "divLogicExpression";
        var _divFilterContentPrefix = "divFilterContent";
        var _divFilterExpressionPrefix = "divFilterExpression";
        var _topLogicIndex;

        var logicMenuItems = [
            { action: "and", text: "并且", iconImage: "/Common/Content/default/js/slickgrid/images/sort_ascend.png" },
            { action: "or", text: "或者", iconImage: "/Common/Content/default/js/slickgrid/images/sort_descend.png" },
            { action: "addCondition", text: "添加条件", newHorLine: true },
            { action: "addGroup", text: "添加分组", iconImage: "/Common/Content/default/js/slickgrid/images/column-chooser.gif" },
            { action: "removeGroup", text: "清除分组", newHorLine: true, iconImage: "/Common/Content/default/js/slickgrid/images/column-format.png" },
        ];

        //从window对象读取列定义
        var _filterSource = window.getFilterSource();
        if (!_filterSource) {
            throw ("请查看SlickGrid页面有没有定义getFilterSource()方法！");
        }

        var _grid = _filterSource.grid;
        var _dataview = _filterSource.dataview;
        var colInfo = Slick.Data.SlickColumnDefSimplify(_grid.getColumns());

        var operatorArray = [
            { "id": "greater", "title": "大于", "text": ">" },
            { "id": "equal", "title": "等于", "text": "=" },
            { "id": "less", "title": "小于", "text": "<" },
            { "id": "not-equal", "title": "不等于", "text": "!=" },
            { "id": "greater-equal", "title": "大于或等于", "text": ">=" },
            { "id": "less-equal", "title": "小于或等于", "text": "<=" }
            //{ "id": "like", "title": "模糊匹配", "text": "like" }
        ];

        //========================================================
        //样式部分
        //========================================================
        //filter对话框初始化
        function init() {
            //绑定事件
            $(document.body).bind("mousedown", handleBodyMouseDown);

            //初始化菜单
            initPopMenus();

            var $topLogicExpression = createTopLogicExpression();
            prepareLogicBlock($topLogicExpression, _topLogicIndex);

            //默认显示第一条记录的过滤条件输入框
            createFirstFilterItem(_topLogicIndex, colInfo[0]);
        }

        function createTopLogicExpression() {
            var index = Math.random() * 1000000;
            index = Math.floor(index);

            _topLogicIndex = index;
            var divId = _divLogicExpressionPrefix + "-" + index;

            var $topLogicExpression = $("<div id='" + divId + "' exptype='logicGroupExp' headindex='" + index + "'></div>")
                .appendTo($("#divDialogContent"));

            return $topLogicExpression;
        }

        //初始化3个弹出菜单窗口
        function initPopMenus() {
            $logicMenu = initLogicMenu($("#divLogicPopMenu"));
            $fieldMenu = initFieldMenu($("#divFieldPopMenu"));
            $operatorMenu = initOperatorMenu($("#divOperatorPopMenu"));
        }

        //生成逻辑菜单栏目项
        function initLogicMenu(menuContainer) {
            //先清空菜单项内容
            if ($(menuContainer))
                $(menuContainer).empty();

            for (var i = 0; i < logicMenuItems.length; i++) {
                var item = logicMenuItems[i];

                if (!item) {
                    continue;
                }

                //line break
                if (item.newHorLine) {
                    $("<hr />").appendTo($(menuContainer));
                }

                //item content
                var $li = $("<div class='slick-column-menuitem' id='divPopMenuItem1001'></div>")
                    .attr("title", item.text)
                    .appendTo($(menuContainer));

                //icon
                var $icon = $("<div class='slick-column-menuicon'></div>")
                    .appendTo($li);
                if (item.iconImage) {
                    $icon.css("background-image", "url(" + item.iconImage + ")")
                }

                //span
                var $span = $("<span class='slick-column-menucontent'></span>")
                    .text(item.text)
                    .appendTo($li);

                //bind event
                $li
                    .bind("click", item, handleLogicMenuClick);
            }
            return $(menuContainer);
        }

        //初始化列标题菜单
        function initFieldMenu(menuContainer) {
            if ($(menuContainer))
                $(menuContainer).empty();

            for (var i = 0; i < colInfo.length; i++) {
                var col = colInfo[i];

                var $li = $("<div class='slick-column-menuitem' ></div>")
                    .attr("title", col.name)
                    .appendTo($(menuContainer));

                //span
                var $span = $("<span class='slick-column-menucontent'></span>")
                    .text(col.name)
                    .appendTo($li);

                //bind event
                $li
                    .bind("click", col, handleFieldMenuItemClick);
            }

            return $(menuContainer);
        }

        //初始化运算符菜单
        function initOperatorMenu(menuContainer) {
            if ($(menuContainer))
                $(menuContainer).empty();

            for (var i = 0; i < operatorArray.length; i++) {
                var item = operatorArray[i];

                var $li = $("<div class='slick-column-menuitem'></div>")
                    .attr("title", item.title)
                    .appendTo($(menuContainer));

                var span = $("<span class='slick-column-menucontent'></span>")
                    .text(item.text)
                    .appendTo($li);

                $li.bind("click", item, handleOperatorMenuItemClick);
            }
            return $(menuContainer);
        }

        function prepareLogicBlock(logicContainer, headindex) {
            $ahreflogiclink = $("<a href='#' class='hrefAndOrItem' exptype='logicOperatorExp' headindex='" + headindex + "'>and</a>")
                   .bind("click", onLogicActionClick)
                   .appendTo(logicContainer);

            //add image
            $imgLogicItemNew = $("<img src='/Common/Content/default/js/slickgrid/images/insert-new.png' class='hrefImageLogicItemNew'/>")
                .bind("click", { "headindex": headindex }, onLogicImageNewClick)
                .appendTo(logicContainer);

            //filter content
            var contentId = _divFilterContentPrefix;
            if (headindex != 0) {
                contentId = contentId + "-" + headindex;
            }
            $filterContentNew = $("<div id='" + contentId + "' exptype='expressionContent' headindex='" + headindex + "'></div>")
                .appendTo(logicContainer);
        }

        function createFirstFilterGroup() {
            var index = Math.random() * 1000000;
            index = Math.floor(index);

            //定位divFilterContent
            var divFilterContent = $currentLogicLink.parent().children("[exptype='expressionContent']")[0];
            var logicId = _divLogicExpressionPrefix + "-" + index;
            var divLogic = $("<div id='" + logicId + "' style='float:left;' exptype='logicGroupExp' headindex='" + index + "'></div>")
                .appendTo($(divFilterContent));

            prepareLogicBlock(divLogic, index);
            createFirstFilterItem(index, colInfo[0]);
        }

        //按照列标题来创建过滤字读的输入框，包括：单个字段、运算符和过滤值的输入项
        function createFirstFilterItem(headindex, column, lastNode) {
            var expressionindex = Math.random() * 1000000;
            expressionindex = Math.floor(expressionindex);

            var contentId = _divFilterContentPrefix;
            if (headindex != "0") {
                contentId = _divFilterContentPrefix + "-" + headindex;
            }

            var filterContent = $("#" + contentId);
            var divId = _divFilterExpressionPrefix + "-" + expressionindex;
            $filterBlock = $("<div id='" + divId + "' style='width:400px;' class='cssFilterExpression' exptype='expression' headindex='" + headindex + "' expressionindex='" + expressionindex + "'></div>");

            //插入节点
            if (lastNode != undefined && lastNode != null) {
                $filterBlock.insertAfter(lastNode);
            }
            else {
                $filterBlock.appendTo(filterContent);
            }

            //字段列标题
            $fieldNameDiv = $("<div style='width:110px;float:left;'></div>")
                .appendTo($filterBlock);

            $fieldNameLink = $("<a href='#' id='ahrefFieldName' exptype='parameterExp' style='margin-left:20px;margin-right:20px;'"
                + " field='" + column.field
                + "' fieldtype='" + column.fieldType
                + "' headindex='" + headindex
                + "' expressionindex='" + expressionindex
                + "'>" + column.name + "</a>")
                .bind('click', onfieldNameLinkClick)
                .appendTo($fieldNameDiv);

            //运算符
            $operatorDiv = $("<div style='width:30px;height:20px;float:left;'></div>")
                .appendTo($filterBlock);

            $operatorLink = $("<a href='#' id='ahrefOperator' exptype='binaryExp' style='margin-right:20px'>=</a>")
                .bind('click', onOperatorLinkClick)
                .appendTo($operatorDiv);

            //过滤值输入列
            $filterValueDiv = $("<div style='width:230px;float:left;'></div>")
               .appendTo($filterBlock);

            var fieldType = column.fieldType;
            if (fieldType == "datetime") {
                $filterValue = $("<input type='text' exptype='constantExp' style='width:200px'/>")
                    .data("datetime-id", headindex)
                    .appendTo($filterValueDiv)
                    .datepicker();
            }
            else if (fieldType == "boolean") {
                $filterValue = $("<input type='checkbox' exptype='constantExp'/>")
                    .appendTo($filterValueDiv);
            }
            else if (fieldType == "dropdownlist") {
                var list = createDropdownlist(column.field);
                $filterValue = $(list)
                    .appendTo($filterValueDiv);
            }
            else {
                $filterValue = $("<input type='text' exptype='constantExp' style='width:200px'/>")
                    .appendTo($filterValueDiv);
            }

            //删除按钮的图片div
            var $divImage = $("<div style='vertical-align: middle;text-align: center;'></div>")
                .appendTo($filterBlock);

            $imgFilterItemDel = $("<img src='/Common/Content/default/js/slickgrid/images/insert-del-grey.png' class='hrefImageFilterItemDel' style='display: block; margin-top: 5px;'/>")
                .bind("click", { "expressionindex": expressionindex }, onFilterItemDelClick)
                .appendTo($divImage);
        }

        //生成列类型为dropdownlist
        function createDropdownlist(field) {
            var option_str = "";
            var preSelect = "<SELECT class='editor-select' exptype='constantExp' style='width:180px;height:20px;margin-left:5px'><OPTION value='-1'>...</OPTION>";
            var endSelect = "</SELECT>";
            var dataSource = Slick.Data.SlickColumnDataSource(_grid.getColumns(), field);
            for (var i = 0, len = dataSource.length; i < len; i++) {
                option_str += "<OPTION value='" + dataSource[i].ID + "'>" + dataSource[i].Text + "</OPTION>";
            }
            var list = preSelect + option_str + endSelect;

            return list;
        }

        //弹出逻辑运算符菜单
        function onLogicActionClick(e) {
            $currentLogicLink = $(this);

            $logicMenu
                .css("top", e.pageY - 195)
                .css("left", e.pageX - 300)
                .fadeIn(150);
        }

        //弹出列标题菜单
        function onfieldNameLinkClick(e) {
            $currentFieldNameLink = $(this);
            $fieldMenu
                .css("top", e.pageY - 195)
                .css("left", e.pageX - 300)
                .fadeIn(150);
        }

        //弹出运算符菜单
        function onOperatorLinkClick(e) {
            $currentOperatorLink = $(this);

            $operatorMenu
                .css("top", e.pageY  - 195)
                .css("left", e.pageX - 300)
                .fadeIn(150);
        }

        //处理逻辑运算andor菜单
        function handleLogicMenuClick(e) {
            if (e.data.action == "and" || e.data.action == "or") {
                $currentLogicLink.text(e.data.action);
            }
            else if (e.data.action == "addCondition") {
                var headindex = $currentLogicLink.parent().attr("headindex");
                var contentDiv = $("#" + _divFilterContentPrefix + "-" + headindex);
                var lastNode = contentDiv.children("[exptype='expression']").last();

                createFirstFilterItem(headindex, colInfo[0], lastNode);
            }
            else if (e.data.action == "addGroup") {
                createFirstFilterGroup();
            }
            else if (e.data.action == "removeGroup") {
                $(".cssFilterExpression").empty();
            }
            hideLogicMenu();
        }

        //单击列标题菜单项事件
        function handleFieldMenuItemClick(e) {
            //执行删除和添加操作
            var headindex = $currentFieldNameLink.attr("headindex");
            var expressionindex = $currentFieldNameLink.attr("expressionindex");
            var filterItem = $("#" + _divFilterExpressionPrefix + "-" + expressionindex);

            createFirstFilterItem(headindex, e.data, filterItem);

            filterItem.remove();

            hideFieldMenu();
        }

        //单击运算符菜单项事件
        function handleOperatorMenuItemClick(e) {
            $currentOperatorLink.text(e.data.text);
            hideOperatorMenu();
        }

        //单击图片，新增条件表达式
        function onLogicImageNewClick(e) {
            var headindex = e.data.headindex;
            var contentDiv = $("#" + _divFilterContentPrefix + "-" + headindex);
            var lastNode = contentDiv.children("[exptype='expression']").last();

            createFirstFilterItem(headindex, colInfo[0], lastNode);
        }

        //删除一条过滤项
        function onFilterItemDelClick(e) {
            var index = e.data.expressionindex;
            var filterItem = $("#" + _divFilterExpressionPrefix + "-" + index);

            filterItem.remove();
        }

        //处理菜单隐藏
        function handleBodyMouseDown(e) {
            if ($logicMenu && $logicMenu[0] != e.target && !$.contains($logicMenu[0], e.target)) {
                hideLogicMenu();
            }

            if ($fieldMenu && $fieldMenu[0] != e.target && !$.contains($fieldMenu[0], e.target)) {
                hideFieldMenu();
            }

            if ($operatorMenu && $operatorMenu[0] != e.target && !$.contains($operatorMenu[0], e.target)) {
                hideOperatorMenu();
            }
        }

        //隐藏逻辑菜单
        function hideLogicMenu() {
            if ($logicMenu) {
                $logicMenu.hide();
            }
        }

        //隐藏列标题菜单
        function hideFieldMenu() {
            if ($fieldMenu) {
                $fieldMenu.hide();
            }
        }

        //隐藏运算符菜单
        function hideOperatorMenu() {
            if ($operatorMenu) {
                $operatorMenu.hide();
            }
        }

        function trigger(evt, args, e) {
            e = e || new Slick.EventData();
            args = args || {};
            args.grid = self;
            return evt.notify(args, e, self);
        }

        //=============================================
        //数据表达式解析部分
        //=============================================
        //表达式生成方法
        var expressionTree = {};

        function buildExpressionFunc() {
            //过滤表达式的字符串生成
            var root = $("#" + _divLogicExpressionPrefix + "-" + _topLogicIndex);
            var groupExpression = parseExpressionGroup(root);
            var filterArgs = {
                "field": colInfo,
                "fieldType": "all",
                "filterType": "complex",
                "colRangeType": "multiplecolumn",
                "expression": groupExpression
            };
            //alert(groupExpression);
            _dataview.setFilterArgs(filterArgs);
            _dataview.refresh();

            var dialogContainerId = "divFilterDialog_" + _grid.getGridId();
            window.$('#' + dialogContainerId).dialog('close');
        }

        //解析表达式group
        function parseExpressionGroup(parent) {
            var logicOperatorNode = parent.children("[exptype='logicOperatorExp']")[0];
            var logicOperator = $(logicOperatorNode).text();

            var contentNode = parent.children("[exptype='expressionContent']")[0];
            var groupExpression = parseExpressionContent(logicOperator, $(contentNode));

            return groupExpression;
        }

        //解析表达式content，包含表达式及嵌套group
        function parseExpressionContent(logicOperator, contentNode) {
            var logOp;
            if (logicOperator == 'and')
                logOp = " && ";
            else if (logicOperator == 'or')
                logOp = " || ";

            var expression = '';
            var expressionContent = '';

            //expressionContent节点包括表达式和逻辑组两种类型节点
            var contentElements = contentNode.children("[exptype]");
            for (var i = 0; i < contentElements.length; i++) {
                var item = $(contentElements[i]);
                var itemType = item.attr("exptype");
                if (itemType == "expression") {
                    expression = parseExpression(item);
                }
                else if (itemType == "logicGroupExp") {
                    expression = parseExpressionGroup(item);
                }

                if (expressionContent != '')
                    expressionContent += logOp;

                //生成表达式语句
                expressionContent += expression;
            }

            //逻辑运算符优先级处理
            if (logicOperator == 'or') {
                expressionContent = "(" + expressionContent + ")";
            }
            return expressionContent;
        }

        //解析条件表达式
        function parseExpression(item) {
            var expression = '';
            var parameterNode = $("div > a[exptype='parameterExp']", item);
            var fieldName = $(parameterNode[0]).attr("field");
            var fieldType = $(parameterNode[0]).attr("fieldtype");

            //列名称表达式
            var parameterExpression = "[" + fieldName + "]";

            //二元运算符
            var binaryNode = $("div > a[exptype='binaryExp']", item);
            var binaryNodeText = binaryNode[0].innerText ? binaryNode[0].innerText : binaryNode[0].text;
            var binaryExpression = getBinaryExpression(binaryNodeText);

            //过滤的数值表达式
            var filterValue = getFilterValue(item, fieldType);
            expression = mergeExpression(fieldType, parameterExpression, binaryExpression, filterValue);
      
            return expression;
        }

        //获取二元运算符
        function getBinaryExpression(nodeText) {
            var operator = nodeText == "=" ? "==" : nodeText;
            return operator;
        }

        //获取过滤数值
        function getFilterValue(item, fieldType) {
            var filterValue;
            var constantNode;
            if (fieldType == "dropdownlist") {
                constantNode = $("div > select[exptype='constantExp']", item);
                filterValue = $(constantNode[0]).val();
                if (filterValue == "-1")
                    filterValue = null;
            }
            else if (fieldType == "boolean") {
                constantNode = $("div > input[exptype='constantExp']", item);
                filterValue = constantNode[0].checked;
            }
            else {
                constantNode = $("div > input[exptype='constantExp']", item);
                filterValue = constantNode[0].value;
                if (filterValue == "")
                    filterValue = null;
            }
            return filterValue;
        }

        //合并表达式
        function mergeExpression(fieldType, parameterExpression, binaryExpression, filterValue) {
            var constantExpression;
            var expression = parameterExpression;

            //添加二元运算符
            if (!(fieldType == "string" && binaryExpression == "==")) {
                expression += binaryExpression;
            }

            //添加常量表达式
            if (fieldType == "string") {
                if (binaryExpression == "==")
                    constantExpression = ".indexOf(\'" + filterValue + "\') != -1";
                else {
                    constantExpression = "\'" + filterValue + "\'";
                }
            }
            else if (fieldType == "number") {
                constantExpression = filterValue;
            }
            else if (fieldType == "boolean") {
                constantExpression = Boolean(filterValue);
            }
            else if (fieldType == "datetime") {
                constantExpression = Date.parse(filterValue);
            }
            else if (fieldType == "dropdownlist") {
                constantExpression = filterValue;
            }
            else {
                throw new Error("未知的数据类型: " + fieldType);
            }
            expression += constantExpression;

            return expression;
        }

        function closeWindowFunc() {
            var dialogContainerId = "divFilterDialog_" + _grid.getGridId();
            window.$('#' + dialogContainerId).dialog('close');
        }

        init();

        $.extend(this, {
            "buildExpression": buildExpressionFunc,
            "closeWindow": closeWindowFunc
        });
    }
    //#endregion

})(jQuery);