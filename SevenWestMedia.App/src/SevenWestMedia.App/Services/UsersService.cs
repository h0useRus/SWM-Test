using Microsoft.Extensions.Logging;
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
        return string.Empty;
    }

    public async ValueTask<string> GetFullNameByIdAsync(int id)
    {
        var users = await _repository.GetUsersAsync();
        var user = users.FirstOrDefault(u => u.Id == id);
        return user != null ? $"{user.First} {user.Last}" : "Not found";
    }
}
