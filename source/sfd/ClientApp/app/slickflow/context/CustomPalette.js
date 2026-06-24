export default class CustomPalette {
    constructor(create, elementFactory, palette, translate) {
        this.create = create;
        this.elementFactory = elementFactory;
        this.translate = translate;

        palette.registerProvider(this);
    }

    getPaletteEntries(element) {
        const {
            create,
            elementFactory,
            translate
        } = this;

        function createLLMServiceTask() {
            return elementFactory.createShape({
                type: 'bpmn:ServiceTask',
                name: 'LLMService',
                sfType: 'LLM'
            });
        }

        // ??????????????????
        function createRAGTask() {
            return elementFactory.createShape({
                type: 'bpmn:ServiceTask',
                name: 'RAGService',
                sfType: 'RAG'
            });
        }

        function createAgentServiceTask() {
            return elementFactory.createShape({
                type: 'bpmn:ServiceTask',
                name: 'AgentService',
                sfType: 'Agent'
            });
        }

        return {
            'llm-service-separator': {
                group: 'activity',
                separator: true
            },
            'create.llm-service': {
                group: 'activity',
                className: 'bpmn-icon-task llm-service-task',
                title: translate('llm'),
                action: {
                    dragstart: function (event) {
                        var taskShape = createLLMServiceTask();
                        create.start(event, taskShape);
                    },
                    click: function (event) {
                        var taskShape = createLLMServiceTask();
                        create.start(event, taskShape);
                    }
                }
            },
            'create.rag-service': {
                group: 'activity',
                className: 'bpmn-icon-task rag-service-task',
                title: translate('rag'),
                action: {
                    dragstart: function (event) {
                        var taskShape = createRAGTask();
                        create.start(event, taskShape);
                    },
                    click: function (event) {
                        var taskShape = createRAGTask();
                        create.start(event, taskShape);
                    }
                }
            },
            'create.agent-service': {
                group: 'activity',
                className: 'bpmn-icon-task agent-service-task',
                title: translate('agent'),
                action: {
                    dragstart: function (event) {
                        var taskShape = createAgentServiceTask();
                        create.start(event, taskShape);
                    },
                    click: function (event) {
                        var taskShape = createAgentServiceTask();
                        create.start(event, taskShape);
                    }
                }
            }
        }
    }
}

CustomPalette.$inject = [
    'create',
    'elementFactory',
    'palette',
    'translate'
]