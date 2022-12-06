namespace WALConnector.Types;

public enum PingGroupPollingMode : byte
{
    AllConcurrent = 0,
    SeriallyAlternated = 1,
}