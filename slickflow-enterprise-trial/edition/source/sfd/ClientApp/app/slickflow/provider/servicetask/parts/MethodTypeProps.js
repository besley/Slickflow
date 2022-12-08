import { SelectEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element, onChange) {
    var methodTypeProperty =
    {
        id: 'methodtype',
        element,
        onChange: onChange,
        component: MethodType,
        isEdited: isTextFieldEntryEdited
    };
    return methodTypeProperty;
}

function MethodType(props) {
    const { element, id, onChange } = props;
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
                    var methodType = serviceElement.methodType;
                    var selectedValue = getOptionValue(methodType);
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

        if (serviceElement) {
            var methodTypeName = getOptionName(value);
            serviceElement.methodType = methodTypeName;
            onChange(methodTypeName);
        }
        //update condition properties
        return modeling.updateProperties(element, {
            extensionElements
        });
    }

    function getOptions() {
        var options = [
            { "label": "WebApi", "value": "1", "name": "WebApi" },
            { "label": "LocalService", "value": "2", "name": "LocalService" },
            { "label": "CSharpLibrary", "value": "3", "name": "CSharpLibrary" },
            { "label": "SQLStoreProcedure", "value": "4", "name": "SQLStoreProcedure" }
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
        label={kresource.getItem('calltype')}
        getValue={getValue}
        setValue={setValue}
        getOptions={getOptions}
        debounce={debounce}
    />
}
