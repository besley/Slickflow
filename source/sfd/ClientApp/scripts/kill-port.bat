@echo off
REM Batch script to kill process on port 5000
REM 停止占用端口 5000 的进程

set PORT=5000

echo Checking for processes using port %PORT%...

for /f "tokens=5" %%a in ('netstat -ano ^| findstr ":%PORT%" ^| findstr "LISTENING"') do (
    set PID=%%a
    goto :found
)

:found
if defined PID (
    echo Found process %PID% using port %PORT%
    taskkill /PID %PID% /F
    echo Process %PID% has been terminated.
) else (
    echo No process found using port %PORT%
)

pause
