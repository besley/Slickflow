import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export default class sfModelUtility {
    constructor() {

    }

    static isSplit(gateway) {
        var incomingCount = gateway.incoming.length;
        var outgoingCount = gateway.outgoing.length;

        if (incomingCount <= outgoingCount) return true;
        else return false;
    }

    static isJoin(gateway) {
        var incomingCount = gateway.incoming.length;
        var outgoingCount = gateway.outgoing.length;

        if (incomingCount >= outgoingCount) return true;
        else return false;
    }


    static isOrGateway(gateway) {
        if (gateway.type === 'bpmn:InclusiveGateway') return true;
        else return false;
    }

    static isXOrGateway(gateway) {
        if (gateway.type === 'bpmn:ExclusiveGateway') return true;
        else return false;
    }

    static isAndGateway(gateway) {
        if (gateway.type === 'bpmn:ParallelGateway') return true;
        else return false;
    }

    static isXOrSplit(gateway) {
        if (this.isXOrGateway(gateway) && this.isSplit(gateway)) return true;
        else return false;
    }

    static isXOrJoin(gateway) {
        if (this.isXOrGateway(gateway) && this.isJoin(gateway)) return true;
        else return false;
    }

    static isApprovalOrSplit(gateway) {
        var activityTypeDetail = this.getGatewayDetail(gateway);
        if (activityTypeDetail) {
            var extraSplitType = activityTypeDetail.extraSplitType;
            if (extraSplitType === 'ApprovalOrSplit') return true;
        }
        return false;
    }

    static isAndSplitMI(gateway) {
        var activityTypeDetail = this.getGatewayDetail(gateway);
        if (activityTypeDetail) {
            var extraSplitType = activityTypeDetail.extraSplitType;
            if (extraSplitType === 'AndSplitMI') return true;
        }
        return false;
    }

    static isEOrJoin(gateway) {
        var activityTypeDetail = this.getGatewayDetail(gateway);
        if (activityTypeDetail) {
            var extraSplitType = activityTypeDetail.extraJoinType;
            if (extraSplitType === 'EOrJoin') return true;
        }
        return false;
    }

    static isAndJoinMI(gateway) {
        var activityTypeDetail = this.getGatewayDetail(gateway);
        if (activityTypeDetail) {
            var extraSplitType = activityTypeDetail.extraJoinType;
            if (extraSplitType === 'AndJoinMI') return true;
        }
        return false;
    }

    static getGatewayDetail(element) {
        var businessObject = getBusinessObject(element);
        var atdElement = this.getExtensionElement(businessObject, 'sf:GatewayDetail');
        return atdElement;
    }
     
    static getExtensionElement(element, type) {
        if (!element.extensionElements
            || !element.extensionElements.values) {
            return;
        }
        return element.extensionElements.values.filter((extensionElement) => {
            return extensionElement.$instanceOf(type);
        })[0];
    }
}