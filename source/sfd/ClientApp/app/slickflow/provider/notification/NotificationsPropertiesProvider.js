import notificationsProps from './parts/NotificationsProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';
import { ListGroup } from '@bpmn-io/properties-panel';

const LOW_PRIORITY = 500;

export default function NotificationsPropertiesProvider(propertiesPanel, injector, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:Task')
                || is(element, 'bpmn:UserTask')) {
                
                groups.push(createNotificationsGroup(element, injector, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

NotificationsPropertiesProvider.$inject = ['propertiesPanel', 'injector', 'translate'];

function createNotificationsGroup(element, injector, translate) {
    const notificationsGroup = {
        id: 'notification',
        label: kresource.getItem('notification'),
        component: ListGroup,
        ...notificationsProps({ element, injector})
    };
    return notificationsGroup;
}
