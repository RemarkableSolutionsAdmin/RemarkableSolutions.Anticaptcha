using System;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Base;

namespace RemarkableSolutions.Anticaptcha.Api.Anticaptchas
{
    public class AntiGateTask : AnticaptchaBase
    {
        public Uri WebsiteUrl { protected get; set; }
        public string TemplateName { protected get; set; }
        public JObject Variables { protected get; set; }
        public string ProxyAddress { protected get; set; }
        public int ProxyPort { protected get; set; }
        public string ProxyLogin { protected get; set; }
        public string ProxyPassword { protected get; set; }

        public override JObject GetPostData()
        {
            var postData = new JObject
            {
                {"type", "AntiGateTask"},
                {"websiteURL", WebsiteUrl.ToString()},
                {"templateName", TemplateName},
            };

            if (ProxyAddress != null && ProxyPort != 0)
            {
                postData["proxyAddress"] = ProxyAddress;
                postData["proxyPort"] = ProxyPort;
            }

            if (ProxyLogin != null && ProxyPassword != null)
            {
                postData["proxyLogin"] = ProxyLogin;
                postData["proxyPassword"] = ProxyPassword;
            }

            if (Variables != null)
            {
                postData["variables"] = Variables;
            }

            return postData;
        }

        protected override bool IsPostDataValid()
        {
            return !string.IsNullOrEmpty(WebsiteUrl.AbsoluteUri);
        }
    }
}