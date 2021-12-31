namespace SFSServices.Models
{
    [Microsoft.AspNetCore.Mvc.BindProperties]
    public class CreditReportQuery
    {
        public int ApplicationId { get; set; }
        //If we are only supporting one source/bureau can I eliminate the variables below and distill this down to just a single int to get?
        public string Source { get; set; }
        public string Bureau { get; set; }
    }
}
