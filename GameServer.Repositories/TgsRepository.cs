using Tgs.GameServer.Repositories.Entities;

namespace Tgs.GameServer.Repositories;

internal class TgsRepository : ITgsRepository
{
	private readonly TgsContext _dbContext;

	public TgsRepository(TgsContext dbContext)
	{
		_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public async ValueTask UpdateGameProviderStatusAsync(
		Guid id,
		GameStatus status,
		string modifier,
		CancellationToken cancellationToken = default)
	{
		var gpData = await _dbContext.GameProviders.FindAsync(
			new object?[] { id },
			cancellationToken: cancellationToken).ConfigureAwait(false);

		if (gpData is null)
			return;

		gpData.Status = (short)status;
		gpData.UpdatedByUsername = modifier;
		gpData.UpdatedOn = DateTime.UtcNow;

		_ = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask<GameStatus?> GetGameProviderStatusAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var gpData = await _dbContext.GameProviders.FindAsync(
			new object?[] { id },
			cancellationToken: cancellationToken).ConfigureAwait(false);

		return gpData is null ? null : (GameStatus)gpData.Status;
	}
}
