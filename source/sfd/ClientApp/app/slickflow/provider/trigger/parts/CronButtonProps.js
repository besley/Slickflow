import { useService } from 'bpmn-js-properties-panel';
import { getBusinessObject } from 'bpmn-js/lib/util/ModelUtil';

export default function (element) {
    var cronButtonProperty =
    {
        id: 'cronButton',
        element,
        component: CronButton
    };
    return cronButtonProperty;
}

function CronButton(props) {
    const { element, id } = props;
    const debounce = useService('debounceInput');

    function addCronExpression() {
        var expression = $("#txtCronExpression").val();
        var BootstrapDialog = require('bootstrap5-dialog');
        BootstrapDialog.show({
            title: kresource.getItem("cronexpression"),
            message: $('<div id="popupContent"></div>'),
            onshown: function () {
                $("#popupContent").load('pages/cron/edit.html');
                //var expr = this.data.expression;
                //setTimeout(function () {
                //    if (expr && expr !== "null") {
                //        $("#cronExpressionValue").html(expr);
                //        $.syncCronExpression(expr);

                //        $.updateCronExpression();
                //        $.updateCronGui();
                //    }
                //}, 200);
            },
            onhidden: function () {
               // executeMultiCommad(element, bpmnFactory, commandStack);
            },
            data: { "expression": expression }
        });
    }

    return <input type='button'
        style="margin-left:10px;"
        id={id}
        element={element}
        value={kresource.getItem('cronbutton')}
        onclick={addCronExpression}
        debounce={debounce}
    />
}

