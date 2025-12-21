/**
 * 静态资源复制脚本
 * 在构建后复制静态资源到 dist 目录
 */
import { copyFileSync, mkdirSync, cpSync, existsSync, statSync } from 'fs';
import { resolve, dirname, join } from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const projectRoot = resolve(__dirname, '..');
const distDir = resolve(projectRoot, 'dist');
const appDir = resolve(projectRoot, 'app');

// 复制目录的辅助函数
function copyDir(src, dest) {
    if (!existsSync(src)) {
        console.warn(`Source directory does not exist: ${src}`);
        return;
    }
    
    if (!existsSync(dest)) {
        mkdirSync(dest, { recursive: true });
    }
    
    try {
        cpSync(src, dest, { recursive: true });
        console.log(`✓ Copied: ${src} -> ${dest}`);
    } catch (error) {
        console.error(`✗ Failed to copy ${src}:`, error.message);
    }
}

// 复制文件的辅助函数
function copyFile(src, dest) {
    if (!existsSync(src)) {
        console.warn(`Source file does not exist: ${src}`);
        return;
    }
    
    const destDir = dirname(dest);
    if (!existsSync(destDir)) {
        mkdirSync(destDir, { recursive: true });
    }
    
    try {
        copyFileSync(src, dest);
        console.log(`✓ Copied: ${src} -> ${dest}`);
    } catch (error) {
        console.error(`✗ Failed to copy ${src}:`, error.message);
    }
}

console.log('Starting static assets copy...\n');

// 1. 复制 app 目录下的静态资源
const staticDirs = [
    { from: 'app/pages', to: 'pages' },
    { from: 'app/script', to: 'script' },
    { from: 'app/viewjs', to: 'viewjs' },
    { from: 'app/resources', to: 'resources' },
    { from: 'app/images', to: 'images' },
    { from: 'app/css', to: 'css' },
    { from: 'app/config', to: 'config' }
];

staticDirs.forEach(({ from, to }) => {
    const src = resolve(projectRoot, from);
    const dest = resolve(distDir, to);
    copyDir(src, dest);
});

// 2. 复制单个文件
copyFile(
    resolve(appDir, 'favicon.ico'),
    resolve(distDir, 'favicon.ico')
);

// 3. 复制 vendor 目录（从 node_modules）
const vendorMappings = [
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
    { from: 'node_modules/@fortawesome/fontawesome-free', to: 'vendor/fortawesome' }
];

vendorMappings.forEach(({ from, to }) => {
    const src = resolve(projectRoot, from);
    const dest = resolve(distDir, to);
    copyDir(src, dest);
});

console.log('\n✓ Static assets copy completed!');

