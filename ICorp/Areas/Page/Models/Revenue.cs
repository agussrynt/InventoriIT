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

    public class ProjectExist
    {
        public int ID { get; set; }
        public string? NamaProject { get; set; }
        public string? Segmen { get; set; }
        public int IDSegmen { get; set; }
        public string? Asset { get; set; }
        public int IDAsset { get; set; }
        public string? Customer { get; set; }
        public int IDCustomer { get; set; }
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
}
