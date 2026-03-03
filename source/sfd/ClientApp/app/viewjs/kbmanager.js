import kconfig from '../config/kconfig.js'
import jshelper from '../script/jshelper.js'

const kbmanager = (function () {
    function kbmanager() {
    }

    kbmanager.pselectedDocument = null;
    kbmanager.kbGrid = null;
    kbmanager.isResizing = false;
    kbmanager.startX = 0;
    kbmanager.leftWidth = 40;

    kbmanager.init = function () {
        kbmanager.initDivider();
        kbmanager.initGrid();
        kbmanager.getDocumentList();
        // Clear form initially
        $('#kbQuestion').val('');
        $('#kbIntent').val('');
        $('#kbAnswer').val('');
        $('#kbMetadataTextarea').val('');
    };

    kbmanager.initGrid = function () {
        var divGrid = document.querySelector('#kbDocumentGrid');
        if (!divGrid) return;
        
        // Remove loading overlay temporarily before clearing
        var loadingOverlay = divGrid.querySelector('#kbLoadingOverlay');
        if (loadingOverlay) {
            loadingOverlay.remove();
        }
        
        $(divGrid).empty();

        var gridOptions = {
            theme: window.themeBalham,
            columnDefs: [
                { headerName: 'Id', field: 'Id', width: 80, resizable: true },
                { headerName: 'Content', field: 'Content', width: 500, resizable: true, minWidth: 300 }
            ],
            rowSelection: {
                mode: 'singleRow',
                checkboxes: false,
                enableClickSelection: true
            },
            onSelectionChanged: kbmanager.onSelectionChanged,
            onRowDoubleClicked: kbmanager.onRowDoubleClicked,
            suppressHorizontalScroll: false, // 允许水平滚动
            alwaysShowHorizontalScroll: false // 只在需要时显示滚动条
        };

        kbmanager.kbGrid = window.createGrid(divGrid, gridOptions);
        window.kbGrid = kbmanager.kbGrid;
        // Set empty data initially to show headers
        kbmanager.kbGrid.setGridOption('rowData', []);
        
        // Re-append loading overlay after grid is created
        if (loadingOverlay) {
            divGrid.appendChild(loadingOverlay);
        }
    };

    kbmanager.initDivider = function () {
        const divider = document.getElementById('kbDivider');
        const container = document.querySelector('.kb-container');
        const leftPanel = document.querySelector('.kb-left-panel');
        const rightPanel = document.querySelector('.kb-right-panel');

        divider.addEventListener('mousedown', function (e) {
            kbmanager.isResizing = true;
            kbmanager.startX = e.clientX;
            kbmanager.leftWidth = (leftPanel.offsetWidth / container.offsetWidth) * 100;
            document.addEventListener('mousemove', kbmanager.handleMouseMove);
            document.addEventListener('mouseup', kbmanager.handleMouseUp);
            e.preventDefault();
        });

        kbmanager.handleMouseMove = function (e) {
            if (!kbmanager.isResizing) return;
            const container = document.querySelector('.kb-container');
            const deltaX = e.clientX - kbmanager.startX;
            const containerWidth = container.offsetWidth;
            const newLeftWidth = ((kbmanager.leftWidth / 100 * containerWidth) + deltaX) / containerWidth * 100;
            
            if (newLeftWidth >= 20 && newLeftWidth <= 80) {
                leftPanel.style.width = newLeftWidth + '%';
                rightPanel.style.width = (100 - newLeftWidth) + '%';
            }
        };

        kbmanager.handleMouseUp = function () {
            kbmanager.isResizing = false;
            document.removeEventListener('mousemove', kbmanager.handleMouseMove);
            document.removeEventListener('mouseup', kbmanager.handleMouseUp);
        };
    };

    kbmanager.showLoading = function () {
        var divGrid = document.querySelector('#kbDocumentGrid');
        if (!divGrid) return;
        
        var overlay = document.getElementById('kbLoadingOverlay');
        if (!overlay) {
            // Create overlay if it doesn't exist
            overlay = document.createElement('div');
            overlay.id = 'kbLoadingOverlay';
            overlay.className = 'kb-loading-overlay';
            overlay.innerHTML = '<div class="kb-loading-progress"><div class="kb-loading-progress-bar"></div></div>';
            divGrid.appendChild(overlay);
        }
        
        // Force display
        overlay.style.display = 'flex';
        overlay.classList.add('show');
        overlay.style.zIndex = '9999';
    };

    kbmanager.hideLoading = function () {
        var overlay = document.getElementById('kbLoadingOverlay');
        if (overlay) {
            overlay.classList.remove('show');
            overlay.style.display = 'none';
        }
    };

    kbmanager.getDocumentList = function () {
        kbmanager.showLoading();
        var url = kconfig.webApiUrl + 'api/Wf2AI/GetKnowledgeBaseDocuments';
        jshelper.ajaxGet(url, null, function (result) {
            kbmanager.hideLoading();
            if (result && result.Status === 1) {
                if (kbmanager.kbGrid) {
                    kbmanager.kbGrid.setGridOption('rowData', result.Entity || []);
                } else {
                    kbmanager.initGrid();
                    kbmanager.kbGrid.setGridOption('rowData', result.Entity || []);
                }
            } else {
                if (window.kmsgbox) {
                    window.kmsgbox.error('Failed to load documents', result ? result.Message : 'Unknown error');
                }
            }
        });
    };

    kbmanager.onSelectionChanged = function (event) {
        var selectedRows = event.api.getSelectedRows();
        if (selectedRows && selectedRows.length > 0) {
            kbmanager.fillForm(selectedRows[0]);
        }
    };

    kbmanager.onRowDoubleClicked = function (event) {
        kbmanager.fillForm(event.data);
    };

    kbmanager.fillForm = function (entity) {
        kbmanager.pselectedDocument = entity;
                
        // Parse Content field (JSON format)
        var contentObj = {};
        var question = '';
        var intent = '';
        var answer = '';
        
        try {
            if (entity.Content) {               
                // If Content is already an object, use it directly
                if (typeof entity.Content === 'object' && entity.Content !== null) {
                    contentObj = entity.Content;
                } else if (typeof entity.Content === 'string') {
                    // Try to parse as JSON string
                    var contentStr = entity.Content.trim();
                    
                    // Check if it looks like JSON (starts with { or [)
                    if (contentStr.startsWith('{') || contentStr.startsWith('[')) {
                        try {
                            contentObj = JSON.parse(contentStr);
                        } catch (parseError) {
                            // If JSON parse fails, try to extract values from the string
                            contentObj = {};
                        }
                    } else {
                        // Not JSON format, try to parse special format: =Question;Question2 | Intent: xxx | Answer: xxx
                        contentObj = kbmanager.parseContentString(contentStr);
                    }
                }
            } 
        } catch (e) {
            console.error('Error processing Content field:', e);
            contentObj = {};
        }

        // Extract Question, Intent, Answer from content object
        // Support both PascalCase and camelCase, and also check for nested structures
        if (contentObj && Object.keys(contentObj).length > 0) {           
            // Try multiple field name variations
            question = contentObj.Question || contentObj.question || contentObj.QuestionText || contentObj['Question'] || '';
            intent = contentObj.Intent || contentObj.intent || contentObj.IntentText || contentObj['Intent'] || '';
            answer = contentObj.Answer || contentObj.answer || contentObj.AnswerText || contentObj['Answer'] || '';
                        
            // If still empty and contentObj has text field, try to parse it
            if (!question && !intent && !answer && contentObj.text) {
                // Try to parse textContent using the same parseContentString method
                var textContent = contentObj.text;
                if (typeof textContent === 'string') {
                    var parsedText = kbmanager.parseContentString(textContent);
                    question = parsedText.Question || '';
                    intent = parsedText.Intent || '';
                    answer = parsedText.Answer || '';
                }
            }
        } 

        // Set values to input fields
        $('#kbQuestion').val(question || '');
        $('#kbIntent').val(intent || '');
        $('#kbAnswer').val(answer || '');

        // Display Metadata
        if (entity.Metadata) {
            try {
                var metadata = typeof entity.Metadata === 'string' 
                    ? JSON.parse(entity.Metadata) 
                    : entity.Metadata;
                $('#kbMetadataTextarea').val(JSON.stringify(metadata, null, 2));
            } catch (e) {
                console.warn('Failed to parse Metadata as JSON:', e);
                $('#kbMetadataTextarea').val(entity.Metadata);
            }
        } else {
            $('#kbMetadataTextarea').val('');
        }
    };

    // Parse Content string in special format: =Question;Question2 | Intent: xxx | Answer: xxx
    // 解析特殊格式的Content字符串：=Question;Question2 | Intent: xxx | Answer: xxx
    kbmanager.parseContentString = function (contentStr) {
        var result = {
            Question: '',
            Intent: '',
            Answer: ''
        };
        
        try {
            // Format: =Question1;Question2;Question3 | Intent: xxx | Answer: xxx
            // Remove leading = if present
            var str = contentStr.trim();
            if (str.startsWith('=')) {
                str = str.substring(1).trim();
            }
            
            // Split by | to get parts
            var parts = str.split('|');
            
            if (parts.length >= 1) {
                // First part is questions (before first |)
                var questionPart = parts[0].trim();
                if (questionPart) {
                    // Questions are separated by ;
                    result.Question = questionPart;
                }
            }
            
            if (parts.length >= 2) {
                // Second part should be Intent
                var intentPart = parts[1].trim();
                if (intentPart.startsWith('Intent:')) {
                    result.Intent = intentPart.substring('Intent:'.length).trim();
                } else if (intentPart.startsWith('Intent：')) {
                    // Support Chinese colon
                    result.Intent = intentPart.substring('Intent：'.length).trim();
                }
            }
            
            if (parts.length >= 3) {
                // Third part should be Answer
                var answerPart = parts[2].trim();
                if (answerPart.startsWith('Answer:')) {
                    result.Answer = answerPart.substring('Answer:'.length).trim();
                } else if (answerPart.startsWith('Answer：')) {
                    // Support Chinese colon
                    result.Answer = answerPart.substring('Answer：'.length).trim();
                } else {
                    // If no Answer: prefix, treat the whole part as answer
                    result.Answer = answerPart;
                }
            }
            
            // If there are more parts, append them to Answer
            if (parts.length > 3) {
                for (var i = 3; i < parts.length; i++) {
                    result.Answer += ' | ' + parts[i].trim();
                }
            }
        } catch (e) {
            console.error('Error parsing content string:', e);
            // Fallback: treat as plain text
            result = { text: contentStr };
        }
        
        return result;
    };

    // Removed save, generateMetadata, and newForm functions as they are no longer needed
    // 已移除保存、生成元数据和新建表单函数，因为不再需要

    kbmanager.search = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            title: kresource.getItem('kb.search'),
            message: $('<div id="kbSearchContent"></div>').html(`
                <div class="form-group">
                    <label><span class="lang" as="kb.search.query"></span></label>
                    <input type="text" id="kbSearchQuery" class="form-control" placeholder="Enter search query" />
                </div>
                <div class="form-group">
                    <label><span class="lang" as="kb.search.threshold"></span></label>
                    <input type="number" id="kbSearchThreshold" class="form-control" value="0.5" min="0" max="1" step="0.1" />
                </div>
                <div class="form-group">
                    <label><span class="lang" as="kb.search.count"></span></label>
                    <input type="number" id="kbSearchCount" class="form-control" value="5" min="1" max="20" />
                </div>
            `),
            onshown: function (dialog) {
                // Set focus to search query input field
                // 设置焦点到查询问题输入框
                setTimeout(function () {
                    var queryInput = document.getElementById('kbSearchQuery');
                    if (queryInput) {
                        queryInput.focus();
                        queryInput.select(); // Optional: select existing text if any
                    }
                }, 100);
            },
            buttons: [{
                label: kresource.getItem('kb.search'),
                cssClass: 'btn-primary',
                action: function (dialog) {
                    var query = $('#kbSearchQuery').val().trim();
                    var threshold = parseFloat($('#kbSearchThreshold').val()) || 0.5;
                    var count = parseInt($('#kbSearchCount').val()) || 5;

                    if (!query) {
                        if (window.kmsgbox) {
                            window.kmsgbox.warn('Please enter a search query');
                        } else {
                            alert('Please enter a search query');
                        }
                        return;
                    }

                    dialog.close();
                    kbmanager.performSearch(query, threshold, count);
                }
            }, {
                label: kresource.getItem('cancel'),
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
    };

    kbmanager.performSearch = function (query, threshold, count) {
        kbmanager.showLoading();
        var url = kconfig.webApiUrl + 'api/Wf2AI/SearchKnowledgeBase';
        var data = {
            Query: query,
            Threshold: threshold,
            Count: count
        };

        jshelper.ajaxPost(url, JSON.stringify(data), function (result) {
            kbmanager.hideLoading();
            if (result && result.Status === 1) {
                if (kbmanager.kbGrid) {
                    // Process the search results similar to GetAll
                    var documents = result.Entity || [];
                    kbmanager.kbGrid.setGridOption('rowData', documents);
                }
                if (window.kmsgbox) {
                    window.kmsgbox.info('Search completed: ' + (result.Entity ? result.Entity.length : 0) + ' results found');
                }
            } else {
                if (window.kmsgbox) {
                    window.kmsgbox.error('Search failed', result ? result.Message : 'Unknown error');
                } else {
                    console.error('Search failed:', result ? result.Message : 'Unknown error');
                }
            }
        });
    };

    kbmanager.refresh = function () {
        kbmanager.getDocumentList();
    };

    return kbmanager;
})();

window.kbmanager = kbmanager;
export default kbmanager;

