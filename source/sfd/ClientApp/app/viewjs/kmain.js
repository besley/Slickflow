import jquery from 'jquery';

import { debounce } from 'min-dash';

import slick from '../script/slick.event.js'
window.slick = slick;

import jshelper from '../script/jshelper.js'
window.jshelper = jshelper;

import processlist from '../viewjs/processlist.js'
window.processlist = processlist;

import rolelist from '../viewjs/rolelist.js'
window.rolelist = rolelist;

import userlist from '../viewjs/userlist.js'
window.userlist = userlist;

import kresource from '../viewjs/kresource.js'
window.kresource = kresource;

import kmsgbox from '../script/kmsgbox.js'
window.kmsgbox = kmsgbox;

import processapi from '../viewjs/processapi.js'
window.processapi = processapi;

const kmain = (function () {
    function kmain() {

    }

    kmain.mProcessID = 0;
    kmain.mProcessGUID = '';
    kmain.mProcessVersion = '';
    kmain.mBpmnModeler = null;
    kmain.mxSelectedProcessEntity = null;
    kmain.mxImageUrl = {};

    kmain.init = function (modeler) {
        //waiting...
        showProgressBar();

        kmain.mBpmnModeler = modeler;

        //register process opened event
        if ("undefined" !== typeof processlist) {
            processlist.afterCreated.subscribe(afterProcessCreated);
            processlist.afterOpened.subscribe(afterProcessOpened);
            processlist.diagramCreated.subscribe(diagramProcessCreated);
        }

        initializeGlobalVariables();
    }

    //#region preparation
    function showProgressBar() {
        $(".progress-bar").animate({
            width: "70%",
        }, 200);

        setTimeout(function () {
            $(".progress").hide();
        }, 1000);
    }
	//#endregion

    var initializeGlobalVariables = function () {
        kmain.mxSelectedProcessEntity = null;

        kmain.mxImageUrl['sequence'] = 'slickflow-sequence-dailywork-demo.gif';
        kmain.mxImageUrl['andsplit'] = 'slickflow-andsplit-demo.gif';
        kmain.mxImageUrl['orsplit'] = 'slickflow-orsplit-ordervip-demo.gif';
        kmain.mxImageUrl['xorsplit'] = 'slickflow-xorsplit-askforleave-demo.gif';
        kmain.mxImageUrl['approvalorsplit'] = 'slickflow-approvalorsplit-vacation-demo.gif';
        kmain.mxImageUrl['eorjoinbranchcount'] = 'slickflow-eorjoin-branchcount-demo.gif';
        kmain.mxImageUrl['eorjoinbranchforced'] = 'slickflow-eorjoin-branchforced-demo.gif';
        kmain.mxImageUrl['misequence'] = 'slickflow-multiple-sequence-demo.gif';
        kmain.mxImageUrl['startconditional'] = 'slickflow-start-conditionial-demo.gif';
        kmain.mxImageUrl['startsplit'] = 'slickflow-start-split-demo.gif';
    }


    kmain.createNewDiagram = function () {
        kmain.mxSelectedProcessEntity = null;
        processapi.InitNewBPMNFile(function (result) {
            if (result.Entity !== null) {
                kmain.mxSelectedProcessEntity = result.Entity;
                var diagramXML = result.Entity.XmlContent;
                openDiagram(diagramXML);
            }
        })
    }

    kmain.openDiagramFile = function (xml) {
        openDiagram(xml);
    }

    async function openDiagram(xml) {

        try {
            var container = $('#js-drop-zone');
            await kmain.mBpmnModeler.importXML(xml);

            container
                .removeClass('with-error')
                .addClass('with-diagram');
        } catch (err) {

            container
                .removeClass('with-diagram')
                .addClass('with-error');

            container.find('.error pre').text(err.message);

            console.error(err);
        }
    }

    kmain.openProcess = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupContent"></div>'),
            title: kresource.getItem("processlist"),
            onshown: function () {
                $("#popupContent").load('pages/process/list.html')
            },
            draggable: true
        });
    }

    kmain.saveProcessFile = function (processEntity, xml) {
        const canvas = kmain.mBpmnModeler.get("canvas");
        const rootElement = canvas.getRootElement();
        const processName = rootElement.businessObject.name;
        const processCode = rootElement.businessObject.code;

        //need to be sure wether the sub process node modifed
        if (rootElement.type !== "bpmn:SubProcess"
            && processName !== processEntity.ProcessName) {
            //the main process info is modified, need to be saved
            var content = kresource.getItem('processnamechangedconfirmmsg');
            kmsgbox.confirm(content, function () {
                var entity = {
                    "ProcessGUID": processEntity.ProcessGUID,
                    "ProcessName": processName,
                    "ProcessCode": processCode,
                    "Version": processEntity.Version,
                    "IsUsing": processEntity.IsUsing,
                    "XmlContent": xml
                };
                processapi.saveProcessFile(entity, function (result) {
                    if (result.Status === 1) {
                        kmsgbox.info(kresource.getItem("processxmlsaveokmsg"));
                    } else {
                        kmsgbox.error(kresource.getItem("processxmlsaveerrormsg"), result.Message);
                    }
                });
            });
        } else {
            var existEntity = {
                "ProcessGUID": processEntity.ProcessGUID,
                "ProcessName": processEntity.ProcessName,
                "ProcessCode": processEntity.ProcessCode,
                "Version": processEntity.Version,
                "IsUsing": processEntity.IsUsing,
                "XmlContent": xml
            };
            processapi.saveProcessFile(existEntity, function (result) {
                if (result.Status === 1) {
                    kmsgbox.info(kresource.getItem("processxmlsaveokmsg"));
                } else {
                    kmsgbox.error(kresource.getItem("processxmlsaveerrormsg"), result.Message);
                }
            });
        }
    }

    function diagramProcessCreated(e, data) {
        initializeGlobalVariables();
        kmain.mxSelectedProcessEntity = data.ProcessEntity;
        kmain.mBpmnModeler.saveXML({ format: true }).then(function (result) {
            var xmlContent = result.xml;
            if (data.ProcessEntity !== null) {
                var entity = {
                    "ProcessGUID": kmain.mxSelectedProcessEntity.ProcessGUID,
                    "ProcessName": kmain.mxSelectedProcessEntity.ProcessName,
                    "ProcessCode": kmain.mxSelectedProcessEntity.ProcessCode,
                    "Version": kmain.mxSelectedProcessEntity.Version,
                    "XmlContent": xmlContent
                };
                processapi.saveProcessFile(entity, function (result) {
                    if (result.Status !== 1) {
                        kmsgbox.error(kresource.getItem("processxmlsaveerrormsg"), kmsgbox.result.Message);
                    } else {
                        kmsgbox.info(kresource.getItem("processxmlsaveokmsg"));
                    }
                });
            } else {
                kmsgbox.error(kresource.getItem("processxmlsaveerrormsg"));
            }
        });
    }

    function afterProcessCreated(e, data) {
        //intialize process variables
        initializeGlobalVariables();
    }

    function afterProcessOpened(e, data) {
        //intialize process variables
        initializeGlobalVariables();

        var query = {
            "ProcessGUID": data.ProcessEntity.ProcessGUID,
            "Version": data.ProcessEntity.Version
        };

        processapi.queryProcessFile(query, function (result) {
            if (result.Status === 1) {
                //bpmnjs load xml
                var entity = result.Entity;
                kmain.mxSelectedProcessEntity = entity;
                processlist.mxSelectedProcessEntity = entity;
                //open diagram
                openDiagram(entity.XmlContent);
            } else {
                kmsgbox.error(kresource.getItem('processopenerrormsg'), result.Message);
            }
        });
    }

    //xml of slickflow specifictaion
    kmain.previewXml = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupXmlContent"></div>'),
            title: kresource.getItem("content"),
            onshown: function () {
                $("#popupXmlContent").load('pages/process/xmlcontent.html')
            },
            draggable: true
        });
    }

    function download(name, data) {
        //creating an invisible element
        var element = document.createElement('a');
        element.setAttribute('href',
            'data:application/bpmn20-xml;charset=UTF-8,'
            + encodeURIComponent(data));
        element.setAttribute('download', name);

        document.body.appendChild(element);

        //onClick property
        element.click();

        document.body.removeChild(element);
    }

    kmain.downloadXml = debounce(async function () {
        try {

            const { xml } = await this.mBpmnModeler.saveXML({ format: true });

            download('diagram.bpmn', xml);

        } catch (err) {

            console.error('Error happened saving diagram: ', err);

            download('diagram.bpmn', null);
        }
    }, 500)

    kmain.downloadSvg = debounce(async function () {
        try {

            const { svg } = await this.mBpmnModeler.saveSVG();

            download('diagram.svg', svg);
        } catch (err) {

            console.error('Error happened saving SVG: ', err);

            download('diagram.svg', null);
        }
    }, 500)

    kmain.importDiagram = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupImportXml"></div>'),
            title: kresource.getItem("importxml"),
            onshown: function () {
                $("#popupImportXml").load('pages/process/import.html')
            },
            draggable: true
        });
    }

    kmain.importMxGraphDiagram = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupImportMxGraphXml"></div>'),
            title: kresource.getItem("importmxgraphxml"),
            onshown: function () {
                $("#popupImportMxGraphXml").load('pages/process/importmxgraph.html')
            },
            draggable: true
        });
    }

    kmain.validateProcess = function (entity, xml) {
        var vEntity = {
            "ProcessGUID": entity.ProcessGUID,
            "Version": entity.Version,
            "XmlContent": xml
        };

        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/ValidateProcess', JSON.stringify(vEntity), function (result) {
            if (result.Status === 1) {
                var validateResult = result.Entity;
                var resultType = validateResult.ProcessValidatedResultType;
                if (resultType === "None") {
                    kmsgbox.info(kresource.getItem('processvalidateokmsg'));
                } else if (resultType === "Successed") {
                    kmsgbox.info(kresource.getItem('processvalidateresult_type_successed'));
                } else if (resultType === "WithoutStartEvent") {
                    kmsgbox.warn(kresource.getItem('processvalidateresult_type_withoutstartevent'));
                } else if (resultType === "WithoutEndEvent") {
                    kmsgbox.warn(kresource.getItem('processvalidateresult_type_withoutendevent'));
                } else if (resultType === "WithoutStartEndPath") {
                    kmsgbox.warn(kresource.getItem('processvalidateresult_type_withoutstartendpath'));
                } 
            } else {
                kmsgbox.error(kresource.getItem('processvalidateexceptionmsg'), result.Message);
            }
        });
    }

    //#region domain lang and step test
    kmain.codeStudio = function () {
        var win = window.open("pages/model/index.html", '_blank');
        win.focus();
    }

    kmain.createByTemplate = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        kmain.templateDialog = BootstrapDialog.show({
            size: BootstrapDialog.SIZE_LARGE,
            closable: true,
            message: $('<div id="openTemplateGallery"></div>'),
            title: kresource.getItem("templategallery"),
            onshown: function () {
                $("#openTemplateGallery").load('pages/template/index.html')
            },
            draggable: true
        });
    }

    kmain.gotoTutorial = function () {
        var lang = kresource.getLang();
        var url = (lang === "zh") ? "https://www.cnblogs.com/slickflow/p/11936786.html"
            : "https://www.codeproject.com/Articles/5252483/Slickflow-Coding-Graphic-Model-User-Manual";
        window.open(url, "_blank");
    }

    kmain.simuTest = function () {
        var win = window.open(kconfig.webTestUrl, '_blank');
        win.focus();
    }

    kmain.openDemo = function (pattern) {
        var win = window.open(kconfig.demoUrl + '/demo/' + kmain.mxImageUrl[pattern], '_blank');
        win.focus();
    }

    kmain.webJob = function () {
        var win = window.open("http://localhost/sfj2/", '_blank');
        win.focus();
    }

    kmain.changeLang = function (lang) {
        kresource.setLang(lang);
        window.location.reload();
    }

    kmain.openHelpPage = function () {
        var win = null;
        var lang = kresource.getLang();
        if (lang === 'zh') {
            win = window.open("http://doc.slickflow.com/", '_blank');
        } else {
            win = window.open("http://doc.slickflow.net/", '_blank');
        }
        win.focus();
    }

    function setMxGraphLang(lang) {
        if (lang === undefined || lang === null) lang = kresource.getLang();

        mxLanguage = lang;
        mxClient.language = lang;
    }
    //#endregion

    return kmain;
})()

export default kmain;


