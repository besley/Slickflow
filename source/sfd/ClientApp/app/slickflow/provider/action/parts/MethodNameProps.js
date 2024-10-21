import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import sfModelUtility from '../../SfModelUtility';

export default function (element) {
    var methodNameProperty = {
        id: 'methodname',
        element,
        component: MethodName,
        isEdited: isTextFieldEntryEdited
    };
    return methodNameProperty;
}

function MethodName(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var actionsElement = sfModelUtility.getExtensionElement(businessObject, 'sf:Actions');
            if (actionsElement
                && actionsElement.actions) {
                var actionElement = actionsElement.actions[0];
                if (actionElement) {
                    var methodName = actionElement.methodName;
                    return methodName;
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
        var actionsElement = sfModelUtility.getExtensionElement(businessObject, 'sf:Actions');

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
        if (actionElement) actionElement.methodName = value;

        return modeling.updateProperties(element, {
            extensionElements
        });
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('methodname')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}