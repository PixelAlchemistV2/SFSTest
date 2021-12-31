namespace SFSServices.S1
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using SFSServices.Models;
    using SFSServices.S1.Models;
    using SFSServices.S1.Repositories;

    public class Service1
    {
        private readonly ICreditRepository _repository;
        public Service1(ICreditRepository repository) => _repository = repository;

        [FunctionName("GetCreditReport")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] CreditReportQuery req,
            ILogger log)
        {
            log.LogInformation("GetCreditReport function recieved a request." + JsonConvert.SerializeObject(req));

            CreditReport report = await _repository.GetCreditReportAsync(req.ApplicationId);

            if(report == null)
            {
                log.LogInformation("Credit Report with appID " + req.ApplicationId + " not Found!");
                return new NotFoundObjectResult(req);
            }

            log.LogInformation("Credit Report with appID " + req.ApplicationId + " Found!");
            return new OkObjectResult(report);
        }
    }
}
