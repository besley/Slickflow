const CopyPlugin = require('copy-webpack-plugin');
const webpack = require('webpack');
const path = require('path');

const basePath = '.';

const absoluteBasePath = path.resolve(path.join(__dirname, basePath));

module.exports = {
  mode: 'development',
/*  mode:'production',*/
  entry: ['./app/index.js'],
  output: {
    path: path.resolve(__dirname, 'public'),
    filename: 'index.js'
  },
  devtool: 'source-map',
  module: {
    rules: [
      {
        test: /\.m?js$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: {
            plugins: [
              [ '@babel/plugin-transform-react-jsx', {
                'importSource': '@bpmn-io/properties-panel/preact',
                'runtime': 'automatic'
              } ]
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
            { from: 'app/config/.', to: 'config/.' },
            { from: 'node_modules/jquery/dist', to: 'vendor/jquery/dist' },
            { from: 'node_modules/bpmn-js/dist/assets', to: 'vendor/bpmn-js/assets' },
            { from: 'node_modules/bpmn-js-properties-panel/dist/assets', to: 'vendor/bpmn-js-properties-panel/assets' },
            { from: 'node_modules/bootstrap/dist', to: 'vendor/bootstrap/dist' },
            { from: 'node_modules/bootstrap-icons/font', to: 'vendor/bootstrap-icons/font' },
            { from: 'node_modules/bootstrap5-dialog/dist/css', to: 'vendor/bootstrap5-dialog/dist/css' },
            { from: 'node_modules/ag-grid-community/dist', to: 'vendor/ag-grid-community/dist' },
            { from: 'node_modules/fine-uploader/fine-uploader', to: 'vendor/fine-uploader/fine-uploader' },
            { from: 'node_modules/codemirror', to: 'vendor/codemirror' },
      ]
    })
  ]
};