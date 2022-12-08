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

        if (processName !== processEntity.ProcessName) {
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
                openDiagram(entity.XmlContent);
                kmain.mxSelectedProcessEntity =entity;
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

        // Above code is equivalent to
        // <a href="path of file" download="file name">

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

    kmain.validateProcess = function () {
        console.log('validate process...')
    }


    //#region domain lang and step test
    kmain.codeStudio = function () {
        var win = window.open("pages/model/index.html", '_blank');
        win.focus();
    }

    kmain.gotoTutorial = function () {
        processmodel.gotoTutorial();
    }

    kmain.simuTest = function () {
        var win = window.open("http://localhost/sfw2/", '_blank');
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

    function setMxGraphLang(lang) {
        if (lang === undefined || lang === null) lang = kresource.getLang();

        mxLanguage = lang;
        mxClient.language = lang;
    }
    //#endregion

    return kmain;
})()

export default kmain;


