import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';



import {
    createElement,
    createSubInfoes,
    getSubInfoes,
    getSubInfoesExtension
} from '../util';

import { forEach, without } from 'min-dash';

export default function SubInfoesProps({ element, injector, event }) {
    const subInfoes = getSubInfoes(element) || [];
    const bpmnFactory = injector.get('bpmnFactory'),
        commandStack = injector.get('commandStack');

    const items = subInfoes.map((subInfo, index) => {
        const id = element.id + '-subinfo-' + subInfo.get('subId');
        return {
            id,
            label: subInfo.get('subProcessName') || '',
            autoFocusEntry: id + '-processname',
            remove: removeFactory({ commandStack, element, subInfo })
        };
    });

    return {
        items,
        add: addFactory({ element, bpmnFactory, commandStack })
    };
}

function addFactory({ element, bpmnFactory, commandStack }) {
    return function (event) {
        event.stopPropagation();

        if ("undefined" !== typeof processlist) {
            processlist.afterSubProcessSelected.subscribe(afterSubProcessSelected);
        }

        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupContent"></div>'),
            title: kresource.getItem("processlist"),
            onshown: function () {
                $("#popupContent").load('pages/subprocess/list.html')
            },
            onhidden: function () {
                executeMultiCommand(element, bpmnFactory, commandStack);
            },
            draggable: true
        });
    }
}

var mxSubProcessSelected = null;
var mxSubProcessType = null;

function afterSubProcessSelected(evt, data) {
    mxSubProcessSelected = data.ProcessEntity;
}

function isSubInfoExisted(element) {
    var isExisted = false;
    const subInfoes = getSubInfoes(element) || [];
    const subInfo = mxSubProcessSelected;
    if (subInfoes !== null) {
        subInfoes.forEach(function (item, index) {
            if (item.ID === subInfo.ID) {
                isExisted = true;
            }
        })
    }
    return isExisted;
}

function executeMultiCommand(element, bpmnFactory, commandStack) {
    if (isSubInfoExisted(element) === true) {
        return;
    }

    const commands = [];
    const businessObject = getBusinessObject(element);

    let extensionElements = businessObject.get('extensionElements');

    if (!extensionElements) {
        extensionElements = createElement(
            'bpmn:ExtensionElements',
            { values: [] },
            businessObject,
            bpmnFactory
        );

        commands.push({
            cmd: 'element.updateModdleProperties',
            context: {
                element,
                moddleElement: businessObject,
                properties: { extensionElements}
            }
        });
    }

    let extension = getSubInfoesExtension(element);

    if (!extension) {
        extension = createSubInfoes({
            values: []
        }, extensionElements, bpmnFactory);

        commands.push({
            cmd: 'element.updateModdleProperties',
            context: {
                element,
                moddleElement: extensionElements,
                properties: {
                    values: [...extensionElements.get('values'), extension]
                }
            }
        });
    }

    if (mxSubProcessSelected !== null) {
        const newSubInfo = createElement('sf:SubInfo', {
            subId: mxSubProcessSelected.ID,
            subProcessGUID: mxSubProcessSelected.ProcessGUID,
            subProcessName: mxSubProcessSelected.ProcessName,
            subType: '',
            subVar: ''
        }, extension, bpmnFactory);

        commands.push({
            cmd: 'element.updateModdleProperties',
            context: {
                element,
                moddleElement: extension,
                properties: {
                    values: [...extension.get('values'), newSubInfo]
                }
            }
        });
        commandStack.execute('properties-panel.multi-command-executor', commands);
    }
}

function removeFactory({ commandStack, element, subInfo }) {
    return function (event) {
        event.stopPropagation();
        const extension = getSubInfoesExtension(element);
        if (!extension) {
            return;
        }

        const subInfoes = without(extension.get('values'), subInfo);
        commandStack.execute('element.updateModdleProperties', {
            element,
            moddleElement: extension,
            properties: {
                values: subInfoes
            }
        });
    }
}


