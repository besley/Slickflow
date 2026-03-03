# PowerShell script to kill process on port 5000
# 停止占用端口 5000 的进程

$port = 5000

Write-Host "Checking for processes using port $port..." -ForegroundColor Yellow

# Get process ID using the port
$connections = netstat -ano | findstr ":$port"

if ($connections) {
    # Extract process ID
    $processId = ($connections | Select-String -Pattern 'LISTENING\s+(\d+)').Matches.Groups[1].Value
    
    if ($processId) {
        Write-Host "Found process $processId using port $port" -ForegroundColor Yellow
        
        # Get process name
        $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
        if ($process) {
            Write-Host "Process name: $($process.ProcessName)" -ForegroundColor Yellow
            Write-Host "Killing process $processId..." -ForegroundColor Red
            Stop-Process -Id $processId -Force
            Write-Host "Process $processId has been terminated." -ForegroundColor Green
        } else {
            Write-Host "Process $processId not found (may have already terminated)" -ForegroundColor Yellow
        }
    } else {
        Write-Host "Could not extract process ID from netstat output" -ForegroundColor Red
    }
} else {
    Write-Host "No process found using port $port" -ForegroundColor Green
}
