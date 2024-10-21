import fireTypeProps from './parts/FireTypeProps';
import methodTypeProps from './parts/MethodTypeProps'
import subMethodTypeProps from './parts/SubMethodTypeProps'
import argusProps from './parts/ArgusProps'
import codeTextProps from './parts/CodeTextProps'
import scriptTextProps from './parts/ScriptTextProps'
import assemblyFullNameProps from './parts/AssemblyFullNameProps'
import typeFullNameProps from './parts/TypeFullNameProps'
import methodNameProps from './parts/MethodNameProps'
import expressionProps from './parts/ExpressionProps'

import { is } from 'bpmn-js/lib/util/ModelUtil';

const LOW_PRIORITY = 500;

export default function ActionPropertiesProvider(propertiesPanel, translate) {
    this.getGroups = function (element) {
        var isNot = !is(element, 'bpmn:ServiceTask') && !is(element, 'bpmn:ScriptTask');
        var isYes = is(element, 'bpmn:Task') || is(element, 'bpmn:UserTask')
            || is(element, 'bpmn:intermediateThrowEvent') || is(element, 'bpmn:intermediateThrowEvent');
        return function (groups) {
            if (isNot && isYes) {
                groups.push(createActionGroup(element, translate));
            }
            return groups;
        }
    }
    propertiesPanel.registerProvider(LOW_PRIORITY, this);
}

ActionPropertiesProvider.$inject = ['propertiesPanel', 'translate'];

function createActionGroup(element, translate) {
    const actionGroup = {
        id: 'action',
        label: kresource.getItem('action'),
        entries: [fireTypeProps(element),
            methodTypeProps(element, onChange),
            subMethodTypeProps(element, true),
            argusProps(element),
            codeTextProps(element),
            scriptTextProps(element),
            assemblyFullNameProps(element),
            typeFullNameProps(element),
            methodNameProps(element),
            expressionProps(element)
        ]
    };
    return actionGroup;
}

//action configuration
var myActionPanel = {};
myActionPanel["LocalService"] = ["argus", "expression"];
myActionPanel["CSharpLibrary"] = ["argus", "assembly", "typefullname", "methodname"];
myActionPanel["WebApi"] = ["argus", "expression", "submethodtype"];
myActionPanel["SQL"] = ["argus", "scripttext"];
myActionPanel["SQLStoreProcedure"] = ["argus", "expression"];
myActionPanel["Python"] = ["argus", "codetext"];

//input controls
var myInputControls = ["argus", "expression", "assembly", "typefullname", "methodname", "submethodtype", "scripttext", "codetext"];


function onChange(value) {
    //hide input controls
    myInputControls.forEach(function (item, i) {
        $("div.bio-properties-panel-entry[data-entry-id='" + item + "']").hide();
    })

    //display input controls
    var fields = myActionPanel[value];
    $(".div.bio-properties-panel-input").hide();
    if (fields !== undefined) {
        fields.forEach(function (item, i) {
            $("div.bio-properties-panel-entry[data-entry-id='" + item + "']").show();
        })
    }
}
