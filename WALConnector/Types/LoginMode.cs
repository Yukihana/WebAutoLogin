namespace AutoLoginConnector.Types;

public enum LoginMode : byte
{
    Disabled = 0,
    OnDisconnected = 1,
    IfAlternateRoute = 2,
    Always = 3,
}