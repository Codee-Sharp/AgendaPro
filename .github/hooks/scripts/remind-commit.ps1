$ErrorActionPreference = 'Stop'

$gitCommand = Get-Command git -ErrorAction SilentlyContinue

if (-not $gitCommand) {
    $payload = @{ continue = $true } | ConvertTo-Json -Compress
    Write-Output $payload
    exit 0
}

$repoRoot = git rev-parse --show-toplevel 2>$null
if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($repoRoot)) {
    $payload = @{ continue = $true } | ConvertTo-Json -Compress
    Write-Output $payload
    exit 0
}

$statusLines = git -C $repoRoot status --porcelain 2>$null
if ($LASTEXITCODE -ne 0) {
    $payload = @{ continue = $true } | ConvertTo-Json -Compress
    Write-Output $payload
    exit 0
}

if ($statusLines) {
    $payload = @{
        continue = $true
        systemMessage = 'There are uncommitted changes in this workspace. Review the diff and commit if this turn completed a meaningful change.'
    } | ConvertTo-Json -Compress
    Write-Output $payload
    exit 0
}

$payload = @{ continue = $true } | ConvertTo-Json -Compress
Write-Output $payload