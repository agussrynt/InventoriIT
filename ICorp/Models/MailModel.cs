namespace InventoryIT.Models
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

    public class MailAccModel
    {
        public string Nama { get; set; }
        public string Email { get; set; }
    }
}
