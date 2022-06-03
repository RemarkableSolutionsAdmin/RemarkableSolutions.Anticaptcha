using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Anticaptchas;
using Xunit;

namespace RemarkableSolutions.Anticaptcha.Tests.Api.Anticaptchas
{
    public class RecaptchaV3ProxylessTaskTests
    {
        [Fact]
        public async Task ShouldReturnCorrectCaptchaResult_WhenCallingFactualAnticaptchaTask()
        {
            var anticaptchaTask = new RecaptchaV3ProxylessTask
            {
                ClientKey = TestHelper.ClientKey,
                WebsiteUrl = new Uri("https://www.netflix.com/login"),
                WebsiteKey = "6Lf8hrcUAAAAAIpQAFW2VFjtiYnThOjZOA5xvLyR",
                IsEnterprise = true
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