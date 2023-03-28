namespace Tgs.GameServer.Clients;

public interface IGameServerClient
{
	ValueTask SetGameProviderStatusAsync(Guid id, GameStatus status, CancellationToken cancellationToken = default);

	ValueTask<GameStatus?> GetGameProviderStatusAsync(Guid id, CancellationToken cancellationToken = default);
}
