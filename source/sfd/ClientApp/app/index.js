import jquery from 'jquery';
var $ = require("jquery");
window.$ = $;

import bootstrap from 'bootstrap'
import BootstrapDialog from 'bootstrap5-dialog'
window.BootstrapDialog = BootstrapDialog;

import { createGrid, ModuleRegistry, AllCommunityModule, themeBalham } from 'ag-grid-community';

// Classic light theme — matches demo.slickflow.com/sfd/# (blue header, white rows)
const _sfGridLight = themeBalham.withParams({
    headerBackgroundColor:       '#49afcd',
    headerTextColor:             '#ffffff',
    headerFontSize:              13,
    headerFontWeight:            600,
    headerHeight:                38,
    fontSize:                    13,
    rowHeight:                   36,
    borderRadius:                2,
    wrapperBorderRadius:         4,
    borderColor:                 '#d0d5da',
    rowHoverColor:               '#f0f8fb',
    selectedRowBackgroundColor:  '#c8e6f5',
    oddRowBackgroundColor:       '#f9fafb',
});

// Dark variant — used for dark themes (deep-sea, blue, green, purple, red)
const _sfGridDark = themeBalham.withParams({
    headerBackgroundColor:       '#2d3748',
    headerTextColor:             '#e2e8f0',
    headerFontSize:              13,
    headerFontWeight:            500,
    headerHeight:                38,
    backgroundColor:             '#1a202c',
    foregroundColor:             '#cbd5e0',
    borderColor:                 '#4a5568',
    rowHoverColor:               'rgba(255,255,255,.04)',
    selectedRowBackgroundColor:  'rgba(59,130,246,.20)',
    oddRowBackgroundColor:       '#1e2533',
    fontSize:                    13,
    rowHeight:                   36,
    borderRadius:                2,
    wrapperBorderRadius:         4,
});

// Returns the right theme for the current sfd theme (checks body class at call time)
function sfGetGridTheme() {
    var body = document.body;
    var isSand = body.classList.contains('sf-theme-sand');
    // No sf-theme-* class = default dark theme
    var hasDark = ['sf-theme-blue','sf-theme-green','sf-theme-purple','sf-theme-red'].some(
        function(c){ return body.classList.contains(c); }
    );
    var isDefault = !body.classList.contains('sf-theme-sand')
        && !body.classList.contains('sf-theme-blue')
        && !body.classList.contains('sf-theme-green')
        && !body.classList.contains('sf-theme-purple')
        && !body.classList.contains('sf-theme-red');
    return (isSand) ? _sfGridLight : _sfGridDark;
}

window.themeBalham   = _sfGridLight;   // backward compat default (sand / no-theme)
window.sfGetGridTheme = sfGetGridTheme;
window.createGrid = createGrid;

ModuleRegistry.registerModules([AllCommunityModule])

import BpmnModeler from 'bpmn-js/lib/Modeler';

import {
    BpmnPropertiesPanelModule,
    BpmnPropertiesProviderModule
} from 'bpmn-js-properties-panel';

import sfModdleDescriptor from './slickflow/descriptors/sf';
import magicModdleDescriptor from './slickflow/descriptors/magic';
import identityModdleDescriptor from './slickflow/descriptors/identity';
import actionModdleDescriptor from './slickflow/descriptors/action';
import sectionModdleDescriptor from './slickflow/descriptors/section';
import boundaryModdleDescriptor from './slickflow/descriptors/boundary';
import transitionModdleDescriptor from './slickflow/descriptors/transition';
import performersModdleDescriptor from './slickflow/descriptors/performers';
import formsModdleDescriptor from './slickflow/descriptors/forms';
import notificationModdleDescriptor from './slickflow/descriptors/notification';
import variableModdleDescriptor from './slickflow/descriptors/variable';
import gatewayModdleDescriptor from './slickflow/descriptors/gateway';
import multisignModdleDescriptor from './slickflow/descriptors/multisign';
import subinfoesModdleDescriptor from './slickflow/descriptors/subinfoes';
import servicetaskModdleDescriptor from './slickflow/descriptors/servicetask';
import aiservicetaskModdleDescriptor from './slickflow/descriptors/aiservicetask';
import scripttaskModdleDescriptor from './slickflow/descriptors/scripttask';
import ruletaskModdleDescriptor from './slickflow/descriptors/ruletask';

sfModdleDescriptor.types.push(identityModdleDescriptor.identity);
sfModdleDescriptor.types.push(transitionModdleDescriptor.transition);
sfModdleDescriptor.types.push(gatewayModdleDescriptor.gateway);
sfModdleDescriptor.types.push(multisignModdleDescriptor.multisign);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(actionModdleDescriptor.action);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(sectionModdleDescriptor.section);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(boundaryModdleDescriptor.boundary);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(servicetaskModdleDescriptor.service);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(aiservicetaskModdleDescriptor.aiService);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(scripttaskModdleDescriptor.script);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(ruletaskModdleDescriptor.ruletask);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(performersModdleDescriptor.performers);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(formsModdleDescriptor.forms);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(notificationModdleDescriptor.notifications);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(variableModdleDescriptor.variables);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(subinfoesModdleDescriptor.subinfoes);

//import external property panel
import SfCommandInterceptor from './slickflow/module/SfCommandInterceptor';
import SfCommandExtension from './slickflow/module/SfCommandExtension';
import SfCustomRenderer from './slickflow/module/SfCustomRenderer';

const SfCustomRendererModule = {
    __init__: ['sfCustomRenderer'],
    sfCustomRenderer: ['type', SfCustomRenderer]
};

import actionPropertiesProviderModule from './slickflow/provider/action/';
import transitionPropertiesProviderModule from './slickflow/provider/transition/';
import sectionPropertiesProviderModule from './slickflow/provider/section/';
import boundaryPropertiesProviderModule from './slickflow/provider/boundary/';
import gatewayPropertiesProviderModule from './slickflow/provider/gateway/';
import multisignPropertiesProviderModule from './slickflow/provider/multisign/';
import subinfoesPropertiesProviderModule from './slickflow/provider/subinfoes/';
import triggerPropertiesProviderModule from './slickflow/provider/trigger/';
import servicetaskPropertiesProviderModule from './slickflow/provider/servicetask/';
// import aiservicetaskPropertiesProviderModule from './slickflow/provider/aiservicetask/'; // Removed: aidetai property group is no longer needed

import scripttaskPropertiesProviderModule from './slickflow/provider/scripttask/';
import ruletaskPropertiesProviderModule from './slickflow/provider/ruletask/';
import performersPropertiesProviderModule from './slickflow/provider/performers/';
import formsPropertiesProviderModule from './slickflow/provider/forms/';
import fieldsPropertiesProviderModule from './slickflow/provider/fields/';
import notificationPropertiesProviderModule from './slickflow/provider/notification/';
import variablePropertiesProviderModule from './slickflow/provider/variable/';
import identityPropertiesProviderModule from './slickflow/provider/identity';
import customContextModule from './slickflow/context';

import { initWfVariableCacheLoader, stripSfVariablesFromDiagram } from './slickflow/provider/variable/wfVariableCache';

import {
    debounce
} from 'min-dash';

var container = $('#js-drop-zone');

var bpmnModeler = new BpmnModeler({
    container: '#js-canvas',
    propertiesPanel: {
        parent: '#js-properties-panel'
    },
    moddleExtensions: {
        sf: sfModdleDescriptor,
        magic: magicModdleDescriptor
    },
    additionalModules: [
        BpmnPropertiesPanelModule,
        SfCommandInterceptor,
        SfCommandExtension,
        identityPropertiesProviderModule,
        actionPropertiesProviderModule,
        transitionPropertiesProviderModule,
        sectionPropertiesProviderModule,
        boundaryPropertiesProviderModule,
        gatewayPropertiesProviderModule,
        multisignPropertiesProviderModule,
        triggerPropertiesProviderModule,
        servicetaskPropertiesProviderModule,
        // aiservicetaskPropertiesProviderModule, // Removed: aidetai property group is no longer needed
        scripttaskPropertiesProviderModule,
        ruletaskPropertiesProviderModule,
        notificationPropertiesProviderModule,
        variablePropertiesProviderModule,
        formsPropertiesProviderModule,
        fieldsPropertiesProviderModule,
        subinfoesPropertiesProviderModule,
        performersPropertiesProviderModule,
        customContextModule,
        SfCustomRendererModule
    ]
});

const propertiesPanel = bpmnModeler.get('propertiesPanel');

const eventBus = bpmnModeler.get('eventBus');
const modeling = bpmnModeler.get('modeling');
const elementRegistry = bpmnModeler.get('elementRegistry');
const injector = bpmnModeler.get('injector');
initWfVariableCacheLoader(eventBus, injector);
eventBus.on('saveXML.start', 2000, function () {
    stripSfVariablesFromDiagram(modeling, elementRegistry);
});

//import kmain js file
import kmain from './viewjs/kmain.js'
window.kmain = kmain;
kmain.init(bpmnModeler);

import ktemplate from './viewjs/ktemplate.js'
window.ktemplate = ktemplate;

import kaidialog from './viewjs/kaidialog.js'
window.kaidialog = kaidialog;

import setting from './viewjs/setting.js'
window.setting = setting;

import kbmanager from './viewjs/kbmanager.js'
window.kbmanager = kbmanager;

//#region File Drops
function registerFileDrop(container, callback) {

    function handleFileSelect(e) {
        e.stopPropagation();
        e.preventDefault();

        var files = e.dataTransfer.files;

        var file = files[0];

        var reader = new FileReader();

        reader.onload = function (e) {

            var xml = e.target.result;

            callback(xml);
        };

        reader.readAsText(file);
    }

    function handleDragOver(e) {
        e.stopPropagation();
        e.preventDefault();

        e.dataTransfer.dropEffect = 'copy'; // Explicitly show this is a copy.
    }

    container.get(0).addEventListener('dragover', handleDragOver, false);
    container.get(0).addEventListener('drop', handleFileSelect, false);
}


////// file drag / drop ///////////////////////

// check file api availability
if (!window.FileList || !window.FileReader) {
    window.alert(
        'Looks like you use an older browser that does not support drag and drop. ' +
        'Try using Chrome, Firefox or the Internet Explorer > 10.');
} else {
    registerFileDrop(container, kmain.openDiagramFile);
}
//#endregion

//#region render functions and button event
// bootstrap diagram functions
$(function () {
    $('#js-create-diagram').click(function (e) {
        e.stopPropagation();
        e.preventDefault();

        kmain.createNewDiagram();
    });

    $('#js-open-process-list').click(function (e) {
        e.stopPropagation();
        e.preventDefault();

        kmain.openProcess();
    });
});

$('#btnCreateProcess').click(function (e) {
    e.stopPropagation();
    e.preventDefault();

    //display ai dialog
    document.querySelector('.dialog-container').style.display = 'block';
    document.getElementById('userInput').focus();

    kmain.createNewDiagram();
});

$('#btnOpenProcess').click(function (e) {
    e.stopPropagation();
    e.preventDefault();

    kmain.openProcess();
});

$('#btnSaveProcess').click(async function (e) {
    e.stopPropagation();
    e.preventDefault();

    const { xml } = await bpmnModeler.saveXML({ format: true });

    kmain.saveProcessFile(kmain.mxSelectedProcessEntity, xml);
});


$('#btnValidateProcess').click(async function (e) {
    const { xml } = await bpmnModeler.saveXML({ format: true });
    kmain.validateProcess(kmain.mxSelectedProcessEntity, xml);
});

$('#btnHelp').click(function (e) {
    e.stopPropagation();
    e.preventDefault();

    kmain.openHelpPage();
});
//#endregion
