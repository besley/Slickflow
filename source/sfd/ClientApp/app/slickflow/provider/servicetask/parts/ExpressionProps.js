import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var expressionProperty =
    {
        id: 'expression',
        element,
        component: Expression,
        isEdited: isTextFieldEntryEdited
    };
    return expressionProperty;
}

function Expression(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var servicesElement = getExtensionElement(businessObject, 'sf:Services');
            if (servicesElement
                && servicesElement.services) {
                var serviceElement = servicesElement.services[0];
                if (serviceElement) {
                    var expression = serviceElement.expression;
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
        var servicesElement = getExtensionElement(businessObject, 'sf:Services');

        var serviceElement = null;
        if (!servicesElement) {
            servicesElement = moddle.create('sf:Services');
            extensionElements.get('values').push(servicesElement);
            serviceElement = moddle.create('sf:Service');
            servicesElement.get('services').push(serviceElement);
        } else {
            serviceElement = servicesElement.get('services')[0];
            if (!serviceElement) {
                serviceElement = moddle.create('sf:Service');
                servicesElement.get('services').push(serviceElement);
            }
        }

        if (serviceElement) serviceElement.expression = value;
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
        label={kresource.getItem('expression')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

