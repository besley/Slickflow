import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import sfModelUtility from '../../SfModelUtility';

export default function (element) {
    var argusProperty = {
        id: 'argus',
        element,
        component: Argus,
        isEdited: isTextFieldEntryEdited
    };
    return argusProperty;
}

function Argus(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var scriptsElement = sfModelUtility.getExtensionElement(businessObject, 'sf:Scripts');
            if (scriptsElement
                && scriptsElement.scripts) {
                var scriptElement = scriptsElement.scripts[0];
                if (scriptElement) {
                    var argus = scriptElement.argus;
                    return argus;
                } else {
                    return '';
                }
            } else {
                return '';
            }
        }
        else
            return '';
    }

    const setValue = value => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        var scriptsElement = sfModelUtility.getExtensionElement(businessObject, 'sf:Scripts');

        var scriptElement = null;
        if (!scriptsElement) {
            scriptsElement = moddle.create('sf:Scripts');
            extensionElements.get('values').push(scriptsElement);
            scriptElement = moddle.create('sf:Service');
            scriptsElement.get('scripts').push(scriptElement);
        } else {
            scriptElement = scriptsElement.get('scripts')[0];
            if (!scriptElement) {
                scriptElement = moddle.create('sf:Service');
                scriptsElement.get('scripts').push(scriptElement);
            }
        }
        if (scriptElement) scriptElement.argus = value;
        //update condition properties
        return modeling.updateProperties(element, {
            extensionElements
        });
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('argumentslist')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}