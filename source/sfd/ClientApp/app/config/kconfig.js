const kconfig = (function () {
    function kconfig() { }

    // webpack DefinePlugin injection
    // DefinePlugin 在编译时会直接将 __KCONFIG_ENV__ 替换为字符串值（如 "prod" 或 "dev"）
    // 如果文件被直接复制（未经过 webpack 处理），__KCONFIG_ENV__ 会是未定义的，使用默认值 'dev'
    const env = typeof __KCONFIG_ENV__ !== 'undefined' ? __KCONFIG_ENV__ : 'dev';

    const presets = {
        dev: {
            webApiUrl: "http://localhost/sfdapi/",
            webTestUrl: "http://localhost/sfw/",
            webAiUrl: "http://localhost/sfai/",
            demoUrl: "http://localhost:5000/images/"
        },
        prod: {
            webApiUrl: "https://demo.slickflow.com/sfdapi/",
            webTestUrl: "https://demo.slickflow.com/sfw/",
            webAiUrl: "https://demo.slickflow.com/sfai/",
            demoUrl: "https://demo.slickflow.com/sfd/images/"
        }
    };

    const selected = presets[env] || presets.dev;

    kconfig.webApiUrl = selected.webApiUrl;
    kconfig.webTestUrl = selected.webTestUrl;
    kconfig.webAiUrl = selected.webAiUrl;
    kconfig.demoUrl = selected.demoUrl;

    return kconfig;
})();

export default kconfig;