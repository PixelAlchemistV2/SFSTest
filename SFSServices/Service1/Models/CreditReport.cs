namespace SFSServices.S1.Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using SFSServices.Models;

    public record class CreditReport
    {
        [BsonId]
        public int ApplicationId;
        public string CustomerName;
        public string Source;
        public string Bureau;
        //public int MinPaymentPercentage; //This seems obsolete? It's in the model but not used.
        public Tradeline[] Tradelines;
    }
}
