import kconfig from '../config/kconfig.js';
import axconfigapi from '../viewjs/axconfigapi.js';

const axconfig = (function () {
    function axconfig() {
    }

    axconfig.init = function () {
        // 标签页切换
        $('.tab').on('click', function () {
            const tabId = $(this).data('tab');
            $('.tab').removeClass('active');
            $(this).addClass('active');
            $('.tab-pane').removeClass('active');
            $(`#${tabId}-tab`).addClass('active');
        });

        // 温度滑块值显示
        $('#temperature').on('input', function () {
            $(this).next('.slider-value').text($(this).val());
        });

        // 保存配置（仅保存 AxConfigEntity，不再包含变量列表）
        $('#save-btn').on('click', function () {
            // 收集表单数据
            const configData = {
                ConfigUUID: $('#config-uuid').val(),
                ModelProviderId: parseInt($('#account-credential').val()) || null,
                ModelName: $('#model-version').val().trim() || null,
                Description: $('#axconfig-description').val(),
                Temperature: parseFloat($('#temperature').val()) || 0,
                MaxTokens: parseInt($('#max-tokens').val()) || 0,
                SystemPrompt: $('#system-prompt').val(),
                UserMessage: $('#user-message-prompt').val(),
                ResponseFormat: $('input[name="format"]:checked').val(),
                Timeout: parseInt($('#timeout').val()) || 0,
                MaxRetries: parseInt($('#max-retries').val()) || 0,
                ErrorHandling: $('#error-handling').val(),
                FallbackAgent: $('#fallback-agent').val(),
                LogLevel: $('#log-level').val(),
                CustomInstructions: $('#custom-instructions').val()
            };

            // 验证必填字段
            var activityName = $('#activity-name').val();
            if (!activityName) {
                showNotification(kresource.getItem('axconfig.validation.service.name'), 'error');
                return;
            }

            if (!configData.SystemPrompt) {
                showNotification(kresource.getItem('axconfig.validation.system.prompt'), 'error');
                return;
            }

            // 验证 ModelProviderId（账号凭证）
            if (!configData.ModelProviderId || configData.ModelProviderId === null) {
                showNotification(kresource.getItem('axconfig.validation.model.provider.id') || 'Please select an account credential', 'error');
                return;
            }

            // 验证 ModelName（模型名称）
            if (!configData.ModelName || configData.ModelName.trim() === '') {
                showNotification(kresource.getItem('axconfig.validation.model.name') || 'Model name is required', 'error');
                return;
            }

            // 发送数据到后端API（仅 AxConfigEntity）
            saveAxConfig(configData);
        });

        // Account credential dropdown change handler
        $('#account-credential').on('change', function () {
            var selectedValue = $(this).val();
            $('#btn-edit-credential').prop('disabled', !selectedValue);
        });

        // Add credential button click handler
        $('#btn-add-credential').on('click', function () {
            openSettingPage(null, function () {
                // Reload account credential list after setting page closes
                loadAccountCredentialList();
            });
        });

        // Edit credential button click handler
        $('#btn-edit-credential').on('click', function () {
            var selectedId = $('#account-credential').val();
            if (selectedId) {
                openSettingPage(parseInt(selectedId), function () {
                    // Reload account credential list after setting page closes
                    loadAccountCredentialList();
                });
            }
        });

        //load service data
        loadAxConfigData();
        
        //load account credential list
        loadAccountCredentialList();
    }

    function loadAxConfigData() {
        var currentElement = kmain.currentSelectedElement;
        if (currentElement === undefined || currentElement.businessObject === null) {
            return false;
        }
        
        var businessObject = currentElement.businessObject;
        var activityName = businessObject.name;
        var configUUID = axconfig.getConfigUUID(businessObject);

        // 写入活动名称（只用于显示，不保存）
        document.getElementById('activity-name').value = activityName || '';
        // 写入配置UUID
        document.getElementById('config-uuid').value = configUUID || '';

        if (configUUID && configUUID !== '') {
            axconfigapi.getByUUID(configUUID, function (result) {
                if (result.Status === 1) {
                    // 从后端返回的是 AxConfigEntity 对象
                    var axConfigEntity = result.Entity;

                    if (axConfigEntity) {
                        // 保存 AxConfigEntity 到缓存
                        axconfig.mcurrentAxConfigEntity = axConfigEntity;

                        // 渲染表单数据：仅传递 AxConfigEntity
                        renderAxConfigData(axConfigEntity);
                    }
                } else {
                    kmsgbox.error(kresource.getItem("axconfigerrormsg"));
                }
            });
        }
    }

    function renderAxConfigData(axConfigEntity) {
        if (!axConfigEntity) return false;

        // 填充基本设置（activity_name 不保存，只显示当前元素名称）
        document.getElementById('config-uuid').value = axConfigEntity.ConfigUUID || '';
        document.getElementById('axconfig-description').value = axConfigEntity.Description || '';

        // 填充模型配置
        if (axConfigEntity.ModelProviderId) {
            // 设置 account-credential
            var credentialSelect = document.getElementById('account-credential');
            var modelId = axConfigEntity.ModelProviderId;
            
            // 找到对应的 credential
            var matchingOption = $(credentialSelect).find('option[value="' + modelId + '"]');
            if (matchingOption.length > 0) {
                credentialSelect.value = modelId;
                $('#btn-edit-credential').prop('disabled', false);
            }
        }
        
        // 填充模型版本号
        if (axConfigEntity.ModelName) {
            document.getElementById('model-version').value = axConfigEntity.ModelName;
        }
        if (axConfigEntity.Temperature !== undefined && axConfigEntity.Temperature !== null) {
            document.getElementById('temperature').value = axConfigEntity.Temperature;
            var sliderValueElement = document.querySelector('.slider-value');
            if (sliderValueElement) {
                sliderValueElement.textContent = axConfigEntity.Temperature;
            }
        }
        document.getElementById('max-tokens').value = axConfigEntity.MaxTokens || '';
        if (axConfigEntity.ResponseFormat) {
            const formatRadio = document.querySelector(`input[name="format"][value="${axConfigEntity.ResponseFormat}"]`);
            if (formatRadio) {
                formatRadio.checked = true;
            }
        }

        // 填充提示词
        document.getElementById('system-prompt').value = axConfigEntity.SystemPrompt || '';
        document.getElementById('user-message-prompt').value = axConfigEntity.UserMessage || '';

        // 填充高级设置
        document.getElementById('timeout').value = axConfigEntity.Timeout || '';
        document.getElementById('max-retries').value = axConfigEntity.MaxRetries || '';
        document.getElementById('error-handling').value = axConfigEntity.ErrorHandling || 'retry';
        document.getElementById('fallback-agent').value = axConfigEntity.FallbackAgent || '';
        document.getElementById('log-level').value = axConfigEntity.LogLevel || 'warn';
        document.getElementById('custom-instructions').value = axConfigEntity.CustomInstructions || '';
    }

    // 显示通知
    function showNotification(message, type) {
        const notification = $('#notification');
        notification.text(message);
        notification.removeClass('success error');
        notification.addClass(type);
        notification.addClass('show');

        setTimeout(() => {
            notification.removeClass('show');
        }, 3000);
    }

    // 保存 AXConfig 配置（仅 AxConfigEntity）
    function saveAxConfig(configData) {
        var currentElement = kmain.currentSelectedElement;
        if (!currentElement || !currentElement.businessObject) {
            showNotification(kresource.getItem('axconfig.error.no.element'), 'error');
            return;
        }

        var businessObject = currentElement.businessObject;
        var configUUID = axconfig.getConfigUUID(businessObject);
        var activityName = businessObject.name; // 只用于显示，不保存
        var activityId = businessObject.id;

        // 获取或创建 AxConfigEntity
        var axConfigEntity = axconfig.mcurrentAxConfigEntity;
        if (axConfigEntity === null || axConfigEntity === undefined) {
            axConfigEntity = {};
            axConfigEntity.ProcessId = kmain.mxSelectedProcessEntity.ProcessId;
            axConfigEntity.Version = kmain.mxSelectedProcessEntity.Version;
            axConfigEntity.ActivityId = activityId;
        }

        // 更新 AXConfig 实体（不包含 activity_name）
        axConfigEntity.ConfigUUID = configUUID || configData.ConfigUUID;
        axConfigEntity.ModelProviderId = configData.ModelProviderId;
        axConfigEntity.Description = configData.Description;
        axConfigEntity.Temperature = configData.Temperature;
        axConfigEntity.MaxTokens = configData.MaxTokens;
        axConfigEntity.SystemPrompt = configData.SystemPrompt;
        axConfigEntity.UserMessage = configData.UserMessage;
        axConfigEntity.ResponseFormat = configData.ResponseFormat;
        axConfigEntity.Timeout = configData.Timeout;
        axConfigEntity.MaxRetries = configData.MaxRetries;
        axConfigEntity.ErrorHandling = configData.ErrorHandling;
        axConfigEntity.FallbackAgent = configData.FallbackAgent;
        axConfigEntity.LogLevel = configData.LogLevel;
        axConfigEntity.CustomInstructions = configData.CustomInstructions;
        axConfigEntity.ModelName = configData.ModelName;

        // 直接传递 AxConfigEntity 到后端
        axconfigapi.save(axConfigEntity, function (result) {
            if (result.Status === 1) {
                // 更新本地缓存的实体
                if (result.Entity) {
                    axconfig.mcurrentAxConfigEntity = result.Entity;
                }
                showNotification(kresource.getItem('axconfig.save.success') || '配置保存成功！', 'success');
            } else {
                kmsgbox.error(result.Message);
            }
        });
    }

    // 获取服务类型
    axconfig.getServiceType = function (businessObject) {
        // 1. 首先尝试从扩展元素获取
        var aiServiceConfig = getAIServiceConfig(businessObject);
        if (aiServiceConfig && aiServiceConfig.type) {
            return aiServiceConfig.type;
        }

        // 2. 从 businessObject 直接属性获取
        if (businessObject && businessObject.type) {
            return businessObject.type;
        }

        return null;
    }

    // 获取配置 UUID
    axconfig.getConfigUUID = function (businessObject) {
        var aiServiceConfig = getAIServiceConfig(businessObject);
        if (aiServiceConfig && aiServiceConfig.configUUID) {
            return aiServiceConfig.configUUID;
        }

        return businessObject.uuid || null;
    }

    // 读取 AI 服务配置函数
    function getAIServiceConfig(businessObject) {
        if (!businessObject) {
            console.warn('businessObject is null');
            return null;
        }

        if (!businessObject.extensionElements) {
            console.log('none extension elements');
            return null;
        }

        var extensionElements = businessObject.extensionElements;

        if (!extensionElements.values || extensionElements.values.length === 0) {
            console.log('extension elements is null');
            return null;
        }

        // 查找 sf:AIServices
        var aiServices = extensionElements.values.find(function (value) {
            return value && value.$type === 'sf:AIServices';
        });

        if (!aiServices) {
            console.log('not found sf:AIServices');
            return null;
        }

        // 获取 AI Service 配置
        if (aiServices.aiServices && aiServices.aiServices.length > 0) {
            var aiService = aiServices.aiServices[0];
            return aiService;
        }

        console.log('AI Service array is empty');
        return null;
    }


    // 加载账号凭证列表（Account Credential）
    function loadAccountCredentialList() {
        axconfigapi.getModelList(function (result) {
            if (result.Status === 1 && result.Entity && Array.isArray(result.Entity)) {
                const credentialSelect = document.getElementById('account-credential');
                const currentValue = credentialSelect.value;
                // 如果当前没值，则尝试从已加载的 AxConfigEntity 中获取 ModelProviderId
                const existingEntity = axconfig.mcurrentAxConfigEntity;
                const targetId = currentValue ||
                    (existingEntity && (existingEntity.ModelProviderId || existingEntity.modelProviderId || existingEntity.model_provider_id)) ||
                    '';
                
                // 清空现有选项（保留第一个请选择选项）
                credentialSelect.innerHTML = '<option value="">-- <span class="lang" as="selectplaceholder"></span> --</option>';
                
                // 填充账号凭证列表：绑定 model_provider_id (Id) 和 model_provider (ModelProvider)
                result.Entity.forEach(function (model) {
                    const option = document.createElement('option');
                    const idVal = model.ID || model.Id || model.id;
                    if (idVal === undefined || idVal === null) return;
                    option.value = idVal;
                    // 显示 model_provider 名称
                    option.textContent = model.ModelProvider || model.modelProvider || model.model_provider || 'Unnamed';
                    $(option).data('provider', option.textContent);
                    credentialSelect.appendChild(option);
                });
                
                // 恢复之前选择的值（含后端现有配置）
                if (targetId) {
                    credentialSelect.value = targetId;
                    $('#btn-edit-credential').prop('disabled', false);
                } else {
                    $('#btn-edit-credential').prop('disabled', true);
                }
                
                // 触发本地化更新
                if (typeof kresource !== 'undefined' && kresource.localize) {
                    kresource.localize();
                }
            }
        });
    }

    // 打开 Setting 页面
    function openSettingPage(modelId, callback) {
        var BootstrapDialog = require('bootstrap5-dialog');
        var dialog = BootstrapDialog.show({
            message: $('<div id="popupSetting"></div>'),
            title: kresource.getItem("setting") || 'Setting',
            size: BootstrapDialog.SIZE_LARGE,
            cssClass: 'setting-dialog-large',
            onshown: function (dialogRef) {
                $("#popupSetting").load('pages/setting/index.html', function (response, status, xhr) {
                    if (status === "error") {
                        console.error("Failed to load setting page");
                        return;
                    }
                    
                    // 等待 setting 模块和 DOM 都加载完成
                    var checkCount = 0;
                    var maxChecks = 50; // 最多检查 5 秒（50 * 100ms）
                    var checkInterval = setInterval(function () {
                        checkCount++;
                        // 检查 setting 模块是否已加载，以及 DOM 元素是否存在
                        if (typeof window.setting !== 'undefined' && 
                            typeof window.setting.init === 'function' &&
                            $('#popupSetting #save-btn').length > 0) {
                            
                            // 确保 setting 已初始化（如果还没有初始化）
                            if (!$('#popupSetting #save-btn').data('initialized')) {
                                try {
                                    // 重新初始化 setting（如果 DOM 已加载但未初始化）
                                    window.setting.init();
                                    $('#popupSetting #save-btn').data('initialized', true);
                                } catch (e) {
                                    console.error("Error initializing setting:", e);
                                }
                            }
                            
                            // 如果提供了 modelId，则选中该模型
                            if (modelId && typeof window.setting.selectModel === 'function') {
                                window.setting.selectModel(modelId);
                            }
                            
                            clearInterval(checkInterval);
                        } else if (checkCount >= maxChecks) {
                            console.warn("Setting module or DOM not loaded after timeout");
                            clearInterval(checkInterval);
                        }
                    }, 100);
                });
            },
            onhidden: function () {
                // 当对话框关闭时，执行回调
                if (callback && typeof callback === 'function') {
                    callback();
                }
            },
            draggable: true
        });
    }

    axconfig.delete = function (serviceIdentifier) {
        axconfigapi.delete(serviceIdentifier, function (result) {
            if (result.Status === 1) {
                ;
            } else {
                kmsgbox.error(kresource.getItem("axconfigdelerrormsg", result.Message));
            }
        });
    }
    return axconfig;
})()

export default axconfig
