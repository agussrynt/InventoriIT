namespace InventoryIT.Areas.Page.Models
{
    public class MasterKertasKerja
    {
        public int Year { get; set; }
        public string Indikator { get; set; }
        public string Parameter { get; set; }
        public string FaktorUjiKesesuaian { get; set; }
        public string unsurPemenuhan { get; set; }
    }

    public class Indikator
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ImportMKK
    {
        public IFormFile FileUpload { get; set; }
    }

    public class StoreModel
    {
        public string Year { get; set; }
        public string UserName { get; set; }
        public ParameterHeader[] ParameterHeader { get; set; }
        public ParameterContent[] ParameterContent { get; set; }
        public ParameterFUK[] ParameterFUK { get; set; }
        public ParameterUP[] ParameterUP { get; set; }
    }

    public class MasterAuditViewModel
    {
        public string Year { get; set; }
        public string UserName { get; set; }
        public List<ParameterHeader> ParameterHeader { get; set; }
        public List<ParameterContent> ParameterContent { get; set; }
        public List<ParameterFUK> ParameterFUK { get; set; }
        public List<ParameterUP> ParameterUP { get; set; }

        public MasterAuditViewModel()
        {
            ParameterHeader = new List<ParameterHeader>();
            ParameterContent = new List<ParameterContent>();
            ParameterFUK = new List<ParameterFUK>();
            ParameterUP = new List<ParameterUP>();
        }
    }

    public class ParameterHeader
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Aspek { get; set; }
    }

    public class ParameterContent
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public double Bobot { get; set; }
        public string Aspek { get; set; }
    }

    public class ParameterFUK
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int Sequence { get; set; }
        public int Parent { get; set; }
        public string Child { get; set; }
        public string Description { get; set; }
        public string ContentDescription { get; set; }
        public int? ChildParent { get; set; }
    }

    public class ParameterUP
    {
        public int Id { get; set; }
        public int FUKId { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public string FUKDescription { get; set; }
    }
}
