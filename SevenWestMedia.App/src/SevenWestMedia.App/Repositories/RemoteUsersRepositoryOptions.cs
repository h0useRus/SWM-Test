namespace SevenWestMedia.App.Repositories;

internal class RemoteUsersRepositoryOptions
{
    public bool UseCache { get; set; } = true;
    public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromMinutes(1);
}
