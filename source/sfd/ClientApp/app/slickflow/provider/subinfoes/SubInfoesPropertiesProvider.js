import subInfoesProps from './parts/SubInfoesProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';
import { ListGroup } from '@bpmn-io/properties-panel';

const LOW_PRIORITY = 500;

export default function SubInfoesPropertiesProvider(propertiesPanel, injector, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:SubProcess')) {
                groups.push(createSubInfoesGroup(element, injector, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

SubInfoesPropertiesProvider.$inject = ['propertiesPanel', 'injector', 'translate'];

function createSubInfoesGroup(element, injector, translate) {
    const subInfoesGroup = {
        id: 'subinfoes',
        label: kresource.getItem('subprocess'),
        component: ListGroup,
        ...subInfoesProps({ element, injector })
    };
    return subInfoesGroup;
}