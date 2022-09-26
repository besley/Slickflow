/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。

The Slickflow Designer project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

var sfmain = (function () {
    function sfmain() {
    }

    //process list
    sfmain.load = function () {
        sfconfig.initRunner();
        processlist.getProcessList();
    }

    //startup
    sfmain.start = function () {
        processlist.start();
    }

    //run
    sfmain.run = function () {
        processlist.run();
    }

    //revise
    sfmain.revise = function () {
        processlist.revise();
    }

    //withdraw
    sfmain.withdraw = function () {
        processlist.withdraw();
    }

    //sendback
    sfmain.sendback = function () {
        processlist.sendback();
    }

    //resend
    sfmain.resend = function () {
        kmsgbox.confirm(kresource.getItem("processresendconfirmmsg"), function (result) {
            if (result === "Yes") {
                processlist.resend();
            }
        });
    }

    //reject
    sfmain.reject = function () {
        processlist.reject();
    }

    //close
    sfmain.close = function () {
        processlist.close();
    }

    //todo
    sfmain.task = function () {
        processlist.getTaskList();
    }

    //done
    sfmain.done = function () {
        processlist.getDoneList();
    }

    //agree
    sfmain.agree = function () {
        processlist.agree();
    }

    //refuse
    sfmain.refuse = function () {
        processlist.refuse();
    }

    //signforward
    sfmain.signforward = function () {
        processlist.signforward();
    }

    //flowchart
    sfmain.graph = function () {
        var entity = processlist.pselectedProcessEntity;

        if (entity !== null) {
            var appInstanceID = sfconfig.Runner.AppInstanceID;
            var processGUID = entity.ProcessGUID;
            var version = entity.Version;

            window.open('/sfd/pages/diagram?AppInstanceID=' + appInstanceID
                + '&ProcessGUID=' + processGUID + '&Version=' + version +  '&Mode=' + 'READONLY');
        } else {
            kmsgbox.warn(kresource.getItem('processselectedwarnmsg'));
            return false;
        }
    }

    //clear
    sfmain.deleteInstance = function () {
        processlist.deleteInstance();
    }

    sfmain.loadVariable = function () {
        if (processlist.pselectedTaskEntity !== null) {
            var taskID = processlist.pselectedTaskEntity.ID;
            BootstrapDialog.show({
                title: kresource.getItem('list'),
                message: $('<div></div>').load('variable/list/' + taskID),
                draggable: true
            });
        }
        else {
            kmsgbox.warn(kresource.getItem('processvariableopenmsg'));
        }
    }
    return sfmain;
})();