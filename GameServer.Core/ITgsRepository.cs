namespace Tgs.GameServer;

public interface ITgsRepository
{
	ValueTask UpdateGameProviderStatusAsync(
		Guid id,
		GameStatus status,
		string modifier,
		CancellationToken cancellationToken = default);

	ValueTask<GameStatus?> GetGameProviderStatusAsync(Guid id, CancellationToken cancellationToken = default);
}
