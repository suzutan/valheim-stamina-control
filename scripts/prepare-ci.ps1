$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$deps = Join-Path $repoRoot '.deps'
New-Item -ItemType Directory -Path $deps -Force | Out-Null

function Get-CheckedPackage([string]$Url, [string]$Name, [string]$Sha256) {
    $zip = Join-Path $deps "$Name.zip"
    Invoke-WebRequest -Uri $Url -OutFile $zip
    if ((Get-FileHash -LiteralPath $zip).Hash -ne $Sha256) { throw "$Name checksum mismatch" }
    $destination = Join-Path $deps $Name
    Expand-Archive -LiteralPath $zip -DestinationPath $destination -Force
    return $destination
}

$bep = Get-CheckedPackage 'https://thunderstore.io/package/download/denikson/BepInExPack_Valheim/5.4.2350/' 'bepinex' '37A91C000B4E88F2ED7A4BD7D812239852D2E36CBF0FF0A9F5FAACFBA46B105F'
$sync = Get-CheckedPackage 'https://thunderstore.io/package/download/shudnal/ConditionalConfigSync/1.0.4/' 'sync' 'F14D14216E7C6E32F70051952B49A93D515139811AC9317A00822B18B07CA5D0'

# Fetch the dedicated server anonymously from Valve; only use its assemblies as
# compiler references. No game files are committed or included in release assets.
$steam = Join-Path $deps 'steamcmd'
Invoke-WebRequest 'https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip' -OutFile (Join-Path $deps 'steamcmd.zip')
Expand-Archive (Join-Path $deps 'steamcmd.zip') $steam -Force
$server = Join-Path $deps 'server'
$managed = Join-Path $server 'valheim_server_Data/Managed'
for ($attempt = 1; $attempt -le 3; $attempt++) {
    & (Join-Path $steam 'steamcmd.exe') +force_install_dir $server +login anonymous +app_update 896660 validate +quit
    if ($LASTEXITCODE -eq 0 -and (Test-Path (Join-Path $managed 'assembly_valheim.dll'))) { break }
    if ($attempt -eq 3) { throw 'SteamCMD could not install Valheim dedicated-server references' }
}
$core = Get-ChildItem $bep -Filter BepInEx.dll -Recurse | Select-Object -First 1 -ExpandProperty DirectoryName
if (!$core) { throw 'BepInEx.dll missing from package' }
foreach ($file in @('assembly_valheim.dll', 'UnityEngine.CoreModule.dll', 'UnityEngine.dll')) {
    if (!(Test-Path (Join-Path $managed $file))) { throw "Missing game reference: $file" }
}
"GAME_MANAGED_PATH=$managed" | Out-File $env:GITHUB_ENV -Append -Encoding utf8
"BEPINEX_PATH=$core" | Out-File $env:GITHUB_ENV -Append -Encoding utf8
"SYNC_PATH=$sync" | Out-File $env:GITHUB_ENV -Append -Encoding utf8
