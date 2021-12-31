//If I had more time to work on this I would create some tests with Moq
//where I would have everything mocked out to do proper unit tests
//I've got very limited time with crunch at my current job so my tests are more basic

namespace Service1Tests
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NUnit.Framework;
    using FluentAssertions;
    using SFSServices.S1.Models;
    using Microsoft.AspNetCore.Mvc;
    using SFSServices.S1.Repositories;
    using MongoDB.Driver;
    using SFSServices.Models;
    using SFSServices.S1;

    internal class Service1TestsNoHTTPCosmosDB
    {
        private TestData? TestData;
        private ILogger<Service1>? logger;
        private CreditRepositoryCosmosDB? repo;

        private MongoClient? mongoClient;
       
        //I would come up with a better way of injecting theese into the local tests, because I believe this isn't the same on all machines, if it is the same then no issue.
        private static readonly string connString = "mongodb://localhost:C2y6yDjf5%2FR%2Bob0N8A7Cgv30VRDJIWEHLM%2B4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw%2FJw%3D%3D@localhost:10255/admin?ssl=true";
        private static readonly string dbName = "CreditDatabase";

        [SetUp]
        public async Task Setup()
        {
            logger = new LoggerFactory().CreateLogger<Service1>();

            mongoClient = new MongoClient(connString);
            var db = mongoClient.GetDatabase(dbName);
            repo = new CreditRepositoryCosmosDB(db);

            TestData = new TestData();

            foreach(var i in TestData.CreditReports)
            {
                await repo.AddCreditReportAsync(i);
            }
        }

        [Test]
        [TestCase(1001500)]
        [TestCase(1001501)]
        [TestCase(2001523)]
        public async Task Service1ShouldReturnValues(int applicationId)
        {
            var query = new CreditReportQuery() { ApplicationId = applicationId, Source = "ABC", Bureau = "EFX" };

            var S1 = new Service1(repo);

            var result = await S1.Get(query, logger);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();

            var rawValue = (result as OkObjectResult)?.Value;
            rawValue.Should().NotBeNull();
            rawValue.Should().BeAssignableTo<CreditReport>();

            var creditData = rawValue as CreditReport;

            creditData?.ApplicationId.Should().Be(query.ApplicationId);
            creditData?.Bureau.Should().Be(query.Bureau);
            creditData?.Source.Should().Be(query.Source);

            creditData?.Tradelines.Length.Should().BeGreaterThan(0);
            foreach (var TL in creditData?.Tradelines)
            {
                TL.TradelineId.Should().NotBe(0);
                TL.AccountNumber.Should().NotBeNullOrEmpty();
                TL.Balance.Should().BeGreaterThan(0);
                TL.MonthlyPayment.Should().BeGreaterThan(0);
                TL.TradelineType.Should().NotBeNullOrEmpty();
            }
        }

        [Test]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(3000044)]
        public async Task Service1ShouldNotReturnValues(int applicationId)
        {
            var query = new CreditReportQuery() { ApplicationId = applicationId, Source = "ABC", Bureau = "EFX" };

            var S1 = new Service1(repo);

            var result = await S1.Get(query, logger);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();

            var rawValue = (result as NotFoundObjectResult)?.Value;
            rawValue.Should().NotBeNull();
            rawValue.Should().BeAssignableTo<CreditReportQuery>();

            var returnedQuery = (rawValue as CreditReportQuery);
            returnedQuery.Should().NotBeNull();
            returnedQuery.Should().BeEquivalentTo(query);
        }
    }
}
