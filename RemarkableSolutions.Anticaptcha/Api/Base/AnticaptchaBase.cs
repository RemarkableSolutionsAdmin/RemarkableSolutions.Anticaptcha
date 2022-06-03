using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Models;
using RemarkableSolutions.Anticaptcha.Api.Responses;
using RemarkableSolutions.Anticaptcha.Common;
using RemarkableSolutions.Anticaptcha.Helpers;

namespace RemarkableSolutions.Anticaptcha.Api.Base
{
    public abstract class AnticaptchaBase : IAnticaptchaTask
    {
        public virtual SolutionData GetTaskSolution() => TaskInfo.Solution;
        public string ErrorMessage { get; private set; }
        public int TaskId { get; private set; }
        public string ClientKey { set; private get; }
        public TaskResultResponse TaskInfo { get; protected set; }

        private const int SoftId = 1023;
        public abstract JObject GetPostData();
        protected abstract bool IsPostDataValid();

        private JObject GetJObjectDto(Dictionary<string, object> dictionary)
        {
            var jObject = JObject.FromObject(dictionary);
            jObject.Add("softId", SoftId);
            jObject.Add("clientKey", ClientKey);
            return jObject;
        }


        public CaptchaTask CreateTask()
        {
            return CreateTaskLogic(false).Result;
        }

        public async Task<CaptchaTask> CreateTaskAsync()
        {
            return await CreateTaskLogic(true);
        }

        private async Task<CaptchaTask> CreateTaskLogic(bool isAsync)
        {
            if (!IsPostDataValid())
            {
                return new CaptchaTask { Message = ErrorMessages.AnticaptchaPostDataValidationError };
            }

            var postData = GetPostData();

            if (postData == null)
            {
                return new CaptchaTask { Message = ErrorMessages.AnticaptchaPreparingTaskError };
            }

            var postDataJObject = GetJObjectDto(new Dictionary<string, object> { { "task", postData } });
            var postResult = isAsync ?
                await AnticaptchaApi.CallApiMethodAsync(AnticaptchaApi.ApiMethod.CreateTask, postDataJObject) :
                AnticaptchaApi.CallApiMethod(AnticaptchaApi.ApiMethod.CreateTask, postDataJObject);

            if (postResult == null)
            {
                return new CaptchaTask { Message = ErrorMessages.AnticaptchaApiError };
            }

            if (!postResult.Success)
            {
                return new CaptchaTask { Message = postResult.Error };
            }

            var response = new CreateTaskResponse(postResult.Response);

            if (!response.ErrorId.Equals(0))
            {
                return new CaptchaTask { Message = ErrorMessages.AnticaptchaApiSpecificError(response.ErrorCode, response.ErrorDescription) };
            }

            if (response.TaskId == null)
            {
                return new CaptchaTask { Message = ErrorMessages.AnticaptchaNullTaskError };
            }

            TaskId = (int)response.TaskId;
            return new CaptchaTask { Success = true, Message = string.Empty };
        }

        public async Task<CaptchaTask> WaitForTaskResultAsync(int maxSeconds = 120, int currentSecond = 0)
        {
            return await WaitForTaskResultLogic(true, maxSeconds, currentSecond);
        }

        public CaptchaTask WaitForTaskResult(int maxSeconds = 120, int currentSecond = 0)
        {
            return WaitForTaskResultLogic(false, maxSeconds, currentSecond).Result;
        }

        private async Task<CaptchaTask> WaitForTaskResultLogic(bool isAsync, int maxSeconds, int currentSecond)
        {
            if (currentSecond >= maxSeconds)
            {
                return new CaptchaTask { Message = ErrorMessages.AnticaptchaTimeoutError };
            }

            if (isAsync)
            {
                await Task.Delay(currentSecond.Equals(0) ? 3000 : 1000);
            }
            else
            {
                Thread.Sleep(currentSecond.Equals(0) ? 3000 : 1000);
            }

            var jsonPostData = GetJObjectDto(new Dictionary<string, object> { { "taskId", TaskId } });
            var postResult = isAsync ? await AnticaptchaApi.CallApiMethodAsync(AnticaptchaApi.ApiMethod.GetTaskResult, jsonPostData) :
                AnticaptchaApi.CallApiMethod(AnticaptchaApi.ApiMethod.GetTaskResult, jsonPostData);

            if (postResult == null)
            {
                return new CaptchaTask { Message = ErrorMessages.AnticaptchaApiGetTaskError };
            }

            if (!postResult.Success)
            {
                return new CaptchaTask { Message = postResult.Error };
            }

            TaskInfo = new TaskResultResponse(postResult.Response);

            if (!TaskInfo.ErrorId.Equals(0))
            {
                return new CaptchaTask { Message = TaskInfo.ErrorDescription };
            }

            switch (TaskInfo.Status)
            {
                case TaskResultResponse.StatusType.Processing:
                    return WaitForTaskResult(maxSeconds, currentSecond + 1);
                case TaskResultResponse.StatusType.Ready when !TaskInfo.Solution.IsValid():
                    return new CaptchaTask { Message = ErrorMessages.AnticaptchaNoSolutionFromAPIError };
                case TaskResultResponse.StatusType.Ready:
                    return new CaptchaTask { Success = true, Message = "The task is complete!" };
                case null:
                    return new CaptchaTask { Message = ErrorMessages.AnticaptchaUnknownStatusError };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public double? GetBalance()
        {
            var postData = GetJObjectDto(new Dictionary<string, object>());
            var postResult = AnticaptchaApi.CallApiMethod(AnticaptchaApi.ApiMethod.GetBalance, postData);

            if (postResult == null)
            {
                DebugHelper.Out("API error", DebugHelper.Type.Error);
                return null;
            }

            if (!postResult.Success)
            {
                //TODO!.
                // return new TaskActionResult { Success = false, Message = postResult.Error };
            }
            var balanceResponse = new BalanceResponse(postResult.Response);

            if (!balanceResponse.ErrorId.Equals(0))
            {
                ErrorMessage = balanceResponse.ErrorDescription;
                DebugHelper.Out("API error " + balanceResponse.ErrorId + ": " + balanceResponse.ErrorDescription, DebugHelper.Type.Error);
                return null;
            }

            return balanceResponse.Balance;
        }

        public SolutionData SolveCaptcha()
        {
            CreateTask();
            WaitForTaskResult();
            return GetTaskSolution();
        }

        public async Task<SolutionData> SolveCaptchaAsync()
        {
            await CreateTaskAsync();
            await WaitForTaskResultAsync();
            return GetTaskSolution();
        }
    }
}