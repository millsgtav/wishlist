param(
    [string]$Configuration = 'Release',
    [string]$Version = '1.0.0.0'
)

$ErrorActionPreference = 'Stop'
$project = Join-Path $PSScriptRoot 'Jellyfin.Plugin.Wishlist.csproj'
$output = Join-Path $PSScriptRoot "artifacts\jellyfin-wishlist_$Version"

dotnet publish $project --configuration $Configuration --output $output /p:Version=$Version
Compress-Archive -Path "$output\*" -DestinationPath "$output.zip" -Force
Write-Output "Created $output.zip"
