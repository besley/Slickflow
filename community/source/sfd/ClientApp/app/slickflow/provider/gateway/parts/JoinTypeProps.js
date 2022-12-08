import { SelectEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var joinTypeProperty =
    {
        id: 'joinType',
        element,
        component: JoinType,
        isEdited: isTextFieldEntryEdited
    };
    return joinTypeProperty;
}

function JoinType(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var atdElement = getExtensionElement(businessObject, 'sf:GatewayDetail');

            if (atdElement) {
                var extraJoinType = atdElement.extraJoinType;
                var slectedValue = getOptionValue(extraJoinType);
                return slectedValue;
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
        var atdElement = getExtensionElement(businessObject, 'sf:GatewayDetail');
        if (!atdElement) {
            atdElement = moddle.create('sf:GatewayDetail');
            extensionElements.get('values').push(atdElement);
        }
       
        if (atdElement) atdElement.extraJoinType = getOptionName(value);
       
        //update condition properties
        return modeling.updateProperties(element, {
            extensionElements
        });
    }

    function getOptions() {
        //扩展合并类型
        var options = [
            { "label": kresource.getItem("eorjoin"), "value": "1", "name": "EOrJoin" },
            { "label": kresource.getItem("andjoinmi"), "value": "2", "name": "AndJoinMI" }
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
        label={kresource.getItem('extrajointype')}
        getValue={getValue}
        setValue={setValue}
        getOptions={getOptions}
        debounce={debounce}
    />
}
