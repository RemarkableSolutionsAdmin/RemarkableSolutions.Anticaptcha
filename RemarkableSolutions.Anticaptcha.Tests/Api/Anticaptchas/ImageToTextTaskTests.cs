using System.Threading.Tasks;
using RemarkableSolutions.Anticaptcha.Api.Anticaptchas;
using RemarkableSolutions.Anticaptcha.Common;
using Xunit;

namespace RemarkableSolutions.Anticaptcha.Tests.Api.Anticaptchas
{
    public class ImageToTextTaskTests
    {
        private const string ExpectedCaptchaResult = "W68HP";

        private static ImageToTextTask CreateImageToText(string? clientKey, string filePath = "")
        {
            return new ImageToTextTask
            {
                ClientKey = clientKey ?? TestHelper.ClientKey,
                FilePath = filePath
            };
        }
        
        [Fact]
        public async Task ShouldReturnCorrectCaptchaResult_WhenCallingFactualAnticaptchaTask()
        {
            var anticaptchaTask = CreateImageToText(null, filePath: "Resources\\captchaexample.png");

            var result = anticaptchaTask.CreateTask();
            Assert.True(result.Success);
            Assert.Equal(result.Message, string.Empty);
            var taskResult = anticaptchaTask.WaitForTaskResult(); 
            Assert.True(taskResult.Success);
            Assert.NotNull(anticaptchaTask.TaskInfo);
            Assert.Equal(ExpectedCaptchaResult, anticaptchaTask.TaskInfo?.Solution.Text);
        }

        [Fact]
        public async Task ShouldReturnFalseWithCorrectException_WhenCallingWithIncorrectFilePath()
        {
            var anticaptchaTask = CreateImageToText(null, filePath: "dsa.png");
            
            var result = anticaptchaTask.CreateTask();
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.AnticaptchaPostDataValidationError, result.Message);
        }
    }
}