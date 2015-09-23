/// <reference path="../complex.js" />
// make sure required JavaScript modules are loaded
if (typeof jQuery === "undefined") {
    throw "SlickGrid requires jquery module to be loaded";
}
if (!jQuery.fn.drag) {
    throw "SlickGrid requires jquery.event.drag module to be loaded";
}

//#region Slick.Core JS
/***
 * Contains core SlickGrid classes.
 * @module Core
 * @namespace Slick
 */

(function ($) {
    // register namespace
    $.extend(true, window, {
        "Slick": {
            "Event": Event,
            "EventData": EventData,
            "EventHandler": EventHandler,
            "Range": Range,
            "NonDataRow": NonDataItem,
            "Group": Group,
            "GroupTotals": GroupTotals,
            "EditorLock": EditorLock,

            /***
             * A global singleton editor lock.
             * @class GlobalEditorLock
             * @static
             * @constructor
             */
            "GlobalEditorLock": new EditorLock()
        }
    });

    /***
     * An event object for passing data to event handlers and letting them control propagation.
     * <p>This is pretty much identical to how W3C and jQuery implement events.</p>
     * @class EventData
     * @constructor
     */
    function EventData() {
        var isPropagationStopped = false;
        var isImmediatePropagationStopped = false;

        /***
         * Stops event from propagating up the DOM tree.
         * @method stopPropagation
         */
        this.stopPropagation = function () {
            isPropagationStopped = true;
        };

        /***
         * Returns whether stopPropagation was called on this event object.
         * @method isPropagationStopped
         * @return {Boolean}
         */
        this.isPropagationStopped = function () {
            return isPropagationStopped;
        };

        /***
         * Prevents the rest of the handlers from being executed.
         * @method stopImmediatePropagation
         */
        this.stopImmediatePropagation = function () {
            isImmediatePropagationStopped = true;
        };

        /***
         * Returns whether stopImmediatePropagation was called on this event object.\
         * @method isImmediatePropagationStopped
         * @return {Boolean}
         */
        this.isImmediatePropagationStopped = function () {
            return isImmediatePropagationStopped;
        }
    }

    /***
     * A simple publisher-subscriber implementation.
     * @class Event
     * @constructor
     */
    function Event() {
        var handlers = [];

        /***
       * Check an event handler is already registered.
       * @param fn {Function} Event handler.
       */
        this.isExist = function (fn) {
            for (var i = handlers.length - 1; i >= 0; i--) {
                if ("" + handlers[i] === "" + fn) { //compare function name
                    return true;
                }
            }
            return false;
        }

        /***
         * Adds an event handler to be called when the event is fired.
         * <p>Event handler will receive two arguments - an <code>EventData</code> and the <code>data</code>
         * object the event was fired with.<p>
         * @method subscribe
         * @param fn {Function} Event handler.
         */
        this.subscribe = function (fn) {
            var b = this.isExist(fn);
            if (b == false) {
                handlers.push(fn);
            }
        };

        /***
         * Removes an event handler added with <code>subscribe(fn)</code>.
         * @method unsubscribe
         * @param fn {Function} Event handler to be removed.
         */
        this.unsubscribe = function (fn) {
            for (var i = handlers.length - 1; i >= 0; i--) {
                if (handlers[i] === fn) {
                    handlers.splice(i, 1);
                }
            }
        };

        /***
         * Fires an event notifying all subscribers.
         * @method notify
         * @param args {Object} Additional data object to be passed to all handlers.
         * @param e {EventData}
         *      Optional.
         *      An <code>EventData</code> object to be passed to all handlers.
         *      For DOM events, an existing W3C/jQuery event object can be passed in.
         * @param scope {Object}
         *      Optional.
         *      The scope ("this") within which the handler will be executed.
         *      If not specified, the scope will be set to the <code>Event</code> instance.
         */
        this.notify = function (args, e, scope) {
            e = e || new EventData();
            scope = scope || this;

            var returnValue;
            for (var i = 0; i < handlers.length && !(e.isPropagationStopped() || e.isImmediatePropagationStopped()) ; i++) {
                returnValue = handlers[i].call(scope, e, args);
            }

            return returnValue;
        };
    }

    function EventHandler() {
        var handlers = [];

        this.subscribe = function (event, handler) {
            handlers.push({
                event: event,
                handler: handler
            });
            event.subscribe(handler);

            return this;  // allow chaining
        };

        this.unsubscribe = function (event, handler) {
            var i = handlers.length;
            while (i--) {
                if (handlers[i].event === event &&
                    handlers[i].handler === handler) {
                    handlers.splice(i, 1);
                    event.unsubscribe(handler);
                    return;
                }
            }

            return this;  // allow chaining
        };

        this.unsubscribeAll = function () {
            var i = handlers.length;
            while (i--) {
                handlers[i].event.unsubscribe(handlers[i].handler);
            }
            handlers = [];

            return this;  // allow chaining
        }
    }

    /***
     * A structure containing a range of cells.
     * @class Range
     * @constructor
     * @param fromRow {Integer} Starting row.
     * @param fromCell {Integer} Starting cell.
     * @param toRow {Integer} Optional. Ending row. Defaults to <code>fromRow</code>.
     * @param toCell {Integer} Optional. Ending cell. Defaults to <code>fromCell</code>.
     */
    function Range(fromRow, fromCell, toRow, toCell) {
        if (toRow === undefined && toCell === undefined) {
            toRow = fromRow;
            toCell = fromCell;
        }

        /***
         * @property fromRow
         * @type {Integer}
         */
        this.fromRow = Math.min(fromRow, toRow);

        /***
         * @property fromCell
         * @type {Integer}
         */
        this.fromCell = Math.min(fromCell, toCell);

        /***
         * @property toRow
         * @type {Integer}
         */
        this.toRow = Math.max(fromRow, toRow);

        /***
         * @property toCell
         * @type {Integer}
         */
        this.toCell = Math.max(fromCell, toCell);

        /***
         * Returns whether a range represents a single row.
         * @method isSingleRow
         * @return {Boolean}
         */
        this.isSingleRow = function () {
            return this.fromRow == this.toRow;
        };

        /***
         * Returns whether a range represents a single cell.
         * @method isSingleCell
         * @return {Boolean}
         */
        this.isSingleCell = function () {
            return this.fromRow == this.toRow && this.fromCell == this.toCell;
        };

        /***
         * Returns whether a range contains a given cell.
         * @method contains
         * @param row {Integer}
         * @param cell {Integer}
         * @return {Boolean}
         */
        this.contains = function (row, cell) {
            return row >= this.fromRow && row <= this.toRow &&
                cell >= this.fromCell && cell <= this.toCell;
        };

        /***
         * Returns a readable representation of a range.
         * @method toString
         * @return {String}
         */
        this.toString = function () {
            if (this.isSingleCell()) {
                return "(" + this.fromRow + ":" + this.fromCell + ")";
            }
            else {
                return "(" + this.fromRow + ":" + this.fromCell + " - " + this.toRow + ":" + this.toCell + ")";
            }
        }
    }


    /***
     * A base class that all special / non-data rows (like Group and GroupTotals) derive from.
     * @class NonDataItem
     * @constructor
     */
    function NonDataItem() {
        this.__nonDataRow = true;
    }


    /***
     * Information about a group of rows.
     * @class Group
     * @extends Slick.NonDataItem
     * @constructor
     */
    function Group() {
        this.__group = true;
        this.__updated = false;

        /***
         * Number of rows in the group.
         * @property count
         * @type {Integer}
         */
        this.count = 0;

        /***
         * Grouping value.
         * @property value
         * @type {Object}
         */
        this.value = null;

        /***
         * Formatted display value of the group.
         * @property title
         * @type {String}
         */
        this.title = null;

        /***
         * Whether a group is collapsed.
         * @property collapsed
         * @type {Boolean}
         */
        this.collapsed = false;

        /***
         * GroupTotals, if any.
         * @property totals
         * @type {GroupTotals}
         */
        this.totals = null;
    }

    Group.prototype = new NonDataItem();

    /***
     * Compares two Group instances.
     * @method equals
     * @return {Boolean}
     * @param group {Group} Group instance to compare to.
     */
    Group.prototype.equals = function (group) {
        return this.value === group.value &&
            this.count === group.count &&
            this.collapsed === group.collapsed;
    };

    /***
     * Information about group totals.
     * An instance of GroupTotals will be created for each totals row and passed to the aggregators
     * so that they can store arbitrary data in it.  That data can later be accessed by group totals
     * formatters during the display.
     * @class GroupTotals
     * @extends Slick.NonDataItem
     * @constructor
     */
    function GroupTotals() {
        this.__groupTotals = true;

        /***
         * Parent Group.
         * @param group
         * @type {Group}
         */
        this.group = null;
    }

    GroupTotals.prototype = new NonDataItem();

    /***
     * A locking helper to track the active edit controller and ensure that only a single controller
     * can be active at a time.  This prevents a whole class of state and validation synchronization
     * issues.  An edit controller (such as SlickGrid) can query if an active edit is in progress
     * and attempt a commit or cancel before proceeding.
     * @class EditorLock
     * @constructor
     */
    function EditorLock() {
        var activeEditController = null;

        /***
         * Returns true if a specified edit controller is active (has the edit lock).
         * If the parameter is not specified, returns true if any edit controller is active.
         * @method isActive
         * @param editController {EditController}
         * @return {Boolean}
         */
        this.isActive = function (editController) {
            return (editController ? activeEditController === editController : activeEditController !== null);
        };

        /***
         * Sets the specified edit controller as the active edit controller (acquire edit lock).
         * If another edit controller is already active, and exception will be thrown.
         * @method activate
         * @param editController {EditController} edit controller acquiring the lock
         */
        this.activate = function (editController) {
            if (editController === activeEditController) { // already activated?
                return;
            }
            if (activeEditController !== null) {
                throw "SlickGrid.EditorLock.activate: an editController is still active, can't activate another editController";
            }
            if (!editController.commitCurrentEdit) {
                throw "SlickGrid.EditorLock.activate: editController must implement .commitCurrentEdit()";
            }
            if (!editController.cancelCurrentEdit) {
                throw "SlickGrid.EditorLock.activate: editController must implement .cancelCurrentEdit()";
            }
            activeEditController = editController;
        };

        /***
         * Unsets the specified edit controller as the active edit controller (release edit lock).
         * If the specified edit controller is not the active one, an exception will be thrown.
         * @method deactivate
         * @param editController {EditController} edit controller releasing the lock
         */
        this.deactivate = function (editController) {
            if (activeEditController !== editController) {
                throw "SlickGrid.EditorLock.deactivate: specified editController is not the currently active one";
            }
            activeEditController = null;
        };

        /***
         * Attempts to commit the current edit by calling "commitCurrentEdit" method on the active edit
         * controller and returns whether the commit attempt was successful (commit may fail due to validation
         * errors, etc.).  Edit controller's "commitCurrentEdit" must return true if the commit has succeeded
         * and false otherwise.  If no edit controller is active, returns true.
         * @method commitCurrentEdit
         * @return {Boolean}
         */
        this.commitCurrentEdit = function () {
            return (activeEditController ? activeEditController.commitCurrentEdit() : true);
        };

        /***
         * Attempts to cancel the current edit by calling "cancelCurrentEdit" method on the active edit
         * controller and returns whether the edit was successfully cancelled.  If no edit controller is
         * active, returns true.
         * @method cancelCurrentEdit
         * @return {Boolean}
         */
        this.cancelCurrentEdit = function cancelCurrentEdit() {
            return (activeEditController ? activeEditController.cancelCurrentEdit() : true);
        };
    }
})(jQuery);
//#endregion

/**
 * @license
 * (c) 2009-2012 Michael Leibman
 * michael{dot}leibman{at}gmail{dot}com
 * http://github.com/mleibman/slickgrid
 *
 * Distributed under MIT license.
 * All rights reserved.
 *
 * SlickGrid v2.1
 *
 * NOTES:
 *     Cell/row DOM manipulations are done directly bypassing jQuery's DOM manipulation methods.
 *     This increases the speed dramatically, but can only be done safely because there are no event handlers
 *     or data associated with any cell/row DOM nodes.  Cell editors must make sure they implement .destroy()
 *     and do proper cleanup.
 */

if (typeof Slick === "undefined") {
    throw "slick.core.js not loaded";
}

//#region Slick.Formatter JS
/***
 * Contains basic SlickGrid formatters.
 * @module Formatters
 * @namespace Slick
 */

(function ($) {
    // register namespace
    $.extend(true, window, {
        "Slick": {
            "Formatters": {
                "EnmuabledText": EnumabledTextFormatter,
                "PercentComplete": PercentCompleteFormatter,
                "PercentCompleteBar": PercentCompleteBarFormatter,
                "PercentStatusBar": PercentStatusBarFormatter,
                "YesNo": YesNoFormatter,
                "Checkmark": CheckmarkFormatter,
                "CheckmarkOread": CheckmarkOreadFormatter,
                "CheckBoxVisible": CheckBoxVisibleFormatter,
                "RowNumber": RowNumberFormatter,
                "DataTime": datetimeFormatter,
                "HyperLinkInTab": hyperLinkInTabFormatter,
                "HyperLinkNewPage": hyperLinkNewPageFormatter,
                "VerCellMerged": VerCellMergedFormatter
            }
        }
    });

    function EnumabledTextFormatter(row, cell, value, columnDef, dataContext) {
        if (value == null || value === "") {
            return "";
        }

        var emuabledTextArray = columnDef.enumabledTextArray;
        if (emuabledTextArray == null || emuabledTextArray.length == 0) {
            return "";
        }

        return emuabledTextArray[value - 1];
    }

    function PercentCompleteFormatter(row, cell, value, columnDef, dataContext) {
        if (value == null || value === "") {
            return "-";
        } else if (value < 50) {
            return "<span style='color:red;font-weight:bold;'>" + value + "%</span>";
        } else {
            return "<span style='color:green'>" + value + "%</span>";
        }
    }

    function PercentCompleteBarFormatter(row, cell, value, columnDef, dataContext) {
        if (value == null || value === "") {
            return "";
        }

        var color;

        if (value < 30) {
            color = "red";
        } else if (value < 70) {
            color = "silver";
        } else {
            color = "green";
        }

        return "<span class='percent-complete-bar' style='background:" + color + ";width:" + value + "%'></span>";
    }

    function PercentStatusBarFormatter(row, cell, value, columnDef, dataContext) {
        if (value == null || value === "") {
            return "";
        }

        var color;
        var pValue = (100 * value) / columnDef.maxStatus;

        if (pValue < 30) {
            color = "silver";
        } else if (pValue < 70) {
            color = "green";
        } else {
            color = "red";
        }

        return "<span class='percent-complete-bar' style='background:" + color + ";width:" + pValue + "%'></span>";
    }

    function YesNoFormatter(row, cell, value, columnDef, dataContext) {
        return value ? "Yes" : "No";
    }

    function CheckmarkFormatter(row, cell, value, columnDef, dataContext) {
        return value ? "<img src='/Common/Content/default/images/tick.png'>" : "";
    }
    function CheckmarkOreadFormatter(row, cell, value, columnDef, dataContext)
    {
        return value ? "<img src='/Common/Content/default/images/yes.png'>" : "<img src='/Common/Content/default/images/no.png'>";
    }
    function CheckBoxVisibleFormatter(row, cell, value, columnDef, dataContext) {
        var checkbox;

        if (value) {
            checkbox = "<INPUT type=checkbox checked='true' class='editor-checkbox' />";
        }
        else {
            checkbox = "<INPUT type=checkbox class='editor-checkbox' />";
        }
        return checkbox;
    }

    //#region Cell Merged Functions
    //1.) renderOptions in pages:
    //    var _renderOptions = {
    //  "lastRendering": 0,
    //  "isNextMerged": 0,
    //  "changedCells": {}
    //};

    //2.) datasrouce of rendering:
    //function getRenderDataItmes() {
    //    var grid = window.pwpProductList.getGridControl();
    //    var dataView = grid.getData();
    //    var items = dataView.getItems();

    //    return items;
    //}

    //3.) related methods in page javascript:         
    //rows changed
    //dataViewProduct.onRowsChanged.subscribe(function (e, args) {
    //    gridProduct.invalidateRows(args.rows);
    //    gridProduct.render();
    //    var changes = window._renderOptions.changedCells;       //generated from the formatter
    //    gridProduct.setCellCssStyles('cell-noneline-bottom', changes);
    //});

    //  // wire up model events to drive the grid
    //dataViewProduct.onRowCountChanged.subscribe(function (e, args) {
    //    gridProduct.updateRowCount();
    //    gridProduct.render();
    //    var options = window._renderOptions;
    //    options.lastRendering = 1;      //in the row count change render, we dont use formatter, the second, in rows changed, we did render staff.
    //});
    //#endregion

    //#region Vertical Cell Merged Function
    function VerCellMergedFormatter(row, cell, value, columnDef, dataContext) {
        var options = window._renderOptions;

        if (options.lastRendering != 1) {
            return;
        }

        var items = window.getRenderDataItmes();
        var fieldName = columnDef.field;
        var rowsLength = items.length;
        var currentItem = items[row];

        var nextRowIndex = row + 1;
        if (nextRowIndex < rowsLength) {
            var nextValue = items[nextRowIndex][fieldName];

            if (value == nextValue) {
                if (!options.changedCells[row]) {
                    options.changedCells[row] = {};
                }
                options.changedCells[row][fieldName] = "cell-noneline-bottom";
                options.isNextMerged = 1;

                if (row > 0) {
                    if (options.changedCells[row - 1] && options.changedCells[row - 1][fieldName]) {
                        options.mergedCells.addUnique(row);
                        options.previousRow = row;
                        return '';
                    }
                }
            }
            else {
                var idx = $.inArray(row, options.mergedCells);
                if (idx != -1) {
                    options.previousRow = row;
                    return '';
                } else {
                    if (options.isNextMerged == 1) {
                        options.isNextMerged = 0;
                        if (options.previousRow > row) {
                            ;
                        } else {
                            options.mergedCells.addUnique(row);
                            options.previousRow = row;
                            return '';
                        }
                    }
                }
            }
        }

        options.previousRow = row;
        return value;
    }
    //#endregion

    function hyperLinkInTabFormatter(row, cell, value, columnDef, dataContext) {
        var action = columnDef.linkAction;
        var actionName = action();
        var actionName = actionName + "(" + row + ");";
        var link = "<a href='#' style='text-decoration: underline;' onclick='" + actionName + "' rowIndex='" + row + "'>" + value + "</a>";
        return link;
    }

    function hyperLinkNewPageFormatter(row, cell, value, columnDef, dataContext) {
        var action = columnDef.linkAction;
        var queryStrings = action(dataContext);

        return "<a href='" + columnDef.linkUrl + queryStrings + "' style='text-decoration: underline;'>" + value + '</a>';
    }

    function RowNumberFormatter(row, cell, value, columnDef, dataContext) {
        return row + 1;
    }
    //时间格式处理
    function datetimeFormatter(row, cell, value, columnDef, dataContext) {
        if (value != null && value != "") {
            return value.substring(0, 10);
        }
    }
})(jQuery);
//#endregion

//#region Slick.Editor JS
/***
 * Contains basic SlickGrid editors.
 * @module Editors
 * @namespace Slick
 */

(function ($) {
    // register namespace
    $.extend(true, window, {
        "Slick": {
            "Editors": {
                "Text": TextEditor,
                "Integer": IntegerEditor,
                "Decimal": DecimalEditor,
                "Date": DateEditor,
                "DropDownList": DropDownList,
                "DropDownWithText": DropDownEditor,
                "YesNoSelect": YesNoSelectEditor,
                "Checkbox": CheckboxEditor,
                "CheckboxRefered": CheckboxReferedEditor,
                "TextButton": TextButtonEditor,
                "PercentComplete": PercentCompleteEditor,
                "LongText": LongTextEditor,
                "SelectCell": SelectCellEditor,
                "RowNumberSelector": RowNumberSelector
            }
        }
    });

    function isTextSelected(select_field) {
        //check txt length, if it is null
        var len = $(select_field).val().length;
        if (len == 0) {
            return true;
        }

        var selected = false;
        //IE  
        if (document.selection) {
            var sel = document.selection.createRange();
            if (sel.text.length > 0) {
                selected = true;
            }
        }
            //FF  
        else if (select_field.selectionStart || select_field.selectionStart == '0') {
            var startP = select_field.selectionStart;
            var endP = select_field.selectionEnd;
            if (startP != endP) {
                selected = true;
            }
        }
        return selected;
    }

    function TextEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $input = $("<INPUT type=text class='editor-text' />")
                .bind("dblclick", function (e) {
                    return args.grid.onDblClick.notify(args, e, this);
                })
                .bind("keydown.nav keypress mousemove", function (e) {
                    var isSelected = isTextSelected(this);

                    if (!isSelected) {
                        if (e.keyCode === $.ui.keyCode.LEFT && $(this).caret() != 0) {
                            e.stopImmediatePropagation();
                        } else if (e.keyCode === $.ui.keyCode.RIGHT && $(this).caret() != $(this).val().length) {
                            e.stopImmediatePropagation();
                        }
                    }
                })
                .appendTo(args.container)
                .focus()
                .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.val(defaultValue);
            $input[0].defaultValue = defaultValue;
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        //字段校验
        this.validate = function () {
            //必填字段的验证
            var validator = args.column.validator;
            if (validator && validator.required) {
                var value = $input.val();
                if (!value || value.length == 0) {
                    return { valid: false, msg: "字段:[" + args.column.name + "]不能为空！" };
                }
                else if (validator.maxlength) {
                    if (value.length > validator.maxlength) {
                        return { valid: false, msg: "字段:[" + args.column.name + "]长度超过最大输入长度:" + validator.maxlength };
                    }
                }
            }

            //字段外部检验器的验证
            if (args.column.validate) {
                var validationResults = args.column.validate($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function IntegerEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $input = $("<INPUT type=text class='editor-text' />")
                .bind("keydown.nav keypress mousemove", function (e) {
                    var isSelected = isTextSelected(this);

                    if (!isSelected) {
                        if (e.keyCode === $.ui.keyCode.LEFT && $(this).caret() != 0) {
                            e.stopImmediatePropagation();
                        } else if (e.keyCode === $.ui.keyCode.RIGHT && $(this).caret() != $(this).val().length) {
                            e.stopImmediatePropagation();
                        }
                    }
                })
                .appendTo(args.container)
                .focus()
                .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field];
            $input.val(defaultValue);
            $input[0].defaultValue = defaultValue;
            $input.select();
        };

        this.serializeValue = function () {
            return parseInt($input.val(), 10) || 0;
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (isNaN($input.val())) {
                return {
                    valid: false,
                    msg: "Please enter a valid integer"
                };
            }

            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function DecimalEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $input = $("<INPUT type=text class='editor-text' />")
                .bind("keydown.nav keypress mousemove", function (e) {
                    var isSelected = isTextSelected(this);

                    if (!isSelected) {
                        if (e.keyCode === $.ui.keyCode.LEFT && $(this).caret() != 0) {
                            e.stopImmediatePropagation();
                        } else if (e.keyCode === $.ui.keyCode.RIGHT && $(this).caret() != $(this).val().length) {
                            e.stopImmediatePropagation();
                        }
                    }
                })
                .appendTo(args.container)
                .focus()
                .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field];
            $input.val(defaultValue);
            $input[0].defaultValue = defaultValue;
            $input.select();
        };

        this.serializeValue = function () {
            return parseFloat($input.val(), 10) || 0;
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (isNaN($input.val())) {
                return {
                    valid: false,
                    msg: "Please enter a valid integer"
                };
            }

            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function DateEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;
        var calendarOpen = false;

        this.init = function () {
            $input = $("<INPUT type=text class='editor-text' />")
                .bind("keydown.nav keypress mousemove", function (e) {
                    var isSelected = isTextSelected(this);

                    if (!isSelected) {
                        if (e.keyCode === $.ui.keyCode.LEFT && $(this).caret() != 0) {
                            e.stopImmediatePropagation();
                        } else if (e.keyCode === $.ui.keyCode.RIGHT && $(this).caret() != $(this).val().length) {
                            e.stopImmediatePropagation();
                        }
                    }
                })
                .appendTo(args.container)
                .focus()
                .select();

            $input.datepicker({
                showOn: "button",
                buttonImageOnly: true,
                buttonImage: "/Common/Content/default/images/calendar.gif",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                beforeShow: function () {
                    calendarOpen = true;
                },
                onClose: function () {
                    calendarOpen = false;
                    $(this).focus();
                }
            });
            $input.width($input.width() - 18);
        };

        this.destroy = function () {
            $.datepicker.dpDiv.stop(true, true);
            $input.datepicker("hide");
            $input.datepicker("destroy");
            $input.remove();
        };

        this.show = function () {
            if (calendarOpen) {
                $.datepicker.dpDiv.stop(true, true).show();
            }
        };

        this.hide = function () {
            if (calendarOpen) {
                $.datepicker.dpDiv.stop(true, true).hide();
            }
        };

        this.position = function (position) {
            if (!calendarOpen) {
                return;
            }
            $.datepicker.dpDiv
                .css("top", position.top + 30)
                .css("left", position.left);
        };

        this.focus = function () {
            $input.focus();
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field];
            $input.val(defaultValue);
            $input[0].defaultValue = defaultValue;
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            var result;
            var str = $input.val();
            var isRight = Date.parse(str);

            if (isRight) {
                result = {
                    valid: true,
                    msg: null
                };
            } else {
                result = {
                    valid: false,
                    msg: "请输入正确的日期格式！"
                };
            }
            return result;
        };

        this.init();
    }

    function DropDownList(args) {
        var sid = "dz9a_" + Math.round(100000 * Math.random());
        var $select = $("");
        var defaultValue = "", isInvalidItem = false;
        var scope = this;
        var dataSouce = args.grid.$selDropdownlistDatasource;
        var changedEvent = args.grid.$selDropdownlistChangedEvent;

        this.init = function () {
            if (dataSouce != undefined && dataSouce != null) {
                $select = this.preRender(args.grid.$selDropdownlistDatasource);
                if (changedEvent != null) {
                    $select.change(function (e) {
                        changedEvent(this, { "ID": $select.val(), "txt": $select.find('option:selected').text() });
                    });
                }

                $select.click(function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                });

                $select.appendTo(args.container);
                $select.focus();
            }
        };

        this.preRender = function (dataSource) {
            var option_str = "";
            var preSelect = "<SELECT id='" + sid + "' tabIndex='0' class='editor-select'><OPTION value='-1'></OPTION>";
            var endSelect = "</SELECT>";

            var len = dataSource.length;
            if (len > 0 && dataSource[0].txt != undefined) {
                for (var i = 0; i < len; i++)
                    option_str += "<OPTION value='" + dataSource[i].ID + "'>" + dataSource[i].txt + "</OPTION>";
            }
            else {
                for (var i = 0; i < len; i++)
                    option_str += "<OPTION value='" + dataSource[i].ID + "'>" + dataSource[i].Text + "</OPTION>";
            }

            var list = preSelect + option_str + endSelect;
            return $(list);
        }

        this.destroy = function () {
            if ($(args.container).is('div') == true) {
                $(args.container).empty();
            }
        };

        this.focus = function () {
            $select.focus();
        };

        this.serializeValue = function () {
            if ($select.val() != -1) {
                return $select.find('option:selected').text();
            } else {
                if (isInvalidItem == true) {
                    return defaultValue;
                } else {
                    return '';
                }
            }
        };

        this.applyValue = function (item, state) {
            if (state != undefined) {
                item[args.column.field] = state;
            }
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field];
            $("#" + sid + " option")
                .filter(function () { return $.trim($(this).text()) == defaultValue; })
                .attr('selected', true);

            if ($select.val() == -1) {
                isInvalidItem = true;
            }
        };

        this.isValueChanged = function () {
            return ($select.val() != defaultValue);
        };

        this.validate = function () {
            return {
                valid: true,
                msg: null
            };
        };

        this.init();

        return $select.val();
    }

    function DropDownEditor(args) {
        var $select = $("");
        var defaultValue = "";
        var scope = this;
        var dataSource = args.grid.$selDropdownlistDatasource;
        var changedEvent = args.grid.$selDropdownlistChangedEvent;

        this.init = function () {
            if (dataSource != undefined && dataSource != null) {
                $select = this.preRender(dataSource);
                $select.appendTo(args.container);

                if (changedEvent != null) {
                    $select.editableSelect().change(function (e) {
                        var index = -1;
                        //get the current selected value, if the value is inputed, return -1.
                        var text = $select.data("editableSelect").editableSelect.$textbox.val();
                        var items = jQuery.grep(dataSource, function (x) {
                            return x.txt == text;
                        });
                        if (items && items.length == 1)
                            index = items[0].ID;
                        changedEvent(this, { "ID": index, "txt": text });
                    });
                } else {
                    throw Error("需要实现下拉框的change事件!");
                }

                this.disableEditor();

                if ($select.data("editableSelect") && $select.data("editableSelect").editableSelect.$textbox) {
                    $select.data("editableSelect").editableSelect.$textbox.focus();
                } else {
                    $select.focus();
                }
            }
        };

        //editable by diffrent rows
        this.disableEditor = function (input) {
            var uneditable = args.grid.$selDropdownlistUneditable;
            if (uneditable) {
                $select.data("editableSelect").editableSelect.$textbox.attr('readonly', 'readonly');
            }
            else {
                $select.data("editableSelect").editableSelect.$textbox.removeAttr('readonly');
            }
            args.grid.$selDropdownlistUneditable = false;
        }

        this.preRender = function (dataSource) {
            var option_str = "";
            var preSelect = "<SELECT tabIndex='0' class='makeEditable editor-select'><OPTION value='-1'></OPTION>";
            var endSelect = "</SELECT>";

            var len = dataSource.length;
            if (len > 0 && dataSource[0].txt != undefined) {
                for (var i = 0; i < len; i++)
                    option_str += "<OPTION value='" + dataSource[i].ID + "'>" + dataSource[i].txt + "</OPTION>";
            }
            else {
                for (var i = 0; i < len; i++)
                    option_str += "<OPTION value='" + dataSource[i].ID + "'>" + dataSource[i].Text + "</OPTION>";
            }

            var list = preSelect + option_str + endSelect;
            return $(list);
        }

        this.destroy = function () {
            if ($(args.container).is('div') == true) {
                $(args.container).empty();
            }
        }

        this.focus = function () {
            $select.focus();
        }

        this.serializeValue = function () {
            var text = $select.data("editableSelect").editableSelect.$textbox.val();
            if (text && text != "") {
                if (!(dataSource || dataSource[0].xdata)) {
                    return text;
                }

                //if display value is different with xdata value, the xdata value is true wanted.
                var xdata;
                var items = jQuery.grep(dataSource, function (x) {
                    return x.txt == text;
                });

                if (items && items.length == 1) {
                    xdata = items[0].xdata;
                }

                //the value is inputed, not exist in dropdownlist
                if (!xdata) {
                    return text;
                }

                return xdata;
            } else {
                return "";
            }
        }

        this.applyValue = function (item, state) {
            if (state != undefined) {
                item[args.column.field] = state;
            }
        }

        this.loadValue = function (item) {
            defaultValue = item[args.column.field];
            $select.data("editableSelect").editableSelect.$textbox.val(defaultValue);
        }

        this.isValueChanged = function () {
            var text = $select.data("editableSelect").editableSelect.$textbox.val();
            return (text != defaultValue);
        }

        this.validate = function () {
            return {
                valid: true,
                msg: null
            };
        }

        this.init();

        return $select.val();
    }

    function YesNoSelectEditor(args) {
        var $select;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $select = $("<SELECT tabIndex='0' class='editor-yesno'><OPTION value='yes'>Yes</OPTION><OPTION value='no'>No</OPTION></SELECT>");
            $select.appendTo(args.container);
            $select.focus();
        };

        this.destroy = function () {
            $select.remove();
        };

        this.focus = function () {
            $select.focus();
        };

        this.loadValue = function (item) {
            $select.val((defaultValue = item[args.column.field]) ? "yes" : "no");
            $select.select();
        };

        this.serializeValue = function () {
            return ($select.val() == "yes");
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return ($select.val() != defaultValue);
        };

        this.validate = function () {
            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function TextButtonEditor(args) {
        var $input;
        var $txtBox;
        var $button;
        var defaultValue;
        var scope = this;

        this.init = function () {
            var width = $(args.container).width() - 17;
            $input = $("<INPUT type='text' class='editor-text' style='width:" + width + "px'/><INPUT type='button' class='editor-button hideFocus' style='width:16px;height:17px;font-size:10px;text-align:top;'/>")
                .appendTo(args.container);

            $txtBox = $($input[0]);
            $button = $($input[1]);

            $txtBox.bind("keydown.nav keypress mousemove", function (e) {
                var isSelected = isTextSelected(this);

                if (!isSelected) {
                    if (e.keyCode === $.ui.keyCode.LEFT && $(this).caret() != 0) {
                        e.stopImmediatePropagation();
                    } else if (e.keyCode === $.ui.keyCode.RIGHT && $(this).caret() != $(this).val().length) {
                        e.stopImmediatePropagation();
                    }
                }
            });

            $button.bind('click', args, function (e) {
                args.grid.onTextButtonClick(args);
            });

            this.disableEditor();
        };

        this.disableEditor = function () {
            var canManualEdit = args.column.canManualEdit;
            if (canManualEdit == false) {
                $txtBox.attr('readonly', 'readonly');
            }
            else {
                $txtBox.removeAttr('readonly');
            }
            $txtBox.select();
        }

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $txtBox.focus();
        };

        this.getValue = function () {
            return $txtBox.val();
        };

        this.setValue = function (value) {
            $txtBox.val(value);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $txtBox.val(defaultValue);
            $txtBox[0].defaultValue = defaultValue;
            $txtBox.select();
        };

        this.serializeValue = function () {
            var outerContent = args.container.textContent;
            if (outerContent) {
                return outerContent;
            }
            else {
                return $txtBox.val();
            }
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
            $txtBox.val(state);
        };

        this.isValueChanged = function () {
            var outerContent = args.container.textContent;
            if ((outerContent != null) && (outerContent != defaultValue)) {
                return true;
            }
            else {
                return (!($txtBox.val() == "" && defaultValue == null)) && ($txtBox.val() != defaultValue);
            }
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($txtBox.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function CheckboxEditor(args) {
        var $select;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $select = $("<INPUT type=checkbox value='true' class='editor-checkbox' hideFocus>");
            $select.appendTo(args.container);
            $select.focus();
        };

        this.destroy = function () {
            $select.remove();
        };

        this.focus = function () {
            $select.focus();
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field];
            if (defaultValue == true) {
                $select.checked = true;
            } else {
                $select.checked = false;
            }
        };

        this.serializeValue = function () {
            return $select.checked;
        };

        this.applyValue = function (item, state) {
            //item[args.column.field] = state;
            if (state == "checked")
                item[args.column.field] = true;
            else
                item[args.column.field] = false;
        };

        this.isValueChanged = function () {
            return ($select.checked != defaultValue);
        };

        this.validate = function () {
            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function CheckboxReferedEditor(args) {
        var $select;
        var defaultValue;
        var scope = this;

        this.init = function () {
            //has rendered a select by checkboxvisible formatter
            $select = $("input:checkbox", args.container)[0];
            if ($select) {
                $select.focus();
            }
        };

        this.destroy = function () {
            $(args.container).empty();
        };

        this.focus = function () {
            $select.focus();
        };

        this.loadValue = function (item) {
            //display value by checkboxvisible formatter, and then load the value again to make changed event.
            defaultValue = item[args.column.field];
        };

        this.serializeValue = function () {
            if ($select) {
                return $select.checked;
            } else {
                return false;
            }
        };

        this.applyValue = function (item, state) {
            if (state == true)
                item[args.column.field] = true;
            else
                item[args.column.field] = false;
        };

        this.isValueChanged = function () {
            return ($select.checked != defaultValue);
        };

        this.validate = function () {
            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function PercentCompleteEditor(args) {
        var $input, $picker;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $input = $("<INPUT type=text class='editor-percentcomplete' />");
            $input.width($(args.container).innerWidth() - 25);
            $input.appendTo(args.container);

            $picker = $("<div class='editor-percentcomplete-picker' />").appendTo(args.container);
            $picker.append("<div class='editor-percentcomplete-helper'><div class='editor-percentcomplete-wrapper'><div class='editor-percentcomplete-slider' /><div class='editor-percentcomplete-buttons' /></div></div>");

            $picker.find(".editor-percentcomplete-buttons").append("<button val=0>Not started</button><br/><button val=50>In Progress</button><br/><button val=100>Complete</button>");

            $input.focus().select();

            $picker.find(".editor-percentcomplete-slider").slider({
                orientation: "vertical",
                range: "min",
                value: defaultValue,
                slide: function (event, ui) {
                    $input.val(ui.value)
                }
            });

            $picker.find(".editor-percentcomplete-buttons button").bind("click", function (e) {
                $input.val($(this).attr("val"));
                $picker.find(".editor-percentcomplete-slider").slider("value", $(this).attr("val"));
            })
        };

        this.destroy = function () {
            $input.remove();
            $picker.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.loadValue = function (item) {
            $input.val(defaultValue = item[args.column.field]);
            $input.select();
        };

        this.serializeValue = function () {
            return parseInt($input.val(), 10) || 0;
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ((parseInt($input.val(), 10) || 0) != defaultValue);
        };

        this.validate = function () {
            if (isNaN(parseInt($input.val(), 10))) {
                return {
                    valid: false,
                    msg: "Please enter a valid positive number"
                };
            }

            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    /*
     * An example of a "detached" editor.
     * The UI is added onto document BODY and .position(), .show() and .hide() are implemented.
     * KeyDown events are also handled to provide handling for Tab, Shift-Tab, Esc and Ctrl-Enter.
     */
    function LongTextEditor(args) {
        var $input, $wrapper;
        var defaultValue;
        var scope = this;

        this.init = function () {
            var $container = $("body");

            $wrapper = $("<DIV style='z-index:10000;position:absolute;background:white;padding:5px;border:3px solid gray; -moz-border-radius:10px; border-radius:10px;'/>")
                .appendTo($container);

            $input = $("<TEXTAREA hidefocus rows=5 style='backround:white;width:250px;height:80px;border:0;outline:0'>")
                .appendTo($wrapper);

            $("<DIV style='text-align:right'><BUTTON>Save</BUTTON><BUTTON>Cancel</BUTTON></DIV>")
                .appendTo($wrapper);

            $wrapper.find("button:first").bind("click", this.save);
            $wrapper.find("button:last").bind("click", this.cancel);
            $input.bind("keydown", this.handleKeyDown);

            scope.position(args.position);
            $input.focus().select();
        };

        this.handleKeyDown = function (e) {
            if (e.which == $.ui.keyCode.ENTER && e.ctrlKey) {
                scope.save();
            } else if (e.which == $.ui.keyCode.ESCAPE) {
                e.preventDefault();
                scope.cancel();
            } else if (e.which == $.ui.keyCode.TAB && e.shiftKey) {
                e.preventDefault();
                args.grid.navigatePrev();
            } else if (e.which == $.ui.keyCode.TAB) {
                e.preventDefault();
                args.grid.navigateNext();
            }
        };

        this.save = function () {
            args.commitChanges();
        };

        this.cancel = function () {
            $input.val(defaultValue);
            args.cancelChanges();
        };

        this.hide = function () {
            $wrapper.hide();
        };

        this.show = function () {
            $wrapper.show();
        };

        this.position = function (position) {
            $wrapper
                .css("top", position.top - 5)
                .css("left", position.left - 5)
        };

        this.destroy = function () {
            $wrapper.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.loadValue = function (item) {
            $input.val(defaultValue = item[args.column.field]);
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function SelectCellEditor(args) {
        var $select;
        var defaultValue;
        var scope = this;

        this.init = function () {
            if (args.column.options) {
                opt_values = args.column.options.split(',');
            } else {
                opt_values = "yes,no".split(',');
            }
            option_str = ""
            for (i in opt_values) {
                v = opt_values[i];
                option_str += "<OPTION value='" + v + "'>" + v + "</OPTION>";
            }
            $select = $("<SELECT tabIndex='0' class='editor-select'>" + option_str + "</SELECT>");
            $select.appendTo(args.container);
            $select.focus();
        };

        this.destory = function () {
            $select.remove();
        };

        this.focus = function () {
            $select.focus();
        };

        this.loadValue = function (item) {
            defaultValue = item[arg.column.field];
            $select.val(defaultValue);
        };

        this.serializeValue = function () {
            if (args.column.options) {
                return $select.val();
            } else {
                return ($select.val() == "yes");
            }
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = stat;
        };

        this.isValueChanged = function () {
            return ($select.val() != defaultValue);
        };

        this.validate = function () {
            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function RowNumberSelector(args) {
        var ranges = [], row = args.row;
        var grid, defaultValue;
        var self = this;

        this.init = function () {
            grid = args.grid;
            $(args.container)
                .focus()
                .select();
        }

        this.loadValue = function (item) {
            return row + 1;
        }

        this.applyValue = function (item, state) {
            return;
        };

        this.serializeValue = function () {
            return row + 1;
        };

        this.isValueChanged = function () {
            return false;
        };

        this.destroy = function () {
        };

        this.init();
    }
})(jQuery);
//#endregion

//#region SlickGrid js
(function ($) {
    // Slick.Grid
    $.extend(true, window, {
        Slick: {
            Grid: SlickGrid
        }
    });

    // shared across all grids on the page
    var scrollbarDimensions;
    var maxSupportedCssHeight;  // browser's breaking point

    //////////////////////////////////////////////////////////////////////////////////////////////
    // SlickGrid class implementation (available as Slick.Grid)

    /**
     * Creates a new instance of the grid.
     * @class SlickGrid
     * @constructor
     * @param {Node}              container   Container node to create the grid in.
     * @param {Array,Object}      data        An array of objects for databinding.
     * @param {Array}             columns     An array of column definitions.
     * @param {Object}            options     Grid options.
     **/
    function SlickGrid(container, data, columns, options) {
        // settings
        var defaults = {
            explicitInitialization: false,
            rowHeight: 25,
            defaultColumnWidth: 80,
            enableAddRow: false,
            leaveSpaceForNewRows: false,
            editable: false,
            autoEdit: true,
            enableCellNavigation: true,
            enableColumnReorder: true,
            asyncEditorLoading: false,
            asyncEditorLoadDelay: 100,
            forceFitColumns: false,
            enableAsyncPostRender: false,
            asyncPostRenderDelay: 50,
            autoHeight: false,
            editorLock: Slick.GlobalEditorLock,
            showTopPanel: false,
            topPanelHeight: 25,
            formatterFactory: null,
            editorFactory: null,
            cellFlashingCssClass: "flashing",
            selectedCellCssClass: "selected",
            multiSelect: true,
            enableTextSelectionOnCells: false,
            dataItemColumnValueExtractor: null,
            frozenColumn: -1,
            fullWidthRows: false,
            multiColumnSort: false,
            defaultFormatter: defaultFormatter,
            forceSyncScrolling: false
        };

        var columnDefaults = {
            name: "",
            resizable: true,
            sortable: false,
            minWidth: 30,
            rerenderOnResize: false,
            headerCssClass: null,
            defaultSortAsc: true
        };

        // scroller
        var th;   // virtual height
        var h;    // real scrollable height
        var ph;   // page height
        var n;    // number of pages
        var cj;   // "jumpiness" coefficient

        var page = 0;       // current page
        var offset = 0;     // current page offset
        var vScrollDir = 1;

        // private
        var initialized = false;
        var $container;
        var uid = "slickgrid_" + Math.round(1000000 * Math.random());
        var self = this;
        var $focusSink, $focusSink2;
        var $headerScroller;
        var $headers;
        var $topPanel;
        var $viewport;
        var $canvas;
        var $style;
        var $boundAncestors;
        var stylesheet, columnCssRulesL, columnCssRulesR;
        var viewportH, viewportW;
        var canvasWidth, canvasWidthL, canvasWidthR;
        var headersWidth, headersWidthL, headersWidthR;
        var viewportHasHScroll, viewportHasVScroll;
        var headerColumnWidthDiff = 0,
            headerColumnHeightDiff = 0,
            // border+padding
            cellWidthDiff = 0,
            cellHeightDiff = 0;
        var absoluteColumnMinWidth;
        var numberOfRows = 0;
        var paneTopH = 0;
        var viewportTopH = 0;
        var topPanelH = 0;

        var tabbingDirection = 1;
        var $activeCanvasNode;
        var $activeViewportNode;
        var activePosX;
        var activeRow, activeCell;
        var activeCellNode = null;
        var currentEditor = null;
        var serializedEditorValue;
        var editController;

        var rowsCache = {};
        var renderedRows = 0;
        var numVisibleRows;
        var prevScrollTop = 0;
        var scrollTop = 0;
        var lastRenderedScrollTop = 0;
        var lastRenderedScrollLeft = 0;
        var prevScrollLeft = 0;
        var scrollLeft = 0;

        var selectionModel;
        var selectedRows = [];
        var shiftSelectedRows = [];

        var plugins = [];
        var cellCssClasses = {};

        var columnsById = {};
        var sortColumns = [];
        var columnPosLeft = [];
        var columnPosRight = [];


        // async call handles
        var h_editorLoader = null;
        var h_render = null;
        var h_postrender = null;
        var postProcessedRows = {};
        var postProcessToRow = null;
        var postProcessFromRow = null;

        // perf counters
        var counter_rows_rendered = 0;
        var counter_rows_removed = 0;

        var $paneHeaderL;
        var $paneHeaderR;
        var $paneTopL;
        var $paneTopR;

        var $headerScrollerL;
        var $headerScrollerR;

        var $headerL;
        var $headerR;

        var $viewportTopL;
        var $viewportTopR;

        var $canvasTopL;
        var $canvasTopR;

        var $headerScrollContainer;
        var $viewportScrollContainerX;
        var $viewportScrollContainerY;
        //////////////////////////////////////////////////////////////////////////////////////////////
        // Initialization

        function init() {
            //var start = new Date().getTime();
            var visibleColumns = getVisibleColumns();
            columns = visibleColumns;

            $container = $(container);
            if ($container.length < 1) {
                throw new Error("SlickGrid requires a valid container, " + container + " does not exist in the DOM.");
            }

            // calculate these only once and share between grid instances
            maxSupportedCssHeight = maxSupportedCssHeight || getMaxSupportedCssHeight();
            scrollbarDimensions = scrollbarDimensions || measureScrollbar();

            options = $.extend({}, defaults, options);
            validateAndEnforceOptions();
            columnDefaults.width = options.defaultColumnWidth;

            columnsById = {};
            for (var i = 0; i < columns.length; i++) {
                var m = columns[i] = $.extend({}, columnDefaults, columns[i]);
                columnsById[m.id] = i;
                if (m.minWidth && m.width < m.minWidth) {
                    m.width = m.minWidth;
                }
                if (m.maxWidth && m.width > m.maxWidth) {
                    m.width = m.maxWidth;
                }
            }

            // validate loaded JavaScript modules against requested options
            if (options.enableColumnReorder && !$.fn.sortable) {
                throw new Error("SlickGrid's 'enableColumnReorder = true' option requires jquery-ui.sortable module to be loaded");
            }

            editController = {
                "commitCurrentEdit": commitCurrentEdit,
                "cancelCurrentEdit": cancelCurrentEdit
            };

            $container
                .empty()
                .css("overflow", "hidden")
                .css("outline", 0)
                .addClass(uid)
                .addClass("ui-widget");

            // set up a positioning container if needed
            if (!/relative|absolute|fixed/.test($container.css("position"))) {
                $container.css("position", "relative");
            }

            var index = Math.floor(Math.random() * 1000);
            var sinkId = "focusSink_" + index;
            var paneHeaderLId = "paneHeaderL_" + index;
            var paneHeaderRId = "paneHeaderR_" + index;
            var paneTopLId = "paneTopL_" + index;
            var paneTopRId = "paneTopR_" + index;
            var headerScrollerLId = "headerScrollerL_" + index;
            var headerScrollerRId = "headerScrollerR_" + index;
            var headerLId = "headerL_" + index;
            var headerRId = "headerR_" + index;
            var viewportTopLId = "viewportTopL_" + index;
            var viewportTopRId = "viewportTopR_" + index;
            var canvasTopLId = "canvasTopL_" + index;
            var canvasTopRId = "canvasTopR_" + index;

            var sinkHtml = "<div id='" + sinkId + "' class='slick-focusSink' tabIndex='0' hideFocus style='position:fixed;width:0;height:0;top:0;left:0;outline:0;'></div>"
                + "<div id='" + paneHeaderLId + "' class='slick-pane slick-pane-header slick-pane-left' tabIndex='0'>"
                    + "<div id='" + headerScrollerLId + "' class='ui-state-default slick-header slick-header-left'>"
                        + "<div id='" + headerLId + "' class='slick-header-columns slick-header-columns-left' style='left:-1000px' />"
                    + "</div>"
                + "</div>"
                + "<div id='" + paneHeaderRId + "' class='slick-pane slick-pane-header slick-pane-right' tabIndex='0'>"
                    + "<div id='" + headerScrollerRId + "' class='ui-state-default slick-header slick-header-right'>"
                         + "<div id='" + headerRId + "' class='slick-header-columns slick-header-columns-right' style='left:-1000px' />"
                    + "</div>"
                + "</div>"
                + "<div id='" + paneTopLId + "' class='slick-pane slick-pane-top slick-pane-left' tabIndex='0' >"
                    + "<div id='" + viewportTopLId + "' class='slick-viewport slick-viewport-top slick-viewport-left' tabIndex='0' hideFocus >"
                         + "<div id='" + canvasTopLId + "' class='grid-canvas grid-canvas-top grid-canvas-left' tabIndex='0' hideFocus />"
                    + "</div>"
                + "</div>"
                + "<div id='" + paneTopRId + "' class='slick-pane slick-pane-top slick-pane-right' tabIndex='0'>"
                    + "<div id='" + viewportTopRId + "' class='slick-viewport slick-viewport-top slick-viewport-right' tabIndex='0' hideFocus >"
                        + "<div id='" + canvasTopRId + "' class='grid-canvas grid-canvas-top grid-canvas-right' tabIndex='0' hideFocus />"
                    + "</div>"
                + "</div>";

            //Containers used for scrolling frozen columns
            $container.html(sinkHtml);

            $focusSink = $("#" + sinkId);
            $paneHeaderL = $("#" + paneHeaderLId);
            $paneHeaderR = $("#" + paneHeaderRId);
            $paneTopL = $("#" + paneTopLId);
            $paneTopR = $("#" + paneTopRId);
            $headerScrollerL = $("#" + headerScrollerLId);
            $headerScrollerR = $("#" + headerScrollerRId);
            $headerL = $("#" + headerLId);
            $headerR = $("#" + headerRId);
            $viewportTopL = $("#" + viewportTopLId);
            $viewportTopR = $("#" + viewportTopRId);
            $canvasTopL = $("#" + canvasTopLId);
            $canvasTopR = $("#" + canvasTopRId);

            // Cache the header scroller containers
            $headerScroller = $().add($headerScrollerL).add($headerScrollerR);

            // Cache the header columns
            $headers = $().add($headerL).add($headerR);

            // Cache the viewports
            $viewport = $().add($viewportTopL).add($viewportTopR);
            // Default the active viewport to the top left
            $activeViewportNode = $viewportTopL;

            // Cache the canvases
            $canvas = $().add($canvasTopL).add($canvasTopR);

            // Default the active canvas to the top left
            $activeCanvasNode = $canvasTopL;

            $focusSink2 = $focusSink.clone().appendTo($container);

            if (!options.explicitInitialization) {
                finishInitialization();
            }
        }

        function getVisibleColumns() {
            var visibleColumns = jQuery.grep(columns, function (x) {
                return (x && x.visible != false);
            });
            return visibleColumns;
        }

        function getGridId() {
            return uid;
        }

        function getHeaderColumnWidthDiff() {
            return headerColumnWidthDiff;
        }

        function finishInitialization() {
            if (!initialized) {
                initialized = true;

                getViewportWidth();
                getViewportHeight();

                // header columns and cells may have different padding/border
                // skewing width calculations (box-sizing, hello?)
                // calculate the diff so we can set consistent sizes
                measureCellPaddingAndBorder();

                // for usability reasons, all text selection in SlickGrid is
                // disabled with the exception of input and textarea elements (selection
                // must be enabled there so that editors work as expected); note that
                // selection in grid cells (grid body) is already unavailable in
                // all browsers except IE
                disableSelection($headers); // disable all text selection in header (including input and textarea)

                if (!options.enableTextSelectionOnCells) {
                    // disable text selection in grid cells except in input and textarea elements
                    // (this is IE-specific, because selectstart event will only fire in IE)
                    $viewport.bind("selectstart.ui", function (event) {
                        return $(event.target).is("input,textarea");
                    });
                }

                setFrozenOptions();
                setPaneVisibility();
                setScroller();
                setOverflow();

                updateColumnCaches();
                createColumnHeaders();      //slow
                setupColumnSort();
                createCssRules();
                resizeCanvas();         //slow
                bindAncestorScrollEvents();

                $container
                    .bind("resize.slickgrid", resizeCanvas);
                $viewport
                    .bind("scroll", handleScroll);
                if (jQuery.fn.mousewheel && (options.frozenColumn > -1)) {
                    $viewport
                       .bind("mousewheel", handleMouseWheel);
                }
                $headerScroller
                    .bind("contextmenu", handleHeaderContextMenu)
                    .bind("click", handleHeaderClick)
                    .delegate(".slick-header-column", "mouseenter", handleHeaderMouseEnter)
                    .delegate(".slick-header-column", "mouseleave", handleHeaderMouseLeave);
                $focusSink.add($focusSink2)
                    .bind("keydown", handleKeyDown);
                $canvas
                    .bind("keydown", handleKeyDown)
                    .bind("click", handleClick)
                    .bind("dblclick", handleDblClick)
                    .bind("contextmenu", handleContextMenu)
                    .bind("draginit", handleDragInit)
                    .bind("dragstart", { distance: 3 }, handleDragStart)
                    .bind("drag", handleDrag)
                    .bind("dragend", handleDragEnd)
                    .delegate(".slick-row", "mouseenter", handleRowMouseEnter)
                    .delegate(".slick-row", "mouseleave", handleRowMouseLeave)
                    .delegate(".slick-cell", "mouseenter", handleMouseEnter)
                    .delegate(".slick-cell", "mouseleave", handleMouseLeave);
            }
        }

        function registerPlugin(plugin) {
            plugins.unshift(plugin);
            plugin.init(self);
        }

        function unregisterPlugin(plugin) {
            for (var i = plugins.length; i >= 0; i--) {
                if (plugins[i] === plugin) {
                    if (plugins[i].destroy) {
                        plugins[i].destroy();
                    }
                    plugins.splice(i, 1);
                    break;
                }
            }
        }

        function setSelectionModel(model) {
            if (selectionModel) {
                selectionModel.onSelectedRangesChanged.unsubscribe(handleSelectedRangesChanged);
                if (selectionModel.destroy) {
                    selectionModel.destroy();
                }
            }

            selectionModel = model;
            if (selectionModel) {
                selectionModel.init(self);
                selectionModel.onSelectedRangesChanged.subscribe(handleSelectedRangesChanged);
            }
        }

        function getSelectionModel() {
            return selectionModel;
        }

        function getCanvasNode(cell) {
            if (options.frozenColumn > 0) {
                if (cell > options.frozenColumn) {
                    return $canvas[1];
                } else {
                    return $canvas[0];
                }
            }
            else {
                return $canvas[0];
            }

        }

        function getActiveCanvasNode(element) {
            setActiveCanvasNode(element);

            return $activeCanvasNode[0];
        }

        function getCanvases() {
            return $canvas;
        }

        function setActiveCanvasNode(element) {
            if (element) {
                $activeCanvasNode = $(element.target).closest('.grid-canvas');
            }
        }

        function getViewportNode() {
            return $viewport[0];
        }

        function getActiveViewportNode(element) {
            setActiveViewPortNode(element);

            return $activeViewportNode[0];
        }

        function setActiveViewportNode(element) {
            if (element) {
                $activeViewportNode = $(element.target).closest('.slick-viewport');
            }
        }

        function measureScrollbar() {
            var $c = $("<div style='position:absolute; top:-20000px; left:-20000px; width:100px; height:100px; overflow:scroll;'></div>").appendTo("body");
            var dim = {
                width: $c.width() - $c[0].clientWidth,
                height: $c.height() - $c[0].clientHeight
            };
            $c.remove();
            return dim;
        }

        function getHeadersWidth() {
            headersWidth = headersWidthL = headersWidthR = 0;

            for (var i = 0, ii = columns.length; i < ii; i++) {
                var width = columns[i].width;

                if ((options.frozenColumn) > -1 && (i > options.frozenColumn)) {
                    headersWidthR += width;
                } else {
                    headersWidthL += width;
                }
            }

            if (options.frozenColumn > -1) {
                headersWidthL = headersWidthL + 1000;

                headersWidthR = Math.max(headersWidthR, viewportW) + headersWidthL;
                headersWidthR += scrollbarDimensions.width;
            } else {
                headersWidthL += scrollbarDimensions.width;
                headersWidthL = Math.max(headersWidthL, viewportW) + 1000;
            }

            headersWidth = headersWidthL + headersWidthR;
            return headersWidth;
        }

        function getCanvasWidth() {
            var availableWidth = viewportHasVScroll ? viewportW - scrollbarDimensions.width : viewportW;

            var i = columns.length;

            canvasWidthL = canvasWidthR = 0;

            while (i--) {
                if ((options.frozenColumn > -1) && (i > options.frozenColumn)) {
                    canvasWidthR += columns[i].width;
                } else {
                    canvasWidthL += columns[i].width;
                }
            }

            var totalRowWidth = canvasWidthL + canvasWidthR;

            return options.fullWidthRows ? Math.max(totalRowWidth, availableWidth) : totalRowWidth;
        }

        function updateCanvasWidth(forceColumnWidthsUpdate) {
            var oldCanvasWidth = canvasWidth;
            var oldCanvasWidthL = canvasWidthL;
            var oldCanvasWidthR = canvasWidthR;
            var widthChanged;
            canvasWidth = getCanvasWidth();

            widthChanged = canvasWidth !== oldCanvasWidth || canvasWidthL !== oldCanvasWidthL || canvasWidthR !== oldCanvasWidthR;

            if (widthChanged || options.frozenColumn > -1) {
                $canvasTopL.width(canvasWidthL);

                getHeadersWidth();

                if (headersWidthL < 1035)
                    headersWidthL += 1000;
                if (headersWidthR < 1035)
                    headersWidthR += 1000;

                $headerL.width(headersWidthL);
                $headerR.width(headersWidthR);

                if (options.frozenColumn > -1) {
                    $canvasTopR.width(canvasWidthR);

                    $paneHeaderL.width(canvasWidthL);
                    $paneHeaderR.css('left', canvasWidthL);
                    $paneHeaderR.css('top', 0);

                    $paneTopL.width(canvasWidthL);
                    $paneTopR.css('left', canvasWidthL);

                    $viewportTopL.width(canvasWidthL);
                    $viewportTopR.width(viewportW - canvasWidthL);

                } else {
                    //$paneHeaderL.width('100%');
                    //$paneTopL.width('100%');
                    //$viewportTopL.width('100%');
                }

                viewportHasHScroll = (canvasWidth > viewportW - scrollbarDimensions.width);
            }

            if (widthChanged || forceColumnWidthsUpdate) {
                applyColumnWidths();
            }
        }

        function disableSelection($target) {
            if ($target && $target.jquery) {
                $target.attr("unselectable", "on").css("MozUserSelect", "none").bind("selectstart.ui", function () {
                    return false;
                }); // from jquery:ui.core.js 1.7.2
            }
        }

        function getMaxSupportedCssHeight() {
            var supportedHeight = 1000000;
            // FF reports the height back but still renders blank after ~6M px
            var testUpTo = (!Sys.ie) ? 6000000 : 1000000000;//$.browser.mozilla
            var div = $("<div style='display:none' />").appendTo(document.body);

            while (true) {
                var test = supportedHeight * 2;
                div.css("height", test);
                if (test > testUpTo || div.height() !== test) {
                    break;
                } else {
                    supportedHeight = test;
                }
            }

            div.remove();
            return supportedHeight;
        }

        // TODO:  this is static.  need to handle page mutation.
        function bindAncestorScrollEvents() {
            var elem = $canvasTopL[0];
            while ((elem = elem.parentNode) != document.body && elem != null) {
                // bind to scroll containers only
                if (elem == $viewportTopL[0] || elem.scrollWidth != elem.clientWidth || elem.scrollHeight != elem.clientHeight) {
                    var $elem = $(elem);
                    if (!$boundAncestors) {
                        $boundAncestors = $elem;
                    } else {
                        $boundAncestors = $boundAncestors.add($elem);
                    }
                    $elem.bind("scroll." + uid, handleActiveCellPositionChange);
                }
            }
        }

        function unbindAncestorScrollEvents() {
            if (!$boundAncestors) {
                return;
            }
            $boundAncestors.unbind("scroll." + uid);
            $boundAncestors = null;
        }

        function updateColumnHeader(columnId, title, toolTip) {
            if (!initialized) {
                return;
            }
            var idx = getColumnIndex(columnId);
            if (idx == null) {
                return;
            }

            var columnDef = columns[idx];
            var $header = $headers.children().eq(idx);
            if ($header) {
                if (title !== undefined) {
                    columns[idx].name = title;
                }
                if (toolTip !== undefined) {
                    columns[idx].toolTip = toolTip;
                }

                trigger(self.onBeforeHeaderCellDestroy, {
                    "node": $header[0],
                    "column": columnDef
                });

                $header.attr("title", toolTip || "").children().eq(0).html(title);

                trigger(self.onHeaderCellRendered, {
                    "node": $header[0],
                    "column": columnDef
                });
            }
        }

        function createColumnHeaders() {
            function hoverBegin() {
                $(this).addClass("ui-state-hover");
            }

            function hoverEnd() {
                $(this).removeClass("ui-state-hover");
            }

            $headers.find(".slick-header-column")
                .each(function () {
                    var columnDef = $(this).data("column");
                    if (columnDef) {
                        trigger(self.onBeforeHeaderCellDestroy, {
                            "node": this,
                            "column": columnDef
                        });
                    }
                });

            $headerL.empty();
            $headerR.empty();

            getHeadersWidth();

            if (headersWidthL < 1035)
                headersWidthL += 1000;
            if (headersWidthR < 1035)
                headersWidthR +=1000;

            $headerL.width(headersWidthL);
            $headerR.width(headersWidthR);

            var maxSpanHeight = 16;
            var divHeaderID;
            var divHeaderCSS = 'dHCss0x9liY86gong' + uid;

            for (var i = 0; i < columns.length; i++) {
                var m = columns[i];

                var $headerTarget = (options.frozenColumn > -1) ? ((i <= options.frozenColumn) ? $headerL : $headerR) : $headerL;

                divHeaderID = uid + m.id;

                var header = $("<div class='ui-state-default slick-header-column " + divHeaderCSS + "' id='" + divHeaderID + "' />")
                    .html("<span class='slick-column-name'>" + m.name + "</span>")
                    .width(m.width - headerColumnWidthDiff)
                    .attr("title", m.toolTip || "")
                    .data("column", m)
                    .addClass(m.headerCssClass || "")
                    .appendTo($headerTarget);

                var divHeader = $("#" + divHeaderID);
                var spanHeader = divHeader.children().first();  //get span in div

                if (spanHeader.height() > divHeader.height()
                    && spanHeader.height() > maxSpanHeight) {
                    maxSpanHeight = spanHeader.height();
                }

                if (options.enableColumnReorder || m.sortable) {
                    header.hover(hoverBegin, hoverEnd);
                }

                if (m.sortable) {
                    header.addClass("slick-header-sortable");
                    header.append("<span class='slick-sort-indicator' />");
                }

                trigger(self.onHeaderCellRendered, {
                    "node": header[0],
                    "column": m
                });
            }

            //setting header height
            $("div." + divHeaderCSS).css("height", maxSpanHeight);

            setSortColumns(sortColumns);
            setupColumnResize();
            if (options.enableColumnReorder) {
                setupColumnReorder();
            }
        }

        function setupColumnSort() {
            $headers.click(function (e) {
                // temporary workaround for a bug in jQuery 1.7.1
                // (http://bugs.jquery.com/ticket/11328)
                e.metaKey = e.metaKey || e.ctrlKey;

                if ($(e.target).hasClass("slick-resizable-handle")) {
                    return;
                }

                var $col = $(e.target).closest(".slick-header-column");
                if (!$col.length) {
                    return;
                }

                var column = $col.data("column");
                if (column.sortable) {
                    if (!getEditorLock().commitCurrentEdit()) {
                        return;
                    }

                    var sortOpts = null;
                    var i = 0;
                    for (; i < sortColumns.length; i++) {
                        if (sortColumns[i].columnId == column.id) {
                            sortOpts = sortColumns[i];
                            sortOpts.sortAsc = !sortOpts.sortAsc;
                            break;
                        }
                    }

                    if (e.metaKey && options.multiColumnSort) {
                        if (sortOpts) {
                            sortColumns.splice(i, 1);
                        }
                    } else {
                        if ((!e.shiftKey && !e.metaKey) || !options.multiColumnSort) {
                            sortColumns = [];
                        }

                        if (!sortOpts) {
                            sortOpts = {
                                columnId: column.id,
                                sortAsc: true
                            };
                            sortColumns.push(sortOpts);
                        } else if (sortColumns.length == 0) {
                            sortColumns.push(sortOpts);
                        }
                    }

                    setSortColumns(sortColumns);

                    if (!options.multiColumnSort) {
                        trigger(self.onSort, {
                            multiColumnSort: false,
                            sortCol: column,
                            sortAsc: sortOpts.sortAsc
                        }, e);
                    } else {
                        trigger(
                        self.onSort, {
                            multiColumnSort: true,
                            sortCols: $.map(
                            sortColumns, function (col) {
                                return {
                                    sortCol: columns[getColumnIndex(col.columnId)],
                                    sortAsc: col.sortAsc
                                };
                            })
                        }, e);
                    }
                }
            });
        }

        function setupColumnReorder() {
            $headers.filter(":ui-sortable").sortable("destroy");
            var columnScrollTimer = null;

            function scrollColumnsRight() {
                $viewportScrollContainerX[0].scrollLeft = $viewportScrollContainerX[0].scrollLeft + 10;
            }

            function scrollColumnsLeft() {
                $viewportScrollContainerX[0].scrollLeft = $viewportScrollContainerX[0].scrollLeft - 10;
            }

            $headers.sortable({
                containment: "parent",
                distance: 3,
                axis: "x",
                cursor: "default",
                tolerance: "intersection",
                helper: "clone",
                placeholder: "slick-sortable-placeholder ui-state-default slick-header-column",
                forcePlaceholderSize: true,
                start: function (e, ui) {
                    $(ui.helper).addClass("slick-header-column-active");
                },
                beforeStop: function (e, ui) {
                    $(ui.helper).removeClass("slick-header-column-active");
                },
                sort: function (e, ui) {
                    if (e.originalEvent.pageX > $container[0].clientWidth) {
                        //if (!(columnScrollTimer)) {
                        //    columnScrollTimer = setInterval(
                        //    scrollColumnsRight, 100);
                        //}
                    } else if (e.originalEvent.pageX < $viewportScrollContainerX.offset().left) {
                        //if (!(columnScrollTimer)) {
                        //    columnScrollTimer = setInterval(
                        //    scrollColumnsLeft, 100);
                        //}
                    } else {
                        clearInterval(columnScrollTimer);
                        columnScrollTimer = null;
                    }
                },
                stop: function (e) {
                    clearInterval(columnScrollTimer);
                    columnScrollTimer = null;

                    if (!getEditorLock().commitCurrentEdit()) {
                        $(this).sortable("cancel");
                        return;
                    }

                    var reorderedIds = $headerL.sortable("toArray");
                    reorderedIds = reorderedIds.concat($headerR.sortable("toArray"));

                    var reorderedColumns = [];
                    for (var i = 0; i < reorderedIds.length; i++) {
                        reorderedColumns.push(columns[getColumnIndex(reorderedIds[i].replace(uid, ""))]);
                    }
                    setColumns(reorderedColumns);

                    trigger(self.onColumnsReordered, {});
                    e.stopPropagation();
                    setupColumnResize();
                }
            });
        }

        function setupColumnResize() {
            var $col, j, c, pageX, columnElements, minPageX, maxPageX, firstResizable, lastResizable;
            columnElements = $headers.children();
            columnElements.find(".slick-resizable-handle").remove();
            columnElements.each(function (i, e) {
                if (columns[i].resizable) {
                    if (firstResizable === undefined) {
                        firstResizable = i;
                    }
                    lastResizable = i;
                }
            });
            if (firstResizable === undefined) {
                return;
            }
            columnElements.each(function (i, e) {
                if (i < firstResizable || (options.forceFitColumns && i >= lastResizable)) {
                    return;
                }
                $col = $(e);
                $("<div class='slick-resizable-handle' />")
                    .appendTo(e)
                    .bind("dragstart", function (e, dd) {
                        if (!getEditorLock().commitCurrentEdit()) {
                            return false;
                        }
                        pageX = e.pageX;
                        $(this).parent().addClass("slick-header-column-active");
                        var shrinkLeewayOnRight = null,
                            stretchLeewayOnRight = null;
                        // lock each column's width option to current width
                        columnElements.each(function (i, e) {
                            columns[i].previousWidth = $(e).outerWidth();
                        });
                        if (options.forceFitColumns) {
                            shrinkLeewayOnRight = 0;
                            stretchLeewayOnRight = 0;
                            // colums on right affect maxPageX/minPageX
                            for (j = i + 1; j < columnElements.length; j++) {
                                c = columns[j];
                                if (c.resizable) {
                                    if (stretchLeewayOnRight !== null) {
                                        if (c.maxWidth) {
                                            stretchLeewayOnRight += c.maxWidth - c.previousWidth;
                                        } else {
                                            stretchLeewayOnRight = null;
                                        }
                                    }
                                    shrinkLeewayOnRight += c.previousWidth - Math.max(c.minWidth || 0, absoluteColumnMinWidth);
                                }
                            }
                        }
                        var shrinkLeewayOnLeft = 0,
                            stretchLeewayOnLeft = 0;
                        for (j = 0; j <= i; j++) {
                            // columns on left only affect minPageX
                            c = columns[j];
                            if (c.resizable) {
                                if (stretchLeewayOnLeft !== null) {
                                    if (c.maxWidth) {
                                        stretchLeewayOnLeft += c.maxWidth - c.previousWidth;
                                    } else {
                                        stretchLeewayOnLeft = null;
                                    }
                                }
                                shrinkLeewayOnLeft += c.previousWidth - Math.max(c.minWidth || 0, absoluteColumnMinWidth);
                            }
                        }
                        if (shrinkLeewayOnRight === null) {
                            shrinkLeewayOnRight = 100000;
                        }
                        if (shrinkLeewayOnLeft === null) {
                            shrinkLeewayOnLeft = 100000;
                        }
                        if (stretchLeewayOnRight === null) {
                            stretchLeewayOnRight = 100000;
                        }
                        if (stretchLeewayOnLeft === null) {
                            stretchLeewayOnLeft = 100000;
                        }
                        maxPageX = pageX + Math.min(shrinkLeewayOnRight, stretchLeewayOnLeft);
                        minPageX = pageX - Math.min(shrinkLeewayOnLeft, stretchLeewayOnRight);
                    }).bind("drag", function (e, dd) {
                        var actualMinWidth, d = Math.min(maxPageX, Math.max(minPageX, e.pageX)) - pageX,
                            x;

                        if (d < 0) { // shrink column
                            x = d;

                            var newCanvasWidthL = 0, newCanvasWidthR = 0;

                            for (j = i; j >= 0; j--) {
                                c = columns[j];
                                if (c.resizable) {
                                    actualMinWidth = Math.max(c.minWidth || 0, absoluteColumnMinWidth);
                                    if (x && c.previousWidth + x < actualMinWidth) {
                                        x += c.previousWidth - actualMinWidth;
                                        c.width = actualMinWidth;
                                    } else {
                                        c.width = c.previousWidth + x;
                                        x = 0;
                                    }
                                }
                            }

                            for (k = 0; k <= i; k++) {
                                c = columns[k];

                                if ((options.frozenColumn > -1) && (k > options.frozenColumn)) {
                                    newCanvasWidthR += c.width;
                                } else {
                                    newCanvasWidthL += c.width;
                                }
                            }

                            if (options.forceFitColumns) {
                                x = -d;
                                for (j = i + 1; j < columnElements.length; j++) {
                                    c = columns[j];
                                    if (c.resizable) {
                                        if (x && c.maxWidth && (c.maxWidth - c.previousWidth < x)) {
                                            x -= c.maxWidth - c.previousWidth;
                                            c.width = c.maxWidth;
                                        } else {
                                            c.width = c.previousWidth + x;
                                            x = 0;
                                        }

                                        if ((options.frozenColumn > -1) && (j > options.frozenColumn)) {
                                            newCanvasWidthR += c.width;
                                        } else {
                                            newCanvasWidthL += c.width;
                                        }
                                    }
                                }
                            } else {
                                for (j = i + 1; j < columnElements.length; j++) {
                                    c = columns[j];

                                    if ((options.frozenColumn > -1) && (j > options.frozenColumn)) {
                                        newCanvasWidthR += c.width;
                                    } else {
                                        newCanvasWidthL += c.width;
                                    }
                                }
                            }
                        } else { // stretch column
                            x = d;

                            var newCanvasWidthL = 0, newCanvasWidthR = 0;

                            for (j = i; j >= 0; j--) {
                                c = columns[j];
                                if (c.resizable) {
                                    if (x && c.maxWidth && (c.maxWidth - c.previousWidth < x)) {
                                        x -= c.maxWidth - c.previousWidth;
                                        c.width = c.maxWidth;
                                    } else {
                                        c.width = c.previousWidth + x;
                                        x = 0;
                                    }
                                }
                            }

                            for (k = 0; k <= i; k++) {
                                c = columns[k];

                                if ((options.frozenColumn > -1) && (k > options.frozenColumn)) {
                                    newCanvasWidthR += c.width;
                                } else {
                                    newCanvasWidthL += c.width;
                                }
                            }

                            if (options.forceFitColumns) {
                                x = -d;
                                for (j = i + 1; j < columnElements.length; j++) {
                                    c = columns[j];
                                    if (c.resizable) {
                                        actualMinWidth = Math.max(c.minWidth || 0, absoluteColumnMinWidth);
                                        if (x && c.previousWidth + x < actualMinWidth) {
                                            x += c.previousWidth - actualMinWidth;
                                            c.width = actualMinWidth;
                                        } else {
                                            c.width = c.previousWidth + x;
                                            x = 0;
                                        }

                                        if ((options.frozenColumn > -1) && (j > options.frozenColumn)) {
                                            newCanvasWidthR += c.width;
                                        } else {
                                            newCanvasWidthL += c.width;
                                        }
                                    }
                                }
                            } else {
                                for (j = i + 1; j < columnElements.length; j++) {
                                    c = columns[j];

                                    if ((options.frozenColumn > -1) && (j > options.frozenColumn)) {
                                        newCanvasWidthR += c.width;
                                    } else {
                                        newCanvasWidthL += c.width;
                                    }
                                }
                            }
                        }

                        if (options.frozenColumn > -1 && newCanvasWidthL != canvasWidthL) {
                            $headerL.width(newCanvasWidthL + 1000);
                            $paneHeaderR.css('left', newCanvasWidthL);
                        }

                        applyColumnHeaderWidths();
                        if (options.syncColumnCellResize) {
                            updateCanvasWidth();
                            applyColumnWidths();
                        }
                    }).bind("dragend", function (e, dd) {
                        var newWidth, newWidthT, newJ;
                        $(this).parent().removeClass("slick-header-column-active");
                        for (j = 0; j < columnElements.length; j++) {
                            c = columns[j];
                            newWidth = $(columnElements[j]).outerWidth();

                            if (c.previousWidth !== newWidth) {
                                newWidthT = newWidth;
                                newJ = j;
                                if (c.rerenderOnResize) {
                                    invalidateAllRows();
                                }
                            }
                        }
                        updateCanvasWidth(true);
                        render();
                        trigger(self.onColumnsResized, { "column": newJ, "width": newWidthT });
                    });
            });
        }

        function getVBoxDelta($el) {
            var p = ["borderTopWidth", "borderBottomWidth", "paddingTop", "paddingBottom"];
            var delta = 0;
            $.each(p, function (n, val) {
                delta += parseFloat($el.css(val)) || 0;
            });
            return delta;
        }

        function setFrozenOptions() {
            options.frozenColumn = (options.frozenColumn >= 0
                                     && options.frozenColumn < columns.length
                                   )
                                   ? parseInt(options.frozenColumn)
                                   : -1;
        }

        function setPaneVisibility() {
            if (options.frozenColumn > -1) {
                $paneHeaderR.show();
                $paneTopR.show();
            } else {
                $paneHeaderR.hide();
                $paneTopR.hide();
            }
        }

        function setOverflow() {
            if (options.frozenColumn > -1) {
                $viewportTopL.css({ 'overflow-x': 'scroll', 'overflow-y': 'hidden' });
                $viewportTopR.css({ 'overflow-x': 'scroll', 'overflow-y': 'auto' });
            }
            else {
                $viewportTopL.css({ 'overflow-x': 'auto', 'overflow-y': 'auto' });
                $viewportTopR.css({ 'overflow-x': 'auto', 'overflow-y': 'auto' });
            }
        }

        function setScroller() {
            if (options.frozenColumn > -1) {
                $headerScrollContainer = $headerScrollerR;
                $viewportScrollContainerX = $viewportScrollContainerY = $viewportTopR;
            } else {
                $headerScrollContainer = $headerScrollerL;
                $viewportScrollContainerX = $viewportScrollContainerY = $viewportTopL;
            }
        }

        function measureCellPaddingAndBorder() {
            var el;
            var h = ["borderLeftWidth", "borderRightWidth", "paddingLeft", "paddingRight"];
            var v = ["borderTopWidth", "borderBottomWidth", "paddingTop", "paddingBottom"];

            el = $("<div class='ui-state-default slick-header-column' style='visibility:hidden'>-</div>").appendTo($headers);
            headerColumnWidthDiff = headerColumnHeightDiff = 0;
            $.each(h, function (n, val) {
                headerColumnWidthDiff += parseFloat(el.css(val)) || 0;
            });
            $.each(v, function (n, val) {
                headerColumnHeightDiff += parseFloat(el.css(val)) || 0;
            });
            el.remove();

            var r = $("<div class='slick-row' />").appendTo($canvas);
            el = $("<div class='slick-cell' id='' style='visibility:hidden'>-</div>").appendTo(r);
            cellWidthDiff = cellHeightDiff = 0;
            $.each(h, function (n, val) {
                cellWidthDiff += parseFloat(el.css(val)) || 0;
            });
            $.each(v, function (n, val) {
                cellHeightDiff += parseFloat(el.css(val)) || 0;
            });
            r.remove();

            absoluteColumnMinWidth = Math.max(headerColumnWidthDiff, cellWidthDiff);
        }

        function createCssRules() {
            //window.console.log("create rules");
            $style = $("<style type='text/css' rel='stylesheet' />").appendTo($("head"));
            var rowHeight = (options.rowHeight - cellHeightDiff);
            var rules = [
                "." + uid + " .slick-header-column { left: 1000px; }",
                "." + uid + " .slick-top-panel { height:" + options.topPanelHeight + "px; }",
                "." + uid + " .slick-cell { height:" + rowHeight + "px; }",
                "." + uid + " .slick-row { height:" + options.rowHeight + "px; }"
            ];

            for (var i = 0; i < columns.length; i++) {
                rules.push("." + uid + " .l" + i + " { }");
                rules.push("." + uid + " .r" + i + " { }");
            }

            if ($style[0].styleSheet) { // IE
                try{
                    $style[0].styleSheet.cssText = rules.join(" ");
                } catch (e) {
                    if (e.message.indexOf('参数无效') != -1) {
                        var msg = "页面加载超出限定范围31，请您减少底部Tab页数目！";
                        Sys.alert(msg);
                        logError(e, msg);
                    }
                }
            } else {
                $style[0].appendChild(document.createTextNode(rules.join(" ")));
            }
        }

        function getColumnCssRules(idx) {
            if (!stylesheet) {
                var sheets = document.styleSheets;
                var i;
                for (i = 0; i < sheets.length; i++) {
                    if ((sheets[i].ownerNode || sheets[i].owningElement) == $style[0]) {
                        stylesheet = sheets[i];
                        break;
                    }
                }

                if (!stylesheet && i == 31 && sheets.length == 31) {
                    //throw new Error("Cannot find stylesheet. index: " + i + " length: " + sheets.length);
                    var msg = "页面加载超出限定范围31，请您减少底部Tab页数目！";
                    Sys.alert(msg);
                    throw new Error(msg);
                }

                // find and cache column CSS rules
                columnCssRulesL = [];
                columnCssRulesR = [];
                var cssRules = (stylesheet.cssRules || stylesheet.rules);
                var matches, columnIdx;
                for (var i = 0; i < cssRules.length; i++) {
                    var selector = cssRules[i].selectorText;
                    if (matches = /\.l\d+/.exec(selector)) {
                        columnIdx = parseInt(matches[0].substr(2, matches[0].length - 2), 10);
                        columnCssRulesL[columnIdx] = cssRules[i];
                    } else if (matches = /\.r\d+/.exec(selector)) {
                        columnIdx = parseInt(matches[0].substr(2, matches[0].length - 2), 10);
                        columnCssRulesR[columnIdx] = cssRules[i];
                    }
                }
            }

            return {
                "left": columnCssRulesL[idx],
                "right": columnCssRulesR[idx]
            };
        }

        function removeCssRules() {
            $style.remove();
            stylesheet = null;
        }

        function destroy() {
            //window.console.log("destroy");
            getEditorLock().cancelCurrentEdit();

            trigger(self.onBeforeDestroy, {});

            var i = plugins.length;
            while (i--) {
                unregisterPlugin(plugins[i]);
            }

            if (options.enableColumnReorder) {
                $headers.filter(":ui-sortable").sortable("destroy");
            }

            //if (options.enableColumnReorder && $headers.sortable) {
            //    $headers.sortable("destroy");
            //}

            unbindAncestorScrollEvents();
            $container.unbind(".slickgrid");
            removeCssRules();

            $canvas.unbind("draginit dragstart dragend drag");
            $container.empty().removeClass(uid);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////
        // General

        function trigger(evt, args, e) {
            e = e || new Slick.EventData();
            args = args || {};
            args.grid = self;
            return evt.notify(args, e, self);
        }

        function getEditorLock() {
            return options.editorLock;
        }

        function getEditController() {
            return editController;
        }

        function getColumnIndex(id) {
            return columnsById[id];
        }

        function autosizeColumns() {
            var i, c, widths = [],
                shrinkLeeway = 0,
                total = 0,
                prevTotal, availWidth = viewportHasVScroll ? viewportW - scrollbarDimensions.width : viewportW;

            for (i = 0; i < columns.length; i++) {
                c = columns[i];
                widths.push(c.width);
                total += c.width;
                if (c.resizable) {
                    shrinkLeeway += c.width - Math.max(c.minWidth, absoluteColumnMinWidth);
                }
            }

            // shrink
            prevTotal = total;
            while (total > availWidth && shrinkLeeway) {
                var shrinkProportion = (total - availWidth) / shrinkLeeway;
                for (i = 0; i < columns.length && total > availWidth; i++) {
                    c = columns[i];
                    var width = widths[i];
                    if (!c.resizable || width <= c.minWidth || width <= absoluteColumnMinWidth) {
                        continue;
                    }
                    var absMinWidth = Math.max(c.minWidth, absoluteColumnMinWidth);
                    var shrinkSize = Math.floor(shrinkProportion * (width - absMinWidth)) || 1;
                    shrinkSize = Math.min(shrinkSize, width - absMinWidth);
                    total -= shrinkSize;
                    shrinkLeeway -= shrinkSize;
                    widths[i] -= shrinkSize;
                }
                if (prevTotal == total) {  // avoid infinite loop
                    break;
                }
                prevTotal = total;
            }

            // grow
            prevTotal = total;
            while (total < availWidth) {
                var growProportion = availWidth / total;
                for (i = 0; i < columns.length && total < availWidth; i++) {
                    c = columns[i];
                    if (!c.resizable || c.maxWidth <= c.width) {
                        continue;
                    }
                    var growSize = Math.min(Math.floor(growProportion * c.width) - c.width, (c.maxWidth - c.width) || 1000000) || 1;
                    total += growSize;
                    widths[i] += growSize;
                }
                if (prevTotal == total) {  // avoid infinite loop
                    break;
                }
                prevTotal = total;
            }

            var reRender = false;
            for (i = 0; i < columns.length; i++) {
                if (columns[i].rerenderOnResize && columns[i].width != widths[i]) {
                    reRender = true;
                }
                columns[i].width = widths[i];
            }

            applyColumnHeaderWidths();
            updateCanvasWidth(true);
            if (reRender) {
                invalidateAllRows();
                render();
            }
        }

        function applyColumnHeaderWidths() {
            if (!initialized) {
                return;
            }
            var h;
            for (var i = 0, headers = $headers.children(), ii = headers.length; i < ii; i++) {
                h = $(headers[i]);
                if (h.width() !== columns[i].width - headerColumnWidthDiff) {
                    h.width(columns[i].width - headerColumnWidthDiff);
                }
            }

            updateColumnCaches();
        }

        function applyColumnWidths() {
            var x = 0,
                w, rule;
            for (var i = 0; i < columns.length; i++) {
                w = columns[i].width;

                rule = getColumnCssRules(i);
                rule.left.style.left = x + "px";
                rule.right.style.right = (((options.frozenColumn != -1 && i > options.frozenColumn) ? canvasWidthR : canvasWidthL) - x - w) + "px";

                // If this column is frozen, reset the css left value since the
                // column starts in a new viewport.
                if (options.frozenColumn == i) {
                    x = 0;
                } else {
                    x += columns[i].width;
                }
            }
        }

        function setSortColumn(columnId, ascending) {
            setSortColumns([{
                columnId: columnId,
                sortAsc: ascending
            }]);
        }

        function setSortColumns(cols) {
            sortColumns = cols;

            var headerColumnEls = $headers.children();
            headerColumnEls.removeClass("slick-header-column-sorted").find(".slick-sort-indicator").removeClass("slick-sort-indicator-asc slick-sort-indicator-desc");

            $.each(sortColumns, function (i, col) {
                if (col.sortAsc == null) {
                    col.sortAsc = true;
                }
                var columnIndex = getColumnIndex(col.columnId);
                if (columnIndex != null) {
                    headerColumnEls.eq(columnIndex).addClass("slick-header-column-sorted").find(".slick-sort-indicator").addClass(
                    col.sortAsc ? "slick-sort-indicator-asc" : "slick-sort-indicator-desc");
                }
            });
        }

        function getSortColumns() {
            return sortColumns;
        }

        function handleSelectedRangesChanged(e, ranges) {
            selectedRows = [];
            var hash = {};
            for (var i = 0; i < ranges.length; i++) {
                for (var j = ranges[i].fromRow; j <= ranges[i].toRow; j++) {
                    if (!hash[j]) {  // prevent duplicates
                        selectedRows.push(j);
                        hash[j] = {};
                    }
                    for (var k = ranges[i].fromCell; k <= ranges[i].toCell; k++) {
                        if (canCellBeSelected(j, k)) {
                            hash[j][columns[k].id] = options.selectedCellCssClass;
                        }
                    }
                }
            }

            setCellCssStyles(options.selectedCellCssClass, hash);

            trigger(self.onSelectedRowsChanged, {
                rows: getSelectedRows()
            }, e);
        }

        function getColumns() {
            return columns;
        }

        function updateColumnCaches() {
            // Pre-calculate cell boundaries.
            columnPosLeft = [];
            columnPosRight = [];
            var x = 0;
            for (var i = 0, ii = columns.length; i < ii; i++) {
                columnPosLeft[i] = x;
                columnPosRight[i] = x + columns[i].width;

                if (options.frozenColumn == i) {
                    x = 0;
                } else {
                    x += columns[i].width;
                }
            }
        }

        function setColumns(columnDefinitions) {
            columns = columnDefinitions;

            columnsById = {};
            for (var i = 0; i < columns.length; i++) {
                var m = columns[i] = $.extend({}, columnDefaults, columns[i]);
                columnsById[m.id] = i;
                if (m.minWidth && m.width < m.minWidth) {
                    m.width = m.minWidth;
                }
                if (m.maxWidth && m.width > m.maxWidth) {
                    m.width = m.maxWidth;
                }
            }

            updateColumnCaches();

            if (initialized) {
                setPaneVisibility();
                setOverflow();
                invalidateAllRows();
                createColumnHeaders();
                removeCssRules();
                createCssRules();
                resizeCanvas();
                updateCanvasWidth();
                applyColumnWidths();
                handleScroll();
            }
        }

        function getOptions() {
            return options;
        }

        function setOptions(args) {
            if (!getEditorLock().commitCurrentEdit()) {
                return;
            }

            makeActiveCellNormal();

            if (options.enableAddRow !== args.enableAddRow) {
                invalidateRow(getDataLength());
            }

            options = $.extend(options, args);
            validateAndEnforceOptions();

            //$viewport.css("overflow-y", options.autoHeight ? "hidden" : "auto");
            render();
        }

        function validateAndEnforceOptions() {
            if (options.autoHeight) {
                options.leaveSpaceForNewRows = false;
            }
        }

        function setData(newData, scrollToTop) {
            data = newData;
            invalidateAllRows();
            updateRowCount();
            if (scrollToTop) {
                scrollTo(0);
            }
        }

        function updateData() {
            if (currentEditor && getEditorLock() && getEditorLock().isActive()) {
                getEditorLock().commitCurrentEdit();
            }
        }

        function getData() {
            if (data && data.getItems)
                return data.getItems();
            else
                return data;
        }

        function getDataLength() {
            if (data && data.getLength) {
                return data.getLength();
            } else if (data) {
                return data.length;
            } else {
                return 0;
            }
        }

        function getDataItem(i) {
            if (data && data.getItem) {
                return data.getItem(i);
            } else if (data) {
                return data[i];
            } else {
                return null;
            }
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////
        // Rendering / Scrolling

        function scrollTo(y) {
            y = Math.max(y, 0);
            y = Math.min(y, th - $viewportScrollContainerY.height() + ((viewportHasHScroll || options.frozenColumn > -1) ? scrollbarDimensions.height : 0));

            var oldOffset = offset;

            page = Math.min(n - 1, Math.floor(y / ph));
            offset = Math.round(page * cj);
            var newScrollTop = y - offset;

            if (offset != oldOffset) {
                var range = getVisibleRange(newScrollTop);
                cleanupRows(range);
                updateRowPositions();
            }

            if (prevScrollTop != newScrollTop) {
                vScrollDir = (prevScrollTop + oldOffset < newScrollTop + offset) ? 1 : -1;

                lastRenderedScrollTop = (scrollTop = prevScrollTop = newScrollTop);

                if (options.frozenColumn > -1) {
                    $viewportTopL[0].scrollTop = newScrollTop;
                }

                $viewportScrollContainerY[0].scrollTop = newScrollTop;

                trigger(self.onViewportChanged, {});
            }
        }

        function defaultFormatter(row, cell, value, columnDef, dataContext) {
            if (value == null) {
                return "";
            } else {
                return value.toString().replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            }
        }

        function getFormatter(row, column) {
            var rowMetadata = data.getItemMetadata && data.getItemMetadata(row);

            // look up by id, then index
            var columnOverrides = rowMetadata && rowMetadata.columns && (rowMetadata.columns[column.id] || rowMetadata.columns[getColumnIndex(column.id)]);

            return (columnOverrides && columnOverrides.formatter) || (rowMetadata && rowMetadata.formatter) || column.formatter || (options.formatterFactory && options.formatterFactory.getFormatter(column)) || options.defaultFormatter;
        }

        function getEditor(row, cell) {
            var column = columns[cell];
            var rowMetadata = data.getItemMetadata && data.getItemMetadata(row);
            var columnMetadata = rowMetadata && rowMetadata.columns;

            if (columnMetadata && columnMetadata[column.id] && columnMetadata[column.id].editor !== undefined) {
                return columnMetadata[column.id].editor;
            }
            if (columnMetadata && columnMetadata[cell] && columnMetadata[cell].editor !== undefined) {
                return columnMetadata[cell].editor;
            }

            return column.editor || (options.editorFactory && options.editorFactory.getEditor(column));
        }

        function getDataItemValueForColumn(item, columnDef) {
            if (options.dataItemColumnValueExtractor) {
                return options.dataItemColumnValueExtractor(item, columnDef);
            }
            return item[columnDef.field];
        }

        function appendRowHtml(stringArrayL, stringArrayR, row, range) {
            var d = getDataItem(row);
            var dataLoading = row < getDataLength() && !d;
            var rowCss = "slick-row" +
                (dataLoading ? " loading" : "") +
                (row === activeRow ? " active" : "") +
                (row % 2 == 1 ? " odd" : " even");

            var metadata = data.getItemMetadata && data.getItemMetadata(row);

            if (metadata && metadata.cssClasses) {
                rowCss += " " + metadata.cssClasses;
            }

            var rowHtml = "<div class='ui-widget-content " + rowCss + "' style='top:"
                            + (options.rowHeight * row - offset)
                            + "px'>";

            stringArrayL.push(rowHtml);

            if (options.frozenColumn > -1) {
                stringArrayR.push(rowHtml);
            }

            var colspan, m;
            for (var i = 0, ii = columns.length; i < ii; i++) {
                m = columns[i];
                colspan = 1;
                if (metadata && metadata.columns) {
                    var columnData = metadata.columns[m.id] || metadata.columns[i];
                    colspan = (columnData && columnData.colspan) || 1;
                    if (colspan === "*") {
                        colspan = ii - i;
                    }
                }

                // Do not render cells outside of the viewport.
                if (columnPosRight[Math.min(ii - 1, i + colspan - 1)] > range.leftPx) {
                    if (columnPosLeft[i] > range.rightPx) {
                        // All columns to the right are outside the range.
                        break;
                    }

                    if ((options.frozenColumn > -1) && (i > options.frozenColumn)) {
                        appendCellHtml(stringArrayR, row, i, colspan);
                    } else {
                        appendCellHtml(stringArrayL, row, i, colspan);
                    }
                } else if ((options.frozenColumn > -1) && (i <= options.frozenColumn)) {
                    appendCellHtml(stringArrayL, row, i, colspan);
                }

                if (colspan > 1) {
                    i += (colspan - 1);
                }
            }

            stringArrayL.push("</div>");

            if (options.frozenColumn > -1) {
                stringArrayR.push("</div>");
            }
        }

        function appendCellHtml(stringArray, row, cell, colspan) {
            var m = columns[cell];
            var d = getDataItem(row);
            var cellCss = "slick-cell l" + cell + " r" + Math.min(columns.length - 1, cell + colspan - 1)
                        + (m.cssClass ? " " + m.cssClass : "");
            if (row === activeRow && cell === activeCell) {
                cellCss += (" active");
            }

            // TODO:  merge them together in the setter
            for (var key in cellCssClasses) {
                if (cellCssClasses[key][row] && cellCssClasses[key][row][m.id]) {
                    cellCss += (" " + cellCssClasses[key][row][m.id]);
                }
            }

            stringArray.push("<div class='" + cellCss + "'>");

            // if there is a corresponding row (if not, this is the Add New row or this data hasn't been loaded yet)
            if (d) {
                var value = getDataItemValueForColumn(d, m);
                stringArray.push(getFormatter(row, m)(row, cell, value, m, d));
            }

            stringArray.push("</div>");

            rowsCache[row].cellRenderQueue.push(cell);
            rowsCache[row].cellColSpans[cell] = colspan;
        }


        function cleanupRows(rangeToKeep) {
            for (var i in rowsCache) {
                var removeFrozenRow = true;

                if (((i = parseInt(i, 10)) !== activeRow)
                     && (i < rangeToKeep.top || i > rangeToKeep.bottom)
                     && (removeFrozenRow)
                   ) {
                    removeRowFromCache(i);
                }
            }
        }

        function invalidate() {
            updateRowCount();
            invalidateAllRows();
            render();
        }

        function invalidateAllRows() {
            if (currentEditor) {
                makeActiveCellNormal();
            }
            for (var row in rowsCache) {
                removeRowFromCache(row);
            }
        }

        function removeRowFromCache(row) {
            var cacheEntry = rowsCache[row];
            if (!cacheEntry) {
                return;
            }

            cacheEntry.rowNode[0].parentElement.removeChild(cacheEntry.rowNode[0]);

            // Remove the row from the right viewport
            if (cacheEntry.rowNode[1]) {
                cacheEntry.rowNode[1].parentElement.removeChild(cacheEntry.rowNode[1]);
            }

            delete rowsCache[row];
            delete postProcessedRows[row];
            renderedRows--;
            counter_rows_removed++;
        }

        function invalidateRows(rows) {
            var i, rl;
            if (!rows || !rows.length) {
                return;
            }
            vScrollDir = 0;
            for (i = 0, rl = rows.length; i < rl; i++) {
                if (currentEditor && activeRow === rows[i]) {
                    makeActiveCellNormal();
                }
                if (rowsCache[rows[i]]) {
                    removeRowFromCache(rows[i]);
                }
            }
        }

        function invalidateRow(row) {
            invalidateRows([row]);
        }

        function updateCell(row, cell) {
            var cellNode = getCellNode(row, cell);
            if (!cellNode) {
                return;
            }

            var m = columns[cell],
                d = getDataItem(row);
            if (currentEditor && activeRow === row && activeCell === cell) {
                currentEditor.loadValue(d);
            } else {
                cellNode.innerHTML = d ? getFormatter(row, m)(row, cell, getDataItemValueForColumn(d, m), m, d) : "";
                invalidatePostProcessingResults(row);
            }
        }

        function updateRow(row) {
            var cacheEntry = rowsCache[row];
            if (!cacheEntry) {
                return;
            }

            ensureCellNodesInRowsCache(row);

            for (var columnIdx in cacheEntry.cellNodesByColumnIdx) {
                if (!cacheEntry.cellNodesByColumnIdx.hasOwnProperty(columnIdx)) {
                    continue;
                }

                columnIdx = columnIdx | 0;
                var m = columns[columnIdx],
                    d = getDataItem(row),
                    node = cacheEntry.cellNodesByColumnIdx[columnIdx][0];

                if (row === activeRow && columnIdx === activeCell && currentEditor) {
                    currentEditor.loadValue(d);
                } else if (d) {
                    node.innerHTML = getFormatter(row, m)(row, columnIdx, getDataItemValueForColumn(d, m), m, d);
                } else {
                    node.innerHTML = "";
                }
            }

            invalidatePostProcessingResults(row);
        }

        function getViewportHeight() {
            if (options.autoHeight) {
                viewportH = options.rowHeight
                          * (getDataLength()
                              + (options.enableAddRow ? 1 : 0)
                            )
                          + ((options.frozenColumn == -1) ? $headers.outerHeight() : 0);
            } else {

                viewportH = parseFloat($.css($container[0], "height", true))
                          - parseFloat($.css($container[0], "paddingTop", true))
                          - parseFloat($.css($container[0], "paddingBottom", true))
                          - parseFloat($.css($headerScroller[0], "height"))
                          - getVBoxDelta($headerScroller)
                          - topPanelH;
            }

            numVisibleRows = Math.ceil(viewportH / options.rowHeight);
        }

        function getViewportWidth() {
            viewportW = parseFloat($.css($container[0], "width", true));
        }

        function resizeCanvas() {
            if (!initialized) {
                return;
            }

            paneTopH = 0
            viewportTopH = 0

            getViewportWidth();
            getViewportHeight();

            paneTopH = viewportH;

            // The top pane includes the top panel and the header row
            paneTopH += topPanelH;

            if (options.frozenColumn > -1 && options.autoHeight) {
                paneTopH += scrollbarDimensions.height;
            }

            // The top viewport does not contain the top panel or header row
            viewportTopH = paneTopH - topPanelH;

            if (options.autoHeight) {
                if (options.frozenColumn > -1) {
                    $container.height(
                        paneTopH
                        + parseFloat($.css($headerScrollerL[0], "height"))
                    );
                }

                $paneTopL.css('position', 'relative');
            }

            $paneTopL.css({
                'top': $paneHeaderL.height()
                , 'height': paneTopH
            });

            var paneBottomTop = $paneTopL.position().top
                                + paneTopH;

            $viewportTopL.height(viewportTopH);

            if (options.frozenColumn > -1) {
                $paneTopR.css({
                    'top': $paneHeaderL.height()
                   , 'height': paneTopH
                });

                $viewportTopR.height(viewportTopH);

            }

            $viewportTopR.height(viewportTopH);


            if (options.forceFitColumns) {
                autosizeColumns();
            }

            updateRowCount();
            handleScroll();
            // Since the width has changed, force the render() to reevaluate virtually rendered cells.
            lastRenderedScrollLeft = -1;
            render();
        }

        function updateRowCount() {
            if (!initialized) {
                return;
            }

            var oldH = $canvasTopL.height();

            numberOfRows = getDataLength() + (options.enableAddRow ? 1 : 0) + (options.leaveSpaceForNewRows ? numVisibleRows - 1 : 0);


            var tempViewportH = $viewportScrollContainerY.height();
            var oldViewportHasVScroll = viewportHasVScroll;
            // with autoHeight, we do not need to accommodate the vertical scroll bar
            viewportHasVScroll = !options.autoHeight && (numberOfRows * options.rowHeight > tempViewportH);

            // remove the rows that are now outside of the data range
            // this helps avoid redundant calls to .removeRow() when the size of
            // the data decreased by thousands of rows
            var l = options.enableAddRow ? getDataLength() : getDataLength() - 1;
            for (var i in rowsCache) {
                if (i >= l) {
                    removeRowFromCache(i);
                }
            }

            th = Math.max(options.rowHeight * numberOfRows, tempViewportH - scrollbarDimensions.height);

            if (activeCellNode && activeRow > l) {
                resetActiveCell();
            }

            if (th < maxSupportedCssHeight) {
                // just one page
                h = ph = th;
                n = 1;
                cj = 0;
            } else {
                // break into pages
                h = maxSupportedCssHeight;
                ph = h / 100;
                n = Math.floor(th / ph);
                cj = (th - h) / (n - 1);
            }

            if (h !== oldH) {
                $canvasTopL.css("height", h);
                $canvasTopR.css("height", h);

                scrollTop = $viewportScrollContainerY[0].scrollTop;
            }

            var oldScrollTopInRange = (scrollTop + offset <= th - tempViewportH);

            if (th == 0 || scrollTop == 0) {
                page = offset = 0;
            } else if (oldScrollTopInRange) {
                // maintain virtual position
                scrollTo(scrollTop + offset);
            } else {
                // scroll to bottom
                scrollTo(th - tempViewportH);
            }

            if (h != oldH && options.autoHeight) {
                resizeCanvas();
            }

            if (options.forceFitColumns && oldViewportHasVScroll != viewportHasVScroll) {
                autosizeColumns();
            }
            updateCanvasWidth(false);
        }

        function getVisibleRange(viewportTop, viewportLeft) {
            if (viewportTop == null) {
                viewportTop = scrollTop;
            }
            if (viewportLeft == null) {
                viewportLeft = scrollLeft;
            }

            return {
                top: Math.floor((viewportTop + offset) / options.rowHeight),
                bottom: Math.ceil((viewportTop + offset + viewportH) / options.rowHeight),
                leftPx: viewportLeft,
                rightPx: viewportLeft + viewportW
            };
        }

        function getRenderedRange(viewportTop, viewportLeft) {
            var range = getVisibleRange(viewportTop, viewportLeft);
            var buffer = Math.round(viewportH / options.rowHeight);
            var minBuffer = 3;

            if (vScrollDir == -1) {
                range.top -= buffer;
                range.bottom += minBuffer;
            } else if (vScrollDir == 1) {
                range.top -= minBuffer;
                range.bottom += buffer;
            } else {
                range.top -= minBuffer;
                range.bottom += minBuffer;
            }

            range.top = Math.max(0, range.top);
            range.bottom = Math.min(options.enableAddRow ? getDataLength() : getDataLength() - 1, range.bottom);

            range.leftPx -= viewportW;
            range.rightPx += viewportW;

            range.leftPx = Math.max(0, range.leftPx);
            range.rightPx = Math.min(canvasWidth, range.rightPx);

            return range;
        }

        function ensureCellNodesInRowsCache(row) {
            var cacheEntry = rowsCache[row];
            if (cacheEntry) {
                if (cacheEntry.cellRenderQueue.length) {
                    var $lastNode = cacheEntry.rowNode.children().last();
                    while (cacheEntry.cellRenderQueue.length) {
                        var columnIdx = cacheEntry.cellRenderQueue.pop();

                        cacheEntry.cellNodesByColumnIdx[columnIdx] = $lastNode;
                        $lastNode = $lastNode.prev();

                        // Hack to retrieve the frozen columns because
                        if ($lastNode.length == 0) {
                            $lastNode = $(cacheEntry.rowNode[0]).children().last();
                        }
                    }
                }
            }
        }

        function cleanUpCells(range, row) {
            // Ignore frozen rows
            var totalCellsRemoved = 0;
            var cacheEntry = rowsCache[row];

            // Remove cells outside the range.
            var cellsToRemove = [];
            for (var i in cacheEntry.cellNodesByColumnIdx) {
                // I really hate it when people mess with Array.prototype.
                if (!cacheEntry.cellNodesByColumnIdx.hasOwnProperty(i)) {
                    continue;
                }

                // This is a string, so it needs to be cast back to a number.
                i = i | 0;

                // Ignore frozen columns
                if (i <= options.frozenColumn) {
                    continue;
                }

                var colspan = cacheEntry.cellColSpans[i];
                if (columnPosLeft[i] > range.rightPx || columnPosRight[Math.min(columns.length - 1, i + colspan - 1)] < range.leftPx) {
                    if (!(row == activeRow && i == activeCell)) {
                        cellsToRemove.push(i);
                    }
                }
            }

            var cellToRemove;
            while ((cellToRemove = cellsToRemove.pop()) != null) {
                cacheEntry.cellNodesByColumnIdx[cellToRemove][0].parentElement.removeChild(cacheEntry.cellNodesByColumnIdx[cellToRemove][0]);
                delete cacheEntry.cellColSpans[cellToRemove];
                delete cacheEntry.cellNodesByColumnIdx[cellToRemove];
                if (postProcessedRows[row]) {
                    delete postProcessedRows[row][cellToRemove];
                }
                totalCellsRemoved++;
            }
        }

        function cleanUpAndRenderCells(range) {
            var cacheEntry;
            var stringArray = [];
            var processedRows = [];
            var cellsAdded;
            var totalCellsAdded = 0;
            var colspan;

            for (var row = range.top; row <= range.bottom; row++) {
                cacheEntry = rowsCache[row];
                if (!cacheEntry) {
                    continue;
                }

                // cellRenderQueue populated in renderRows() needs to be cleared first
                ensureCellNodesInRowsCache(row);

                cleanUpCells(range, row);

                // Render missing cells.
                cellsAdded = 0;

                var metadata = data.getItemMetadata && data.getItemMetadata(row);
                metadata = metadata && metadata.columns;

                // TODO:  shorten this loop (index? heuristics? binary search?)
                for (var i = 0, ii = columns.length; i < ii; i++) {
                    // Cells to the right are outside the range.
                    if (columnPosLeft[i] > range.rightPx) {
                        break;
                    }

                    // Already rendered.
                    if ((colspan = cacheEntry.cellColSpans[i]) != null) {
                        i += (colspan > 1 ? colspan - 1 : 0);
                        continue;
                    }

                    colspan = 1;
                    if (metadata) {
                        var columnData = metadata[columns[i].id] || metadata[i];
                        colspan = (columnData && columnData.colspan) || 1;
                        if (colspan === "*") {
                            colspan = ii - i;
                        }
                    }

                    if (columnPosRight[Math.min(ii - 1, i + colspan - 1)] > range.leftPx) {
                        appendCellHtml(stringArray, row, i, colspan);
                        cellsAdded++;
                    }

                    i += (colspan > 1 ? colspan - 1 : 0);
                }

                if (cellsAdded) {
                    totalCellsAdded += cellsAdded;
                    processedRows.push(row);
                }
            }

            if (!stringArray.length) {
                return;
            }

            var x = document.createElement("div");
            x.innerHTML = stringArray.join("");

            var processedRow;
            var $node;
            while ((processedRow = processedRows.pop()) != null) {
                cacheEntry = rowsCache[processedRow];
                var columnIdx;
                while ((columnIdx = cacheEntry.cellRenderQueue.pop()) != null) {
                    $node = $(x).children().last();

                    if ((options.frozenColumn > -1) && (columnIdx > options.frozenColumn)) {
                        $(cacheEntry.rowNode[1]).append($node);
                    } else {
                        $(cacheEntry.rowNode[0]).append($node);
                    }

                    cacheEntry.cellNodesByColumnIdx[columnIdx] = $node;
                }
            }
        }

        function renderRows(range) {
            var stringArrayL = [],
                stringArrayR = [],
                rows = [],
                needToReselectCell = false;

            for (var i = range.top; i <= range.bottom; i++) {
                if (rowsCache[i]) {
                    continue;
                }
                renderedRows++;
                rows.push(i);

                // Create an entry right away so that appendRowHtml() can
                // start populatating it.
                rowsCache[i] = {
                    "rowNode": null,

                    // ColSpans of rendered cells (by column idx).
                    // Can also be used for checking whether a cell has been rendered.
                    "cellColSpans": [],

                    // Cell nodes (by column idx).  Lazy-populated by ensureCellNodesInRowsCache().
                    "cellNodesByColumnIdx": [],

                    // Column indices of cell nodes that have been rendered, but not yet indexed in
                    // cellNodesByColumnIdx.  These are in the same order as cell nodes added at the
                    // end of the row.
                    "cellRenderQueue": []
                };

                appendRowHtml(stringArrayL, stringArrayR, i, range);
                if (activeCellNode && activeRow === i) {
                    needToReselectCell = true;
                }
                counter_rows_rendered++;
            }

            if (!rows.length) {
                return;
            }

            var x = document.createElement("div"),
                xRight = document.createElement("div");

            x.innerHTML = stringArrayL.join("");
            xRight.innerHTML = stringArrayR.join("");

            for (var i = 0, ii = rows.length; i < ii; i++) {
                if (options.frozenColumn > -1) {
                    rowsCache[rows[i]].rowNode = $()
                        .add($(x.firstChild).appendTo($canvasTopL))
                        .add($(xRight.firstChild).appendTo($canvasTopR));
                } else {
                    rowsCache[rows[i]].rowNode = $()
                        .add($(x.firstChild).appendTo($canvasTopL));
                }
            }

            if (needToReselectCell) {
                activeCellNode = getCellNode(activeRow, activeCell);
            }
        }

        function startPostProcessing() {
            if (!options.enableAsyncPostRender) {
                return;
            }
            clearTimeout(h_postrender);
            h_postrender = setTimeout(asyncPostProcessRows, options.asyncPostRenderDelay);
        }

        function invalidatePostProcessingResults(row) {
            delete postProcessedRows[row];
            postProcessFromRow = Math.min(postProcessFromRow, row);
            postProcessToRow = Math.max(postProcessToRow, row);
            startPostProcessing();
        }

        function updateRowPositions() {
            for (var row in rowsCache) {
                rowsCache[row].rowNode.css('top', (row * options.rowHeight - offset) + "px");
            }
        }

        function render() {
            if (!initialized) {
                return;
            }
            var visible = getVisibleRange();
            var rendered = getRenderedRange();

            // remove rows no longer in the viewport
            cleanupRows(rendered);

            // add new rows & missing cells in existing rows
            if (lastRenderedScrollLeft != scrollLeft) {
                cleanUpAndRenderCells(rendered);
            }

            // render missing rows
            renderRows(rendered);

            postProcessFromRow = visible.top;
            postProcessToRow = Math.min(options.enableAddRow ? getDataLength() : getDataLength() - 1, visible.bottom);
            startPostProcessing();

            lastRenderedScrollTop = scrollTop;
            lastRenderedScrollLeft = scrollLeft;
            h_render = null;
        }

        function handleMouseWheel(event, delta, deltaX, deltaY) {
            scrollTop = $viewportScrollContainerY[0].scrollTop - (deltaY * options.rowHeight);
            scrollLeft = $viewportScrollContainerX[0].scrollLeft + (deltaX * 10);
            _handleScroll();
            event.preventDefault();
        }

        function handleScroll() {
            scrollTop = $viewportScrollContainerY[0].scrollTop;
            scrollLeft = $viewportScrollContainerX[0].scrollLeft;

            _handleScroll();
        }

        function _handleScroll() {
            var maxScrollDistanceY = $viewportScrollContainerY[0].scrollHeight - $viewportScrollContainerY[0].clientHeight;
            var maxScrollDistanceX = $viewportScrollContainerY[0].scrollWidth - $viewportScrollContainerY[0].clientWidth;

            // Ceiling the max scroll values
            if (scrollTop > maxScrollDistanceY) {
                scrollTop = maxScrollDistanceY;
            }
            if (scrollLeft > maxScrollDistanceX) {
                scrollLeft = maxScrollDistanceX;
            }

            var vScrollDist = Math.abs(scrollTop - prevScrollTop);
            var hScrollDist = Math.abs(scrollLeft - prevScrollLeft);

            if (hScrollDist) {
                prevScrollLeft = scrollLeft;
                $headerScrollContainer[0].scrollLeft = scrollLeft;
                $viewportScrollContainerX[0].scrollLeft = scrollLeft;
            }

            if (vScrollDist) {
                vScrollDir = prevScrollTop < scrollTop ? 1 : -1;
                prevScrollTop = scrollTop

                $viewportScrollContainerY[0].scrollTop = scrollTop;
                if (options.frozenColumn > -1) {
                    $viewportTopL[0].scrollTop = scrollTop;
                }

                // switch virtual pages if needed
                if (vScrollDist < viewportH) {
                    scrollTo(scrollTop + offset);
                } else {
                    var oldOffset = offset;
                    if (h == viewportH) {
                        page = 0;
                    } else {
                        page = Math.min(n - 1, Math.floor(scrollTop * ((th - viewportH) / (h - viewportH)) * (1 / ph)));
                    }
                    offset = Math.round(page * cj);
                    if (oldOffset != offset) {
                        invalidateAllRows();
                    }
                }
            }

            if (hScrollDist || vScrollDist) {
                if (h_render) {
                    clearTimeout(h_render);
                }

                if (Math.abs(lastRenderedScrollTop - scrollTop) > 20 ||
                    Math.abs(lastRenderedScrollLeft - scrollLeft) > 20) {
                    if (options.forceSyncScrolling || (
                        Math.abs(lastRenderedScrollTop - scrollTop) < viewportH &&
                        Math.abs(lastRenderedScrollLeft - scrollLeft) < viewportW)) {
                        render();
                    } else {
                        h_render = setTimeout(render, 50);
                    }

                    trigger(self.onViewportChanged, {});
                }
            }

            trigger(self.onScroll, {
                scrollLeft: scrollLeft,
                scrollTop: scrollTop
            });
        }

        function asyncPostProcessRows() {
            while (postProcessFromRow <= postProcessToRow) {
                var row = (vScrollDir >= 0) ? postProcessFromRow++ : postProcessToRow--;
                var cacheEntry = rowsCache[row];
                if (!cacheEntry || row >= getDataLength()) {
                    continue;
                }

                if (!postProcessedRows[row]) {
                    postProcessedRows[row] = {};
                }

                ensureCellNodesInRowsCache(row);
                for (var columnIdx in cacheEntry.cellNodesByColumnIdx) {
                    if (!cacheEntry.cellNodesByColumnIdx.hasOwnProperty(columnIdx)) {
                        continue;
                    }

                    columnIdx = columnIdx | 0;

                    var m = columns[columnIdx];
                    if (m.asyncPostRender && !postProcessedRows[row][columnIdx]) {
                        var node = cacheEntry.cellNodesByColumnIdx[columnIdx];
                        if (node) {
                            m.asyncPostRender(node, row, getDataItem(row), m);
                        }
                        postProcessedRows[row][columnIdx] = true;
                    }
                }

                h_postrender = setTimeout(asyncPostProcessRows, options.asyncPostRenderDelay);
                return;
            }
        }

        function updateCellCssStylesOnRenderedRows(addedHash, removedHash) {
            var node, columnId, addedRowHash, removedRowHash;
            for (var row in rowsCache) {
                removedRowHash = removedHash && removedHash[row];
                addedRowHash = addedHash && addedHash[row];

                if (removedRowHash) {
                    for (columnId in removedRowHash) {
                        if (!addedRowHash || removedRowHash[columnId] != addedRowHash[columnId]) {
                            node = getCellNode(row, getColumnIndex(columnId));
                            if (node) {
                                $(node).removeClass(removedRowHash[columnId]);
                            }
                        }
                    }
                }

                if (addedRowHash) {
                    for (columnId in addedRowHash) {
                        if (!removedRowHash || removedRowHash[columnId] != addedRowHash[columnId]) {
                            node = getCellNode(row, getColumnIndex(columnId));
                            if (node) {
                                $(node).addClass(addedRowHash[columnId]);
                            }
                        }
                    }
                }
            }
        }

        function addCellCssStyles(key, hash) {
            if (cellCssClasses[key]) {
                throw "addCellCssStyles: cell CSS hash with key '" + key + "' already exists.";
            }

            cellCssClasses[key] = hash;
            updateCellCssStylesOnRenderedRows(hash, null);

            trigger(self.onCellCssStylesChanged, {
                "key": key,
                "hash": hash
            });
        }

        function removeCellCssStyles(key) {
            if (!cellCssClasses[key]) {
                return;
            }

            updateCellCssStylesOnRenderedRows(null, cellCssClasses[key]);
            delete cellCssClasses[key];

            trigger(self.onCellCssStylesChanged, {
                "key": key,
                "hash": null
            });
        }

        function setCellCssStyles(key, hash) {
            var prevHash = cellCssClasses[key];

            cellCssClasses[key] = hash;
            updateCellCssStylesOnRenderedRows(hash, prevHash);

            trigger(self.onCellCssStylesChanged, {
                "key": key,
                "hash": hash
            });
        }

        function getCellCssStyles(key) {
            return cellCssClasses[key];
        }

        function flashCell(row, cell, speed) {
            speed = speed || 100;
            if (rowsCache[row]) {
                var $cell = $(getCellNode(row, cell));

                function toggleCellClass(times) {
                    if (!times) {
                        return;
                    }
                    setTimeout(function () {
                        $cell.queue(function () {
                            $cell.toggleClass(options.cellFlashingCssClass).dequeue();
                            toggleCellClass(times - 1);
                        });
                    }, speed);
                }

                toggleCellClass(4);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // Interactivity

        function handleDragInit(e, dd) {
            var cell = getCellFromEvent(e);
            if (!cell || !cellExists(cell.row, cell.cell)) {
                return false;
            }

            retval = trigger(self.onDragInit, dd, e);
            if (e.isImmediatePropagationStopped()) {
                return retval;
            }

            // if nobody claims to be handling drag'n'drop by stopping immediate
            // propagation, cancel out of it
            return false;
        }

        function handleDragStart(e, dd) {
            //if there is active cell, commit it first
            if (currentEditor && getEditorLock() && getEditorLock().isActive()) {
                getEditorLock().commitCurrentEdit();
            }

            var cell = getCellFromEvent(e);
            if (!cell || !cellExists(cell.row, cell.cell)) {
                return false;
            }

            var retval = trigger(self.onDragStart, dd, e);
            if (e.isImmediatePropagationStopped()) {
                return retval;
            }

            return false;
        }

        function handleDrag(e, dd) {
            return trigger(self.onDrag, dd, e);
        }

        function handleDragEnd(e, dd) {
            trigger(self.onDragEnd, dd, e);
        }

        function handleKeyDown(e) {
            trigger(self.onKeyDown, {
                row: activeRow,
                cell: activeCell
            }, e);

            var handled = e.isImmediatePropagationStopped();
            if (!handled) {
                if (!e.shiftKey && !e.altKey && !e.ctrlKey) {
                    if (e.which == 27) {
                        if (!getEditorLock().isActive()) {
                            return; // no editing mode to cancel, allow bubbling and default processing (exit without cancelling the event)
                        }
                        cancelEditAndSetFocus();
                    } else if (e.which == 37) {
                        handled = navigateLeft();
                    } else if (e.which == 39) {
                        handled = navigateRight();
                    } else if (e.which == 38) {
                        handled = navigateUp();
                    } else if (e.which == 40) {
                        handled = navigateDown();
                    } else if (e.which == 9) {
                        handled = navigateNext();
                    } else if (e.which == 13) {
                        if (options.editable) {
                            if (currentEditor) {
                                // adding new row
                                if (activeRow === getDataLength()) {
                                    navigateDown();
                                } else {
                                    commitEditAndSetFocus();
                                }
                            } else {
                                if (getEditorLock().commitCurrentEdit()) {
                                    makeActiveCellEditable();
                                }
                                navigateNext();
                            }
                        }
                        handled = true;
                    }
                } else if (e.which == 9 && e.shiftKey && !e.ctrlKey && !e.altKey) {
                    handled = navigatePrev();
                }
                //else if (e.ctrlKey && e.keyCode == 67) {    //ctrl+c
                //    copytoClipboard();
                //} else if (e.ctrlKey && e.keyCode == 86) {      //ctrl+v
                //    copyfromClipboard();
                //}
            }

            if (handled) {
                // the event has been handled so don't let parent element (bubbling/propagation) or browser (default) handle it
                e.stopPropagation();
                e.preventDefault();
                try {
                    e.originalEvent.keyCode = 0; // prevent default behaviour for special keys in IE browsers (F3, F5, etc.)
                }
                // ignore exceptions - setting the original event's keycode throws
                // access denied exception for "Ctrl" (hitting control key only, nothing else), "Shift" (maybe others)
                catch (error) {
                }
            }
        }

        function copytoClipboard() {
            var ranges = getSelectionModel().getSelectedRanges();
            if (ranges.length == 0) {
            } else {
            }

            var cVal, rVal = "";
            var cell = getActiveCell();
            var dd = getData();
            var isRowSelector = columns[cell.cell].isRowSelector;       //isRowSelector usually is an attribute in the first column definition.

            if (isRowSelector && isRowSelector == true) {
                for (var i = 0; i < columns.length; i++) {
                    cVal = dd[cell.row][columns[i].field];
                    if (cVal != undefined) {
                        rVal += cVal + "\t";
                    } else {
                        rVal += " " + "\t";
                    }
                }
                rVal.trim("\t");
            }
            else {
                rVal = dd[cell.row][columns[cell.cell].field];
            }

            if (rVal != "") {
                window.clipboardData.setData("text", rVal);
            }
        }

        function copyfromClipboard() {
            var ptxt = window.clipboardData.getData("text").split('\t');
            var dd = getData();
            var cell = getActiveCell();
            var len = 0;

            if (dd.length - cell.cell < ptxt.length) {
                len = dd.length - cell.cell;
            }
            else {
                len = ptxt.length;
            }

            for (var i = 0; i < len; i++) {
                if (ptxt[i]) {
                    dd[cell.row][columns[cell.cell + i].field] = ptxt[i];
                }
            }
            dd[cell.row]["sta"] = 0;
            updateRow(cell.row);
            render();
        }

        function handleClick(e) {
            if (!currentEditor) {
                // if this click resulted in some cell child node getting focus,
                // don't steal it back - keyboard events will still bubble up
                if (e.target != document.activeElement || $(e.target).hasClass("slick-cell")) {
                    setFocus();
                }
            }

            var cell = getCellFromEvent(e);
            if (!cell || (currentEditor !== null && activeRow == cell.row && activeCell == cell.cell)) {
                return;
            }

            trigger(self.onClick, {
                row: cell.row,
                cell: cell.cell
            }, e);
            if (e.isImmediatePropagationStopped()) {
                return;
            }

            if (canCellBeActive(cell.row, cell.cell) == true) {
                if (!getEditorLock().isActive() || getEditorLock().commitCurrentEdit()) {
                    setActiveCellInternal(getCellNode(cell.row, cell.cell), (cell.row === getDataLength()) || options.autoEdit);
                }
            }
        }

        function handleContextMenu(e) {
            var $cell = $(e.target).closest(".slick-cell", $canvas);
            if ($cell.length === 0) {
                return;
            }

            // are we editing this cell?
            if (activeCellNode === $cell[0] && currentEditor !== null) {
                return;
            }

            //set the active cell to update the ui - don't update ui if multiple rows or cells are already selected
            var activeCell;
            if (getSelectedRows().length < 2) {
                var ranges = self.getSelectionModel().getSelectedRanges();
                if (ranges && ranges[0] && (ranges[0].fromCell == ranges[0].toCell)) {
                    var c = getCellFromEvent(e);
                    setActiveCell(c.row, c.cell);
                    activeCell = { row: c.row, cell: c.cell };
                }
            }

            trigger(self.onContextMenu, activeCell, e);
        }

        function handleDblClick(e) {
            var cell = getCellFromEvent(e);
            if (!cell || (currentEditor !== null && activeRow == cell.row && activeCell == cell.cell)) {
                return;
            }

            trigger(self.onDblClick, {
                row: cell.row,
                cell: cell.cell
            }, e);
            if (e.isImmediatePropagationStopped()) {
                return;
            }

            if (options.editable) {
                gotoCell(cell.row, cell.cell, true);
            }
        }

        function handleHeaderMouseEnter(e) {
            trigger(self.onHeaderMouseEnter, {
                "column": $(this).data("column")
            }, e);
        }

        function handleHeaderMouseLeave(e) {
            trigger(self.onHeaderMouseLeave, {
                "column": $(this).data("column")
            }, e);
        }

        function handleHeaderContextMenu(e) {
            var $header = $(e.target).closest(".slick-header-column", ".slick-header-columns");
            var column = $header && $header.data("column");
            trigger(self.onHeaderContextMenu, {
                column: column
            }, e);
        }

        function handleHeaderClick(e) {
            var $header = $(e.target).closest(".slick-header-column", ".slick-header-columns");
            var column = $header && $header.data("column");
            if (column) {
                trigger(self.onHeaderClick, { column: column }, e);
            }
        }

        function handleRowMouseEnter(e) {
            var row = getRowFromNode($(e.currentTarget));
            trigger(self.onRowMouseEnter, {
                row: row
            }, e);
        }

        function handleRowMouseLeave(e) {
            var row = getRowFromNode($(e.currentTarget));
            trigger(self.onRowMouseLeave, {
                row: row
            }, e);
        }

        function handleMouseEnter(e) {
            trigger(self.onMouseEnter, {}, e);
        }

        function handleMouseLeave(e) {
            trigger(self.onMouseLeave, {}, e);
        }

        function cellExists(row, cell) {
            return !(row < 0 || row >= getDataLength() || cell < 0 || cell >= columns.length);
        }

        function getCellFromPoint(x, y) {
            var row = Math.floor((y + offset) / options.rowHeight);
            var cell = 0;

            var w = 0;
            for (var i = 0; i < columns.length && w < x; i++) {
                w += columns[i].width;
                cell++;
            }

            if (cell < 0) {
                cell = 0;
            }

            return {
                row: row,
                cell: cell - 1
            };
        }

        function getCellFromNode(cellNode) {
            // read column number from .l<columnNumber> CSS class
            var cls = /l\d+/.exec(cellNode.className);
            if (!cls) {
                throw "getCellFromNode: cannot get cell - " + cellNode.className;
            }
            return parseInt(cls[0].substr(1, cls[0].length - 1), 10);
        }

        function getRowFromNode(rowNode) {
            for (var row in rowsCache) {
                if (rowsCache[row].rowNode[0] === rowNode[0]) {
                    return row | 0;
                }
            }

            return null;
        }

        function getCellFromEvent(e) {
            var $cell = $(e.target).closest(".slick-cell", $canvas);
            if (!$cell.length) {
                return null;
            }

            // TODO: This change eliminates the need for getCellFromEvent since
            //  we're ultimately calling getCellFromPoint.  Need to further analyze
            //  if getCellFromEvent can work with frozen columns

            var c = $cell.parents('.grid-canvas').offset();
            if (!c) {
                return null;
            }

            var rowOffset = 0;
            var isBottom = $cell.parents('.grid-canvas-bottom').length;

            var row = getCellFromPoint(e.clientX - c.left, e.clientY - c.top + rowOffset + $(document).scrollTop()).row;
            var cell = getCellFromNode($cell[0]);

            if (row == null || cell == null) {
                return null;
            } else {
                return {
                    "row": row,
                    "cell": cell
                };
            }
        }

        function getCellNodeBox(row, cell) {
            if (!cellExists(row, cell)) {
                return null;
            }

            var y1 = row * options.rowHeight - offset;
            var y2 = y1 + options.rowHeight - 1;
            var x1 = 0;
            for (var i = 0; i < cell; i++) {
                x1 += columns[i].width;

                if (options.frozenColumn == i) {
                    x1 = 0;
                }
            }
            var x2 = x1 + columns[cell].width;

            return {
                top: y1,
                left: x1,
                bottom: y2,
                right: x2
            };
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // Cell switching

        function resetActiveCell() {
            setActiveCellInternal(null, false);
        }

        function setFocus() {
            try {
                if (tabbingDirection == -1) {
                    $focusSink[0].focus();
                } else {
                    $focusSink2[0].focus();
                }
            } catch (error) {
            }
        }

        function scrollCellIntoView(row, cell) {
            // Don't scroll to frozen cells
            if (cell <= options.frozenColumn) {
                return;
            }

            var colspan = getColspan(row, cell);
            var left = columnPosLeft[cell],
                right = columnPosRight[cell + (colspan > 1 ? colspan - 1 : 0)],
                scrollRight = scrollLeft + $viewportScrollContainerX.width();
            //scrollRight = scrollLeft;

            if (left < scrollLeft) {
                $viewportScrollContainerX.scrollLeft(left);
                handleScroll();
                render();
            } else if (right > scrollRight) {
                $viewportScrollContainerX.scrollLeft(Math.min(left, right - $viewportScrollContainerX[0].clientWidth));
                handleScroll();
                render();
            }
        }

        function setActiveCellInternal(newCell, editMode) {
            if (activeCellNode !== null) {
                makeActiveCellNormal();
                $(activeCellNode).removeClass("active");
                if (rowsCache[activeRow]) {
                    $(rowsCache[activeRow].rowNode).removeClass("active");
                }
            }

            var activeCellChanged = (activeCellNode !== newCell);
            activeCellNode = newCell;

            if (activeCellNode != null) {
                var $activeCellNode = $(activeCellNode);
                var $activeCellOffset = $activeCellNode.offset();

                var rowOffset = Math.floor($activeCellNode.parents('.grid-canvas').offset().top);
                var isBottom = $activeCellNode.parents('.grid-canvas-bottom').length;

                cell = getCellFromPoint($activeCellOffset.left, Math.ceil($activeCellOffset.top) - rowOffset);

                activeRow = cell.row;
                activeCell = activePosX = activeCell = activePosX = getCellFromNode(activeCellNode[0]);

                $(activeCellNode).addClass("active");
                $(rowsCache[activeRow].rowNode).addClass("active");

                if (options.editable && editMode && isCellPotentiallyEditable(activeRow, activeCell)) {
                    clearTimeout(h_editorLoader);

                    if (options.asyncEditorLoading) {
                        h_editorLoader = setTimeout(function () {
                            makeActiveCellEditable();
                        }, options.asyncEditorLoadDelay);
                    } else {
                        makeActiveCellEditable();
                    }
                }
            } else {
                activeRow = activeCell = null;
            }

            if (activeCellChanged) {
                setTimeout(scrollActiveCellIntoView, 50);
                trigger(self.onActiveCellChanged, getActiveCell());
            }
        }

        function clearTextSelection() {
            if (document.selection && document.selection.empty) {
                try {
                    //IE fails here if selected element is not in dom
                    document.selection.empty();
                } catch (e) { }
            } else if (window.getSelection) {
                var sel = window.getSelection();
                if (sel && sel.removeAllRanges) {
                    sel.removeAllRanges();
                }
            }
        }

        function isCellPotentiallyEditable(row, cell) {
            // is the data for this row loaded?
            if (row < getDataLength() && !getDataItem(row)) {
                return false;
            }

            // are we in the Add New row?  can we create new from this cell?
            if (columns[cell].cannotTriggerInsert && row >= getDataLength()) {
                return false;
            }

            // does this cell have an editor?
            if (!getEditor(row, cell)) {
                return false;
            }

            return true;
        }

        function makeActiveCellNormal() {
            if (!currentEditor) {
                return;
            }
            trigger(self.onBeforeCellEditorDestroy, {
                editor: currentEditor
            });
            currentEditor.destroy();
            currentEditor = null;

            if (activeCellNode) {
                var d = getDataItem(activeRow);
                $(activeCellNode).removeClass("editable invalid");
                if (d) {
                    var column = columns[activeCell];
                    var formatter = getFormatter(activeRow, column);
                    activeCellNode[0].innerHTML = formatter(activeRow, activeCell, getDataItemValueForColumn(d, column), column, getDataItem(activeRow));
                    invalidatePostProcessingResults(activeRow);
                }
            }

            // if there previously was text selected on a page (such as selected
            // text in the edit cell just removed),
            // IE can't set focus to anything else correctly
            if (navigator.userAgent.toLowerCase().match(/msie/)) {
                clearTextSelection();
            }

            getEditorLock().deactivate(editController);
        }

        function makeActiveCellEditable(editor) {
            if (!activeCellNode) {
                return;
            }
            if (!options.editable) {
                throw "Grid : makeActiveCellEditable : should never get called when options.editable is false";
            }

            // cancel pending async call if there is one
            clearTimeout(h_editorLoader);

            if (!isCellPotentiallyEditable(activeRow, activeCell)) {
                return;
            }

            var columnDef = columns[activeCell];
            var item = getDataItem(activeRow);

            if (trigger(self.onBeforeEditCell, {
                row: activeRow,
                cell: activeCell,
                item: item,
                column: columnDef
            }) === false) {
                setFocus();
                return;
            }

            getEditorLock().activate(editController);
            $(activeCellNode).addClass("editable");

            // don't clear the cell if a custom editor is passed through
            if (!columnDef.editorRefered && !editor) {
                activeCellNode[0].innerHTML = "";
            }

            currentEditor = new (editor || getEditor(activeRow, activeCell))({
                grid: self,
                gridPosition: absBox($container[0]),
                position: absBox(activeCellNode[0]),
                container: activeCellNode,
                column: columnDef,
                item: item || {},
                commitChanges: commitEditAndSetFocus,
                cancelChanges: cancelEditAndSetFocus
            });

            if (item) {
                currentEditor.loadValue(item);
            }

            serializedEditorValue = currentEditor.serializeValue();

            if (currentEditor.position) {
                handleActiveCellPositionChange();
            }
        }

        function commitEditAndSetFocus() {
            // if the commit fails, it would do so due to a validation error
            // if so, do not steal the focus from the editor
            if (getEditorLock().commitCurrentEdit()) {
                setFocus();
                if (options.autoEdit) {
                    navigateNext();
                }
            }
        }

        function cancelEditAndSetFocus() {
            if (getEditorLock().cancelCurrentEdit()) {
                setFocus();
            }
        }

        function absBox(elem) {
            var box = {
                top: elem.offsetTop,
                left: elem.offsetLeft,
                bottom: 0,
                right: 0,
                width: $(elem).outerWidth(),
                height: $(elem).outerHeight(),
                visible: true
            };
            box.bottom = box.top + box.height;
            box.right = box.left + box.width;

            // walk up the tree
            var offsetParent = elem.offsetParent;
            while ((elem = elem.parentNode) != document.body) {
                if (box.visible && elem.scrollHeight != elem.offsetHeight && $(elem).css("overflowY") != "visible") {
                    box.visible = box.bottom > elem.scrollTop && box.top < elem.scrollTop + elem.clientHeight;
                }

                if (box.visible && elem.scrollWidth != elem.offsetWidth && $(elem).css("overflowX") != "visible") {
                    box.visible = box.right > elem.scrollLeft && box.left < elem.scrollLeft + elem.clientWidth;
                }

                box.left -= elem.scrollLeft;
                box.top -= elem.scrollTop;

                if (elem === offsetParent) {
                    box.left += elem.offsetLeft;
                    box.top += elem.offsetTop;
                    offsetParent = elem.offsetParent;
                }

                box.bottom = box.top + box.height;
                box.right = box.left + box.width;
            }

            return box;
        }

        function getActiveCellPosition() {
            return absBox(activeCellNode[0]);
        }

        function getCellPosition(row, cell) {
            var activeCellNode = getCellNode(row, cell);
            return absBox(activeCellNode[0]);
        }

        function getGridPosition() {
            return absBox($container[0]);
        }

        function handleActiveCellPositionChange() {
            if (!activeCellNode) {
                return;
            }

            trigger(self.onActiveCellPositionChanged, {});

            if (currentEditor) {
                var cellBox = getActiveCellPosition();
                if (currentEditor.show && currentEditor.hide) {
                    if (!cellBox.visible) {
                        currentEditor.hide();
                    } else {
                        currentEditor.show();
                    }
                }

                if (currentEditor.position) {
                    currentEditor.position(cellBox);
                }
            }
        }

        function getCellEditor() {
            return currentEditor;
        }

        function getActiveCell() {
            if (!activeCellNode) {
                return null;
            } else {
                return {
                    row: activeRow,
                    cell: activeCell
                };
            }
        }

        function getActiveCellNode() {
            return activeCellNode;
        }

        function scrollActiveCellIntoView() {
            if (activeRow != null && activeCell != null) {
                scrollCellIntoView(activeRow, activeCell);
            }
        }

        function scrollRowIntoView(row, doPaging) {

            var viewportScrollH = $viewportScrollContainerY.height();

            var rowAtTop = row * options.rowHeight;
            var rowAtBottom = (row + 1) * options.rowHeight
                            - viewportScrollH
                            + (viewportHasHScroll ? scrollbarDimensions.height : 0);

            // need to page down?
            if ((row + 1) * options.rowHeight > scrollTop + viewportScrollH + offset) {
                scrollTo(doPaging ? rowAtTop : rowAtBottom);
                render();
            }
                // or page up?
            else if (row * options.rowHeight < scrollTop + offset) {
                scrollTo(doPaging ? rowAtBottom : rowAtTop);
                render();
            }
        }

        function scrollRowToTop(row) {
            scrollTo(row * options.rowHeight);
            render();
        }

        function scrollRowToCurrentTop() {
            scrollTo(scrollTop);
            render();
        }

        function getColspan(row, cell) {
            var metadata = data.getItemMetadata && data.getItemMetadata(row);
            if (!metadata || !metadata.columns) {
                return 1;
            }

            var columnData = metadata.columns[columns[cell].id] || metadata.columns[cell];
            var colspan = (columnData && columnData.colspan);
            if (colspan === "*") {
                colspan = columns.length - cell;
            } else {
                colspan = colspan || 1;
            }
            return (colspan || 1);
        }

        function findFirstFocusableCell(row) {
            var cell = 0;
            while (cell < columns.length) {
                if (canCellBeActive(row, cell)) {
                    return cell;
                }
                cell += getColspan(row, cell);
            }
            return null;
        }

        function findLastFocusableCell(row) {
            var cell = 0;
            var lastFocusableCell = null;
            while (cell < columns.length) {
                if (canCellBeActive(row, cell)) {
                    lastFocusableCell = cell;
                }
                cell += getColspan(row, cell);
            }
            return lastFocusableCell;
        }

        function gotoRight(row, cell, posX) {
            if (cell >= columns.length) {
                return null;
            }

            do {
                cell += getColspan(row, cell);
            } while (cell < columns.length && !canCellBeActive(row, cell));

            if (cell < columns.length) {
                return {
                    "row": row,
                    "cell": cell,
                    "posX": cell
                };
            }
            return null;
        }

        function gotoLeft(row, cell, posX) {
            if (cell <= 0) {
                return null;
            }

            var firstFocusableCell = findFirstFocusableCell(row);
            if (firstFocusableCell === null || firstFocusableCell >= cell) {
                return null;
            }

            var prev = {
                "row": row,
                "cell": firstFocusableCell,
                "posX": firstFocusableCell
            };
            var pos;
            while (true) {
                pos = gotoRight(prev.row, prev.cell, prev.posX);
                if (!pos) {
                    return null;
                }
                if (pos.cell >= cell) {
                    return prev;
                }
                prev = pos;
            }
        }

        function gotoDown(row, cell, posX) {
            var prevCell;
            while (true) {
                if (++row >= getDataLength() + (options.enableAddRow ? 1 : 0)) {
                    return null;
                }

                prevCell = cell = 0;
                while (cell <= posX) {
                    prevCell = cell;
                    cell += getColspan(row, cell);
                }

                if (canCellBeActive(row, prevCell)) {
                    return {
                        "row": row,
                        "cell": prevCell,
                        "posX": posX
                    };
                }
            }
        }

        function gotoUp(row, cell, posX) {
            var prevCell;
            while (true) {
                if (--row < 0) {
                    return null;
                }

                prevCell = cell = 0;
                while (cell <= posX) {
                    prevCell = cell;
                    cell += getColspan(row, cell);
                }

                if (canCellBeActive(row, prevCell)) {
                    return {
                        "row": row,
                        "cell": prevCell,
                        "posX": posX
                    };
                }
            }
        }

        function gotoNext(row, cell, posX) {
            if (row == null && cell == null) {
                row = cell = posX = 0;
                if (canCellBeActive(row, cell)) {
                    return {
                        "row": row,
                        "cell": cell,
                        "posX": cell
                    };
                }
            }

            var pos = gotoRight(row, cell, posX);
            if (pos) {
                return pos;
            }

            var firstFocusableCell = null;
            while (++row < getDataLength() + (options.enableAddRow ? 1 : 0)) {
                firstFocusableCell = findFirstFocusableCell(row);
                if (firstFocusableCell !== null) {
                    return {
                        "row": row,
                        "cell": firstFocusableCell,
                        "posX": firstFocusableCell
                    };
                }
            }
            return null;
        }

        function gotoPrev(row, cell, posX) {
            if (row == null && cell == null) {
                row = getDataLength() + (options.enableAddRow ? 1 : 0) - 1;
                cell = posX = columns.length - 1;
                if (canCellBeActive(row, cell)) {
                    return {
                        "row": row,
                        "cell": cell,
                        "posX": cell
                    };
                }
            }

            var pos;
            var lastSelectableCell;
            while (!pos) {
                pos = gotoLeft(row, cell, posX);
                if (pos) {
                    break;
                }
                if (--row < 0) {
                    return null;
                }

                cell = 0;
                lastSelectableCell = findLastFocusableCell(row);
                if (lastSelectableCell !== null) {
                    pos = {
                        "row": row,
                        "cell": lastSelectableCell,
                        "posX": lastSelectableCell
                    };
                }
            }
            return pos;
        }

        function navigateRight() {
            return navigate("right");
        }

        function navigateLeft() {
            return navigate("left");
        }

        function navigateDown() {
            return navigate("down");
        }

        function navigateUp() {
            return navigate("up");
        }

        function navigateNext() {
            return navigate("next");
        }

        function navigatePrev() {
            return navigate("prev");
        }

        /**
         * @param {string} dir Navigation direction.
         * @return {boolean} Whether navigation resulted in a change of active cell.
         */
        function navigate(dir) {
            if (!options.enableCellNavigation) {
                return false;
            }

            if (!activeCellNode && dir != "prev" && dir != "next") {
                return false;
            }

            if (!getEditorLock().commitCurrentEdit()) {
                return true;
            }
            setFocus();

            var tabbingDirections = {
                "up": -1,
                "down": 1,
                "left": -1,
                "right": 1,
                "prev": -1,
                "next": 1
            };
            tabbingDirection = tabbingDirections[dir];

            var stepFunctions = {
                "up": gotoUp,
                "down": gotoDown,
                "left": gotoLeft,
                "right": gotoRight,
                "prev": gotoPrev,
                "next": gotoNext
            };
            var stepFn = stepFunctions[dir];
            var pos = stepFn(activeRow, activeCell, activePosX);
            if (pos) {
                var isAddNewRow = (pos.row == getDataLength());
                scrollRowIntoView(pos.row, !isAddNewRow);
                scrollCellIntoView(pos.row, pos.cell);
                setActiveCellInternal(getCellNode(pos.row, pos.cell), isAddNewRow || options.autoEdit);
                activePosX = pos.posX;
                return true;
            } else {
                setActiveCellInternal(getCellNode(activeRow, activeCell), (activeRow == getDataLength()) || options.autoEdit);
                return false;
            }
        }

        function getCellNode(row, cell) {
            if (rowsCache[row]) {
                ensureCellNodesInRowsCache(row);
                return rowsCache[row].cellNodesByColumnIdx[cell];
            }
            return null;
        }

        function setActiveCell(row, cell) {
            if (!initialized) {
                return;
            }
            if (row > getDataLength() || row < 0 || cell >= columns.length || cell < 0) {
                return;
            }

            if (!options.enableCellNavigation) {
                return;
            }

            scrollRowIntoView(row, false);
            scrollCellIntoView(row, cell);
            setActiveCellInternal(getCellNode(row, cell), false);
        }

        function canCellBeActive(row, cell) {
            if (!options.enableCellNavigation || row >= getDataLength() + (options.enableAddRow ? 1 : 0) || row < 0 || cell >= columns.length || cell < 0) {
                return false;
            }

            var rowMetadata = data.getItemMetadata && data.getItemMetadata(row);
            if (rowMetadata && typeof rowMetadata.focusable === "boolean") {
                return rowMetadata.focusable;
            }

            var columnMetadata = rowMetadata && rowMetadata.columns;
            if (columnMetadata && columnMetadata[columns[cell].id] && typeof columnMetadata[columns[cell].id].focusable === "boolean") {
                return columnMetadata[columns[cell].id].focusable;
            }
            if (columnMetadata && columnMetadata[cell] && typeof columnMetadata[cell].focusable === "boolean") {
                return columnMetadata[cell].focusable;
            }

            if (typeof columns[cell].focusable === "boolean") {
                return columns[cell].focusable;
            }

            return true;
        }

        function canCellBeSelected(row, cell) {
            if (row >= getDataLength() || row < 0 || cell >= columns.length || cell < 0) {
                return false;
            }

            var rowMetadata = data.getItemMetadata && data.getItemMetadata(row);
            if (rowMetadata && typeof rowMetadata.selectable === "boolean") {
                return rowMetadata.selectable;
            }

            var columnMetadata = rowMetadata && rowMetadata.columns && (rowMetadata.columns[columns[cell].id] || rowMetadata.columns[cell]);
            if (columnMetadata && typeof columnMetadata.selectable === "boolean") {
                return columnMetadata.selectable;
            }
            if (columns[cell] != undefined && typeof columns[cell].selectable === "boolean") {
                return columns[cell].selectable;
            }

            return true;
        }

        function gotoCell(row, cell, forceEdit) {
            if (!initialized) {
                return;
            }
            if (!canCellBeActive(row, cell)) {
                return;
            }

            if (!getEditorLock().commitCurrentEdit()) {
                return;
            }

            scrollRowIntoView(row, false);
            scrollCellIntoView(row, cell);

            var newCell = getCellNode(row, cell);

            // if selecting the 'add new' row, start editing right away
            setActiveCellInternal(newCell, forceEdit || (row === getDataLength()) || options.autoEdit);

            // if no editor was created, set the focus back on the grid
            if (!currentEditor) {
                setFocus();
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////
        // IEditor implementation for the editor lock

        function commitCurrentEdit() {
            var item = getDataItem(activeRow);
            var column = columns[activeCell];

            if (currentEditor) {
                if (currentEditor.isValueChanged()) {
                    var validationResults = currentEditor.validate();

                    if (validationResults.valid) {
                        if (activeRow < getDataLength()) {
                            var editCommand = {
                                row: activeRow,
                                cell: activeCell,
                                editor: currentEditor,
                                serializedValue: currentEditor.serializeValue(),
                                prevSerializedValue: serializedEditorValue,
                                execute: function () {
                                    this.editor.applyValue(item, this.serializedValue);
                                    updateRow(this.row);
                                },
                                undo: function () {
                                    this.editor.applyValue(item, this.prevSerializedValue);
                                    updateRow(this.row);
                                }
                            };

                            if (options.editCommandHandler) {
                                makeActiveCellNormal();
                                options.editCommandHandler(item, column, editCommand);
                            } else {
                                editCommand.execute();
                                makeActiveCellNormal();
                            }

                            trigger(self.onCellChange, {
                                row: activeRow,
                                cell: activeCell,
                                item: item
                            });
                        } else {
                            var newItem = {};
                            currentEditor.applyValue(newItem, currentEditor.serializeValue());
                            makeActiveCellNormal();
                            trigger(self.onAddNewRow, {
                                item: newItem,
                                column: column
                            });
                        }

                        // check whether the lock has been re-acquired by event handlers
                        return !getEditorLock().isActive();
                    } else {
                        // TODO: remove and put in onValidationError handlers in examples
                        $(activeCellNode).removeClass("invalid");
                        $(activeCellNode).width();  // force layout
                        $(activeCellNode).addClass("invalid");

                        trigger(self.onValidationError, {
                            editor: currentEditor,
                            cellNode: activeCellNode,
                            validationResults: validationResults,
                            row: activeRow,
                            cell: activeCell,
                            column: column
                        });

                        currentEditor.focus();
                        return false;
                    }
                }

                makeActiveCellNormal();
            }
            return true;
        }

        function cancelCurrentEdit() {
            makeActiveCellNormal();
            return true;
        }

        function rowsToRanges(rows) {
            var ranges = [];
            var lastCell = columns.length - 1;
            for (var i = 0; i < rows.length; i++) {
                ranges.push(new Slick.Range(rows[i], 0, rows[i], lastCell));
            }
            return ranges;
        }

        function getSelectedRows() {
            if (!selectionModel) {
                throw "Selection model is not set";
            }
            return selectedRows;
        }

        function setSelectedRows(rows) {
            if (!selectionModel) {
                throw "Selection model is not set";
            }
            //make active cell in the selected row and be selected.
            if (rows && rows.length == 1) {
                setActiveCell(rows[0], 0);
            }
            selectionModel.setSelectedRanges(rowsToRanges(rows));
        }

        function getShiftSelectedRows() {
            return shiftSelectedRows;
        }

        function getSelectedDataItem() {
            if (!selectionModel) {
                throw "Selection model is not set";
            }

            if (data.getItemByIdx) {
                return data.getItemByIdx(selectedRows[0]);
            }
            else {
                return getDataItem(selectedRows[0]);
            }
        }

        function getSelectedDataItems() {
            if (!selectionModel) {
                throw "Selection model is not set";
            }

            var items;
            if (data.getItemByIdx) {
                //when dataview is parametered...
                items = selectedRows.map(function (x) {
                    return data.getItemByIdx(x);
                });
            }
            else {
                items = jQuery.map(selectedRows, function (x) {
                    return getDataItem(x);
                });
            }
            return items;
        }

        function getCheckedDataItem() {
            if (!selectionModel) {
                throw "Selection model is not set";
            }

            var item;
            if (data.getItems) {
                //when dataview is parametered...
                var rows = data.getItems();
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].checked == true) {
                        item = rows[i];
                        break;
                    }
                }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].checked == true) {
                        item = data[i];
                        break;
                    }
                }
            }
            return item;
        }

        function getCheckedDataItems() {
            if (!selectionModel) {
                throw "Selection model is not set";
            }

            var items;
            if (data.getItems) {
                //when dataview is parametered...
                var rows = data.getItems();
                items = jQuery.grep(rows, function (x) {
                    return x.checked == true;
                });
            }
            else {
                items = jQuery.grep(data, function (x) {
                    return x.checked == true;
                });
            }
            return items;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////
        // Debug

        this.debug = function () {
            var s = "";

            s += ("\n" + "counter_rows_rendered:  " + counter_rows_rendered);
            s += ("\n" + "counter_rows_removed:  " + counter_rows_removed);
            s += ("\n" + "renderedRows:  " + renderedRows);
            s += ("\n" + "numVisibleRows:  " + numVisibleRows);
            s += ("\n" + "maxSupportedCssHeight:  " + maxSupportedCssHeight);
            s += ("\n" + "n(umber of pages):  " + n);
            s += ("\n" + "(current) page:  " + page);
            s += ("\n" + "page height (ph):  " + ph);
            s += ("\n" + "vScrollDir:  " + vScrollDir);

            alert(s);
        };

        // a debug helper to be able to access private members
        this.eval = function (expr) {
            return eval(expr);
        };

        //////////////////////////////////////////////////////////////////////////////////////////////
        // Public API

        $.extend(this, {
            "slickGridVersion": "2.1",

            // Events
            "onScroll": new Slick.Event(),
            "onSort": new Slick.Event(),
            "onHeaderMouseEnter": new Slick.Event(),
            "onHeaderMouseLeave": new Slick.Event(),
            "onHeaderContextMenu": new Slick.Event(),
            "onHeaderClick": new Slick.Event(),
            "onHeaderCellRendered": new Slick.Event(),
            "onBeforeHeaderCellDestroy": new Slick.Event(),
            "onHeaderRowCellRendered": new Slick.Event(),
            "onBeforeHeaderRowCellDestroy": new Slick.Event(),
            "onRowMouseEnter": new Slick.Event(),
            "onRowMouseLeave": new Slick.Event(),
            "onMouseEnter": new Slick.Event(),
            "onMouseLeave": new Slick.Event(),
            "onClick": new Slick.Event(),
            "onDblClick": new Slick.Event(),
            "onContextMenu": new Slick.Event(),
            "onKeyDown": new Slick.Event(),
            "onAddNewRow": new Slick.Event(),
            "onValidationError": new Slick.Event(),
            "onViewportChanged": new Slick.Event(),
            "onColumnsReordered": new Slick.Event(),
            "onColumnsResized": new Slick.Event(),
            "onCellChange": new Slick.Event(),
            "onBeforeEditCell": new Slick.Event(),
            "onBeforeCellEditorDestroy": new Slick.Event(),
            "onBeforeDestroy": new Slick.Event(),
            "onActiveCellChanged": new Slick.Event(),
            "onActiveCellPositionChanged": new Slick.Event(),
            "onDragInit": new Slick.Event(),
            "onDragStart": new Slick.Event(),
            "onDrag": new Slick.Event(),
            "onDragEnd": new Slick.Event(),
            "onSelectedRowsChanged": new Slick.Event(),
            "onCellCssStylesChanged": new Slick.Event(),

            // Methods
            "registerPlugin": registerPlugin,
            "unregisterPlugin": unregisterPlugin,
            "getColumns": getColumns,
            "setColumns": setColumns,
            "getColumnIndex": getColumnIndex,
            "createColumnHeaders": createColumnHeaders,
            "updateColumnHeader": updateColumnHeader,
            "setSortColumn": setSortColumn,
            "setSortColumns": setSortColumns,
            "getSortColumns": getSortColumns,
            "autosizeColumns": autosizeColumns,
            "getOptions": getOptions,
            "setOptions": setOptions,
            "getData": getData,
            "getDataLength": getDataLength,
            "getDataItem": getDataItem,
            "setData": setData,
            "update": updateData,
            "getSelectionModel": getSelectionModel,
            "setSelectionModel": setSelectionModel,
            "getSelectedRows": getSelectedRows,
            "setSelectedRows": setSelectedRows,
            "getShiftSelectedRows": getShiftSelectedRows,
            "getRangesFromRows": rowsToRanges,
            "getSelectedDataItem": getSelectedDataItem,
            "getSelectedDataItems": getSelectedDataItems,
            "getCheckedDataItem": getCheckedDataItem,
            "getCheckedDataItems": getCheckedDataItems,
            "getVisibleColumns": getVisibleColumns,
            "getGridId": getGridId,

            "getHeadersWidth": getHeadersWidth,
            "getHeaderColumnWidthDiff": getHeaderColumnWidthDiff,

            "render": render,
            "invalidate": invalidate,
            "invalidateRow": invalidateRow,
            "invalidateRows": invalidateRows,
            "invalidateAllRows": invalidateAllRows,
            "updateCell": updateCell,
            "updateRow": updateRow,
            "getViewport": getVisibleRange,
            "getRenderedRange": getRenderedRange,
            "resizeCanvas": resizeCanvas,
            "updateRowCount": updateRowCount,
            "scrollRowIntoView": scrollRowIntoView,
            "scrollRowToTop": scrollRowToTop,
            "scrollCellIntoView": scrollCellIntoView,
            "scrollActiveCellIntoView": scrollActiveCellIntoView,
            "getCanvasNode": getCanvasNode,
            "getCanvases": getCanvases,
            "getActiveCanvasNode": getActiveCanvasNode,
            "setActiveCanvasNode": setActiveCanvasNode,
            "getViewportNode": getViewportNode,
            "getActiveViewportNode": getActiveViewportNode,
            "setActiveViewportNode": setActiveViewportNode,
            "focus": setFocus,

            "getCellFromPoint": getCellFromPoint,
            "getCellFromEvent": getCellFromEvent,
            "getActiveCell": getActiveCell,
            "setActiveCell": setActiveCell,
            "getActiveCellNode": getActiveCellNode,
            "getActiveCellPosition": getActiveCellPosition,
            "getCellPosition": getCellPosition,
            "resetActiveCell": resetActiveCell,
            "editActiveCell": makeActiveCellEditable,
            "getCellEditor": getCellEditor,
            "getCellNode": getCellNode,
            "getCellNodeBox": getCellNodeBox,
            "canCellBeSelected": canCellBeSelected,
            "canCellBeActive": canCellBeActive,
            "navigatePrev": navigatePrev,
            "navigateNext": navigateNext,
            "navigateUp": navigateUp,
            "navigateDown": navigateDown,
            "navigateLeft": navigateLeft,
            "navigateRight": navigateRight,
            "gotoCell": gotoCell,
            "getGridPosition": getGridPosition,
            "flashCell": flashCell,
            "addCellCssStyles": addCellCssStyles,
            "setCellCssStyles": setCellCssStyles,
            "removeCellCssStyles": removeCellCssStyles,
            "getCellCssStyles": getCellCssStyles,

            "init": finishInitialization,
            "destroy": destroy,

            // IEditor implementation
            "getEditorLock": getEditorLock,
            "getEditController": getEditController
        });

        init();
    }
}(jQuery));
//#endregion

//#region Slick.RowSelector JS
(function ($) {
    // register namespace
    $.extend(true, window, {
        "Slick": {
            "RowSelectionModel": SelectionModel,
            "RowMoveManager": MoveManager
        }
    });

    //#region 行选择模型
    function SelectionModel(options) {
        var _grid;
        var _ranges = [];
        var _self = this;
        var _handler = new Slick.EventHandler();
        var _inHandler;
        var _options;
        var _defaults = {
            selectActiveRow: true
        };

        function init(grid) {
            _options = $.extend(true, {}, _defaults, options);
            _grid = grid;
            _handler.subscribe(_grid.onActiveCellChanged,
                wrapHandler(handleActiveCellChange));
            _handler.subscribe(_grid.onKeyDown,
                wrapHandler(handleKeyDown));
            _handler.subscribe(_grid.onClick,
                wrapHandler(handleClick));
        }

        function destroy() {
            _handler.unsubscribeAll();
        }

        function wrapHandler(handler) {
            return function () {
                if (!_inHandler) {
                    _inHandler = true;
                    handler.apply(this, arguments);
                    _inHandler = false;
                }
            };
        }

        function rangesToRows(ranges) {
            var rows = [];
            for (var i = 0; i < ranges.length; i++) {
                for (var j = ranges[i].fromRow; j <= ranges[i].toRow; j++) {
                    rows.push(j);
                }
            }
            return rows;
        }

        function rowsToRanges(rows) {
            var ranges = [];
            var lastCell = _grid.getColumns().length - 1;
            for (var i = 0; i < rows.length; i++) {
                ranges.push(new Slick.Range(rows[i], 0, rows[i], lastCell));
            }
            return ranges;
        }

        function getRowsRange(from, to) {
            var i, rows = [];
            for (i = from; i <= to; i++) {
                rows.push(i);
            }
            for (i = to; i < from; i++) {
                rows.push(i);
            }
            return rows;
        }

        function getSelectedRows() {
            return rangesToRows(_ranges);
        }

        function setSelectedRows(rows) {
            setSelectedRanges(rowsToRanges(rows));
        }

        function setSelectedRanges(ranges) {
            _ranges = ranges;
            _self.onSelectedRangesChanged.notify(_ranges);
        }

        function getSelectedRanges() {
            return _ranges;
        }

        function handleActiveCellChange(e, data) {
            if (_options.selectActiveRow) {
                setSelectedRanges([new Slick.Range(data.row, 0, data.row, _grid.getColumns().length - 1)]);
            }
        }

        function handleKeyDown(e) {
            var activeRow = _grid.getActiveCell();
            if (activeRow && e.shiftKey && !e.ctrlKey && !e.altKey && !e.metaKey && (e.which == 38 || e.which == 40)) {
                var selectedRows = getSelectedRows();
                selectedRows.sort(function (x, y) {
                    return x - y
                });

                if (!selectedRows.length) {
                    selectedRows = [activeRow.row];
                }

                var top = selectedRows[0];
                var bottom = selectedRows[selectedRows.length - 1];
                var active;

                if (e.which == 40) {
                    active = activeRow.row < bottom || top == bottom ? ++bottom : ++top;
                } else {
                    active = activeRow.row < bottom ? --bottom : --top;
                }

                if (active >= 0 && active < _grid.getDataLength()) {
                    _grid.scrollRowIntoView(active);
                    _ranges = rowsToRanges(getRowsRange(top, bottom));
                    setSelectedRanges(_ranges);
                }

                e.preventDefault();
                e.stopPropagation();
            }
        }

        function handleClick(e) {
            var cell = _grid.getCellFromEvent(e);
            if (!cell || !_grid.canCellBeActive(cell.row, cell.cell)) {
                return false;
            }

            var selection = rangesToRows(_ranges);
            var idx = $.inArray(cell.row, selection);

            if (!e.ctrlKey && !e.shiftKey && !e.metaKey) {
                return false;
            }
            else if (_grid.getOptions().multiSelect) {
                if (idx === -1 && (e.ctrlKey || e.metaKey)) {
                    selection.push(cell.row);
                    _grid.setActiveCell(cell.row, cell.cell);
                } else if (idx !== -1 && (e.ctrlKey || e.metaKey)) {
                    selection = $.grep(selection, function (o, i) {
                        return (o !== cell.row);
                    });
                    _grid.setActiveCell(cell.row, cell.cell);
                } else if (selection.length && e.shiftKey) {
                    var last = selection.pop();
                    var from = Math.min(cell.row, last);
                    var to = Math.max(cell.row, last);
                    selection = [];
                    for (var i = from; i <= to; i++) {
                        if (i !== last) {
                            selection.push(i);
                        }
                    }
                    selection.push(last);
                    _grid.setActiveCell(cell.row, cell.cell);
                }
            }

            _ranges = rowsToRanges(selection);
            setSelectedRanges(_ranges);
            e.stopImmediatePropagation();

            return true;
        }

        $.extend(this, {
            "getSelectedRows": getSelectedRows,
            "setSelectedRows": setSelectedRows,

            "getSelectedRanges": getSelectedRanges,
            "setSelectedRanges": setSelectedRanges,

            "init": init,
            "destroy": destroy,

            "onSelectedRangesChanged": new Slick.Event()
        });
    }
    //#endregion

    //#region 行删除管理器
    function MoveManager(options) {
        var _grid;
        var _canvas;
        var _dragging;
        var _self = this;
        var _handler = new Slick.EventHandler();
        var _defaults = {
            cancelEditOnDrag: false
        };

        function init(grid) {
            options = $.extend(true, {}, _defaults, options);
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
            // prevent the grid from cancelling drag'n'drop by default
            e.stopImmediatePropagation();
        }

        function handleDragStart(e, dd) {
            var cell = _grid.getCellFromEvent(e);

            if (options.cancelEditOnDrag && _grid.getEditorLock().isActive()) {
                _grid.getEditorLock().cancelCurrentEdit();
            }

            if (_grid.getEditorLock().isActive() || !/move|selectAndMove/.test(_grid.getColumns()[cell.cell].behavior)) {
                return false;
            }

            _dragging = true;
            e.stopImmediatePropagation();

            var selectedRows = _grid.getSelectedRows();

            if (selectedRows.length == 0 || $.inArray(cell.row, selectedRows) == -1) {
                selectedRows = [cell.row];
                _grid.setSelectedRows(selectedRows);
            }

            var rowHeight = _grid.getOptions().rowHeight;

            dd.selectedRows = selectedRows;

            dd.selectionProxy = $("<div class='slick-reorder-proxy'/>")
                .css("position", "absolute")
                .css("zIndex", "99999")
                .css("width", $(_canvas).innerWidth())
                .css("height", rowHeight * selectedRows.length)
                .appendTo(_canvas);

            dd.guide = $("<div class='slick-reorder-guide'/>")
                .css("position", "absolute")
                .css("zIndex", "99998")
                .css("width", $(_canvas).innerWidth())
                .css("top", -1000)
                .appendTo(_canvas);

            dd.insertBefore = -1;
        }

        function handleDrag(e, dd) {
            if (!_dragging) {
                return;
            }

            e.stopImmediatePropagation();

            var top = e.pageY - $(_canvas).offset().top;
            dd.selectionProxy.css("top", top - 5);

            var insertBefore = Math.max(0, Math.min(Math.round(top / _grid.getOptions().rowHeight), _grid.getDataLength()));
            if (insertBefore !== dd.insertBefore) {
                var eventData = {
                    "rows": dd.selectedRows,
                    "insertBefore": insertBefore
                };

                if (_self.onBeforeMoveRows.notify(eventData) === false) {
                    dd.guide.css("top", -1000);
                    dd.canMove = false;
                } else {
                    dd.guide.css("top", insertBefore * _grid.getOptions().rowHeight);
                    dd.canMove = true;
                }

                dd.insertBefore = insertBefore;
            }
        }

        function handleDragEnd(e, dd) {
            if (!_dragging) {
                return;
            }
            _dragging = false;
            e.stopImmediatePropagation();

            dd.guide.remove();
            dd.selectionProxy.remove();

            if (dd.canMove) {
                var eventData = {
                    "rows": dd.selectedRows,
                    "insertBefore": dd.insertBefore
                };
                // TODO:  _grid.remapCellCssClasses ?
                _self.onMoveRows.notify(eventData);
            }
        }

        $.extend(this, {
            "onBeforeMoveRows": new Slick.Event(),
            "onMoveRows": new Slick.Event(),

            "init": init,
            "destroy": destroy
        });
    }
    //#endregion

})(jQuery);
//#endregion

//#region Slick.CellSelector JS
(function ($) {
    $.extend(true, window, {
        "Slick": {
            "Cell": {
                "SelectionModel": SelectionModel,
                "CopyManager": CopyManager,
                "CopyManagerExt": CopyManagerExternal,
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
                "z-index": 999,
                "border": "2px solid black"
            }
        });

        var _options;
        var _defaults = {
            selectActiveCell: true
        };
        var _data;
        var _columns;
        var _currentSelectedCell;
        var _keyPressedType = {
            isRowSelector: false,
            key: ''
        };

        function init(grid) {
            _options = $.extend(true, {}, _defaults, options);
            _grid = grid;
            if (_options.data != undefined)
                _data = options.data;

            if (_options.column != undefined)
                _columns = options.columns;

            _canvas = _grid.getCanvasNode();
            _grid.onActiveCellChanged.subscribe(handleActiveCellChange);
            _grid.registerPlugin(_selector);

            _selector.onCellRangeSelected.subscribe(handleCellRangeSelected);

            _grid.onClick.subscribe(handleCanvasClick);
        }

        function destroy() {
            _grid.onClick.unsubscribe(handleCanvasClick);
            _grid.onActiveCellChanged.unsubscribe(handleActiveCellChange);
            _selector.onCellRangeSelected.unsubscribe(handleCellRangeSelected);

            _grid.unregisterPlugin(_selector);
        }

        function handleCanvasClick(e) {
            var cell = _grid.getCellFromEvent(e);
            _currentSelectedCell = cell;

            var column = _grid.getColumns()[cell.cell];

            //row selector 
            if (column.isRowSelector && column.isRowSelector == true) {
                _keyPressedType.isRowSelector = true;
            } else {
                _keyPressedType.isRowSelector = false;
            }

            //key press type
            if (!e.ctrlKey && !e.shiftKey && !e.metaKey) {
                _keyPressedType.key = 'normal';
            } else if (_grid.getOptions().multiSelect) {
                if (e.ctrlKey || e.metaKey) {
                    _keyPressedType.key = 'ctrl';
                } else if (e.shiftKey) {
                    _keyPressedType.key = 'shift';
                }
            }
        }
        function getSelectedRanges() {
            return _ranges;
        }

        function setSelectedRanges(ranges) {
            _ranges = removeInvalidRange(ranges);
            _self.onSelectedRangesChanged.notify(_ranges);
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

        function handleCellRangeSelected(e, args) {
            setSelectedRanges([args.range]);
            getSumOnSelectedRangesChanged(args.range);
        }

        function handleActiveCellChange(e, args) {
            if (_keyPressedType.isRowSelector == false) {
                if (_options.selectActiveCell) {
                    setSelectedRanges([new Slick.Range(args.row, args.cell)]);
                }
                return;
            }

            var ranges;
            var cell = _currentSelectedCell;
            var rows = _grid.getShiftSelectedRows();

            if (_keyPressedType.key == 'normal') {
                rows.splice(0, rows.length);
                rows.push(cell.row);
                ranges = _grid.getRangesFromRows(rows);
                setSelectedRanges(ranges);

                return;
            } else if (_grid.getOptions().multiSelect) {
                if (_keyPressedType.key == 'ctrl') {
                    var idx = $.inArray(cell.row, rows);

                    if (idx === -1) {
                        rows.push(cell.row);
                    } else {
                        rows.splice(rows.indexOf(cell.row), 1);
                    }
                    rows.sort();

                    ranges = _grid.getRangesFromRows(rows);
                    setSelectedRanges(ranges);

                    return;
                } else if (_keyPressedType.key == 'shift') {
                    var last = rows.pop();
                    var from = Math.min(cell.row, last);
                    var to = Math.max(cell.row, last);

                    rows.splice(0, rows.length);
                    for (var i = from; i <= to; i++) {
                        rows.push(i);
                    }
                    ranges = _grid.getRangesFromRows(rows);
                    setSelectedRanges(ranges);

                    return;
                }
            }
            return;
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
            var $menu = $("<div class='slick-column-summary' style='display:none;position:absolute;z-index:20;'></div>")
                .text("合计：" + sum)
                .appendTo(document.body);
                
            var position = _grid.getCellPosition(toRow, toCell);
            var top = position.top;
            var left = position.left;

            $menu
                .css("top", top + 25)
                .css("left", left + 10)
                .fadeIn(150);

            setTimeout(function () {
                $menu.remove();
            }, 4000);
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
            //_grid.onKeyDown.subscribe(handleKeyDown);     //confict with copymanagerext
            _grid.onContextMenu.subscribe(handleCellContextMenu);
            _grid.onClick.subscribe(handleGridClick);

            $menu = $("<div class='slick-column-menu' style='display:none;position:absolute;z-index:20;'></div>")
                .appendTo(document.body);

            prepareCellMenuItem();
        }

        function destroy() {
            //_grid.onKeyDown.unsubscribe(handleKeyDown);   //conflict with copymanagerext
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

            if (e.which == 67 && (e.ctrlKey || e.metaKey)) {    //CTRL + C
                copyCellRanges(e);
            }

            if (e.which == 86 && (e.ctrlKey || e.metaKey)) {    //CTRL + V
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
                    var cell = _grid.getActiveCell();
                    if (cell && cell.row >= _data.length) {
                        //新增行粘贴记录
                        var range = new Slick.Range(cell.row, 0, cell.row, 100);
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
                    }
                    _data[to.fromRow + i][_columns[to.fromCell + j].field] = val;
                    _data[to.fromRow + i]["sta"] = 0;
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
            if (args.cell) {
                if (!_columns[args.cell].hasPZ) {
                    $menu.find(".slick-column-menuitem").eq(0).css("display", "none");
                    $menu.find("hr").eq(0).css("display", "none");
                }
                else {
                    $menu.find(".slick-column-menuitem").eq(0).css("display", "block");
                    $menu.find("hr").eq(0).css("display", "block");
                }

            }

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

    //#region 扩展复制管理器 Excel
    function CopyManagerExternal(options) {
        var _grid;
        var _dataview = options.dataview;
        var _self = this;
        var _copiedRanges;
        var _options = options || {};
        var _copiedCellStyleLayerKey = _options.copiedCellStyleLayerKey || "copy-manager";
        var _copiedCellStyle = _options.copiedCellStyle || "copied";
        var _clareCopyTI = 0;
        var _bodyElement = _options.bodyElement || document.body;

        var keyCodes = {
            'C': 67,
            'V': 86,
            'ESC': 27,
            'ALT': 18
        };

        function init(grid) {
            _grid = grid;
            _grid.onKeyDown.subscribe(handleKeyDown);

            var cellSelectionModel = grid.getSelectionModel();
            if (!cellSelectionModel) {
                throw new Error('need to set cell section model for slickgrid.');
            }

            cellSelectionModel.onSelectedRangesChanged.subscribe(function (e, args) {
                _grid.focus();
            });
        }

        function destroy() {
            _grid.onKeyDown.unsubscribe(handleKeyDown);
        }

        function getDataItemValueForColumn(row, item, columnDef) {
            if (_options.dataItemColumnValueExtractor) {
                return _options.dataItemColumnValueExtractor(item, columnDef);
            }

            var retVal = '';
            var $container = $("<div id='divTempContainer_27z2923' style='top:-10000px; left:-10000px;display:none;'></div>")
                    .appendTo("body");

            if (columnDef.editor) {
                var fName = getFnName(columnDef.editor);
                if (fName == "DecimalEditor"
                    || fName == "IntegerEditor"
                    || fName == "TextEditor"
                    || fName == "DateEditor"
                    || fName == "TextButtonEditor") {
                    return item[columnDef.field];
                } else {
                    var editorArgs = {
                        'grid': _grid,
                        'container': $container,
                        'column': columnDef,
                        'row': row,
                        'position': { 'top': 0, 'left': 0 }
                    };

                    var editor = new columnDef.editor(editorArgs);
                    editor.loadValue(item);
                    retVal = editor.serializeValue();
                    $container.remove();
                }
            } else {
                retVal = item[columnDef.field];
            }

            return retVal;
        }

        function setDataItemValueForColumn(item, columnDef, value, y, x) {
            if (_options.dataItemColumnValueSetter) {
                return _options.dataItemColumnValueSetter(item, columnDef, value);
            }

            if (columnDef.editor) {
                var $container = $("<div id='divTempContainer_27z2923' style='top:-10000px; left:-10000px;display:none;'></div>")
                    .appendTo("body");

                var editorArgs = {
                    'grid': _grid,
                    'container': $container,
                    'column': columnDef,
                    'position': { 'top': 0, 'left': 0 }
                };

                try {
                    var editor = new columnDef.editor(editorArgs);
                    editor.loadValue(item);
                    editor.applyValue(item, value);
                    editor.destroy();
                    $container.remove();
                } catch (error) {
                }
            } else {
            }
        }

        function decodeTabularData(ta) {
            var columns = _grid.getColumns();
            var clipText = ta.value;
            var clipRows = clipText.split(/[\n\f\r]/);
            var clippedRange = [];

            _bodyElement.removeChild(ta);

            for (var i = 0; i < clipRows.length; i++) {
                if (clipRows[i] != "") {
                    clippedRange[i] = clipRows[i].split("\t");
                }
            }

            var selectedCell = _grid.getActiveCell();
            var ranges = _grid.getSelectionModel().getSelectedRanges();
            var selectedRange = ranges && ranges.length ? ranges[0] : null;
            var activeRow = null;
            var activeCell = null;

            if (selectedRange) {
                activeRow = selectedRange.fromRow;
                activeCell = selectedRange.fromCell;
            } else if (selectedCell) {
                activeRow = selectedCell.row;
                activeCell = selectedCell.cell;
            } else {
                return;
            }

            //rownumber can not be pasted
            var column = columns[activeCell];
            if (column.isRowSelector && column.isRowSelector == true) {
                activeCell = activeCell + 1;
            } else {
                ;
            }

            var oneCellToMultiple = false;
            var destH = clippedRange.length;
            var destW = clippedRange.length ? clippedRange[0].length : 0;

            if (clippedRange.length == 1 && clippedRange[0].length == 1 && selectedRange) {
                oneCellToMultiple = true;
                destH = selectedRange.toRow - selectedRange.fromRow + 1;
                destW = selectedRange.toCell - selectedRange.fromCell + 1;
            }

            var avaliableRows = _grid.getData().length - activeRow;
            var addRows = 0;
            if (avaliableRows < destH) {
                //生成默认的新记录
                var newRow = _self.onAddNewRows.notify({});
                var d = _grid.getData();
                for (addRows = 1; addRows <= destH - avaliableRows; addRows++) {
                    d.push(newRow);
                }
                _grid.setData(d);
                _grid.render();
            }

            var clipCommand = {
                isClipboardCommand: true,
                clippedRange: clippedRange,
                oldValues: [],
                cellExternalCopyManager: _self,
                _options: _options,
                setDataItemValueForColumn: setDataItemValueForColumn,
                markCopySelection: markCopySelection,
                oneCellToMultiple: oneCellToMultiple,
                activeRow: activeRow,
                activeCell: activeCell,
                destH: destH,
                destW: destW,
                desty: activeRow,
                destx: activeCell,
                maxDestY: _grid.getDataLength(),
                maxDestX: _grid.getColumns().length,
                h: 0,
                w: 0,

                execute: function () {
                    this.h = 0;
                    for (var y = 0; y < destH; y++) {
                        this.oldValues[y] = [];
                        this.w = 0;
                        this.h++;

                        for (var x = 0; x < destW; x++) {
                            this.w++;
                            var desty = activeRow + y;
                            var destx = activeCell + x;

                            if (desty < this.maxDestY && destx < this.maxDestX) {
                                _grid.getData()[desty]["sta"] = 0;

                                var nd = _grid.getCellNode(desty, destx);
                                var dt = _grid.getDataItem(desty);

                                this.oldValues[y][x] = dt[columns[destx]['id']];

                                if (oneCellToMultiple)
                                    this.setDataItemValueForColumn(dt, columns[destx], clippedRange[0][0], desty, destx);
                                else
                                    this.setDataItemValueForColumn(dt, columns[destx], clippedRange[y][x], desty, destx);

                                _grid.updateCell(desty, destx);

                            }
                        }
                    }

                    if (addRows) {
                        _grid.setData(d);
                    } else {
                        var d1 = _grid.getData();
                        _grid.setData(d1);
                    }
                    _grid.render();

                    var bRange = {
                        'fromCell': activeCell,
                        'fromRow': activeRow,
                        'toCell': activeCell + this.w - 1,
                        'toRow': activeRow + this.h - 1
                    };

                    this.markCopySelection([bRange]);
                    _grid.getSelectionModel().setSelectedRanges([bRange]);
                    _self.onPasteCells.notify({ ranges: [bRange] });
                },

                undo: function () {
                    for (var y = 0; y < destH; y++) {
                        for (var x = 0; x < destW; x++) {
                            var desty = activeRow + y;
                            var desttx = activeCell + x;

                            if (desty < this.maxDestY && destx < this.maxDestX) {
                                var nd = _grid.getCellNode(desty, destx);
                                var dt = _grid.getDataItem(desty);
                                if (oneCellToMultiple)
                                    this.setDataItemValueForColumn(dt, columns[destx], this.oldValues[0][0]);
                                else
                                    this.setDataItemValueForColumn(dt, columns[destx], this.oldValues[y][x]);

                                if (_dataview) {
                                    _dataview.updateItem(dt.ID, dt);
                                } else {
                                    _grid.updateCell(desty, destx);
                                }
                            }
                        }
                    }

                    var bRange = {
                        'fromCell': activeCell,
                        'fromRow': activeRow,
                        'toCell': activeCell + this.w - 1,
                        'toRow': activeRow + this.h - 1
                    };

                    this.markCopySelection([bRange]);
                    _grid.getSelectionModel().setSelectedRanges([bRange]);
                    _self.onPasteCells.notify({ ranges: [bRange] });

                    if (addRows > 1) {
                        var d = _grid.getData();
                        for (; addRows > 1; addRows--)
                            d.splice(d.length - 1, 1);

                        _grid.setData(d);
                        _grid.render();
                    }
                }
            };

            if (_options.clipboardCommandHandler) {
                _options.clipboardCommandHandler(clipCommand);
            } else {
                clipCommand.execute();
            }
        }

        function createTextArea(innerText) {
            var ta = document.createElement('textarea');
            ta.style.position = 'absolute';
            ta.style.left = '-1000px';
            ta.style.top = document.body.scrollTop + 'px';
            ta.value = innerText;
            _bodyElement.appendChild(ta);
            ta.select();

            return ta;
        }

        function handleKeyDown(e, args) {
            var ranges;
            if ($(e.target).hasClass("slick-focusSink") || $(e.target).hasClass("slick-cell")) {
                if (e.which == keyCodes.ESC) {      //ESC
                    if (_copiedRanges) {
                        e.preventDefault();
                        clearCopySelection();
                        _self.onCopyCancelled.notify({ ranges: _copiedRanges });
                        _copiedRanges = null;
                    }
                }

                if (e.which == keyCodes.ALT)
                {
                    e.stopImmediatePropagation();
                    ranges = _grid.getSelectionModel().getSelectedRanges();
                    if (ranges.length != 0) {
                        var dt = _grid.getData();
                        var columns = _grid.getColumns();
                        var rangesdt=_grid.getDataItem(ranges[0].fromRow);
                        for (var i = ranges[0].fromRow; i <= ranges[0].toRow; i++)
                        {
                            for (var j = ranges[0].fromCell; j <= ranges[0].toCell; j++)
                            {
                                var cell = _grid.getVisibleColumns()[j].field;
                                if (columns[j].editor != undefined) {
                                    dt[i][cell] = rangesdt[cell];
                                }
                            }
                            _grid.updateRow(i);
                        }
                        _grid.updateRowCount();
                        _self.onAltCope.notify({ ranges: ranges });
                    }
                }
                if (e.which == keyCodes.C && (e.ctrlKey || e.metaKey)) {    //Ctrl+C
                    ranges = _grid.getSelectionModel().getSelectedRanges();

                    if (ranges.length != 0) {
                        _copiedRanges = ranges;
                        markCopySelection(ranges);
                        _self.onCopyCells.notify({ ranges: ranges });

                        var column, columns = _grid.getColumns();
                        var clipTextArr = [];

                        for (var rg = 0; rg < ranges.length; rg++) {
                            var range = ranges[rg];
                            var clipTextRows = [];

                            for (var i = range.fromRow; i < range.toRow + 1; i++) {
                                var clipTextCells = [];
                                var dt = _grid.getDataItem(i);
                                //rownumber can not be copyed
                                for (var j = range.fromCell; j < range.toCell + 1; j++) {
                                    column = columns[j];
                                    if (column.isRowSelector && column.isRowSelector == true) {
                                        continue;
                                    }
                                    clipTextCells.push(getDataItemValueForColumn(i, dt, columns[j]));
                                }
                                clipTextRows.push(clipTextCells.join("\t"));
                            }
                            clipTextArr.push(clipTextRows.join("\r\n"));
                        }

                        var clipText;
                        if (ranges.length > 1) {
                            clipText = clipTextArr.join('\r\n');
                        } else {
                            clipText = clipTextArr.join('');
                        }

                        var $focus = $(_grid.getActiveCellNode());
                        var ta = createTextArea(clipText);
                        ta.focus();

                        setTimeout(function () {
                            _bodyElement.removeChild(ta);
                            if ($focus && $focus.length > 0) {
                                $focus.attr('tabIndex', '-1');
                                $focus.focus();
                                $focus.removeAttr('tabIndex');
                            }
                        }, 100);
                        return false;
                    }
                }
                pasteCellRange(e);      //Ctrl + V

                return false;
            }
            pasteCellRange(e);
        }

        function pasteCellRange(e) {
            if ((e.which == keyCodes.V) && (e.ctrlKey || e.metaKey)) {  //Ctrl+V
                var ta = createTextArea('');
                setTimeout(function () {
                    decodeTabularData(ta);
                }, 100);
            }
        }


        function markCopySelection(ranges) {
            clearCopySelection();
            var columns = _grid.getColumns();
            var hash = {};
            for (var i = 0; i < ranges.length; i++) {
                for (var j = ranges[i].fromRow; j <= ranges[i].toRow; j++) {
                    hash[j] = {};
                    for (var k = ranges[i].fromCell; k <= ranges[i].toCell && k < columns.length; k++) {
                        hash[j][columns[k].id] = _copiedCellStyle;
                    }
                }
            }
            _grid.setCellCssStyles(_copiedCellStyleLayerKey, hash);
            clearTimeout(_clareCopyTI);
            _clareCopyTI = setTimeout(function () {
                _self.clearCopySelection();
            }, 2000);
        }

        function clearCopySelection() {
            _grid.removeCellCssStyles(_copiedCellStyleLayerKey);
        }

        $.extend(this, {
            "init": init,
            "destroy": destroy,
            "clearCopySelection": clearCopySelection,
            "handleKeyDown": handleKeyDown,

            "onCopyCells": new Slick.Event(),
            "onCopyCancelled": new Slick.Event(),
            "onPasteCells": new Slick.Event(),
            "onAltCope": new Slick.Event(),
            "onAddNewRows": new Slick.Event()
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

        function exportExcel(fileName) {
            $hidden = $("<input type='hidden' id='fileName' name='fileName'/>")
                .appendTo($("#exportform"));

            if (fileName) {
                $hidden.val(fileName);
            }

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
                var canvasNode = grid.getCanvasNode(range.fromCell);
                _elem = $("<div style='border: 2px solid black;z-index:999;'></div>", { css: options.selectionCss })
                    .css("position", "absolute")
                    .appendTo(canvasNode);
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
        var _columnOffset, _hasRightCanvas;
        var _self = this;
        var _handler = new Slick.EventHandler();
        var _defaults = {
            selectionCss: {
                "z-index": 999,
                "border": "2px dashed blue"
            }
        };

        function init(grid) {
            options = $.extend(true, {}, _defaults, options);
            _decorator = new Slick.Cell.RangeDecorator(grid, options);
            _grid = grid;
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
            _columnOffset = 0;
            _hasRightCanvas = _grid.frozenColumn > -1;
            if (_hasRightCanvas) {
                _columnOffset = $('.grid-canvas-left').width();
            }

            var cell = _grid.getCellFromEvent(e);
            _canvas = _grid.getCanvasNode(cell.cell);

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

            dd.range = { start: cell, end: {} };
            return _decorator.show(new Slick.Range(cell.row, cell.cell));
        }

        function handleDrag(e, dd) {
            if (!_dragging) {
                return;
            }
            e.stopImmediatePropagation();

            var end;
            if (_hasRightCanvas == false) {
                end = _grid.getCellFromEvent(e);
            } else {
                end = _grid.getCellFromPoint(
                    e.pageX - $(_canvas).offset().left + _columnOffset,
                    e.pageY - $(_canvas).offset().top
                );
            }

            if (!end || !_grid.canCellBeSelected(end.row, end.cell)) {
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
                if ($node && $node[0] && ($node.innerWidth() < $node[0].scrollWidth)) {
                    text = $.trim($node.text());
                    if (options.maxToolTipLength && text.length > options.maxToolTipLength) {
                        text = text.substr(0, options.maxToolTipLength - 3) + "...";
                    }
                } else {
                    text = "";
                }

                if ($node) {
                    $node.attr("title", text);
                }
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
//#endregion

//#region Slick.ColumnPlug JS
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
                var checked = e.target.checked;
                _grid.getDataItem([args.row])["checked"] = checked;

                toggleRowSelection(args.row, checked);
                e.stopPropagation();
                e.stopImmediatePropagation();
                _self.onCheckBoxClicked.notify({ "row": args.row, "checked": e.target.checked });
            }
        }

        function toggleRowSelection(row, checked) {
            if (checked == true) {
                if (!_selectedRowsLookup[row]) {
                    _grid.setSelectedRows(_grid.getSelectedRows().concat(row));
                    _selectedRowsLookup[row] = row;
                }
            } else {
                _grid.setSelectedRows($.grep(_grid.getSelectedRows(), function (n) {
                    return n != row
                }));
                delete _selectedRowsLookup[row];
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
                    //checkbox 置为选中状态
                    $("input:checkbox.slick-checkbox-firstColumn").each(function () {
                        $(this)[0].checked = true;
                    });
                    //setTimeout(function () {
                    _grid.setSelectedRows(rows);
                    //}, 50);
                } else {
                    //checkbox 置为未选中
                    $("input:checkbox.slick-checkbox-firstColumn").each(function () {
                        $(this)[0].checked = false;
                    });
                    //setTimeout(function () {
                    _grid.setSelectedRows([]);
                    //}, 50);
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
//#endregion

//#region Slick.ControlExtension JS
(function ($) {
    //column picker
    $.extend(true, window, {
        "Slick": {
            "Controls": {
                "Pager": SlickGridPager,
                "PagerSvr": SlickGridPagerSvr,
                "ColumnPicker": SlickColumnPicker,
                "filterDialog": SlickFilterDialog,
                "FooterSummary": FooterSummary
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
                    if (options && options.isCompact && options.isCompact == true) {
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
            { action: "saveStyle", bindType: "click", text: "保存界面", iconImage: "/Common/Content/default/js/slickgrid/images/save.png" },
            { action: "restoreStyle", bindType: "click", text: "还原默认界面" },
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

                $popMenu.css('height', '20px');
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

                    if (columns[i].id) {
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
                height = height > 500 ? 500 : height;
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
                    grid.render();
                }
                return;
            }

            if ($(e.target).data("option") == "syncresize") {
                if (e.target.checked) {
                    grid.setOptions({ syncColumnCellResize: true });
                } else {
                    //grid.setOptions({ syncColumnCellResize: false });
                    grid.resizeCanvas();
                }
                return;
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
                    return;
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
                else if (item.action == "saveStyle") {
                    saveStyle();
                }
                else if (item.action == "restoreStyle") {
                    restoreStyle();
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

        function saveStyle() {
            trigger(self.onStyleSave, {});
        }

        function restoreStyle() {
            trigger(self.onStyleRestore, {});
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
            "onDataColumnSort": new Slick.Event(),
            "onStyleSave": new Slick.Event(),
            "onStyleRestore": new Slick.Event()
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
            $currentLogicLink = $(e.currentTarget);
            $logicMenu
                .css("top", $currentLogicLink[0].offsetTop + 10)
                .css("left", $currentLogicLink[0].offsetLeft + 10)
                .fadeIn(150);
        }

        //弹出列标题菜单
        function onfieldNameLinkClick(e) {
            $currentFieldNameLink = $(e.currentTarget);
            $fieldMenu
                .css("top", $currentFieldNameLink[0].offsetTop + 10)
                .css("left", $currentFieldNameLink[0].offsetLeft + 10)
                .fadeIn(150);
        }

        //弹出运算符菜单
        function onOperatorLinkClick(e) {
            $currentOperatorLink = $(e.currentTarget);

            $operatorMenu
                .css("top", $currentOperatorLink[0].offsetTop + 10)
                .css("left", $currentOperatorLink[0].offsetLeft + 10)
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

    //#region 底部统计行
    function FooterSummary(dataview, grid, $container) {
        var $status;
        var columnFilters = {};
        var columns, items, columnSummaries = {};
        var prevscrollLeft = 0;

        function handleDataChanged(e, args) {
            var rows = [];
            for (var i = 0; i < dataview.getLength() ; i++) {
                rows.push(dataview.getItem(i));
            }

            items = rows;

            constructSummaries();
            constructSummaryFooterUI();
        }

        function init() {
            grid.onScroll.subscribe(function (e, args) {
                if (typeof $footerScroller != 'undefined') {
                    $footerScroller.scrollLeft(args.scrollLeft);
                    prevscrollLeft = args.scrollLeft;
                }
            });
            grid.onColumnsResized.subscribe(function (e, args) {
                var column = args.column;
                var width = args.width - 9;
                var footerId = grid.getGridId() + columns[column].id + "_summary";

                $("#" + footerId).css("width", width);
            });
            dataview.onPagingInfoChanged.subscribe(handleDataChanged);
            dataview.onRowsChanged.subscribe(function (e, args) {
                grid.invalidateRows(args.rows);
                grid.render();
                handleDataChanged(e, args);
            });
            columns = grid.getColumns();
        }

        function constructSummaries() {
            columnSummaries = {};

            for (var i = 0, len = items.length; i < len; i++) {
                var row = items[i];

                for (var k = 0, lenK = columns.length; k < lenK; k++) {
                    var m = columns[k];
                    var value = row[m.field];

                    if (m.footerSummary && m.footerSummary == true) {
                        if (!isNaN(value)) {
                            if (!columnSummaries[m.id]) {
                                columnSummaries[m.id] = 0;
                            }
                            columnSummaries[m.id] += value;
                        }
                    }
                }
            }
        }

        function constructSummaryFooterUI() {
            $container.empty();

            $footerScroller = $("<div class='slick-footer ui-state-default' style='overflow:hidden;position:relative;' />").appendTo($container);
            $footers = $("<div class='slick-footer-columns' style='left:-1000px'/>").appendTo($footerScroller);


            $container.children().wrapAll("<div class='slick-summaryfooter'/>");
            function onMouseEnter() {
                $(this).addClass("ui-state-hover");
            }

            function onMouseLeave() {
                $(this).removeClass("ui-state-hover");
            }

            $footers.find(".slick-footer-column")
                .each(function () {
                    var columnDef = $(this).data("column");
                });
            $footers.empty();

            $footers.find(".slick-footerrow-column")
                .each(function () {
                    var columnDef = $(this).data("column");
                });
            $footers.empty();

            var headerWidth = grid.getHeadersWidth();
            $footers.width(headerWidth);

            for (var i = 0; i < columns.length; i++) {
                var m = columns[i];
                var value = "";
                if (columnSummaries[m.id]) {
                    if (m.footerSummary && m.footerSummary == true) {
                        value = (Math.round(parseFloat(columnSummaries[m.id]) * 100) / 100);
                    }
                }

                var footer = $("<div class='ui-state-default slick-footer-column slick-summaryfooter-column' id='" + grid.getGridId() + m.id + "_summary'/>")
                    .html("<span class='slick-column-name' title='" + value + "'>" + value + "</span>")
                    .width(m.width - grid.getHeaderColumnWidthDiff())
                    .attr("title", m.toolTip || "")
                    .data("column", m)
                    .addClass(m.headerCssClass || "")
                    .appendTo($footers);
            }
            //记住滚动条位置
            if (prevscrollLeft > 0) {
                $footerScroller.scrollLeft(prevscrollLeft);
            }
        }

        init();
    }
    //#endregion

})(jQuery);
//#endregion

//#region Slick.HeaderExtension JS
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
            grid.createColumnHeaders();

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
                var items = dataview.getItems();
                var filterItems = unique(items, column.field);
                for (var i = 0; i < filterItems.length; i++) {
                    appendHeaderMenuItem(filterItems[i], column, headerMenu);
                }
            }
        }

        function appendHeaderMenuItem(item, column, headerMenu) {
            var isAppend = true, txtView = "";
            if (column.fieldType == "boolean") {
                if (parseBool(item) == true)
                    txtView = "是";
                else if (parseBool(item) == false)
                    txtView = "否";
                else
                    isAppend = false;
            } else {
                txtView = item;
            }

            if (isAppend == true) {
                headerMenu.push({
                    ID: i,
                    Text: txtView,
                    Value: item,
                    itemValueType: "dataItem"
                });
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
                if (checked == true) {
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
            //var height = length * 15;
            //height = height > 500 ? 500 : height;
            //$menu.css('height', height + "px");

            var height = 24 * length;
            var maxHeight = $(window).height() - 150;
            if (height > maxHeight) {
                $menu.css('height', maxHeight + "px");
            }
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
            if ($menu) $menu.remove();
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
//#endregion

//#region Slick.Filter JS
(function ($) {
    //数据过滤
    $.extend(true, window,
    {
        "Slick": {
            "Data": {
                "SlickFilter": SlickFilter,
                "SlickColumnDefSimplify": SlickColumnDefSimplify,
                "SlickColumnDataSource": SlickColumnDataSource,
                "SlickExpressionFormatted": SlickExpressionFormatted,
                "SlickExpressionFieldType": SlickExpressionFieldType
            }
        }
    });

    //#region 获取各列字段的定义
    function SlickColumnDefSimplify(columns) {
        var col;
        var simpleCol = [];
        for (var i = 0; i < columns.length; i++) {
            if (columns[i].id == "_checkbox_selector")
                continue;

            var hasFilter = columns[i].hasFilter;
            if (!hasFilter || hasFilter == false)
                continue;

            col = {
                id: columns[i].id,
                name: columns[i].name,
                field: columns[i].field,
                fieldType: columns[i].fieldType,
                hasFilter: columns[i].hasFilter
            };
            simpleCol.push(col);
        }
        return simpleCol;
    }
    //#endregion

    //#region 获取列绑定的数据源
    function SlickColumnDataSource(columns, field) {
        var column;
        for (var i = 0; i < columns.length; i++) {
            column = columns[i];
            if (column.field == field && column.fieldType == "dropdownlist")
                return column.dataSource;
        }
        return null;
    }
    //#endregion

    //#region 根据字段类型格式化表达式
    function SlickExpressionFormatted(fieldName, fieldType, expression, value) {
        if (fieldType == "datetime")
            expression = expression.replaceAll(fieldName, Date.parse(value));
        else if (fieldType == "string")
            expression = expression.replaceAll(fieldName, "\'" + value + "\'");
        else
            expression = expression.replaceAll(fieldName, value);

        return expression;
    }
    //#endregion

    //#region 根据字段名称获取字段类型
    function SlickExpressionFieldType(colRangeType, colInfo, fieldName) {
        var col, fieldType;
        if (colRangeType == "multiplecolumn") {
            for (var i = 0; i < colInfo.length; i++) {
                col = colInfo[i];
                if (col.field == fieldName) {
                    fieldType = col["fieldType"];
                    break;
                }
            }
        }
        else if (colRangeType == "onecolumn") {
            if (colInfo.field == fieldName) {
                fieldType = colInfo["fieldType"];
            }
        }

        return fieldType;
    }
    //#endregion

    //#region 过滤器实现类
    function SlickFilter() {
        //表达式解析器
        function filterParser(item, args) {
            var result = true;
            var valueExpression;
            var filterType = args.filterType;
            var expression = args.expression;

            //根据过滤类型判断
            if (filterType == "clear") {
                //清除过滤选项，直接返回
                return true;
            }
            else if (filterType == "single") {
                //根据字段类型生成表达式
                var fieldName = "[" + args.field + "]";

                valueExpression = Slick.Data.SlickExpressionFormatted(fieldName, args.fieldType, expression, item[args.field]);
                result = eval(valueExpression);
            }
            else if (filterType == "complex") {
                var fieldName, fieldNameBracket, fieldType;
                var colInfo = args.field;

                //匹配字段名称并替换
                var regex = /\[([^\]]*)\]/g;
                while (match = regex.exec(expression)) {
                    fieldName = match[1];
                    fieldNameBracket = "[" + fieldName + "]";
                    fieldType = Slick.Data.SlickExpressionFieldType(args.colRangeType, colInfo, fieldName);
                    expression = Slick.Data.SlickExpressionFormatted(fieldNameBracket, fieldType, expression, item[fieldName]);
                }

                result = eval(expression);
            }
            return result;
        }

        $.extend(this, {
            "filterParser": filterParser
        });
    }
    //#endregion

})(jQuery);
//#endregion

