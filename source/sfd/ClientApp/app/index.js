import jquery from 'jquery';
var $ = require("jquery");
window.$ = $;

import bootstrap from 'bootstrap'
import BootstrapDialog from 'bootstrap5-dialog'
window.BootstrapDialog = BootstrapDialog;

import agGrid from 'ag-grid-community'
window.agGrid = require('ag-grid-community');

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
import notificationModdleDescriptor from './slickflow/descriptors/notification';
import gatewayModdleDescriptor from './slickflow/descriptors/gateway';
import multisignModdleDescriptor from './slickflow/descriptors/multisign';
import subinfoesModdleDescriptor from './slickflow/descriptors/subinfoes';
import servicetaskModdleDescriptor from './slickflow/descriptors/servicetask';
import scripttaskModdleDescriptor from './slickflow/descriptors/scripttask';
//import triggerModdleDescriptor from './slickflow/descriptors/trigger';

sfModdleDescriptor.types.push(identityModdleDescriptor.identity);
sfModdleDescriptor.types.push(transitionModdleDescriptor.transition);
sfModdleDescriptor.types.push(gatewayModdleDescriptor.gateway);
sfModdleDescriptor.types.push(multisignModdleDescriptor.multisign);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(actionModdleDescriptor.action);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(sectionModdleDescriptor.section);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(boundaryModdleDescriptor.boundary);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(servicetaskModdleDescriptor.service);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(scripttaskModdleDescriptor.script);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(performersModdleDescriptor.performers);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(notificationModdleDescriptor.notifications);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(subinfoesModdleDescriptor.subinfoes);
/*sfModdleDescriptor.types.push(triggerModdleDescriptor.trigger);*/

//import external property panel
import SfCommandInterceptor from './slickflow/module/SfCommandInterceptor';
import SfCommandExtension from './slickflow/module/SfCommandExtension';

/*import magicPropertiesProviderModule from './slickflow/provider/magic';*/
import actionPropertiesProviderModule from './slickflow/provider/action/';
import transitionPropertiesProviderModule from './slickflow/provider/transition/';
import sectionPropertiesProviderModule from './slickflow/provider/section/';
import boundaryPropertiesProviderModule from './slickflow/provider/boundary/';
import gatewayPropertiesProviderModule from './slickflow/provider/gateway/';
import multisignPropertiesProviderModule from './slickflow/provider/multisign/';
import subinfoesPropertiesProviderModule from './slickflow/provider/subinfoes/';
import triggerPropertiesProviderModule from './slickflow/provider/trigger/';
import servicetaskPropertiesProviderModule from './slickflow/provider/servicetask/';
import scripttaskPropertiesProviderModule from './slickflow/provider/scripttask/';
import performersPropertiesProviderModule from './slickflow/provider/performers/';
import notificationPropertiesProviderModule from './slickflow/provider/notification/';
//import expressionPropertiesProviderModule from './slickflow/provider/expression';
//import SfOverallPropertiesProviderModule from './slickflow/provider/overall/';
import identityPropertiesProviderModule from './slickflow/provider/identity';
/*import customContextModule from './slickflow/context';*/


import {
  debounce
} from 'min-dash';

import '../styles/app.less';

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
/*      magicPropertiesProviderModule,*/
      transitionPropertiesProviderModule,
      sectionPropertiesProviderModule,
      boundaryPropertiesProviderModule,
      gatewayPropertiesProviderModule,
      multisignPropertiesProviderModule,
      triggerPropertiesProviderModule,
      servicetaskPropertiesProviderModule,
      scripttaskPropertiesProviderModule,
      notificationPropertiesProviderModule,
      performersPropertiesProviderModule,
      subinfoesPropertiesProviderModule
/*      expressionPropertiesProviderModule,*/
/*      customContextModule*/
  ]
});
//const eventList = bpmnModeler.get('eventBus');
//console.log(eventList);

const propertiesPanel = bpmnModeler.get('propertiesPanel');

//import kmain js file
import kmain from '/app/viewjs/kmain.js'
window.kmain = kmain;
kmain.init(bpmnModeler);

import ktemplate from '/app/viewjs/ktemplate.js'
window.ktemplate = ktemplate;

//#region File Drops
function registerFileDrop(container, callback) {

  function handleFileSelect(e) {
    e.stopPropagation();
    e.preventDefault();

    var files = e.dataTransfer.files;

    var file = files[0];

    var reader = new FileReader();

    reader.onload = function(e) {

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
$(function() {
  $('#js-create-diagram').click(function(e) {
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

$('#btnOpenTemplateGallery').click(async function (e) {
    e.stopPropagation();
    e.preventDefault();

    kmain.createByTemplate()
})

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