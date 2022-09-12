using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTLicVerify.Models;
using Microsoft.Extensions.Logging;


namespace WTLicVerify.EvatoExternallCall
{
    public class EnvatoCalls
    {
        public static async Task<EnvatoAccess> GetAccessToekn(string code) {
            var varRestClient = new RestClient("https://api.envato.com/token");

            var restRequest = new RestRequest(Method.POST);
            restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("application/x-www-form-urlencoded",
                $"grant_type=authorization_code&code={code}&client_id=wtactivation-aad8crdk&client_secret=Lx95GWxfn1kLGwdLkVEyiLNaGBrtWu3i",
                ParameterType.RequestBody);
            restRequest.AddHeader("user-agent", "Purchase code verification");
            IRestResponse varRestResponse = await varRestClient.ExecuteAsync(restRequest);

            if (varRestResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<EnvatoAccess>(varRestResponse.Content);
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> RefreshAccessToekn(string refreshToken)
        {
            var varRestClient = new RestClient("https://api.envato.com/token?grant_type=refresh_token&refresh_token=" 
                + refreshToken + "&client_id=wtactivation-aad8crdk&client_secret=Lx95GWxfn1kLGwdLkVEyiLNaGBrtWu3i");

            var restRequest = new RestRequest(Method.POST);
            restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("application/x-www-form-urlencoded",
                $"grant_type=refresh_token&refresh_token={refreshToken}&client_id=wtactivation-aad8crdk&client_secret=Lx95GWxfn1kLGwdLkVEyiLNaGBrtWu3i",
                ParameterType.RequestBody);

            IRestResponse varRestResponse = await varRestClient.ExecuteAsync(restRequest);

            if (varRestResponse.IsSuccessful)
            {
                EnvatoAccess refreshentity = JsonConvert.DeserializeObject<EnvatoAccess>(varRestResponse.Content);
                return refreshentity.access_token;
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> GetUserEmail(string accessToken)
        {
            var varRestClient = new RestClient("https://api.envato.com/v1/market/private/user/email.json");
            //var varRestClient = new RestClient("https://sandbox.bailey.sh/v1/market/private/user/email.json");
            var restRequest = new RestRequest(Method.GET);
            restRequest.AddHeader("Authorization", "Bearer " + accessToken);
            IRestResponse varRestResponse = await varRestClient.ExecuteAsync(restRequest);

            if (varRestResponse.IsSuccessful)
            {
                dynamic result = JsonConvert.DeserializeObject(varRestResponse.Content);
                return result.email;
            }
            else
            {
                return null;
            }
        }

        public static async Task<Dictionary<string, AuthorSale>> GetUserPurchaseCodes(string accessToken, ILogger<LicController>  logger)
        {
            var varRestClient = new RestClient("https://api.envato.com/v3/market/buyer/purchases");
            //var varRestClient = new RestClient("https://sandbox.bailey.sh/v3/market/buyer/purchases");
            var restRequest = new RestRequest(Method.GET);
            restRequest.AddHeader("Authorization", "Bearer " + accessToken);
            IRestResponse varRestResponse = await varRestClient.ExecuteAsync(restRequest);
           
            if (varRestResponse.IsSuccessful)
            {
                logger.LogDebug("GetUserPurchaseCodes - Successful - " + varRestResponse.Content);
                EnvatoBuyerPurchases result = JsonConvert.DeserializeObject<EnvatoBuyerPurchases>(varRestResponse.Content);
                return result.purchases.Select(x => new { Key = x.code  , Value = x}).ToDictionary(z=> z.Key,z=>z.Value);
            }
            else
            {
                logger.LogDebug("GetUserPurchaseCodes - not Successful - " + varRestResponse.ErrorMessage);
                return new Dictionary<string, AuthorSale>();
            }
        }

    }
}
