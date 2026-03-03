
import { debounce } from 'min-dash';

import slick from '../script/slick.event.js'
window.slick = slick;

import jshelper from '../script/jshelper.js'
window.jshelper = jshelper;

import kmsgbox from '../script/kmsgbox.js'
window.kmsgbox = kmsgbox;

import { TaskAssistant } from './kaist.js'

import processlist from './processlist.js'
window.processlist = processlist;

import rolelist from './rolelist.js'
window.rolelist = rolelist;

import userlist from './userlist.js'
window.userlist = userlist;

import formlist from './formlist.js'
window.formlist = formlist;

import fieldlist from './fieldlist.js'
window.fieldlist = fieldlist;

import axconfig from './axconfig.js'
window.axconfig = axconfig;

import kresource from './kresource.js'
window.kresource = kresource;

import processapi from './processapi.js'
window.processapi = processapi;

const kmain = (function () {
    function kmain() {

    }

    kmain.mBpmnModeler = null;
    kmain.mxSelectedProcessEntity = null;
    kmain.mxImageUrl = {};
    kmain.mxTaskAssistant = null;
    kmain.mxFirstActivity = null;

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

        //init global variables
        initializeGlobalVariables();

        //init modeler elements click event
        initializeElementEvent();

        //init ai create diagram hintbox
        initializeCreateByAiHintBox();

        //init agent propery page
        initializeBottomPanelActions();
    }

    function initializeElementEvent() {
        var modeler = kmain.mBpmnModeler;
        modeler.on('selection.changed', (event) => {
            const newElement = event.newSelection[0] || null;
            updateElementSelection(newElement);
        });

        modeler.on('element.click', (event) => {
            if (event.element.type === 'bpmn:Process') {
                updateElementSelection(null);
            } else {
                updateElementSelection(event.element);
            }
        });
    }

    function updateElementSelection(element) {
        kmain.currentSelectedElement = element;
        if (element !== null) {
            const aiNodeConfigButton = document.getElementById('aiNodeConfigButton');
            var businessObject = element.businessObject;
            var serviceType = axconfig.getServiceType(businessObject);
            if (serviceType === "LLM" || serviceType === "RAG") {
                aiNodeConfigButton.disabled = false;
                aiNodeConfigButton.classList.remove('disabled');
                aiNodeConfigButton.title = kresource.getItem("aibuttonconfigtitleenabled");
            } else {
                aiNodeConfigButton.disabled = true;
                aiNodeConfigButton.classList.add('disabled');
                aiNodeConfigButton.title = kresource.getItem("aibuttonconfigtitledisabled"); 
            }
        }
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

    var initializeCreateByAiHintBox = function () {
        document.querySelector('.hint-btn').addEventListener('click', function (e) {
            e.stopPropagation();
            const hintBox = document.querySelector('.hint-box');
            hintBox.style.display = hintBox.style.display === 'block' ? 'none' : 'block';
        });

        document.addEventListener('click', function (e) {
            const hintBox = document.querySelector('.hint-box');
            if (hintBox.style.display === 'block' &&
                !e.target.closest('.hint-box') &&
                !e.target.closest('.hint-btn')) {
                hintBox.style.display = 'none';
            }
        });
    }

    var initializeBottomPanelActions = function () {
        //make botton button panel draggable
        makeButtonPanelDraggable();

        //button event
        const aiNodeConfigButton = document.getElementById('aiNodeConfigButton');
        const downloadXmlButton = document.getElementById('downloadXmlButton');
        const downloadSvgButton = document.getElementById('downloadSvgButton');
        const zoomInButton = document.getElementById('zoomInButton');
        const zoomOutButton = document.getElementById('zoomOutButton');
        const tryButton = document.getElementById('tryButton');

        aiNodeConfigButton.addEventListener('click', () => {
            var BootstrapDialog = require('bootstrap5-dialog');
            BootstrapDialog.show({
                message: $('<div id="popupAxConfigProperty"></div>'),
                title: kresource.getItem("axconfig.title"),
                onshown: function (dialog) {
                    $("#popupAxConfigProperty").load('pages/axconfig/edit.html')
                },
                draggable: true
            });
        });

        downloadXmlButton.addEventListener('click', () => {
            kmain.downloadXml();
        });

        downloadSvgButton.addEventListener('click', () => {
            kmain.downloadSvg();
        });

        zoomInButton.addEventListener('click', () => {
            var canvas = kmain.mBpmnModeler.get("canvas");
            const zoom = canvas.zoom();
            canvas.zoom(Math.max(zoom * 0.8, 0.2));
        });

        zoomOutButton.addEventListener('click', () => {
            var canvas = kmain.mBpmnModeler.get("canvas");
            const zoom = canvas.zoom();
            canvas.zoom(Math.min(zoom * 1.2, 4.0));
        });

        tryButton.addEventListener('click', () => {
            kmain.simuTest();
        });
    }

    function makeButtonPanelDraggable() {
        const panel = document.querySelector('.bottom-panel');
        let isDragging = false;
        let startX, startY, initialLeft, initialTop;

        panel.addEventListener('mousedown', (e) => {
            if (e.target.closest('.panel-button')) {
                return;
            }

            isDragging = true;
            startX = e.clientX;
            startY = e.clientY;

            // get current position
            const rect = panel.getBoundingClientRect();
            initialLeft = rect.left;
            initialTop = rect.top;

            // apply css
            panel.style.opacity = '0.7';
            panel.style.cursor = 'grabbing';

            e.preventDefault();
        });

        document.addEventListener('mousemove', (e) => {
            if (!isDragging) return;

            const deltaX = e.clientX - startX;
            const deltaY = e.clientY - startY;

            // update position
            panel.style.left = (initialLeft + deltaX) + 'px';
            panel.style.top = (initialTop + deltaY) + 'px';
            panel.style.transform = 'none'; // remove initial transform
            panel.style.bottom = 'auto'; // remove initial bottom
        });

        document.addEventListener('mouseup', (e) => {
            if (!isDragging) return;

            isDragging = false;
            panel.style.opacity = '1';
            panel.style.cursor = 'move';
        });

        // 防止拖动过程中选择文本
        panel.addEventListener('selectstart', (e) => {
            if (isDragging) {
                e.preventDefault();
            }
        });
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
        setDeepSeekInputPrompt();
    }

    function setDeepSeekInputPrompt() {
        document.getElementById('aiDialog').style.display = 'flex';
        document.getElementById('userInput').focus();
        //show deepseek user input prompt
        document.getElementById("userInput").setAttribute("placeholder", kresource.getItem("deepseekuserinputprompt"));
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    }

    kmain.openDiagramFile = function (xml) {
        openDiagram(xml);
    }

    function openDiagram(xml) {

        try {
            const result = kmain.mBpmnModeler.importXML(xml);
            var container = $('#js-drop-zone');
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

    async function getFirstStepInfo(xml) {        
        const result = await kmain.mBpmnModeler.importXML(xml);
        if (result.warnings.length === 0) {
            //加载节点信息
            const taskAsst = new TaskAssistant();
            taskAsst.initialize(kmain.mBpmnModeler);
            kmain.mxTaskAssistant = taskAsst;

            //获取第一个办理节点信息
            var query = { "ProcessId": kmain.mxSelectedProcessEntity.ProcessId, "Version": kmain.mxSelectedProcessEntity.Version };
            processapi.getFirstStepInfo(query, function (result) {
                if (result.Status === 1) {
                    kmain.mxFirstActivity = result.Entity;
                }
            });
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
        //hide ai-dialog
        document.querySelector('.dialog-container').style.display = 'none';
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
                    "ProcessId": processEntity.ProcessId,
                    "ProcessName": processName,
                    "ProcessCode": processCode,
                    "Version": processEntity.Version,
                    "Status": processEntity.Status,
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
                "ProcessId": processEntity.ProcessId,
                "ProcessName": processEntity.ProcessName,
                "ProcessCode": processEntity.ProcessCode,
                "Version": processEntity.Version,
                "Status": processEntity.Status,
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
                    "ProcessId": kmain.mxSelectedProcessEntity.ProcessId,
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

        var id = data.ProcessEntity.Id;
        kmain.editProcessById(id);
    }

    kmain.editProcessById = function (id) {
        processapi.queryProcessFileById(id, function (result) {
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

            console.log('Error happened saving diagram: ', err);

            download('diagram.bpmn', null);
        }
    }, 500)

    kmain.downloadSvg = debounce(async function () {
        try {

            const { svg } = await this.mBpmnModeler.saveSVG();

            download('diagram.svg', svg);
        } catch (err) {

            console.log('Error happened saving SVG: ', err);

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

    kmain.openSetting = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            message: $('<div id="popupSetting"></div>'),
            title: kresource.getItem("setting"),
            size: BootstrapDialog.SIZE_LARGE,
            cssClass: 'setting-dialog-large',
            onshown: function (dialog) {
                $("#popupSetting").load('pages/setting/index.html')
            },
            draggable: true
        });
    }

    kmain.validateProcess = function (entity, xml) {
        var vEntity = {
            "ProcessId": entity.ProcessId,
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

    kmain.showProgressBar = function () {
        const progressBar = document.getElementById('progressBar');
        progressBar.style.display = 'block';
    }

    kmain.hideProgressBar = function () {
        const progressBar = document.getElementById('progressBar');
        progressBar.style.display = 'none';
    }

    kmain.createByAI = function (message, callback) {
        kmain.showProgressBar();

        var aiRequest = {
            Message: message
        };

        jshelper.ajaxPost(kconfig.webAiUrl + 'api/aigen2demo/CreateProcessByAI', JSON.stringify(aiRequest), function (result) {
            if (result.Status === 1) {
                if (result.Entity !== null) {
                    kmain.mxSelectedProcessEntity = result.Entity;
                    var diagramXML = result.Entity.XmlContent;
                    openDiagram(diagramXML);
                }
            } else {
                kmsgbox.error(kresource.getItem('kmaincreatebyaierrormsg'), result.Message);
            }

            kmain.hideProgressBar();
            callback();
        });
    }

    kmain.openTemplateGallery = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        
        kmain.templateDialog = BootstrapDialog.show({
            size: BootstrapDialog.SIZE_LARGE,
            closable: true,
            message: $('<div id="openTemplateGallery"></div>'),
            title: kresource.getItem("templategallery"),
            onshown: function (dialog) {
                $("#openTemplateGallery").load('pages/template/index.html', function() {
                    // adjust Template Gallery page
                    var $modalDialog = $('#openTemplateGallery').closest('.modal-dialog');
                    if ($modalDialog.length) {
                        $modalDialog.css('max-width', '1200px');
                        $modalDialog.css('width', '90%');
                    }
                    
                    // set focus to Standard Process menu
                    setTimeout(function() {
                        var $standardProcessItem = $('.menuItemTemplate[data-id="standard"]');
                        if ($standardProcessItem.length) {
                            // ensure this menu active
                            $standardProcessItem.addClass('active').siblings().removeClass('active');
                            
                            if (!$standardProcessItem.attr('tabindex')) {
                                $standardProcessItem.attr('tabindex', '0');
                            }
                            
                            $standardProcessItem.focus();
                            
                            // ensure visible
                            var elementTop = $standardProcessItem.offset().top;
                            var elementHeight = $standardProcessItem.outerHeight();
                            var windowTop = $(window).scrollTop();
                            var windowHeight = $(window).height();
                            
                            if (elementTop < windowTop || elementTop + elementHeight > windowTop + windowHeight) {
                                $standardProcessItem[0].scrollIntoView({ behavior: 'smooth', block: 'nearest' });
                            }
                        }
                    }, 200);
                });
            },
            draggable: true
        });
    }

    // Keep backward compatibility
    kmain.createByTemplate = function () {
        kmain.openTemplateGallery();
    }

    kmain.openCodeStudio = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        
        // Clean up previous CodeMirror instance if exists
        // 清理之前的 CodeMirror 实例（如果存在）
        if (kmain.codeMirrorEditor) {
            var editorElement = kmain.codeMirrorEditor.getWrapperElement();
            if (editorElement && editorElement.parentNode) {
                editorElement.parentNode.removeChild(editorElement);
            }
            kmain.codeMirrorEditor = null;
        }
        
        kmain.codeStudioDialog = BootstrapDialog.show({
            size: BootstrapDialog.SIZE_LARGE,
            closable: true,
            message: $('<div id="openCodeStudio"></div>'),
            title: kresource.getItem("codestudio"),
            onshown: function (dialog) {
                $("#openCodeStudio").load('pages/codestudio/index.html', function() {
                    // adjust Code Studio page
                    var $modalDialog = $('#openCodeStudio').closest('.modal-dialog');
                    if ($modalDialog.length) {
                        $modalDialog.css('max-width', '1000px');
                        $modalDialog.css('width', '90%');
                    }
                    
                    // Initialize Code Studio after page is loaded
                    kresource.localize();
                    kmain.initCodeStudio();
                });
            },
            onhidden: function (dialog) {
                // Clean up CodeMirror instance when dialog is closed
                // 对话框关闭时清理 CodeMirror 实例
                if (kmain.codeMirrorEditor) {
                    var editorElement = kmain.codeMirrorEditor.getWrapperElement();
                    if (editorElement && editorElement.parentNode) {
                        editorElement.parentNode.removeChild(editorElement);
                    }
                    kmain.codeMirrorEditor = null;
                }
            },
            draggable: true
        });
    }

    kmain.initCodeStudio = function () {
        // Initialize CodeMirror for C# syntax highlighting
        // 初始化 CodeMirror 以支持 C# 语法高亮
        if (typeof CodeMirror !== 'undefined' && !kmain.codeMirrorEditor) {
            var txtCodeElement = document.getElementById('txtCode');
            if (txtCodeElement) {
                kmain.codeMirrorEditor = CodeMirror.fromTextArea(txtCodeElement, {
                    mode: 'text/x-csharp',  // C# syntax mode
                    theme: 'monokai',        // Dark theme with syntax highlighting
                    lineNumbers: true,       // Show line numbers
                    indentUnit: 4,           // Indent with 4 spaces
                    indentWithTabs: false,   // Use spaces instead of tabs
                    smartIndent: true,       // Smart indentation
                    lineWrapping: true,      // Wrap long lines
                    matchBrackets: true,     // Highlight matching brackets
                    autoCloseBrackets: true, // Auto-close brackets
                    foldGutter: true,        // Enable code folding
                    gutters: ['CodeMirror-linenumbers', 'CodeMirror-foldgutter'],
                    extraKeys: {
                        "Ctrl-Space": "autocomplete",  // Autocomplete with Ctrl+Space
                        "Ctrl-/": "toggleComment",     // Toggle comment with Ctrl+/
                        "Shift-Tab": "indentLess",     // Shift+Tab to unindent
                        "Tab": "indentMore"            // Tab to indent
                    },
                    viewportMargin: Infinity // Allow scrolling beyond end of document
                });

                // Refresh CodeMirror after a short delay to ensure proper rendering
                setTimeout(function() {
                    if (kmain.codeMirrorEditor) {
                        kmain.codeMirrorEditor.refresh();
                    }
                }, 100);
            }
        }

        $("#ddlTemplateType").change(function (i, o) {
            var option = $(this).val();
            //load template content
            kmain.loadCodeStudioTemplate(option);
        });

        //init code mirror textarea
        kmain.loadCodeStudioTemplate('Default');
    }

    kmain.loadCodeStudioTemplate = function (option) {
        processapi.loadTemplate(option, function (template) {
            // Use CodeMirror API if available, otherwise fallback to textarea
            // 如果 CodeMirror 可用则使用其 API，否则回退到 textarea
            if (kmain.codeMirrorEditor) {
                kmain.codeMirrorEditor.setValue(template.Content || '');
                // Refresh to ensure proper rendering
                setTimeout(function() {
                    kmain.codeMirrorEditor.refresh();
                }, 50);
            } else {
                $("#txtCode").val(template.Content);
            }
        });
    }

    kmain.executeCodeStudio = function () {
        // Get code from CodeMirror if available, otherwise from textarea
        // 如果 CodeMirror 可用则从其获取代码，否则从 textarea 获取
        var text = '';
        if (kmain.codeMirrorEditor) {
            text = kmain.codeMirrorEditor.getValue();
        } else {
            var txtCodeElement = document.getElementById('txtCode');
            text = txtCodeElement ? txtCodeElement.value : '';
        }

        if (text !== "") {
            // Security check: Validate namespace requirements
            // 安全检查：验证命名空间要求
            var validationResult = kmain.validateCodeNamespaces(text);
            if (!validationResult.isValid) {
                kmsgbox.warn(validationResult.message || 'Code validation failed: Namespace requirements not met.');
                return;
            }
            
            kmsgbox.showProgressBar();
            var entity = { "Body": text };
            processapi.executeProcessGraph(entity, function (result) {
                if (result.Status === 1) { 
                    var processEntity = result.Entity
                    processlist.pselectedProcessEntity = processEntity;
                    kmain.mxSelectedProcessEntity = processEntity;
                    //render process into graph canvas
                    var xml = processEntity.XmlContent;
                    kmain.openDiagramFile(xml);
                    if (kmain.codeStudioDialog) {
                        kmain.codeStudioDialog.close();
                    }
                } else {
                    kmsgbox.warn(result.Message || kresource.getItem('processopenerrormsg') || 'Failed to execute process graph');
                }
                kmsgbox.hideProgressBar();
            });
        }
        else {
            kmsgbox.warn(kresource.getItem('domainlangwwarnmsg'));
        }
    }

    /**
     * Validate code namespace requirements
     * 验证代码命名空间要求
     * @param {string} codeText - The code text to validate
     * @returns {object} - {isValid: boolean, message: string}
     */
    kmain.validateCodeNamespaces = function (codeText) {
        if (!codeText || typeof codeText !== 'string') {
            return { isValid: false, message: 'Code text is empty or invalid.' };
        }

        // Required namespaces (must be in first two lines)
        // 必需的命名空间（必须在前两行）
        var requiredNamespaces = [
            'using Slickflow.Graph.Model;',
            'using Slickflow.Engine.Common;'
        ];

        // Normalize line endings and split into lines
        // 标准化换行符并分割成行
        var lines = codeText.replace(/\r\n/g, '\n').replace(/\r/g, '\n').split('\n');
        
        // Check first two non-empty lines
        // 检查前两个非空行
        var nonEmptyLines = lines.filter(function(line) {
            return line.trim().length > 0;
        });

        if (nonEmptyLines.length < 2) {
            return { 
                isValid: false, 
                message: 'Code must contain at least two non-empty lines with required namespace declarations.' 
            };
        }

        var firstLine = nonEmptyLines[0].trim();
        var secondLine = nonEmptyLines[1].trim();

        // Check if first two lines contain required namespaces (order may vary)
        // 检查前两行是否包含必需的命名空间（顺序可能不同）
        var hasFirstNamespace = false;
        var hasSecondNamespace = false;

        if (firstLine === requiredNamespaces[0] || firstLine === requiredNamespaces[1]) {
            if (firstLine === requiredNamespaces[0]) {
                hasFirstNamespace = true;
            } else {
                hasSecondNamespace = true;
            }
        }

        if (secondLine === requiredNamespaces[0] || secondLine === requiredNamespaces[1]) {
            if (secondLine === requiredNamespaces[0]) {
                hasFirstNamespace = true;
            } else {
                hasSecondNamespace = true;
            }
        }

        if (!hasFirstNamespace || !hasSecondNamespace) {
            return { 
                isValid: false, 
                message: 'Code must start with the following two namespace declarations:\n' +
                         '  ' + requiredNamespaces[0] + '\n' +
                         '  ' + requiredNamespaces[1] 
            };
        }

        // Check for additional using statements that are not allowed
        // 检查是否有不允许的其他 using 语句
        var allowedNamespaces = [
            'Slickflow.Graph.Model',
            'Slickflow.Engine.Common'
        ];

        var usingPattern = /^\s*using\s+([^;]+);/;
        for (var i = 0; i < lines.length; i++) {
            var line = lines[i].trim();
            var match = line.match(usingPattern);
            if (match) {
                var namespace = match[1].trim();
                var isAllowed = false;
                for (var j = 0; j < allowedNamespaces.length; j++) {
                    if (namespace === allowedNamespaces[j] || namespace.startsWith(allowedNamespaces[j] + '.')) {
                        isAllowed = true;
                        break;
                    }
                }
                if (!isAllowed) {
                    return { 
                        isValid: false, 
                        message: 'Security violation: Unauthorized namespace "' + namespace + '" is not allowed. ' +
                                 'Only namespaces starting with "Slickflow.Graph.Model" or "Slickflow.Engine.Common" are permitted.' 
                    };
                }
            }
        }

        // Security check: Only Workflow methods are allowed
        // 安全检查：只允许 Workflow 方法调用
        var securityCheckResult = kmain.validateOnlyWorkflowMethods(codeText);
        if (!securityCheckResult.isValid) {
            return securityCheckResult;
        }

        return { isValid: true, message: 'Code validation passed.' };
    };

    /**
     * Validate that code only contains Workflow method calls
     * 验证代码只包含 Workflow 方法调用
     * @param {string} codeText - The code text to validate
     * @returns {object} - {isValid: boolean, message: string}
     */
    kmain.validateOnlyWorkflowMethods = function (codeText) {
        // Remove using statements and comments for analysis
        // 移除 using 语句和注释以便分析
        var codeWithoutUsings = codeText
            .replace(/\/\/.*/g, '')  // Remove single-line comments
            .replace(/\/\*[\s\S]*?\*\//g, '')  // Remove multi-line comments
            .replace(/^\s*using\s+[^;]+;\s*$/gm, '');  // Remove using statements

        // 1. Check for System. calls (except allowed namespaces)
        // 检查 System. 调用（除了允许的命名空间）
        var systemCallPattern = /\bSystem\.\w+/gi;
        var systemMatches = codeWithoutUsings.match(systemCallPattern);
        if (systemMatches) {
            for (var i = 0; i < systemMatches.length; i++) {
                var systemCall = systemMatches[i];
                return { 
                    isValid: false, 
                    message: 'Security violation: Unauthorized System call "' + systemCall + '" is not allowed. ' +
                             'Only Workflow method calls are permitted.' 
                };
            }
        }

        // 2. Check for variable declarations other than Workflow declarations
        // 检查除了 Workflow 声明之外的其他变量声明
        var variablePattern = /\b(string|int|bool|double|float|object|var)\s+\w+\s*=/gi;
        var variableMatches = codeWithoutUsings.match(variablePattern);
        if (variableMatches) {
            for (var i = 0; i < variableMatches.length; i++) {
                var variableDecl = variableMatches[i];
                // Check if it's an allowed Workflow declaration
                // 检查是否是允许的 Workflow 声明
                var matchIndex = codeWithoutUsings.indexOf(variableDecl);
                var contextStart = Math.max(0, matchIndex - 100);
                var contextLength = Math.min(200, codeWithoutUsings.length - contextStart);
                var context = codeWithoutUsings.substring(contextStart, contextStart + contextLength);
                
                // Allow new Workflow(...) or Workflow.LoadProcess(...)
                // 允许 new Workflow(...) 或 Workflow.LoadProcess(...)
                if (context.indexOf('new Workflow') === -1 &&
                    context.indexOf('Workflow.LoadProcess') === -1) {
                    return { 
                        isValid: false, 
                        message: 'Security violation: Unauthorized variable declaration "' + variableDecl.trim() + '" is not allowed. ' +
                                 'Only ProcessModelBuilder declarations (CreateProcess or LoadProcess) are permitted.' 
                    };
                }
            }
        }

        // 3. Check for method calls that are not Workflow/NodeBuilder/EdgeBuilder methods
        // 检查不是 Workflow/NodeBuilder/EdgeBuilder 方法的方法调用
        var allowedMethodNames = [
            // Workflow class name (for new Workflow(...) and Workflow.LoadProcess(...))
            'Workflow',
            // Workflow static methods
            'CreateProcess', 'LoadProcess', 'Create',
            // Workflow instance methods
            'Start', 'Task', 'End', 'AndSplit', 'AndJoin',
            'XOrSplit', 'XOrJoin', 'Parallels', 'Branch', 'Connect', 'Build', 'Update',
            'Add', 'Insert', 'Set', 'Replace', 'Exchange', 'Fork', 'Remove',
            'GetBuilder', 'SetUrl', 'SetName', 'Serialize',
            // NodeBuilder methods
            'NodeBuilder', 'CreateTask', 'CreateStart', 'CreateEnd', 'CreateGateway',
            // EdgeBuilder methods
            'EdgeBuilder', 'CreateEdge', 'AddCondition',
            // Variable names
            'wf', 'wf2'
        ];
        
        var methodCallPattern = /\b\w+\s*\(/g;
        var methodMatches = codeWithoutUsings.match(methodCallPattern);
        if (methodMatches) {
            for (var i = 0; i < methodMatches.length; i++) {
                var methodCall = methodMatches[i];
                var methodName = methodCall.replace(/\s*\(/, '').trim();
                
                // Check if it's an allowed method
                // 检查是否是允许的方法
                var isAllowed = false;
                for (var j = 0; j < allowedMethodNames.length; j++) {
                    if (methodName.toLowerCase() === allowedMethodNames[j].toLowerCase()) {
                        isAllowed = true;
                        break;
                    }
                }
                
                if (!isAllowed) {
                    // Check if it's part of a Workflow/NodeBuilder/EdgeBuilder chain
                    // 检查是否是 Workflow/NodeBuilder/EdgeBuilder 链的一部分
                    var matchIndex = codeWithoutUsings.indexOf(methodCall);
                    var beforeMatch = Math.max(0, matchIndex - 50);
                    var contextBefore = codeWithoutUsings.substring(beforeMatch, matchIndex);
                    
                    // Allow methods called on wf, new Workflow(...), Workflow.LoadProcess(...), NodeBuilder, or EdgeBuilder
                    // 允许在 wf、new Workflow(...)、Workflow.LoadProcess(...)、NodeBuilder 或 EdgeBuilder 上调用的方法
                    if (contextBefore.toLowerCase().indexOf('wf') === -1 &&
                        contextBefore.toLowerCase().indexOf('new workflow') === -1 &&
                        contextBefore.toLowerCase().indexOf('workflow.') === -1 &&
                        contextBefore.toLowerCase().indexOf('workflow(') === -1 &&
                        contextBefore.toLowerCase().indexOf('processmodelbuilder') === -1 &&
                        contextBefore.toLowerCase().indexOf('vertexbuilder') === -1 &&
                        contextBefore.toLowerCase().indexOf('linkbuilder') === -1) {
                        return { 
                            isValid: false, 
                            message: 'Security violation: Unauthorized method call "' + methodName + '" is not allowed. ' +
                                     'Only Workflow, NodeBuilder, and EdgeBuilder method calls are permitted.' 
                        };
                    }
                }
            }
        }

        // 4. Check for foreach, for, while, if, etc. control structures
        // 检查 foreach, for, while, if 等控制结构
        // Note: Match "for" only when it's a standalone keyword, not part of "Fork" method name
        // 注意：只匹配独立的 "for" 关键字，不匹配 "Fork" 方法名中的 "for"
        var controlStructurePattern = /\b(foreach|for|while|if|switch|try|catch|finally|using\s*\()/gi;
        var controlMatches = [];
        var regex = new RegExp(controlStructurePattern);
        var match;
        var lastIndex = 0;
        
        while ((match = regex.exec(codeWithoutUsings)) !== null) {
            var matchValue = match[0];
            var matchIndex = match.index;
            
            // Double-check: if matched "for", make sure it's not part of "Fork" method
            // 双重检查：如果匹配到 "for"，确保它不是 "Fork" 方法的一部分
            if (matchValue.toLowerCase() === 'for' && !matchValue.toLowerCase().includes('foreach')) {
                // Check if it's followed by 'k' (part of "Fork")
                // 检查它后面是否是 'k'（"Fork" 的一部分）
                if (matchIndex + matchValue.length < codeWithoutUsings.length) {
                    var nextChar = codeWithoutUsings[matchIndex + matchValue.length];
                    if (/[a-zA-Z]/.test(nextChar) && nextChar.toLowerCase() === 'k') {
                        // This is part of "Fork", skip it
                        // 这是 "Fork" 的一部分，跳过它
                        continue;
                    }
                }
            }
            controlMatches.push({ value: matchValue, index: matchIndex });
        }
        
        if (controlMatches.length > 0) {
            for (var i = 0; i < controlMatches.length; i++) {
                var matchValue = controlMatches[i].value;
                return { 
                    isValid: false, 
                    message: 'Security violation: Control structure "' + matchValue + '" is not allowed. ' +
                             'Only Workflow method calls are permitted.' 
                };
            }
        }

        // 5. Check for class, struct, interface, enum declarations
        // 检查 class, struct, interface, enum 声明
        var typeDeclarationPattern = /\b(class|struct|interface|enum|namespace)\s+\w+/gi;
        var typeMatches = codeWithoutUsings.match(typeDeclarationPattern);
        if (typeMatches) {
            for (var i = 0; i < typeMatches.length; i++) {
                return { 
                    isValid: false, 
                    message: 'Security violation: Type declaration "' + typeMatches[i] + '" is not allowed. ' +
                             'Only Workflow method calls are permitted.' 
                };
            }
        }

        return { isValid: true, message: 'Code validation passed.' };
    };

    kmain.gotoTutorial = function () {
        var lang = kresource.getLang();
        var url = (lang === "zh") ? "https://www.cnblogs.com/slickflow/p/11936786.html"
            : "https://www.codeproject.com/Articles/5252483/Slickflow-Coding-Graphic-Model-User-Manual";
        window.open(url, "_blank");
    }

    kmain.openKnowledgeBase = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            size: BootstrapDialog.SIZE_LARGE,
            closable: true,
            message: $('<div id="knowledgeBaseContent"></div>'),
            title: kresource.getItem("knowledgebase"),
            onshown: function (dialog) {
                $("#knowledgeBaseContent").load('pages/knowledgebase/index.html', function() {
                    // Adjust dialog width
                    var $modalDialog = $('#knowledgeBaseContent').closest('.modal-dialog');
                    if ($modalDialog.length) {
                        $modalDialog.css('max-width', '1100px');
                        $modalDialog.css('width', '85%');
                    }
                });
            },
            draggable: true
        });
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


