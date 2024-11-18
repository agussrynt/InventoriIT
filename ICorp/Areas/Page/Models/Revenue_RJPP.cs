namespace PlanCorp.Areas.Page.Models
{
    public class Revenue_RJPP
    {
        public int ID { get; set; }
        public int Id_DetailRevHPPGA { get; set; }
        public int Revenue { get; set; }
        public int TotalRevenue { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
