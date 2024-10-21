import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var completeOrderProperty =
    {
        id: 'completeOrder',
        element,
        component: CompleteOrder,
        isEdited: isTextFieldEntryEdited
    };
    return completeOrderProperty;
}

function CompleteOrder(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var gbsElement = getExtensionElement(businessObject, 'sf:MultiSignDetail');

            if (gbsElement) {
                var completeOrder = gbsElement.completeOrder;
                return completeOrder;
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
        var gbsElement = getExtensionElement(businessObject, 'sf:MultiSignDetail');
        if (!gbsElement) {
            gbsElement = moddle.create('sf:MultiSignDetail');
            extensionElements.get('values').push(gbsElement);
        }

        if (gbsElement) gbsElement.completeOrder = value;
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

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('passovervalue')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

