using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Newtonsoft.Json;

namespace ConsulKeyVault
{
    class Program
    {
        static Uri url = new Uri("http://localhost:8502/");
        static void Main(string[] args)
        {
            TesteUsandoHttpClientHardCode();

            Console.ReadKey();
        }
        private static void TesteUsandoHttpClientHardCode()
        {
            InsereRegistrosHardCode();

            using var http = new HttpClient();
            KeyVaultHttpService client = new KeyVaultHttpService(http);
            client.DeleteKey(new Entity("nomedachave0", Ambiente.producao, "maxpedido", "vendas")).GetAwaiter().GetResult();
        }
        private static void InsereRegistrosHardCode()
        {
            using var http = new HttpClient();
            KeyVaultHttpService client = new KeyVaultHttpService(http);
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachavetestes{i}", Ambiente.desenvolvimento, "maxgestao", "gestao");
                var resultado = client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachavedev{i}", Ambiente.teste, "maxgestao", "gestao");
                client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachaveprod{i}", Ambiente.producao, "maxgestao", "gestao");
                client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachavetestes{i}", Ambiente.desenvolvimento, "maxpedido", "vendas");
                var resultado = client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachavedev{i}", Ambiente.teste, "maxpedido", "vendas");
                client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachaveprod{i}", Ambiente.producao, "maxpedido", "vendas");
                client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
        }
        private static void TesteUsandoBiblioteca()
        {
            InsereRegistros();
            using var client = new KeyVaultClient(url);
            
            client.DeleteKey(new Entity("nomedachave0", Ambiente.producao, "maxpedido", "vendas")).GetAwaiter().GetResult();

            var teste = client.GetAllKeys().GetAwaiter().GetResult();
        }
        private static void InsereRegistros()
        {
            using var client = new KeyVaultClient(url);
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachavetestes{i}", Ambiente.desenvolvimento, "maxgestao", "gestao");
                var resultado = client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachavedev{i}", Ambiente.teste, "maxgestao", "gestao");
                client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachaveprod{i}", Ambiente.producao, "maxgestao", "gestao");
                client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachavetestes{i}", Ambiente.desenvolvimento, "maxpedido", "vendas");
                var resultado = client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachavedev{i}", Ambiente.teste, "maxpedido", "vendas");
                client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
            for (int i = 0; i < 10; i++)
            {
                var t = new Entity($"nomedachave{i}", $"valordachaveprod{i}", Ambiente.producao, "maxpedido", "vendas");
                client.CreateOrUpdateKey(t).GetAwaiter().GetResult();
            }
        }
        #region consul
        public static ConsulClientConfiguration ObterConfiguracao()
        {
            return new ConsulClientConfiguration() { Address = url };
        }
        public static async Task<string> HelloConsul()
        {
            using (var client = new ConsulClient())
            {
                client.Config.Address = url;


                var putPair = new KVPair("hello/hellow")
                {
                    Value = Encoding.UTF8.GetBytes("Hello Consul")
                };

                var putAttempt = await client.KV.Put(putPair);

                if (putAttempt.Response)
                {
                    var getPair = await client.KV.Get("hello/hellow");
                    return Encoding.UTF8.GetString(getPair.Response.Value, 0,
                        getPair.Response.Value.Length);
                }
                return "";
            }
        }
        #endregion
    }
}
