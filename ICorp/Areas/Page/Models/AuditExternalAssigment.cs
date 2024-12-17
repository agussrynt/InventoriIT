namespace InventoryIT.Areas.Page.Models
{
    public class AuditExternalAssigmentRecomendation
    {
        public decimal ID_ASSIGMENT_RECOMENDATION { get; set; }
        public decimal ID_AUDIT_EXTERNAL_DATA_RECOMENDATION { get; set; }
        public DateTime? DUE_DATE { get; set; }
        public string CREATEDBY { get; set; }
        public string FUNCTION_STR { get; set; }
        public string PIC_STR { get; set; }
        public Guid USERID { get; set; }
        public List<AuditExternalAssigmentRecomendationFunctionPic> PIC { get; set; }
        public AuditExternalAssigmentRecomendation()
        {
            PIC = new List<AuditExternalAssigmentRecomendationFunctionPic>();
        }
    }

    public class AuditExternalAssigmentRecomendationFunctionPic
    {
        public long ID_AUDIT_EXTERNAL_DATA_RECOMENDATION_ASSIGMENT_FUNCTION_PIC { get; set; }
        public long ID_AUDIT_EXTERNAL_DATA_RECOMENDATION_ASSIGMENT_FUNCTION { get; set; }
        public Guid USERID { get; set; }
        public string FULL_NAME { get; set; }
        public string USERNAME { get; set; }
        public string ID_FUNGSI { get; set; }
        public string FUNGSINAME { get; set; }
    }

    public class AuditExternalAssigmentRecomendationList : AuditExternalAssigmentRecomendation
    {
        public string REKOMENDASI { get; set; }
        public string AUDITOR_NAME { get; set; }
        public string  STATUS { get; set; }
        public long ID_AUDIT_EXTERNAL { get; set; }
        public string ID_FUNGSI { get; set; }
        public string USERID_STR { get; set; }
    }

}
