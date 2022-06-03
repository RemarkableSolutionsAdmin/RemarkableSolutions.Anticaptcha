using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RemarkableSolutions.Anticaptcha.Api.Anticaptchas;
using Xunit;

namespace RemarkableSolutions.Anticaptcha.Tests.Api.Anticaptchas
{
    public class GeeTestProxylessTaskTests
    {
        private class GeeTestModel
        {
            public GeeTestData Data { get; set; }
            public string Status { get; set; }
            [JsonProperty("err_msg")]
            public string ErrorMessage { get; set; }
        }

        private class GeeTestData
        {
            [JsonProperty("gt")]
            public string WebsiteKey { get; set; }
            [JsonProperty("challenge")]
            public string WebsiteChallenge { get; set; }
        }
        
        private (string websiteKey, string websiteChallenge) GetTokens()
        {
            var response = new WebClient().DownloadString("https://auth.geetest.com/api/init_captcha?time=1561554686474");
            var model = JsonConvert.DeserializeObject<GeeTestModel>(response);
            return (model.Data.WebsiteKey, model.Data.WebsiteChallenge);
        }
        
        [Fact]
        public async Task ShouldReturnCorrectCaptchaResult_WhenCallingFactualAnticaptchaTask()
        {
            var (websiteKey, websiteChallenge) = GetTokens();
            var anticaptchaTask = new GeeTestProxylessTask()
            {
                ClientKey = TestHelper.ClientKey,
                WebsiteUrl = new Uri("http://www.supremenewyork.com"),
                WebsiteKey = websiteKey,
                WebsiteChallenge = websiteChallenge
            };

            var result = anticaptchaTask.CreateTask();
            Assert.True(result.Success);
            Assert.Equal(result.Message, string.Empty);
            var taskResult = anticaptchaTask.WaitForTaskResult(); 
            Assert.True(taskResult.Success);
            Assert.NotNull(anticaptchaTask.TaskInfo);
            Assert.Null(anticaptchaTask.TaskInfo?.ErrorCode);
        }
    }
}