import { ListGroup } from '@bpmn-io/properties-panel';
import { openJsonEditorDialog, getSectionGetSet } from './parts/CustomProps';

const LOW_PRIORITY = 500;

export default function SectionPropertiesProvider(propertiesPanel, translate, injector) {
    this.getGroups = function (element) {
        return function (groups) {
            groups.push(createSectionGroup(element, translate, injector));
            return groups;
        };
    };
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

SectionPropertiesProvider.$inject = ['propertiesPanel', 'translate', 'injector'];

function createSectionGroup(element, translate, injector) {
    var moddle = injector.get('moddle');
    var modeling = injector.get('modeling');
    var _a = getSectionGetSet(element, moddle, modeling);
    var getValue = _a.getValue;
    var setValue = _a.setValue;
    return {
        id: 'extension',
        label: kresource.getItem('extraproperties'),
        component: ListGroup,
        items: [],
        add: function () {
            openJsonEditorDialog(getValue, setValue);
        }
    };
}
