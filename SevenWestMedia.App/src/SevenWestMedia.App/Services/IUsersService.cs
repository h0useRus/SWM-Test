using SevenWestMedia.App.Entities;

namespace SevenWestMedia.App.Services;

public interface IUsersService
{
    ValueTask<string> GetFullNameByIdAsync(int id);
    ValueTask<string> GetFirstNamesByAgeAsync(int age);
    ValueTask<IEnumerable<AgeStats>> GetGenderStatisticsByAge();
}
