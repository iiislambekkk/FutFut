namespace FutFut.Common.Settings;

public class EFCoreSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string Database { get; init; }

    public string ConnectionString => $"Host={Host};Port={Port};Username={UserName};Password={Password};Database={Database}";
}