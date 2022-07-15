
function SfCommandExtension(eventBus) {
    eventBus.on('element.click', function (e) {
        var modeling = kmain.mBpmnModeler.get('modeling');
        var element = e.element;
        if (element.type === "bpmn:StartEvent") {
            modeling.updateProperties(element, {
                name: "Start"
            });
        } else if (element.type === "bpmn:EndEvent") {
            modeling.updateProperties(element, {
                name: "End"
            });
        } else if (element.type === "bpmn:Task") {
            ;
        }
    });
}

SfCommandExtension.$inject = ["eventBus"];

export default {
    __init__: ["sfCommandExtension"],
    sfCommandExtension: ["type", SfCommandExtension]
};