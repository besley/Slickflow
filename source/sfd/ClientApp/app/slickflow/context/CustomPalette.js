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

        // 创建插件节点的工厂函数
        function createPluginTask() {
            return elementFactory.createShape({
                type: 'bpmn:ServiceTask',
                name: 'PlugInService',
                sfType: 'PlugIn'
            });
        }

        return {
            'llm-service-separator': {
                group: 'activity',
                separator: true
            },
            'create.llm-service': {
                group: 'activity',
                className: 'bpmn-icon-service-task llm-service-task',
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
            'create.plugin-service': {
                group: 'activity',
                className: 'bpmn-icon-service-task plugin-service-task',
                title: translate('plugin'),
                action: {
                    dragstart: function (event) {
                        var taskShape = createPluginTask();
                        create.start(event, taskShape);
                    },
                    click: function (event) {
                        var taskShape = createPluginTask();
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