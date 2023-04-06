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
	public async Task 取得所有的GP列表()
	{
		// Arrange
		await using var web = GameServerTestHost.Factory
			.WithWebHostBuilder(hostBuilder => hostBuilder
				.ConfigureLogging(logging => logging.AddXUnit(_testOutputHelper))
				.ConfigureServices(services => services
					.AddHttpClientLoggingHandler()));

		var httpClient = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<GameServerClient>());

		var sut = new GameServerClient(
			httpClient,
			Array.Empty<IRequestContentHandler>());

		var expected = new[]
		{
			new GameProvider(Guid.NewGuid(), "GP1", null),
			new GameProvider(Guid.NewGuid(), "GP2", null)
		};

		var dbContext = web.Services.GetRequiredService<TgsContext>();

		await dbContext.GameProviderRows.AddRangeAsync(
			new GameProviderRow
			{
				Id = expected[0].Id,
				Code = expected[0].Code,
				Name = "test",
				CreatedByUsername = "sys",
				UpdatedByUsername = "sys"
			},
			new GameProviderRow
			{
				Id = expected[1].Id,
				Code = expected[1].Code,
				Name = "test",
				CreatedByUsername = "sys",
				UpdatedByUsername = "sys"
			});

		_ = await dbContext.SaveChangesAsync();

		// Act
		var actual = await sut.ListGameProvidersAsync().ToArrayAsync();

		// Assert
		Assert.Equal(expected, actual);
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

		var targetGp = new GameProviderRow
		{
			Id = gpId,
			Status = (short)GameStatus.Active,
			Code = "aaaa",
			Name = "AAAA",
			CreatedByUsername = "system",
			UpdatedByUsername = "system"
		};

		_ = await dbContext.GameProviderRows.AddAsync(targetGp);

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

		var targetGp = new GameProviderRow
		{
			Id = gpId,
			Status = (short)GameStatus.Inactive,
			Code = "aaaa",
			Name = "AAAA",
			CreatedByUsername = "system",
			UpdatedByUsername = "system"
		};

		_ = await dbContext.GameProviderRows.AddAsync(targetGp);

		_ = await dbContext.SaveChangesAsync();

		// Act
		var actual = await sut.GetGameProviderStatusAsync(gpId);

		// Assert
		Assert.Equal(GameStatus.Inactive, actual);
	}
}
