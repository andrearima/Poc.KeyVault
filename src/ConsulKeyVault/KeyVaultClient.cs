using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Consul;

namespace ConsulKeyVault
{
    public class KeyVaultClient : IDisposable
    {
        private readonly ConsulClient _client;
        private readonly string TokenAdmin = "527351b7-1239-1301-2576-b80a668391be";
        public KeyVaultClient()
        {
            _client = new ConsulClient();
        }
        public KeyVaultClient(string url)
        {
            _client = new ConsulClient();
            SetToken();
            SetUrlAddress(url);
        }
        public KeyVaultClient(Uri url)
        {
            _client = new ConsulClient();
            SetToken();
            SetUrlAddress(url);
        }
        private void SetToken()
        {
            _client.Config.Token = TokenAdmin;
        }
        public void SetUrlAddress(string url)
        {
            _client.Config.Address = new Uri(url);
        }
        public void SetUrlAddress(Uri uri)
        {
            _client.Config.Address = uri;
        }
        public async Task<bool> CreateOrUpdateKey(Entity KeyValue)
        {
            var putAttempt = await _client.KV.Put(KeyValue.GetKVPair());
            return putAttempt.Response;
        }
        public async Task<bool> DeleteKey(Entity keyValue)
        {
            if (!keyValue.IsValid()) throw new ApplicationException("Entidade inválida, informe corretamente todos os parametros do construtor.");

            try
            {
                var y = await _client.KV.Delete(keyValue.GetKeyPathName());
                return y.Response;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<IEnumerable<Entity>> GetAllKeys()
        {
            var resultadoConsulta = await _client.KV.List("Maxima");
            var retorno = new List<Entity>();
            foreach (var kvs in resultadoConsulta.Response)
            {
                retorno.Add(EntityFactory.NewEntityByKvPair(kvs));
            }
            return retorno;
        }
        

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
