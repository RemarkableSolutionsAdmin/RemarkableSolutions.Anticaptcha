using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Anticaptchas;
using Xunit;

namespace RemarkableSolutions.Anticaptcha.Tests.Api.Anticaptchas
{
    public class RecaptchaV2EnterpriseProxylessTaskTests
    {
        [Fact]
        public async Task ShouldReturnCorrectCaptchaResult_WhenCallingFactualAnticaptchaTask()
        {
            var anticaptchaTask = new RecaptchaV2EnterpriseProxylessTask
            {
                ClientKey = TestHelper.ClientKey,
                WebsiteUrl = new Uri("https://store.steampowered.com/join"),
                WebsiteKey = "6LdIFr0ZAAAAAO3vz0O0OQrtAefzdJcWQM2TMYQH"
            };

            anticaptchaTask.EnterprisePayload.Add("test", "qwerty");
            anticaptchaTask.EnterprisePayload.Add("secret", "AB_12345");

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