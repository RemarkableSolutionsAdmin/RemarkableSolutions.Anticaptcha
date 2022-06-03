﻿using System;
using System.Collections.Generic;
using System.Globalization;
using RemarkableSolutions.Anticaptcha.Helpers;

namespace RemarkableSolutions.Anticaptcha.Api.Responses
{
    public partial class TaskResultResponse
    {
        public enum StatusType
        {
            Processing,
            Ready
        }

        public TaskResultResponse(dynamic json)
        {
            ErrorId = JsonHelper.ExtractInt(json, "errorId");

            if (ErrorId != null)
                if (ErrorId.Equals(0))
                {
                    Status = ParseStatus(JsonHelper.ExtractStr(json, "status"));

                    if (Status.Equals(StatusType.Ready))
                    {
                        Cost = JsonHelper.ExtractDouble(json, "cost");
                        Ip = JsonHelper.ExtractStr(json, "ip", null, true);
                        SolveCount = JsonHelper.ExtractInt(json, "solveCount", null, true);
                        CreateTime = UnixTimeStampToDateTime(JsonHelper.ExtractDouble(json, "createTime"));
                        EndTime = UnixTimeStampToDateTime(JsonHelper.ExtractDouble(json, "endTime"));

                        Solution = new SolutionData
                        {
                            Token = JsonHelper.ExtractStr(json, "solution", "token", true),
                            GRecaptchaResponse =
                                JsonHelper.ExtractStr(json, "solution", "gRecaptchaResponse", silent: true),
                            GRecaptchaResponseMd5 =
                                JsonHelper.ExtractStr(json, "solution", "gRecaptchaResponseMd5", silent: true),
                            Text = JsonHelper.ExtractStr(json, "solution", "text", silent: true),
                            Url = JsonHelper.ExtractStr(json, "solution", "url", silent: true),
                            Challenge = JsonHelper.ExtractStr(json, "solution", "challenge", silent: true),
                            Seccode = JsonHelper.ExtractStr(json, "solution", "seccode", silent: true),
                            Validate = JsonHelper.ExtractStr(json, "solution", "validate", silent: true),
                            CaptchaId = JsonHelper.ExtractStr(json, "solution", "captcha_id", silent: true),
                            LotNumber = JsonHelper.ExtractStr(json, "solution", "lot_number", silent: true),
                            PassToken = JsonHelper.ExtractStr(json, "solution", "pass_token", silent: true),
                            GenTime = JsonHelper.ExtractInt(json, "solution", "gen_time", silent: true),
                            CaptchaOutput = JsonHelper.ExtractStr(json, "solution", "captcha_output", silent: true),
                            Cookies = json["solution"]["cookies"],
                            LocalStorage = json["solution"]["localStorage"],
                            Fingerprint = json["solution"]["fingerprint"],
                            Domain = JsonHelper.ExtractStr(json, "solution", "domain", silent: true),
                        };

                        try
                        {
                            Solution.CellNumbers = json["solution"]["cellNumbers"].ToObject<List<int>>();
                        }
                        catch
                        {
                            Solution.CellNumbers = new List<int>();
                        }

                        try
                        {
                            Solution.Answers = json.solution.answers;
                        }
                        catch
                        {
                            Solution.Answers = null;
                        }

                        if (Solution.GRecaptchaResponse == null && Solution.Text == null && Solution.Answers == null
                            && Solution.Token == null && Solution.Challenge == null && Solution.Seccode == null &&
                            Solution.Validate == null && Solution.CellNumbers.Count == 0 && Solution.LocalStorage == null
                            && Solution.Cookies == null && Solution.Fingerprint == null && Solution.CaptchaId == null)
                        {
                            DebugHelper.Out("Got no 'solution' field from API", DebugHelper.Type.Error);
                        }
                    }
                }
                else
                {
                    ErrorCode = JsonHelper.ExtractStr(json, "errorCode");
                    ErrorDescription = JsonHelper.ExtractStr(json, "errorDescription") ?? "(no error description)";

                    DebugHelper.Out(ErrorDescription, DebugHelper.Type.Error);
                }
            else
            {
                DebugHelper.Out("Unknown error", DebugHelper.Type.Error);
            }
        }

        public int? ErrorId { get; }
        public string ErrorCode { get; private set; }
        public string ErrorDescription { get; }
        public StatusType? Status { get; }
        public SolutionData Solution { get; }
        public double? Cost { get; private set; }
        public string Ip { get; private set; }

        /// <summary>
        ///     Task create time in UTC
        /// </summary>
        public DateTime? CreateTime { get; private set; }

        /// <summary>
        ///     Task end time in UTC
        /// </summary>
        public DateTime? EndTime { get; private set; }

        public int? SolveCount { get; private set; }

        private StatusType? ParseStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
                return null;

            try
            {
                return (StatusType)Enum.Parse(
                    typeof(StatusType),
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(status),
                    true
                );
            }
            catch
            {
                return null;
            }
        }

        private static DateTime? UnixTimeStampToDateTime(double? unixTimeStamp)
        {
            if (unixTimeStamp == null)
                return null;

            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return dtDateTime.AddSeconds((double)unixTimeStamp).ToUniversalTime();
        }
    }
}