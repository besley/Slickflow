import timerExpressionProps from './parts/TimerExpressionProps';
import conditionExpressionProps from './parts/ConditionExpressionProps';
import messageRefProps from './parts/MessageRefProps';
import signalRefProps from './parts/SignalRefProps';

import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';
import { is } from 'bpmn-js/lib/util/ModelUtil';
import sfModelUtility from '../SfModelUtility'

const LOW_PRIORITY = 500;

export default function TriggerPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:StartEvent')
                || is(element, 'bpmn:IntermediateCatchEvent')
                || is(element, 'bpmn:IntermediateThrowEvent')
                || is(element, 'bpmn:EndEvent')){
                const group = createExpressionGroup(element, translate);
                if (group) groups.push(group);
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

TriggerPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createExpressionGroup(element, translate) {  
    const entries = [];
    const businessObject = getBusinessObject(element);
    if (businessObject.eventDefinitions) {
        const eventDefinition = businessObject.eventDefinitions[0]
        if (eventDefinition) {
            if (eventDefinition.$type === "bpmn:ConditionalEventDefinition") {
                entries.push(conditionExpressionProps(element));
            } else if (eventDefinition.$type === "bpmn:TimerEventDefinition") {
                entries.push(timerExpressionProps(element));
            } else if (eventDefinition.$type === "bpmn:MessageEventDefinition") {
                entries.push(messageRefProps(element));
            } else if (eventDefinition.$type === "bpmn:SignalEventDefinition") {
                entries.push(signalRefProps(element));
            }
        }

        const triggerGroup = {
            id: 'trigger',
            label: kresource.getItem('trigger'),
            entries: entries
        };
        return triggerGroup;
    }
}
