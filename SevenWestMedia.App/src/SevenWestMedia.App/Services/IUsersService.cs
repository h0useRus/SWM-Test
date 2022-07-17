namespace SevenWestMedia.App.Services;

public interface IUsersService
{
    ValueTask<string> GetFullNameByIdAsync(int id);
    ValueTask<string> GetFirstNamesByAgeAsync(int age);
}
