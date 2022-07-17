using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SevenWestMedia.App.Entities;
using System.Net.Http.Json;

namespace SevenWestMedia.App.Repositories;

internal class RemoteUsersRepository : IUsersRepository
{
    private const string cacheKey = "users";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly RemoteUsersRepositoryOptions _otions;
    private readonly ILogger<RemoteUsersRepository> _logger;

    public RemoteUsersRepository(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache,  IOptions<RemoteUsersRepositoryOptions> options, ILogger<RemoteUsersRepository> logger)
    {
        _otions = options.Value;
        _logger = logger;
        _httpClientFactory = httpClientFactory;    
        _memoryCache = memoryCache;
    }

    public async ValueTask<List<User>> GetUsersAsync()
    {
        if(_otions.UseCache)
        {
            var cachedUsers = _memoryCache.Get<List<User>>(cacheKey);
            if (cachedUsers != null)
            {
                _logger.LogDebug($"Getting users form the memory cache");
                return cachedUsers;
            }
        }
        _logger.LogDebug($"Getting users from remote api");

        //var users = await _httpClientFactory.CreateClient("UsersApi").GetFromJsonAsync<List<User>>("/sampletest");

        using var response = await _httpClientFactory.CreateClient("UsersApi").GetAsync("/sampletest");
        if(response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                Error = (sender, args) =>
                {
                    _logger.LogDebug($"JSON Parsing Error. Type={args.CurrentObject} Member={args.ErrorContext.Member} Path={args.ErrorContext.Path}  Error={args.ErrorContext.Error.Message}");
                }
            };
            var users = JsonConvert.DeserializeObject<List<User>>(json, jsonSerializerSettings);

            if (users != null)
            {
                _logger.LogDebug($"Caching users into the memory");
                _memoryCache.Set(cacheKey, users, _otions.CacheExpiration);
                return users;
            }
        }

        _logger.LogDebug($"Problem with getting users, returning empty list");
        return new List<User>();
    }
}
