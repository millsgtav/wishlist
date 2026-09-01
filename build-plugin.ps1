param(
    [string]$Configuration = 'Release',
    [string]$Version
)

$ErrorActionPreference = 'Stop'
$project = Join-Path $PSScriptRoot 'Jellyfin.Plugin.Wishlist.csproj'

if (-not $Version) {
    # Default to the version declared in the csproj so the code is the single source of truth
    $Version = ([xml](Get-Content $project -Raw)).Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1
}

$output = Join-Path $PSScriptRoot "artifacts\jellyfin-wishlist_$Version"

dotnet publish $project --configuration $Configuration --output $output /p:Version=$Version
Compress-Archive -Path "$output\*" -DestinationPath "$output.zip" -Force
Write-Output "Created $output.zip"
