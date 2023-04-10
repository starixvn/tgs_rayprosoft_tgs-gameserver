using Tgs.GameServer.Clients;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameServerClient(this IServiceCollection services, Action<IHttpClientBuilder> buildHttpClient)
	{
		buildHttpClient?.Invoke(services.AddHttpClient<IGameServerClient, GameServerClient>());

		return services;
	}
}
