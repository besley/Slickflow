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

        function createMITask(event) {
            const shape = elementFactory.createShape({ type: 'bpmn:MITask' });
            create.start(event, shape);
        }

        return {
            'create.mi-task': {
                group: 'activity',
                className: 'bpmn-icon-mi-task',
                title: translate('Create MITask'),
                action: {
                    dragstart: createMITask,
                    click:createMITask
                }
            },
        }
    }
}

CustomPalette.$inject = [
    'create',
    'elementFactory',
    'palette',
    'translate'
]