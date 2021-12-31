namespace Service1Tests.DevRepo
{
    using SFSServices.S1.Models;
    using SFSServices.S1.Repositories;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    //This is just a mock of a datastore so I can isolate the function app logic for testing. I could use Moq but this was just quick and easy
    public class DevCreditRepository : ICreditRepository
    {
        private readonly CreditReport[] creditDatas;
        public DevCreditRepository(CreditReport[] creditDatas) => this.creditDatas = creditDatas;

        public Task AddCreditReportAsync(CreditReport _) => throw new NotImplementedException(); //This is just for basic unit tests

#pragma warning disable CS1998 //Justification: this is the bare minium implementation for unit testing 
        public async Task<CreditReport?> GetCreditReportAsync(int applicationId) =>
            creditDatas.Where(x => x.ApplicationId == applicationId).FirstOrDefault();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
