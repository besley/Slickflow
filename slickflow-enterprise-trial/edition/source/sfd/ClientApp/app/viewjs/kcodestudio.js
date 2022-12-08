
import kresource from './kresource.js'
window.kresource = kresource;

import processmodel from './processmodel.js'
window.processmodel = processmodel;

// bootstrap diagram functions
$(function () {
    processmodel.initLang();

    var csharpEditor = CodeMirror.fromTextArea(document.getElementById("txtCode"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: "text/x-java"
    });
    processmodel.mcodemirrorEditor = csharpEditor;

    window.console.log('process model...')
});