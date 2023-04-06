using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tgs.GameServer.Clients;

internal class GameServerClient : IGameServerClient
{
	private static readonly JsonSerializerOptions _JsonSerializerOptions = new(JsonSerializerDefaults.Web)
	{
		WriteIndented = false,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		Converters = { new JsonStringEnumConverter() }
	};

	private readonly HttpClient _httpClient;
	private readonly IEnumerable<IRequestContentHandler> _requestContentHandlers;

	public GameServerClient(HttpClient httpClient, IEnumerable<IRequestContentHandler> requestContentHandlers)
	{
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		_requestContentHandlers = requestContentHandlers ?? throw new ArgumentNullException(nameof(requestContentHandlers));
	}

	public async ValueTask SetGameProviderStatusAsync(Guid id, GameStatus status, CancellationToken cancellationToken = default)
	{
		var requestContent = JsonContent.Create<object?>(null);

		PrepareHeaders(requestContent.Headers);

		var response = await _httpClient.PatchAsync(
			$"/gp/status/{id}/{status}",
			requestContent,
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public async ValueTask<GameStatus?> GetGameProviderStatusAsync(Guid id, CancellationToken cancellationToken = default)
		=> await _httpClient.GetAsync($"/gp/status/{id}", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<GameStatus>(_JsonSerializerOptions)
				.ToAsyncEnumerable())
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(false);

	public IAsyncEnumerable<GameProvider> ListGameProvidersAsync(CancellationToken cancellationToken = default)
		=> _httpClient.GetAsync("/gp", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<GameProvider[]>(_JsonSerializerOptions)
				.ToAsyncEnumerable()
				.Where(arr => arr is not null)
				.SelectMany(arr => arr!.ToAsyncEnumerable()));

	private void PrepareHeaders(HttpContentHeaders headers)
	{
		foreach (var handler in _requestContentHandlers)
			handler.PrepareHeaders(headers);
	}
}
