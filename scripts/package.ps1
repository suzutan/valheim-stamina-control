param(
    [Parameter(Mandatory)][ValidatePattern('^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)$')][string]$Version
)
$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$dll = Join-Path $repoRoot 'Source/bin/Release/net472/StaminaControl.dll'
$assemblyVersion = [Reflection.AssemblyName]::GetAssemblyName($dll).Version
if ($assemblyVersion.ToString(3) -ne $Version) { throw "DLL version $assemblyVersion does not match $Version" }
$staging = Join-Path $repoRoot ('artifacts/staging-' + [guid]::NewGuid().ToString('N'))
$pluginDir = Join-Path $staging 'BepInEx/plugins/StaminaControl'
New-Item -ItemType Directory -Path $pluginDir -Force | Out-Null
Copy-Item -LiteralPath $dll -Destination $pluginDir
foreach ($name in @('README.md', 'CHANGELOG.md', 'LICENSE')) {
    Copy-Item -LiteralPath (Join-Path $repoRoot $name) -Destination $staging
}
# Include the linked documentation so the README works when read from the ZIP.
Copy-Item -LiteralPath (Join-Path $repoRoot 'docs') -Destination $staging -Recurse
$zip = Join-Path $repoRoot "artifacts/StaminaControl-$Version.zip"
Compress-Archive -Path (Join-Path $staging '*') -DestinationPath $zip -Force
$archive = [IO.Compression.ZipFile]::OpenRead($zip)
try {
    $dlls = @($archive.Entries | Where-Object { $_.FullName.EndsWith('.dll') })
    if ($dlls.Count -ne 1 -or $dlls[0].FullName.Replace('\','/') -ne 'BepInEx/plugins/StaminaControl/StaminaControl.dll') {
        throw 'Unexpected DLL in release archive'
    }
} finally { $archive.Dispose() }
$hash = (Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash.ToLowerInvariant()
"$hash  StaminaControl-$Version.zip" | Set-Content -LiteralPath "$zip.sha256" -Encoding utf8NoBOM
Write-Output $zip
