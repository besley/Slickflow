
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

        //init ai testdialog
        initializedAiTestDialog();
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
            if (serviceType === "LLM" || serviceType === "PlugIn") {
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
        // 提示框显示/隐藏逻辑
        document.querySelector('.hint-btn').addEventListener('click', function (e) {
            e.stopPropagation();
            const hintBox = document.querySelector('.hint-box');
            hintBox.style.display = hintBox.style.display === 'block' ? 'none' : 'block';
        });

        // 点击外部区域关闭提示框
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
        //底部按钮条可以拖动
        makeButtonPanelDraggable();

        //各类按钮事件
        const aiNodeConfigButton = document.getElementById('aiNodeConfigButton');
        const downloadXmlButton = document.getElementById('downloadXmlButton');
        const downloadSvgButton = document.getElementById('downloadSvgButton');
        const zoomInButton = document.getElementById('zoomInButton');
        const zoomOutButton = document.getElementById('zoomOutButton');

        // 打开属性页框
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

        // 执行下载XML操作
        downloadXmlButton.addEventListener('click', () => {
            kmain.downloadXml();
        });

        // 执行下载SVG操作
        downloadSvgButton.addEventListener('click', () => {
            kmain.downloadSvg();
        });

        // 执行缩小操作
        zoomInButton.addEventListener('click', () => {
            var canvas = kmain.mBpmnModeler.get("canvas");
            const zoom = canvas.zoom();
            canvas.zoom(Math.max(zoom * 0.8, 0.2));
        });

        // 执行放大操作
        zoomOutButton.addEventListener('click', () => {
            var canvas = kmain.mBpmnModeler.get("canvas");
            const zoom = canvas.zoom();
            canvas.zoom(Math.min(zoom * 1.2, 4.0));
        });
    }

    function makeButtonPanelDraggable() {
        const panel = document.querySelector('.bottom-panel');
        let isDragging = false;
        let startX, startY, initialLeft, initialTop;

        panel.addEventListener('mousedown', (e) => {
            // 检查是否点击了按钮，如果是则不允许拖动
            if (e.target.closest('.panel-button')) {
                return;
            }

            isDragging = true;
            startX = e.clientX;
            startY = e.clientY;

            // 获取当前位置
            const rect = panel.getBoundingClientRect();
            initialLeft = rect.left;
            initialTop = rect.top;

            // 应用拖动样式
            panel.style.opacity = '0.7';
            panel.style.cursor = 'grabbing';

            e.preventDefault();
        });

        document.addEventListener('mousemove', (e) => {
            if (!isDragging) return;

            const deltaX = e.clientX - startX;
            const deltaY = e.clientY - startY;

            // 更新位置
            panel.style.left = (initialLeft + deltaX) + 'px';
            panel.style.top = (initialTop + deltaY) + 'px';
            panel.style.transform = 'none'; // 移除初始的 transform
            panel.style.bottom = 'auto'; // 移除初始的 bottom
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

    var initializedAiTestDialog = function () {
        const tryButton = document.getElementById('tryButton');
        const modalAiChatDialog = document.getElementById('modal-aiChatDialog');
        const closeModal = document.getElementById('closeModal');
        const chatContainer = document.getElementById('chatContainer');
        const messageInput = document.getElementById('messageInput');
        const sendBtn = document.getElementById('sendBtn');
        const uploadBtn = document.getElementById('uploadBtn');
        const fileInput = document.getElementById('fileInput');
        const clearBtn = document.getElementById('clearBtn');
        const imageModal = document.getElementById('imageModal');
        const modalImage = document.getElementById('modalImage');
        const closeImageModal = document.getElementById('closeImageModal');

        let messageHistory = [];
        // 打开模态框
        tryButton.addEventListener('click', () => {
            kmain.simuTest();
        });

        // 关闭模态框
        closeModal.addEventListener('click', () => {
            modalAiChatDialog.classList.remove('active');
            setTimeout(() => {
                modalAiChatDialog.style.display = 'none';
            }, 300);
        });

        // 点击模态框背景关闭
        modalAiChatDialog.addEventListener('click', (e) => {
            if (e.target === modalAiChatDialog) {
                modalAiChatDialog.classList.remove('active');
                setTimeout(() => {
                    modalAiChatDialog.style.display = 'none';
                }, 300);
            }
        });

        // 调整文本区域高度
        messageInput.addEventListener('input', function () {
            this.style.height = 'auto';
            this.style.height = (this.scrollHeight) + 'px';
        });

        // 发送消息
        sendBtn.addEventListener('click', sendMessage);
        messageInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                sendMessage();
            }
        });

        // 上传图片
        uploadBtn.addEventListener('click', () => fileInput.click());
        fileInput.addEventListener('change', handleImageUpload);

        // 清除记录
        clearBtn.addEventListener('click', clearChat);

        // 关闭图片模态框
        closeImageModal.addEventListener('click', () => {
            imageModal.classList.remove('active');
        });

        // 点击图片模态框背景关闭
        imageModal.addEventListener('click', (e) => {
            if (e.target === imageModal) {
                imageModal.classList.remove('active');
            }
        });

        function sendMessage() {
            const message = messageInput.value.trim();
            if (message) {
                addMessage(message, 'user');
                messageInput.value = '';
                messageInput.style.height = 'auto';

                // 模拟AI响应
                simulateAIResponse(message);
            }
        }

        function addMessage(content, sender, imageUrl = null) {
            const messageDiv = document.createElement('div');
            messageDiv.className = `message ${sender}-message`;

            const avatarDiv = document.createElement('div');
            avatarDiv.className = `avatar ${sender}-avatar`;
            avatarDiv.innerHTML = sender === 'user' ?
                '<i class="fas fa-user"></i>' :
                '<i class="fas fa-robot"></i>';

            const contentDiv = document.createElement('div');
            contentDiv.className = 'message-content';

            if (imageUrl) {
                const imageContainer = document.createElement('div');
                imageContainer.className = 'uploaded-image-container';

                const img = document.createElement('img');
                img.src = imageUrl;
                img.className = 'uploaded-image';
                img.addEventListener('click', () => openImageModal(imageUrl));

                const zoomButton = document.createElement('button');
                zoomButton.className = 'zoom-btn';
                zoomButton.innerHTML = '<i class="fas fa-expand"></i>';
                zoomButton.addEventListener('click', () => openImageModal(imageUrl));

                imageContainer.appendChild(img);
                imageContainer.appendChild(zoomButton);
                contentDiv.appendChild(imageContainer);
            }

            if (content) {
                const p = document.createElement('p');
                p.textContent = content;
                contentDiv.appendChild(p);
            }

            messageDiv.appendChild(avatarDiv);
            messageDiv.appendChild(contentDiv);

            chatContainer.appendChild(messageDiv);
            chatContainer.scrollTop = chatContainer.scrollHeight;

            // 添加到历史记录
            messageHistory.push({
                sender,
                content,
                imageUrl,
                timestamp: new Date()
            });
        }

        function handleImageUpload(e) {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (event) {
                    addMessage('已上传图片,准备下一步处理!', 'user', event.target.result);
                    var matchedStep = kmain.mxTaskAssistant.findMostSimilarTask("上传图片");
                    if (matchedStep !== null) {
                        var content = kresource.getItem('handlefileuploadedconfirmmsg');

                        console.log(matchedStep);
                        var taskNodeName = matchedStep.task.businessObject.name;

                        //图片上传按钮触发流程启动
                        if (taskNodeName === kmain.mxFirstActivity.ActivityName) {
                            content = "图片上传后触发启动流程操作" + content + taskNodeName;
                            kmsgbox.confirm(content, function () {
                                var runner = {};
                                runner["AppName"] = "Order-Books";
                                runner["AppInstanceId"] = "123";
                                runner["AppInstanceCode"] = "123-code";
                                runner["UserId"] = "01";
                                runner["UserName"] = "Zero";
                                runner["ProcessId"] = kmain.mxSelectedProcessEntity.ProcessId;
                                runner["Version"] = kmain.mxSelectedProcessEntity.Version;

                                processapi.start(runner, function (result) {
                                    if (result.Status === 1) {
                                        kmsgbox.info(kresource.getItem('processstartedokmsg'));
                                    } else {
                                        kmsgbox.error(kresource.getItem('processstartederrormsg'), result.Message);
                                    }
                                })
                            });
                        }
                    }

                    // 模拟AI对图片的响应
                    simulateImageResponse(event.target.result);
                };
                reader.readAsDataURL(file);
            }
            // 清除文件输入值，允许再次选择相同文件
            e.target.value = '';
        }

        function clearChat() {
            // 清除聊天记录
            chatContainer.innerHTML = '';
            messageHistory = [];

            // 添加初始AI消息
            const initialMessage = document.createElement('div');
            initialMessage.className = 'message ai-message';
            initialMessage.innerHTML = `
                <div class="avatar ai-avatar">
                    <i class="fas fa-robot"></i>
                </div>
                <div class="message-content">
                    <p>您好！我是AI助手，您可以输入文本内容或者上传图片文件，然后跟我进行多轮会话式交互。</p>
                </div>
            `;
            chatContainer.appendChild(initialMessage);
        }

        // 打开图片模态框
        function openImageModal(imageUrl) {
            modalImage.src = imageUrl;
            imageModal.classList.add('active');
        }

        function simulateAIResponse(userMessage) {
            // 显示正在输入指示器
            const typingIndicator = document.createElement('div');
            typingIndicator.className = 'message ai-message';
            typingIndicator.innerHTML = `
                <div class="avatar ai-avatar">
                    <i class="fas fa-robot"></i>
                </div>
                <div class="message-content">
                    <div class="typing-indicator">
                        <div class="typing-dot"></div>
                        <div class="typing-dot"></div>
                        <div class="typing-dot"></div>
                    </div>
                </div>
            `;
            chatContainer.appendChild(typingIndicator);
            chatContainer.scrollTop = chatContainer.scrollHeight;

            // 模拟AI处理时间
            setTimeout(() => {
                chatContainer.removeChild(typingIndicator);

                // 根据用户消息生成响应
                let response;
                if (userMessage.toLowerCase().includes('你好') || userMessage.toLowerCase().includes('hi')) {
                    response = '您好！很高兴为您服务。请问有什么我可以帮助您的？';
                } else if (userMessage.toLowerCase().includes('天气')) {
                    response = '目前无法直接获取实时天气信息，但我可以帮您分析天气数据或提供一般性建议。';
                } else if (userMessage.toLowerCase().includes('清除')) {
                    response = '聊天记录已清除，我们可以开始新的对话。';
                } else {
                    response = '感谢您的消息。我已经收到您的请求，正在处理中。';
                }

                addMessage(response, 'ai');

                // 随机模拟Agent执行后续操作
                if (Math.random() > 0.5) {
                    simulateAgentAction();
                }
            }, 1500);
        }

        function simulateImageResponse(imageUrl) {
            // 显示正在输入指示器
            const typingIndicator = document.createElement('div');
            typingIndicator.className = 'message ai-message';
            typingIndicator.innerHTML = `
                <div class="avatar ai-avatar">
                    <i class="fas fa-robot"></i>
                </div>
                <div class="message-content">
                    <div class="typing-indicator">
                        <div class="typing-dot"></div>
                        <div class="typing-dot"></div>
                        <div class="typing-dot"></div>
                    </div>
                </div>
            `;
            chatContainer.appendChild(typingIndicator);
            chatContainer.scrollTop = chatContainer.scrollHeight;

            // 模拟AI处理时间
            setTimeout(() => {
                chatContainer.removeChild(typingIndicator);

                addMessage('感谢分享图片！我已经接收到您上传的图片，正在分析其中的内容。', 'ai');

                // 模拟Agent执行图片相关操作
                simulateImageAgentAction();
            }, 2000);
        }

        function simulateAgentAction() {
            const actions = [
                '正在执行数据查询...',
                '调用分析API处理您的请求...',
                '生成可视化报告...',
                '更新知识图谱...',
                '执行模型训练任务...'
            ];

            const action = actions[Math.floor(Math.random() * actions.length)];

            const actionDiv = document.createElement('div');
            actionDiv.className = 'agent-action';
            actionDiv.innerHTML = `
                <div><strong>Agent操作:</strong> ${action}</div>
                <div style="margin-top: 8px; color: #64748b; font-size: 0.8em;">
                    <i class="fas fa-clock"></i> 已完成于 ${new Date().toLocaleTimeString()}
                </div>
            `;

            const lastMessage = chatContainer.lastChild;
            lastMessage.appendChild(actionDiv);

            chatContainer.scrollTop = chatContainer.scrollHeight;
        }

        function simulateImageAgentAction() {
            const actions = [
                '执行图像识别分析...',
                '提取图像中的关键特征...',
                '与图库进行匹配比较...',
                '生成图像描述...',
                '执行图像增强处理...'
            ];

            const action = actions[Math.floor(Math.random() * actions.length)];

            const actionDiv = document.createElement('div');
            actionDiv.className = 'agent-action';
            actionDiv.innerHTML = `
                <div><strong>图像处理Agent:</strong> ${action}</div>
                <div style="margin-top: 8px; color: #64748b; font-size: 0.8em;">
                    <i class="fas fa-clock"></i> 已完成于 ${new Date().toLocaleTimeString()}
                </div>
            `;

            const lastMessage = chatContainer.lastChild;
            lastMessage.appendChild(actionDiv);

            chatContainer.scrollTop = chatContainer.scrollHeight;
        }
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

    kmain.createByTemplate = function () {
        var BootstrapDialog = require('bootstrap5-dialog');
        
        // 在弹窗打开前，保存 body 的原始样式
        var $body = $('body');
        var originalBodyPaddingRight = $body.css('padding-right');
        var originalBodyOverflow = $body.css('overflow');
        
        kmain.templateDialog = BootstrapDialog.show({
            size: BootstrapDialog.SIZE_LARGE,
            closable: true,
            message: $('<div id="openTemplateGallery"></div>'),
            title: kresource.getItem("templategallery"),
            onshown: function (dialog) {
                // 防止 body padding-right 影响工具栏
                // Bootstrap modal 可能会给 body 添加 padding-right 来防止滚动条闪烁
                // 我们需要确保这个 padding 不影响 fixed 定位的工具栏
                var $currentBody = $('body');
                $currentBody.css({
                    'padding-right': '0',
                    'overflow': 'hidden'
                });
                
                // 确保按钮栏位置不受影响
                $('.top-toolbar').css({
                    'right': '0',
                    'margin-right': '0',
                    'padding-right': '0',
                    'width': '100%'
                });
                
                $("#openTemplateGallery").load('pages/template/index.html', function() {
                    // 调整弹窗宽度以适应Template Gallery页面内容
                    var $modalDialog = $('#openTemplateGallery').closest('.modal-dialog');
                    if ($modalDialog.length) {
                        $modalDialog.css('max-width', '1200px');
                        $modalDialog.css('width', '90%');
                    }
                    
                    // 设置焦点到 Standard Process 菜单项
                    setTimeout(function() {
                        var $standardProcessItem = $('.menuItemTemplate[data-id="standard"]');
                        if ($standardProcessItem.length) {
                            // 确保该菜单项是激活状态
                            $standardProcessItem.addClass('active').siblings().removeClass('active');
                            
                            // 设置 tabindex 使其可聚焦
                            if (!$standardProcessItem.attr('tabindex')) {
                                $standardProcessItem.attr('tabindex', '0');
                            }
                            
                            // 设置焦点
                            $standardProcessItem.focus();
                            
                            // 确保菜单项在可视区域内
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
            onhidden: function(dialog) {
                // 弹窗关闭时，恢复 body 的原始样式（如果需要）
                // BootstrapDialog 会自动处理，这里作为额外保障
                var $currentBody = $('body');
                $currentBody.css({
                    'padding-right': originalBodyPaddingRight,
                    'overflow': originalBodyOverflow
                });
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


