import overdueProps from './parts/OverdueProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';

const LOW_PRIORITY = 500;

export default function BoundaryPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:Task')
                || is(element, 'bpmn:UserTask')) {
                groups.push(createBoundaryGroup(element, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

BoundaryPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createBoundaryGroup(element, translate) {
    const boundaryGroup = {
        id: 'boundary',
        label: kresource.getItem('overdue'),
        entries: [overdueProps(element)]
    };
    return boundaryGroup;
}
