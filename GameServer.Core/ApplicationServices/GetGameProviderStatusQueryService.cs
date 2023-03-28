using Tgs.ServiceUtility;

namespace Tgs.GameServer.ApplicationServices;

internal class GetGameProviderStatusQueryService : IGetService<GetGameProviderStatusQuery, GameStatus?>
{
	private readonly ITgsRepository _tgsRepository;

	public GetGameProviderStatusQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask<GameStatus?> GetAsync(GetGameProviderStatusQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetGameProviderStatusAsync(query.GameProviderId, cancellationToken);
}
