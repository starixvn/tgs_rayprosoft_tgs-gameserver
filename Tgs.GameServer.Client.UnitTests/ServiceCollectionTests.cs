using Microsoft.Extensions.DependencyInjection;
using Tgs.GameServer.Clients;

namespace Tgs.GameServer.Client.UnitTests;

public class ServiceCollectionTests
{
	[Fact]
	public void 註冊GameServerClient()
	{
		// Arrange
		var services = new ServiceCollection();

		_ = services.AddGameServerClient(httpBuilder => httpBuilder
			.ConfigureHttpClient(http => http.BaseAddress = new Uri("https://localhost")));

		var sut = services.BuildServiceProvider();

		// Act
		var actual = sut.GetRequiredService<IGameServerClient>();

		// Assert
		_ = Assert.IsType<GameServerClient>(actual);
	}
}
