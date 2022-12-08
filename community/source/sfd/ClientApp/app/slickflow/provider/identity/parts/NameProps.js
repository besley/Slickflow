import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';

export default function (element) {
    var nameProperty =  
        {
            id: 'name',
            element,
            component: Name,
            isEdited: isTextFieldEntryEdited
        };
    return nameProperty;
}

function Name(props) {
    const { element, id } = props;
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        return element.businessObject.name || '';
    }

    const setValue = value => {
        return modeling.updateProperties(element, {
            name: value
        });
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('name')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

