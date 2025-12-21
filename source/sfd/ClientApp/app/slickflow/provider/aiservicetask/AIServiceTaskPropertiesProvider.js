
import configUUIDProps from './parts/ConfigUUIDProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';

const LOW_PRIORITY = 500;

export default function AIServiceTaskPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            // skip AI detail for LLM / PlugIn service tasks
            if (is(element, 'bpmn:ServiceTask') && isLLMOrPlugIn(element)) {
                groups.push(createAIServiceTaskGroup(element, translate));
            }
            return groups;
        }
    };

    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

AIServiceTaskPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createAIServiceTaskGroup(element, translate) {
    const aiTaskGroup = {
        id: 'aiServiceTask',
        label: kresource.getItem('aidetail'),
        entries: [
            configUUIDProps(element)
        ]
    };
    return aiTaskGroup
}

function isLLMOrPlugIn(element) {
    const sfType = element?.sfType || element?.businessObject?.sfType;
    return sfType === 'LLM' || sfType === 'PlugIn';
}
