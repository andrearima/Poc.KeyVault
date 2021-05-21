using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HashiCorpVault
{
    public class HashiVaultHttpService
    {
        private readonly string baseUrl = "http://localhost:8200/secrets/kv";
        private HttpClient _client;
        public HashiVaultHttpService(HttpClient client)
        {
            _client = client;
        }
        public void teste()
        {
            _client.DefaultRequestHeaders.Add("X-Vault-Token", "s.Z0KD6AfX484usffPE00WSBVC");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonSerializer.Serialize(new { key = "teste", value = "teste" });
            //StringContent content = new StringContent(json);
            //var response = _client.PostAsync(baseUrl + "/secret/mySecretPath", content).GetAwaiter().GetResult() ;

            var response = _client.GetAsync(baseUrl).GetAwaiter().GetResult();
        }
    }
}
