import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';



import {
    createElement,
    createNotifications,
    getNotifications,
    getNotificationsExtension
} from '../util';

import { forEach, without } from 'min-dash';

export default function NotificationsProps({element, injector, event}) {
    const notifications = getNotifications(element) || [];
    const bpmnFactory = injector.get('bpmnFactory'),
        commandStack = injector.get('commandStack');

    const items = notifications.map((notification, index) => {
        const id = element.id + '-user-' + index;
        return {
            id,
            label: notification.get('name') || '',
            autoFocusEntry: id + '-name',
            remove: removeFactory({ commandStack, element, notification })
        };
    });

    return {
        items,
        add: addFactory({ element, bpmnFactory, commandStack })
    };
}

function addFactory({ element, bpmnFactory, commandStack }) {
    return function (event) {
        event.stopPropagation();

        if ("undefined" !== typeof userlist) {
            userlist.afterRecipientSelected.subscribe(afterRecipientSelected);
        }

        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupContent"></div>'),
            title: kresource.getItem("userlist"),
            onshown: function () {
                $("#popupContent").load('pages/user/list.html')
            },
            onhidden: function () {
                executeMultiCommad(element, bpmnFactory, commandStack);
            },
            draggable: true
        });

    }
}

var mxRecipientSelected = null;
var mxRecipientType = null;

function afterRecipientSelected(evt, data) {
    mxRecipientSelected = data.RecipientItem;
    mxRecipientType = data.RecipientType;
}

function isRecipientExisted(element) {
    var isExisted = false;
    const notifications = getNotifications(element) || [];
    const recipient = mxRecipientSelected;
    if (recipient !== null) {
        notifications.forEach(function (item, index) {
            if (item.outerId === recipient.Id) {
                isExisted = true;
            }
        })
    }
    return isExisted;
}

function executeMultiCommad(element, bpmnFactory, commandStack) {
    //check the recipient whether exist
    if (isRecipientExisted(element) === true) {
        return;
    }

    const commands = [];

    const businessObject = getBusinessObject(element);

    let extensionElements = businessObject.get('extensionElements');

    if (!extensionElements) {
        extensionElements = createElement(
            'bpmn:ExtensionElements',
            { values: [] },
            businessObject,
            bpmnFactory
        );

        commands.push({
            cmd: 'element.updateModdleProperties',
            context: {
                element,
                moddleElement: businessObject,
                properties: { extensionElements }
            }
        });
    }

    let extension = getNotificationsExtension(element);

    if (!extension) {
        extension = createNotifications({
            values: []
        }, extensionElements, bpmnFactory);

        commands.push({
            cmd: 'element.updateModdleProperties',
            context: {
                element,
                moddleElement: extensionElements,
                properties: {
                    values: [...extensionElements.get('values'), extension]
                }
            }
        });
    }

    if (mxRecipientSelected !== null) {
        const newNotification = createElement('sf:Notification', {
            name: mxRecipientSelected.UserName,
            outerId: mxRecipientSelected.UserId,
            outerCode: mxRecipientSelected.UserCode ? mxRecipientSelected.UserCode : '',
            outerType: mxRecipientType    //user
        }, extension, bpmnFactory);

        commands.push({
            cmd: 'element.updateModdleProperties',
            context: {
                element,
                moddleElement: extension,
                properties: {
                    values: [...extension.get('values'), newNotification]
                }
            }
        });
        commandStack.execute('properties-panel.multi-command-executor', commands);
    } 
}

function removeFactory({ commandStack, element, notification }) {
    return function (event) {
        event.stopPropagation();
        const extension = getNotificationsExtension(element);
        if (!extension) {
            return;
        }

        const notifications = without(extension.get('values'), notification);
        commandStack.execute('element.updateModdleProperties', {
            element,
            moddleElement: extension,
            properties: {
                values: notifications
            }
        });
    };
}