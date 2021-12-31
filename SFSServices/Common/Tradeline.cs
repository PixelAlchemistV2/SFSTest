namespace SFSServices.Models
{
    using Newtonsoft.Json;

    public class Tradeline
    {
        public ulong TradelineId;
        public string AccountNumber; //Could this be a ulong or some other non string value for smaller storage?
        public int Balance;
        public int MonthlyPayment;
        [JsonProperty("type")] //I am just swapping from type which with naming convetion rules would be Type to a more friendly and descriptive name to avoid confusion with System.Type
        public string TradelineType; //Could this be a enum, or even a boolean? it seems like the options are really secured and unsecured, maybe bool isSecured instead?
        public bool IsMortgage; //Are all mortgages secured? are all non mortagaues unsecured?
    }
}
