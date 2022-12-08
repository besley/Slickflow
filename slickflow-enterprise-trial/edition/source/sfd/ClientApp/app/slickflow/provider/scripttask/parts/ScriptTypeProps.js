import { SelectEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var scriptTypeProperty =
    {
        id: 'scripttype',
        element,
        component: ScriptType,
        isEdited: isTextFieldEntryEdited
    };
    return scriptTypeProperty;
}

function ScriptType(props) {
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
                    var scriptType = scriptElement.scriptType;
                    var selectedValue = getOptionValue(scriptType);
                    return selectedValue;
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
        if (scriptElement) scriptElement.scriptType = getOptionName(value);

        //update condition properties
        return modeling.updateProperties(element, {
            extensionElements
        });
    }

    function getOptions() {
        var options = [
            { "label": "SQL", "value": "1", "name": "SQL" },
            { "label": "Javascript", "value": "2", "name": "Javascript" },
            { "label": "Python", "value": "3", "name": "Python" }
        ];
        return options;
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

    function getOptionName(value) {
        var name = '';
        var options = getOptions();
        forEach(options, item => {
            if (item.value === value) {
                name = item.name;
            }
        })
        return name;
    }

    function getOptionValue(name) {
        var value = '';
        var options = getOptions();
        forEach(options, item => {
            if (item.name === name) {
                value = item.value;
            }
        })
        return value;
    }

    return <SelectEntry
        id={id}
        element={element}
        label={kresource.getItem('scripttype')}
        getValue={getValue}
        setValue={setValue}
        getOptions={getOptions}
        debounce={debounce}
    />
}
