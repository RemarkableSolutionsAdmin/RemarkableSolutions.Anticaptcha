using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Anticaptchas;
using Xunit;

namespace RemarkableSolutions.Anticaptcha.Tests.Api.Anticaptchas
{
    public class RecaptchaV2ProxylessTaskTests
    {
        [Fact]
        public async Task ShouldReturnCorrectCaptchaResult_WhenCallingFactualAnticaptchaTask()
        {
            var anticaptchaTask = new RecaptchaV2EnterpriseProxylessTask
            {
                ClientKey = TestHelper.ClientKey,
                WebsiteUrl = new Uri("http://http.myjino.ru/recaptcha/test-get.php"),
                WebsiteKey = "6Lc_aCMTAAAAABx7u2W0WPXnVbI_v6ZdbM6rYf16"
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