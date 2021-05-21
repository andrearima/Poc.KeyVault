using Consul;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsulKeyVault
{
    /// <summary>
    /// Sem uso de bibliotecas, apenas HTTP Client
    /// </summary>
    public class KeyVaultHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrlSecure = "https://localhost:8501/v1/kv/";
        private readonly string baseUrl = "http://localhost:8502/v1/kv/";
        private readonly string TokenMASTER = "527351b7-1239-1301-2576-b80a668391be";
        private readonly string TokenAdmin = "cfe66bda-e181-a627-f0f9-4f59095e9980";
        public KeyVaultHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("X-Consul-Token", TokenAdmin);
        }
        public async Task<bool> CreateOrUpdateKey(Entity keyValue)
        {
            if (!keyValue.IsValid()) throw new ApplicationException("Entidade inválida, informe corretamente todos os parametros do construtor.");


            Console.WriteLine($"Criando Chave {keyValue.KeyName} - Ambiente {keyValue.Ambiente} - Produto: {keyValue.Produto}");

            var conteudo = new StringContent(keyValue.GetKeyPairValue());

            _httpClient.DefaultRequestHeaders.Add("key", keyValue.GetKeyPathName());
            var response = await _httpClient.PutAsync(baseUrl + $"/Maxima/{keyValue.GetKeyPathName()}/{keyValue.KeyName}", conteudo);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            return false;
        }
        public async Task<bool> DeleteKey(Entity keyValue)
        {
            if (!keyValue.IsValid()) throw new ApplicationException("Entidade inválida, informe corretamente todos os parametros do construtor.");

            var response = await _httpClient.DeleteAsync(baseUrl + $"/{keyValue.GetKeyPathName()}/{keyValue.KeyName}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            return false;
        }
        public async Task<IEnumerable<Entity>> GetAllKeys()
        {
            var response = await _httpClient.GetAsync($"{baseUrl}?recurse=true");
            var respostaFinal = new List<Entity>();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return respostaFinal;

            var resultado = JsonConvert.DeserializeObject<List<KVPair>>(await response.Content.ReadAsStringAsync());
            foreach (var item in resultado)
            {
                respostaFinal.Add(EntityFactory.NewEntityByKvPair(item));
            }

            return respostaFinal;
        }
        public async Task<Entity> GetKey(string KeyPathAndName)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}Maxima/gestao/maxgestao/producao/nomedachave0");

            throw new NotImplementedException();
        }
    }
}
