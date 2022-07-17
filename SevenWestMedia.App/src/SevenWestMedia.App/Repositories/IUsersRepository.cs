using SevenWestMedia.App.Entities;

namespace SevenWestMedia.App.Repositories;

public interface IUsersRepository
{
    ValueTask<List<User>> GetUsersAsync();
}
