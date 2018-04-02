
//var javaObj;

var units = ["seconds", "minutes", "hours", "daysOfMonth", "months", "daysOfWeek"];

var isSecondsEnabled = false;

var secondsProps, minutesProps, hoursProps, daysOfMonthProps, monthsProps, daysOfWeekProps;

var dontChangeTabs = false;

function initUnits() {
    secondsProps = new unit(1, 0, [], "*");
    minutesProps = new unit(1, 0, [], "*");
    hoursProps = new unit(3, 0, [2], "2");
    daysOfMonthProps = new unit(1, 0, [], "*");
    monthsProps = new unit(1, 0, [], "*");
    daysOfWeekProps = new unit(1, 0, [], "*");
}

initUnits();

function unit(mode, interval, array, text) {
    this.mode = mode;
    this.interval = interval;
    this.units = array;
    this.text = text;
};


$.updateCronExpression = function () {

    var txt = "";
    $.each(units, function (index, value) {
        if (!isSecondsEnabled && value === "seconds") {

        }
        else {
            var props = window[value + "Props"];
            if (props.mode == 3) {
                //console.log("mode == 3: " + value);
                if (units.length > 0) {
                    props.units.sort(function (a, b) { return a > b ? +1 : a < b ? -1 : 0; });

                    //var unitTxt = props.units.toString();
                    var unitTxt = "";

                    var isSequence = false;
                    for (var i = 0; i < props.units.length; i++) {
                        var current = props.units[i];
                        if (i < props.units.length - 1) {
                            var next = props.units[i + 1];

                            if (next === (current + 1)) {
                                if (isSequence === false) {
                                    isSequence = true;
                                    unitTxt += current + "-";
                                }
                            }
                            else {
                                isSequence = false;
                                unitTxt += current + ",";
                            }
                        }
                        else {
                            unitTxt += current;
                        }

                    }

                    props.text = unitTxt;
                    //console.log(unitTxt);
                }
            }

            txt += props.text;
            if (index < units.length - 1) {
                txt += " ";
            }

            $.updateUnitTabsExpression(value);
            $.updateAccordionExpression(value);
        }
    });

    $("#cronExpressionValue").html(txt);

    $.fillTriggerDatetimes();

    //$.updateCronGui();

}


$.updateCronGui = function () {
    $.each(units, function (index, value) {
        $.resetSlider(value);
        $.resetUnitButtons(value);
        $.resetEveryButton(value);
        var unitPropsName = value + "Props";
        $.putAsteriskOnTab(window[unitPropsName].mode, value);
        $.setGuiUnitValues(value, window[unitPropsName].text);
        $.setActiveUnitTab(value, window[unitPropsName].mode);
    });
}

$.setActiveUnitTab = function (unit, mode) {
    if (dontChangeTabs) {
        return;
    }

    var tabName;
    if (mode == 1) {
        tabName = "tabEvery_" + unit;
    }
    else if (mode == 2) {
        tabName = "tabEach_" + unit;
    }
    else if (mode == 3) {
        tabName = "tabSelected_" + unit;
    }
    $("#" + tabName).click();
}

$.setGuiUnitValues = function (unit, txt) {
    if (txt.includes("*") && !txt.includes("/")) {
        $("#everyUnit_" + unit).removeClass('btn-success').removeClass('btn-default').addClass('btn-success');
    }
    else if (txt.includes("*") && txt.includes("/")) {
        $("#eachUnit_" + unit).removeClass('topcoat-range-input-active').removeClass('topcoat-range-input').addClass('topcoat-range-input-active');
        var unitPropsName = unit + "Props";
        $("#eachUnit_" + unit).val(window[unitPropsName].interval);
        /*
        var splits = txt.split("/");
        $(this).val(splits[1]);
        */
    }
    else {
        var splits = txt.split(",");
        var i;
        for (i = 0; i < splits.length; ++i) {
            if (!splits[i].includes("-")) {
                $("#selectedUnit_" + unit + splits[i]).each(function () {
                    $(this).removeClass('btn-success');
                    $(this).removeClass('btn-default');
                    $(this).addClass('btn-success');
                });
            }
            else {
                var splits2 = splits[i].split("-");
                var j;
                for (j = parseInt(splits2[0]); j <= parseInt(splits2[1]); j++) {
                    $("#selectedUnit_" + unit + j).each(function () {
                        $(this).removeClass('btn-success');
                        $(this).removeClass('btn-default');
                        $(this).addClass('btn-success');
                    });
                }
            }
        }
    }
}

$.updateUnitTabsExpression = function (unit) {
    var unitPropsName = unit + "Props";
    var unitProps = window[unitPropsName];

    //alert("name: " + unitPropsName + ", props: " + unitProps);

    $("div[id^='" + unit + "Text']").html(unitProps.text);
}

$.updateAccordionExpression = function (unit) {
    var unitPropsName = unit + "Props";
    var unitProps = window[unitPropsName];

    //alert("name: " + unitPropsName + ", props: " + unitProps);

    $("div[id^='content_" + unit + "']").html("[ " + unitProps.text + " ]");
}

$.fillTriggerDatetimes = function () {
    $("#triggerDateTimes").empty();

    //var cron = '15 10 * * ? *';
    var cron = $("#cronExpressionValue").html();
    var s;
    if (isSecondsEnabled) {
        s = later.parse.cron(cron, true);
    }
    else {
        s = later.parse.cron(cron);
    }

    var result = later.schedule(s).next(16);

    $(result).each(function (index, value) {
        var dateString = value.toISOString().substring(0, 19).replace("T", " ");
        $("#triggerDateTimes").append("<li class='list-group-item'>" + dateString + "</li>");
    });
}

$.setmode = function (mode, unit) {

    var unitPropsName = unit + "Props";

    var unitProps = window[unitPropsName];

    //alert("name: " + unitPropsName + ", props: " + unitProps);


    if (unitProps.mode != mode) {
        unitProps.mode = mode;
        switch (mode) {
            case 1:
                unitProps.interval = 0;
                unitProps.units = [];
                $.resetSlider(unit);
                $.resetUnitButtons(unit);
                break;
            case 2:
                unitProps.units = [];
                $.resetUnitButtons(unit);
                $.resetEveryButton(unit);
                break;
            case 3:
                unitProps.interval = 0;
                $.resetSlider(unit);
                $.resetEveryButton(unit);
                break;
        }

        $.putAsteriskOnTab(mode, unit);
    }

    switch (mode) {
        case 1:
            unitProps.text = "*";
            break;
        case 2:
            unitProps.text = "*/" + $("#eachUnit_" + unit).val();
            break;
        case 3:
            if (typeof unitProps.units !== 'undefined' && unitProps.units.length > 0) {
                unitProps.text = unitProps.units.toString();
            }
            else {
                unitProps.text = "*";
            }
            break;
    }

    $.updateCronExpression();
    $.updateCronGui();
}

$.resetSlider = function (unit) {
    $("#eachUnit_" + unit).val(0);
    $("#eachUnit_" + unit).removeClass('topcoat-range-input-active').addClass('topcoat-range-input');
}

$.resetUnitButtons = function (unit) {
    $("button[id*='selectedUnit_" + unit + "']").each(function () {
        $(this).removeClass('btn-success').addClass('btn-default');
    });
}

$.resetEveryButton = function (unit) {
    $("#everyUnit_" + unit).removeClass('btn-success').addClass('btn-default');
}

$.putAsteriskOnTab = function (mode, unit) {
    //console.log("mode " + mode + ", unit " + unit);
    $("#tabEvery_" + unit).children("span").remove();
    $("#tabEach_" + unit).children("span").remove();
    $("#tabSelected_" + unit).children("span").remove();
    switch (mode) {

        case 1:
            $("#tabEvery_" + unit).append(" <span class='glyphicon glyphicon-asterisk glyphiconTab' aria-hidden='true'></span>");
            break;
        case 2:
            $("#tabEach_" + unit).append(" <span class='glyphicon glyphicon-asterisk glyphiconTab' aria-hidden='true'></span>");
            break;
        case 3:
            $("#tabSelected_" + unit).append(" <span class='glyphicon glyphicon-asterisk glyphiconTab' aria-hidden='true'></span>");
            break;
    }
}

//Accordion-GUI
$("div[id*='accordionheader']").click(function () {
    var isActiveCollapsed = false;
    if ($(this).hasClass("activeAccordion")) {
        isActiveCollapsed = true;
    }


    $("div[id^='accordionheader']").removeClass('activeAccordion').addClass('inactiveAccordion');
    $("div[id^='accordionheader']").find('span').removeClass('glyphicon-triangle-bottom').addClass('glyphicon-triangle-right');
    if (!isActiveCollapsed) {
        $(this).removeClass('inactiveAccordion').addClass('activeAccordion');
        $(this).find('span').removeClass('glyphicon-triangle-right').addClass('glyphicon-triangle-bottom');
    }
});

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    for (var i = 0; i < 6; i++) {
        for (var j = 0; j < 10; j++) {
            $('#selectSeconds').append('<button id="selectedUnit_seconds' + (i * 10 + j) + '" type="button" class="btn btn-default selectedUnitButton">' + (i * 10 + j) + '</button>');
            $('#selectMinutes').append('<button id="selectedUnit_minutes' + (i * 10 + j) + '" type="button" class="btn btn-default selectedUnitButton">' + (i * 10 + j) + '</button>');
            if (i < 3 && !(i == 2 && j > 3)) {
                $('#selectHours').append('<button id="selectedUnit_hours' + (i * 10 + j) + '" type="button" class="btn btn-default selectedUnitButton">' + (i * 10 + j) + '</button>');
            }
            if (i < 4 && !(i == 3 && j > 1) && !(i == 0 && j == 0)) {
                $('#selectDays').append('<button id="selectedUnit_daysOfMonth' + (i * 10 + j) + '" type="button" class="btn btn-default selectedUnitButton">' + (i * 10 + j) + '</button>');
            }
        }
        $('#selectSeconds').append('<br>');
        $('#selectMinutes').append('<br>');
        if (i < 3) {
            $('#selectHours').append('<br>');
        }
        if (i < 4) {
            $('#selectDays').append('<br>');
        }

    }

    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    for (var i = 0; i < months.length; i++) {
        $('#selectMonths').append('<button id="selectedUnit_months' + (i + 1) + '" type="button" class="btn btn-default selectedUnitButton bigUnitButton">' + months[i] + '</button>');
        if (i != 0 && (i + 1) % 6 == 0) {
            $('#selectMonths').append('<br>');
        }
    }

    var daysOfWeek = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    for (var i = 0; i < daysOfWeek.length; i++) {
        $('#selectDaysOfWeek').append('<button id="selectedUnit_daysOfWeek' + (i) + '" type="button" class="btn btn-default selectedUnitButton bigUnitButton">' + daysOfWeek[i] + '</button>');
    }

    $("button[id*='everyUnit']").click(function () {
        var unit = this.id.substring(10, this.id.length);
        var unitPropsName = unit + "Props";

        if ($(this).hasClass("btn-default")) {
            $.setmode(1, unit);
            $(this).removeClass('btn-default').addClass('btn-success disabled');
        }

    });

    $("button[id*='selectedUnit']").click(function () {
        var name = this.id.substring(13, this.id.length);
        var unit = name.replace(/[0-9]/g, "")
        var unitId = name.replace(/^\D+/g, '');

        var unitPropsName = unit + "Props";
        var unitProps = window[unitPropsName];
        //alert(unitPropsName);

        if ($(this).hasClass("btn-default")) {
            unitProps.units.push(parseInt(unitId));
            $.setmode(3, unit);
            $(this).removeClass('btn-default').addClass('btn-success');
        }
        else {
            var index = unitProps.units.indexOf(parseInt(unitId));
            if (index > -1) {
                /*
                var removeItem = units[index];
                units = jQuery.grep(units, function(value) {
                    return value != removeItem;
                });
                */
                unitProps.units.splice(index, 1);

                $(this).removeClass('btn-success').addClass('btn-default');
                //alert(unit);

                if (unitProps.units.length > 0) {
                    $.setmode(3, unit);
                }
                else {
                    dontChangeTabs = true;
                    $.setmode(1, unit);

                    //console.log("UNIT=" + unit);
                    unitProps.mode = 1;
                    unitProps.interval = 0;
                    unitProps.units = [];
                    unitProps.text = "*";

                    $("button[id*='everyUnit_" + unit + "']").removeClass('btn-default').addClass('btn-success disabled');

                    $.updateCronExpression();
                    $.putAsteriskOnTab(unitProps.mode, unit);
                    $.setGuiUnitValues(unit, unitProps.txt);
                    $.setActiveUnitTab(unit, 3);
                    dontChangeTabs = false;
                }
            }
        }
        unitProps.units.sort(function (a, b) { return a > b ? +1 : a < b ? -1 : 0; });
    });

    $("input[id*='eachUnit']").change(function () {
        var unit = this.id.substring(9, this.id.length);
        var unitPropsName = unit + "Props";
        var unitProps = window[unitPropsName];
        unitProps.interval = $(this).val();

        //alert(unitPropsName + " " + unitProps.interval)
        $.setmode(2, unit);

        $(this).removeClass('topcoat-range-input').addClass('topcoat-range-input-active');
    });

    $("#setSeconds").click(function () {

        isSecondsEnabled = !isSecondsEnabled;
        if (isSecondsEnabled) {
            $("#setSeconds").append("<span class='glyphicon glyphicon-ok' aria-hidden='true'></span>");

        }
        else {
            $(".glyphicon-ok").remove();
        }

        $("#secondsPanel").toggleClass('hidden');
        $.updateCronExpression();
        $.updateCronGui();

    });

    $.updateCronExpression();
    $.updateCronGui();


    //$(function(){ alert("test"); });
});

function isCronOk(cronText) {


    /*
    //doesnt work
    var isOk = true;

    var sched =later.parse.cron(editField.value);
    if (sched.error != -1)
    {
        isOk = true;
    }
    else
    {
        isOk = false;
    }
    console.log("expression is " + isOk + ", expressionValue = " + editField.value + ", Schedules = " + sched.toString() + ", Error = " + sched.error);
                var result = later.schedule(sched).next(5);
    for (i = 0; i < result.length; i++) { 
        console.log(result[i].toISOString().substring(0, 19).replace("T"," "));
    }
    */

    cronText = cronText.trim();
    var units = cronText.split((/[\s]+/));
    //console.log(units.length);
    //check 5 units
    var unitsNo;
    var hourPos;
    if (!isSecondsEnabled) {
        unitsNo = 5;
        hourPos = 1;
    }
    else {
        unitsNo = 6;
        hourPos = 2;
    }

    if (units.length != unitsNo) {
        return false;
    }

    var reg = new RegExp('^[0-9,/* \-]+$');
    if (!reg.test(cronText)) {
        return false;
    }

    var splits = cronText.split(" ");
    loop1:
    for (var i = 0; i < splits.length; i++) {
        if (splits[i] === "*") {
            continue;
        }

        if (splits[i].length > 1 && splits[i].substring(0, 2) === "*/") {
            if ((isSecondsEnabled && i > 2) || (!isSecondsEnabled && i > 1)) {
                //console.log(i + ">" + (unitsNo - 4));
                return false;
            }
            if (isNumber(splits[i].substring(2, splits[i].length)) && !splits[i].includes("-")) {

                var n = parseInt(splits[i].substring(2, splits[i].length));
                var result = false;

                //console.log(n + " " + hourPos);
                if (i === hourPos) {
                    if (n >= 0 && n <= 23) {
                        continue;
                    }
                    else {
                        return false;
                    }
                }

                if (n >= 0 && n <= 60) {
                    continue;
                }

                return false;
            }
            else {
                return false;
            }
        }


        var reg2 = new RegExp('^[0-9,\-]+$');
        if (!reg2.test(splits[i])) {
            //console.log("reg2");
            return false;
        }
        var unitSplits = splits[i].split(",");
        //console.log(unitSplits.toString() + " " + unitSplits.length);
        if (unitSplits.length === 0) {
            return false;
        }
        if (splits[i].substring(0, 1) === "," || splits[i].substring(splits[i].length - 1, splits[i].length) == ",") {
            return false;
        }

        var result = false;
        for (var j = 0; j < unitSplits.length; j++) {
            //console.log(unitSplits[j].toString());
            if (isNumber(unitSplits[j])) {

                if (!unitSplits[j].includes("-")) {
                    //console.log("hourPos = " + hourPos + " " + unitSplits[j].toString() + " " + i);
                    var n = parseInt(unitSplits[j]);
                    //console.log("n = " + n);
                    if (i === hourPos) {
                        if (n >= 0 && n <= 23) {
                            result = true;
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        if (n >= 0 && n <= 60) {
                            result = true;
                        }
                        else {
                            return false;
                        }
                    }
                }
                else {
                    var rangeSplits = unitSplits[j].split("-");
                    for (var k = 0; k < rangeSplits.length; k++) {
                        var n = parseInt(rangeSplits[k]);
                        if (i === hourPos) {
                            if (n >= 0 && n <= 23) {
                                result = true;
                            }
                            else {
                                return false;
                            }
                        }
                        else {
                            if (n >= 0 && n <= 60) {
                                result = true;
                            }
                            else {
                                return false;
                            }
                        }
                    }
                }

            }
            else {
                return false;
            }
        }

        //console.log("result = " + result);
        if (result) {
            continue loop1;
        }

        return false;
    }


    return true;
}

function isNumber(obj) { return !isNaN(parseFloat(obj)) }

function validateCron() {
    var editField = document.getElementById('cronEditField');
    var parent = editField.parentElement;
    var glyph = document.getElementById('cronEditFieldGlyph');
    var okButton = document.getElementById('okButton');

    var isOk = isCronOk(editField.value);

    //console.log("expression is " + isOk);

    if (isOk) {
        parent.classList.remove("has-error");
        parent.classList.add("has-success");
        glyph.classList.remove("glyphicon-remove");
        glyph.classList.add("glyphicon-ok");
        okButton.classList.remove("disabled");
        okButton.disabled = false;
    }
    else {
        parent.classList.remove("has-success");
        parent.classList.add("has-error");
        glyph.classList.remove("glyphicon-ok");
        glyph.classList.add("glyphicon-remove");
        okButton.classList.add("disabled");
        okButton.disabled = true;
    }
}

function setJava(java) {
    javaObj = java;
    /*
    BootstrapDialog.show({
        message: 'Hi Apple! ' + javaObj + ' test'
            });
            */
}

function callJava() {

    //console.log('test');
    //if(typeof javaObj != 'undefined') // Any scope)
    /*
    if (window['javaObj'] != void 0)
    {
javaObj.exit();
BootstrapDialog.show({
    message: 'javaObj ' + javaObj
        });
}
else
{
BootstrapDialog.show({
    message: 'javaObj is undefined'
        });
}
	
*/
}

document.getElementById("editButton").addEventListener("click", function () {

    var cronExpr = $("#cronExpressionValue").text();

    $("#cronEditField").change(function () {
        alert("Handler for .change() called.");
    });

    BootstrapDialog.show({
        title: "<span class='glyphicon glyphicon-edit glyphIconDialogTitle'></span>编辑 Cron 表达式",
        message: function (dialogRef) {
            var $content = $("<form class='form-inline'><div class='form-group has-success has-feedback'><input type='text' class='form-control' onInput='validateCron()' id='cronEditField' value='" + cronExpr + "'><span id='cronEditFieldGlyph' class='glyphicon glyphicon-ok form-control-feedback'></span></div></form>");

            return $content;
        },
        buttons: [
            {
                id: 'okButton',
                label: '确定',
                action: function (dialogRef) {
                    var cronExpr = dialogRef.getModalBody().find('input').val();
                    $("#cronExpressionValue").html(cronExpr);
                    $.syncCronExpression(cronExpr);
                    
                    $.updateCronExpression();
                    $.updateCronGui();
                    dialogRef.close();
                }
            },
            {
                id: 'cancelBtn',
                label: '取消',
                action: function (dialogRef) {
                    dialogRef.close();
                }
            }
        ],
        cssClass: 'editcron-dialog',
    });

});

$.syncCronExpression = function (cronExpr) {
    var splits = cronExpr.split(" ");
    //console.log("SPlits: " + splits.toString());
    var i;
    if (isSecondsEnabled) {
        i = 0;
    }
    else {
        i = 1;
    }
    var j = 0;
    while (i < units.length) {
        var unitPropsName = units[i] + "Props";
        var txt = splits[j];
        window[unitPropsName].text = txt;
        //console.log(unitPropsName + ".text = " + txt);

        if (txt.includes("*") && !txt.includes("/")) {
            window[unitPropsName].mode = 1;
        }
        else if (txt.includes("*") && txt.includes("/")) {
            window[unitPropsName].mode = 2;
            window[unitPropsName].interval = txt.split("/")[1];
        }
        else {
            window[unitPropsName].mode = 3;
            var unitSplits = txt.split(",");
            //console.log("Unitsplits: " + unitSplits);
            var unitsArray = [];
            for (var k = 0; k < unitSplits.length; k++) {
                if (!unitSplits[k].includes("-")) {
                    unitsArray[unitsArray.length] = parseInt(unitSplits[k]);
                }
                else {
                    var splits2 = unitSplits[k].split("-");
                    for (var m = parseInt(splits2[0]); m <= parseInt(splits2[1]); m++) {
                        unitsArray[unitsArray.length] = m;
                    }
                }
            }
            //console.log("UnitsArray: " + unitsArray.toString());
            window[unitPropsName].units = unitsArray;
        }

        i++; j++;
    }
}


document.getElementById("resetButton").addEventListener("click", function () {

    BootstrapDialog.show({
        title: "<span class='glyphicon glyphicon-question-sign glyphIconDialogTitle'></span>确认提示",
        message: '确定要重置 Cron 表达式吗?',
        buttons: [ {
            label: '确定',
            action: function (dialog) {
                initUnits();
                $.each(units, function (index, value) {
                    $.resetSlider(value);
                    $.resetUnitButtons(value);
                });
                $.updateCronExpression();
                $.updateCronGui();
                dialog.close();
            }
        }, {
            label: '取消',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });

});

document.getElementById("copyButton").addEventListener("click", function () {
    copyToClipboard(document.getElementById("cronExpressionValue"));

    BootstrapDialog.show({
        title: "<span class='glyphicon glyphicon-info-sign glyphIconDialogTitle'></span>信息提示",
        message: '已经复制 Cron 表达式 <b>[ ' + document.getElementById("cronExpressionValue").textContent + ' ]</b> 到剪贴板中!',
        buttons: [{
            label: '确定',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });

});

function copyToClipboard(elem) {
    // create hidden text element, if it doesn't already exist
    var targetId = "_hiddenCopyText_";
    var isInput = elem.tagName === "INPUT" || elem.tagName === "TEXTAREA";
    var origSelectionStart, origSelectionEnd;
    if (isInput) {
        // can just use the original source element for the selection and copy
        target = elem;
        origSelectionStart = elem.selectionStart;
        origSelectionEnd = elem.selectionEnd;
    } else {
        // must use a temporary form element for the selection and copy
        target = document.getElementById(targetId);
        if (!target) {
            var target = document.createElement("textarea");
            target.style.position = "absolute";
            target.style.left = "-9999px";
            target.style.top = "0";
            target.id = targetId;
            document.body.appendChild(target);
        }
        target.textContent = elem.textContent;
    }
    // select the content
    var currentFocus = document.activeElement;
    target.focus();
    target.setSelectionRange(0, target.value.length);

    // copy the selection
    var succeed;
    try {
        succeed = document.execCommand("copy");
    } catch (e) {
        succeed = false;
    }
    // restore original focus
    if (currentFocus && typeof currentFocus.focus === "function") {
        currentFocus.focus();
    }

    if (isInput) {
        // restore prior selection
        elem.setSelectionRange(origSelectionStart, origSelectionEnd);
    } else {
        // clear temporary content
        target.textContent = "";
    }
    return succeed;
}
