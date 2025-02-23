import kconfig from '../config/kconfig.js';
const fieldlist = (function () {
    function fieldlist() {
    }

    fieldlist.afterFieldActivityEditSelected = new slick.Event();
    fieldlist.FieldActivityInfo = null;
    fieldlist.pmxFormList = null;
    fieldlist.pmxCellValueChanged = false;
    var pselectedFieldActivityEditItem = null;

    fieldlist.init = function () {
        onFireFormListChanged();
        fillFieldActivityEditList(null);
        fieldlist.getFieldActivityEditList();
    }

    function onFireFormListChanged() {
        $("#ddlFormList").on("change", function () {
            var selectedFormID = $(this).val(); 
            //fill form list into dropdown box
            fieldlist.pmxFormList.forEach(function (form, index) {
                if (form.FormID === selectedFormID) {
                    var fieldEditDataSource = fieldlist.pmxFormList[index].FieldActivityEditList;
                    fillFieldActivityEditList(fieldEditDataSource);
                    return;
                }
            })
        });
    }

    fieldlist.getFieldActivityEditList = function () {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/wf2xml/GetFieldActivityEditInfo', JSON.stringify(fieldlist.FieldActivityInfo), function (result) {
            if (result.Status === 1) {
                //Form List 
                var formList = result.Entity;    
                fieldlist.pmxFormList = formList;

                //fill form list into dropdown box
                formList.forEach(function (form, index) {
                    $('#ddlFormList')
                        .append($('<option>')
                            .text(form.FormName)
                            .attr('value', form.FormID)
                        );
                })

                if (formList && formList.length > 0) {
                    $('#ddlFormList option:first').attr('selected', 'selected');
                }

                if (formList && formList.length > 0) {
                    //fill field list grid
                    var firstForm = formList[0];

                    var fieldEditDataSource = firstForm.FieldActivityEditList;
                    if (fieldEditDataSource) {
                        fillFieldActivityEditList(fieldEditDataSource);
                    }
                }
            }
        });
    }

    function fillFieldActivityEditList(fieldDataSource) {
        //Field List
        var divFieldActivityEditGrid = document.querySelector('#myFieldActivityEditGrid');
        $(divFieldActivityEditGrid).empty();

        var gridOptions = {
            theme: themeBalham,
            columnDefs: [
                { headerName: kresource.getItem("fieldName"), field: "FieldName", width: 240 },
                { headerName: kresource.getItem('isNotVisible'), field: "IsNotVisible", cellDataType: 'boolean', editable: true, cellRenderer: 'agCheckboxCellRenderer', cellEditor: "agCheckboxCellEditor", width: 120 },
                { headerName: kresource.getItem('isReadOnly'), field: "IsReadOnly", cellDataType: 'boolean', editable: true, cellRenderer: 'agCheckboxCellRenderer', cellEditor: 'agCheckboxCellEditor', width: 120 }
            ],
            rowSelection: {
                mode: 'singleRow',
                checkboxes: false,
                enableClickSelection: true
            },
            onSelectionChanged: onSelectionChanged,
            onCellValueChanged: onCellValueChanged
        };

        gridOptions.rowData = fieldDataSource;
        const gridApi = createGrid(divFieldActivityEditGrid, gridOptions);

        function onSelectionChanged() {
            var selectedRows = gridApi.getSelectedRows();
            selectedRows.forEach(function (selectedRow, index) {
                pselectedFieldActivityEditItem = selectedRow;
            })
        }

        function onCellValueChanged(e, args) {
            fieldlist.pmxCellValueChanged = true;
        }
    }

    fieldlist.save = function () {
        if (fieldlist.pmxCellValueChanged === false) {
            return;
        }

        var index = $('#ddlFormList option:selected').index();
        var selectedFormID = $('#ddlFormList').val();
        var selectedFormName = $('#ddlFormList').text();
        var fieldEditDataSource = fieldlist.pmxFormList[index].FieldActivityEditList;

        //If there is field activity edit list data, then confirm to save
        if (fieldEditDataSource && fieldEditDataSource.length > 0) {
            kmsgbox.confirm(kresource.getItem('fieldactivityeditlistsaveconfirmmsg'), function () {
                var activity = fieldlist.FieldActivityInfo;
                var entity = {
                    "ProcessID": activity.ProcessID,
                    "ProcessID": activity.ProcessID,
                    "ProcessVersion": activity.ProcessVersion,
                    "ProcessName": activity.ProcessName,
                    "ActivityID": activity.ActivityID,
                    "ActivityName": activity.ActivityName,
                    "FormID": selectedFormID,
                    "FormName": selectedFormName,
                    "FieldActivityEditList": fieldEditDataSource
                };

                //save field activity edit info into database
                jshelper.ajaxPost(kconfig.webApiUrl + 'api/wf2xml/SaveFieldActivityEditInfo', JSON.stringify(entity), function (result) {
                    if (result.Status === 1) {
                        kmsgbox.info(kresource.getItem('fieldactivityeditlistsaveokmsg'));
                    } else {
                        kmsgbox.error(kresource.getItem('fieldactivityeditlistsaveerrormsg'), result.Message);
                    }
                });
                return;
            });
        } else {
            kmsgbox.warn(kresource.getItem('fieldactivityeditlistsavewarnmsg'));
        }
    }

    return fieldlist
})()

export default fieldlist;

