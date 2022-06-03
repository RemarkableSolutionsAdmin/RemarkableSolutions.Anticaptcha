using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Base;

namespace RemarkableSolutions.Anticaptcha.Api.Anticaptchas
{
    public class GeeTestV4ProxylessTask : AnticaptchaBase
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsiteKey { protected get; set; }
        public string GeetestApiServerSubdomain { protected get; set; }
        public Dictionary<string, string> initParameters = new Dictionary<string, string>();

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                {"type", "GeeTestTaskProxyless"},
                {"websiteURL", WebsiteUrl},
                {"gt", WebsiteKey},
                {"version", 4},
            };

            if (!string.IsNullOrEmpty(GeetestApiServerSubdomain))
            {
                postData["geetestApiServerSubdomain"] = GeetestApiServerSubdomain;
            }
            if (initParameters.Count > 0)
            {
                postData["initParameters"] = JObject.FromObject(initParameters);
            }

            return postData;
        }

        protected override bool IsPostDataValid()
        {
            return !string.IsNullOrEmpty(WebsiteUrl.AbsoluteUri);
        }
    }
}