import kmsgbox from "../script/kmsgbox";

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
            var templateTypeId = $(e.target).data("id");

            if (templateTypeId === 'business') {
                $('#divBusinessTemplate').show();
                $('#divStandardTemplate').hide();
                $('#divCodeTextTemplate').hide();
            } else if (templateTypeId === 'standard') {
                $('#divStandardTemplate').show();
                $('#divBusinessTemplate').hide();
                $('#divCodeTextTemplate').hide();
            } else if (templateTypeId === 'codetext') {
                $('#divCodeTextTemplate').show();
                $('#divBusinessTemplate').hide();
                $('#divStandardTemplate').hide();
            }
        })

        $("div.btnAddToWorkSpace").on('click', function (e) {
            kmsgbox.showProgressBar();

            var templateName = '';
            var templateType = $(e.target).data("type");
            var templateId = $(e.target).data("id");
            if (templateType === "std") {
                templateName = ktemplate.mxTemplateGallery[templateType][templateId];
            } else if (templateType === "biz") {
                templateName = ktemplate.mxTemplateGallery[templateType][templateId];
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
                } else {
                    kmsgbox.warn(result.Message);
                }
                kmain.templateDialog.close();
                kmsgbox.hideProgressBar();
            });
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

    //trial feature
    ktemplate.executeGraph = function () {
        var text = $("#txtCode")[0].value;

        if (text !== "") {
            var entity = { "Body": text };
            processapi.executeProcessGraph(entity, function (result) {
                kmsgbox.info(result.Message);
                //kmain.templateDialog.close();
            });
        }
        else {
            kmsgbox.warn(kresource.getItem('domainlangwwarnmsg'));
        }
    }

    ktemplate.executeGraphAdvanced = function () {
        kmsgbox.showProgressBar();

        var text = $("#txtCode")[0].value;

        if (text !== "") {
            var entity = { "Body": text };
            processapi.executeProcessGraph(entity, function (result) {
                if (result.Status === 1) {
                    var xml = entity.XmlContent;
                    processlist.pselectedProcessEntity = entity;
                    kmain.mxSelectedProcessEntity = entity;
                    //render process into graph canvas
                    kmain.openDiagramFile(xml);
                    kmain.templateDialog.close();
                } else {
                    kmsgbox.warn(result.Message);
                }
                kmsgbox.hideProgressBar();
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