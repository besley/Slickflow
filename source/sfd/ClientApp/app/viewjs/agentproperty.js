import axconfigapi from './axconfigapi.js';

const agentproperty = (function () {
    function agentproperty() { }

    agentproperty.init = function () {
        var currentElement = kmain.currentSelectedElement;
        if (!currentElement || !currentElement.businessObject) return;

        var bo = currentElement.businessObject;
        var activityName = bo.name || 'Agent';
        var activityId   = bo.id;

        // Header title
        var titleEl = document.querySelector('.agent-title');
        if (titleEl) {
            titleEl.innerHTML = '<i class="fas fa-robot"></i> ' + activityName;
        }

        // Agent name input (first text input in basic tab)
        var nameInput = document.querySelector('#basic-tab .form-group:first-child input[type="text"]');
        if (nameInput) nameInput.value = activityName;

        // Activity ID / identifier input
        var idInput = document.querySelector('#basic-tab .form-group:nth-child(2) input[type="text"]');
        if (idInput) idInput.value = activityId || '';

        // Load axconfig from backend
        var pe = kmain.mxSelectedProcessEntity;
        if (!pe || !pe.ProcessId || !pe.Version || !activityId) return;

        axconfigapi.getByProcessVersionActivity(
            pe.ProcessId,
            pe.Version,
            activityId,
            function (result) {
                if (result.Status === 1 && result.Entity) {
                    var cfg = result.Entity;

                    // Description (full-width textarea in basic tab)
                    var descTA = document.querySelector('#basic-tab .form-group.full-width textarea');
                    if (descTA && cfg.Description) descTA.value = cfg.Description;

                    // System prompt (first textarea in prompt tab)
                    var sysPromptTA = document.querySelector('#prompt-tab .form-group:first-child textarea');
                    if (sysPromptTA && cfg.SystemPrompt) sysPromptTA.value = cfg.SystemPrompt;

                    // User message / few-shot (second textarea in prompt tab)
                    var userMsgTA = document.querySelector('#prompt-tab .form-group:nth-child(2) textarea');
                    if (userMsgTA && cfg.UserMessage) userMsgTA.value = cfg.UserMessage;
                }
            },
            function (xhr, status, error) {
                console.warn('[agentproperty] Failed to load config:', error);
            }
        );
    };

    return agentproperty;
})();

export default agentproperty;
