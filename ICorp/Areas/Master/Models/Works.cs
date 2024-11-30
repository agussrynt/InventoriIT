namespace PlanCorp.Areas.Master.Models
{
    public class Works
    {
        public int ID { get; set; }
        public string? Pekerjaan { get; set; }
        public int IsAktif { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
    }
}