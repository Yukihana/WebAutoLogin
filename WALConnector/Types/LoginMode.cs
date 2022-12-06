namespace WALConnector.Types;

public enum LoginBehaviour : byte
{
    Disabled = 0,
    OnDisconnected = 1,
    IfAlternateRoute = 2,
    Always = 3,
}