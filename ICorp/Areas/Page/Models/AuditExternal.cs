namespace InventoryIT.Areas.Page.Models
{
    public class AuditExternalList
    {
        public int ID_AUDIT_EXTERNAL { get; set; }
        public string AUDITOR_NAME { get; set; }
        public decimal TOTAL_SCORE { get; set; }
        public int RECOMENDATION_UPLOAD { get; set; }
        public string ATTACHMENT { get; set; }
        public string ATTACHMENT_NAME { get; set; }
        public DateTime DATE { get; set; }
    }

    public class AuditExternal
    {
        public int ID_AUDIT_EXTERNAL { get; set; }
        public string AUDITOR_NAME { get; set; }
        public DateTime DATE { get; set; }
        public string ATTACHMENT { get; set; }
        public string ATTACHMENT_NAME { get; set; }
        public string ATTACHMENT_DATA_SCORE { get; set; }
        public string ATTACHMENT_RECOMENDATION { get; set; }
        public string CREATEDBY { get; set; }
        public DateTime CREATEDON { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime MODIFIEDON { get; set; }
        public string ATTACHMENT_DATA_SCORE_NAME { get; set; }
        public string ATTACHMENT_RECOMENDATION_NAME { get; set; }
        public List<AuditExternalDataScore> dataScores { get; set; }

        public List<AuditExternalDataRecomendation> dataRecomendations { get; set; }

        public AuditExternal()
        {
            dataScores = new List<AuditExternalDataScore>();
            dataRecomendations = new List<AuditExternalDataRecomendation>();
        }
    }

    public class AuditExternalDataScore
    {
        public int ID_AUDIT_EXTERNAL_DATA_SCORE { get; set; }
        public int ID_AUDIT_EXTERNAL { get; set; }
        public string INDIKATOR { get; set; }
        public decimal BOBOT { get; set; }
        public int  JUMLAH_PARAMATER { get; set; }
        public decimal SCORE { get; set; }
        public int CAPAIAN { get; set; }
        public string CREATEDBY { get; set; }
        public DateTime CREATEDON { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime MODIFIEDON { get; set; }

    }

    public class AuditExternalDataRecomendation
    {
        public int ID_AUDIT_EXTERNAL_DATA_RECOMENDATION { get; set; }
        public int ID_AUDIT_EXTERNAL { get; set; }
        public string REKOMENDASI { get; set; }
        public string ASPEK { get; set; }
        public string CREATEDBY { get; set; }
        public DateTime CREATEDON { get; set; }
        public string MODIFIEDBY { get; set; }
        public DateTime MODIFIEDON { get; set; }

    }
}
