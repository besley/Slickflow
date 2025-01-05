const kconfig = (function () {
    function kconfig() {

    }
    kconfig.webApiUrl = "http://localhost/sfdapi/"
    kconfig.webTestUrl = "http://localhost/sfw/"
    kconfig.demoUrl = "http://localhost:5000/images/"
    //kconfig.webApiUrl = "https://demo.slickflow.com/sfdapi/"
    //kconfig.webTestUrl = "https://demo.slickflow.com/sfw/"
    //kconfig.demoUrl = "https://demo.slickflow.com/sfd/images/"
    return kconfig;
})()

export default kconfig;