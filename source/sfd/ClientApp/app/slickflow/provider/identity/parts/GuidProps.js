import { TextFieldEntry, isTextFieldEntryEdited  } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { v4 as uuidv4 } from 'uuid';

export default function (element) {
    var property = 
        {
            id: 'guid',
            element,
            component: Guid,
            isEdited: false
    };
    return property;
}

function Guid(props) {
    const { element, id } = props;
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        return element.businessObject.guid || '';
    }

    const setValue = value => {
        return modeling.updateProperties(element, {
            guid: value
        });
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('guid')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}