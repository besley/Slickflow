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
            var actionsElement = getExtensionElement(businessObject, 'sf:Actions');

            if (actionsElement
                && actionsElement.actions) {
                var actionElement = actionsElement.actions[0];
                if (actionElement) {
                    var methodType = actionElement.methodType;
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
        var actionsElement = getExtensionElement(businessObject, 'sf:Actions');

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

        if (actionElement) {
            var methodTypeName = getOptionName(value);
            actionElement.methodType = methodTypeName
            onChange(methodTypeName);
        }
        //update condition properties
        return modeling.updateProperties(element, {
            extensionElements
        });
    }

    function getOptions() {
        var options = [
            { "label": kresource.getItem("optiondefault"), "value": "0", "name": "Optiondefault" },
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
