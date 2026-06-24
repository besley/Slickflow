import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

function getExtensionElement(element, type) {
    if (!element.extensionElements || !element.extensionElements.values) return null;
    return element.extensionElements.values.filter((ext) => ext.$instanceOf(type))[0] || null;
}

function getRuleConfig(element) {
    const bo = getBusinessObject(element);
    const cfgs = getExtensionElement(bo, 'sf:RuleConfigs');
    if (cfgs && cfgs.ruleConfigs && cfgs.ruleConfigs.length > 0) {
        return cfgs.ruleConfigs[0];
    }
    return null;
}

/** Legacy BPMN had sf:ruleConfig/@bindingsJson; runtime only needs ruleSetCode + wf_rule_set. */
function stripLegacyBindingsJson(ruleConfig) {
    if (!ruleConfig) return;
    if (Object.prototype.hasOwnProperty.call(ruleConfig, 'bindingsJson')) {
        delete ruleConfig.bindingsJson;
    }
    const attrs = ruleConfig.$attrs;
    if (attrs && typeof attrs === 'object' && Object.prototype.hasOwnProperty.call(attrs, 'bindingsJson')) {
        delete attrs.bindingsJson;
    }
}

export default function RuleConfigProps({ element, injector }) {
    const cfg = getRuleConfig(element);
    const code = cfg ? (cfg.ruleSetCode || '') : '';

    return {
        items: [
            {
                id: element.id + '-ruleconfig',
                label: code ? ('RuleSet: ' + code) : 'RuleSet: (not set)',
                autoFocusEntry: element.id + '-ruleconfig-name',
                remove: function () { }
            }
        ],
        add: addFactory({ element, injector })
    };
}

function addFactory({ element, injector }) {
    return function (event) {
        event.stopPropagation();
        const modeling = injector.get('modeling');
        const bo = getBusinessObject(element);
        const moddle = injector.get('moddle');

        const current = getRuleConfig(element);
        const currentCode = current ? (current.ruleSetCode || '') : '';

        const apiBase = (window.kconfig && window.kconfig.webApiUrl) || '';
        const loadRuleSets = function (cb) {
            if (!apiBase) return cb([]);
            fetch(apiBase + 'api/RuleSet/GetList', { method: 'GET', headers: { Accept: 'application/json' } })
                .then((r) => r.json())
                .then((res) => cb((res && res.Status === 1 && Array.isArray(res.Entity)) ? res.Entity : []))
                .catch(() => cb([]));
        };

        loadRuleSets(function (rows) {
            const options = ['<option value="">-- select --</option>']
                .concat(rows.map(function (r) {
                    const c = r.RuleSetCode || r.ruleSetCode || '';
                    const n = r.RuleSetName || r.ruleSetName || '';
                    return '<option value="' + c + '"' + (c === currentCode ? ' selected' : '') + '>' + c + (n ? (' - ' + n) : '') + '</option>';
                }))
                .join('');

            const message = window.$(
                '<div style="padding:8px;">' +
                '<div class="form-group"><label>Rule Set (wf_rule_set.rule_set_code)</label>' +
                '<select id="dlgRuleSetCode" class="form-control">' + options + '</select></div>' +
                '<p style="font-size:12px;color:#6b7280;margin-top:8px;">Rule inputs use process/activity variables by name at runtime; rule body is <code>wf_rule_set.rule_content</code>.</p>' +
                '</div>'
            );

            window.BootstrapDialog.show({
                title: 'Rule Set',
                message: message,
                draggable: true,
                buttons: [
                    { label: 'Cancel', action: function (d) { d.close(); } },
                    {
                        label: 'Save',
                        cssClass: 'btn-primary',
                        action: function (d) {
                            const code = window.$('#dlgRuleSetCode').val() || '';

                            const extensionElements = bo.extensionElements || moddle.create('bpmn:ExtensionElements');
                            let ruleConfigs = getExtensionElement(bo, 'sf:RuleConfigs');
                            let ruleConfig = ruleConfigs && ruleConfigs.ruleConfigs && ruleConfigs.ruleConfigs[0];
                            if (!ruleConfigs) {
                                ruleConfigs = moddle.create('sf:RuleConfigs');
                                extensionElements.get('values').push(ruleConfigs);
                            }
                            if (!ruleConfig) {
                                ruleConfig = moddle.create('sf:RuleConfig');
                                ruleConfigs.get('ruleConfigs').push(ruleConfig);
                            }
                            ruleConfig.ruleSetCode = code;
                            stripLegacyBindingsJson(ruleConfig);
                            modeling.updateProperties(element, { extensionElements: extensionElements });
                            d.close();
                            return true;
                        }
                    }
                ]
            });
        });
    };
}
