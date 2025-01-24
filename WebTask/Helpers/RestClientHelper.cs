using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebTask.Web.Servicios
{
    public class RestClientHelper
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public RestClientHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration["APIClient"])
            };
        }

        public async Task<(byte[] fileData, string fileName)> GetBinaryRequestAsync(string endPoint, string token = null, Dictionary<string, string> queryParams = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endPoint);

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (queryParams != null)
            {
                var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                request.RequestUri = new Uri($"{request.RequestUri}?{queryString}");
            }

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var contentDisposition = response.Content.Headers.ContentDisposition;
                string fileName = contentDisposition?.FileName;

                var fileData = await response.Content.ReadAsByteArrayAsync();
                return (fileData, fileName);
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return (null, null);
            }
        }

        public async Task<T> GetRequestAsync<T>(string endPoint, string token = null, Dictionary<string, string> queryParams = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endPoint);

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (queryParams != null)
            {
                var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                request.RequestUri = new Uri($"{request.RequestUri}?{queryString}");
            }

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return default;
            }
        }

        public async Task<T> PostRequestAsync<T>(string endPoint, string token = null, object payload = null, Dictionary<string, string> queryParams = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endPoint);

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (queryParams != null)
            {
                var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                request.RequestUri = new Uri($"{request.RequestUri}?{queryString}");
            }

            if (payload != null)
            {
                var jsonContent = JsonSerializer.Serialize(payload);
                request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return default;
            }
        }

        public async Task<List<T>> ListPostRequestAsync<T>(string endPoint, string token = null, object payload = null, Dictionary<string, string> queryParams = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endPoint);

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (queryParams != null)
            {
                var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                request.RequestUri = new Uri($"{request.RequestUri}?{queryString}");
            }

            if (payload != null)
            {
                var jsonContent = JsonSerializer.Serialize(payload);
                request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<T>>(content);
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return default;
            }
        }
    }
}