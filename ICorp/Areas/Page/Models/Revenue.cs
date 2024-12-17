namespace InventoryIT.Areas.Page.Models
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

    public class ProjectRevenue 
    { 
        public int? IDMapping { get; set;} 
        public int? IDHeader { get; set;} 
        public int? IDProject { get; set;} 
        public int? IDSegmen { get; set;} 
        public int? IDAsset { get; set;} 
        public int? IDCustomer { get; set;} 
        public int? IDContract { get; set;} 
        public int? IDPekerjaan { get; set;} 
        public String? Segmen { get; set;} 
        public String? Asset { get; set;} 
        public String? Customer { get; set;} 
        public String? Contract { get; set;} 
        public String? Pekerjaan { get; set;} 
       
    }

    public class ProjectDD
    {
        public int ID { get;  set;}
        public String? NamaProject { get; set;}
    }

    public class CostCenterFill
    {
        public string? FundsCenter { get; set; }
        public string? Name { get; set; }
    }

    public class RegionalFill
    {
        public int ID { get; set;}
        public string? Customer { get; set; }
        public string? Regional { get; set; }

    }

    public class ProjectExist
    {
        public int ID { get; set; }
        public string? NamaProject { get; set; }
        public string? Segmen { get; set; }
        public int IDSegmen { get; set; }
        public string? Asset { get; set; }
        public int IDAsset { get; set; }
        public string? CostCenter { get; set; }
        public string? Customer { get; set; }
        public int IDCustomer { get; set; }
        public string? Regional { get; set; }
        public string? Contract { get; set; }
        public int IDContract { get; set; }
        public string? Probability { get; set; }
        public string? Sumur { get; set; }
        public string? ControlProject { get; set; }
        public int IDPekerjaan { get; set; }
        public string? Pekerjaan { get; set; }
        public int IDSBT { get; set; }
        public string? SbtIndex { get; set; }
    }

    public class MappingProjectRevenue
    {
        public int IDHeader { get; set; }
        public int IDProject { get; set; }
        public string? NamaProject { get; set; }
        public int Segmen { get; set; }
        public int Asset { get; set; }
        public int Customer { get; set; }
        public int Contract { get; set; }
        public string? Probability { get; set; }
        public string? Sumur { get; set; }
        public string? ControlProject { get; set; }
        public int Pekerjaan { get; set; }
        public int SBT { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class ImportProject
    {
        public int IDHeader { get; set; }
        public string? NamaProject { get; set; }
        public string? Segmen { get; set; }
        public string? Asset { get; set; }
        public string? Customer { get; set; }
        public string? Contract { get; set; }
        public string? Probability { get; set; }
        public string? Sumur { get; set; }
        public string? ControlProject { get; set; }
        public string? Pekerjaan { get; set; }
        public string? SBT { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class DetailRevenue
    {
        public int IDHeader { get; set;}
        public int IDProject { get; set;}
        public string? Project { get; set;}
        public Decimal? Januari { get; set; }
        public Decimal? Februari { get; set;}
        public Decimal? Maret { get; set; }
        public Decimal? April { get; set; }
        public Decimal? Mei { get; set; }
        public Decimal? Juni { get; set; }
        public Decimal? Juli { get; set; }
        public Decimal? Agustus { get; set; }
        public Decimal? September { get; set; }
        public Decimal? Oktober { get; set; }
        public Decimal? November { get; set; }
        public Decimal? Desember { get; set; }
        public Decimal? Total { get; set; }
    }

    public class ImportLog
    {
        public int RowNumber { get; set; }
        public string? NamaProject { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class AmountLog
    {
        public int idProject { get; set; }
        public string? Month { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class AmountRevenue
    {
        public int Project { get; set; }
        public int IDHeader { get; set; }
        public Dictionary<string, decimal> Changes { get; set; }
    }
}
