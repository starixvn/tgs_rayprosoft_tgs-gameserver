using System.Net.Http.Headers;

namespace Tgs.GameServer.Clients;

public interface IRequestContentHandler
{
	void PrepareHeaders(HttpContentHeaders headers);
}
