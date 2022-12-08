import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export function getPerformersExtension(element) {
    const businessObject = getBusinessObject(element);
    return getExtension(businessObject, 'sf:Performers');
}

export function getPerformers(element) {
    const performers = getPerformersExtension(element);

    return performers && performers.get('values');
}

export function getExtension(element, type) {
    if (!element.extensionElements) {
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

export function createPerformers(properties, parent, bpmnFactory) {
    return createElement('sf:Performers', properties, parent, bpmnFactory);
}