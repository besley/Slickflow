import kconfig from '../config/kconfig.js';
const formlist = (function () {
    function formlist() {
    }

    formlist.afterFormSelected = new slick.Event();
    var pselectedFormItem = null;

    formlist.getFormList = function () {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/wf2xml/GetFormAll', null, function (result) {
            if (result.Status === 1) {
                var divFormGrid = document.querySelector('#myFormGrid');
                var gridOptions = {
                    theme: themeBalham,
                    columnDefs: [
                        { headerName: "Id", field: "FormId", width: 60 },
                        { headerName: kresource.getItem('formName'), field: "FormName", width: 200 },
                        { headerName: kresource.getItem('formCode'), field: "FormCode", width: 200 }
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
                const gridApi = createGrid(divFormGrid, gridOptions);

                function onSelectionChanged() {
                    var selectedRows = gridApi.getSelectedRows();
                    selectedRows.forEach(function (selectedRow, index) {
                        pselectedFormItem = selectedRow;
                    })
                }

                function onRowDoubleClicked(e, args) {
                    formlist.sure();
                }
            }
        });
    }

    formlist.sure = function () {
        if (pselectedFormItem !== null) {
            slick.trigger(formlist.afterFormSelected, {
                "FormItem": pselectedFormItem
            })
        } else {
            kmsgbox.warn(kresource.getItem('formlistselectwarnmsg'));
        }
    }

    return formlist
})()

export default formlist;

