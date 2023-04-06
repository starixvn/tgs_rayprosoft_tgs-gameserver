namespace Tgs.GameServer.ViewModels;

public class GameProviderViewModel
{
	public required Guid Id { get; set; }

	public required string Code { get; set; }

	public string? Description { get; set; }
}
