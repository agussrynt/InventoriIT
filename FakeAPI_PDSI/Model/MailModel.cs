namespace FakeAPI_PDSI.Model
{
    public class MailModel
    {
        public string recipient { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }

    public class MailResultModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public MailResultModel()
        {
            Status = "F";
            Message = "Invalid Username";
        }
    }
}
