namespace SFSServices.S2
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class Service2
    {
        private readonly IDTIService DTIService;
        public Service2(IDTIService _DTIService) => DTIService = _DTIService;

        [FunctionName("GetDTI")]
        public async Task<IActionResult> Get(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] DTIQuery req,
           ILogger log)
        {
            log.LogInformation("GetDTI function recieved a request." + JsonConvert.SerializeObject(req));

            var (DTIData,error) = await DTIService.GetDTIData(req, log);

            if (error != null)
            { //I would do more robust and descriptive error handling if had more time.
                log.LogInformation("Credit Report with appID " + req.AppId + " not Found!");
                return new NotFoundObjectResult(error);
            }

            log.LogInformation("Credit Report with appID " + req.AppId + " Found!");
            return new OkObjectResult(DTIData);
        }
    }
}
