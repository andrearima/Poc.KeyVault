using Consul;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KeyVault
{
    /// <summary>
    /// Sem uso de bibliotecas, apenas HTTP Client
    /// </summary>
    public class KeyVaultHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrlSecure = "https://localhost:8501/v1/kv/";
        private readonly string baseUrl = "http://localhost:8502/v1/kv/";
        public KeyVaultHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreateOrUpdateKey(Entity KeyValue)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteKey(Entity keyValue)
        {
            if (!keyValue.IsValid()) throw new ApplicationException("Entidade inválida, informe corretamente todos os parametros do construtor.");

            throw new NotImplementedException();

        }
        public async Task<IEnumerable<Entity>> GetAllKeys()
        {
            var response = await _httpClient.GetAsync($"{baseUrl}?recurse=true");
            var resultado = JsonConvert.DeserializeObject<List<KVPair>>(await response.Content.ReadAsStringAsync());

            var respostaFinal = new List<Entity>();
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
