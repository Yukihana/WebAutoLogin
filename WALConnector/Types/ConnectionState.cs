namespace WALConnector.Types;

public enum ConnectionState : byte
{
    Disconnected,
    Connected,
    AlternativeConnection,
    PartiallyConnected,
}