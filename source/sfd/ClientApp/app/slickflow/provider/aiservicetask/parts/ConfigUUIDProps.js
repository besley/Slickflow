import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import sfModelUtility from '../../SfModelUtility';

export default function (element) {
    var configUUIDProperty = {
        id: 'configUUID',
        element,
        component: ConfigUUID,
        isEdited: isTextFieldEntryEdited
    };
    return configUUIDProperty;
}

function ConfigUUID(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var aiServicesElement = sfModelUtility.getExtensionElement(businessObject, 'sf:AIServices');
            if (aiServicesElement && aiServicesElement.aiServices) {
                var aiServiceElement = aiServicesElement.aiServices[0];
                if (aiServiceElement) {
                    var configUUID = aiServiceElement.configUUID;
                    return configUUID;
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
        var aiServicesElement = sfModelUtility.getExtensionElement(businessObject, 'sf:AIServices');

        var aiServiceElement = null;
        if (!aiServicesElement) {
            aiServicesElement = moddle.create('sf:AIServices');
            extensionElements.get('values').push(aiServicesElement);
            aiServiceElement = moddle.create('sf:AIService');
            aiServicesElement.get('aiServices').push(aiServiceElement);
        } else {
            aiServiceElement = aiServicesElement.get('aiServices')[0];
            if (!aiServiceElement) {
                aiServiceElement = moddle.create('sf:AIService');
                aiServicesElement.get('aiServices').push(aiServiceElement);
            }
        }
        if (aiServiceElement) aiServiceElement.configUUID = value;

        return modeling.updateProperties(element, {
            extensionElements
        });
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('configUUID')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
        disabled
    />
}