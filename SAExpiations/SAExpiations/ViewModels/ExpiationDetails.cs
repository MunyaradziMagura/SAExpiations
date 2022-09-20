namespace SAExpiations.ViewModels
{
    public class ExpiationDetails
    {
        public string ExpiationOffenceCode { get; set; } = null!;
        public string NoticeStatusDesc { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public int StatusCount { get; set; }


    }
}
