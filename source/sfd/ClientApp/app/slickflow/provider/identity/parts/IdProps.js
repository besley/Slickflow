import { TextFieldEntry, isTextFieldEntryEdited  } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';

export default function (element) {
    var property = 
        {
            id: 'id',
            element,
            component: IDComp,
            isEdited: false
    };
    return property;
}

function IDComp(props) {
    const { element, id } = props;
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        return element.businessObject.id || '';
    }

    const setValue = value => {
        return modeling.updateProperties(element, {
            id: value
        });
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('id')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
        disabled={true}
    />
}