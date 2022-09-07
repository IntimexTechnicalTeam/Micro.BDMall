
namespace Web.Framework
{
    public class RestClientHelper
    {
        public static async Task<OutT> HttpPostAsync<OutT>(string url, object T, AuthorizationType authorizationType, string token = "", string jsonType = "application/json; charset=utf-8")
        {
            var result = await ExecuteClientAsync<OutT>(Method.POST, url, T, authorizationType, token, jsonType);
            return result;
        }

        public static async Task<OutT> HttpGetAsync<OutT>(string url, object T, AuthorizationType authorizationType, string token = "", string jsonType = "application/json; charset=utf-8")
        {
            var result = await ExecuteClientAsync<OutT>(Method.GET, url, T, authorizationType, token, jsonType);
            return result;
        }

        static async Task<OutT> ExecuteClientAsync<OutT>(Method method, string url, object T, AuthorizationType authorizationType, string token = "", string jsonType = "application/json; charset=utf-8")
        {
            var client = new RestClient(url);

            var request = new RestRequest(method).AddDecompressionMethod(System.Net.DecompressionMethods.GZip);

            if (T != null) request.AddParameter(jsonType, JsonUtil.ToJson(T), ParameterType.RequestBody);
            if (!string.IsNullOrEmpty(token)) request.AddHeader("Authorization", $"{authorizationType} {token}");

            IRestResponse response = await client.ExecuteAsync(request);
            var result = JsonUtil.JsonToObject<OutT>(response.Content);
            return result;
        }
    }

    public enum AuthorizationType
    {
        Bearer,
        Basic
    }
}
