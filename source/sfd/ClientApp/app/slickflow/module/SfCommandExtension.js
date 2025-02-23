
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';
import jshelper from "../../script/jshelper";

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
        } else if (element.type === "bpmn:Collaboration") {
            var businessObject = getBusinessObject(element);
            if (businessObject.guid === undefined) {
                var strRandom = jshelper.getRandomString(6);
                modeling.updateProperties(element, {
                    name: "Collaboration_Name_" + strRandom,
                    code: "Collaboration_Code_" + strRandom
                })
            }
        } else if (element.type === "bpmn:Participant") {
            var businessObject = getBusinessObject(element);
            if (businessObject.name === undefined) {
                var strRandom = jshelper.getRandomString(6);
                modeling.updateProperties(element, {
                    name: 'PoolProcess_Name_' + strRandom,
                    code: 'PoolProcess_Code_' + strRandom
                })
            }

            var processRef = businessObject.processRef;
            if (processRef.guid === undefined) {
                var strRandom = jshelper.getRandomString(6);
                processRef.name = 'Process_Name_Child_' + strRandom;
                processRef.code = 'Process_Code_Child' + strRandom;
            }
        } else if (element.type === "bpmn:Process") {
            var businessObject = getBusinessObject(element);
            if (businessObject.name === undefined) {
                var strRandom = jshelper.getRandomString(6);
                modeling.updateProperties(element, {
                    name: 'Process_Name_' + strRandom,
                    code: 'Process_Code_' + strRandom
                })
            }
        }

    });
}

SfCommandExtension.$inject = ["eventBus"];

export default {
    __init__: ["sfCommandExtension"],
    sfCommandExtension: ["type", SfCommandExtension]
};