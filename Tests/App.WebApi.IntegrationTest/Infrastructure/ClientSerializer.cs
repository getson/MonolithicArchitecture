using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace App.WebApi.IntegrationTest.Infrastructure
{
    public static class ClientSerializer
    {
        static ClientSerializer()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public static T Deserialize<T>(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
                // ignored
            }
            return default(T);
        }

        public static string Serialize<T>(T @object)
        {
            return JsonConvert.SerializeObject(@object);
        }
    }
}