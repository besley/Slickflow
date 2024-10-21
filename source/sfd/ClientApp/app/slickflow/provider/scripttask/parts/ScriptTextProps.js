import { TextAreaEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var scriptTextProperty =
    {
        id: 'scripttext',
        element,
        component: ScriptText,
        isEdited: isTextFieldEntryEdited
    };
    return scriptTextProperty;
}

function ScriptText(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var scriptsElement = getExtensionElement(businessObject, 'sf:Scripts');
            if (scriptsElement
                && scriptsElement.scripts) {
                var scriptElement = scriptsElement.scripts[0];
                if (scriptElement) {
                    var scriptText = scriptElement.scriptText;
                    return scriptText;
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
        var scriptsElement = getExtensionElement(businessObject, 'sf:Scripts');

        var scriptElement = null;
        if (!scriptsElement) {
            scriptsElement = moddle.create('sf:Scripts');
            extensionElements.get('values').push(scriptsElement);
            scriptElement = moddle.create('sf:Script');
            scriptsElement.get('scripts').push(scriptElement);
        } else {
            scriptElement = scriptsElement.get('scripts')[0];
            if (!scriptElement) {
                scriptElement = moddle.create('sf:Script');
                scriptsElement.get('scripts').push(scriptElement);
            }
        }

        if (scriptElement) scriptElement.scriptText = value;
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
        label={kresource.getItem('scripttext')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

