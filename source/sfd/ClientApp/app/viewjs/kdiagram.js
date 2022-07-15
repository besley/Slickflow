import kconfig from '../../config/kconfig.js'
import jshelper from '../script/jshelper.js'

const kdiagram = (function () {
    function kdiagram() {
    }

    kdiagram.loadXml = function (appInstanceID, processGUID, version) {
        var processQuery = {
            ProcessGUID: processGUID,
            Version: version
        };

        var taskQuery = {
            AppInstanceID: appInstanceID,
            ProcessGUID: processGUID,
            Version: version
        };

        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/QueryProcessFile',
            JSON.stringify(processQuery),
            function (result) {
                if (result.Status === 1) {
                    var processEntity = result.Entity;
                    var xml = processEntity.XmlContent;

                    var viewer = new BpmnJS({
                        container: $('#js-canvas'),
                        height: 600
                    });

                    try {
                        const bpmnResult = viewer.importXML(xml);
                        const { warnings } = bpmnResult;
                    } catch (err) {
                        console.log(err.message, err.warnings);
                    }

                    //render running nodes with red color
                    jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/QueryReadyActivityInstance',
                        JSON.stringify(taskQuery), function (result) {
                            if (result.Status === 1) {
                                renderReadyTasks(result.Entity, viewer);
                            } else {
                                kmsgbox.error(kresource.getItem('processdiagramreaderrormsg'), result.Message);
                            }
                    });

                    jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/QueryCompletedTransitionInstance',
                        JSON.stringify(taskQuery), function (result) {
                            if (result.Status === 1) {
                                renderCompletedTransitions(result.Entity, viewer);
                            } else {
                                kmsgbox.error(kresource.getItem('processdiagramreaderrormsg'), result.Message);
                            }
                        });
                } else {
                    kmsgbox.error(kresource.getItem('processdiagramreaderrormsg'), result.Message);
                }
            }
        );
    }

    function renderReadyTasks(activityList, viewer) {
        var elementRegistry = viewer.get('elementRegistry');
        var todoTasks = elementRegistry.filter(function (element) {
            return ((element.type === 'bpmn:Task' || element.type === 'bpmn:UserTask')
                && isReadyTask(activityList, element));
        });
        var modeling = viewer.get('modeling');
        modeling.setColor(todoTasks, {
            stroke: 'yellow',
            fill: 'green'
        });
    }

    function isReadyTask(activityList, element) {
        var isOk = false;
        $.each(activityList, function (idnex, item) {
            if (element.businessObject.$attrs["sf:guid"] === item.ActivityGUID) {
                isOk = true;
                return false;
            }
        })
        return isOk;
    }

    function renderCompletedTransitions(transitionList, viewer) {
        var elementRegistry = viewer.get('elementRegistry');
        var transitions = elementRegistry.filter(function (element) {
            return (element.type === 'bpmn:SequenceFlow'
                && isCompletedTransition(transitionList, element));
        });
        var modeling = viewer.get('modeling');
        modeling.setColor(transitions, {
            stroke: 'red',
            fill: 'yellow'
        });
    }

    function isCompletedTransition(transitionList, element) {
        var isOk = false;
        $.each(transitionList, function (idnex, item) {
            if (element.businessObject.$attrs["sf:guid"] === item.TransitionGUID) {
                isOk = true;
                return false;
            }
        })
        return isOk;
    }

    return kdiagram;
})()

window.kdiagram = kdiagram;

export default kdiagram;