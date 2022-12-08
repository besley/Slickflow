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
import identityModdleDescriptor from './slickflow/descriptors/identity';
import sectionModdleDescriptor from './slickflow/descriptors/section';
import transitionModdleDescriptor from './slickflow/descriptors/transition';
import performersModdleDescriptor from './slickflow/descriptors/performers';
import gatewayModdleDescriptor from './slickflow/descriptors/gateway';

sfModdleDescriptor.types.push(identityModdleDescriptor.identity);
sfModdleDescriptor.types.push(transitionModdleDescriptor.transition);
sfModdleDescriptor.types.push(gatewayModdleDescriptor.gateway);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(sectionModdleDescriptor.section);
sfModdleDescriptor.types = sfModdleDescriptor.types.concat(performersModdleDescriptor.performers);

//import external property panel
import SfCommandInterceptor from './slickflow/module/SfCommandInterceptor';
import SfCommandExtension from './slickflow/module/SfCommandExtension';

import transitionPropertiesProviderModule from './slickflow/provider/transition';
import sectionPropertiesProviderModule from './slickflow/provider/section/';
import gatewayPropertiesProviderModule from './slickflow/provider/gateway/';
import performersPropertiesProviderModule from './slickflow/provider/performers/';
import identityPropertiesProviderModule from './slickflow/provider/identity';

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
        sf: sfModdleDescriptor
    },
  additionalModules: [
    BpmnPropertiesPanelModule,
      SfCommandInterceptor,
      SfCommandExtension,
      identityPropertiesProviderModule,
      transitionPropertiesProviderModule,
      sectionPropertiesProviderModule,
      gatewayPropertiesProviderModule,
      performersPropertiesProviderModule
  ]
});

const propertiesPanel = bpmnModeler.get('propertiesPanel');

//import kmain js file
import kmain from '/app/viewjs/kmain.js'
window.kmain = kmain;
kmain.init(bpmnModeler);

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
//#endregion