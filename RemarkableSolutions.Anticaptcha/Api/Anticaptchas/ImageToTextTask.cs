using System.IO;
using Newtonsoft.Json.Linq;
using RemarkableSolutions.Anticaptcha.Api.Base;
using RemarkableSolutions.Anticaptcha.Enums;
using RemarkableSolutions.Anticaptcha.Helpers;

namespace RemarkableSolutions.Anticaptcha.Api.Anticaptchas
{
    public class ImageToTextTask : AnticaptchaBase
    {

        public ImageToTextTask()
        {
            BodyBase64 = "";
            Phrase = false;
            Case = false;
            Numeric = NumericOption.NoRequirements;
            Math = 0;
            MinLength = 0;
            MaxLength = 0;
        }

        public string BodyBase64 { private get; set; }
        public string FilePath
        {
            set
            {
                if (!File.Exists(value))
                {
                    DebugHelper.Out("File " + value + " not found", DebugHelper.Type.Error);
                }
                else
                {
                    BodyBase64 = StringHelper.ImageFileToBase64String(value);

                    if (BodyBase64 == null)
                    {
                        DebugHelper.Out(
                            "Could not convert the file " + value + " to base64. Is this an image file?",
                            DebugHelper.Type.Error
                            );
                    }
                }
            }
        }

        protected override bool IsPostDataValid()
        {
            return !string.IsNullOrEmpty(BodyBase64);
        }

        public bool Phrase { private get; set; }
        public bool Case { private get; set; }
        public NumericOption Numeric { private get; set; }
        public int Math { private get; set; }
        public int MinLength { private get; set; }
        public int MaxLength { private get; set; }

        public override JObject GetPostData()
        {
            if (string.IsNullOrEmpty(BodyBase64))
            {
                return null;
            }

            return new JObject
            {
                {"type", "ImageToTextTask"},
                {"body", BodyBase64.Replace("\r", "").Replace("\n", "")},
                {"phrase", Phrase},
                {"case", Case},
                {"numeric", Numeric.Equals(NumericOption.NoRequirements) ? 0 : Numeric.Equals(NumericOption.NumbersOnly) ? 1 : 2},
                {"math", Math},
                {"minLength", MinLength},
                {"maxLength", MaxLength}
            };
        }
    }
}