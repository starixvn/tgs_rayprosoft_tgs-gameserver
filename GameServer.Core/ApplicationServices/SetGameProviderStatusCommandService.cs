using Tgs.ServiceUtility;

namespace Tgs.GameServer.ApplicationServices;

internal class SetGameProviderStatusCommandService : ICommandService<SetGameProviderStatusCommand>
{
	private readonly ITgsRepository _tgsRepository;

	public SetGameProviderStatusCommandService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask ExecuteAsync(SetGameProviderStatusCommand command, CancellationToken cancellationToken = default)
		=> _tgsRepository.UpdateGameProviderStatusAsync(
			command.GameProviderId,
			command.Status,
			command.Modifier,
			cancellationToken);
}
