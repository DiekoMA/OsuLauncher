using API.Objects;
using Refit;

namespace API;

[Headers("Content-Type: application/json", "Accept: application/json")]
public interface IOsuClient
{
    [Get("/news")]
    Task<NewsResponse> GetNewsListingsAsync();

    [Get("/users/{userId}")]
    Task<User> GetUserAsync([Authorize("Bearer")] string token,int userId);
    
    [Get("/users/me")]
    Task<User> GetAuthenticatedUserAsync([Authorize("Bearer")] string token);

    [Get("/wiki/{locale}/{path}")]
    Task<Wiki> GetWikiAsync([Authorize("Bearer")] string token,string locale, string path);
}