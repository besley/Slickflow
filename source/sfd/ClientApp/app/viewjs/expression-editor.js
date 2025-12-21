// Avoid redeclaration when script is loaded multiple times
if (typeof window.expressionEditor === 'undefined') {
    window.expressionEditor = (function () {
    let currentExpression = '';
    let expressionGroups = [];
    let availableFields = []; // Will be populated from process variables

    function init() {
        // Load available fields from process variables (if available)
        loadAvailableFields();

        // Load existing expression if provided
        const urlParams = new URLSearchParams(window.location.search);
        const expression = urlParams.get('expression') || '';
        if (expression) {
            parseExpression(expression);
        } else {
            // Start with one empty group
            addExpressionGroup();
        }

        // Event handlers - use event delegation to handle dynamically loaded content
        // Remove any existing handlers first to avoid duplicates
        $(document).off('click', '#btn-add-group');
        $(document).on('click', '#btn-add-group', function (e) {
            e.preventDefault();
            e.stopPropagation();
            addExpressionGroup();
            return false;
        });
        
        // Also bind directly to the button if it exists (for immediate binding)
        $('#btn-add-group').off('click').on('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            addExpressionGroup();
            return false;
        });

        // Check if we're in BootstrapDialog mode
        const isInBootstrapDialog = typeof window.BootstrapDialog !== 'undefined' && 
                                     window.$('#expressionEditorContent').length > 0;

        if (!isInBootstrapDialog) {
            // Show footer buttons only if not in BootstrapDialog
            $('#editor-footer').show();
        }

        $('#btn-ok').on('click', function () {
            const expression = generateExpression();
            // Return expression via callback (for BootstrapDialog)
            if (window.__expressionEditorCallback) {
                window.__expressionEditorCallback(expression);
            }
            // Also support window.opener for backward compatibility
            if (window.opener && window.opener.expressionEditorCallback) {
                window.opener.expressionEditorCallback(expression);
            }
            // Close dialog if BootstrapDialog is available
            if (isInBootstrapDialog) {
                // The dialog will be closed by the button action handler in ConditionProps.js
                // Just trigger the callback
            } else {
                // Fallback to window.close() if not in BootstrapDialog
                window.close();
            }
        });

        $('#btn-cancel').on('click', function () {
            // Close dialog if BootstrapDialog is available
            if (isInBootstrapDialog) {
                // Find and close the current dialog
                const $dialog = window.$('#expressionEditorContent').closest('.modal');
                if ($dialog.length) {
                    window.BootstrapDialog.close($dialog);
                }
            } else {
                // Fallback to window.close() if not in BootstrapDialog
                window.close();
            }
        });

        $('#btn-clear').on('click', function () {
            if (confirm(kresource.getItem('expression.editor.clear.confirm') || 'Are you sure you want to clear all expressions?')) {
                expressionGroups = [];
                $('#expression-groups').empty();
                addExpressionGroup();
                updatePreview();
            }
        });

        // Update preview on any change
        $(document).on('change input', '.expression-item-field input, .expression-item-operator select, .expression-item-value input, .expression-group-operator select, .expression-item-logic select', function () {
            updatePreview();
        });

        // Ensure the add group button is bound (in case it's loaded dynamically)
        // Use a small delay to ensure DOM is ready
        setTimeout(function() {
            $('#btn-add-group').off('click').on('click', function () {
                addExpressionGroup();
            });
        }, 100);

        updatePreview();
    }

    function loadAvailableFields() {
        // Try to get process variables from parent window or global context
        if (window.opener && window.opener.getProcessVariables) {
            availableFields = window.opener.getProcessVariables() || [];
        } else if (typeof kmain !== 'undefined' && kmain.getProcessVariables) {
            availableFields = kmain.getProcessVariables() || [];
        } else {
            // Default fields
            availableFields = [
                { name: 'amount', label: 'Amount' },
                { name: 'status', label: 'Status' },
                { name: 'priority', label: 'Priority' }
            ];
        }
    }

    function addExpressionGroup() {
        const groupId = 'group-' + Date.now() + '-' + Math.random().toString(36).substr(2, 9);
        const group = {
            id: groupId,
            operator: 'AND', // AND/OR between groups
            items: []
        };
        expressionGroups.push(group);

        renderExpressionGroup(group);
    }

    function renderExpressionGroup(group) {
        const groupHtml = `
            <div class="expression-group" data-group-id="${group.id}">
                <div class="expression-group-header">
                    <span class="expression-group-title">${kresource.getItem('expression.editor.group') || 'Group'} ${expressionGroups.indexOf(group) + 1}</span>
                    <div class="expression-group-operator">
                        <label>${kresource.getItem('expression.editor.group.operator') || 'Group Operator'}:</label>
                        <select class="group-operator-select">
                            <option value="AND" ${group.operator === 'AND' ? 'selected' : ''}>AND</option>
                            <option value="OR" ${group.operator === 'OR' ? 'selected' : ''}>OR</option>
                        </select>
                    </div>
                    <button type="button" class="expression-group-remove" data-group-id="${group.id}">
                        <i class="fas fa-times"></i> ${kresource.getItem('delete') || 'Delete'}
                    </button>
                </div>
                <div class="expression-items" data-group-id="${group.id}">
                    ${group.items.map((item, index) => renderExpressionItem(group.id, item, index)).join('')}
                </div>
                <div class="add-item-container">
                    <button type="button" class="btn btn-add-item" data-group-id="${group.id}">
                        <i class="fas fa-plus"></i> ${kresource.getItem('expression.editor.add.condition') || 'Add Condition'}
                    </button>
                </div>
            </div>
        `;

        $('#expression-groups').append(groupHtml);

        // Event handlers for this group
        $(`.expression-group[data-group-id="${group.id}"] .btn-add-item`).on('click', function () {
            addExpressionItem(group.id);
        });

        $(`.expression-group[data-group-id="${group.id}"] .expression-group-remove`).on('click', function () {
            removeExpressionGroup(group.id);
        });

        $(`.expression-group[data-group-id="${group.id}"] .group-operator-select`).on('change', function () {
            group.operator = $(this).val();
            updatePreview();
        });

        // Attach event handlers for existing items
        group.items.forEach((item, index) => {
            attachItemEventHandlers(group.id, index);
        });
    }

    function addExpressionItem(groupId) {
        const group = expressionGroups.find(g => g.id === groupId);
        if (!group) return;

        const item = {
            field: '',
            operator: '==',
            value: '',
            logic: group.items.length > 0 ? 'AND' : '' // AND/OR between items in same group
        };
        group.items.push(item);

        const itemsContainer = $(`.expression-items[data-group-id="${groupId}"]`);
        itemsContainer.append(renderExpressionItem(groupId, item, group.items.length - 1));

        // Attach event handlers for the new item
        attachItemEventHandlers(groupId, group.items.length - 1);
        updatePreview();
    }

    function renderExpressionItem(groupId, item, index) {
        const isFirst = index === 0;
        const logicSelect = isFirst ? '' : `
            <div class="expression-item-logic">
                <select class="item-logic-select">
                    <option value="AND" ${item.logic === 'AND' ? 'selected' : ''}>AND</option>
                    <option value="OR" ${item.logic === 'OR' ? 'selected' : ''}>OR</option>
                </select>
            </div>
        `;

        return `
            <div class="expression-item" data-group-id="${groupId}" data-item-index="${index}">
                ${logicSelect}
                <span class="expression-item-left-bracket">(</span>
                <div class="expression-item-field">
                    <input type="text" class="item-field-input" placeholder="${kresource.getItem('expression.editor.field') || 'Field'}" value="${item.field || ''}" />
                </div>
                <div class="expression-item-operator">
                    <select class="item-operator-select">
                        <option value="==" ${item.operator === '==' ? 'selected' : ''}>==</option>
                        <option value="!=" ${item.operator === '!=' ? 'selected' : ''}>!=</option>
                        <option value=">" ${item.operator === '>' ? 'selected' : ''}>&gt;</option>
                        <option value=">=" ${item.operator === '>=' ? 'selected' : ''}>&gt;=</option>
                        <option value="<" ${item.operator === '<' ? 'selected' : ''}>&lt;</option>
                        <option value="<=" ${item.operator === '<=' ? 'selected' : ''}>&lt;=</option>
                        <option value="contains" ${item.operator === 'contains' ? 'selected' : ''}>contains</option>
                        <option value="startsWith" ${item.operator === 'startsWith' ? 'selected' : ''}>startsWith</option>
                        <option value="endsWith" ${item.operator === 'endsWith' ? 'selected' : ''}>endsWith</option>
                    </select>
                </div>
                <div class="expression-item-value">
                    <input type="text" class="item-value-input" placeholder="${kresource.getItem('expression.editor.value') || 'Value'}" value="${item.value || ''}" />
                </div>
                <span class="expression-item-right-bracket">)</span>
                <button type="button" class="expression-item-remove" data-group-id="${groupId}" data-item-index="${index}">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        `;
    }

    function attachItemEventHandlers(groupId, itemIndex) {
        const group = expressionGroups.find(g => g.id === groupId);
        if (!group || !group.items[itemIndex]) return;

        const item = group.items[itemIndex];
        const selector = `.expression-item[data-group-id="${groupId}"][data-item-index="${itemIndex}"]`;

        $(selector + ' .item-field-input').on('input', function () {
            item.field = $(this).val();
            updatePreview();
        });

        $(selector + ' .item-operator-select').on('change', function () {
            item.operator = $(this).val();
            updatePreview();
        });

        $(selector + ' .item-value-input').on('input', function () {
            item.value = $(this).val();
            updatePreview();
        });

        $(selector + ' .item-logic-select').on('change', function () {
            item.logic = $(this).val();
            updatePreview();
        });

        $(selector + ' .expression-item-remove').on('click', function () {
            removeExpressionItem(groupId, itemIndex);
        });
    }

    function removeExpressionItem(groupId, itemIndex) {
        const group = expressionGroups.find(g => g.id === groupId);
        if (!group) return;

        group.items.splice(itemIndex, 1);
        renderExpressionGroupItems(group);
        updatePreview();
    }

    function removeExpressionGroup(groupId) {
        const index = expressionGroups.findIndex(g => g.id === groupId);
        if (index === -1) return;

        if (confirm(kresource.getItem('expression.editor.remove.group.confirm') || 'Are you sure you want to remove this group?')) {
            expressionGroups.splice(index, 1);
            $(`.expression-group[data-group-id="${groupId}"]`).remove();
            updatePreview();
        }
    }

    function renderExpressionGroupItems(group) {
        const itemsContainer = $(`.expression-items[data-group-id="${group.id}"]`);
        itemsContainer.empty();
        group.items.forEach((item, index) => {
            itemsContainer.append(renderExpressionItem(group.id, item, index));
            attachItemEventHandlers(group.id, index);
        });
    }

    function generateExpression() {
        if (expressionGroups.length === 0) return '';

        const groupExpressions = expressionGroups.map(group => {
            if (group.items.length === 0) return '';

            const itemExpressions = group.items.map((item, index) => {
                if (!item.field || item.value === '') return '';

                let expression = '';
                if (index > 0 && item.logic) {
                    expression += ' ' + item.logic + ' ';
                }

                // Build condition: field operator value
                const field = item.field.trim();
                const operator = item.operator;
                const value = item.value.trim();

                // Handle different operators
                switch (operator) {
                    case '==':
                        expression += `${field} == ${formatValue(value)}`;
                        break;
                    case '!=':
                        expression += `${field} != ${formatValue(value)}`;
                        break;
                    case '>':
                        expression += `${field} > ${formatValue(value)}`;
                        break;
                    case '>=':
                        expression += `${field} >= ${formatValue(value)}`;
                        break;
                    case '<':
                        expression += `${field} < ${formatValue(value)}`;
                        break;
                    case '<=':
                        expression += `${field} <= ${formatValue(value)}`;
                        break;
                    case 'contains':
                        expression += `${field}.contains(${formatValue(value)})`;
                        break;
                    case 'startsWith':
                        expression += `${field}.startsWith(${formatValue(value)})`;
                        break;
                    case 'endsWith':
                        expression += `${field}.endsWith(${formatValue(value)})`;
                        break;
                    default:
                        expression += `${field} == ${formatValue(value)}`;
                }

                return expression;
            }).filter(expr => expr !== '');

            return itemExpressions.join('');
        }).filter(expr => expr !== '');

        // Combine groups with group operators
        let finalExpression = '';
        groupExpressions.forEach((groupExpr, index) => {
            if (index > 0 && expressionGroups[index].operator) {
                finalExpression += ' ' + expressionGroups[index].operator + ' ';
            }
            finalExpression += '(' + groupExpr + ')';
        });

        return finalExpression.trim();
    }

    function formatValue(value) {
        // Try to determine if value is a number
        if (!isNaN(value) && value.trim() !== '') {
            return value;
        }
        // Otherwise, treat as string
        return `"${value.replace(/"/g, '\\"')}"`;
    }

    function updatePreview() {
        const expression = generateExpression();
        const previewElement = $('#expression-preview');
        
        if (expression) {
            previewElement.text(expression).removeClass('empty');
        } else {
            previewElement.text(kresource.getItem('expression.editor.preview.empty') || '(No expression)').addClass('empty');
        }
    }

    function parseExpression(expression) {
        // Simple parser - can be enhanced for more complex expressions
        expressionGroups = [];
        $('#expression-groups').empty();
        
        // If expression is simple, try to parse it
        if (expression && expression.trim()) {
            const group = {
                id: 'group-parsed-' + Date.now(),
                operator: 'AND',
                items: []
            };
            
            // Pattern 1: Method calls like field.contains(value), field.startsWith(value), field.endsWith(value)
            const methodPattern = /(\w+)\.(contains|startsWith|endsWith)\s*\(\s*([^)]+)\s*\)/g;
            let match;
            const matchedPositions = new Set();
            
            // First, try to match method calls
            while ((match = methodPattern.exec(expression)) !== null) {
                const field = match[1].trim();
                const method = match[2].trim();
                let value = match[3].trim();
                
                // Remove quotes if present
                value = value.replace(/^["']|["']$/g, '');
                
                group.items.push({
                    field: field,
                    operator: method, // contains, startsWith, endsWith
                    value: value,
                    logic: group.items.length > 0 ? 'AND' : ''
                });
                
                // Mark this position as matched
                matchedPositions.add(match.index);
            }
            
            // Pattern 2: Comparison operators (>=, <=, ==, !=, >, <)
            // CRITICAL: multi-character operators (>=, <=, ==, !=) MUST come before single-character operators (>, <)
            // to avoid matching '>' when '>=' is intended, or '<' when '<=' is intended
            // This regex matches: field operator value, where value can be a number, quoted string, or unquoted string
            const comparisonPattern = /(\w+)\s*(>=|<=|==|!=|>|<)\s*([^\s&|()]+(?:\s+[^\s&|()]+)*?)(?=\s*(?:AND|OR|$|\)|&|\||\s*(?:>=|<=|==|!=|>|<)))/g;
            
            while ((match = comparisonPattern.exec(expression)) !== null) {
                // Skip if this position was already matched as a method call
                if (matchedPositions.has(match.index)) {
                    continue;
                }
                
                const field = match[1].trim();
                const operator = match[2].trim();
                let value = match[3].trim();
                
                // Remove quotes if present (both single and double quotes)
                value = value.replace(/^["']|["']$/g, '');
                // Remove trailing operators/logic words that might have been captured
                value = value.replace(/\s*(AND|OR|&|\|).*$/i, '').trim();
                // Remove any trailing comparison operators that might have been captured
                value = value.replace(/\s*(>=|<=|==|!=|>|<).*$/, '').trim();
                
                // Only add if value is not empty
                if (value) {
                    group.items.push({
                        field: field,
                        operator: operator,
                        value: value,
                        logic: group.items.length > 0 ? 'AND' : ''
                    });
                }
            }
            
            // If we found items, use them; otherwise start with empty group
            if (group.items.length > 0) {
                expressionGroups.push(group);
                renderExpressionGroup(group);
            } else {
                addExpressionGroup();
            }
        } else {
            addExpressionGroup();
        }
        
        updatePreview();
    }

    function clearAll() {
        expressionGroups = [];
        $('#expression-groups').empty();
        addExpressionGroup();
        updatePreview();
    }

    // Public API
    return {
        init: init,
        getExpression: generateExpression,
        setExpression: function(expr) {
            currentExpression = expr;
            parseExpression(expr);
        },
        clearAll: clearAll,
        addExpressionGroup: addExpressionGroup
    };
    })();
}
