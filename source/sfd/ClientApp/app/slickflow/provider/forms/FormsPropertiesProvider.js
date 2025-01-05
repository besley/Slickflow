import formsProps from './parts/FormsProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';
import { ListGroup } from '@bpmn-io/properties-panel';

const LOW_PRIORITY = 500;

export default function FormsPropertiesProvider(propertiesPanel, injector, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:Process')) {
                groups.splice(1, 0, createFormsGroup(element, injector, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

FormsPropertiesProvider.$inject = ['propertiesPanel', 'injector', 'translate'];

function createFormsGroup(element, injector, translate) {
    const formsGroup = {
        id: 'form',
        label: kresource.getItem('form'),
        component: ListGroup,
        ...formsProps({ element, injector})
    };
    return formsGroup;
}
