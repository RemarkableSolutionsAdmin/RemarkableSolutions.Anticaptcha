using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RemarkableSolutions.Anticaptcha.Api.Models
{
    public class SolutionData
    {
        public JObject Answers { get; internal set; } // Will be available for CustomCaptcha tasks only!
        public string GRecaptchaResponse { get; internal set; } // Will be available for Recaptcha tasks only!
        public string GRecaptchaResponseMd5 { get; internal set; } // for Recaptcha with isExtended=true property
        public string Text { get; internal set; } // Will be available for ImageToText tasks only!
        public string Url { get; internal set; } // Will be available for AntiGate tasks
        public string Token { get; internal set; } // Will be available for FunCaptcha tasks only!
        public string Challenge { get; internal set; }  // Will be available for GeeTest tasks only
        public string Seccode { get; internal set; }  // Will be available for GeeTest tasks only
        public string Validate { get; internal set; }  // Will be available for GeeTest tasks only
        public List<int> CellNumbers = new List<int>(); // Will be available for Square tasks only

        // Available for AntiGate tasks only
        public JObject Cookies { get; set; }
        public JObject LocalStorage { get; set; }
        public JObject Fingerprint { get; set; }

        // Available for Geetest V4 only
        public string CaptchaId { get; internal set; }
        public string LotNumber { get; internal set; }
        public string PassToken { get; internal set; }
        public string GenTime { get; internal set; }
        public string CaptchaOutput { get; internal set; }

        public string Domain { get; set; }
        
        public CaptchaTask CaptchaTask { get; set; }

        public bool IsValid()
        {
            return GRecaptchaResponse != null ||
                   Text != null ||
                   Answers != null ||
                   Token != null ||
                   Challenge != null ||
                   Seccode != null ||
                   Validate != null ||
                   CellNumbers.Count != 0 ||
                   LocalStorage != null ||
                   Cookies != null ||
                   Fingerprint != null;
        }
    }
}