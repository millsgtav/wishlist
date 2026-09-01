using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Wishlist;

public sealed class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    public static Plugin Instance { get; private set; } = null!;

    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    public override string Name => "Wishlist";

    public override Guid Id => Guid.Parse("611ce32a-d57d-4573-a7ec-bf4748b708f9");

    public IEnumerable<PluginPageInfo> GetPages()
    {
        return
        [
            new PluginPageInfo
            {
                Name = "wishlist",
                EmbeddedResourcePath = "Jellyfin.Plugin.Wishlist.Configuration.configPage.html",
            },
        ];
    }
}
