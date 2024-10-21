import { CheckboxEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var defaultBranchProperty =
    {
        id: 'defaultBranch',
        element,
        component: DefaultBranch,
        isEdited: isTextFieldEntryEdited
    };
    return defaultBranchProperty;
}

function DefaultBranch(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var gbsElement = getExtensionElement(businessObject, 'sf:GroupBehaviours');

            if (gbsElement) {
                var defaultBranch = gbsElement.defaultBranch;
                return defaultBranch;
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
        if (gbsElement) gbsElement.defaultBranch = String(value);
       
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

    return <CheckboxEntry
        id={id}
        element={element}
        description={kresource.getItem('defaultbranchdesc')}
        label={kresource.getItem('defaultbranch')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

