if (!window.console) {
    window.console = {};
}
if (!console.log) {
    console.log = function () { };
}

window.slick = window.slick || {},
slick.WEB_ROOT_HOST = "http://" + window.location.host + '/webdemo';
