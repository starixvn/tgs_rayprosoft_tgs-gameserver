using Microsoft.EntityFrameworkCore;
using Tgs.GameServer;
using Tgs.GameServer.Repositories;
using Tgs.GameServer.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameServerRepositories(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> dbConfigure)
		=> services
			.AddDbContext<TgsContext>(dbConfigure)
			.AddTransient<ITgsRepository, TgsRepository>();
}
