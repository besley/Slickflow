
import complexTypeProps from './parts/ComplexTypeProps';
import mergeTypeProps from './parts/MergeTypeProps';
import compareTypeProps from './parts/CompareTypeProps';
import completeOrderProps from './parts/CompleteOrderProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';

const LOW_PRIORITY = 500;

export default function MultiplePropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
    return function(groups) {
        if (is(element, 'bpmn:Task')
            || is(element, 'bpmn:UserTask')) {
            groups.push(createMultiSignGroup(element, translate));
      }
      return groups;
    }
  };

  propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

MultiplePropertiesProvider.$inject = [ 'propertiesPanel', 'translate' ];

function createMultiSignGroup(element, translate) {
    const multiSignGroup = {
        id: 'multisign',
        label: kresource.getItem('multisign'),
        entries: [complexTypeProps(element), mergeTypeProps(element), compareTypeProps(element), completeOrderProps(element)]
    };

    return multiSignGroup
}
