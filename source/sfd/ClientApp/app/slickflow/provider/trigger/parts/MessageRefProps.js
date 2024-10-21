import { TextFieldEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';
import jshelper from "../../../../script/jshelper";

import { forEach } from "min-dash";

export default function (element) {
    var messageRefProperty =
    {
        id: 'messageRef',
        element,
        component: MessageRef,
        isEdited: isTextFieldEntryEdited
    };
    return messageRefProperty;
}

function MessageRef(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const translate = useService('translate');
    const debounce = useService('debounceInput');
    const bpmnFactory = useService('bpmnFactory');

    const getValue = () => {
        const businessObject = getBusinessObject(element);
        var messageEventDefinition = businessObject.eventDefinitions[0];
        var messageRef = messageEventDefinition.messageRef;
        // search for message with the same name:
        var definitions = businessObject.$parent.$parent;

        if (messageRef) {
            var msg = FindMessageById(definitions, messageRef.id);
            if (msg) return msg.name;
        }
        return '';
    }

    const setValue = value => {
        const businessObject = getBusinessObject(element);
        var definitions = businessObject.$parent.$parent;
        var rootElements = definitions.rootElements;

        if (businessObject.eventDefinitions) {
            var messageEventDefinition = businessObject.eventDefinitions[0];
            var messageRef = messageEventDefinition.messageRef;

            var msg;
            if (messageRef) {
                msg = FindMessageById(definitions, messageRef.id);
            }

            //set message value
            if (msg) {
                msg.name = value;
            } else {
                msg = bpmnFactory.create("bpmn:Message");
                msg.id = "Message_" + jshelper.getRandomString(6);
                msg.name = value;
                rootElements.push(msg);
            }
            messageEventDefinition.messageRef = msg;
        }
    }

    function FindMessageById(definitions, messageRefId) {
        var msg;
        var rootElements = definitions.rootElements;
        var messages = rootElements.filter(function (element) {
            return element.$type === 'bpmn:Message';
        });

        for (var i in messages) {
            if (messages[i].id === messageRefId) {
                msg = messages[i];
                break;
            }
        }
        return msg;
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
        label={kresource.getItem('messageref')}
        getValue={getValue}
        setValue={setValue}
        debounce={debounce}
    />
}

