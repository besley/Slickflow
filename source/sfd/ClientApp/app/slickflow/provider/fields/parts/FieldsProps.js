import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export default function FieldsProps({ element, injector, event }) {
    const bpmnFactory = injector.get('bpmnFactory'),
        commandStack = injector.get('commandStack');
    const items = [];
    return {
        items,
        add: addFactory({ element, bpmnFactory, commandStack })
    };
}

function addFactory({ element, bpmnFactory, commandStack }) {
    const businessObject = getBusinessObject(element);
    var activityID = businessObject.get("id");
    var activityName = businessObject.get("name");
    var activityCode = businessObject.get('code');

    var processElement = element.parent;
    var processBusinessObject = getBusinessObject(processElement);
    var processID = processBusinessObject.get("id");
    var processCode = processBusinessObject.get("code");
    var processName = processBusinessObject.get("name");

    var fieldActivityInfo = {
        "ProcessID": processID,
        "ProcessCode": processCode,
        "ProcessName": processName,
        "ProcessVersion": processlist.pselectedProcessEntity !== null ? processlist.pselectedProcessEntity.Version : "1",
        "ActivityID": activityID,
        "ActivityCode": activityCode,
        "ActivityName": activityName
    }

    return function (event) {
        event.stopPropagation();

        if ("undefined" !== typeof fieldlist) {
            fieldlist.FieldActivityInfo = fieldActivityInfo;
            fieldlist.afterFieldActivityEditSelected.subscribe(afterFieldActivityEditSelected);
        }

        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupContent"></div>'),
            title: kresource.getItem("fieldlist"),
            onshown: function () {
                $("#popupContent").load('pages/field/list.html')
            },
            onhidden: function () {
                executeMultiCommad(element, bpmnFactory, commandStack);
            },
            draggable: true
        });

    }
}

function afterFieldActivityEditSelected(evt, data) {
    
}

function executeMultiCommad(element, bpmnFactory, commandStack) {
}