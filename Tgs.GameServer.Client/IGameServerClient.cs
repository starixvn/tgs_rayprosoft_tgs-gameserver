namespace Tgs.GameServer.Clients;

public interface IGameServerClient
{
	IAsyncEnumerable<GameProvider> ListGameProvidersAsync(CancellationToken cancellationToken = default);

	ValueTask SetGameProviderStatusAsync(Guid id, GameStatus status, CancellationToken cancellationToken = default);

	ValueTask<GameStatus?> GetGameProviderStatusAsync(Guid id, CancellationToken cancellationToken = default);
}
