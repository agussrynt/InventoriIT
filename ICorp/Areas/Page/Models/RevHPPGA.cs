namespace PlanCorp.Areas.Page.Models
{
    public class RevHPPGA
    {
    }

    public class RevenueRJPP
    {
        public int ID { get; set; }
        public int Id_DetailRevHPPGA { get; set; }
        public int Tahun { get; set; }
        public int Revenue { get; set; }
        public int TotalRevenue { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class HPPRJPP
    {
        public int ID { get; set; }
        public int Id_DetailRevHPPGA { get; set; }
        public int Tahun { get; set; }
        public int HPP { get; set; }
        public int TotalHPP { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class DetailRevGAHPP
    {
        public int ID { get; set; }
        public string? SegmentRJPP { get; set; }
        public string? NamaCostCenter { get; set; }
        public string? HP { get; set; }
        public string? UniqueCode { get; set; }
        public string? PIC { get; set; }
        public string? Costumer { get; set; }
        public string? Project { get; set; }
        public string? HPPSales { get; set; }
        public string? GASales { get; set; }
        public string? Ownership { get; set; }
        public string? Probability { get; set; }
        public int Tahun { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string? UpdatedBy { get; set; }
    }
}