using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RemarkableSolutions.Anticaptcha.Helpers
{
    public static class HttpHelper
    {
        public class PostResult
        {
            public bool Success { get; set; }
            public string Error { get; set; }
            public dynamic Response { get; set; }
        }
        
        public static PostResult Post(Uri url, string postData)
        {
            dynamic result;
            var postBody = Encoding.UTF8.GetBytes(postData);
            var request = CreatePostRequest(url, postData);

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postBody, 0, postBody.Length);
                    stream.Close();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    var rawResponse = streamReader.ReadToEnd();
                    result = JsonConvert.DeserializeObject(rawResponse);

                    response.Close();
                }
            }
            catch (Exception ex)
            {
                return new PostResult{Success = false, Error = ex.Message};;
            }

            return new PostResult { Success = true, Response = result};
        }
        
        public static async Task<PostResult> PostAsync(Uri url, string post)
        {
            dynamic result;
            var postBody = Encoding.UTF8.GetBytes(post);
            var request = CreatePostRequest(url, post);

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    await stream.WriteAsync(postBody, 0, postBody.Length);
                    stream.Close();
                }
                
                using (var response = await request.GetResponseAsync())
                {
                    var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    var rawResponse = await streamReader.ReadToEndAsync();
                    result = JsonConvert.DeserializeObject(rawResponse);
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                return new PostResult{Success = false, Error = ex.Message};;
            }

            return new PostResult { Success = true, Response = result};
        }

        private static HttpWebRequest CreatePostRequest(Uri url, string post)
        {
            var postBody = Encoding.UTF8.GetBytes(post);
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = postBody.Length;
            request.Timeout = 30000;
            return request;
        }
    }
}