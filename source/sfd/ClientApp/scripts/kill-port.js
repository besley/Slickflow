// Node.js script to kill process on port 5000
// 停止占用端口 5000 的进程

const { exec } = require('child_process');
const os = require('os');

const PORT = 5000;
const isWindows = os.platform() === 'win32';

console.log(`Checking for processes using port ${PORT}...`);

if (isWindows) {
    // Windows: Use netstat to find process
    exec(`netstat -ano | findstr ":${PORT}" | findstr "LISTENING"`, (error, stdout, stderr) => {
        if (error) {
            console.log(`No process found using port ${PORT}`);
            return;
        }

        const lines = stdout.trim().split('\n');
        if (lines.length === 0 || !lines[0]) {
            console.log(`No process found using port ${PORT}`);
            return;
        }

        // Extract PID from last column
        const parts = lines[0].trim().split(/\s+/);
        const pid = parts[parts.length - 1];

        if (pid) {
            console.log(`Found process ${pid} using port ${PORT}`);
            console.log(`Killing process ${pid}...`);
            
            exec(`taskkill /PID ${pid} /F`, (killError, killStdout, killStderr) => {
                if (killError) {
                    console.error(`Failed to kill process ${pid}:`, killError.message);
                } else {
                    console.log(`Process ${pid} has been terminated.`);
                }
            });
        }
    });
} else {
    // Unix-like: Use lsof or fuser
    exec(`lsof -ti:${PORT}`, (error, stdout, stderr) => {
        if (error) {
            console.log(`No process found using port ${PORT}`);
            return;
        }

        const pid = stdout.trim();
        if (pid) {
            console.log(`Found process ${pid} using port ${PORT}`);
            console.log(`Killing process ${pid}...`);
            
            exec(`kill -9 ${pid}`, (killError) => {
                if (killError) {
                    console.error(`Failed to kill process ${pid}:`, killError.message);
                } else {
                    console.log(`Process ${pid} has been terminated.`);
                }
            });
        }
    });
}
