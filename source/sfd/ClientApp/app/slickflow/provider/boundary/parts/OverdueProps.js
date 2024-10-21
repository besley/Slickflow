import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

import { forEach } from "min-dash";

export default function (element) {
    var timerProperty =
    {
        id: 'overdue',
        element,
        component: Overdue,
        isEdited: isTextFieldEntryEdited
    };
    return timerProperty;
}

function Overdue(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        var businessObject = getBusinessObject(element);
        const extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (extensionElements) {
            var boundiresElement = getExtensionElement(businessObject, 'sf:Boundaries');
            if (boundiresElement) {
                var boundaryElement = SearchPropertyElement(boundiresElement);
                if (boundaryElement) {
                    var expression = boundaryElement.expression;
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
        var boundiresElement = getExtensionElement(businessObject, 'sf:Boundaries');

        var boundaryElement = null;
        if (!boundiresElement) {
            boundiresElement = moddle.create('sf:Boundaries');
            extensionElements.get('values').push(boundiresElement);
            boundaryElement = moddle.create('sf:Boundary');
            boundaryElement.event = "Timer";
            boundiresElement.get('boundaries').push(boundaryElement);
        } else {
            boundaryElement = boundiresElement.get('boundaries')[0];
            if (!boundaryElement) {
                boundaryElement = moddle.create('sf:Boundary');
                boundiresElement.get('boundaries').push(boundaryElement);
            }
        }
        if (boundaryElement) boundaryElement.expression = value;
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
        var list = element.boundaries;
        forEach(list, item => {
            var event = item.event;
            if (event === 'Timer') {
                node = item;
            }
        })
        return node;
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('overdueexpression')}
        description={kresource.getItem('exmapleperiod')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

