import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export default function (element) {
    var expressionProperty = 
        {
            id: 'expression',
            element,
            component: Expression,
            isEdited: isTextFieldEntryEdited
    };
    return expressionProperty;
}

function Expression(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const bpmnFactory = useService('bpmnFactory');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        const businessObject = getBusinessObject(element);
        if (businessObject.eventDefinitions) {
            var eventDefinition = businessObject.eventDefinitions[0];
            var formalExpression = eventDefinition.get("condition");
            if (formalExpression) {
                return formalExpression.body;
            } else {
                return '';
            }
        }
        return '';

    }

    const setValue = value => {
        const businessObject = getBusinessObject(element);
        if (businessObject.eventDefinitions) {
            var eventDefinition = businessObject.eventDefinitions[0];

            const formalExpression = bpmnFactory.create("bpmn:FormalExpression", {
                body: value
            });
            eventDefinition.set("condition", formalExpression);
        }
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

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('conditionexpression')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

