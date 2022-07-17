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

        var userId = 41;
        Console.WriteLine($"Full name of user with ID {userId} is {await _service.GetFullNameByIdAsync(userId)}");

        var age = 23;
        Console.WriteLine($"First name of users with age {age} is {await _service.GetFirstNamesByAgeAsync(age)}");

        Console.WriteLine("Age Gender statistics");
        var stats = await _service.GetGenderStatisticsByAge();
        foreach (var ageStat in stats)
        {
            Console.Write($"Age: {ageStat.Age}");
            foreach(var gender in ageStat.GenderStats)
            {
                Console.Write($" {gender.Gender}: {gender.Count}");
            }
            Console.WriteLine();
        }
        _logger.LogDebug("Closing application");
    }
}
