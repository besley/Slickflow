import customProps from './parts/CustomProps';


const LOW_PRIORITY = 500;

export default function SectionPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            groups.push(createSectionGroup(element, translate));
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

SectionPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createSectionGroup(element, translate) {
    const sectionGroup = {
        id: 'extension',
        label: kresource.getItem('extraproperties'),
        entries: [customProps(element)]
    };
    return sectionGroup;
}
