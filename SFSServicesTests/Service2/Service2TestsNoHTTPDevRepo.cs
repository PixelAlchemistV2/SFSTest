//If I had more time to work on this I would create some tests with Moq
//where I would have everything mocked out to do proper unit tests
//I've got very limited time with crunch at my current job so my tests are more basic
namespace SFSTests.Service2Tests
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NUnit.Framework;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using SFSServices.S1;
    using Service1Tests;
    using Service1Tests.DevRepo;
    using SFSServices.S2;
    using System.Net.Http;
    using SFSTests.Service2Tests.DevConnector;
    using SFSServices.S2.Models;

    internal class Service2TestsNoHTTPDevRepo
    {
        private ILogger<Service1>? logger1;
        private IDTIService? dtiService;

        [SetUp]
        public void Setup()
        {
            logger1 = new LoggerFactory().CreateLogger<Service1>();          
            dtiService = new TestDTIService(new Service1(new DevCreditRepository(new TestData().CreditReports)));
        }

        [Test]
        [TestCase(1001500)]
        [TestCase(1001501)]
        [TestCase(2001523)]
        public async Task Service2ShouldReturnValues(int applicationId)
        {
            var query = new DTIQuery() { AppId = applicationId, AnnualIncome = 500000 };

            var Service2 = new Service2(dtiService);

            var result = await Service2.Get(query, logger1);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();

            var rawValue = (result as OkObjectResult)?.Value;
            rawValue.Should().NotBeNull();
            rawValue.Should().BeAssignableTo<CreditData>();

            var creditData = rawValue as CreditData;

            creditData?.ApplicationId.Should().Be(query.AppId);
            creditData?.AnnualIncome.Should().Be(query.AnnualIncome);
            creditData?.DebtToIncome.Should().NotBeApproximately(0.0f,0.001f);
            creditData?.NumberOfUnsecuredLines.Should().NotBe(0);
            creditData?.TotalUnsecuredBalance.Should().NotBe(0);

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
        public async Task Service2ShouldNotReturnValues(int applicationId)
        {
            var query = new DTIQuery() { AppId = applicationId, AnnualIncome = 500000 };

            var Service2 = new Service2(dtiService);

            var result = await Service2.Get(query, logger1);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();

            var rawValue = (result as NotFoundObjectResult)?.Value;
            rawValue.Should().NotBeNull();
            rawValue.Should().BeAssignableTo<HttpRequestException>();
        }
    }
}