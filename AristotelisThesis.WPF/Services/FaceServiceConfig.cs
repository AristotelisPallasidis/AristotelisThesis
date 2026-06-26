namespace AristotelisThesis.WPF.Services
{
    /// <summary>
    /// Connection settings for the local Python face-embedding service.
    /// </summary>
    public static class FaceServiceConfig
    {
        public const string Host = "127.0.0.1";
        public const int Port = 8500;
        public static string BaseUrl => $"http://{Host}:{Port}";

        // The face-match distance threshold lives in AuthenticationService (the matcher),
        // since matching is done in the domain layer.
    }
}
