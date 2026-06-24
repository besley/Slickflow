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

/**
 * Remove sf:Variables extension from the element (variables are persisted in wf_variable only).
 */
export function clearVariablesExtension(element, commandStack) {
    const businessObject = getBusinessObject(element);
    const extension = getVariablesExtension(element);
    if (!extension || !businessObject.extensionElements) {
        return;
    }
    const extensionElements = businessObject.extensionElements;
    const values = extensionElements.get('values') || [];
    const newValues = values.filter(function (e) {
        return e.$type !== 'sf:Variables';
    });
    if (newValues.length === values.length) {
        return;
    }
    commandStack.execute('element.updateModdleProperties', {
        element: element,
        moddleElement: extensionElements,
        properties: { values: newValues }
    });
}

