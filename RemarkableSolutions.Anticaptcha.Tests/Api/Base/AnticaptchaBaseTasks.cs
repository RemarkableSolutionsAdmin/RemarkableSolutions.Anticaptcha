using System.Threading.Tasks;
using RemarkableSolutions.Anticaptcha.Api.Anticaptchas;
using Xunit;

namespace RemarkableSolutions.Anticaptcha.Tests.Api.Base
{
    public class AnticaptchaBaseTasks
    {
        [Fact]
        public async Task ShouldReturnCorrectBalance_WhenCallingFactualAnticaptchaTask()
        {
            var anticaptchaTask = new ImageToTextTask { ClientKey = TestHelper.ClientKey };
            var balance = anticaptchaTask.GetBalance();
            Assert.NotNull(balance);
        }
    }
}