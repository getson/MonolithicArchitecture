using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Xunit;
using HttpMethod = System.Net.Http.HttpMethod;

namespace App.WebApi.IntegrationTest.Infrastructure
{
    public static class Extensions
    {
        public static async Task<T> GetObjectAsync<T>(this HttpResponseMessage responseMessage)
        {
            var response = await responseMessage.Content.ReadAsStringAsync();

            return ClientSerializer.Deserialize<T>(response);
        }

        public static async Task<IReadOnlyCollection<T>> GetCollectionAsync<T>(this HttpResponseMessage responseMessage)
        {
            var response = await responseMessage.Content.ReadAsStringAsync();
            return ClientSerializer.Deserialize<IReadOnlyCollection<T>>(response);
        }

        public static T GetObject<T>(this HttpResponseMessage responseMessage)
        {
            var response = responseMessage.Content.ReadAsStringAsync().Result;

            return ClientSerializer.Deserialize<T>(response);
        }

        public static List<T> GetCollection<T>(this HttpResponseMessage responseMessage)
        {
            var response = responseMessage.Content.ReadAsStringAsync().Result;
            return ClientSerializer.Deserialize<List<T>>(response);
        }

        public static HttpContent ToJsonContent<T>(this T @object)
        {
            var jsonString = ClientSerializer.Serialize(@object);
            return new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
        }
    }

    public class ControllerTestBase : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public ControllerTestBase(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        protected async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _fixture.Client.GetAsync(url);
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T @object)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = @object.ToJsonContent()
            };
            return await _fixture.Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T @object)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = @object.ToJsonContent()
            };
            return await _fixture.Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> GetWithHeadersAsync<T>(string url, Dictionary<string, string> headers, T @object)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = @object.ToJsonContent()
            };

            foreach (var header in headers)
            {
                if (header.Value.Equals("Authorization"))
                {
                    var authorization = new AuthenticationHeaderValue("Bearer", header.Value);
                    requestMessage.Headers.Authorization = authorization;
                }
                else
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            return await _fixture.Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> DeleteAsync<T>(string url, T @object)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url + "?" + GetQueryString(@object));
            //{
            //    Content = @object.ToJsonContent()
            //};

            return await _fixture.Client.SendAsync(requestMessage);
        }

        public string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _fixture.Client.DeleteAsync(url);
        }
    }
}