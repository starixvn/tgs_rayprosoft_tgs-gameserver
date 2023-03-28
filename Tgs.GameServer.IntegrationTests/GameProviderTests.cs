using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tgs.GameServer.Clients;
using Tgs.GameServer.Repositories.Entities;
using Xunit.Abstractions;

namespace Tgs.GameServer.IntegrationTests;

public class GameProviderTests
{
	private readonly ITestOutputHelper _testOutputHelper;

	public GameProviderTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
	}

	[Fact]
	public async Task 設定GP狀態()
	{
		// Arrange
		await using var web = GameServerTestHost.Factory
			.WithWebHostBuilder(hostBuilder => hostBuilder
				.ConfigureLogging(logging => logging.AddXUnit(_testOutputHelper)));

		var modifier = "test1";

		var httpClient = web.CreateClient();

		var sut = new GameServerClient(
			httpClient,
			new[] {
				new StubRequestContentHandler(modifier)
			});

		var gpId = Guid.NewGuid();
		var status = GameStatus.Maintenance;

		var dbContext = web.Services.GetRequiredService<TgsContext>();

		var targetGp = new GameProvider
		{
			Id = gpId,
			Status = (short)GameStatus.Active,
			Code = "aaaa",
			Name = "AAAA",
			CreatedByUsername = "system",
			UpdatedByUsername = "system"
		};

		_ = await dbContext.GameProviders.AddAsync(targetGp);

		_ = await dbContext.SaveChangesAsync();

		// Act
		await sut.SetGameProviderStatusAsync(gpId, status);

		// Assert
		Assert.Equal((short)status, targetGp.Status);
		Assert.Equal(modifier, targetGp.UpdatedByUsername);
	}

	[Fact]
	public async Task 取得GP狀態()
	{
		// Arrange
		await using var web = GameServerTestHost.Factory
			.WithWebHostBuilder(hostBuilder => hostBuilder
				.ConfigureLogging(logging => logging.AddXUnit(_testOutputHelper)));

		var httpClient = web.CreateClient();

		var sut = new GameServerClient(
			httpClient,
			Array.Empty<IRequestContentHandler>());

		var gpId = Guid.NewGuid();

		var dbContext = web.Services.GetRequiredService<TgsContext>();

		var targetGp = new GameProvider
		{
			Id = gpId,
			Status = (short)GameStatus.Inactive,
			Code = "aaaa",
			Name = "AAAA",
			CreatedByUsername = "system",
			UpdatedByUsername = "system"
		};

		_ = await dbContext.GameProviders.AddAsync(targetGp);

		_ = await dbContext.SaveChangesAsync();

		// Act
		var actual = await sut.GetGameProviderStatusAsync(gpId);

		// Assert
		Assert.Equal(GameStatus.Inactive, actual);
	}
}
