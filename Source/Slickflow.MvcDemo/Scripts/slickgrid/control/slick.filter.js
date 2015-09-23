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