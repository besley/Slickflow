import performersProps from './parts/PerformersProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';
import { ListGroup } from '@bpmn-io/properties-panel';

const LOW_PRIORITY = 500;

export default function PerformersPropertiesProvider(propertiesPanel, injector, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:Task')
                || is(element, 'bpmn:UserTask')) {

                groups.push(createPerformersGroup(element, injector, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

PerformersPropertiesProvider.$inject = ['propertiesPanel', 'injector', 'translate'];

function createPerformersGroup(element, injector, translate) {
    const performersGroup = {
        id: 'role',
        label: kresource.getItem('role'),
        component: ListGroup,
        ...performersProps({ element, injector})
    };
    return performersGroup;
}
