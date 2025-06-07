﻿namespace FutFut.Common.Settings;

public class AWS3Settings
{
    public string AccessKey { get; init; } = String.Empty;
    public string SecretKey { get; init; } = String.Empty;
    public string Region { get; init; } = String.Empty;
    public string Bucket { get; init; } = String.Empty;
    public string Endpoint { get; init; } = String.Empty;
    public bool UseR2 { get; init; }
}