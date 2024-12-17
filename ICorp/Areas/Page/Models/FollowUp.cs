namespace InventoryIT.Areas.Page.Models
{
    public class FollowUpList
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuditBy { get; set; }
        public string AuditDate { get; set; }
        public string Status { get; set; }
    }

    public class FollowUpDetail
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public int FukId { get; set; }
        public string Year { get; set; }
        public string Indikator { get; set; }
        public string Parameter { get; set; }
        public string FaktorUjiKesesuaian { get; set; }
        public string UnsurPemenuhan { get; set; }
        public int Sequence { get; set; }
        public float Score { get; set; }
        public string Child { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Review { get; set; }
        public string Upload { get; set; }
        public string FollowUp { get; set; }
        public string Recommendation { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class FollowUpUpload
    {
        public int UpID { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
        public string userName { get; set; }
    }
}
