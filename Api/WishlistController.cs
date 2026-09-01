using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.Wishlist.Api;

[ApiController]
[Authorize(Policy = "RequiresElevation")]
[Route("Wishlist")]
public sealed class WishlistController : ControllerBase
{
    private const string TmdbImageBaseUrl = "https://image.tmdb.org/t/p/w342";
    private static readonly object ConfigurationLock = new();
    private readonly IHttpClientFactory _httpClientFactory;

    public WishlistController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<WishlistItem>> GetItems()
    {
        lock (ConfigurationLock)
        {
            return Ok(Plugin.Instance.Configuration.Items.OrderByDescending(item => item.AddedAt).ToList());
        }
    }

    [HttpPost]
    public ActionResult<WishlistItem> AddItem([FromBody] AddWishlistItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 300)
        {
            return BadRequest("A title of up to 300 characters is required.");
        }

        if (request.MediaType is not ("movie" or "tv"))
        {
            return BadRequest("Media type must be movie or tv.");
        }

        var item = new WishlistItem
        {
            Title = request.Title.Trim(),
            MediaType = request.MediaType,
            Year = request.Year,
            Overview = request.Overview,
            PosterUrl = request.PosterUrl,
            TmdbId = request.TmdbId,
        };

        lock (ConfigurationLock)
        {
            Plugin.Instance.Configuration.Items.Add(item);
            Plugin.Instance.SaveConfiguration(Plugin.Instance.Configuration);
        }

        return CreatedAtAction(nameof(GetItems), item);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteItem(Guid id)
    {
        lock (ConfigurationLock)
        {
            var item = Plugin.Instance.Configuration.Items.FirstOrDefault(candidate => candidate.Id == id);
            if (item is null)
            {
                return NotFound();
            }

            Plugin.Instance.Configuration.Items.Remove(item);
            Plugin.Instance.SaveConfiguration(Plugin.Instance.Configuration);
        }

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<TmdbSearchResult>>> SearchAsync([FromQuery] string query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Plugin.Instance.Configuration.TmdbApiKey))
        {
            return BadRequest("Set a TMDb API key in the Wishlist plugin settings first.");
        }

        if (string.IsNullOrWhiteSpace(query) || query.Length > 300)
        {
            return BadRequest("A search query of up to 300 characters is required.");
        }

        var escapedQuery = Uri.EscapeDataString(query.Trim());
        var apiKey = Uri.EscapeDataString(Plugin.Instance.Configuration.TmdbApiKey);
        var client = _httpClientFactory.CreateClient();
        using var response = await client.GetAsync(
            $"https://api.themoviedb.org/3/search/multi?api_key={apiKey}&query={escapedQuery}&include_adult=false",
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "TMDb search failed. Check the API key and try again.");
        }

        var payload = await response.Content.ReadFromJsonAsync<TmdbSearchResponse>(cancellationToken: cancellationToken);
        var results = payload?.Results
            .Where(result => result.MediaType is "movie" or "tv")
            .Take(10)
            .Select(result => new TmdbSearchResult
            {
                TmdbId = result.Id,
                Title = result.Title ?? result.Name ?? "Untitled",
                MediaType = result.MediaType!,
                Year = GetYear(result.ReleaseDate ?? result.FirstAirDate),
                Overview = result.Overview,
                PosterUrl = string.IsNullOrEmpty(result.PosterPath) ? null : TmdbImageBaseUrl + result.PosterPath,
            })
            .ToList() ?? [];

        return Ok(results);
    }

    private static int? GetYear(string? date)
    {
        return DateTime.TryParse(date, out var parsedDate) ? parsedDate.Year : null;
    }
}

public class AddWishlistItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public int? Year { get; set; }
    public string? Overview { get; set; }
    public string? PosterUrl { get; set; }
    public int? TmdbId { get; set; }
}

public sealed class TmdbSearchResponse
{
    public List<TmdbSearchItem> Results { get; set; } = [];
}

public sealed class TmdbSearchItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Name { get; set; }
    public string? MediaType { get; set; }
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public string? ReleaseDate { get; set; }
    public string? FirstAirDate { get; set; }
}

public sealed class TmdbSearchResult : AddWishlistItemRequest
{
}
