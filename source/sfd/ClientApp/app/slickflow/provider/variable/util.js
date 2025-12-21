import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export function getVariables(element) {
    const variables = getVariablesExtension(element);
    if (!variables) {
        return { inputVariables: [], outputVariables: [] };
    }
    return {
        inputVariables: variables.get('inputVariables') || [],
        outputVariables: variables.get('outputVariables') || []
    };
}

export function getVariablesExtension(element) {
    const businessObject = getBusinessObject(element);
    return getExtension(businessObject, 'sf:Variables');
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

export function createVariables(properties, parent, bpmnFactory) {
    return createElement('sf:Variables', properties, parent, bpmnFactory);
}

