import ruleConfigProps from './parts/RuleConfigProps';
import { is } from 'bpmn-js/lib/util/ModelUtil';
import { ListGroup } from '@bpmn-io/properties-panel';

const LOW_PRIORITY = 500;

export default function RuleTaskPropertiesProvider(propertiesPanel, injector, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:BusinessRuleTask')) {
                groups.push(createRuleConfigGroup(element, injector, translate));
            }
            return groups;
        };
    };
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

RuleTaskPropertiesProvider.$inject = [ 'propertiesPanel', 'injector', 'translate' ];

function createRuleConfigGroup(element, injector) {
    return {
        id: 'ruleConfig',
        label: 'Rule Set',
        component: ListGroup,
        ...ruleConfigProps({ element, injector })
    };
}

