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
                "TextButton" : TextButtonEditor,
                "PercentComplete": PercentCompleteEditor,
                "LongText": LongTextEditor,
                "SelectCell": SelectCellEditor
            }
        }
    });

    function TextEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $input = $("<INPUT type=text class='editor-text' />")
                .bind("dblclick", function (e) {
                    return args.grid.onDblClick.notify(args, e, this);
                })
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
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
                    return  { valid: false, msg: "字段:[" + args.column.name + "]不能为空！" };
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
            $input = $("<INPUT type=text class='editor-text' />");

            $input.bind("keydown.nav", function (e) {
                if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                    e.stopImmediatePropagation();
                }
            });

            $input.appendTo(args.container);
            $input.focus().select();
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
            $input = $("<INPUT type=text class='editor-text' />");

            $input.bind("keydown.nav", function (e) {
                if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                    e.stopImmediatePropagation();
                }
            });

            $input.appendTo(args.container);
            $input.focus().select();
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
            $input = $("<INPUT type=text class='editor-text' />");
            $input.appendTo(args.container);
            $input.focus().select();
            $input.datepicker({
                showOn: "button",
                buttonImageOnly: true,
                buttonImage: "/Common/Content/default/images/calendar.gif",
                beforeShow: function () {
                    calendarOpen = true
                },
                onClose: function () {
                    calendarOpen = false
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
            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    function DropDownList(args) {
        var $select = $("");
        var defaultValue = "";
        var scope = this;
        var dataSouce = args.grid.$selDropdownlistDatasource;
        var changedEvent = args.grid.$selDropdownlistChangedEvent;

        this.init = function () {
            if (dataSouce != undefined && dataSouce != null) {
                $select = this.preRender(args.grid.$selDropdownlistDatasource);
                if (changedEvent != null)
                    $select.change(function () {
                        changedEvent(this, { "ID": $select.val(), "txt": $select.find('option:selected').text() });
                    });

                $select.appendTo(args.container);
                $select.focus();
            }            
        };

        this.preRender = function (dataSource) {
            var option_str = "";
            var preSelect = "<SELECT tabIndex='0' class='editor-select'><OPTION value='-1'></OPTION>";
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
            $(args.container).empty();
        };

        this.focus = function () {
            $select.focus();
        };

        this.serializeValue = function () {
            if ($select.val() != -1) {
                return $select.find('option:selected').text();
            }
        };

        this.applyValue = function (item, state) {
            if (state != undefined) {
                item[args.column.field] = state;
            }
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field];
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
                }
                $select.focus();
            }
        };

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
            $(args.container).empty();
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
            }

            var isSelected = $select.data("editableSelect").editableSelect.$textbox.data("isSelected");
            if (text == "" && isSelected == true) {
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
        }

        this.isValueChanged = function () {
            return ($select.val() != defaultValue);
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

            $txtBox.bind("keydown.nav", function (e) {
                if (e.keyCode == $.ui.keyCode.LEFT || e.keyCode == $.ui.keyCode.RIGHT) {
                    e.stopImmediatePropagation();
                }
            })

            //$button.bind('click', args.container, function (e) {
            //    e.stopImmediatePropagation();
            //    args.grid.onTextButtonClick(args.container);
            //});

            $button.bind('click', args, function (e) {
                e.stopImmediatePropagation();
                args.grid.onTextButtonClick(args);
            });
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
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
            if (defaultValue) {
                $select.attr("checked", "checked");
            } else {
                $select.removeAttr("checked");
            }
        };

        this.serializeValue = function () {
            return $select.attr("checked");
        };

        this.applyValue = function (item, state) {
            //item[args.column.field] = state;
            if (state == "checked")
                item[args.column.field] = true;
            else
                item[args.column.field] = false;
        };

        this.isValueChanged = function () {
            return ($select.attr("checked") != defaultValue);
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
            $select = $("input:checkbox", args.container)
            $select.focus();
        };

        this.destroy = function () {
            //$select.remove();
        };

        this.focus = function () {
            $select.focus();
        };

        this.loadValue = function (item) {
            //has loaded value by checkboxvisible formatter
        };

        this.serializeValue = function () {
            return $select.attr("checked");
        };

        this.applyValue = function (item, state) {
            if (state == "checked")
                item[args.column.field] = true;
            else
                item[args.column.field] = false;
        };

        this.isValueChanged = function () {
            return ($select.attr("checked") != defaultValue);
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
})(jQuery);