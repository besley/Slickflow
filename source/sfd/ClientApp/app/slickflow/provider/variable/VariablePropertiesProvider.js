import variablesProps from './parts/VariablesProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';
import { ListGroup } from '@bpmn-io/properties-panel';
import { getVariables } from './util';

const LOW_PRIORITY = 500;

export default function VariablePropertiesProvider(propertiesPanel, injector, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            // 为所有任务节点类型添加变量配置（合并 Input 和 Output）
            if (is(element, 'bpmn:Task') ||
                is(element, 'bpmn:UserTask') ||
                is(element, 'bpmn:ServiceTask') ||
                is(element, 'bpmn:ScriptTask') ||
                is(element, 'bpmn:BusinessRuleTask') ||
                is(element, 'bpmn:SendTask') ||
                is(element, 'bpmn:ReceiveTask') ||
                is(element, 'bpmn:ManualTask')) {
                
                groups.push(createVariablesGroup(element, injector, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

VariablePropertiesProvider.$inject = ['propertiesPanel', 'injector', 'translate'];

function createVariablesGroup(element, injector, translate) {
    const variablesGroup = {
        id: 'variables',
        label: 'Variables',
        component: ListGroup,
        ...variablesProps({ element, injector })
    };
    return variablesGroup;
}

