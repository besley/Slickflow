import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';



import {
    createElement,
    createPerformers,
    getPerformers,
    getPerformersExtension
} from '../util';

import { forEach, without } from 'min-dash';

export default function PerformersProps({element, injector, event}) {
    const performers = getPerformers(element) || [];
    const bpmnFactory = injector.get('bpmnFactory'),
        commandStack = injector.get('commandStack');

    const items = performers.map((performer, index) => {
        const id = element.id + '-performer-' + index;
        return {
            id,
            label: performer.get('name') || '',
            autoFocusEntry: id + '-name',
            remove: removeFactory({ commandStack, element, performer })
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

        if ("undefined" !== typeof rolelist) {
            rolelist.afterParticipantSelected.subscribe(afterParticipantSelected);
        }

        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupContent"></div>'),
            title: kresource.getItem("rolelist"),
            onshown: function () {
                $("#popupContent").load('pages/role/list.html')
            },
            onhidden: function () {
                executeMultiCommad(element, bpmnFactory, commandStack);
            },
            draggable: true
        });

    }
}

var mxParticipantSelected = null;
var mxParticipantType = null;

function afterParticipantSelected(evt, data) {
    mxParticipantSelected = data.ParticipantItem;
    mxParticipantType = data.ParticipantType;
}

function isParticipantExisted(element) {
    var isExisted = false;
    const performers = getPerformers(element) || [];
    const participant = mxParticipantSelected;
    if (participant !== null) {
        performers.forEach(function (item, index) {
            if (item.outerId === participant.ID) {
                isExisted = true;
            }
        })
    }
    return isExisted;
}

function executeMultiCommad(element, bpmnFactory, commandStack) {
    //check the participant whether exist
    if (isParticipantExisted(element) === true) {
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
                properties: { extensionElements }
            }
        });
    }

    let extension = getPerformersExtension(element);

    if (!extension) {
        extension = createPerformers({
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

    const newPerformer = createElement('sf:Performer', {
        name: mxParticipantSelected.RoleName,
        outerId: mxParticipantSelected.ID,
        outerCode: mxParticipantSelected.RoleCode,
        outerType: mxParticipantType    //role
    }, extension, bpmnFactory);

    commands.push({
        cmd: 'element.updateModdleProperties',
        context: {
            element,
            moddleElement: extension,
            properties: {
                values: [...extension.get('values'), newPerformer]
            }
        }
    });

    commandStack.execute('properties-panel.multi-command-executor', commands);
}

function removeFactory({ commandStack, element, performer }) {
    return function (event) {
        event.stopPropagation();
        const extension = getPerformersExtension(element);
        if (!extension) {
            return;
        }

        const performers = without(extension.get('values'), performer);
        commandStack.execute('element.updateModdleProperties', {
            element,
            moddleElement: extension,
            properties: {
                values: performers
            }
        });
    };
}





