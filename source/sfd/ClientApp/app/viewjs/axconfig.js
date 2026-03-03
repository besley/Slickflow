import kconfig from '../config/kconfig.js';
import axconfigapi from '../viewjs/axconfigapi.js';

const axconfig = (function () {
    function axconfig() {
    }

    /**
     * Initialize axconfig module
     */
    axconfig.init = function () {
        $('.tab').on('click', function () {
            const tabId = $(this).data('tab');
            $('.tab').removeClass('active');
            $(this).addClass('active');
            $('.tab-pane').removeClass('active');
            $(`#${tabId}-tab`).addClass('active');
        });

        $('#temperature').on('input', function () {
            $(this).next('.slider-value').text($(this).val());
        });

        $('#similarity-threshold').on('input', function () {
            const value = parseFloat($(this).val()).toFixed(2);
            $('#similarity-value').text(value);
        });

        $('#save-btn').on('click', function () {
            var currentElement = kmain.currentSelectedElement;
            if (!currentElement || !currentElement.businessObject) {
                showNotification(kresource.getItem('axconfig.error.no.element') || 'No element selected', 'error');
                return;
            }
            var businessObject = currentElement.businessObject;

            const searchModes = [];
            if ($('#search-mode-semantic').is(':checked')) {
                searchModes.push('semantic');
            }
            if ($('#search-mode-keyword').is(':checked')) {
                searchModes.push('keyword');
            }
            if ($('#search-mode-metadata').is(':checked')) {
                searchModes.push('metadata');
            }
            
            var serviceType = axconfig.getServiceType(businessObject) || 'LLM';
            var $modelSelect = $('#model-select');
            var selectedId = $modelSelect.val();
            var selectedOpt = selectedId ? $modelSelect.find('option:selected') : null;
            var modelNameFromOpt = selectedOpt && selectedOpt.length ? (selectedOpt.data('model-name') || selectedOpt.data('modelName') || '') : '';
            const configData = {
                ServiceType: serviceType,
                ModelProviderId: (selectedId && selectedId !== '') ? parseInt(selectedId, 10) : null,
                ModelName: (modelNameFromOpt || '').trim() || (selectedOpt ? selectedOpt.text().split(' - ')[0].trim() : '') || null,
                Description: $('#axconfig-description').val(),
                Temperature: parseFloat($('#temperature').val()) || 0,
                MaxTokens: parseInt($('#max-tokens').val()) || 0,
                MemoryTurns: parseInt($('#memory-turns').val()) || 10,
                SystemPrompt: $('#system-prompt').val(),
                UserMessage: $('#user-message-prompt').val(),
                ResponseFormat: $('input[name="format"]:checked').val(),
                Timeout: parseInt($('#timeout').val()) || 0,
                MaxRetries: parseInt($('#max-retries').val()) || 0,
                ErrorHandling: $('#error-handling').val(),
                FallbackAgent: $('#fallback-agent').val(),
                LogLevel: $('#log-level').val(),
                CustomInstructions: $('#custom-instructions').val(),
                RagSearchStrategy: $('#knowledge-search-strategy').val() || 'similarity',
                RagSearchCount: parseInt($('#knowledge-search-count').val()) || null,
                RagSimilarityThreshold: parseFloat($('#similarity-threshold').val()) || null,
                RagSearchMode: searchModes.join(',') || null,
                RagFunction: $('#knowledge-function-name').val().trim() || null,
                RagEmbeddingModelId: ($('#knowledge-embedding-model').val() && $('#knowledge-embedding-model').val() !== '') ? parseInt($('#knowledge-embedding-model').val(), 10) : null,
                RagEmbeddingDimensions: parseInt($('#knowledge-embedding-dimensions').val()) || 1536
            };

            var activityName = $('#activity-name').val();
            if (!activityName) {
                showNotification(kresource.getItem('axconfig.validation.service.name'), 'error');
                return;
            }

            if (!configData.SystemPrompt) {
                showNotification(kresource.getItem('axconfig.validation.system.prompt'), 'error');
                return;
            }

            if (!configData.ModelProviderId || configData.ModelProviderId === null) {
                showNotification(kresource.getItem('axconfig.validation.model.provider.id') || 'Please select an account credential', 'error');
                return;
            }

            if (!configData.ModelName || configData.ModelName.trim() === '') {
                showNotification(kresource.getItem('axconfig.validation.model.name') || 'Model name is required', 'error');
                return;
            }

            saveAxConfig(configData);
        });

        loadAxConfigData();
        loadModelSelectList();
        loadEmbeddingModelList();
    }

    function toggleKnowledgeBaseTab(serviceType) {
        const knowledgeTab = $('.tab[data-tab="knowledge"]');
        const knowledgePane = $('#knowledge-tab');
        
        if (serviceType === 'RAG') {
            knowledgeTab.show();
        } else {
            knowledgeTab.hide();
            if (knowledgeTab.hasClass('active')) {
                knowledgeTab.removeClass('active');
                knowledgePane.removeClass('active');
                $('.tab[data-tab="basic"]').addClass('active');
                $('#basic-tab').addClass('active');
            }
        }
    }

    /**
     * Load AI node configuration data
     */
    function loadAxConfigData() {
        var currentElement = kmain.currentSelectedElement;
        if (currentElement === undefined || currentElement.businessObject === null) {
            return false;
        }
        
        var businessObject = currentElement.businessObject;
        var activityName = businessObject.name;
        var activityId = businessObject.id;

        document.getElementById('activity-name').value = activityName || '';

        var serviceTypeFromNode = axconfig.getServiceType(businessObject);
        if (serviceTypeFromNode) {
            document.getElementById('service-type').value = serviceTypeFromNode;
            toggleKnowledgeBaseTab(serviceTypeFromNode);
        } else {
            document.getElementById('service-type').value = 'LLM';
            toggleKnowledgeBaseTab('LLM');
        }

        var processId = kmain.mxSelectedProcessEntity ? kmain.mxSelectedProcessEntity.ProcessId : null;
        var version = kmain.mxSelectedProcessEntity ? kmain.mxSelectedProcessEntity.Version : null;

        if (processId && version && activityId) {
            axconfigapi.getByProcessVersionActivity(processId, version, activityId, function (result) {
                if (result.Status === 1) {
                    var axConfigEntity = result.Entity;

                    if (axConfigEntity) {
                        axconfig.mcurrentAxConfigEntity = axConfigEntity;
                        try {
                            renderAxConfigData(axConfigEntity);
                        } catch (renderEx) {
                            console.error('renderAxConfigData error:', renderEx);
                            kmsgbox.error((kresource.getItem("axconfigerrormsg") || '') + ' ' + (renderEx && renderEx.message ? renderEx.message : ''));
                        }
                    } else {
                        axconfig.mcurrentAxConfigEntity = null;
                        if (serviceTypeFromNode) {
                            document.getElementById('service-type').value = serviceTypeFromNode;
                            toggleKnowledgeBaseTab(serviceTypeFromNode);
                        }
                    }
                } else {
                    var errMsg = (result && result.Message) ? result.Message : kresource.getItem("axconfigerrormsg");
                    kmsgbox.error(errMsg);
                }
            }, function (xhr, status, error) {
                var errMsg = kresource.getItem("axconfigerrormsg") + (xhr && xhr.responseJSON && xhr.responseJSON.Message ? ': ' + xhr.responseJSON.Message : (error ? ': ' + error : ''));
                kmsgbox.error(errMsg);
            });
        } else {
            if (serviceTypeFromNode) {
                document.getElementById('service-type').value = serviceTypeFromNode;
                toggleKnowledgeBaseTab(serviceTypeFromNode);
            }
        }
    }

    /**
     * Render AI node configuration data to form
     */
    function renderAxConfigData(axConfigEntity) {
        if (!axConfigEntity) return false;

        if (axConfigEntity.ServiceType) {
            document.getElementById('service-type').value = axConfigEntity.ServiceType;
            toggleKnowledgeBaseTab(axConfigEntity.ServiceType);
        } else {
            var currentElement = kmain.currentSelectedElement;
            if (currentElement && currentElement.businessObject) {
                var serviceTypeFromNode = axconfig.getServiceType(currentElement.businessObject);
                if (serviceTypeFromNode) {
                    document.getElementById('service-type').value = serviceTypeFromNode;
                    toggleKnowledgeBaseTab(serviceTypeFromNode);
                } else {
                    document.getElementById('service-type').value = 'LLM';
                    toggleKnowledgeBaseTab('LLM');
                }
            } else {
                document.getElementById('service-type').value = 'LLM';
                toggleKnowledgeBaseTab('LLM');
            }
        }
        document.getElementById('axconfig-description').value = axConfigEntity.Description || '';

        var modelSelect = document.getElementById('model-select');
        if (modelSelect) {
            if (axConfigEntity.ModelProviderId) {
                var modelId = axConfigEntity.ModelProviderId;
                var opt = modelSelect.querySelector('option[value="' + modelId + '"]');
                modelSelect.value = opt ? String(modelId) : '';
            } else {
                modelSelect.value = '';
            }
        }
        if (axConfigEntity.Temperature !== undefined && axConfigEntity.Temperature !== null) {
            document.getElementById('temperature').value = axConfigEntity.Temperature;
            var sliderValueElement = document.querySelector('.slider-value');
            if (sliderValueElement) {
                sliderValueElement.textContent = axConfigEntity.Temperature;
            }
        }
        document.getElementById('max-tokens').value = axConfigEntity.MaxTokens || '';
        document.getElementById('memory-turns').value = (axConfigEntity.MemoryTurns !== undefined && axConfigEntity.MemoryTurns !== null) ? axConfigEntity.MemoryTurns : 10;
        if (axConfigEntity.ResponseFormat) {
            const formatRadio = document.querySelector(`input[name="format"][value="${axConfigEntity.ResponseFormat}"]`);
            if (formatRadio) {
                formatRadio.checked = true;
            }
        }

        document.getElementById('system-prompt').value = axConfigEntity.SystemPrompt || '';
        document.getElementById('user-message-prompt').value = axConfigEntity.UserMessage || '';

        document.getElementById('timeout').value = axConfigEntity.Timeout || '';
        document.getElementById('max-retries').value = axConfigEntity.MaxRetries || '';
        document.getElementById('error-handling').value = axConfigEntity.ErrorHandling || 'retry';
        document.getElementById('fallback-agent').value = axConfigEntity.FallbackAgent || '';
        document.getElementById('log-level').value = axConfigEntity.LogLevel || 'warn';
        document.getElementById('custom-instructions').value = axConfigEntity.CustomInstructions || '';

        var embeddingSelect = document.getElementById('knowledge-embedding-model');
        if (embeddingSelect) {
            if (axConfigEntity.RagEmbeddingModelId) {
                var opt = embeddingSelect.querySelector('option[value="' + axConfigEntity.RagEmbeddingModelId + '"]');
                embeddingSelect.value = opt ? String(axConfigEntity.RagEmbeddingModelId) : '';
            } else {
                embeddingSelect.value = '';
            }
        }
        var embDimEl = document.getElementById('knowledge-embedding-dimensions');
        if (embDimEl) {
            embDimEl.value = (axConfigEntity.RagEmbeddingDimensions !== undefined && axConfigEntity.RagEmbeddingDimensions !== null) ? axConfigEntity.RagEmbeddingDimensions : '1536';
        }
        var searchStrategyEl = document.getElementById('knowledge-search-strategy');
        if (searchStrategyEl) searchStrategyEl.value = 'similarity';
        var searchCountEl = document.getElementById('knowledge-search-count');
        if (searchCountEl && axConfigEntity.RagSearchCount !== undefined && axConfigEntity.RagSearchCount !== null) {
            searchCountEl.value = axConfigEntity.RagSearchCount;
        }
        if (axConfigEntity.RagSimilarityThreshold !== undefined && axConfigEntity.RagSimilarityThreshold !== null) {
            document.getElementById('similarity-threshold').value = axConfigEntity.RagSimilarityThreshold;
            var similarityValueElement = document.getElementById('similarity-value');
            if (similarityValueElement) {
                similarityValueElement.textContent = parseFloat(axConfigEntity.RagSimilarityThreshold).toFixed(2);
            }
        }
        if (axConfigEntity.RagSearchMode) {
            const modes = axConfigEntity.RagSearchMode.split(',');
            var semEl = document.getElementById('search-mode-semantic');
            if (semEl) semEl.checked = modes.includes('semantic');
            var kwEl = document.getElementById('search-mode-keyword');
            if (kwEl) kwEl.checked = modes.includes('keyword');
            var metaEl = document.getElementById('search-mode-metadata');
            if (metaEl) metaEl.checked = modes.includes('metadata');
        }
        var fnEl = document.getElementById('knowledge-function-name');
        if (fnEl && axConfigEntity.RagFunction) fnEl.value = axConfigEntity.RagFunction;
    }

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

    /**
     * Save AI node configuration
     */
    function saveAxConfig(configData) {
        var currentElement = kmain.currentSelectedElement;
        if (!currentElement || !currentElement.businessObject) {
            showNotification(kresource.getItem('axconfig.error.no.element'), 'error');
            return;
        }

        var businessObject = currentElement.businessObject;
        var activityId = businessObject.id;

        var axConfigEntity = axconfig.mcurrentAxConfigEntity;
        if (axConfigEntity === null || axConfigEntity === undefined) {
            axConfigEntity = {};
        }

        axConfigEntity.ProcessId = kmain.mxSelectedProcessEntity.ProcessId;
        axConfigEntity.Version = kmain.mxSelectedProcessEntity.Version;
        axConfigEntity.ActivityId = activityId;

        axConfigEntity.ServiceType = configData.ServiceType || 'LLM';
        axConfigEntity.ModelProviderId = configData.ModelProviderId;
        axConfigEntity.Description = configData.Description;
        axConfigEntity.Temperature = configData.Temperature;
        axConfigEntity.MaxTokens = configData.MaxTokens;
        axConfigEntity.SystemPrompt = configData.SystemPrompt;
        axConfigEntity.UserMessage = configData.UserMessage;
        axConfigEntity.ResponseFormat = configData.ResponseFormat;
        axConfigEntity.Timeout = configData.Timeout;
        axConfigEntity.MaxRetries = configData.MaxRetries;
        axConfigEntity.MemoryTurns = configData.MemoryTurns !== undefined && configData.MemoryTurns !== null ? configData.MemoryTurns : 10;
        axConfigEntity.ErrorHandling = configData.ErrorHandling;
        axConfigEntity.FallbackAgent = configData.FallbackAgent;
        axConfigEntity.LogLevel = configData.LogLevel;
        axConfigEntity.CustomInstructions = configData.CustomInstructions;
        axConfigEntity.ModelName = configData.ModelName;
        axConfigEntity.RagSearchStrategy = configData.RagSearchStrategy || 'similarity';
        axConfigEntity.RagSearchCount = configData.RagSearchCount;
        axConfigEntity.RagSimilarityThreshold = configData.RagSimilarityThreshold;
        axConfigEntity.RagSearchMode = configData.RagSearchMode;
        axConfigEntity.RagFunction = configData.RagFunction;
        axConfigEntity.RagEmbeddingModelId = configData.RagEmbeddingModelId || null;
        axConfigEntity.RagEmbeddingDimensions = configData.RagEmbeddingDimensions !== undefined && configData.RagEmbeddingDimensions !== null ? configData.RagEmbeddingDimensions : 1536;

        axconfigapi.save(axConfigEntity, function (result) {
            if (result.Status === 1) {
                if (result.Entity) {
                    axconfig.mcurrentAxConfigEntity = result.Entity;
                }
                showNotification(kresource.getItem('axconfig.save.success') || 'Configuration saved successfully', 'success');
                
                try {
                    var $modal = $('#save-btn').closest('.modal');
                    if ($modal && $modal.length && typeof $modal.modal === 'function') {
                        $modal.modal('hide');
                    } else if (typeof window.BootstrapDialog !== 'undefined' &&
                               typeof window.BootstrapDialog.closeAll === 'function') {
                        window.BootstrapDialog.closeAll();
                    }
                } catch (e) {
                    console && console.warn && console.warn('Close ai node property dialog failed:', e);
                }
            } else {
                kmsgbox.error(result.Message);
            }
        });
    }

    /**
     * Get service type from business object
     */
    axconfig.getServiceType = function (businessObject) {
        var aiServiceConfig = getAIServiceConfig(businessObject);
        if (aiServiceConfig && aiServiceConfig.type) {
            return aiServiceConfig.type;
        }

        if (businessObject && businessObject.type) {
            return businessObject.type;
        }

        return null;
    }

    /**
     * Get AI service configuration from business object
     */
    function getAIServiceConfig(businessObject) {
        if (!businessObject) {
            return null;
        }

        if (!businessObject.extensionElements) {
            return null;
        }

        var extensionElements = businessObject.extensionElements;

        if (!extensionElements.values || extensionElements.values.length === 0) {
            return null;
        }

        var aiServices = extensionElements.values.find(function (value) {
            return value && value.$type === 'sf:AiServices';
        });

        if (!aiServices) {
            return null;
        }

        if (aiServices.aiServices && aiServices.aiServices.length > 0) {
            var aiService = aiServices.aiServices[0];
            return aiService;
        }

        return null;
    }



    /**
     * Load model select list (ai_model_provider for LLM/RAG node)
     */
    function loadModelSelectList() {
        axconfigapi.getModelList(function (result) {
            if (result.Status === 1 && result.Entity && Array.isArray(result.Entity)) {
                var selectEl = document.getElementById('model-select');
                if (!selectEl) return;
                var currentVal = selectEl.value;
                selectEl.innerHTML = '<option value="">-- <span class="lang" as="selectplaceholder"></span> --</option>';
                var entity = axconfig.mcurrentAxConfigEntity;
                result.Entity.forEach(function (model) {
                    var idVal = model.ID || model.Id || model.id;
                    if (idVal === undefined || idVal === null) return;
                    var modelName = (model.ModelName || model.modelName || '').trim();
                    var provider = (model.ModelProvider || model.modelProvider || '').trim();
                    var displayText = modelName ? (modelName + (provider ? ' - ' + provider : '')) : (provider || 'Unnamed');
                    var option = document.createElement('option');
                    option.value = idVal;
                    option.textContent = displayText;
                    $(option).data('model-name', modelName || provider || '');
                    selectEl.appendChild(option);
                });
                if (currentVal) selectEl.value = currentVal;
                else if (entity && entity.ModelProviderId) {
                    var opt = selectEl.querySelector('option[value="' + entity.ModelProviderId + '"]');
                    if (opt) selectEl.value = String(entity.ModelProviderId);
                }
                if (typeof kresource !== 'undefined' && kresource.localize) kresource.localize();
            }
        });
    }

    /**
     * Load embedding model list (vector_model type from ai_model_provider)
     */
    function loadEmbeddingModelList() {
        axconfigapi.getEmbeddingModelList(function (result) {
            if (result.Status === 1 && result.Entity && Array.isArray(result.Entity)) {
                const selectEl = document.getElementById('knowledge-embedding-model');
                if (!selectEl) return;
                selectEl.innerHTML = '<option value="">-- <span class="lang" as="selectplaceholder"></span> --</option>';
                result.Entity.forEach(function (model) {
                    const idVal = model.ID || model.Id || model.id;
                    if (idVal === undefined || idVal === null) return;
                    const option = document.createElement('option');
                    option.value = idVal;
                    option.textContent = (model.ModelProvider || model.modelProvider || '') + (model.Description ? ' - ' + model.Description : '');
                    selectEl.appendChild(option);
                });
                var entity = axconfig.mcurrentAxConfigEntity;
                var targetId = entity && (entity.RagEmbeddingModelId || entity.ragEmbeddingModelId);
                if (targetId) {
                    var opt = selectEl.querySelector('option[value="' + targetId + '"]');
                    if (opt) selectEl.value = String(targetId);
                }
                if (typeof kresource !== 'undefined' && kresource.localize) kresource.localize();
            }
        });
    }

    /**
     * Load account credential list
     */
    function loadAccountCredentialList() {
        axconfigapi.getModelList(function (result) {
            if (result.Status === 1 && result.Entity && Array.isArray(result.Entity)) {
                const credentialSelect = document.getElementById('account-credential');
                const currentValue = credentialSelect.value;
                const existingEntity = axconfig.mcurrentAxConfigEntity;
                const targetId = currentValue ||
                    (existingEntity && (existingEntity.ModelProviderId || existingEntity.modelProviderId || existingEntity.model_provider_id)) ||
                    '';
                
                credentialSelect.innerHTML = '<option value="">-- <span class="lang" as="selectplaceholder"></span> --</option>';
                
                result.Entity.forEach(function (model) {
                    const option = document.createElement('option');
                    const idVal = model.ID || model.Id || model.id;
                    if (idVal === undefined || idVal === null) return;
                    option.value = idVal;
                    option.textContent = model.ModelProvider || model.modelProvider || model.model_provider || 'Unnamed';
                    $(option).data('provider', option.textContent);
                    credentialSelect.appendChild(option);
                });
                
                if (targetId) {
                    credentialSelect.value = targetId;
                    $('#btn-edit-credential').prop('disabled', false);
                } else {
                    $('#btn-edit-credential').prop('disabled', true);
                }
                
                if (typeof kresource !== 'undefined' && kresource.localize) {
                    kresource.localize();
                }
            }
        });
    }

    /**
     * Open setting page dialog
     */
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
                    
                    var checkCount = 0;
                    var maxChecks = 50;
                    var checkInterval = setInterval(function () {
                        checkCount++;
                        if (typeof window.setting !== 'undefined' && 
                            typeof window.setting.init === 'function' &&
                            $('#popupSetting #save-btn').length > 0) {
                            
                            if (!$('#popupSetting #save-btn').data('initialized')) {
                                try {
                                    window.setting.init();
                                    $('#popupSetting #save-btn').data('initialized', true);
                                } catch (e) {
                                    console.error("Error initializing setting:", e);
                                }
                            }
                            
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
                if (callback && typeof callback === 'function') {
                    callback();
                }
            },
            draggable: true
        });
    }

    axconfig.delete = function (processId, version, activityId) {
        axconfigapi.delete(processId, version, activityId, function (result) {
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
