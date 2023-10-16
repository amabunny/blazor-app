using System.Runtime.Serialization.Json;

namespace Star_Wars_DataBase.Data.Utility
{
    public class HttpClientCache
    {
        private readonly HttpClient _client;
        private readonly string _key;

        public HttpClientCache(HttpClient client, string key)
        {
            _client = client;
            _key = key;
        }

        private async Task<byte[]> GetEntityAsync(string url)
        {
            return await _client.GetByteArrayAsync(url);
        }

        public async Task<T> GetFromCacheOrRequest<T>(string url) where T : class, new()
        {
            var filePath = Path.Combine("wwwroot", "cache");
            DataContractJsonSerializer serializer = new(typeof(T));

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var lowerCaseUrl = url.Split("/");
            var fileName = Path.Combine(filePath, _key + string.Join('_', lowerCaseUrl));
            byte[] cacheBytes;

            if (!File.Exists(fileName))
            {
                cacheBytes = await GetEntityAsync(url);

                await using var fs = File.Create(fileName);
                await fs.WriteAsync(cacheBytes);
            }
            else
            {
                cacheBytes = await File.ReadAllBytesAsync(fileName);
            }

            var ms = new MemoryStream(cacheBytes);
            return serializer.ReadObject(ms) as T ?? new();
        }
    }
}