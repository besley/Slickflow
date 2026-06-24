import { is, getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';
import BaseRenderer from 'diagram-js/lib/draw/BaseRenderer';
import { append as svgAppend, create as svgCreate, attr as svgAttr } from 'tiny-svg';

const HIGH_PRIORITY = 1500;

const ICON_COLOR = 'hsl(225, 10%, 35%)';

const AI_ICON = {
    LLM:   '',
    RAG:   '',
    Agent: ''
};

const AI_TYPES = ['LLM', 'RAG', 'Agent'];

// Resolve sfType from element.sfType (palette drag) or from extension elements (XML load)
function getSfType(element) {
    if (AI_TYPES.indexOf(element.sfType) !== -1) {
        return element.sfType;
    }
    var bo = getBusinessObject(element);
    var values = bo && bo.extensionElements && bo.extensionElements.values;
    if (!values) return null;
    var aiServices = values.find(function(v) {
        return v.$type === 'sf:AiServices' && Array.isArray(v.aiServices) && v.aiServices.length > 0;
    });
    if (!aiServices) return null;
    var type = aiServices.aiServices[0].type;
    return AI_TYPES.indexOf(type) !== -1 ? type : null;
}

export default class SfCustomRenderer extends BaseRenderer {
    constructor(eventBus, bpmnRenderer) {
        super(eventBus, HIGH_PRIORITY);
        this.bpmnRenderer = bpmnRenderer;
    }

    canRender(element) {
        return is(element, 'bpmn:ServiceTask') && !!getSfType(element);
    }

    drawShape(parentNode, element) {
        // Render plain task rectangle (no gear)
        const taskHandler = this.bpmnRenderer._renderer('bpmn:Task');
        const shape = taskHandler(parentNode, element, {});

        // Overlay custom icon at top-left inside the border
        const sfType = getSfType(element);
        const char = AI_ICON[sfType];
        if (char) {
            const icon = svgCreate('text');
            svgAttr(icon, {
                x: 8,
                y: 20,
                style: 'font-family: "Font Awesome 5 Free"; font-weight: 900; font-size: 12px; fill: ' + ICON_COLOR + ';'
            });
            icon.textContent = char;
            svgAppend(parentNode, icon);
        }

        return shape;
    }

    getShapePath(shape) {
        return this.bpmnRenderer.getShapePath(shape);
    }
}

SfCustomRenderer.$inject = ['eventBus', 'bpmnRenderer'];