import { TextAreaEntry, isTextFieldEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export default function (element) {
    var conditionProperty = 
        {
            id: 'condition',
            element,
            component: Condition,
            isEdited: isTextFieldEntryEdited
    };
    return conditionProperty;
}

function Condition(props) {
    const { element, id } = props;
    const moddle = useService('moddle');
    const modeling = useService('modeling');
    const bpmnFactory = useService('bpmnFactory');
    const translate = useService('translate');
    const debounce = useService('debounceInput');

    const getValue = () => {
        const businessObject = getBusinessObject(element);
        var condition = businessObject.conditionExpression;

        if (condition)
            return condition.body;
        else
            return '';
    }

    const setValue = value => {
        const expression = bpmnFactory.create("bpmn:Expression");
        expression.body = value;

        return modeling.updateProperties(element, {
            conditionExpression: expression
        });
    }

    const openExpressionEditor = () => {
        const currentValue = getValue();
        
        // Store current expression value and callback in global scope
        window.__expressionEditorCurrentValue = currentValue || '';
        window.__expressionEditorCallback = function(expression) {
            if (expression !== undefined && expression !== null) {
                setValue(expression);
            }
            delete window.__expressionEditorCurrentValue;
            delete window.__expressionEditorCallback;
        };

        // Use BootstrapDialog to open expression editor
        if (typeof window.BootstrapDialog !== 'undefined') {
            const dialog = window.BootstrapDialog.show({
                message: window.$('<div id="expressionEditorContent"></div>'),
                title: kresource.getItem('expression.editor.title') || 'Expression Editor',
                size: window.BootstrapDialog.SIZE_WIDE || 'size-wide',
                cssClass: 'expression-editor-dialog',
                onshown: function(dialogRef) {
                    const $content = window.$('#expressionEditorContent');
                    
                    // Clear previous content to avoid script redeclaration
                    $content.empty();
                    
                    $content.load('pages/expression/editor.html', function(response, status, xhr) {
                        if (status === "error") {
                            console.error("Failed to load expression editor page");
                            return;
                        }
                        
                        // Remove duplicate script tags if any (jQuery load may create duplicates)
                        const scripts = $content.find('script[src*="expression-editor.js"]');
                        if (scripts.length > 1) {
                            scripts.slice(1).remove();
                        }
                        
                        // Wait for expression editor to initialize
                        // jQuery's .load() automatically executes inline scripts, but external scripts may need time
                        var checkCount = 0;
                        var maxChecks = 50;
                        var checkInterval = setInterval(function() {
                            checkCount++;
                            if (typeof window.expressionEditor !== 'undefined' && 
                                typeof window.expressionEditor.init === 'function') {
                                
                                // Initialize with current expression value
                                if (window.__expressionEditorCurrentValue) {
                                    window.expressionEditor.setExpression(window.__expressionEditorCurrentValue);
                                } else {
                                    window.expressionEditor.init();
                                }
                                
                                clearInterval(checkInterval);
                            } else if (checkCount >= maxChecks) {
                                console.warn("Expression editor not loaded after timeout");
                                clearInterval(checkInterval);
                            }
                        }, 100);
                    });
                    
                    // Adjust dialog size
                    try {
                        const $dlg = dialogRef.getModalDialog();
                        $dlg.css({ 
                            width: '1000px', 
                            maxWidth: '90vw', 
                            height: '700px', 
                            maxHeight: '90vh' 
                        });
                        const $body = $dlg.find('.modal-body');
                        if ($body.length) {
                            $body.css({ 
                                height: 'calc(100% - 120px)', 
                                overflow: 'auto', 
                                padding: '0' 
                            });
                        }
                    } catch (e) {
                        console.warn("Failed to adjust dialog size:", e);
                    }
                },
                draggable: true,
                buttons: [
                    {
                        label: kresource.getItem('cancel') || 'Cancel',
                        action: function(dlg) {
                            delete window.__expressionEditorCurrentValue;
                            delete window.__expressionEditorCallback;
                            dlg.close();
                        }
                    },
                    {
                        label: kresource.getItem('expression.editor.clear') || 'Clear',
                        action: function(dlg) {
                            if (confirm(kresource.getItem('expression.editor.clear.confirm') || 'Are you sure you want to clear all expressions?')) {
                                if (typeof window.expressionEditor !== 'undefined' && 
                                    typeof window.expressionEditor.clearAll === 'function') {
                                    window.expressionEditor.clearAll();
                                }
                            }
                            return false; // Don't close dialog
                        }
                    },
                    {
                        label: kresource.getItem('expression.editor.ok') || 'OK',
                        cssClass: 'btn-primary',
                        action: function(dlg) {
                            // Get expression from editor
                            if (typeof window.expressionEditor !== 'undefined' && 
                                typeof window.expressionEditor.getExpression === 'function') {
                                const expression = window.expressionEditor.getExpression();
                                if (window.__expressionEditorCallback) {
                                    window.__expressionEditorCallback(expression);
                                }
                            }
                            delete window.__expressionEditorCurrentValue;
                            delete window.__expressionEditorCallback;
                            dlg.close();
                            return true;
                        }
                    }
                ]
            });
        } else {
            console.error('BootstrapDialog is not available');
        }
    };

    // Use a wrapper div with custom styling
    return (
        <div className="bio-properties-panel-entry">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '8px' }}>
                <label htmlFor={id} className="bio-properties-panel-label" style={{ margin: 0 }}>
                    {kresource.getItem('conditionexpression') || 'Condition Expression'}
                </label>
                <button
                    type="button"
                    className="btn btn-sm btn-primary"
                    title={kresource.getItem('expression.editor.open') || 'Open Expression Editor'}
                    onClick={openExpressionEditor}
                    style={{ 
                        whiteSpace: 'nowrap', 
                        flexShrink: 0,
                        padding: '4px 8px',
                        minWidth: '32px',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center'
                    }}
                >
                    <i className="fas fa-edit" style={{ fontSize: '14px' }}></i>
                </button>
            </div>
            <div style={{ width: '100%' }}>
                <TextAreaEntry
                    id={id}
                    element={element}
                    label=""
                    getValue={getValue}
                    setValue={setValue}
                    debounce={debounce}
                    disabled={true}
                />
            </div>
        </div>
    );
}

