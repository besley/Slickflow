import { TextAreaEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var customProperty =
    {
        id: 'custom',
        element,
        component: Custom,
        isEdited: isTextFieldEntryEdited
    };
    return customProperty;
}

function Custom(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var sectionsElement = getExtensionElement(businessObject, 'sf:Sections');
            if (sectionsElement) {
                var sectionElement = SearchPropertyElement(sectionsElement);

                if (sectionElement) {
                    var textContent = sectionElement.text;
                    return textContent;
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
        var sectionsElement = getExtensionElement(businessObject, 'sf:Sections');

        var sectionElement = null;
        if (!sectionsElement) {
            sectionsElement = moddle.create('sf:Sections');
            extensionElements.get('values').push(sectionsElement);
            sectionElement = moddle.create('sf:Section');
            sectionElement.name = "myProperties";
            sectionsElement.get('sections').push(sectionElement);
        }
        if (sectionElement) sectionElement.text = value;
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

    function SearchPropertyElement(element) {
        var node = null;
        var list = element.sections;
        forEach(list, item => {
            var name = item.name;
            if (name === 'myProperties') {
                node = item;
            }
        })
        return node;
    }

    return <TextAreaEntry
        id={id}
        element={element}
        label={kresource.getItem('properties')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

