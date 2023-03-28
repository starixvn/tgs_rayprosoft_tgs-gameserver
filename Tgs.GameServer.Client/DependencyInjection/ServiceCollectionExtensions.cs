using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Tgs.GameServer.Clients;

namespace Microsoft.Extensions.DependencyInjection;
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameServerClient(this IServiceCollection services)
	{
		return services
				.AddHttpClient<IGameServerClient, GameServerClient>()
				.Services;
	}
}
