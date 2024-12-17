namespace InventoryIT.Areas.Page.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }

        public float ScoreParameter { get; set; }
        public float ScoreFuk { get; set; }
        public float ScoreUp { get; set; }
    }
    public class AuditDetail
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
        public int Parent { get; set; }
        public string Child { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Review { get; set; }
        public string Upload { get; set; }
        public string File { get; set; }
        public string UpStatus { get; set; }
        public int Audit { get; set; }
    }

    public class AuditReview
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string AuditBy { get; set; }
        public float ScoreParameter { get; set; }
        public float ScoreFuk { get; set; }
        public float ScoreUp { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
