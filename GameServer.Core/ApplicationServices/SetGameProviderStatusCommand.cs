namespace Tgs.GameServer.ApplicationServices;
public record SetGameProviderStatusCommand(Guid GameProviderId, GameStatus Status, string Modifier);
