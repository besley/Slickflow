import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export default function (element) {
    var expressionProperty = 
        {
            id: 'condition',
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
        var condition = businessObject.conditionExpression;

        if (condition)
            return condition.body;
        else
            return '';
    }

    const setValue = value => {
        const businessObject = getBusinessObject(element);
        const conditionObject = businessObject.eventDefinitions[0];
        var expression = conditionObject.condition;
        expression.text = value;


        return modeling.updateProperties(conditionObject, {
            conditionExpression: expression
        });
    }

    function getExtensionElement(element, type) {
        if (!element.extensionElements) {
            return;
        }

        return element.extensionElements.values.filter((extensionElement) => {
            return extensionElement.$instanceOf(type);
        })[0];
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('expression')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

