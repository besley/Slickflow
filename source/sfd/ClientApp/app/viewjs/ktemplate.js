const ktemplate = (function () {
    function ktemplate() {
    }

    var std = "std";
    var biz = "biz";
    ktemplate.mxTemplateGallery = {};
    ktemplate.mxTemplateGallery[std] = {};
    ktemplate.mxTemplateGallery[biz] = {};

    //标准流程模板
    //Standard Process Template
    ktemplate.mxTemplateGallery[std]["blank"] = 'Blank';
    ktemplate.mxTemplateGallery[std]["default"] = 'Default';
    ktemplate.mxTemplateGallery[std]["simple"] = 'Simple';
    ktemplate.mxTemplateGallery[std]["sequence"] = 'Sequence';
    ktemplate.mxTemplateGallery[std]["gateway"] = 'Gateway';
    ktemplate.mxTemplateGallery[std]["mi"] = 'MultipleInstance';
    ktemplate.mxTemplateGallery[std]["andsplitmi"] = 'AndSplitMI';
    ktemplate.mxTemplateGallery[std]["subprocess"] = 'SubProcess';
    ktemplate.mxTemplateGallery[std]["conditional"] = 'Conditional';
    ktemplate.mxTemplateGallery[std]["complex"] = 'Complex';

    //业务流程模板
    //Business Process Template
    ktemplate.mxTemplateGallery[biz]["askforleave"] = 'AskforLeave';
    ktemplate.mxTemplateGallery[biz]["eorder"] = 'EOrder';
    ktemplate.mxTemplateGallery[biz]["reimbursement"] = 'Reimbursement';
    ktemplate.mxTemplateGallery[biz]["warehousing"] = 'Warehousing';
    ktemplate.mxTemplateGallery[biz]["officein"] = 'OfficeIn';
    ktemplate.mxTemplateGallery[biz]["contract"] = 'Contract';

    //代码编辑器
    //Code Editor
    ktemplate.mprocessTemplate = {};
    ktemplate.mprocessTemplate["Default"] = "";
    ktemplate.mprocessTemplate["Sequence"] = "";
    ktemplate.mprocessTemplate["Parallel"] = "";
    ktemplate.mprocessTemplate["Conditional"] = "";
    ktemplate.mprocessTemplate["ProcessModify"] = "";

    ktemplate.init = function () {
        $("li.menuItemTemplate").on('click', function (e) {
            var templateTypeID = $(e.target).data("id");

            if (templateTypeID === 'business') {
                $('#divBusinessTemplate').show();
                $('#divStandardTemplate').hide();
                $('#divCodeTextTemplate').hide();
            } else if (templateTypeID === 'standard') {
                $('#divStandardTemplate').show();
                $('#divBusinessTemplate').hide();
                $('#divCodeTextTemplate').hide();
            } else if (templateTypeID === 'codetext') {
                $('#divCodeTextTemplate').show();
                $('#divBusinessTemplate').hide();
                $('#divStandardTemplate').hide();
            }
        })

        $("div.btnAddToWorkSpace").on('click', function (e) {
            var templateName = '';
            var templateType = $(e.target).data("type");
            var templateID = $(e.target).data("id");
            if (templateType === "std") {
                templateName = ktemplate.mxTemplateGallery[templateType][templateID];
            } else if (templateType === "biz") {
                templateName = ktemplate.mxTemplateGallery[templateType][templateID];
            }

            //create a new process
            processlist.pselectedProcessEntity = null;
            kmain.mxSelectedProcessEntity = null;

            var entity = {
                "TemplateName": templateName
            };
            processapi.createByTemplate(entity, true, function (result) {
                if (result.Status === 1) {
                    processlist.pselectedProcessEntity = result.Entity;
                    kmain.mxSelectedProcessEntity = result.Entity;
                    //render process into graph canvas
                    kmain.openDiagramFile(result.Entity.XmlContent);
                }
            });

            kmain.templateDialog.close();
        })

        $("#ddlTemplateType").change(function (i, o) {
            var option = $(this).val();
            //load template content
            loadTemplate(option);
        });

        //init code mirror textarea
        loadTemplate('Default');
    }

    function loadTemplate(option) {
        var content = ktemplate.mprocessTemplate[option];

        if (content !== undefined && content !== "") {
            $("#txtCode").val(content);
            return;
        }
        processapi.loadTemplate(option, function (template) {
            $("#txtCode").val(template.Content);
            ktemplate.mprocessTemplate[option] = template.Content;
        });
    }

    ktemplate.executeGraph = function () {
        $('#loading-indicator').show();
        var text = $("#txtCode")[0].value;

        if (text !== "") {
            var entity = { "Body": text };
            processapi.executeProcessGraph(entity, function (entity) {
                var xml = entity.XmlContent;
                processlist.pselectedProcessEntity = entity;
                kmain.mxSelectedProcessEntity = entity;
                //render process into graph canvas
                kmain.openDiagramFile(xml);
                kmain.templateDialog.close();

                $('#loading-indicator').hide();
            });
        }
        else {
            kmsgbox.warn(kresource.getItem('domainlangwwarnmsg'));
        }
    }

    return ktemplate;
})()

window.ktemplate = ktemplate;

export default ktemplate;