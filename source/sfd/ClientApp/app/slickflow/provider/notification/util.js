import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export function getNotifications(element) {
    const notifications = getNotificationsExtension(element);

    return notifications && notifications.get('values');
}

export function getNotificationsExtension(element) {
    const businessObject = getBusinessObject(element);
    return getExtension(businessObject, 'sf:Notifications');
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

export function createNotifications(properties, parent, bpmnFactory) {
    return createElement('sf:Notifications', properties, parent, bpmnFactory);
}