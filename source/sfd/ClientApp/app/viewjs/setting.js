import kconfig from '../config/kconfig.js';
import settingapi from './settingapi.js';
import kmsgbox from '../script/kmsgbox.js';
import kresource from './kresource.js';

const setting = (function () {
    function setting() {
    }

    setting.mcurrentModelEntity = null;

    // Helper function to get the correct container (popup or direct page)
    function getContainer() {
        return $('#popupSetting').length > 0 ? $('#popupSetting') : $('body');
    }

    setting.init = function () {
        // Initialize tree parent expand/collapse
        $(document).on('click', '.setting-tree-parent', function (e) {
            e.stopPropagation();
            $(this).toggleClass('expanded');
        });

        // Initialize child item click handler
        $(document).on('click', '.setting-tree-child', function (e) {
            if ($(e.target).hasClass('setting-tree-child-delete')) {
                return; // Don't trigger selection when clicking delete button
            }
            var modelId = $(this).data('model-id');
            if (modelId) {
                selectModel(modelId);
            }
        });

        // Initialize delete button click handler
        $(document).on('click', '.setting-tree-child-delete', function (e) {
            e.stopPropagation();
            var $child = $(this).closest('.setting-tree-child');
            var modelId = $child.data('model-id');
            if (modelId) {
                deleteModel(modelId);
            }
        });

        // Load AI model list
        loadModelList();

        // Test connection button click handler - use event delegation to work with dynamically loaded content
        $(document).on('click', '#popupSetting #test-btn, #test-btn', function () {
            testModelConnection();
        });

        // Save button click handler - use event delegation to work with dynamically loaded content
        $(document).on('click', '#popupSetting #save-btn, #save-btn', function () {
            saveModelConfig();
        });

        // Add new button click handler - use event delegation
        $(document).on('click', '#popupSetting #add-new-btn, #add-new-btn', function () {
            clearModelData();
            setting.mcurrentModelEntity = null;
            // Remove active class from all children
            $('#popupSetting .setting-tree-child, .setting-tree-child').removeClass('active');
        });

        // Handle API key input - clear mask when user starts typing - use event delegation
        $(document).on('focus', '#popupSetting #api-key, #api-key', function () {
            var $input = $(this);
            var maskValue = $input.data('mask-value');
            if ($input.data('has-existing-key') && maskValue && $input.val() === maskValue) {
                $input.val('');
            }
        });

        // Handle API key input - restore mask if user leaves it empty and there's an existing key - use event delegation
        $(document).on('blur', '#popupSetting #api-key, #api-key', function () {
            var $input = $(this);
            var maskValue = $input.data('mask-value');
            if ($input.data('has-existing-key') && maskValue && $input.val().trim() === '') {
                $input.val(maskValue);
            }
        });

        // Model provider change handler - update default base URL - use event delegation
        $(document).on('change input', '#popupSetting #model-provider, #model-provider', function () {
            updateDefaultBaseUrl($(this).val());
        });
    }

    function switchPanel(panelId) {
        var $container = getContainer();
        // Update menu item active state
        $container.find('.setting-menu-item').removeClass('active');
        $container.find('.setting-menu-item[data-panel="' + panelId + '"]').addClass('active');

        // Update panel visibility
        $container.find('.setting-panel').removeClass('active');
        $container.find('#' + panelId + '-panel').addClass('active');
    }

    function updateDefaultBaseUrl(provider) {
        const defaultUrls = {
            'OpenAI': 'https://api.openai.com/v1',
            'Azure OpenAI': 'https://your-resource.openai.azure.com/',
            'Anthropic': 'https://api.anthropic.com/v1',
            'Google': 'https://generativelanguage.googleapis.com/v1',
            'Baidu': 'https://aip.baidubce.com/rpc/2.0/ai_custom/v1',
            'Alibaba': 'https://dashscope.aliyuncs.com/api/v1',
            'DeepSeek': 'https://api.deepseek.com/v1',
            'Moonshot': 'https://api.moonshot.cn/v1',
            'Zhipu': 'https://open.bigmodel.cn/api/paas/v4'
        };

        if (defaultUrls[provider]) {
            var $container = getContainer();
            $container.find('#base-url').val(defaultUrls[provider]);
        }
    }

    function loadModelList() {
        // Load all AI model configurations
        settingapi.getList(function (result) {
            if (result.Status === 1 && result.Entity && result.Entity.length > 0) {
                renderModelList(result.Entity);
                // Load the first active model or the first one
                var activeModel = result.Entity.find(function (m) { return m.IsActive === true; }) || result.Entity[0];
                if (activeModel) {
                    selectModel(activeModel.Id);
                }
            } else {
                // No models, clear the list
                renderModelList([]);
                clearModelData();
            }
        });
    }

    function renderModelList(models) {
        // Support both popup and direct page contexts
        var $children = $('#popupSetting #ai-model-provider-children, #ai-model-provider-children');
        $children.empty();

        if (models && models.length > 0) {
            // 去重：按 Id 过滤，避免重复记录（有用户反馈重复条目）
            var seen = {};
            models.forEach(function (model) {
                if (!model || seen[model.Id]) return;
                seen[model.Id] = true;

                var displayName = (model.ModelProvider || 'Unnamed') + (model.Description ? ' - ' + model.Description : '');
                var $child = $('<li class="setting-tree-child" data-model-id="' + model.Id + '">' +
                    '<span class="setting-tree-child-name">' + displayName + '</span>' +
                    '<button class="setting-tree-child-delete" title="Delete">×</button>' +
                    '</li>');
                $children.append($child);
            });
        }
    }

    function selectModel(modelId) {
        var $container = getContainer();
        // Remove active class from all children
        $container.find('.setting-tree-child').removeClass('active');
        // Add active class to selected child
        $container.find('.setting-tree-child[data-model-id="' + modelId + '"]').addClass('active');

        // Load model data
        settingapi.getById(modelId, function (result) {
            if (result.Status === 1 && result.Entity) {
                setting.mcurrentModelEntity = result.Entity;
                renderModelData(result.Entity);
            } else {
                kmsgbox.error(result.Message || 'Failed to load model configuration');
            }
        });
    }

    // Expose selectModel function for external calls
    setting.selectModel = function (modelId) {
        selectModel(modelId);
    }

    function deleteModel(modelId) {
        kmsgbox.confirm('Are you sure you want to delete this model?', function () {
            settingapi.delete(modelId, function (result) {
                if (result.Status === 1) {
                    kmsgbox.success('Model deleted successfully');
                    // Reload the list
                    loadModelList();
                    // Clear the form if the deleted model was selected
                    if (setting.mcurrentModelEntity && setting.mcurrentModelEntity.Id === modelId) {
                        clearModelData();
                        setting.mcurrentModelEntity = null;
                    }
                } else {
                    kmsgbox.error(result.Message || 'Failed to delete model');
                }
            });
        });
    }

    function clearModelData() {
        var $container = getContainer();
        
        // 清除测试结果
        var $testResult = $container.find('#test-result');
        var $testResultDetails = $container.find('#test-result-details');
        $testResult.hide();
        $testResultDetails.hide();
        
        $container.find('#model-provider').val('');
        $container.find('#base-url').val('');
        $container.find('#api-key').val('');
        $container.find('#api-key').attr('placeholder', 'Enter your API key');
        $container.find('#api-key').removeData('has-existing-key');
        $container.find('#description').val('');
    }

    function renderModelData(model) {
        if (!model) return false;

        var $container = getContainer();
        $container.find('#model-provider').val(model.ModelProvider || '');
        $container.find('#base-url').val(model.BaseUrl || '');
        // Show masked API key if it exists
        if (model.ApiKey && model.ApiKey.length > 0) {
            // Fill with dots to show mask effect (48 dots to match typical API key length)
            var maskLength = Math.max(48, Math.min(64, model.ApiKey.length)); // Use actual length or 48-64 range
            var mask = '•'.repeat(maskLength);
            var $apiKey = $container.find('#api-key');
            $apiKey.val(mask);
            $apiKey.data('has-existing-key', true);
            $apiKey.data('mask-value', mask); // Store mask value for comparison
            $apiKey.attr('placeholder', 'API key is set (enter new key to update)');
        } else {
            var $apiKey = $container.find('#api-key');
            $apiKey.val('');
            $apiKey.removeData('has-existing-key');
            $apiKey.removeData('mask-value');
            $apiKey.attr('placeholder', 'Enter your API key');
        }
        $container.find('#description').val(model.Description || '');
    }

    function testModelConnection() {
        var $container = getContainer();
        
        // 获取表单数据
        var modelProvider = ($container.find('#model-provider').val() || '').trim();
        var baseUrl = ($container.find('#base-url').val() || '').trim();
        var apiKeyInput = ($container.find('#api-key').val() || '').trim();
        var hasExistingKey = $container.find('#api-key').data('has-existing-key');
        var maskValue = $container.find('#api-key').data('mask-value');
        
        // 获取结果区域元素
        var $testResult = $container.find('#test-result');
        var $testResultText = $container.find('#test-result-text');
        var $testResultIcon = $container.find('#test-result-icon');
        var $testResultDetails = $container.find('#test-result-details');
        
        // 隐藏之前的结果
        $testResult.hide();
        $testResultDetails.hide();
        
        // 验证必填字段
        if (!baseUrl) {
            $testResult.show();
            $testResult.css({
                'background-color': '#f8d7da',
                'border': '1px solid #f5c6cb',
                'color': '#721c24'
            });
            $testResultIcon.html('✗');
            $testResultText.text('Base URL is required');
            $testResultDetails.hide();
            return;
        }

        // 处理 API Key：如果输入的是掩码值，说明用户没有修改，需要从当前实体获取
        var apiKey = '';
        if (maskValue && apiKeyInput === maskValue && hasExistingKey) {
            // 用户没有修改 API Key，使用当前保存的实体中的 API Key
            if (setting.mcurrentModelEntity && setting.mcurrentModelEntity.ApiKey) {
                apiKey = setting.mcurrentModelEntity.ApiKey;
            } else {
                $testResult.show();
                $testResult.css({
                    'background-color': '#f8d7da',
                    'border': '1px solid #f5c6cb',
                    'color': '#721c24'
                });
                $testResultIcon.html('✗');
                $testResultText.text('Please enter or update the API Key');
                $testResultDetails.hide();
                return;
            }
        } else if (apiKeyInput && apiKeyInput !== maskValue) {
            // 用户输入了新的 API Key
            apiKey = apiKeyInput;
        } else {
            $testResult.show();
            $testResult.css({
                'background-color': '#f8d7da',
                'border': '1px solid #f5c6cb',
                'color': '#721c24'
            });
            $testResultIcon.html('✗');
            $testResultText.text('API Key is required');
            $testResultDetails.hide();
            return;
        }
        
        // 禁用测试按钮，显示加载状态
        var $testBtn = $container.find('#test-btn');
        var originalText = $testBtn.html();
        $testBtn.prop('disabled', true).html('<span class="spinner-border spinner-border-sm" role="status"></span> Testing...');
        
        // 显示测试中状态
        $testResult.show();
        $testResult.css({
            'background-color': '#d1ecf1',
            'border': '1px solid #bee5eb',
            'color': '#0c5460'
        });
        $testResultIcon.html('⏳');
        $testResultText.text('Testing connection...');
        $testResultDetails.hide();

        // 获取 ApiUUID（从当前模型实体中获取）
        var apiUUID = null;
        if (setting.mcurrentModelEntity && setting.mcurrentModelEntity.ApiUUID) {
            apiUUID = setting.mcurrentModelEntity.ApiUUID;
        }
        
        // 调用测试 API
        settingapi.testConnection(baseUrl, apiKey, modelProvider, apiUUID, function (result) {
            $testBtn.prop('disabled', false).html(originalText);
            
            // 显示结果区域
            $testResult.show();
            $testResultDetails.show();
            
            var isSuccess = result && (result.Success === true || result.Status === 1);
            if (isSuccess) {
                var responseText = (result && result.Entity) ? result.Entity : 'OK';
                var fullResponseText = responseText;

                // 显示简短结果
                if (responseText.length > 100) {
                    responseText = responseText.substring(0, 100) + '...';
                }

                // 设置成功样式
                $testResult.css({
                    'background-color': '#d4edda',
                    'border': '1px solid #c3e6cb',
                    'color': '#155724'
                });
                $testResultIcon.html('✓');
                $testResultText.text('Connection test successful!');

                // 显示详细结果
                $testResultDetails.css({
                    'background-color': '#d4edda',
                    'border': '1px solid #c3e6cb',
                    'color': '#155724'
                });
                $testResultDetails.text('Response: ' + fullResponseText);
            } else {
                var errorMsg = 'Connection test failed';
                if (result && result.Message) errorMsg = result.Message;
                else if (result && result.Entity && typeof result.Entity === 'string') errorMsg = result.Entity;

                // 设置错误样式
                $testResult.css({
                    'background-color': '#f8d7da',
                    'border': '1px solid #f5c6cb',
                    'color': '#721c24'
                });
                $testResultIcon.html('✗');
                $testResultText.text('Connection test failed');

                // 显示详细错误信息
                $testResultDetails.css({
                    'background-color': '#f8d7da',
                    'border': '1px solid #f5c6cb',
                    'color': '#721c24'
                });
                $testResultDetails.text('Error: ' + errorMsg);
            }
        });
    }

    function saveModelConfig() {
        var $container = getContainer();
        
        // Validate form - use || '' to handle undefined values
        var modelProvider = ($container.find('#model-provider').val() || '').trim();
        var baseUrl = ($container.find('#base-url').val() || '').trim();
        var apiKeyInput = ($container.find('#api-key').val() || '').trim();
        var description = ($container.find('#description').val() || '').trim();
        var hasExistingKey = $container.find('#api-key').data('has-existing-key');
        var maskValue = $container.find('#api-key').data('mask-value');
        
        // Check if the input is the mask placeholder
        var apiKey = (maskValue && apiKeyInput === maskValue && hasExistingKey) ? '' : apiKeyInput;

        if (!modelProvider) {
            showNotification('Please select or enter a model provider', 'error');
            return;
        }

        if (!baseUrl) {
            showNotification('Please enter base URL', 'error');
            return;
        }

        // Only require API key if it's a new model or user entered a new key
        if (!apiKey && (!hasExistingKey || !setting.mcurrentModelEntity)) {
            showNotification('Please enter API key', 'error');
            return;
        }

        // Proceed with save
        proceedSave(modelProvider, baseUrl, apiKey, description);
    }

    function proceedSave(modelProvider, baseUrl, apiKey, description) {
        // Prepare entity
        var modelEntity = setting.mcurrentModelEntity || {};
        modelEntity.ModelProvider = modelProvider;
        modelEntity.BaseUrl = baseUrl;
        // Only update APIKey if user entered a new one (not the mask)
        // apiKey is already filtered in saveModelConfig, so if it's not empty, use it
        if (apiKey) {
            modelEntity.ApiKey = apiKey;
        } else if (modelEntity.ApiKey) {
            // Keep existing APIKey if user didn't enter a new one
            // ApiKey is already in modelEntity, no need to set it
        } else {
            // New model without APIKey - this should have been caught by validation
            modelEntity.ApiKey = apiKey;
        }
        modelEntity.Description = description;
        modelEntity.IsActive = true;

        // Save to backend
        settingapi.save(modelEntity, function (result) {
            if (result.Status === 1) {
                // Check if Entity exists in response
                if (result.Entity) {
                    setting.mcurrentModelEntity = result.Entity;
                    showNotification('Model configuration saved successfully!', 'success');
                    
                    var $container = getContainer();
                    // Show masked API key after successful save
                    var savedApiKey = result.Entity.ApiKey || '';
                    var maskLength = savedApiKey.length > 0 ? Math.max(48, Math.min(64, savedApiKey.length)) : 48;
                    var mask = '•'.repeat(maskLength);
                    var $apiKey = $container.find('#api-key');
                    $apiKey.val(mask);
                    $apiKey.data('has-existing-key', true);
                    $apiKey.data('mask-value', mask);
                    $apiKey.attr('placeholder', 'API key saved (enter new key to update)');
                    // Reload the list to reflect changes
                    loadModelList();
                    // Re-select the saved model
                    if (result.Entity.Id) {
                        setTimeout(function () {
                            selectModel(result.Entity.Id);
                        }, 100);
                    }
                } else {
                    // If Entity is missing, try to reload from Id
                    if (modelEntity.Id && modelEntity.Id > 0) {
                        settingapi.getById(modelEntity.Id, function (getResult) {
                            if (getResult.Status === 1 && getResult.Entity) {
                                setting.mcurrentModelEntity = getResult.Entity;
                                showNotification('Model configuration saved successfully!', 'success');
                                loadModelList();
                                setTimeout(function () {
                                    selectModel(getResult.Entity.Id);
                                }, 100);
                            } else {
                                showNotification('Model saved but failed to reload. Please refresh the page.', 'warning');
                                loadModelList();
                            }
                        });
                    } else {
                        showNotification('Model saved but response format is unexpected. Please refresh the page.', 'warning');
                        loadModelList();
                    }
                }
            } else {
                kmsgbox.error(result.Message || 'Failed to save model configuration');
            }
        });
    }

    function showNotification(message, type) {
        var $container = getContainer();
        const notification = $container.find('#notification, .notification').first();
        if (notification.length > 0) {
            notification.text(message);
            notification.removeClass('success error');
            notification.addClass(type);
            notification.addClass('show');

            setTimeout(() => {
                notification.removeClass('show');
            }, 3000);
        } else {
            // Fallback to kmsgbox if notification element not found
            if (type === 'success') {
                kmsgbox.success(message);
            } else {
                kmsgbox.error(message);
            }
        }
    }

    return setting;
})()

export default setting

