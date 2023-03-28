using Microsoft.AspNetCore.Mvc;
using Tgs.GameServer.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameServer.Controllers;

[Route("gp")]
[ApiController]
public class GameProviderController : ControllerBase
{
	[HttpPatch("status/{id}/{status}", Name = "SetGameProviderStatus")]
	public ValueTask SetGameProviderStatusAsync(
		[FromServices] ICommandService<SetGameProviderStatusCommand> setStatusService,
		[FromHeader(Name = "Modifier")] string modifier,
		Guid id,
		GameStatus status)
	{
		var command = new SetGameProviderStatusCommand(id, status, modifier);

		return setStatusService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpGet("status/{id}", Name = "GetGameProviderStatus")]
	public ValueTask<GameStatus?> GetGameProviderStatusAsync(
		[FromServices] IGetService<GetGameProviderStatusQuery, GameStatus?> getService,
		Guid id)
	{
		var query = new GetGameProviderStatusQuery(id);

		return getService.GetAsync(query, HttpContext.RequestAborted);
	}
}
