using NSubstitute;
using Tgs.GameServer;
using Tgs.GameServer.ApplicationServices;

namespace GameServer.Core.UnitTests;

public class GameProviderTests
{
	[Fact]
	public async Task 查詢所有的GameProvider資料()
	{
		// Arrange
		var fakeTgsRepository = Substitute.For<ITgsRepository>();

		var sut = new ListGameProvidersQueryService(fakeTgsRepository);

		var query = new ListGameProvidersQuery();

		// Act
		_ = await sut.QueryAsync(query).ToArrayAsync();

		// Assert
		_ = fakeTgsRepository.Received(1)
			.GetGameProvidersAsync();
	}

	[Fact]
	public async Task 設定GameProvider狀態()
	{
		// Arrange
		var fakeTgsRepository = Substitute.For<ITgsRepository>();

		var sut = new SetGameProviderStatusCommandService(fakeTgsRepository);

		var command = new SetGameProviderStatusCommand(Guid.NewGuid(), GameStatus.Active, "modifier");

		// Act
		await sut.ExecuteAsync(command);

		// Assert
		_ = fakeTgsRepository.Received(1)
			.UpdateGameProviderStatusAsync(
				Arg.Is(command.GameProviderId),
				Arg.Is(command.Status),
				Arg.Is(command.Modifier));
	}

	[Fact]
	public async Task 取得GameProvider狀態()
	{
		// Arrange
		var fakeTgsRepository = Substitute.For<ITgsRepository>();

		var sut = new GetGameProviderStatusQueryService(fakeTgsRepository);

		var query = new GetGameProviderStatusQuery(Guid.NewGuid());

		// Act
		_ = await sut.GetAsync(query);

		// Assert
		_ = fakeTgsRepository.Received(1)
			.GetGameProviderStatusAsync(Arg.Is(query.GameProviderId));
	}
}
