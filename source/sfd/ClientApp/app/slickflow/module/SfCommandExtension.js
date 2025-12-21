
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';
import jshelper from "../../script/jshelper";

function SfCommandExtension(eventBus, modeling, moddle) {

    // 在元素创建时初始化属性（一次性操作）
    eventBus.on('shape.added', function (e) {
        var element = e.element;
        // 使用 setTimeout 延迟执行，避免命令栈冲突
        setTimeout(() => {
            initializeElementProperties(element, modeling, moddle);
        }, 0);
    });

    // 监听形状删除事件
    eventBus.on('shape.remove', function (e) {
        var element = e.element;

        // 处理删除逻辑
        //handleElementRemoval(element);
    });

    // 监听连接线删除事件
    eventBus.on('connection.remove', function (e) {
        var element = e.element;
        //handleElementRemoval(element);
    });

    // 在点击时处理交互逻辑
    eventBus.on('element.click', function (e) {
        var element = e.element;
        // 处理点击时的特定逻辑
        //handleElementClick(element, modeling, moddle);
    });
}

// 初始化属性函数 - 在元素创建时调用
function initializeElementProperties(element, modeling, moddle) {
    var businessObject = getBusinessObject(element);

    switch (element.type) {
        case "bpmn:StartEvent":
            if (!businessObject.name) {
                modeling.updateProperties(element, { name: "Start" });
            }
            break;

        case "bpmn:EndEvent":
            if (!businessObject.name) {
                modeling.updateProperties(element, { name: "End" });
            }
            break;

        case "bpmn:ServiceTask":
            initializeServiceTask(element, modeling, moddle);
            break;

        case "bpmn:Collaboration":
            if (!businessObject.guid) {
                var strRandom = jshelper.getRandomString(6);
                modeling.updateProperties(element, {
                    name: "Collaboration_Name_" + strRandom,
                    code: "Collaboration_Code_" + strRandom
                });
            }
            break;

        case "bpmn:Participant":
            if (!businessObject.name) {
                var strRandom = jshelper.getRandomString(6);
                modeling.updateProperties(element, {
                    name: 'PoolProcess_Name_' + strRandom,
                    code: 'PoolProcess_Code_' + strRandom
                });
            }

            // 初始化 ProcessRef
            var processRef = businessObject.processRef;
            if (processRef && !processRef.guid) {
                var strRandom = jshelper.getRandomString(6);
                modeling.updateProperties(element, {
                    processRef: {
                        name: 'Process_Name_Child_' + strRandom,
                        code: 'Process_Code_Child' + strRandom
                    }
                });
            }
            break;

        case "bpmn:Process":
            if (!businessObject.name) {
                var strRandom = jshelper.getRandomString(6);
                modeling.updateProperties(element, {
                    name: 'Process_Name_' + strRandom,
                    code: 'Process_Code_' + strRandom
                });
            }
            break;
    }
}

function initializeServiceTask(element, modeling, moddle) {
    if (element.sfType === "LLM" || element.sfType === "PlugIn") {
        var businessObject = getBusinessObject(element);
        if (!businessObject.extensionElements) {
            var aiService = moddle.create('sf:AIService', {
                configUUID: jshelper.getUUID(),
                type: element.sfType
            })

            var aiServices = moddle.create('sf:AIServices', {
                aiServices:[aiService]
            })

            var extensionElements = moddle.create('bpmn:ExtensionElements', {
                values:[aiServices]
            })

            modeling.updateProperties(element, {
                extensionElements: extensionElements
            });
        } else {
            ;
        }
    }
}

// 点击处理函数 - 在用户交互时调用
function handleElementClick(element, modeling, moddle) {
    // 这里可以处理一些点击时的特殊逻辑
    // 比如：选中时的样式变化、显示详细信息等
    // 示例：点击时记录日志或执行特定操作
    if (element.type === "bpmn:ServiceTask") {
        ;
        //console.log('ServiceTask clicked, current uuid:', getBusinessObject(element).uuid);
    }
}

function handleElementRemoval(element) {
    // 检查是否是 ServiceTask 且有 AI 配置
    if (element.type === 'bpmn:ServiceTask') {
        ;
        //var businessObject = getBusinessObject(element);
        // 从数据库删除对应的配置记录
        //var configUUID = axconfig.getConfigUUID(businessObject);
        //axconfig.delete(configUUID);
    }
}

SfCommandExtension.$inject = ["eventBus", "modeling", "moddle"];

export default {
    __init__: ["sfCommandExtension"],
    sfCommandExtension: ["type", SfCommandExtension]
};