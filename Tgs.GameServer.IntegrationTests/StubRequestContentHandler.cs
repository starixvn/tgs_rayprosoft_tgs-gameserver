using System.Net.Http.Headers;
using Tgs.GameServer.Clients;

namespace Tgs.GameServer.IntegrationTests;

internal class StubRequestContentHandler : IRequestContentHandler
{
	private readonly string _modifier;

	public StubRequestContentHandler(string modifier)
	{
		if (string.IsNullOrEmpty(modifier))
			throw new ArgumentException($"'{nameof(modifier)}' 不可為 Null 或空白。", nameof(modifier));
		_modifier = modifier;
	}

	public void PrepareHeaders(HttpContentHeaders headers)
	{
		headers.Add("Modifier", _modifier);
	}
}
