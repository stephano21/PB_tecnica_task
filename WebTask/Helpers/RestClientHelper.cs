using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;

namespace WebTask.Web.Servicios
{
    public class RestClientHelper
    {
        private readonly IConfiguration _configuration;
        private readonly RestClient _restClient;

        public RestClientHelper(IConfiguration configuration)
        {
            _configuration = configuration;

            // Configura el cliente RestSharp
            var options = new RestClientOptions(_configuration["APIClient"])
            {
                ThrowOnAnyError = true,
                MaxTimeout = 120000 // Timeout en milisegundos
            };

            _restClient = new RestClient(options, configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // Mantener PascalCase
            }));
        }

        public async Task<(byte[] fileData, string fileName)> GetBinaryRequestAsync(string endPoint, string token = null, Dictionary<string, string> queryParams = null)
        {
            var request = new RestRequest(endPoint, Method.Get);

            if (token != null)
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }

            if (queryParams != null)
            {
                foreach (var param in queryParams)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            var response = await _restClient.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var contentDisposition = response.ContentHeaders.FirstOrDefault(h => h.Name == "Content-Disposition")?.Value?.ToString();
                string fileName = contentDisposition?.Split("filename=")[1].Trim('"');

                return (response.RawBytes, fileName);
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return (null, null);
            }
        }

        public async Task<T> GetRequestAsync<T>(string endPoint, string token = null, Dictionary<string, string> queryParams = null)
        {
            var request = new RestRequest(endPoint, Method.Get);

            if (token != null)
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }

            if (queryParams != null)
            {
                foreach (var param in queryParams)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            var response = await _restClient.ExecuteAsync<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return default;
            }
        }

        public async Task<T> PostRequestAsync<T>(string endPoint, string token = null, object payload = null, Dictionary<string, string> queryParams = null)
        {
            var request = new RestRequest(endPoint, Method.Post);

            if (token != null)
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }

            if (queryParams != null)
            {
                foreach (var param in queryParams)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            var response = await _restClient.ExecuteAsync<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return default;
            }
        }

        public async Task<List<T>> ListPostRequestAsync<T>(string endPoint, string token = null, object payload = null, Dictionary<string, string> queryParams = null)
        {
            var request = new RestRequest(endPoint, Method.Post);

            if (token != null)
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }

            if (queryParams != null)
            {
                foreach (var param in queryParams)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            var response = await _restClient.ExecuteAsync<List<T>>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return default;
            }
        }
        public async Task<T> PutRequestAsync<T>(string endPoint, string token = null, object payload = null, Dictionary<string, string> queryParams = null)
        {
            var request = new RestRequest(endPoint, Method.Put);

            if (token != null)
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }

            if (queryParams != null)
            {
                foreach (var param in queryParams)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            var response = await _restClient.ExecuteAsync<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return default;
            }
        }

        public async Task<bool> DeleteRequestAsync(string endPoint, string token = null, Dictionary<string, string> queryParams = null)
        {
            var request = new RestRequest(endPoint, Method.Delete);

            if (token != null)
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }

            if (queryParams != null)
            {
                foreach (var param in queryParams)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            var response = await _restClient.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"La solicitud falló con el código: {response.StatusCode}");
                return false;
            }
        }
    }
}