import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';

export default function (element) {
    var codeProperty =  
        {
            id: 'code',
            element,
            component: Code,
            isEdited: isTextFieldEntryEdited
        };
    return codeProperty;
}

function Code(props) {
    const { element, id } = props;
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        return element.businessObject.code || '';
    }

    const setValue = value => {
        return modeling.updateProperties(element, {
            code: value
        });
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('code')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

