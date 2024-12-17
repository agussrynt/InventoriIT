namespace InventoryIT.Areas.Page.Models
{
    public class RevHPPGADetail
    {
        public int ID { get; set; }
        public string? SegmentRJPP { get; set; }
        public string? NamaCostCenter { get; set; }
        public string? HP { get; set; }
        public string? KategoriRIG { get; set; }
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

    public class RevHPPGAView
    {
        public int ID { get; set; }
        public string? SegmentRJPP { get; set; }
        public string? NamaCostCenter { get; set; }
        public string? HP { get; set; }
        public string? KategoriRIG { get; set; }
        public string? UniqueCode { get; set; }
        public string? PIC { get; set; }
        public string? Costumer { get; set; }
        public string? Project { get; set; }
        public string? HPPSales { get; set; }
        public string? GASales { get; set; }
        
        // Revenue as dynamic dictionary
        public Dictionary<string, int?> Revenues { get; set; } = new Dictionary<string, int?>();

        // HPP as dynamic dictionary
        public Dictionary<string, int?> HPPs { get; set; } = new Dictionary<string, int?>();
    }

    public class SummaryView
    {
        public string? Category { get; set; }
        public Dictionary<string, int?> RJPP { get; set; } = new Dictionary<string, int?>();

    }
}