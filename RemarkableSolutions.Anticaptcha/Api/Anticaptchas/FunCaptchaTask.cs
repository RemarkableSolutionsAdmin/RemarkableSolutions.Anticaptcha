using System;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Base;
using RemarkableSolutions.Anticaptcha.Enums;
using RemarkableSolutions.Anticaptcha.Helpers;

namespace RemarkableSolutions.Anticaptcha.Api.Anticaptchas
{
    public class FunCaptchaTask : AnticaptchaBase
    {
        public Uri WebsiteUrl { protected get; set; }
        public string WebsitePublicKey { protected get; set; }
        public string ProxyLogin { protected get; set; }
        public string ProxyPassword { protected get; set; }
        public int? ProxyPort { protected get; set; }
        public ProxyTypeOption? ProxyType { protected get; set; }
        public string ProxyAddress { protected get; set; }
        public string UserAgent { protected get; set; }

        public override JObject GetPostData()
        {
            if (ProxyType == null || ProxyPort == null || ProxyPort < 1 || ProxyPort > 65535 ||
                string.IsNullOrEmpty(ProxyAddress))
            {
                DebugHelper.Out("Proxy data is incorrect!", DebugHelper.Type.Error);

                return null;
            }

            return new JObject
            {
                {"type", "FunCaptchaTask"},
                {"websiteURL", WebsiteUrl},
                {"websitePublicKey", WebsitePublicKey},
                {"proxyType", ProxyType.ToString().ToLower()},
                {"proxyAddress", ProxyAddress},
                {"proxyPort", ProxyPort},
                {"proxyLogin", ProxyLogin},
                {"proxyPassword", ProxyPassword},
                {"userAgent", UserAgent}
            };
        }

        protected override bool IsPostDataValid()
        {
            return ProxyType != null &&
                   ProxyPort != null &&
                   !(ProxyPort < 1) &&
                   !(ProxyPort > 65535) &&
                   !string.IsNullOrEmpty(ProxyAddress);
        }
    }
}