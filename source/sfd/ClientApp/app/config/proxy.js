
//proxy setup by using express framework
const express = require('express');
const { createProxyMiddleware } = require('http-proxy-middleware');

const app = express();
app.use(
    '/api',
    createProxyMiddleware({
        target: 'http://localhost:5001',
        changeOrigin: true,
        logLevel: 'debug',
        pathRewrite: {
            '^/api': 'http://localhost:5001/api',
        },
    }),
);
app.listen(5000);