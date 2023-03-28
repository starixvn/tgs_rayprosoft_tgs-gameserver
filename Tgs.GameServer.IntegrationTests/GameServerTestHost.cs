using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tgs.GameServer.Repositories.Entities;

namespace Tgs.GameServer.IntegrationTests;

internal static class GameServerTestHost
{
	public static WebApplicationFactory<Program> Factory => new WebApplicationFactory<Program>()
		.WithWebHostBuilder(hostBuilder => hostBuilder
			.ConfigureAppConfiguration((_, configBuilder) => configBuilder.Sources.Clear())
			.ConfigureServices(services => services
				.Replace(ServiceDescriptor.Singleton(sp =>
				{
					var dbContextOptionsBuilder = new DbContextOptionsBuilder<TgsContext>();
					_ = dbContextOptionsBuilder
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.ConfigureWarnings(x => x.Ignore(
							InMemoryEventId.TransactionIgnoredWarning));

					return new TgsContext(dbContextOptionsBuilder.Options);
				}))));
}
