namespace PlanCorp.Areas.Page.Models
{
    public class HeaderRevenue
    {
        public int? ID { get; set; }
        public string? Tahun { get; set; }
        public decimal? RJPPNextSta { get; set; }
        public decimal? RKAPYearSta { get; set; }
        public decimal? Prognosa { get; set; }
        public decimal? RealisasiBackYear { get; set; }
        public DateTime? CreatedTime { get; set;}
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string? UpdatedBy { get; set; }

    }

    public class HeaderView
    {
        public int? ID { get; set; }
        public string? Tahun { get; set; }
        public decimal? RJPPNextSta { get; set; }
        public decimal? RKAPYearSta { get; set; }
        public decimal? Prognosa { get; set; }
        public decimal? RealisasiBackYear { get; set; }
        public int? TotalProject {  get; set; }
    }
}
