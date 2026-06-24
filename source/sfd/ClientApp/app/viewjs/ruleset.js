import rulesetapi from './rulesetapi.js'

const ruleset = (function () {
    function ruleset() { }

    ruleset.current = null;
    ruleset.editor = null;
    ruleset.gridApi = null;
    ruleset.gridData = [];
    ruleset.templates = {
        ruleTypes: '{\n  "ruleTypes": [\n    "Slickflow.Module.BusinessRule.Approval.LeaveApprovalRule, Slickflow.Module.BusinessRule"\n  ]\n}\n',
        bindingsJson: '{\n  "rules": [\n    {\n      "when": "Amount >= 1000",\n      "set": { "ApprovalLevel": "2", "Route": "Manager" }\n    },\n    {\n      "when": "Amount < 1000",\n      "set": { "ApprovalLevel": "1", "Route": "Self" }\n    }\n  ]\n}\n'
    };

    ruleset.init = function () {
        if (typeof CodeMirror !== 'undefined' && !ruleset.editor) {
            const ta = document.getElementById('txtRuleSetContent');
            if (ta) {
                ruleset.editor = CodeMirror.fromTextArea(ta, {
                    mode: 'application/json',
                    theme: 'monokai',
                    lineNumbers: true,
                    indentUnit: 2,
                    indentWithTabs: false,
                    smartIndent: true,
                    lineWrapping: true,
                    matchBrackets: true,
                    autoCloseBrackets: true
                });

                setTimeout(function () {
                    if (ruleset.editor) {
                        ruleset.editor.refresh();
                    }
                }, 100);
            }
        }
        ruleset.refresh();
        initSplitter();
        if (!ruleset.current) {
            ruleset.createNew();
        }
    }

    ruleset.refresh = function () {
        rulesetapi.getList(function (result) {
            if (result.Status === 1) {
                renderList(result.Entity || []);
                if (!ruleset.current && (result.Entity || []).length > 0) {
                    ruleset.loadByCode(result.Entity[0].RuleSetCode || result.Entity[0].ruleSetCode);
                } else if ((result.Entity || []).length === 0) {
                    ruleset.createNew();
                }
            } else {
                kmsgbox.error(result.Message || 'Load failed');
            }
        });
    }

    function renderList(list) {
        ruleset.gridData = (list || []);
        const divRuleSetGrid = document.querySelector('#myRuleSetGrid');
        if (!divRuleSetGrid) return;
        $(divRuleSetGrid).empty();

        const gridOptions = {
            theme: sfGetGridTheme(),
            columnDefs: [
                { headerName: 'Code', field: 'RuleSetCode', width: 170 },
                { headerName: 'Name', field: 'RuleSetName', width: 180 },
                { headerName: 'Mode', field: 'Mode', width: 120 }
            ],
            rowSelection: {
                mode: 'singleRow',
                checkboxes: false,
                enableClickSelection: true
            },
            onSelectionChanged: function () {
                const selectedRows = ruleset.gridApi.getSelectedRows();
                if (selectedRows && selectedRows.length > 0) {
                    const row = selectedRows[0];
                    const code = row.RuleSetCode || row.ruleSetCode;
                    ruleset.loadByCode(code);
                }
            },
            onRowDoubleClicked: function () { }
        };

        gridOptions.rowData = ruleset.gridData;
        ruleset.gridApi = createGrid(divRuleSetGrid, gridOptions);
        fitGridColumns();
    }

    function fitGridColumns() {
        if (!ruleset.gridApi) return;
        setTimeout(function () {
            if (!ruleset.gridApi) return;
            if (typeof ruleset.gridApi.sizeColumnsToFit === 'function') {
                ruleset.gridApi.sizeColumnsToFit();
            } else if (ruleset.gridApi.api && typeof ruleset.gridApi.api.sizeColumnsToFit === 'function') {
                ruleset.gridApi.api.sizeColumnsToFit();
            }
        }, 0);
    }

    function initSplitter() {
        const container = document.querySelector('.ruleset-container');
        const left = document.querySelector('.ruleset-list');
        const splitter = document.querySelector('#rulesetSplitter');
        if (!container || !left || !splitter) return;
        if (splitter.dataset.inited === '1') return;
        splitter.dataset.inited = '1';

        let dragging = false;
        splitter.addEventListener('pointerdown', function (e) {
            dragging = true;
            document.body.style.userSelect = 'none';
            splitter.setPointerCapture(e.pointerId);
        });
        splitter.addEventListener('pointerup', function (e) {
            dragging = false;
            document.body.style.userSelect = '';
            splitter.releasePointerCapture(e.pointerId);
            fitGridColumns();
        });
        splitter.addEventListener('pointermove', function (e) {
            if (!dragging) return;
            const rect = container.getBoundingClientRect();
            const splitterWidth = splitter.getBoundingClientRect().width || 8;
            let leftWidth = e.clientX - rect.left;
            const minLeft = 220;
            const maxLeft = rect.width - 260 - splitterWidth;
            if (leftWidth < minLeft) leftWidth = minLeft;
            if (leftWidth > maxLeft) leftWidth = maxLeft;
            left.style.flex = '0 0 ' + leftWidth + 'px';
            fitGridColumns();
        });

        if (container.dataset.resizeBinded !== '1') {
            container.dataset.resizeBinded = '1';
            window.addEventListener('resize', fitGridColumns);
        }
    }

    ruleset.createNew = function () {
        ruleset.current = null;
        $("#txtRuleSetCode").val('').prop('disabled', false);
        $("#txtRuleSetName").val('');
        $("#txtRuleSetDesc").val('');
        $("#ddlRuleSetMode").val('ruleTypes');
        setContent(ruleset.templates.ruleTypes);
    }

    ruleset.loadByCode = function (ruleSetCode) {
        if (!ruleSetCode) return;
        rulesetapi.get(ruleSetCode, function (result) {
            if (result.Status === 1) {
                ruleset.current = result.Entity;
                const e = result.Entity || {};
                $("#txtRuleSetCode").val(e.RuleSetCode || e.ruleSetCode || '').prop('disabled', true);
                $("#txtRuleSetName").val(e.RuleSetName || e.ruleSetName || '');
                $("#txtRuleSetDesc").val(e.Description || e.description || '');
                $("#ddlRuleSetMode").val((e.Mode || e.mode || 'ruleTypes'));
                var modeKey = ($("#ddlRuleSetMode").val() || 'ruleTypes');
                var tpl = ruleset.templates[modeKey] || ruleset.templates.ruleTypes;
                setContent((e.RuleContent || e.ruleContent || '').trim() ? (e.RuleContent || e.ruleContent || '') : tpl);
            } else {
                kmsgbox.error(result.Message || 'Load failed');
            }
        });
    }

    function getContent() {
        if (ruleset.editor) return ruleset.editor.getValue();
        return $("#txtRuleSetContent").val() || '';
    }

    function setContent(val) {
        if (ruleset.editor) {
            ruleset.editor.setValue(val || '');
        } else {
            $("#txtRuleSetContent").val(val || '');
        }
    }

    ruleset.save = function () {
        const code = ($("#txtRuleSetCode").val() || '').trim();
        const name = ($("#txtRuleSetName").val() || '').trim();
        const mode = ($("#ddlRuleSetMode").val() || 'ruleTypes').trim();
        if (!code) { kmsgbox.warn('Code is required'); return; }
        if (!name) { kmsgbox.warn('Name is required'); return; }
        const entity = {
            RuleSetCode: code,
            RuleSetName: name,
            Description: ($("#txtRuleSetDesc").val() || '').trim(),
            Mode: mode,
            RuleContent: getContent(),
            IsEnabled: 1
        };

        rulesetapi.save(entity, function (result) {
            if (result.Status === 1) {
                kmsgbox.info(kresource.getItem("save"));
                ruleset.refresh();
                ruleset.loadByCode(code);
            } else {
                kmsgbox.error(result.Message || 'Save failed');
            }
        });
    }

    ruleset.showExampleHelp = function () {
        const mode = ($("#ddlRuleSetMode").val() || 'ruleTypes');
        const sample = (ruleset.templates[mode] || ruleset.templates.ruleTypes);
        const tip = mode === 'bindingsJson'
            ? 'bindingsJson mode: rules are defined in rule_content (JSON when/set). RuleTask supplies process/activity variables by name at runtime.'
            : 'ruleTypes mode: rule_content lists NRules rule types (assembly-qualified names). RuleTask supplies process/activity variables by name at runtime.';
        if (typeof BootstrapDialog !== 'undefined') {
            BootstrapDialog.show({
                title: 'Rule Set Help',
                message: $('<div style="font-size:12px;line-height:1.6;"><div style="margin-bottom:8px;">'
                    + tip
                    + '</div><pre style="background:#111827;color:#e5e7eb;padding:10px;border-radius:4px;max-height:300px;overflow:auto;">'
                    + sample.replace(/</g, '&lt;').replace(/>/g, '&gt;')
                    + '</pre></div>'),
                draggable: true
            });
        } else {
            window.alert(tip + '\n\n' + sample);
        }
    }

    ruleset.deleteCurrent = function () {
        const code = ($("#txtRuleSetCode").val() || '').trim();
        if (!code) return;
        kmsgbox.confirm(kresource.getItem("delete") + "?", function () {
            rulesetapi.delete(code, function (result) {
                if (result.Status === 1) {
                    ruleset.createNew();
                    ruleset.refresh();
                } else {
                    kmsgbox.error(result.Message || 'Delete failed');
                }
            });
        });
    }

    return ruleset;
})()

window.ruleset = ruleset;
export default ruleset;

