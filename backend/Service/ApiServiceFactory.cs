using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Service.Integration;

namespace DiplomApplication
{
    public interface IApiServiceFactory
    {
        IApiService Create(PlatformType platform);
    }

    public class ApiServiceFactory : IApiServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ApiServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IApiService Create(PlatformType platform)
        {
            return platform switch
            {
                PlatformType.Steam => _serviceProvider.GetRequiredService<SteamApiService>(),
                PlatformType.EpicGames => _serviceProvider.GetRequiredService<EpicGamesApiService>(),
                _ => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
            };
        }
    }
}
