namespace SFSServices.S2.Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using SFSServices.Models;
    using System.Linq;

    public class CreditData
    {
        [BsonId]
        public int ApplicationId { get; init; }
        private int annualIncome;
        public int AnnualIncome { get => annualIncome; set { annualIncome = value; UpdateDTI(); } }
        public int TotalUnsecuredBalance;
        public int NumberOfUnsecuredLines;
        public float DebtToIncome;
        public Tradeline[] Tradelines { get; init; }
        //Would add a timestamp for when this data was pulled so I know when it's no longer valid in the cache, I found online that a 'credit pull' is valid for 90 days.

        private void UpdateDTI()
        {
            float monthlyIncome = annualIncome / 12.0f;
            DebtToIncome = Tradelines.Where(TL => !TL.IsMortgage).Sum(TL => TL.MonthlyPayment) / monthlyIncome;
        }

        public CreditData(int appId, Tradeline[] tradelines)
        {
            ApplicationId = appId;
            Tradelines = tradelines;

            var unsecured = Tradelines.Where(TL => TL.TradelineType == "UNSECURED");
            NumberOfUnsecuredLines = unsecured.Count();
            TotalUnsecuredBalance = unsecured.Sum(TL => TL.Balance);
        }
    }  
}
