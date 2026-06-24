
import configUUIDProps  from './parts/ConfigUUIDProps';
import toolSetNameProps from './parts/ToolSetNameProps';

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
    const entries = [configUUIDProps(element)];

    // toolSetName is only relevant for Agent nodes (not LLM / RAG)
    if (getAiServiceType(element) === 'Agent') {
        entries.push(toolSetNameProps(element));
    }

    return {
        id: 'aiServiceTask',
        label: kresource.getItem('aidetail'),
        entries
    };
}

function getAiServiceType(element) {
    const sfType = element?.sfType || element?.businessObject?.sfType;
    if (sfType) return sfType;
    const values = element?.businessObject?.extensionElements?.values;
    if (values) {
        const aiServices = values.find(v =>
            v.$type === 'sf:AiServices' && Array.isArray(v.aiServices) && v.aiServices.length > 0);
        if (aiServices) return aiServices.aiServices[0].type;
    }
    return null;
}

function isAiServiceType(element) {
    // 1. Check sfType set directly on element (palette drag-and-drop path)
    const sfType = element?.sfType || element?.businessObject?.sfType;
    if (sfType === 'LLM' || sfType === 'RAG' || sfType === 'Agent') return true;

    // 2. Fallback: read type from sf:AiServices extension elements (XML load path)
    const values = element?.businessObject?.extensionElements?.values;
    if (values) {
        const aiServices = values.find(v =>
            v.$type === 'sf:AiServices' && Array.isArray(v.aiServices) && v.aiServices.length > 0
        );
        if (aiServices) {
            const t = aiServices.aiServices[0].type;
            return t === 'LLM' || t === 'RAG' || t === 'Agent';
        }
    }
    return false;
}
