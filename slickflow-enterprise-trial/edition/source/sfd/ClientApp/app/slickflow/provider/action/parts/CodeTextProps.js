import { TextAreaEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var codeTextProperty =
    {
        id: 'codetext',
        element,
        component: CodeText,
        isEdited: isTextFieldEntryEdited
    };
    return codeTextProperty;
}

function CodeText(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var actionsElement = getExtensionElement(businessObject, 'sf:Actions');
            if (actionsElement
                && actionsElement.actions) {
                var actionElement = actionsElement.actions[0];
                if (actionElement) {
                    var expression = actionElement.expression;
                    return expression;
                } else {
                    return '';
                }
            }
            else
                return '';
        }
        else
            return '';
    }

    const setValue = value => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        var actionsElement = getExtensionElement(businessObject, 'sf:Actions');

        var actionElement = null;
        if (!actionsElement) {
            actionsElement = moddle.create('sf:Actions');
            extensionElements.get('values').push(actionsElement);
            actionElement = moddle.create('sf:Action');
            actionsElement.get('actions').push(actionElement);
        } else {
            actionElement = actionsElement.get('actions')[0];
            if (!actionElement) {
                actionElement = moddle.create('sf:Action');
                actionsElement.get('actions').push(actionElement);
            }
        }

        if (actionElement) actionElement.expression = value;
        //update condition properties
        return modeling.updateProperties(element, {
            extensionElements
        });
    }


    function getExtensionElement(element, type) {
        if (!element.extensionElements
            || !element.extensionElements.values) {
            return;
        }

        return element.extensionElements.values.filter((extensionElement) => {
            return extensionElement.$instanceOf(type);
        })[0];
    }

    return <TextAreaEntry
        id={id}
        element={element}
        label={kresource.getItem('codetext')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

