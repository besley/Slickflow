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
            var servicesElement = sfModelUtility.getExtensionElement(businessObject, 'sf:Services');
            if (servicesElement
                && servicesElement.services) {
                var serviceElement = servicesElement.services[0];
                if (serviceElement) {
                    var argus = serviceElement.argus;
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
        var servicesElement = sfModelUtility.getExtensionElement(businessObject, 'sf:Services');

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
        if (serviceElement) serviceElement.argus = value;

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