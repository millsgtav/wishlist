# Jellyfin Wishlist

Wishlist is a Jellyfin administration plugin for keeping a shopping list of movies and TV shows. Search TMDb from the Jellyfin dashboard, save titles with their poster and release year, and remove items after you buy them.

## Features

- Search for movies and TV shows using TMDb.
- Sort the shopping list A-Z, Z-A, or by date added.
- Filter the list to all items, movies, or TV shows.
- Show an "Already in library" badge when Jellyfin finds a matching title.
- Rescan the library status without reloading the page.
- Use a compact list layout on mobile devices.

## Install from the plugin repository

1. In Jellyfin, open **Dashboard > Plugins > Repositories**.
2. Select **Add** and enter this repository URL:

	`https://raw.githubusercontent.com/millsgtav/wishlist/main/manifest.json`

3. Save the repository, then open the **Catalog** tab.
4. Find **Wishlist**, select **Install**, and restart Jellyfin when prompted.
5. Open **Dashboard > Plugins > Wishlist**.

The plugin requires Jellyfin 10.11 or later. The plugin API is restricted to Jellyfin users with administrator privileges.

## Configure TMDb search

Search requires a TMDb API key. Create an API key in your [TMDb account settings](https://www.themoviedb.org/settings/api), then:

1. Open **Dashboard > Plugins > Wishlist** in Jellyfin.
2. Expand **Settings**.
3. Enter the key and select **Save settings**.

The key is stored in Jellyfin's plugin configuration and is used by the Jellyfin server to call TMDb. It is not included in this repository, the plugin package, or the installation manifest. Do not paste your key into an issue, pull request, or public configuration file.

## Use the shopping list

Search for a title and select **Add to list**. Use the pills above the shopping list to change its sort order or filter by media type. Select **Rescan** after adding a title to your Jellyfin library; the badge is recalculated from the current library contents.

## Security and privacy

- No credentials or personal paths are required in the source code.
- TMDb searches are sent from the Jellyfin server to TMDb; the plugin does not send requests directly from a user's browser.
- Wishlist data and the TMDb key are stored in Jellyfin's own plugin configuration.
- Keep Jellyfin administrator access limited to trusted users, since administrators can access plugin settings and wishlist data.
