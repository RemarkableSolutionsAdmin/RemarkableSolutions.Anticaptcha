using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Models;
using System.Threading.Tasks;

namespace RemarkableSolutions.Anticaptcha.Api.Base
{
    public interface IAnticaptchaTask
    {
        JObject GetPostData();
        SolutionData GetTaskSolution();
        CaptchaTask CreateTask();
        Task<CaptchaTask> CreateTaskAsync();
        CaptchaTask WaitForTaskResult(int maxSeconds = 120, int currentSecond = 0);
        Task<CaptchaTask> WaitForTaskResultAsync(int maxSeconds = 120, int currentSecond = 0);
        SolutionData SolveCaptcha();
        Task<SolutionData> SolveCaptchaAsync();
    }
}