//If I had more time to work on this I would create some tests with Moq
//where I would have everything mocked out to do proper unit tests
//I've got very limited time with crunch at my current job so my tests are more basic
namespace SFSTests.Service2Tests
{
    using Service1Tests;
    using SFSServices.S1.Models;
    using NUnit.Framework;
    using FluentAssertions;
    using SFSServices.S2.Models;
    using Newtonsoft.Json;
    using SFSServices.Models;

    internal class Service2CalculationTests
    {
        public CreditReport[] reports;

        [SetUp]
        public void Setup()
        {
            reports = new TestData().CreditReports;
        }

        [Test]
        public void ValidateCalcs()
        {
            var TenPercentDTITest = new CreditData(1, new Tradeline[]
                    {
                    new Tradeline()
                    {
                        TradelineId = 1,
                        Balance = 9000,
                        MonthlyPayment = 100,
                        TradelineType = "UNSECURED",
                        IsMortgage = false
                    },
                    new Tradeline()
                    {
                        TradelineId = 2,
                        Balance = 90000000,
                        MonthlyPayment = 10000,
                        TradelineType = "UNSECURED",
                        IsMortgage = true
                    }
            });

            TenPercentDTITest.AnnualIncome = 12000;

            TenPercentDTITest.NumberOfUnsecuredLines.Should().Be(2);
            TenPercentDTITest.TotalUnsecuredBalance.Should().Be(90009000);
            TenPercentDTITest.DebtToIncome.Should().BeApproximately(.1f, .00001f);

            var ToMuchDebtTest = new CreditData(2, new Tradeline[]
                {
                    new Tradeline()
                    {
                        TradelineId = 1,
                        Balance = 9000,
                        MonthlyPayment = 100,
                        TradelineType = "UNSECURED",
                        IsMortgage = false
                    },
                    new Tradeline()
                    {
                        TradelineId = 2,
                        Balance = 90000000,
                        MonthlyPayment = 10000,
                        TradelineType = "UNSECURED",
                        IsMortgage = false
                    }
                });
            

            ToMuchDebtTest.AnnualIncome = 12000;

            ToMuchDebtTest.NumberOfUnsecuredLines.Should().Be(2);
            ToMuchDebtTest.TotalUnsecuredBalance.Should().Be(90009000);
            ToMuchDebtTest.DebtToIncome.Should().BeApproximately(10.1f, .00001f);
        }

        [Test]
        public void ValidateDefaultTestData()
        {
            reports[0].ApplicationId.Should().Be(1001500);
            var Data = JsonConvert.DeserializeObject<CreditData>(JsonConvert.SerializeObject(reports[0]));

            Data.Should().NotBeNull();

            Data.Tradelines.Length.Should().Be(4);
            Data.Tradelines[0].Balance.Should().Be(5655);
            Data.Tradelines[0].MonthlyPayment.Should().Be(150);
            Data.Tradelines[0].IsMortgage.Should().BeFalse();

            Data.Tradelines[1].Balance.Should().Be(12333);
            Data.Tradelines[1].MonthlyPayment.Should().Be(350);
            Data.Tradelines[1].IsMortgage.Should().BeFalse();

            Data.Tradelines[2].Balance.Should().Be(1255);
            Data.Tradelines[2].MonthlyPayment.Should().Be(50);
            Data.Tradelines[2].IsMortgage.Should().BeFalse();

            Data.Tradelines[3].Balance.Should().Be(135455);
            Data.Tradelines[3].MonthlyPayment.Should().Be(1250);
            Data.Tradelines[3].IsMortgage.Should().BeTrue();

            Data.NumberOfUnsecuredLines.Should().Be(3);
            Data.TotalUnsecuredBalance.Should().Be(19243);

            Data.AnnualIncome = 50000;
            Data.DebtToIncome.Should().BeApproximately(0.132f, 0.001f);

            Data.AnnualIncome = 100000;
            Data.DebtToIncome.Should().BeApproximately(0.066f, 0.001f);

            Data.AnnualIncome = 150000;
            Data.DebtToIncome.Should().BeApproximately(0.044f, 0.001f);

            reports[1].ApplicationId.Should().Be(1001501);
            
            Data = JsonConvert.DeserializeObject<CreditData>(JsonConvert.SerializeObject(reports[1]));

            Data.Should().NotBeNull();

            Data.Tradelines.Length.Should().Be(4);
            Data.Tradelines[0].Balance.Should().Be(2655);
            Data.Tradelines[0].MonthlyPayment.Should().Be(80);
            Data.Tradelines[0].IsMortgage.Should().BeFalse();

            Data.Tradelines[1].Balance.Should().Be(17333);
            Data.Tradelines[1].MonthlyPayment.Should().Be(510);
            Data.Tradelines[1].IsMortgage.Should().BeFalse();

            Data.Tradelines[2].Balance.Should().Be(1355);
            Data.Tradelines[2].MonthlyPayment.Should().Be(55);
            Data.Tradelines[2].IsMortgage.Should().BeFalse();

            Data.Tradelines[3].Balance.Should().Be(158466);
            Data.Tradelines[3].MonthlyPayment.Should().Be(1450);
            Data.Tradelines[3].IsMortgage.Should().BeTrue();

            Data.NumberOfUnsecuredLines.Should().Be(3);
            Data.TotalUnsecuredBalance.Should().Be(21343);

            Data.AnnualIncome = 50000;
            Data.DebtToIncome.Should().BeApproximately(0.1548f, 0.001f);

            Data.AnnualIncome = 100000;
            Data.DebtToIncome.Should().BeApproximately(0.0774f, 0.001f);

            Data.AnnualIncome = 150000;
            Data.DebtToIncome.Should().BeApproximately(0.0516f, 0.001f);

            reports[2].ApplicationId.Should().Be(2001523);
            Data = JsonConvert.DeserializeObject<CreditData>(JsonConvert.SerializeObject(reports[2]));

            Data.Should().NotBeNull();

            Data.Tradelines.Length.Should().Be(4);
            Data.Tradelines[0].Balance.Should().Be(5655);
            Data.Tradelines[0].MonthlyPayment.Should().Be(190);
            Data.Tradelines[0].IsMortgage.Should().BeFalse();

            Data.Tradelines[1].Balance.Should().Be(16333);
            Data.Tradelines[1].MonthlyPayment.Should().Be(487);
            Data.Tradelines[1].IsMortgage.Should().BeFalse();

            Data.Tradelines[2].Balance.Should().Be(1058);
            Data.Tradelines[2].MonthlyPayment.Should().Be(35);
            Data.Tradelines[2].IsMortgage.Should().BeFalse();

            Data.Tradelines[3].Balance.Should().Be(224466);
            Data.Tradelines[3].MonthlyPayment.Should().Be(1830);
            Data.Tradelines[3].IsMortgage.Should().BeTrue();

            Data.NumberOfUnsecuredLines.Should().Be(3);
            Data.TotalUnsecuredBalance.Should().Be(23046);

            Data.AnnualIncome = 50000;
            Data.DebtToIncome.Should().BeApproximately(0.17088f, 0.001f);

            Data.AnnualIncome = 100000;
            Data.DebtToIncome.Should().BeApproximately(0.08544f, 0.001f);

            Data.AnnualIncome = 150000;
            Data.DebtToIncome.Should().BeApproximately(0.05696f, 0.001f);
        }
    }
}
