using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Helpers;

namespace RemarkableSolutions.Anticaptcha.Api
{
    public static class AnticaptchaApi
    {
        public const string Host = "api.anti-captcha.com";
        private const SchemeType Scheme = SchemeType.Https;
        public enum ApiMethod
        {
            CreateTask,
            GetTaskResult,
            GetBalance
        }

        private enum SchemeType
        {
            Http,
            Https
        }
        public static HttpHelper.PostResult CallApiMethod(ApiMethod methodName, JObject jsonPostData)
        {
            var uri = CreateAntiCaptchaUri(methodName);
            var data = HttpHelper.Post(uri, JsonConvert.SerializeObject(jsonPostData, Formatting.Indented));
            return data;
        }

        private static Uri CreateAntiCaptchaUri(ApiMethod methodName)
        {
            var methodNameStr = char.ToLowerInvariant(methodName.ToString()[0]) + methodName.ToString().Substring(1);
            return new Uri(Scheme + "://" + Host + "/" + methodNameStr);
        }

        public static async Task<HttpHelper.PostResult> CallApiMethodAsync(ApiMethod methodName, JObject jsonPostData)
        {
            var uri = CreateAntiCaptchaUri(methodName);
            var data = await HttpHelper.PostAsync(uri, JsonConvert.SerializeObject(jsonPostData, Formatting.Indented));
            return data;
        }

    }
}