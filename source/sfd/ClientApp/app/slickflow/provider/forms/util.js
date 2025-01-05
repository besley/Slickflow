import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export function getForms(element) {
    const forms = getFormsExtension(element);

    return forms && forms.get('values');
}

export function getFormsExtension(element) {
    const businessObject = getBusinessObject(element);
    return getExtension(businessObject, 'sf:Forms');
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

export function createForms(properties, parent, bpmnFactory) {
    return createElement('sf:Forms', properties, parent, bpmnFactory);
}