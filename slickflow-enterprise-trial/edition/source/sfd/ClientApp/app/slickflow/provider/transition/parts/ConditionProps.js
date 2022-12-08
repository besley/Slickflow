import { TextAreaEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export default function (element) {
    var conditionProperty = 
        {
            id: 'condition',
            element,
            component: Condition,
            isEdited: isTextFieldEntryEdited
    };
    return conditionProperty;
}

function Condition(props) {
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
        const expression = bpmnFactory.create("bpmn:Expression");
        expression.body = value;

        return modeling.updateProperties(element, {
            conditionExpression: expression
        });
    }

    return <TextAreaEntry
        id={id}
        element={element}
        label={kresource.getItem('conditionexpression')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

