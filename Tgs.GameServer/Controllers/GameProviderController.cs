using Microsoft.AspNetCore.Mvc;
using Tgs.GameServer.ApplicationServices;
using Tgs.GameServer.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameServer.Controllers;

[Route("gp")]
[ApiController]
public class GameProviderController : ControllerBase
{
	[HttpGet(Name = "ListGameProviders")]
	public IAsyncEnumerable<GameProviderViewModel> ListGameProvidersAsync(
		[FromServices] IQueryService<ListGameProvidersQuery, GameProvider> queryService)
		=> queryService.QueryAsync(new(), HttpContext.RequestAborted)
			.Select(gp => new GameProviderViewModel
			{
				Id = gp.Id,
				Code = gp.Code,
				Description = gp.Description
			});

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
