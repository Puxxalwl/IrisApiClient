using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace IrisClient.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> getWithRetryAsync<T>(this HttpClient Client, string url,
            Dictionary<string, object>? Params = null,
            int maxRetries = 3)
        {
            var queryString = Params != null
                ? "?" + string.Join("&", Params.Select(p => $"{p.Key}={Uri.EscapeDataString(FormatValue(p.Value))}"))
                : "";

            var fullUrl = $"{url}{queryString}";

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var response = await Client.GetAsync(fullUrl);

                    var content = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var apiError = JsonSerializer.Deserialize<ApiError>(content);
                            throw new Exception($"API ошибка {apiError?.error_code}: {apiError?.description}");
                        }
                        catch
                        {
                            throw new HttpRequestException($"HTTP ошибка {response.StatusCode}: {content}");
                        }
                    }

                    var result = JsonSerializer.Deserialize<T>(content);
                    return result ?? throw new InvalidOperationException("Десериализация не удалась");
                }
                catch (HttpRequestException) when (attempt < maxRetries)
                {
                    await Task.Delay(1000 * attempt);
                }
            }

            throw new HttpRequestException("Выполнены все попытки, запрос не выполнен");
        }

        private static string FormatValue(object value)
        {
            return value switch
            {
                float f => f.ToString(CultureInfo.InvariantCulture),
                double d => d.ToString(CultureInfo.InvariantCulture),
                long l => l.ToString(CultureInfo.InvariantCulture),
                int i => i.ToString(CultureInfo.InvariantCulture),
                bool b => b ? "1" : "0",
                string s => s,
                _ => value.ToString() ?? ""
            };
        }
    }

    public class ApiError
    {
        public int error_code { get; set; }
        public string description { get; set; } = "";
    }
}