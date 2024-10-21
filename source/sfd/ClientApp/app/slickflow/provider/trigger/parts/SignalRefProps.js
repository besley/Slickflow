import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';
import jshelper from "../../../../script/jshelper";

import { forEach } from "min-dash";

export default function (element) {
    var signalRefProperty =
    {
        id: 'signalRef',
        element,
        component: SignalRef,
        isEdited: isTextFieldEntryEdited
    };
    return signalRefProperty;
}

function SignalRef(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');
    const bpmnFactory = useService('bpmnFactory');

    const getValue = () => {
        const businessObject = getBusinessObject(element);
        var siganlEventDefinition = businessObject.eventDefinitions[0];
        var signalRef = siganlEventDefinition.signalRef;
        // search for message with the same name:
        var definitions = businessObject.$parent.$parent;

        if (signalRef) {
            var signal = FindSignalById(definitions, signalRef.id);
            if (signal) return signal.name;
        }
        return '';
    }

    const setValue = value => {
        const businessObject = getBusinessObject(element);
        var definitions = businessObject.$parent.$parent;
        var rootElements = definitions.rootElements;

        if (businessObject.eventDefinitions) {
            var siganlEventDefinition = businessObject.eventDefinitions[0];
            var signalRef = siganlEventDefinition.signalRef;

            var signal;
            if (signalRef) {
                signal = FindSignalById(definitions, signalRef.id);
            }

            //set message value
            if (signal) {
                signal.name = value;
            } else {
                signal = bpmnFactory.create("bpmn:Signal");
                signal.id = "Siganl_" + jshelper.getRandomString(6);
                signal.name = value;
                rootElements.push(signal);
            }
            siganlEventDefinition.signalRef = signal;
        }
    }

    function FindSignalById(definitions, signalRefId) {
        var signal;
        var rootElements = definitions.rootElements;
        var signals = rootElements.filter(function (element) {
            return element.$type === 'bpmn:Signal';
        });

        for (var i in signals) {
            if (signals[i].id === signalRefId) {
                signal = signals[i];
                break;
            }
        }
        return signal;
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
        label={kresource.getItem('signalref')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

