import { forEach } from "min-dash";

var EntryFieldSetting = (function () {
    function EntryFieldSetting() {
    }

    const myEntryPanel = {};
    myEntryPanel["All"] = ["submethodtype", "argus", "expression", "scripttext", "assembly", "typename", "methodname"];
    myEntryPanel["LocalService"] = ["argus", "expression"];
    myEntryPanel["CSharpLibrary"] = ["argus", "assembly", "typename", "methodname"];
    myEntryPanel["WebApi"] = ["submethodtype", "argus", "expression"];
    myEntryPanel["StoreProcedure"] = ["argus", "expression"];
    myEntryPanel["SQL"] = ["argus", "scripttext"];
    myEntryPanel["Javascript"] = ["argus", "scripttext"];
    myEntryPanel["Python"] = ["argus", "scripttext"];

    function hideAll() {
        var items = myEntryPanel["All"];
        for (var i = 0; i < items.length; i++) {
            var queryElement = "div.bio-properties-panel-entry[data-entry-id='" + items[i] + "']";
            $(queryElement).hide();
        }    
    }

    EntryFieldSetting.show = function (methodType) {
        hideAll();

        var items = myEntryPanel[methodType];
        for (var i = 0; i < items.length; i++) {
            var queryElement = "div.bio-properties-panel-entry[data-entry-id='" + items[i] + "']";
            $(queryElement).show();
        }        
    }
    return EntryFieldSetting;
})()

export default EntryFieldSetting;