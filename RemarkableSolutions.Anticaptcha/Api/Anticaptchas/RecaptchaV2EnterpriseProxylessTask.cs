using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Base;

namespace RemarkableSolutions.Anticaptcha.Api.Anticaptchas
{
    public class RecaptchaV2EnterpriseProxylessTask : AnticaptchaBase
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public Dictionary<string, string> EnterprisePayload = new Dictionary<string, string>();

        public override JObject GetPostData()
        {
            var jsonObject = new JObject
            {
                {"type", "RecaptchaV2EnterpriseTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"websiteKey", WebsiteKey}
            };

            if (EnterprisePayload.Count > 0)
            {
                jsonObject["enterprisePayload"] = JObject.FromObject(EnterprisePayload);
            }

            return jsonObject;
        }

        protected override bool IsPostDataValid()
        {
            return !string.IsNullOrEmpty(WebsiteUrl.AbsoluteUri);
        }
    }
}