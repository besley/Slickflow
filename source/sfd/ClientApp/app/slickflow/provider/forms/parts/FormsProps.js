import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import {
    createElement,
    createForms,
    getForms,
    getFormsExtension
} from '../util';

import { forEach, without } from 'min-dash';

export default function FormsProps({element, injector, event}) {
    const forms = getForms(element) || [];
    const bpmnFactory = injector.get('bpmnFactory'),
        commandStack = injector.get('commandStack');

    const items = forms.map((form, index) => {
        const id = element.id + '-form-' + index;
        return {
            id,
            label: form.get('name') || '',
            autoFocusEntry: id + '-name',
            remove: removeFactory({ commandStack, element, form })
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

        if ("undefined" !== typeof formlist) {
            formlist.afterFormSelected.subscribe(afterFormSelected);
        }

        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupContent"></div>'),
            title: kresource.getItem("formlist"),
            onshown: function () {
                $("#popupContent").load('pages/form/list.html')
            },
            onhidden: function () {
                executeMultiCommad(element, bpmnFactory, commandStack);
            },
            draggable: true
        });

    }
}

var mxFormSelected = null;

function afterFormSelected(evt, data) {
    mxFormSelected = data.FormItem;
}

function isFormSelectedExisted(element) {
    var isExisted = false;
    const forms = getForms(element) || [];
    const formSelected = mxFormSelected;
    if (formSelected !== null) {
        forms.forEach(function (item, index) {
            if (item.outerId === formSelected.FormId) {
                isExisted = true;
            }
        })
    }
    return isExisted;
}
function executeMultiCommad(element, bpmnFactory, commandStack) {
    //check the formSelected whether exist
    if (isFormSelectedExisted(element) === true) {
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

    let extension = getFormsExtension(element);

    if (!extension) {
        extension = createForms({
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

    if (mxFormSelected !== null) {
        const newForm = createElement('sf:Form', {
            name: mxFormSelected.FormName,
            outerId: mxFormSelected.FormId,
            outerCode: mxFormSelected.FormCode
        }, extension, bpmnFactory);

        commands.push({
            cmd: 'element.updateModdleProperties',
            context: {
                element,
                moddleElement: extension,
                properties: {
                    values: [...extension.get('values'), newForm]
                }
            }
        });
        commandStack.execute('properties-panel.multi-command-executor', commands);
    } 
}

function removeFactory({ commandStack, element, form }) {
    return function (event) {
        event.stopPropagation();
        const extension = getFormsExtension(element);
        if (!extension) {
            return;
        }

        const forms = without(extension.get('values'), form);
        commandStack.execute('element.updateModdleProperties', {
            element,
            moddleElement: extension,
            properties: {
                values: forms
            }
        });
    };
}