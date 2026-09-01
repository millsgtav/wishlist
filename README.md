# Jellyfin Wishlist

A lightweight Jellyfin administration plugin for keeping a shopping list of movies and TV shows to buy. Search TMDb from the dashboard, add an item, and keep its poster, type, year, and overview with the list.

## Configure in Jellyfin

1. Build or install the plugin, then restart Jellyfin.
2. Open **Dashboard > Plugins > Wishlist**.
3. Enter your TMDb API key in the plugin settings and save it. The key is saved only in Jellyfin's plugin configuration, never in this repository.
4. Search for a title and add it to the shopping list.

## Build locally

Install the .NET 8 SDK, then run:

```powershell
.\build-plugin.ps1 -Version 1.0.0.0
```

Copy the resulting zip to your Jellyfin plugins directory, extract it, and restart the server. For Jellyfin 10.10.x, the plugin targets ABI `10.10.0.0`.

## Publish through GitHub

1. Create an empty GitHub repository and push this folder to its default branch.
2. Create and push a tag such as `v1.0.0.0`.
3. The included GitHub Actions workflow builds the zip, creates a GitHub release, and updates `manifest.json` with its URL and checksum.
4. In Jellyfin, add `https://raw.githubusercontent.com/<owner>/<repository>/<default-branch>/manifest.json` as a plugin repository, then install Wishlist from the catalog.

The workflow needs repository **Actions: Read and write permissions** so it can commit the updated manifest.
