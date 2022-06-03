using System;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Base;

namespace RemarkableSolutions.Anticaptcha.Api.Anticaptchas
{
    public class RecaptchaV2ProxylessTask : AnticaptchaBase
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string WebsiteSToken { protected get; set; }
        public bool IsInvisible { protected get; set; }
        public string DataSValue { protected get; set; }

        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "RecaptchaV2TaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"websiteKey", WebsiteKey},
                {"websiteSToken", WebsiteSToken},
                {"recaptchaDataSValue", DataSValue},
                {"isInvisible", IsInvisible}
            };
        }

        protected override bool IsPostDataValid()
        {
            return !string.IsNullOrEmpty(WebsiteUrl.AbsoluteUri);
        }
    }
}