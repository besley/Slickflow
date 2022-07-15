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

    if (value < 30 || value < 3) {
      color = "red";
    } else if (value < 70 || value < 7) {
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
      return value ? "<img src='/Common/Content/default/images/yes.png'>" : "<img src='/Common/Content/default/images/no.png'>";
  }

  function CheckBoxVisibleFormatter(row, cell, value, columnDef, dataContext) {
      var checkbox;

      if (value) {
          checkbox = "<INPUT type=checkbox checked='true' class='editor-checkbox'/>";
      }
      else {
          checkbox = "<INPUT type=checkbox class='editor-checkbox'/>";
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
      if (nextRowIndex < rowsLength){
          var nextValue = items[nextRowIndex][fieldName];
          if (value == nextValue) {
              if (!options.changedCells[row]) {
                  options.changedCells[row] = {};
              }
              options.changedCells[row][fieldName] = "noneline-bottom";
              options.isNextMerged = 1;
              return value;
          }
          else {
              if (options.isNextMerged == 1) {
                  options.isNextMerged = 0;
                  return;
              }
          }
      }
      return value;
  }
    //#endregion

  function hyperLinkInTabFormatter(row, cell, value, columnDef, dataContext) {
      var action = columnDef.linkAction;
      var actionName = action();
      var actionName = actionName + "(" + row + ");";
      var link = "<a href='#' onclick='" + actionName + "' rowIndex='" + row + "'>" + value + "</a>";
      return link;
  }

  function hyperLinkNewPageFormatter(row, cell, value, columnDef, dataContext) {
      var action = columnDef.linkAction;
      var queryStrings = action(dataContext);

      return "<a href='" + columnDef.linkUrl + queryStrings + "' target='_blank'>" + value + '</a>';
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