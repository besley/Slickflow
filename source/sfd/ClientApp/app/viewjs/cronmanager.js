
var cronmanager = (function () {
    function cronmanager() {
    }


    //save activity basic information
    cronmanager.saveCron = function () {
        var cronExpression = $("#cronExpressionValue").text();
        $("#txtCronExpression").val(cronExpression);
    }

    return cronmanager;
})()