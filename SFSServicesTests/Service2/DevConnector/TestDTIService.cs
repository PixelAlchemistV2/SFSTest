namespace SFSTests.Service2Tests.DevConnector
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using SFSServices.Models;
    using SFSServices.S1;
    using SFSServices.S2;
    using SFSServices.S2.Models;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class TestDTIService : IDTIService
    {
        private readonly Service1 S1;
        public TestDTIService(Service1 s1) => S1 = s1;

        public async Task<(CreditData, HttpRequestException)> GetDTIData(DTIQuery query, ILogger log)
        {
            var result = await S1.Get(new CreditReportQuery() { ApplicationId = query.AppId, Bureau = "", Source = "" }, log);
            var Ok = (result as OkObjectResult);

            if(Ok == null)
            {
                return (null, new HttpRequestException("404 ApplicationId:" + query.AppId + "not found"));
            }
            //I know this looks bonkers but I'm taking advantage of the fact that I can parse the Json of one object to another and drop unused fields for this assignment to keep my code simple and not have a transfer object or some other mechanism
            //it trades a little bit of data on the wire for simplicty in the implementation because this happens in the framework at the model binding stage for the http request
            //The test implementation can't make use of that so I'm chaining conversions to accomplish the same thing.
            var data = JsonConvert.DeserializeObject<CreditData>(JsonConvert.SerializeObject(Ok.Value));
            data.AnnualIncome = query.AnnualIncome;

            return (data,null);
        }
    }
}
