
import methodTypeProps from './parts/MethodTypeProps';
import subMethodTypeProps from './parts/SubMethodTypeProps';
import argusProps from './parts/ArgusProps';
import expressionProps from './parts/ExpressionProps';

import entryFieldSetting from '../EntryFieldSetting';

import { is } from 'bpmn-js/lib/util/ModelUtil';

const LOW_PRIORITY = 500;

export default function ServiceTaskPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:ServiceTask')) {
                groups.push(createServiceTaskGroup(element, translate));
            }
            return groups;
        }
    };

    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

ServiceTaskPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createServiceTaskGroup(element, translate) {
    const serviceTaskGroup = {
        id: 'servicetask',
        label: kresource.getItem('servicedetail'),
        entries: [
            methodTypeProps(element, onChange),
            subMethodTypeProps(element),
            argusProps(element),
            expressionProps(element)
        ]
    };
    return serviceTaskGroup
}

function onChange(value) {
    entryFieldSetting.show(value);
}
