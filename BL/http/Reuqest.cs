using BE.Counter;
using BL.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BL.http
{
    public class Reuqest<T> where T : class,new() 
    {
        private T Value { get; set; }
        protected virtual HttpClient _httpClient { get; set; }
        public Reuqest() 
        {
        }
        public async Task<string> PostRequest(T Model)
        {
            using (var httpClient = _httpClient ?? new HttpClient())
            {
                var convertJson = new ConvertJson<T>(Model);
                var Json = convertJson.ConverModelToJson();
                var content = new StringContent(Json, Encoding.UTF8, "application/json");
                var resultPostRequest = await httpClient.PostAsync("http://Test.ru", content);
                if(resultPostRequest != null && resultPostRequest.StatusCode == HttpStatusCode.OK) 
                {
                    var result = await resultPostRequest.Content.ReadAsStringAsync();
                    return result;
                }
                throw new Exception();
                
            }
        }
    }
}
