using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using SevenWestMedia.App;
using SevenWestMedia.App.Repositories;
using SevenWestMedia.App.Services;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, configuration) =>
    {
        IHostEnvironment env = context.HostingEnvironment;

        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

    })
    .ConfigureServices((context, services) =>
    {
        services
            // Configuration
            .Configure<RemoteUsersRepositoryOptions>(context.Configuration.GetSection(nameof(RemoteUsersRepositoryOptions)))
            // Adding services
            .AddSingleton<IMemoryCache, MemoryCache>()
            .AddSingleton<IUsersRepository, RemoteUsersRepository>()
            .AddSingleton<IUsersService, UsersService>()
            .AddSingleton<IApp, App>();

        services.AddHttpClient("UsersApi", client =>
        {
            client.BaseAddress = new Uri(context.Configuration["UsersApi:Url"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddPolicyHandler(
            Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .RetryAsync(int.Parse(context.Configuration["UsersApi:RetryAttempts"])));

    })
    .Build()
    .Services
    .GetRequiredService<IApp>()
    .RunAsync();

