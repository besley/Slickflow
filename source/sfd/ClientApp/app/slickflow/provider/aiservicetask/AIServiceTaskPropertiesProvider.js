
import configUUIDProps from './parts/ConfigUUIDProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';

const LOW_PRIORITY = 500;

export default function AiServiceTaskPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            // skip AI detail for LLM / PlugIn service tasks
            if (is(element, 'bpmn:ServiceTask') && isAiServiceType(element)) {
                groups.push(createAIServiceTaskGroup(element, translate));
            }
            return groups;
        }
    };

    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

AiServiceTaskPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

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

function isAiServiceType(element) {
    const sfType = element?.sfType || element?.businessObject?.sfType;
    return sfType === 'LLM' || sfType === 'RAG';
}
