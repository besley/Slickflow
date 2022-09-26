var kmsgbox = (function () {
    function kmsgbox() {
    }

    kmsgbox.warn = function (message, detailMsg) {
        if (detailMsg !== undefined && detailMsg !== '')
            message = message + detailMsg;

        $.msgBox({
            title: kresource.getItem('warning'),
            content: message,
            type: "warn"
        });
    }

    kmsgbox.error = function (message, detailMsg) {
        if (detailMsg !== undefined && detailMsg !== '')
            message = message + detailMsg;

        $.msgBox({
            title: kresource.getItem('error'),
            content: message,
            type: "error"
        });
    }

    kmsgbox.info = function (message) {
        $.msgBox({
            title: kresource.getItem('info'),
            content: message,
            type: "info"
        });
    }

    kmsgbox.confirm = function (message, okFunc) {
        $.msgBox({
            title: kresource.getItem('confirmation'),
            content: message,
            type: "confirm",
            buttons: [{ value: "Yes" }, { value: "Cancel" }],
            success: okFunc
        });
    }
    
    return kmsgbox;
})();