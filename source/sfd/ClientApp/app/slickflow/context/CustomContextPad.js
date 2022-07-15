export default class CustomContextPad {
    constructor(config, contextPad, create, elementFactory, injector, translate) {
        this.create = create;
        this.elementFactory = elementFactory;
        this.translate = translate;

        if (config.autoPlace !== false) {
            this.autoPlace = injector.get('autoPlace', false);
        }

        contextPad.registerProvider(this);
    }

    getContextPadEntries(element) {
        const {
            autoPlace,
            create,
            elementFactory,
            translate
        } = this;

        function appendMITask(event, element) {
            if (autoPlace) {
                const shape = elementFactory.createShape({ type: 'bpmn:MITask' });
                autoPlace.append(element, shape);
            } else {
                appendMITaskStart(event, element);
            }
        }

        function appendMITaskStart(event) {
            const shape = elementFactory.createShape({ type: 'bpmn:MITask' });
            create.start(event, shape, element);
        }

        return {
            'append.mi-task': {
                group: 'model',
                className: 'bpmn-icon-mi-task',
                title: translate('Append MITask'),
                action: {
                    click: appendMITask,
                    dragstart: appendMITaskStart
                }
            }
        };
    }
}

CustomContextPad.$inject = [
    'config',
    'contextPad',
    'create',
    'elementFactory',
    'injector',
    'translate'
]