namespace SFSServices.S2
{
    [Microsoft.AspNetCore.Mvc.BindProperties]
    public class DTIQuery
    {
        public int AppId { get; init; }
        public int AnnualIncome { get; init; }
    }
}
