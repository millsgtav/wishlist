using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Wishlist;

public sealed class PluginConfiguration : BasePluginConfiguration
{
    public string TmdbApiKey { get; set; } = string.Empty;

    public List<WishlistItem> Items { get; set; } = [];
}

public sealed class WishlistItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string MediaType { get; set; } = string.Empty;

    public int? Year { get; set; }

    public string? Overview { get; set; }

    public string? PosterUrl { get; set; }

    public int? TmdbId { get; set; }

    public DateTimeOffset AddedAt { get; set; } = DateTimeOffset.UtcNow;
}
