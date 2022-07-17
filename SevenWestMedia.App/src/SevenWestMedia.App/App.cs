using Microsoft.Extensions.Logging;
using SevenWestMedia.App.Services;

namespace SevenWestMedia.App;

internal class App : IApp
{
    private readonly ILogger<App> _logger;
    private readonly IUsersService _service;

    public App(IUsersService service, ILogger<App> logger)
    {
        _logger = logger;
        _service = service;
    }

    public async Task RunAsync()
    {
        _logger.LogDebug("Starting application");
        var userId = 42;
        Console.WriteLine($"Full name of user with ID {userId} is {await _service.GetFullNameByIdAsync(userId)}");
        _logger.LogDebug("Closing application");
    }
}
