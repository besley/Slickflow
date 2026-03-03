import { createJSONEditor } from 'vanilla-jsoneditor';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';
import { forEach } from 'min-dash';

function parseContent(value) {
    if (value == null || value === '') {
        return { json: {} };
    }
    const str = typeof value === 'string' ? value : String(value);
    try {
        const parsed = JSON.parse(str);
        return { json: parsed != null && typeof parsed === 'object' ? parsed : {} };
    } catch (e) {
        return { text: str };
    }
}

function filterMenuNoTable(items) {
    if (!items || !Array.isArray(items)) return items;
    return items.filter(function (item) {
        if (item.type === 'dropdown-button' && item.main && item.main.text) {
            var t = (item.main.text + '').toLowerCase();
            if (t.indexOf('table') !== -1) return false;
        }
        if (item.text && (item.text + '').toLowerCase().indexOf('table') !== -1) return false;
        if (item.items && Array.isArray(item.items)) {
            item.items = item.items.filter(function (sub) {
                return !(sub.text && (sub.text + '').toLowerCase().indexOf('table') !== -1);
            });
        }
        return true;
    });
}

function openJsonEditorDialog(getValue, setValue) {
    var BootstrapDialog = typeof window !== 'undefined' && window.BootstrapDialog;
    var $ = typeof window !== 'undefined' && window.$;
    if (!BootstrapDialog || !$) {
        console.warn('Extra Properties: BootstrapDialog or jQuery not available.');
        return;
    }
    var content = parseContent(getValue());
    var containerId = 'extra-props-json-editor-container';
    var $message = $('<div id="' + containerId + '" class="extra-props-json-editor-body" style="min-height:400px;height:100%;border:1px solid #ddd;border-radius:4px;overflow:hidden;"></div>');
    var editorInstance = null;
    var dialogRef = BootstrapDialog.show({
        title: kresource.getItem('extraproperties') || 'Extra Properties',
        message: $message,
        size: BootstrapDialog.SIZE_LARGE,
        cssClass: 'extra-props-json-dialog',
        draggable: true,
        onshown: function (dlg) {
            var el = document.getElementById(containerId);
            if (!el) return;
            editorInstance = createJSONEditor({
                target: el,
                props: {
                    content: content,
                    mode: 'text',
                    mainMenuBar: true,
                    navigationBar: false,
                    statusBar: false,
                    askToFormat: false,
                    onRenderMenu: filterMenuNoTable,
                    onChange: function (updatedContent) {
                        content = updatedContent;
                    }
                }
            });
            try {
                var $modal = dlg.getModalDialog && dlg.getModalDialog();
                if ($modal && $modal.length) {
                    $modal.css({ width: '800px', maxWidth: '95vw', height: '560px', maxHeight: '85vh' });
                    $modal.find('.modal-body').css({ height: 'calc(100% - 110px)', overflow: 'hidden', padding: '8px' });
                }
            } catch (e) {}
        },
        buttons: [
            {
                label: kresource.getItem('cancel') || 'Cancel',
                action: function (dlg) {
                    dlg.close();
                }
            },
            {
                label: kresource.getItem('save') || 'OK',
                cssClass: 'btn-primary',
                action: function (dlg) {
                    if (editorInstance) {
                        var c = editorInstance.get();
                        var str = c.json !== undefined ? JSON.stringify(c.json) : (c.text != null ? c.text : '{}');
                        setValue(str);
                    }
                    dlg.close();
                }
            }
        ],
        onhidden: function () {
            if (editorInstance) {
                try {
                    editorInstance.destroy();
                } catch (e) {}
                editorInstance = null;
            }
        }
    });
}

function getExtensionElement(element, type) {
    if (!element.extensionElements || !element.extensionElements.values) {
        return;
    }
    return element.extensionElements.values.find((ext) => ext.$instanceOf(type));
}

function SearchPropertyElement(element) {
    var list = element.sections;
    if (!list) return null;
    var node = null;
    forEach(list, function (item) {
        if (item.name === 'myProperties') node = item;
    });
    return node;
}

export function getSectionGetSet(element, moddle, modeling) {
    function getValue() {
        var businessObject = getBusinessObject(element);
        var extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        if (!extensionElements) return '';
        var sectionsElement = getExtensionElement(businessObject, 'sf:Sections');
        if (!sectionsElement) return '';
        var sectionElement = SearchPropertyElement(sectionsElement);
        return sectionElement ? (sectionElement.text != null ? sectionElement.text : '') : '';
    }
    function setValue(value) {
        var businessObject = getBusinessObject(element);
        var extensionElements = businessObject.extensionElements || moddle.create('bpmn:ExtensionElements');
        var sectionsElement = getExtensionElement(businessObject, 'sf:Sections');
        var sectionElement = null;
        if (!sectionsElement) {
            sectionsElement = moddle.create('sf:Sections');
            extensionElements.get('values').push(sectionsElement);
            sectionElement = moddle.create('sf:Section');
            sectionElement.name = 'myProperties';
            sectionsElement.get('sections').push(sectionElement);
        } else {
            sectionElement = sectionsElement.get('sections')[0];
            if (!sectionElement) {
                sectionElement = moddle.create('sf:Section');
                sectionsElement.get('sections').push(sectionElement);
            }
        }
        if (sectionElement) sectionElement.text = value;
        modeling.updateProperties(element, { extensionElements });
    }
    return { getValue, setValue };
}

export { openJsonEditorDialog };
