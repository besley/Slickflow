
import splitTypeProps from './parts/SplitTypeProps';
import joinTypeProps from './parts/JoinTypeProps';
import passTypeProps from './parts/PassTypeProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';
import sfModelUtility from '../SfModelUtility';

const LOW_PRIORITY = 500;

export default function GatewayPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
    return function(groups) {
        if (is(element, 'bpmn:InclusiveGateway')
            || is(element, 'bpmn:ExclusiveGateway')
            || is(element, 'bpmn:ParallelGateway')) {
            groups.push(createGatewayTypeGroup(element, translate));
      }
      return groups;
    }
  };

  propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

GatewayPropertiesProvider.$inject = [ 'propertiesPanel', 'translate' ];

function createGatewayTypeGroup(element, translate) {
    var entries = [];
    if (sfModelUtility.isSplit(element)) {
        entries.push(splitTypeProps(element))
    }

    if (sfModelUtility.isJoin(element)) {
        entries.push(joinTypeProps(element));
        entries.push(passTypeProps(element));
    }

    const gatewayTypeGroup = {
        id: 'gatewaytype',
        label: kresource.getItem('gateway'),
        entries: entries
    };

    return gatewayTypeGroup
}
