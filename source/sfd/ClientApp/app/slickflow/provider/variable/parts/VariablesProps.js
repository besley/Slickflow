import { getBusinessObject, is } from 'bpmn-js/lib/util/ModelUtil';
import { getVariables, getVariablesExtension, clearVariablesExtension } from '../util';
import { getProcessIdAndVersion } from '../wfVariableCache';

import { without } from 'min-dash';

function readRowField(row, pascalKey, camelKey, snakeKey) {
    if (!row || typeof row !== 'object') {
        return undefined;
    }
    if (typeof row[pascalKey] !== 'undefined') {
        return row[pascalKey];
    }
    if (camelKey && typeof row[camelKey] !== 'undefined') {
        return row[camelKey];
    }
    if (snakeKey && typeof row[snakeKey] !== 'undefined') {
        return row[snakeKey];
    }
    return undefined;
}

/**
 * Build moddle-like variable wrappers from wf_variable rows (for labels / dialog prefill).
 */
function rowsFromCacheToAllVariables(cachedRows) {
    return cachedRows.map(function (row) {
        const directionRaw = readRowField(row, 'Direction', 'direction', null) || 'Input';
        const direction = directionRaw === 'Output' ? 'Output' : 'Input';
        const variable = {
            get: function (key) {
                if (key === 'direction') {
                    return direction;
                }
                if (key === 'name') {
                    return readRowField(row, 'Name', 'name', null) || '';
                }
                if (key === 'type') {
                    return readRowField(row, 'Type', 'type', null) || '';
                }
                if (key === 'defaultValue') {
                    return readRowField(row, 'DefaultValue', 'defaultValue', 'default_value') || '';
                }
                if (key === 'isRequired') {
                    var req = readRowField(row, 'IsRequired', 'isRequired', 'is_required');
                    return !!(req === 1 || req === true);
                }
                if (key === 'varRefDetail') {
                    var sr = readRowField(row, 'SourceRef', 'sourceRef', 'source_ref') || '';
                    var svn = readRowField(row, 'SourceVariableName', 'sourceVariableName', 'source_variable_name') || '';
                    if (sr || svn) {
                        return [
                            {
                                get: function (k) {
                                    if (k === 'sourceRef') {
                                        return sr || '';
                                    }
                                    if (k === 'variableName') {
                                        return svn || '';
                                    }
                                    return '';
                                }
                            }
                        ];
                    }
                    return [];
                }
                return '';
            }
        };
        return { variable: variable, direction: direction, __dbRow: row };
    });
}

function buildItemEntry({ element, injector, commandStack, variable, direction, index, rowSnapshot, fromDb }) {
    const variableDirection = variable.get('direction') || direction;
    const id = element.id + '-variable-' + variableDirection + '-' + index;
    const variableName = variable.get('name') || '';
    const varRefDetail = variable.get('varRefDetail');
    let srcNodeId = '';
    let srcVarName = '';
    if (varRefDetail && varRefDetail.length > 0) {
        const refDetail = varRefDetail[0];
        srcNodeId = refDetail.get('sourceRef') || '';
        srcVarName = refDetail.get('variableName') || '';
    }
    const isRequired = !!variable.get('isRequired');
    const directionLabel = variableDirection === 'Input' ? '[IN]' : '[OUT]';
    let displayLabel = directionLabel + ' ' + (variableName || 'Variable ' + (index + 1));
    if (isRequired) {
        displayLabel += ' • required';
    }
    if (variableDirection === 'Input' && srcNodeId && srcVarName) {
        try {
            const elementRegistry = injector.get('elementRegistry');
            if (elementRegistry) {
                const sourceNode = elementRegistry.get(srcNodeId);
                if (sourceNode) {
                    displayLabel += ' [' + getNodeName(sourceNode) + ']';
                } else {
                    displayLabel += ' [' + srcNodeId + ']';
                }
            } else {
                displayLabel += ' [' + srcNodeId + ']';
            }
        } catch (e) {
            displayLabel += ' [' + srcNodeId + ']';
        }
    }
    const remove = fromDb
        ? removeFactoryDb({ element, injector, commandStack, rowSnapshot: rowSnapshot })
        : removeFactory({ commandStack, element, variable, direction: variableDirection });
    return {
        id: id,
        label: displayLabel,
        autoFocusEntry: id + '-name',
        remove: remove
    };
}

export default function VariablesProps({ element, injector }) {
    const bpmnFactory = injector.get('bpmnFactory');
    const commandStack = injector.get('commandStack');

    const hasCache =
        window.__wfVariableCache &&
        Object.prototype.hasOwnProperty.call(window.__wfVariableCache, element.id);
    const cached = hasCache ? window.__wfVariableCache[element.id] : null;

    let allVariables;
    let items;

    if (cached !== null && cached !== undefined && Array.isArray(cached)) {
        allVariables = rowsFromCacheToAllVariables(cached);
        items = allVariables.map(function (entry, index) {
            return buildItemEntry({
                element: element,
                injector: injector,
                commandStack: commandStack,
                variable: entry.variable,
                direction: entry.direction,
                index: index,
                rowSnapshot: entry.__dbRow || cached[index],
                fromDb: true
            });
        });
    } else {
        const variables = getVariables(element);
        allVariables = [
            ...(variables.inputVariables || []).map(function (v) {
                return { variable: v, direction: v.get('direction') || 'Input' };
            }),
            ...(variables.outputVariables || []).map(function (v) {
                return { variable: v, direction: v.get('direction') || 'Output' };
            })
        ];
        items = allVariables.map(function ({ variable, direction }, index) {
            return buildItemEntry({
                element: element,
                injector: injector,
                commandStack: commandStack,
                variable: variable,
                direction: direction,
                index: index,
                rowSnapshot: null,
                fromDb: false
            });
        });
    }

    return {
        items: items,
        add: addFactory({ element, injector, bpmnFactory, commandStack, initialVariables: allVariables })
    };
}

function removeFactoryDb({ element, injector, commandStack, rowSnapshot }) {
    return function (event) {
        event.stopPropagation();
        const processIdVersion = getProcessIdAndVersion(injector);
        const processId = processIdVersion.processId;
        const version = processIdVersion.version;
        const activityId = element.id;
        const apiBase = (window.kconfig && window.kconfig.webApiUrl) || '';
        if (!apiBase || !processId || !activityId || !rowSnapshot) {
            return;
        }
        const urlGet =
            apiBase +
            'api/WfVariable/GetList?processId=' +
            encodeURIComponent(processId) +
            '&version=' +
            encodeURIComponent(version) +
            '&activityId=' +
            encodeURIComponent(activityId);
        fetch(urlGet, { method: 'GET', headers: { Accept: 'application/json' } })
            .then(function (r) {
                return r.json();
            })
            .then(function (result) {
                if (result.Status !== 1 || !result.Entity || !Array.isArray(result.Entity)) {
                    return null;
                }
                const list = result.Entity.filter(function (v) {
                    var vName = readRowField(v, 'Name', 'name', null) || '';
                    var sName = readRowField(rowSnapshot, 'Name', 'name', null) || '';
                    var sameName = vName === sName;
                    var d1Raw = readRowField(v, 'Direction', 'direction', null) || 'Input';
                    var d2Raw = readRowField(rowSnapshot, 'Direction', 'direction', null) || 'Input';
                    var d1 = d1Raw === 'Output' ? 'Output' : 'Input';
                    var d2 = d2Raw === 'Output' ? 'Output' : 'Input';
                    return !(sameName && d1 === d2);
                });
                const urlSave =
                    apiBase +
                    'api/WfVariable/SaveList?processId=' +
                    encodeURIComponent(processId) +
                    '&version=' +
                    encodeURIComponent(version) +
                    '&activityId=' +
                    encodeURIComponent(activityId);
                return fetch(urlSave, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json', Accept: 'application/json' },
                    body: JSON.stringify(list)
                }).then(function () {
                    return list;
                });
            })
            .then(function (list) {
                if (!list) {
                    return;
                }
                window.__wfVariableCache = window.__wfVariableCache || {};
                window.__wfVariableCache[element.id] = list;
                clearVariablesExtension(element, commandStack);
                injector.get('eventBus').fire('elements.changed', { elements: [element] });
            })
            .catch(function (err) {
                if (window.kmsgbox) {
                    window.kmsgbox.error(err && err.message ? err.message : 'Request failed');
                }
            });
    };
}

// Get all upstream nodes and their output variables
function getUpstreamNodesWithOutputVariables(currentElement, injector) {
    const elementRegistry = injector.get('elementRegistry');
    const modeling = injector.get('modeling');
    
    if (!elementRegistry || !modeling) {
        return [];
    }

    const allElements = elementRegistry.getAll();
    const upstreamNodes = [];
    
    // Traverse all upstream nodes from current element
    const incomingFlows = currentElement.incoming || [];
    const visitedNodes = new Set();
    
    function traverseUpstream(nodeId) {
        if (visitedNodes.has(nodeId)) return;
        visitedNodes.add(nodeId);
        
        const node = elementRegistry.get(nodeId);
        if (!node) return;
        
        // Check if current node is a task
        if (is(node, 'bpmn:Task') || 
            is(node, 'bpmn:UserTask') || 
            is(node, 'bpmn:ServiceTask') || 
            is(node, 'bpmn:ScriptTask') ||
            is(node, 'bpmn:BusinessRuleTask') ||
            is(node, 'bpmn:SendTask') ||
            is(node, 'bpmn:ReceiveTask') ||
            is(node, 'bpmn:ManualTask')) {
            
            let outputVars = [];
            const cachedNv = window.__wfVariableCache && window.__wfVariableCache[node.id];
            if (cachedNv && Array.isArray(cachedNv)) {
                outputVars = cachedNv
                    .filter(function (v) {
                        return (readRowField(v, 'Direction', 'direction', null) || 'Input') === 'Output';
                    })
                    .map(function (v) {
                        return {
                            name: readRowField(v, 'Name', 'name', null) || '',
                            type: readRowField(v, 'Type', 'type', null) || ''
                        };
                    });
            } else {
                const nodeVariables = getVariables(node);
                const allVars = [
                    ...(nodeVariables.inputVariables || []),
                    ...(nodeVariables.outputVariables || [])
                ];
                outputVars = allVars
                    .filter(function (v) {
                        return v.get('direction') === 'Output';
                    })
                    .map(function (v) {
                        return { name: v.get('name') || '', type: v.get('type') || '' };
                    });
            }

            if (outputVars.length > 0) {
                const nodeName = getNodeName(node);
                upstreamNodes.push({
                    nodeId: node.id,
                    nodeName: nodeName,
                    outputVariables: outputVars
                });
            }
        }
        
        // Continue upstream traversal
        const nodeIncoming = node.incoming || [];
        nodeIncoming.forEach(flow => {
            if (flow.source) {
                traverseUpstream(flow.source.id);
            }
        });
    }
    
    // Start traversal from all incoming flows
    incomingFlows.forEach(flow => {
        if (flow.source) {
            traverseUpstream(flow.source.id);
        }
    });
    
    return upstreamNodes;
}

function getNodeName(element) {
    const bo = getBusinessObject(element);
    return bo.get('name') || element.id;
}

function mapApiToDialog(apiList) {
    if (!Array.isArray(apiList)) return [];
    return apiList.map(v => ({
        name: readRowField(v, 'Name', 'name', null) || '',
        type: readRowField(v, 'Type', 'type', null) || '',
        defaultValue: readRowField(v, 'DefaultValue', 'defaultValue', 'default_value') || '',
        direction: (readRowField(v, 'Direction', 'direction', null) || 'Input') === 'Output' ? 'Output' : 'Input',
        sourceNodeId: readRowField(v, 'SourceRef', 'sourceRef', 'source_ref') || '',
        sourceVariableName: readRowField(v, 'SourceVariableName', 'sourceVariableName', 'source_variable_name') || '',
        isRequired: !!(readRowField(v, 'IsRequired', 'isRequired', 'is_required') === 1 || readRowField(v, 'IsRequired', 'isRequired', 'is_required') === true),
        __edit: false
    }));
}

function mapDialogToApi(variablesData) {
    if (!Array.isArray(variablesData)) return [];
    return variablesData.map((v, i) => ({
        id: 0,
        processId: '',
        version: '',
        activityId: '',
        name: v.name || '',
        type: v.type || '',
        direction: v.direction || 'Input',
        defaultValue: v.defaultValue || '',
        default_value: v.defaultValue || '',
        isRequired: v.isRequired ? 1 : 0,
        is_required: v.isRequired ? 1 : 0,
        isReferenced: (v.direction === 'Input' && (v.sourceNodeId || v.sourceVariableName)) ? 1 : 0,
        is_referenced: (v.direction === 'Input' && (v.sourceNodeId || v.sourceVariableName)) ? 1 : 0,
        sourceRef: v.sourceNodeId || '',
        source_ref: v.sourceNodeId || '',
        sourceVariableName: v.sourceVariableName || '',
        source_variable_name: v.sourceVariableName || '',
        sortOrder: i,
        sort_order: i
    }));
}

function addFactory({ element, injector, bpmnFactory, commandStack, initialVariables }) {
    return function (event) {
        event.stopPropagation();

        if (typeof window.BootstrapDialog === 'undefined') {
            console.error('BootstrapDialog is not available');
            return;
        }

        const existingVariables = (initialVariables || []).map(({ variable, direction }) => {
            let sourceNodeId = '';
            let sourceVariableName = '';
            const varRefDetail = variable.get('varRefDetail');
            if (varRefDetail && varRefDetail.length > 0) {
                const refDetail = varRefDetail[0];
                sourceNodeId = refDetail.get('sourceRef') || '';
                sourceVariableName = refDetail.get('variableName') || '';
            }
            const variableDirection = variable.get('direction') || direction;
            return {
                name: variable.get('name') || '',
                type: variable.get('type') || '',
                defaultValue: variable.get('defaultValue') || '',
                direction: variableDirection,
                sourceNodeId,
                sourceVariableName,
                isRequired: !!variable.get('isRequired'),
                __edit: false
            };
        });

        const { processId, version } = getProcessIdAndVersion(injector);
        const activityId = element.id || '';

        const apiBase = (window.kconfig && window.kconfig.webApiUrl) || '';
        if (apiBase && processId && activityId) {
            const url = apiBase + 'api/WfVariable/GetList?processId=' + encodeURIComponent(processId) + '&version=' + encodeURIComponent(version) + '&activityId=' + encodeURIComponent(activityId);
            fetch(url, { method: 'GET', headers: { 'Accept': 'application/json' } })
                .then(r => r.json())
                .then(result => {
                    const initialData = (result.Status === 1 && result.Entity && Array.isArray(result.Entity))
                        ? mapApiToDialog(result.Entity)
                        : existingVariables;
                    openVariablesDialog({ element, injector, bpmnFactory, commandStack, initialData, processId, version, activityId, apiBase });
                })
                .catch(() => {
                    openVariablesDialog({ element, injector, bpmnFactory, commandStack, initialData: existingVariables, processId, version, activityId, apiBase });
                });
        } else {
            openVariablesDialog({ element, injector, bpmnFactory, commandStack, initialData: existingVariables, processId, version, activityId, apiBase: '' });
        }
    };
}

function openVariablesDialog({ element, injector, bpmnFactory, commandStack, initialData, processId, version, activityId, apiBase }) {
    let variablesData = initialData || [];
    processId = processId || '';
    version = version || '1';
    activityId = activityId || (element && element.id) || '';
    apiBase = apiBase || (window.kconfig && window.kconfig.webApiUrl) || '';

    const dialog = window.BootstrapDialog.show({
        message: window.$('<div id="variableDialogContent"></div>'),
        title: 'Variable List',
        size: window.BootstrapDialog.SIZE_WIDE || 'size-wide',
        cssClass: 'variables-dialog-large',
        onshown: function () {
            renderDialogContent();
            try {
                const $dlg = dialog.getModalDialog();
                $dlg.css({ width: '900px', maxWidth: '90vw', height: '600px', maxHeight: '90vh' });
                const $body = $dlg.find('.modal-body');
                if ($body.length) {
                    $body.css({ height: '520px', overflow: 'hidden', display: 'flex', flexDirection: 'column' });
                }
                // Ensure the content container has fixed height
                const $content = window.$('#variableDialogContent');
                if ($content.length) {
                    $content.css({ height: '100%', display: 'flex', flexDirection: 'column', overflow: 'hidden' });
                }
            } catch (e) {
                // ignore
            }
        },
        draggable: true,
        buttons: [
            {
                label: 'Cancel',
                action: function (dlg) {
                    dlg.close();
                }
            },
            {
                label: 'Save',
                cssClass: 'btn-primary',
                action: function (dlg) {
                    if (apiBase && processId && activityId) {
                        const url = apiBase + 'api/WfVariable/SaveList?processId=' + encodeURIComponent(processId) + '&version=' + encodeURIComponent(version) + '&activityId=' + encodeURIComponent(activityId);
                        fetch(url, {
                            method: 'POST',
                            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
                            body: JSON.stringify(mapDialogToApi(variablesData))
                        })
                            .then(r => r.json())
                            .then(result => {
                                if (result.Status === 1) {
                                    refreshVariableCacheAfterSave(element, commandStack, injector);
                                    dlg.close();
                                } else {
                                    const msg = (result && (result.Message || result.message)) || 'Save failed';
                                    if (window.kmsgbox) window.kmsgbox.error(msg); else alert(msg);
                                }
                            })
                            .catch(err => {
                                if (window.kmsgbox) window.kmsgbox.error(err && err.message ? err.message : 'Request failed'); else alert('Request failed');
                            });
                        return false;
                    } else {
                        clearVariablesExtension(element, commandStack);
                        dlg.close();
                        return true;
                    }
                }
            }
        ]
    });

    function renderDialogContent() {
        const html = `
            <div id="variable-dialog-container" style="padding: 16px; height: 100%; display: flex; flex-direction: column; overflow: hidden; box-sizing: border-box;">
                <!-- Input Variables Section -->
                <div id="input-variables-section" style="height: 250px; display: flex; flex-direction: column; margin-bottom: 12px; flex-shrink: 0; box-sizing: border-box;">
                    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 8px; flex-shrink: 0; height: 32px;">
                        <div style="font-weight: 600; font-size: 14px; color: #4b6cb7;">
                            <i class="fas fa-sign-in-alt"></i> Input Variables
                        </div>
                        <button type="button" id="btn-add-input-variable" class="btn btn-sm btn-primary">
                            <i class="fas fa-plus"></i> Add Input
                        </button>
                    </div>
                    <div style="overflow-x: auto; overflow-y: scroll; height: calc(100% - 32px); border: 1px solid #e2e8f0; border-radius: 4px; box-sizing: border-box;">
                        <table class="table table-bordered table-striped table-sm" style="margin-bottom: 0; min-width: 790px;">
                            <thead style="position: sticky; top: 0; background-color: #f1f5f9; z-index: 10;">
                                <tr>
                                    <th style="width: 160px;">Name</th>
                                    <th style="width: 120px;">Type</th>
                                    <th style="width: 90px;">Required</th>
                                    <th style="width: 100px;">Default</th>
                                    <th style="width: 200px;">Source</th>
                                    <th style="width: 120px;">Actions</th>
                                </tr>
                            </thead>
                            <tbody id="input-variables-grid-body"></tbody>
                        </table>
                    </div>
                </div>

                <!-- Output Variables Section -->
                <div id="output-variables-section" style="height: 250px; display: flex; flex-direction: column; flex-shrink: 0; box-sizing: border-box;">
                    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 8px; flex-shrink: 0; height: 32px;">
                        <div style="font-weight: 600; font-size: 14px; color: #4b6cb7;">
                            <i class="fas fa-sign-out-alt"></i> Output Variables
                        </div>
                        <button type="button" id="btn-add-output-variable" class="btn btn-sm btn-primary">
                            <i class="fas fa-plus"></i> Add Output
                        </button>
                    </div>
                    <div style="overflow-x: auto; overflow-y: scroll; height: calc(100% - 32px); border: 1px solid #e2e8f0; border-radius: 4px; box-sizing: border-box;">
                        <table class="table table-bordered table-striped table-sm" style="margin-bottom: 0; min-width: 590px;">
                            <thead style="position: sticky; top: 0; background-color: #f1f5f9; z-index: 10;">
                                <tr>
                                    <th style="width: 160px;">Name</th>
                                    <th style="width: 120px;">Type</th>
                                    <th style="width: 90px;">Required</th>
                                    <th style="width: 100px;">Default</th>
                                    <th style="width: 120px;">Actions</th>
                                </tr>
                            </thead>
                            <tbody id="output-variables-grid-body"></tbody>
                        </table>
                    </div>
                </div>
            </div>
        `;

        window.$('#variableDialogContent').html(html);

        window.$('#btn-add-input-variable').on('click', function () {
            // Check if upstream nodes have output variables
            const upstreamNodes = getUpstreamNodesWithOutputVariables(element, injector);
            const hasOutputVariables = upstreamNodes.some(node => node.outputVariables && node.outputVariables.length > 0);
            
            if (!hasOutputVariables && upstreamNodes.length > 0) {
                window.kmsgbox.warn('The upstream nodes do not have any output variables defined. You may not be able to reference variables from upstream nodes.');
            }
            
            variablesData = variablesData.map(v => ({ ...v, __edit: false }));
            variablesData.push({
                direction: 'Input',
                name: '',
                type: '',
                defaultValue: '',
                isRequired: false,
                sourceNodeId: '',
                sourceVariableName: '',
                __edit: true
            });
            renderGrid();
        });

        window.$('#btn-add-output-variable').on('click', function () {
            variablesData = variablesData.map(v => ({ ...v, __edit: false }));
            variablesData.push({
                direction: 'Output',
                name: '',
                type: '',
                defaultValue: '',
                isRequired: false,
                sourceNodeId: '',
                sourceVariableName: '',
                __edit: true
            });
            renderGrid();
        });

        renderGrid();
    }

    function renderGrid() {
        const $inputTbody = window.$('#input-variables-grid-body');
        const $outputTbody = window.$('#output-variables-grid-body');
        $inputTbody.empty();
        $outputTbody.empty();

        // Separate input and output variables
        const inputVariables = variablesData.filter(v => v.direction === 'Input');
        const outputVariables = variablesData.filter(v => v.direction === 'Output');

        // Render Input Variables
        if (inputVariables.length === 0) {
            $inputTbody.append('<tr><td colspan="6" style="text-align:center; color:#888;">No input variables defined</td></tr>');
        } else {
            inputVariables.forEach((item, localIdx) => {
                const idx = variablesData.indexOf(item);
                const requiredLabel = item.isRequired ? '<span class="badge bg-primary">Yes</span>' : '<span class="badge bg-secondary">No</span>';
                const sourceText = item.sourceVariableName ? `${item.sourceVariableName} @ ${item.sourceNodeId || 'N/A'}` : '—';

                if (item.__edit) {
                    const row = `
                        <tr data-index="${idx}" class="variable-row editing" style="height: 65px;">
                            <td>
                                <input type="text" class="form-control form-control-sm field-name" id="name-${idx}" value="${item.name || ''}" placeholder="Name" style="height: 38px;">
                            </td>
                            <td>
                                <select class="form-control form-control-sm field-type" id="type-${idx}" style="height: 38px;">
                                    <option value="">-- Select Type --</option>
                                    ${renderTypeOption('String', item.type)}
                                    ${renderTypeOption('Integer', item.type)}
                                    ${renderTypeOption('Double', item.type)}
                                    ${renderTypeOption('Boolean', item.type)}
                                    ${renderTypeOption('DateTime', item.type)}
                                    ${renderTypeOption('Object', item.type)}
                                </select>
                            </td>
                            <td style="text-align:center;">
                                <input type="checkbox" class="field-required" id="required-${idx}" ${item.isRequired ? 'checked' : ''} style="width: 18px; height: 18px;">
                            </td>
                            <td>
                                <input type="text" class="form-control form-control-sm field-default" id="default-${idx}" value="${item.defaultValue || ''}" placeholder="Default (optional)" style="height: 38px;">
                            </td>
                            <td>
                                <div style="display:flex; align-items:center; gap:6px;">
                                    <input type="text" class="form-control form-control-sm field-source-display" id="source-display-${idx}" value="${item.sourceVariableName ? `${item.sourceVariableName} (${item.type || ''})` : ''}" placeholder="Select source" readonly style="height: 38px;">
                                    <button type="button" class="btn btn-xs btn-primary btn-select-source" data-index="${idx}"><i class="fas fa-search"></i></button>
                                    <button type="button" class="btn btn-xs btn-secondary btn-clear-source" data-index="${idx}" ${item.sourceVariableName ? '' : 'style="display:none;"'}><i class="fas fa-times"></i></button>
                                </div>
                                <input type="hidden" class="field-source-node" id="source-node-${idx}" value="${item.sourceNodeId || ''}">
                                <input type="hidden" class="field-source-name" id="source-name-${idx}" value="${item.sourceVariableName || ''}">
                            </td>
                            <td>
                                <button class="btn btn-xs btn-link text-success save-variable" data-index="${idx}">Save</button>
                                <button class="btn btn-xs btn-link cancel-variable" data-index="${idx}">Cancel</button>
                                <button class="btn btn-xs btn-link text-danger delete-variable" data-index="${idx}">Delete</button>
                            </td>
                        </tr>
                    `;
                    $inputTbody.append(row);
                } else {
                    const requiredCheckbox = `<input type="checkbox" disabled ${item.isRequired ? 'checked' : ''} style="width: 18px; height: 18px;">`;
                    const row = `
                        <tr data-index="${idx}" class="variable-row" style="height: 65px;">
                            <td>${item.name || ''}</td>
                            <td>${item.type || ''}</td>
                            <td style="text-align:center;">${requiredCheckbox}</td>
                            <td>${item.defaultValue || ''}</td>
                            <td>${sourceText}</td>
                            <td>
                                <button class="btn btn-xs btn-link text-danger delete-variable" data-index="${idx}">Delete</button>
                            </td>
                        </tr>
                    `;
                    $inputTbody.append(row);
                }
            });
        }

        // Render Output Variables
        if (outputVariables.length === 0) {
            $outputTbody.append('<tr><td colspan="5" style="text-align:center; color:#888;">No output variables defined</td></tr>');
        } else {
            outputVariables.forEach((item, localIdx) => {
                const idx = variablesData.indexOf(item);

                if (item.__edit) {
                    const row = `
                        <tr data-index="${idx}" class="variable-row editing" style="height: 65px;">
                            <td>
                                <input type="text" class="form-control form-control-sm field-name" id="name-${idx}" value="${item.name || ''}" placeholder="Name" style="height: 38px;">
                            </td>
                            <td>
                                <select class="form-control form-control-sm field-type" id="type-${idx}" style="height: 38px;">
                                    <option value="">-- Select Type --</option>
                                    ${renderTypeOption('String', item.type)}
                                    ${renderTypeOption('Integer', item.type)}
                                    ${renderTypeOption('Double', item.type)}
                                    ${renderTypeOption('Boolean', item.type)}
                                    ${renderTypeOption('DateTime', item.type)}
                                    ${renderTypeOption('Object', item.type)}
                                </select>
                            </td>
                            <td style="text-align:center;">
                                <input type="checkbox" class="field-required" id="required-${idx}" ${item.isRequired ? 'checked' : ''} style="width: 18px; height: 18px;">
                            </td>
                            <td>
                                <input type="text" class="form-control form-control-sm field-default" id="default-${idx}" value="${item.defaultValue || ''}" placeholder="Default (optional)" style="height: 38px;">
                            </td>
                            <td>
                                <button class="btn btn-xs btn-link text-success save-variable" data-index="${idx}">Save</button>
                                <button class="btn btn-xs btn-link cancel-variable" data-index="${idx}">Cancel</button>
                                <button class="btn btn-xs btn-link text-danger delete-variable" data-index="${idx}">Delete</button>
                            </td>
                        </tr>
                    `;
                    $outputTbody.append(row);
                } else {
                    const requiredCheckbox = `<input type="checkbox" disabled ${item.isRequired ? 'checked' : ''} style="width: 18px; height: 18px;">`;
                    const row = `
                        <tr data-index="${idx}" class="variable-row" style="height: 65px;">
                            <td>${item.name || ''}</td>
                            <td>${item.type || ''}</td>
                            <td style="text-align:center;">${requiredCheckbox}</td>
                            <td>${item.defaultValue || ''}</td>
                            <td>
                                <button class="btn btn-xs btn-link text-danger delete-variable" data-index="${idx}">Delete</button>
                            </td>
                        </tr>
                    `;
                    $outputTbody.append(row);
                }
            });
        }

        // Event handlers for both tables
        const setupEventHandlers = function($tbody) {
            $tbody.find('.variable-row').on('click', function (evt) {
                const idx = Number(window.$(this).data('index'));
                if (Number.isNaN(idx)) return;
                // ignore clicks on buttons, inputs, selects, checkboxes to avoid double handling
                if (window.$(evt.target).closest('button, input, select, textarea').length) return;
                variablesData = variablesData.map((v, i) => ({ ...v, __edit: i === idx }));
                renderGrid();
            });

            $tbody.find('.delete-variable').on('click', function () {
                const index = Number(window.$(this).data('index'));
                variablesData.splice(index, 1);
                renderGrid();
            });

            $tbody.find('.cancel-variable').on('click', function () {
                variablesData = variablesData.map(v => ({ ...v, __edit: false }));
                renderGrid();
            });

            $tbody.find('.save-variable').on('click', function () {
                const idx = Number(window.$(this).data('index'));
                // Determine direction based on which table the row is in
                const isInputTable = window.$(this).closest('#input-variables-grid-body').length > 0;
                const formData = readRow(idx, isInputTable ? 'Input' : 'Output');
                if (!formData) return;
                variablesData[idx] = { ...formData, __edit: false };
                renderGrid();
            });

            $tbody.find('.btn-select-source').on('click', function () {
                const idx = Number(window.$(this).data('index'));
                showSourceSelectionDialog(element, injector, function (selection) {
                    window.$(`#source-display-${idx}`).val(`${selection.varName} (${selection.varType}) from "${selection.nodeName}"`);
                    window.$(`#source-node-${idx}`).val(selection.nodeId);
                    window.$(`#source-name-${idx}`).val(selection.varName);
                    // 引用前置变量时，自动把变量名称和类型填充到当前输入变量
                    window.$(`#name-${idx}`).val(selection.varName || '');
                    window.$(`#type-${idx}`).val(selection.varType || '');
                });
            });

            $tbody.find('.btn-clear-source').on('click', function () {
                const idx = Number(window.$(this).data('index'));
                window.$(`#source-display-${idx}`).val('');
                window.$(`#source-node-${idx}`).val('');
                window.$(`#source-name-${idx}`).val('');
                window.$(this).hide();
            });
        };

        setupEventHandlers($inputTbody);
        setupEventHandlers($outputTbody);
    }

    function readRow(idx, direction) {
        // Direction is determined by which table the row is in (input or output)
        if (!direction) {
            // Fallback: get direction from existing data if not provided
            const existingItem = variablesData[idx];
            direction = existingItem ? existingItem.direction : 'Input';
        }
        
        const name = window.$(`#name-${idx}`).val().trim();
        const type = window.$(`#type-${idx}`).val();
        const defaultValue = window.$(`#default-${idx}`).val().trim() || '';
        const isRequired = window.$(`#required-${idx}`).is(':checked');
        const sourceNodeId = direction === 'Input' ? (window.$(`#source-node-${idx}`).val() || '') : '';
        const sourceVariableName = direction === 'Input' ? (window.$(`#source-name-${idx}`).val() || '') : '';

        if (!name) {
            alert('Variable name is required');
            return null;
        }
        if (!type) {
            alert('Variable type is required');
            return null;
        }

        return {
            direction,
            name,
            type,
            defaultValue,
            isRequired,
            sourceNodeId: direction === 'Input' ? sourceNodeId : '',
            sourceVariableName: direction === 'Input' ? sourceVariableName : ''
        };
    }

    function renderTypeOption(type, current) {
        const selected = current === type ? 'selected' : '';
        return `<option value="${type}" ${selected}>${type}</option>`;
    }
}

function showSourceSelectionDialog(element, injector, onSelect) {
    const upstreamNodes = getUpstreamNodesWithOutputVariables(element, injector);
    
    if (upstreamNodes.length === 0) {
        window.kmsgbox.warn('No upstream nodes with output variables found.');
        return;
    }
    
    let dialogHtml = `
        <div style="padding: 20px; max-height: 500px; overflow-y: auto; min-width: 500px;">
            <div style="margin-bottom: 15px; padding-bottom: 10px; border-bottom: 1px solid #e0e0e0;">
                <strong style="color: #333; font-size: 14px;">Select Output Variable from Upstream Nodes</strong>
                <div style="font-size: 12px; color: #666; margin-top: 4px;">
                    Choose an output variable from upstream nodes to map to this input variable
                </div>
            </div>
    `;
    
    upstreamNodes.forEach((node, nodeIndex) => {
        dialogHtml += `
            <div style="margin-bottom: 20px; border: 1px solid #e0e0e0; border-radius: 4px; padding: 12px; background-color: #fafafa;">
                <div style="font-weight: 600; color: #333; margin-bottom: 10px; display: flex; align-items: center;">
                    <i class="fas fa-cube" style="margin-right: 8px; color: #4b6cb7;"></i>
                    <span>${node.nodeName}</span>
                    <span style="margin-left: 8px; font-size: 11px; color: #999; font-weight: normal;">(${node.nodeId})</span>
                </div>
                <div style="margin-left: 24px;">
        `;
        
        if (node.outputVariables.length === 0) {
            dialogHtml += `<div style="color: #999; font-style: italic; padding: 8px; background-color: #fff; border-radius: 4px;">No output variables defined</div>`;
        } else {
            node.outputVariables.forEach((variable, varIndex) => {
                const radioId = `source-${nodeIndex}-${varIndex}`;
                dialogHtml += `
                    <label style="display: flex; align-items: center; padding: 10px; cursor: pointer; border-radius: 4px; margin-bottom: 6px; background-color: #fff; border: 1px solid #e0e0e0; transition: all 0.2s;" 
                           onmouseover="this.style.backgroundColor='#e3f2fd'; this.style.borderColor='#4b6cb7';" 
                           onmouseout="this.style.backgroundColor='#fff'; this.style.borderColor='#e0e0e0';">
                        <input type="radio" name="selected-source" value="${node.nodeId}::${variable.name}::${variable.type}" 
                               id="${radioId}" 
                               data-node-id="${node.nodeId}" 
                               data-node-name="${node.nodeName}"
                               data-var-name="${variable.name}" 
                               data-var-type="${variable.type}"
                               style="margin-right: 10px; cursor: pointer;">
                        <div style="flex: 1;">
                            <div style="font-weight: 500; color: #333; margin-bottom: 2px;">${variable.name}</div>
                            <div style="font-size: 11px; color: #666;">
                                <span style="background-color: #e3f2fd; color: #1976d2; padding: 2px 6px; border-radius: 3px; font-weight: 500;">${variable.type}</span>
                            </div>
                        </div>
                    </label>
                `;
            });
        }
        
        dialogHtml += `
                </div>
            </div>
        `;
    });
    
    dialogHtml += `</div>`;
    
    window.BootstrapDialog.show({
        message: window.$(dialogHtml),
        title: 'Select Output Variable from Upstream Nodes',
        size: window.BootstrapDialog.SIZE_WIDE || 'size-wide',
        draggable: true,
        cssClass: 'source-selection-dialog',
        buttons: [
            {
                label: 'Cancel',
                action: function (dialog) {
                    dialog.close();
                }
            },
            {
                label: 'Select',
                cssClass: 'btn-primary',
                action: function (dialog) {
                    const selected = window.$('input[name="selected-source"]:checked');
                    if (selected.length === 0) {
                        alert('Please select an output variable');
                        return false;
                    }
                    
                    const nodeId = selected.data('node-id');
                    const nodeName = selected.data('node-name');
                    const varName = selected.data('var-name');
                    const varType = selected.data('var-type');
                    
                    // Populate selection back to dialog
                    if (typeof onSelect === 'function') {
                        onSelect({ nodeId, nodeName, varName, varType });
                    }
                    
                    dialog.close();
                    return true;
                }
            }
        ]
    });
}

function refreshVariableCacheAfterSave(element, commandStack, injector) {
    const processIdVersion = getProcessIdAndVersion(injector);
    const processId = processIdVersion.processId;
    const version = processIdVersion.version;
    const apiBase = (window.kconfig && window.kconfig.webApiUrl) || '';
    if (!apiBase || !processId || !element.id) {
        clearVariablesExtension(element, commandStack);
        return;
    }
    const url =
        apiBase +
        'api/WfVariable/GetList?processId=' +
        encodeURIComponent(processId) +
        '&version=' +
        encodeURIComponent(version) +
        '&activityId=' +
        encodeURIComponent(element.id);
    fetch(url, { method: 'GET', headers: { Accept: 'application/json' } })
        .then(function (r) {
            return r.json();
        })
        .then(function (result) {
            if (result.Status === 1 && result.Entity && Array.isArray(result.Entity)) {
                window.__wfVariableCache = window.__wfVariableCache || {};
                window.__wfVariableCache[element.id] = result.Entity;
            }
            clearVariablesExtension(element, commandStack);
            injector.get('eventBus').fire('elements.changed', { elements: [element] });
        })
        .catch(function () {
            clearVariablesExtension(element, commandStack);
        });
}

function removeFactory({ commandStack, element, variable, direction }) {
    return function (event) {
        event.stopPropagation();
        const extension = getVariablesExtension(element);
        if (!extension) {
            return;
        }

        const propertyName = direction === 'Input' ? 'inputVariables' : 'outputVariables';
        const currentVariables = extension.get(propertyName) || [];
        const updatedVariables = without(currentVariables, variable);

        commandStack.execute('element.updateModdleProperties', {
            element,
            moddleElement: extension,
            properties: {
                [propertyName]: updatedVariables
            }
        });
    };
}
