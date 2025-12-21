import nameProps from './parts/NameProps';
import idProps from './parts/IdProps';
import codeProps from './parts/CodeProps';
import urlProps from './parts/UrlProps';

const LOW_PRIORITY = 500;

export default function IdentitiyPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        return function (groups) {
            groups.push(createIdentityGroup(element, translate));
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

IdentitiyPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createIdentityGroup(element, translate) {
    const identityGroup = {
        id: 'general',
        label: kresource.getItem('general'),
        entries: [nameProps(element), idProps(element), codeProps(element), urlProps(element)]
    };
    return identityGroup;
}
