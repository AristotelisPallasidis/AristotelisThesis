using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AristotelisThesis.Domain.Services;

namespace AristotelisThesis.WPF.Services
{
    /// <summary>
    /// Calls the local Python FastAPI service to encode faces into 128-d embeddings.
    /// Registered as a singleton so the underlying <see cref="HttpClient"/> is reused.
    /// </summary>
    public class PythonFaceRecognitionService : IFaceRecognitionService
    {
        private readonly HttpClient _http;

        public PythonFaceRecognitionService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(FaceServiceConfig.BaseUrl),
                Timeout = TimeSpan.FromSeconds(15)
            };
        }

        public async Task<float[]?> EncodeAsync(byte[] imageBytes)
        {
            try
            {
                using var content = new ByteArrayContent(imageBytes);
                using HttpResponseMessage response = await _http.PostAsync("/encode", content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                await using System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                using JsonDocument doc = await JsonDocument.ParseAsync(stream);
                JsonElement root = doc.RootElement;

                if (!root.TryGetProperty("found", out JsonElement found) || !found.GetBoolean())
                {
                    return null;
                }

                JsonElement arr = root.GetProperty("embedding");
                float[] embedding = new float[arr.GetArrayLength()];
                int i = 0;
                foreach (JsonElement v in arr.EnumerateArray())
                {
                    embedding[i++] = v.GetSingle();
                }
                return embedding;
            }
            catch
            {
                // Service down / network error / malformed response: treat as "no encoding".
                return null;
            }
        }

        public async Task<bool> IsHealthyAsync()
        {
            try
            {
                using HttpResponseMessage response = await _http.GetAsync("/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
