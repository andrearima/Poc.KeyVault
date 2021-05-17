using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyVault
{
    public class Entity
    {
        private KVPair KeyValuePair;
        private string Path;
        public Ambiente Ambiente { get; private set; }
        public string Produto { get; private set; }
        public string Torre { get; private set; }
        public string KeyName { get; private set; }
        public Entity(string keyName, string keyValue, Ambiente ambiente, string produto, string torre)
        {
            Ambiente = ambiente;
            Produto = produto.ToLower();
            Torre = torre.ToLower();
            KeyName = keyName;
            SetKeyPairValue(keyValue);
        }
        public Entity(string keyName, Ambiente ambiente, string produto, string torre)
        {
            Ambiente = ambiente;
            Produto = produto.ToLower();
            Torre = torre.ToLower();
            KeyName = keyName.ToLower();
            SetKvPairInstance();
        }
        public void SetKeyPairValue(string Value)
        {
            if (string.IsNullOrEmpty(Value))
                throw new ApplicationException("Necessário informar o Valor da Chave.");

            SetKvPairInstance();
            KeyValuePair.Value = Encoding.UTF8.GetBytes(Value);
        }
        private void SetKvPairInstance()
        {
            SetPath();
            KeyValuePair = new KVPair($"Maxima/{Path}/{KeyName}");
        }
        private void SetPath()
        {
            Path = $"{Torre}/{Produto}/{Ambiente.ToString().ToLower()}";
        }
        public string GetKeyPairValue()
        {
            return Encoding.UTF8.GetString(KeyValuePair.Value, 0, KeyValuePair.Value.Length);
        }
        public KVPair GetKVPair()
        {
            return KeyValuePair;
        }
        public void SetKVPair(KVPair kvpair)
        {
            KeyValuePair = kvpair;
        }
        public string GetKeyPathName()
        {
            return $"{Path}/{KeyName}";
        }
        public bool IsValid()
        {
            if (KeyValuePair == null)
                return false;
            if (string.IsNullOrEmpty(KeyValuePair.Key))
                return false;

            if (string.IsNullOrEmpty(Produto))
                return false;
            if (string.IsNullOrEmpty(Torre))
                return false;
            if (string.IsNullOrEmpty(KeyName))
                return false;

            return true;
        }
    }

    public class EntityFactory
    {
        public static Entity NewEntityByKvPair(KVPair kvPair)
        {
            var pathsENomeDaChave = kvPair.Key.Split("/");
            var nomeDaTorre = pathsENomeDaChave[1];
            var produto = pathsENomeDaChave[2];
            var ambiente = pathsENomeDaChave[3];
            var ambienteEnum = Enum.Parse<Ambiente>(ambiente);
            var nomeDaChave = pathsENomeDaChave[pathsENomeDaChave.Length - 1];
            var valorDaChave = "";

            List<string> subProdutos = new List<string>();
            /// path fora do padrão, com sub-produtos
            for (int i = 0; i < pathsENomeDaChave.Length - 1; i++)
            {
                if (i != 0 && i != 1 && i != 2 && i != 3 && i != (pathsENomeDaChave.Length - 1))
                {
                    subProdutos.Add(pathsENomeDaChave[i]);
                }
            }

            if (kvPair.Value != null)
                valorDaChave = Encoding.UTF8.GetString(kvPair.Value, 0, kvPair.Value.Length);

            var entity = new Entity(nomeDaChave, valorDaChave, ambienteEnum, produto, nomeDaTorre);
            entity.SetKVPair(kvPair);
            return entity;
        }
    }

    public enum Ambiente
    {
        desenvolvimento = 0,
        teste = 1,
        producao = 2
    }
}
