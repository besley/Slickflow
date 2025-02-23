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

        function createByAI(event) {
            //const shape = elementFactory.createShape({ type: 'bpmn:AI' });
            //create.start(event, shape);
            console.log('hello')
        }

        return {
            'create.ai': {
                group: 'artifact',
                className: 'bpmn-icon-ai',
                title: translate('create ai'),
                action: {
                    dragstart: createByAI,
                    click:createByAI
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