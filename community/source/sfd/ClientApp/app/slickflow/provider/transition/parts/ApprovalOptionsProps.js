import { SelectEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var approvalOptionsProperty =
    {
        id: 'approvalOptions',
        element,
        component: ApprovalOptions,
        isEdited: isTextFieldEntryEdited
    };
    return approvalOptionsProperty;
}

function ApprovalOptions(props) {
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
                var approval = gbsElement.approval;
                var selectedValue = getOptionValue(approval);
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

        if (gbsElement) gbsElement.approval = getOptionName(value);
        //update condition properties
        return modeling.updateProperties(element, {
            extensionElements
        });
    }

    function getOptions() {
        var options = [
            { "label": kresource.getItem("approvalofagreed"), "value": "1", "name": "Agreed" },
            { "label": kresource.getItem("approvalofrefused"), "value": "2", "name": "Refused" }
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
        label={kresource.getItem('approvaloptions')}
        getValue={getValue}
        setValue={setValue}
        getOptions={getOptions}
        debounce={debounce}
    />
}
