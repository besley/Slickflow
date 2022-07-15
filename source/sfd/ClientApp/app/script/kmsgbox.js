

const kmsgbox = (function () {
    function kmsgbox() {
    }

    kmsgbox.warn = function (message, detailMsg) {
        if (detailMsg !== undefined && detailMsg !== '')
            message = message + detailMsg;

        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_WARNING,
            title: kresource.getItem('warning'),
            message: message,
            buttons: [{
                label: kresource.getItem('close'),
                action: function (dialogRef) {
                    dialogRef.close();
                }
            }]
        });
    }

    kmsgbox.error = function (message, detailMsg) {
        if (detailMsg !== undefined && detailMsg !== '')
            message = message + detailMsg;

        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_DANGER,
            title: kresource.getItem('error'),
            message: message,
            buttons: [{
                label: kresource.getItem('close'),
                action: function (dialogRef) {
                    dialogRef.close();
                }
            }]
        });
    }

    kmsgbox.info = function (message) {
        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_PRIMARY,
            title: kresource.getItem('info'),
            message: message,
            buttons: [{
                label: kresource.getItem('close'),
                action: function (dialogRef) {
                    dialogRef.close();
                }
            }]
        });
    }

    kmsgbox.success = function (message) {
        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_SUCCESS,
            title: kresource.getItem('success'),
            message: message,
            buttons: [{
                label: kresource.getItem('close'),
                action: function (dialogRef) {
                    dialogRef.close();
                }
            }]
        });
    }

    kmsgbox.confirm = function (message, okFunc) {
        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_INFO,
            title: kresource.getItem('info'),
            message: message,
            buttons: [{
                label: kresource.getItem('yes'),
                action: function (dialogRef) {
                    okFunc();
                    dialogRef.close();
                }
            }, {
                label: kresource.getItem('no'),
                action: function (dialogRef) {
                    dialogRef.close();
                }
             }]
        });
    }
    return kmsgbox;
})();

export default kmsgbox