using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RemarkableSolutions.Anticaptcha.Api.Anticaptchas;
using Xunit;

namespace RemarkableSolutions.Anticaptcha.Tests.Api.Anticaptchas
{
    public class HCaptchaProxylessTaskTests
    {
        [Fact]
        public async Task ShouldReturnCorrectCaptchaResult_WhenCallingFactualAnticaptchaTask()
        {
            var anticaptchaTask = new HCaptchaProxylessTask()
            {
                ClientKey = TestHelper.ClientKey,
                WebsiteUrl = new Uri("https://democaptcha.com/demo-form-eng/hcaptcha.html/"),
                WebsiteKey = "51829642-2cda-4b09-896c-594f89d700cc",
                UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116"
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