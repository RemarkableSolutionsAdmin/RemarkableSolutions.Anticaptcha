using System;

namespace RemarkableSolutions.Anticaptcha.Tests
{
    public static class TestHelper
    {
        public static string ClientKey => Environment.GetEnvironmentVariable("ClientKey");
    }
}