namespace WALConnector.Services.PingStats;

internal class PingOptions
{
    public int Timeout { get; set; } = 1000;
    public int MaximumPingsCount { get; set; } = 100;
}