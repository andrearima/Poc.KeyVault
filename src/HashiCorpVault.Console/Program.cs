using System;
using System.Collections.Generic;
using System.Net.Http;
using HashiCorpVault;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;

namespace HashiCorpVault.C
{
    class Program
    {
        static void Main(string[] args)
        {
            //var http = new HttpClient();
            //HashiVaultHttpService service = new HashiVaultHttpService(http);
            //service.teste();

            //Console.ReadKey();

            //Vault.VaultClient cliente = new Vault.VaultClient(new Uri("http://127.0.0.1:8200"), "s.FWTa0YMNkH1i1th9DHvJMi3g");
            
            //Vault.VaultHttpClient httpClient = new Vault.VaultHttpClient();
            //var data = new Dictionary<string, string>
            //    {
            //        {"zip", "zap"}
            //    };
            
            //cliente.Secret.Write("secrets/foo", data).GetAwaiter().GetResult();
            //cliente.Secret.Write("secrets/kv/foo", data).GetAwaiter().GetResult();

            //var secret = cliente.Secret.Read<Dictionary<string, string>>("secrets/kv/foo").GetAwaiter().GetResult();
            //Console.WriteLine(secret.Data["zip"]);




            // Initialize one of the several auth methods.
            IAuthMethodInfo authMethod = new TokenAuthMethodInfo("s.D567ZwO7usrg66VG5vE0hpGZ");
            //unsealkey = I4SLinCrhG4/eQ/96apnM80pYmLYe4MWaqHHW4sYwQY=

            // Initialize settings. You can also set proxies, custom delegates etc. here.
            var vaultClientSettings = new VaultClientSettings("http://127.0.0.1:8200", authMethod);

            IVaultClient vaultClient = new VaultClient(vaultClientSettings);

            // Use client to read a key-value secret.
            //var value = new Dictionary<string, object> { { "key1", "val1" }, { "key2", 2 } };
            //vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync("kv", value).GetAwaiter().GetResult();

            var value = new Dictionary<string, object> { { "key1", "val1" }, { "key2", 2 } };
            vaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync("abigail", value).GetAwaiter().GetResult();

            var result = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("abigail").GetAwaiter().GetResult();

            vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync("abigail", value).GetAwaiter().GetResult();

            var k2result= vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("abigail").GetAwaiter().GetResult();

        }
    }
}
