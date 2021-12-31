namespace SFSServices.S1.Repositories
{
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using SFSServices.S1.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Uses the MongoAPI for CosmosDB because it's a bit easier/cleaner than an SQL api for such a simple application
    /// </summary>
    public class CreditRepositoryCosmosDB : ICreditRepository
    {
        private readonly IMongoCollection<CreditReport> creditCollection;
        private readonly ReplaceOptions replace = new() { IsUpsert = true };

        public CreditRepositoryCosmosDB(IMongoDatabase database)
            => creditCollection = database.GetCollection<CreditReport>("CreditData");

        public async Task AddCreditReportAsync(CreditReport toAdd)
            => await creditCollection.ReplaceOneAsync(x => x.ApplicationId == toAdd.ApplicationId, toAdd, replace);

        public async Task<CreditReport> GetCreditReportAsync(int applicationId)
            => await creditCollection.AsQueryable().FirstOrDefaultAsync(x => x.ApplicationId == applicationId);
    }
}
