using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Framework
{
    public class RestClientHelper
    {
        static RestClient client;
        public RestClientHelper() {

            client = new RestClient();
        }

        public static async Task<T> PostAsync<T>(string url, ICollection<KeyValuePair<string, string>> param = null, ICollection<KeyValuePair<string, string>> headers = null)
        {
            client.BaseUrl = new Uri(url);

            var request = new RestRequest(Method.POST);

            if (param != null && param.Any())
            {
                foreach (var item in param)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }

            if (headers != null && headers.Any())
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            var response =await client.ExecuteAsync<T>(request);
            var result = JsonConvert.DeserializeObject<T>(response.Content);

            return result;
        }

        public static async Task<T> GetAsync<T>(string url, ICollection<KeyValuePair<string, string>> param = null, ICollection<KeyValuePair<string, string>> headers = null)
        {
            client.BaseUrl = new Uri(url);

            var request = new RestRequest(Method.GET);

            if (param != null && param.Any())
            {
                foreach (var item in param)
                {
                    request.AddUrlSegment(item.Key, item.Value);
                }
            }

            if (headers != null && headers.Any())
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            var response = await client.ExecuteAsync<T>(request);
            var result = JsonConvert.DeserializeObject<T>(response.Content);

            return result;
        }

        public static  T Post<T>(string url, ICollection<KeyValuePair<string, string>> param = null, ICollection<KeyValuePair<string, string>> headers = null)
        {
            client.BaseUrl = new Uri(url);

            var request = new RestRequest(Method.POST);

            if (param != null && param.Any())
            {
                foreach (var item in param)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }

            if (headers != null && headers.Any())
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            var response =  client.Execute<T>(request);
            var result = JsonConvert.DeserializeObject<T>(response.Content);

            return result;
        }

        public static  T Get<T>(string url, ICollection<KeyValuePair<string, string>> param = null, ICollection<KeyValuePair<string, string>> headers = null)
        {
            client.BaseUrl = new Uri(url);

            var request = new RestRequest(Method.GET);

            if (param != null && param.Any())
            {
                foreach (var item in param)
                {
                    request.AddUrlSegment(item.Key, item.Value);
                }
            }

            if (headers != null && headers.Any())
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            var response =  client.Execute<T>(request);
            var result = JsonConvert.DeserializeObject<T>(response.Content);

            return result;
        }
    }
}
