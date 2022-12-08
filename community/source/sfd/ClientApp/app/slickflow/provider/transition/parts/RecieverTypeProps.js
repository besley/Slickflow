import { SelectEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var recieverTypeProperty =
    {
        id: 'recievertype',
        element,
        component: RecieverType,
        isEdited: isTextFieldEntryEdited
    };
    return recieverTypeProperty;
}

function RecieverType(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var gbsElement = getExtensionElement(businessObject, 'sf:GroupBehaviours');
            if (gbsElement) {
                var recieverType = gbsElement.recieverType;
                var selectedValue = getOptionValue(recieverType);
                return selectedValue;
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
        var gbsElement = getExtensionElement(businessObject, 'sf:GroupBehaviours');
        if (!gbsElement) {
            gbsElement = moddle.create('sf:GroupBehaviours');
            extensionElements.get('values').push(gbsElement);
        }

        if (gbsElement) gbsElement.recieverType = getOptionName(value);
        //update condition properties
        return modeling.updateProperties(element, {
            extensionElements
        });
    }

    function getOptions() {
        var options = [
            { "label": kresource.getItem("superior"), "value": "1", "name": "Superior" },
            { "label": kresource.getItem("compeer"), "value": "2", "name": "Compeer" },
            { "label": kresource.getItem("subordinate"), "value": "3", "name": "Subordinate" }
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
        label={kresource.getItem('recievertype')}
        getValue={getValue}
        setValue={setValue}
        getOptions={getOptions}
        debounce={debounce}
    />
}
