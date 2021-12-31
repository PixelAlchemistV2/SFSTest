namespace Service1Tests
{
    using Newtonsoft.Json;
    using SFSServices.S1.Models;
    using System;
    using System.IO;

    internal class TestData
    {   
        //This nested class just lets me use newtonsoft to very easily read the json data from the text file
        internal class CreditReportTestData
        {
            public CreditReport[] creditReports = Array.Empty<CreditReport>();
        }

        private readonly CreditReportTestData reports;
        public CreditReport[] CreditReports { get => reports.creditReports; }

        public TestData()
        {
            reports = JsonConvert.DeserializeObject<CreditReportTestData>(File.ReadAllText("./TestData.txt"));
        }
    }
}
