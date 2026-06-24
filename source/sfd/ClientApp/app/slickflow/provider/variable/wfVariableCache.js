import { is } from 'bpmn-js/lib/util/ModelUtil';

function getProcessIdAndVersion(injector) {
    let processId = '';
    let version = '1';
    try {
        const canvas = injector.get('canvas');
        const root = canvas && canvas.getRootElement();
        if (root && root.businessObject) {
            processId = root.businessObject.id || root.id || '';
        }
    } catch (e) {
        // ignore
    }
    if (window.kmain && window.kmain.mxSelectedProcessEntity) {
        if (window.kmain.mxSelectedProcessEntity.ProcessId) {
            processId = window.kmain.mxSelectedProcessEntity.ProcessId;
        }
        if (window.kmain.mxSelectedProcessEntity.Version) {
            version = window.kmain.mxSelectedProcessEntity.Version;
        }
    }
    return { processId, version };
}

function isTaskLike(el) {
    if (!el) return false;
    return (
        is(el, 'bpmn:Task') ||
        is(el, 'bpmn:UserTask') ||
        is(el, 'bpmn:ServiceTask') ||
        is(el, 'bpmn:ScriptTask') ||
        is(el, 'bpmn:BusinessRuleTask') ||
        is(el, 'bpmn:SendTask') ||
        is(el, 'bpmn:ReceiveTask') ||
        is(el, 'bpmn:ManualTask')
    );
}

/**
 * After selection changes, load wf_variable rows into window.__wfVariableCache[activityId]
 * so the Variables group can render without reading sf:Variables from BPMN XML.
 */
export function initWfVariableCacheLoader(eventBus, injector) {
    eventBus.on('selection.changed', function (e) {
        const el = e.newSelection && e.newSelection[0];
        if (!isTaskLike(el)) {
            return;
        }
        const { processId, version } = getProcessIdAndVersion(injector);
        const apiBase = (window.kconfig && window.kconfig.webApiUrl) || '';
        if (!apiBase || !processId || !el.id) {
            return;
        }
        const url =
            apiBase +
            'api/WfVariable/GetList?processId=' +
            encodeURIComponent(processId) +
            '&version=' +
            encodeURIComponent(version) +
            '&activityId=' +
            encodeURIComponent(el.id);
        fetch(url, { method: 'GET', headers: { Accept: 'application/json' } })
            .then(function (r) {
                return r.json();
            })
            .then(function (result) {
                if (result.Status === 1 && result.Entity && Array.isArray(result.Entity)) {
                    window.__wfVariableCache = window.__wfVariableCache || {};
                    window.__wfVariableCache[el.id] = result.Entity;
                    // bpmn-js-properties-panel 只响应 selection.changed / elements.changed；
                    // 单独 fire element.changed 不会刷新属性面板，导致变量条数需第二次选中才更新。
                    eventBus.fire('elements.changed', { elements: [el] });
                }
            })
            .catch(function () {
                /* ignore */
            });
    });
}

/**
 * Strip all sf:Variables extensions before serializing BPMN XML (variables live in wf_variable).
 */
export function stripSfVariablesFromDiagram(modeling, elementRegistry) {
    if (!modeling || !elementRegistry) {
        return;
    }
    elementRegistry.getAll().forEach(function (el) {
        const bo = el.businessObject;
        if (!bo || !bo.extensionElements) {
            return;
        }
        const extensionElements = bo.extensionElements;
        const values = extensionElements.get ? extensionElements.get('values') || [] : extensionElements.values || [];
        const newValues = values.filter(function (e) {
            return e.$type !== 'sf:Variables';
        });
        if (newValues.length !== values.length) {
            modeling.updateModdleProperties(el, extensionElements, { values: newValues });
        }
    });
}

export { getProcessIdAndVersion, isTaskLike };
