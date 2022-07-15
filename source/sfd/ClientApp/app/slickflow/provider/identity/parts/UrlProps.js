import { TextFieldEntry, isTextFieldEntryEdited  } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';

export default function (element) {
    var property = 
        {
            id: 'url',
            element,
            component: Url,
            isEdited: false
    };
    return property;
}

function Url(props) {
    const { element, id } = props;
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        return element.businessObject.url || '';
    }

    const setValue = value => {
        return modeling.updateProperties(element, {
            url: value
        });
    }

    return <TextFieldEntry
        id={id}
        element={element}
        label={kresource.getItem('url')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}