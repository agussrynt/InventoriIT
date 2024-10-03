namespace PlanCorp.Areas.Master.Models
{
    public class YearData
    {
        public int Id_YearData { get; set; }
        public string? Year { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
