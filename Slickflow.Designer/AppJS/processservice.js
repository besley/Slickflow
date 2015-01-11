var app = angular.module("processApp", []);

app.service('processAPIService', function ($http) {
    var processAPI = {};
    processAPI.getProcessFile = function (id) {
        return $http({
            method: "GET",
            url: '/SfApi/api/Wf2Xml/GetProcessFile/' + id,
            headers: {
                'Content-Type': 'application/json'
            }
        });
    }

    processAPI.saveProcessFile = function (entity) {
        return $http({
            method: "POST",
            data: JSON.stringify(entity),
            url: '/SfApi/api/Wf2Xml/SaveProcessFile',
            headers: {
                'Content-Type': 'application/json'
            }
        })
    }

    processAPI.getProcessById = function (id) {
        return $http({
            method: "GET",
            url: '/SfApi/api/Wf2Xml/GetProcess/' + id,
            headers: {
                'Content-Type': 'application/json'
            }
        })
    }

    processAPI.getProcess = function () {
        return $http({
            method: "GET",
            url: '/SfApi/api/Wf2Xml/GetProcess',
            headers: {
                'Content-Type': 'application/json'
            }
        })
    }

    return processAPI;
});