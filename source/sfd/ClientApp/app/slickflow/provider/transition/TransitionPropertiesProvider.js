import conditionProps from './parts/ConditionProps';
import recieverTypeProps from './parts/RecieverTypeProps';
import priorityProps from './parts/PriorityProps';
import forcedMergeProps from './parts/ForcedMergeProps';
import approvalOptionsProps from './parts/ApprovalOptionsProps';

import { is } from 'bpmn-js/lib/util/ModelUtil';
import sfModelUtility from '../SfModelUtility'

const LOW_PRIORITY = 500;

export default function TransitionPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            if (is(element, 'bpmn:SequenceFlow')) {
                groups.push(createTransitionGroup(element, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

TransitionPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createTransitionGroup(element, translate) {
    var entries = [conditionProps(element), recieverTypeProps(element)];

    //根据split/join 获取设置条目
    var source = element.source;
    var target = element.target;

    if (sfModelUtility.isApprovalOrSplit(source)) {
        entries.push(approvalOptionsProps(element));
    } else if (sfModelUtility.isXOrSplit(source)) {
        entries.push(priorityProps(element));
    } else if (sfModelUtility.isEOrJoin(target)) {
        entries.push(forcedMergeProps(element));
    }
          
    const transitionGroup = {
        id: 'transition',
        label: kresource.getItem('transition'),
        entries: entries
    };
    return transitionGroup;
}
