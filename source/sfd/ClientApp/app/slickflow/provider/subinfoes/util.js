import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export function getSubInfoes(element) {
    const subInfoes = getSubInfoesExtension(element);

    return subInfoes && subInfoes.get('values');
}

export function getSubInfoesExtension(element) {
    const businessObject = getBusinessObject(element);
    return getExtension(businessObject, 'sf:SubInfoes');
}

export function getExtension(element, type) {
    if (!element.extensionElements || !element.extensionElements.values) {
        return null;
    }

    return element.extensionElements.values.filter(function (e) {
        return e.$instanceOf(type);
    })[0];
}

export function createElement(elementType, properties, parent, factory) {
    const element = factory.create(elementType, properties);

    if (parent) {
        element.$parent = parent;
    }
    return element;
}

export function createSubInfoes(properties, parent, bpmnFactory) {
    return createElement('sf:SubInfoes', properties, parent, bpmnFactory);
}