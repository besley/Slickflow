import fieldsProps from './parts/FieldsProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';
import { ListGroup } from '@bpmn-io/properties-panel';

const LOW_PRIORITY = 500;

export default function FieldsPropertiesProvider(propertiesPanel, injector, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:Task')
                || is(element, 'bpmn:UserTask')) {
                groups.push(createFieldsGroup(element, injector, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

FieldsPropertiesProvider.$inject = ['propertiesPanel', 'injector', 'translate'];

function createFieldsGroup(element, injector, translate) {
    const fieldsGroup = {
        id: 'fieldspermission',
        label: kresource.getItem('fieldspermission'),
        component: ListGroup,
        ...fieldsProps({ element, injector})
    };
    return fieldsGroup;
}
