namespace AristotelisThesis.WPF.Services
{
    /// <summary>
    /// Connection settings for the local Python palmprint-encoding service.
    /// </summary>
    public static class PalmprintServiceConfig
    {
        public const string Host = "127.0.0.1";
        public const int Port = 8501; // face_service uses 8500
        public static string BaseUrl => $"http://{Host}:{Port}";

        // The palm-match distance threshold lives in AuthenticationService (the matcher).
    }
}
