import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';
import sfModelUtility from '../../SfModelUtility';

export default function (element) {
    return {
        id: 'toolSetName',
        element,
        component: ToolSetName,
        isEdited: isTextFieldEntryEdited
    };
}

function ToolSetName(props) {
    const { element, id } = props;
    const moddle   = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getAiServiceElement = () => {
        const bo = getBusinessObject(element);
        const aiServicesEl = sfModelUtility.getExtensionElement(bo, 'sf:AiServices');
        return aiServicesEl?.aiServices?.[0] ?? null;
    };

    const getValue = () => getAiServiceElement()?.toolSetName ?? '';

    const setValue = value => {
        const bo = getBusinessObject(element);
        const extensionElements = bo.extensionElements || moddle.create('bpmn:ExtensionElements');
        let aiServicesEl = sfModelUtility.getExtensionElement(bo, 'sf:AiServices');
        let aiServiceEl;

        if (!aiServicesEl) {
            aiServicesEl = moddle.create('sf:AiServices');
            extensionElements.get('values').push(aiServicesEl);
            aiServiceEl = moddle.create('sf:AiService');
            aiServicesEl.get('aiServices').push(aiServiceEl);
        } else {
            aiServiceEl = aiServicesEl.get('aiServices')[0];
            if (!aiServiceEl) {
                aiServiceEl = moddle.create('sf:AiService');
                aiServicesEl.get('aiServices').push(aiServiceEl);
            }
        }

        aiServiceEl.toolSetName = value || undefined;

        return modeling.updateProperties(element, { extensionElements });
    };

    return <TextFieldEntry
        id={id}
        element={element}
        label={translate('toolSetName')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />;
}
