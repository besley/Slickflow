import kconfig from '../config/kconfig.js';

const userlist = (function () {
    function userlist() {
    }
    userlist.afterRecipientSelected = new slick.Event();

    var pselectedRecipientType = "";
    var pselectedRecipientItem = null;

    userlist.getUserList = function () {
        jshelper.ajaxPost(kconfig.webApiUrl + "api/Wf2Xml/GetUserAll", null, function (result) {
            if (result.Status === 1) {
                var divUserGrid = document.querySelector('#myUserGrid');
                var gridOptions = {
                    theme: themeBalham,
                    columnDefs: [
                        { headerName: 'Id', field: 'UserId', width: 60 },
                        { headerName: kresource.getItem('username'), field: 'UserName', width: 200 },
                        { headerName: kresource.getItem('email'), field: 'EMail', width: 200 }
                    ],
                    rowSelection: {
                        mode: 'singleRow',
                        checkboxes: false,
                        enableClickSelection: true
                    },
                    onSelectionChanged: onSelectionChanged,
                    onRowDoubleClicked: onRowDoubleClicked
                };

                gridOptions.rowData = result.Entity;
                const gridApi = createGrid(divUserGrid, gridOptions);

                function onSelectionChanged() {
                    var selectedRows = gridApi.getSelectedRows();
                    selectedRows.forEach(function (selectedRow, index) {
                        pselectedRecipientType = "User";
                        pselectedRecipientItem = selectedRow;
                    });
                }

                function onRowDoubleClicked(e, args) {
                    userlist.sure();
                }
            }
        });
    }

    userlist.sure = function () {
        if (pselectedRecipientType != ""
            && pselectedRecipientItem != null) {
            slick.trigger(userlist.afterRecipientSelected, {
                "RecipientType": pselectedRecipientType,
                "RecipientItem": pselectedRecipientItem
            });
        } else {
            kmsgbox.warn(kresource.getItem('userlistselectwarnmsg'));
        }
    }
    return userlist;
})()

export default userlist