using Tgs.ServiceUtility;

namespace Tgs.GameServer.ApplicationServices;

internal class ListGameProvidersQueryService : IQueryService<ListGameProvidersQuery, GameProvider>
{
	private readonly ITgsRepository _tgsRepository;

	public ListGameProvidersQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public IAsyncEnumerable<GameProvider> QueryAsync(ListGameProvidersQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetGameProvidersAsync(cancellationToken);
}
