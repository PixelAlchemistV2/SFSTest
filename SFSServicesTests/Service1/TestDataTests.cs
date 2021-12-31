namespace Service1Tests
{
    using NUnit.Framework;
    using FluentAssertions;

    //This is just to confirm that the test data is being read correctly for other unit tests to use
    public class TestDataTests
    {
        private TestData? TestData;

        [SetUp]
        public void Setup()
        {
            TestData = new TestData();
        }

        [Test]
        public void ConfirmTestDataSetup()
        {
            TestData.Should().NotBeNull();

            TestData?.CreditReports.Should().NotBeNull();

            TestData?.CreditReports.Length.Should().BeGreaterThan(0);
        }
    }
}