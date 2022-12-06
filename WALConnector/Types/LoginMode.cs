namespace WALConnector.Types;

public enum LoginBehaviour : byte
{
    Disabled = 0,
    OnDisconnected = 1,
    Always = 2,
}