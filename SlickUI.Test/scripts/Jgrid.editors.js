/***
 * Contains basic SlickGrid editors.
 * @module Editors
 * @namespace Slick
 */

(function ($) {
  // register namespace
  $.extend(true, window, {
      "Jgrid": {
      "Editors": {
        "Text": TextEditor,
        //"Integer": IntegerEditor,
          //"Date": DateEditor,
        "DropDownList": DropDownList
        //"YesNoSelect": YesNoSelectEditor,
        //"Checkbox": CheckboxEditor,
        //"PercentComplete": PercentCompleteEditor,
        //"LongText": LongTextEditor
      }
    }
  });

  function TextEditor(args) {
    var $input;
    var defaultValue;
    var scope = this;
    var cursorInSelectObj = false, changed = false;
    this.cancel = function () { return cursorInSelectObj; }
    this.changed = function () { return changed; }

    this.init = function () {
        var _html = "<textarea style='backround:white;width:250px;height:80px;border:0;outline:0' rows='5' hidefocus=''></textarea>";
        //if()//
        $input = $("<INPUT type=text class='editor-text' />")
          .appendTo(args.container)
          .bind("keydown.nav", function (e) {
              if (e.keyCode === 37 || e.keyCode === 39) {//$.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT
                  e.stopImmediatePropagation();
              }
          })
          .focus()
          .select()
        .blur(function () {
            cursorInSelectObj = false;
            //args.callback();
        })
        .change(function () {
            changed = true;
            //args.callback();
        });

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

    this.validate = function () {
      if (args.column.validator) {
        var validationResults = args.column.validator($input.val());
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
        buttonImage: "../images/calendar.gif",
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
      var cursorInSelectObj = true, changed = false;
      this.cancel = function () { return cursorInSelectObj; }
      this.changed = function () { return changed; }

      this.init = function () {
          if (dataSouce != undefined && dataSouce != null) {
              $select = this.preRender(args.grid.$selDropdownlistDatasource);
              if (changedEvent != null)
                  $select.change(function () {
                      changedEvent(this, { "ID": $select.val(), "txt": $select.find('option:selected').text(), clumn: args.column, x: args.x });
                      changed = true;
                  });

              $select.appendTo(args.container);
              $select.focus(function () {
                  //cursorInSelectObj = true;
                  $("#txt_cancelmemo").val($("#txt_cancelmemo").val() + "focus;");
              });
              $select.blur(function () {
                  //cursorInSelectObj = false;
                  //$("#txt_cancelmemo").val($("#txt_cancelmemo").val() + "blur;");
                  //if (!changed) {
                  //    $(args.container).empty();
                  //    $(args.container).html(defaultValue)
                  //}
                  args.callback();
              })
              .change(function () {
                  changed = true; cursorInSelectObj = false;
                  $("#txt_cancelmemo").val($("#txt_cancelmemo").val() + "change;");
                  args.callback();
              });
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
              return $(args.container).find("input").val();//$select.find('option:selected').text();
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
      item[args.column.field] = state;
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
  //function LongTextEditor(args) {
  //  var $input, $wrapper;
  //  var defaultValue;
  //  var scope = this;

  //  this.init = function () {
  //    var $container = $("body");

  //    $wrapper = $("<DIV style='z-index:10000;position:absolute;background:white;padding:5px;border:3px solid gray; -moz-border-radius:10px; border-radius:10px;'/>")
  //        .appendTo($container);

  //    $input = $("<TEXTAREA hidefocus rows=5 style='backround:white;width:250px;height:80px;border:0;outline:0'>")
  //        .appendTo($wrapper);

  //    $("<DIV style='text-align:right'><BUTTON>Save</BUTTON><BUTTON>Cancel</BUTTON></DIV>")
  //        .appendTo($wrapper);

  //    $wrapper.find("button:first").bind("click", this.save);
  //    $wrapper.find("button:last").bind("click", this.cancel);
  //    $input.bind("keydown", this.handleKeyDown);

  //    scope.position(args.position);
  //    $input.focus().select();
  //  };

  //  this.handleKeyDown = function (e) {
  //    if (e.which == $.ui.keyCode.ENTER && e.ctrlKey) {
  //      scope.save();
  //    } else if (e.which == $.ui.keyCode.ESCAPE) {
  //      e.preventDefault();
  //      scope.cancel();
  //    } else if (e.which == $.ui.keyCode.TAB && e.shiftKey) {
  //      e.preventDefault();
  //      args.grid.navigatePrev();
  //    } else if (e.which == $.ui.keyCode.TAB) {
  //      e.preventDefault();
  //      args.grid.navigateNext();
  //    }
  //  };

  //  this.save = function () {
  //    args.commitChanges();
  //  };

  //  this.cancel = function () {
  //    $input.val(defaultValue);
  //    args.cancelChanges();
  //  };

  //  this.hide = function () {
  //    $wrapper.hide();
  //  };

  //  this.show = function () {
  //    $wrapper.show();
  //  };

  //  this.position = function (position) {
  //    $wrapper
  //        .css("top", position.top - 5)
  //        .css("left", position.left - 5)
  //  };

  //  this.destroy = function () {
  //    $wrapper.remove();
  //  };

  //  this.focus = function () {
  //    $input.focus();
  //  };

  //  this.loadValue = function (item) {
  //    $input.val(defaultValue = item[args.column.field]);
  //    $input.select();
  //  };

  //  this.serializeValue = function () {
  //    return $input.val();
  //  };

  //  this.applyValue = function (item, state) {
  //    item[args.column.field] = state;
  //  };

  //  this.isValueChanged = function () {
  //    return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
  //  };

  //  this.validate = function () {
  //    return {
  //      valid: true,
  //      msg: null
  //    };
  //  };

  //  this.init();
  //}
})(jQuery);

/***
 * Contains basic SlickGrid formatters.
 * @module Formatters
 * @namespace Slick
 */

(function ($) {
    // register namespace
    $.extend(true, window, {
        "Jgrid": {
            "Formatters": {
                "PercentComplete": PercentCompleteFormatter,
                "PercentCompleteBar": PercentCompleteBarFormatter,
                "YesNo": YesNoFormatter,
                "Checkmark": CheckmarkFormatter
            }
        }
    });

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

    function YesNoFormatter(row, cell, value, columnDef, dataContext) {
        return value ? "Yes" : "No";
    }

    function CheckmarkFormatter(row, cell, value, columnDef, dataContext) {
        return value ? "<img src='../images/tick.png'>" : "";
    }
})(jQuery);