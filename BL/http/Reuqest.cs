using BE.Counter;
using BL.Helper;
using DB.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public async Task<string> PostRequest(T Model, string Url)
        {
            using (var httpClient = _httpClient ?? new HttpClient())
            {
                var convertJson = new ConvertJson<T>(Model);
                var Json = convertJson.ConverModelToJson();
                var content = new StringContent(Json, Encoding.UTF8, "application/json");
                var resultPostRequest = await httpClient.PostAsync(Url, content);
                if(resultPostRequest != null && resultPostRequest.StatusCode == HttpStatusCode.OK) 
                {
                    var result = await resultPostRequest.Content.ReadAsStringAsync();
                    return result;
                }
                throw new Exception($"Ошибка загруки код ошибки:{resultPostRequest.StatusCode}");

            }
        }
        public async Task<string> PostRequestWithTocken(T Model, string Url, string Token)
        {
            using (var httpClient = _httpClient ?? new HttpClient())
            {
                var convertJson = new ConvertJson<T>(Model);
                var Json = convertJson.ConverModelToJson();
                var content = new StringContent(Json, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Token");
                var resultPostRequest = await httpClient.PostAsync(Url, content);
                if (resultPostRequest != null && resultPostRequest.StatusCode == HttpStatusCode.OK)
                {
                    var result = await resultPostRequest.Content.ReadAsStringAsync();
                    return result;
                }
                throw new Exception($"Ошибка загруки код ошибки:{resultPostRequest.StatusCode}");

            }
        }
        public async Task<byte[]> GetRequestWithTockenAsync(string Url, string Token)
        {
            using (var httpClient = _httpClient ?? new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                await Task.CompletedTask;
                var resultRequest = await httpClient.GetAsync(Url);
                if (resultRequest != null && resultRequest.StatusCode == HttpStatusCode.OK)
                {
                    var result = await resultRequest.Content.ReadAsStringAsync();
                    byte[] buffer = Encoding.UTF8.GetBytes(result);
                    return buffer;
                }
                throw new Exception($"Ошибка загруки код ошибки:{resultRequest.StatusCode}");

            }
        }
        public async Task<byte[]> GetFileRequestWithTockenAsync(string Url, string Token)
        {
            using (var httpClient = _httpClient ?? new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                await Task.CompletedTask;
                var resultRequest = await httpClient.GetAsync(Url);
                if (resultRequest != null && resultRequest.StatusCode == HttpStatusCode.OK)
                {
                    var result = await resultRequest.Content.ReadAsStreamAsync();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        result.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
                throw new Exception($"Ошибка загруки код ошибки:{resultRequest.StatusCode}");

            }
        }
        public async Task<byte[]> UploadFileAndGetFile(string Url, string Token, Stream Stream, string FileName)
        {
            using (var httpClient = _httpClient ?? new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    //Load the file and set the file's Content-Type header
                    var fileStreamContent = new StreamContent(Stream);
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

                    //Add the file
                    multipartFormContent.Add(fileStreamContent, name: "formFile", fileName: FileName);

                    //Send it
                    var response = await httpClient.PostAsync(Url, multipartFormContent);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStreamAsync();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        result.CopyTo(ms);
                        return ms.ToArray();
                    }

                }
            }
        }
    }
}
