
import argusProps from './parts/ArgusProps';
import scriptTextProps from './parts/ScriptTextProps';
import scriptTypeProps from './parts/ScriptTypeProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';

const LOW_PRIORITY = 500;

export default function ScriptTaskPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:ScriptTask')) {
                groups.push(createScriptTaskGroup(element, translate));
            }
            return groups;
        }
    };

    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

ScriptTaskPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createScriptTaskGroup(element, translate) {
    const scriptTaskGroup = {
        id: 'scripttask',
        label: kresource.getItem('scriptdetail'),
        entries: [scriptTypeProps(element), argusProps(element), scriptTextProps(element)]
    };
    return scriptTaskGroup
}
