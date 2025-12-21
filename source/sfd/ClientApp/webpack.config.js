const CopyPlugin = require('copy-webpack-plugin');
const webpack = require('webpack');
const path = require('path');

const basePath = '.';
// 明确获取环境变量，优先使用 KCONFIG_ENV（由 package.json 脚本设置）
// KCONFIG_ENV 应该是 'dev' 或 'prod'
let buildEnv = process.env.KCONFIG_ENV;

// 如果 KCONFIG_ENV 未设置，回退到基于 NODE_ENV 的判断
if (!buildEnv) {
    buildEnv = process.env.NODE_ENV === 'production' ? 'prod' : 'dev';
}

// 确保 buildEnv 只能是 'dev' 或 'prod'
if (buildEnv !== 'dev' && buildEnv !== 'prod') {
    console.warn(`Warning: Invalid KCONFIG_ENV value "${buildEnv}", defaulting to "dev"`);
    buildEnv = 'dev';
}

// 根据 buildEnv 设置 webpack mode
const webpackMode = buildEnv === 'prod' ? 'production' : 'development';

const absoluteBasePath = path.resolve(path.join(__dirname, basePath));

// 输出调试信息
console.log('Build Environment:', buildEnv);
console.log('Webpack Mode:', webpackMode);
console.log('KCONFIG_ENV:', process.env.KCONFIG_ENV);
console.log('NODE_ENV:', process.env.NODE_ENV);

module.exports = {
    mode: webpackMode,
    entry: ['./app/index.js'],
    output: {
        path: path.resolve(__dirname, 'public'),
        filename: 'index.js'
    },
    devtool: 'source-map',
    devServer: {
        hot: true,  // 启用热重载
        open: true, // 自动打开浏览器
        liveReload: true, // 启用实时重载
        //static: {  // 添加静态文件服务配置
        //    directory: path.join(__dirname, 'public'),
        //},
        //historyApiFallback: false
    },
    module: {
        rules: [
            {
                test: /\.m?js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        plugins: [
                            ['@babel/plugin-transform-react-jsx', {
                                'importSource': '@bpmn-io/properties-panel/preact',
                                'runtime': 'automatic'
                            }]
                        ]
                    }
                }
            },
            {
                test: /\.less$/i,
                use: [
                    // compiles Less to CSS
                    "style-loader",
                    "css-loader",
                    "less-loader",
                ],
            },
            {
                test: /\.bpmn$/,
                use: {
                    loader: 'raw-loader'
                }
            }
        ]
    },
    resolve: {
        mainFields: [
            'browser',
            'module',
            'main'
        ],
        alias: {
            'react': '@bpmn-io/properties-panel/preact/compat'
        },
        modules: [
            'node_modules',
            absoluteBasePath
        ]
    },
    plugins: [
        new webpack.DefinePlugin({
            __KCONFIG_ENV__: JSON.stringify(buildEnv)
        }),
        new CopyPlugin({
            patterns: [
                { from: 'app/index.html', to: '.' },
                { from: 'app/favicon.ico', to: '.' },
                { from: 'app/pages/.', to: 'pages/.' },
                { from: 'app/script/.', to: 'script/.' },
                { from: 'app/viewjs/.', to: 'viewjs/.' },
                { from: 'app/resources/.', to: 'resources/.' },
                { from: 'app/images/.', to: 'images/.' },
                { from: 'app/css/.', to: 'css/.' },
                // 处理 config 目录：kconfig.js 需要替换环境变量，其他文件直接复制
                {
                    from: 'app/config/kconfig.js',
                    to: 'config/kconfig.js',
                    transform(content) {
                        // 将 __KCONFIG_ENV__ 替换为实际的环境值（带引号的字符串）
                        const envValue = JSON.stringify(buildEnv);
                        return content.toString().replace(/__KCONFIG_ENV__/g, envValue);
                    }
                },
                {
                    from: 'app/config',
                    to: 'config',
                    globOptions: {
                        ignore: ['**/kconfig.js']  // 排除 kconfig.js，因为上面已经单独处理了
                    }
                },
                { from: 'node_modules/jquery/dist', to: 'vendor/jquery/dist' },
                { from: 'node_modules/bpmn-js/dist/assets', to: 'vendor/bpmn-js/assets' },
                { from: 'node_modules/bpmn-js-properties-panel/dist/assets', to: 'vendor/bpmn-js-properties-panel/assets' },
                { from: 'node_modules/bootstrap/dist', to: 'vendor/bootstrap/dist' },
                { from: 'node_modules/bootstrap-icons/font', to: 'vendor/bootstrap-icons/font' },
                { from: 'node_modules/bootstrap5-dialog/dist/css', to: 'vendor/bootstrap5-dialog/dist/css' },
                { from: 'node_modules/ag-grid-community/dist', to: 'vendor/ag-grid-community/dist' },
                { from: 'node_modules/ag-grid-community/styles', to: 'vendor/ag-grid-community/styles' },
                { from: 'node_modules/fine-uploader/fine-uploader', to: 'vendor/fine-uploader/fine-uploader' },
                { from: 'node_modules/codemirror', to: 'vendor/codemirror' },
                { from: 'node_modules/@fortawesome', to: 'vendor/fortawesome' },
            ]
        })
    ]
};