namespace SFSServices.S1.Repositories
{
    using SFSServices.S1.Models;
    using System.Threading.Tasks;

    public interface ICreditRepository
    {
        Task<CreditReport> GetCreditReportAsync(int applicationId);
        //If we are only supporting the EFX/ABC source bureau... and this service is supposed to mimic that source... do we need to specify the extra parameters... I opted for no.
        //Task<CreditReport> GetCreditReportAsync(int applicationId, string Source, string Bureau); 
        Task AddCreditReportAsync(CreditReport creditData);
    }
}
