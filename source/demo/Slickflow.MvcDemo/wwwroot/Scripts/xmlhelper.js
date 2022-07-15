var xmlhelper;
if (!xmlhelper) xmlhelper = {};

(function () {
    xmlhelper.parseXML = function (content) {
        var doc = $.parseXML(content);
        return $(doc);
    }

    xmlhelper.appendChild = function (node, child) {
        $(node).append(child);
    }

    xmlhelper.removeChild = function (node, child) {
        $(node).remove(child);
    }

    xmlhelper.getNodeText = function (node) {
        return $(node).text();
    }

    xmlhelper.setNodeText = function (node, txtValue) {
        $(node).text(txtValue);
    }

    xmlhelper.find = function (node, nodeName, txtValue) {
        var result = [];
        $(node).find(nodeName).each(function (item) {
            if (txtValue) {
                if (item.text() == txtValue) {
                    result.push(item);
                }
            } else {
                result.push(item);
            }
        });
    }

    xmlhelper.setAttr = function (node, attrName, attrValue) {
        $(node).attr(attrName).value(attrValue);
    }

    xmlhelper.getAttr = function (node, attrName) {
        return $(node).attr(attrName);
    }
})()