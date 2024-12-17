namespace InventoryIT.Areas.Page.Models
{
    public class DokumenUP
    {
        public int Id { get; set; }
        //public int FukId { get; set; }
        public int Sequence { get; set; }
        public int Parent { get; set; }
        public string Child { get; set; }
        public string Year { get; set; }
        public string Indikator { get; set; }
        public string Parameter { get; set; }
        public int Status { get; set; }
        public string FaktorUjiKesesuaian { get; set; }
        public string UnsurPemenuhan { get; set; }
        public string Upload { get; set; }
        public string Remarks { get; set; }
        public string UpStatus { get; set; }
        public string DocumentReview { get; set; }
        public string FilePath { get; set; }
    }

    public class DokumenList
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DokumenFile
    {
        public int Id { get; set; }
        public int FukId { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }
    }

    public class DokumenUpload
    {
        public int UpID { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
        public string userName { get; set; }
    }
}
