import expressionProps from './parts/ExpressionProps';


import { is } from 'bpmn-js/lib/util/ModelUtil';
import sfModelUtility from '../SfModelUtility'

const LOW_PRIORITY = 500;

export default function ExpressionPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:IntermediateCatchEvent')
                || is(element, 'bpmn:IntermediateThrowEvent')) {
                groups.push(createExpressionGroup(element, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

ExpressionPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createExpressionGroup(element, translate) {   
    const expressionGroup = {
        id: 'expression',
        label: kresource.getItem('expression'),
        entries: [expressionProps(element)]
    };
    return expressionGroup;
}
