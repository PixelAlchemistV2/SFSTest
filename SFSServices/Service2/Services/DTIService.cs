namespace SFSServices.S2
{
    using System.Threading.Tasks;
    using System.Net.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using SFSServices.S2.Models;
    using SFSServices.Models;

    public class DTIService : IDTIService
    {
        private readonly HttpClient httpClient;
        private static readonly string queryStringPrefix = "?" + nameof(CreditReportQuery.ApplicationId) + "=";
        //If we are only supporting ABC/EFX I don't give the option of even querying for anything else. I didn't change the request format though because I suspect it's supposed to represent a service I don't control.
        private static readonly string queryStringSuffix = "&" + nameof(CreditReportQuery.Source) + "=ABC&" + nameof(CreditReportQuery.Bureau) + "=EFX";
        
        public DTIService(HttpClient _httpClient) => httpClient = _httpClient;

        public async Task<(CreditData, HttpRequestException)> GetDTIData(DTIQuery query, ILogger log)
        {
            var (CreditData, error) = await GetCreditData(query.AppId, log);

            if (CreditData == null)
            {
                return (null, error);
            }

            CreditData.AnnualIncome = query.AnnualIncome;
           
            return (CreditData, null);
        }

        //Bullet Point Answer: If the response took 5+ seconds I would cache the info in our database and only pull from the external service if I got a cache miss here.
        private async Task<(CreditData, HttpRequestException)> GetCreditData(int appId, ILogger log)
        {
            try
            {
                var response = await httpClient.GetAsync("GetCreditReport" + GetDefaultQueryString(appId));
                response.EnsureSuccessStatusCode();

                return (JsonConvert.DeserializeObject<CreditData>(await response.Content.ReadAsStringAsync()), null);
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex.Message);
                return (null, ex);
            }
        }

        private static string GetDefaultQueryString(int appId) => queryStringPrefix + appId + queryStringSuffix;
    }
}
