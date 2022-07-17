using Microsoft.Extensions.Logging;
using SevenWestMedia.App.Entities;
using SevenWestMedia.App.Repositories;

namespace SevenWestMedia.App.Services;

internal class UsersService : IUsersService
{
    private readonly ILogger<UsersService> _logger;
    private readonly IUsersRepository _repository;

    public UsersService(IUsersRepository repository, ILogger<UsersService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async ValueTask<string> GetFirstNamesByAgeAsync(int age)
    {
        var users = await _repository.GetUsersAsync();
        return string.Join(',', users.Where(u => u.Age == age).Select(u=>u.First));
    }

    public async ValueTask<string> GetFullNameByIdAsync(int id)
    {
        var users = await _repository.GetUsersAsync();
        var user = users.FirstOrDefault(u => u.Id == id);

        await GetGenderStatisticsByAge();

        return user != null ? $"{user.First} {user.Last}" : "Not found";
    }

    public async ValueTask<IEnumerable<AgeStats>> GetGenderStatisticsByAge()
    {
        var users = await _repository.GetUsersAsync();

        return users
            .GroupBy(u => u.Age)
            .Select(g =>
            {
                var ageStat = new AgeStats(g.Key);
                ageStat.Add(g.Select(u =>
                {
                    return new GenderStats(u.Gender, g.Count(t => t.Gender == u.Gender));
                }));
                return ageStat;
            });
    }

}
