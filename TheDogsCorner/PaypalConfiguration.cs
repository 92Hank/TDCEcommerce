using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheDogsCorner
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;

        static PaypalConfiguration()
        {
            var config = getconfig();
            clientId = "AWIPkEVx73zOSUJBNDKbMfz7QXMHlkjmk34TzQyjPglonJHymHUBZjx6qKyxbV40LPzveDFhmaqoHMlX";
            clientSecret = "EGWmg8talBr4e_EjllyYpNe8twqlj4SKGPsxG_o92JdaEMLCHhF5i35GK6I-tN6DLqZndoole7WGDn88";
        }

        private static Dictionary<string, string> getconfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, getconfig()).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = getconfig();
            return apiContext;
        }
    }
}