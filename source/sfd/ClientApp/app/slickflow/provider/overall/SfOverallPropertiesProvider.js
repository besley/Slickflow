import guidProps from '../identity/parts/GuidProps';
import codeProps from '../identity/parts/CodeProps';
import urlProps from '../identity/parts/UrlProps';

import { forEach } from "min-dash";

const LOW_PRIORITY = 500;

export default function SfOverallPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            const generalGroup = findGroup(groups, 'general');

            if (generalGroup) {
                var guidProperty = guidProps(element);
                generalGroup.entries.push(guidProperty);

                var codeProperty = codeProps(element);
                generalGroup.entries.push(codeProperty);

                var urlProperty = urlProps(element);
                generalGroup.entries.push(urlProperty);
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

function findGroup(groups, id) {
    return groups.find(g => g.id === id);
}

SfOverallPropertiesProvider.$inject = ['propertiesPanel', 'translate'];
