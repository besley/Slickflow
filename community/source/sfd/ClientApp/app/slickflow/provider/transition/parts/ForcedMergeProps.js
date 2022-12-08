import { CheckboxEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var forcedMergeProperty =
    {
        id: 'forcedMerge',
        element,
        component: ForcedMerge,
        isEdited: isTextFieldEntryEdited
    };
    return forcedMergeProperty;
}

function ForcedMerge(props) {
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
                var forcedMerge = gbsElement.forcedMerge;
                return forcedMerge;
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
        if (gbsElement) gbsElement.forcedMerge = String(value);
       
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
        description={translate('when the orjoin type is EOrJoin, this options can be configured.')}
        label={kresource.getItem('mergeoptions')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

