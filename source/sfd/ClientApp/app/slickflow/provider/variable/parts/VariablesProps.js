import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject, is } from 'bpmn-js/lib/util/ModelUtil';
import {
    createElement,
    createVariables,
    getVariables,
    getVariablesExtension
} from '../util';

import { without } from 'min-dash';

export default function VariablesProps({ element, injector }) {
    const variables = getVariables(element);
    const bpmnFactory = injector.get('bpmnFactory'),
        commandStack = injector.get('commandStack');

    // Merge all variables (input + output)
    // Get direction from variable's own direction attribute, not from array position
    const allVariables = [
        ...(variables.inputVariables || []).map(v => ({ 
            variable: v, 
            direction: v.get('direction') || 'Input' 
        })),
        ...(variables.outputVariables || []).map(v => ({ 
            variable: v, 
            direction: v.get('direction') || 'Output' 
        }))
    ];

    const items = allVariables.map(({ variable, direction }, index) => {
        // Get direction from variable's own direction attribute as the source of truth
        const variableDirection = variable.get('direction') || direction;
        const id = element.id + '-variable-' + variableDirection + '-' + index;
        const variableName = variable.get('name') || '';
        const variableType = variable.get('type') || '';
        
        // Read from new XML structure: varRefDetail sub-element
        let sourceNodeId = '';
        let sourceVariableName = '';
        const varRefDetail = variable.get('varRefDetail');
        if (varRefDetail && varRefDetail.length > 0) {
            const refDetail = varRefDetail[0];
            sourceNodeId = refDetail.get('sourceRef') || '';
            sourceVariableName = refDetail.get('variableName') || '';
        }
        
        const isRequired = !!variable.get('isRequired');
        
        // Add label by direction
        const directionLabel = variableDirection === 'Input' ? '[IN]' : '[OUT]';
        let displayLabel = `${directionLabel} ${variableName || `Variable ${index + 1}`}`;
        if (isRequired) {
            displayLabel += ' • required';
        }
        
        // For input variables with source, append source info
        if (variableDirection === 'Input' && sourceNodeId && sourceVariableName) {
            try {
                const elementRegistry = injector.get('elementRegistry');
                if (elementRegistry) {
                    const sourceNode = elementRegistry.get(sourceNodeId);
                    if (sourceNode) {
                        const sourceNodeName = getNodeName(sourceNode);
                        displayLabel += ` [${sourceNodeName}]`;
                    } else {
                        displayLabel += ` [${sourceNodeId}]`;
                    }
                } else {
                    displayLabel += ` [${sourceNodeId}]`;
                }
            } catch (e) {
                displayLabel += ` [${sourceNodeId}]`;
            }
        }
        
        return {
            id,
            label: displayLabel,
            autoFocusEntry: id + '-name',
            remove: removeFactory({ commandStack, element, variable, direction: variableDirection })
        };
    });

    return {
        items,
        add: addFactory({ element, injector, bpmnFactory, commandStack, initialVariables: allVariables })
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
            
            const nodeVariables = getVariables(node);
            // Get all variables (both input and output arrays) and filter by direction
            const allVars = [
                ...(nodeVariables.inputVariables || []),
                ...(nodeVariables.outputVariables || [])
            ];
            // Filter only variables with direction="output"
            const outputVars = allVars.filter(v => {
                const direction = v.get('direction');
                return direction === 'Output';
            });
            
            if (outputVars.length > 0) {
                const nodeName = getNodeName(node);
                upstreamNodes.push({
                    nodeId: node.id,
                    nodeName: nodeName,
                    outputVariables: outputVars.map(v => ({
                        name: v.get('name') || '',
                        type: v.get('type') || ''
                    }))
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

function addFactory({ element, injector, bpmnFactory, commandStack, initialVariables }) {
    return function (event) {
        event.stopPropagation();

        if (typeof window.BootstrapDialog === 'undefined') {
            console.error('BootstrapDialog is not available');
            return;
        }

        const existingVariables = (initialVariables || []).map(({ variable, direction }) => {
            // Read from new XML structure: varRefDetail sub-element
            let sourceNodeId = '';
            let sourceVariableName = '';
            const varRefDetail = variable.get('varRefDetail');
            if (varRefDetail && varRefDetail.length > 0) {
                const refDetail = varRefDetail[0];
                sourceNodeId = refDetail.get('sourceRef') || '';
                sourceVariableName = refDetail.get('variableName') || '';
            }
            
            // Get direction from variable's own direction attribute, not from array position
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

        openVariablesDialog({
            element,
            injector,
            bpmnFactory,
            commandStack,
            initialData: existingVariables
        });
    };
}

function openVariablesDialog({ element, injector, bpmnFactory, commandStack, initialData }) {
    let variablesData = initialData || [];

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
                    applyVariablesChanges(element, bpmnFactory, commandStack, variablesData);
                    dlg.close();
                    return true;
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

function applyVariablesChanges(element, bpmnFactory, commandStack, variablesData) {
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

    let extension = getVariablesExtension(element);

    if (!extension) {
        extension = createVariables({
            inputVariables: [],
            outputVariables: []
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

    const inputVariables = [];
    const outputVariables = [];

    variablesData.forEach(item => {
        // Check if variable is referenced: must be input direction and have both sourceNodeId and sourceVariableName
        const hasSource = item.sourceNodeId && item.sourceNodeId.trim() !== '' && 
                         item.sourceVariableName && item.sourceVariableName.trim() !== '';
        const isReferenced = item.direction === 'Input' && hasSource;
        
        const variableProps = {
            name: item.name,
            type: item.type,
            defaultValue: item.defaultValue || '',
            direction: item.direction,
            isReferenced: isReferenced,
            isRequired: !!item.isRequired
        };
        
        const moddleVar = createElement('sf:Variable', variableProps, extension, bpmnFactory);
        
        // Create and set varRefDetail after creating the variable
        if (isReferenced) {
            try {
                const varRefDetail = bpmnFactory.create('sf:varRefDetail', {
                    sourceRef: item.sourceNodeId.trim(),
                    variableName: item.sourceVariableName.trim()
                });
                // Set parent for the varRefDetail
                varRefDetail.$parent = moddleVar;
                // Set varRefDetail using the property setter
                moddleVar.set('varRefDetail', [varRefDetail]);
            } catch (e) {
                console.error('Error creating varRefDetail:', e);
                // If varRefDetail creation fails, still set isReferenced but without varRefDetail
                moddleVar.set('varRefDetail', []);
            }
        } else {
            // Ensure varRefDetail is empty if not referenced
            moddleVar.set('varRefDetail', []);
        }

        if (item.direction === 'Input') {
            inputVariables.push(moddleVar);
        } else {
            outputVariables.push(moddleVar);
        }
    });

    commands.push({
        cmd: 'element.updateModdleProperties',
        context: {
            element,
            moddleElement: extension,
            properties: {
                inputVariables,
                outputVariables
            }
        }
    });

    commandStack.execute('properties-panel.multi-command-executor', commands);
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
