namespace SFSServices.S2
{
    using Microsoft.Extensions.Logging;
    using SFSServices.S2.Models;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IDTIService
    {
        Task<(CreditData, HttpRequestException)> GetDTIData(DTIQuery query, ILogger log);
    }
}