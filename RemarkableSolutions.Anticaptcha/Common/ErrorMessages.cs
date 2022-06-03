namespace RemarkableSolutions.Anticaptcha.Common
{
    public static class ErrorMessages
    {
        public const string AnticaptchaNoSolutionFromAPIError = "No solution from API";
        public const string AnticaptchaApiError = "Anticaptcha API error";
        public const string AnticaptchaPostDataValidationError = "Anticaptcha validation error";
        public const string AnticaptchaPreparingTaskError = "Anticaptcha task preparing error";
        public const string AnticaptchaNullTaskError = "Anticaptcha task id is null";
        public const string AnticaptchaTimeoutError = "Anticaptcha task timeout";
        public const string AnticaptchaApiGetTaskError = "Anticaptcha API get task error";
        public const string AnticaptchaUnknownStatusError = "An unknown API status, please update your software";

        public static string AnticaptchaApiSpecificError (string errorCode, string errorDescription) => $"API Error {errorCode} : {errorDescription}";
    }
}