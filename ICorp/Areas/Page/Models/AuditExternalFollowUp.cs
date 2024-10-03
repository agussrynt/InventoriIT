using Newtonsoft.Json;

namespace PlanCorp.Areas.Page.Models
{
    public class AuditExternalFollowUp
    {
        public long ID_ASSIGMENT_RECOMENDATION { get; set; }
        public string AUDITEE_FOLLOWUP { get; set; }
        public string ATTACHMENT { get; set; }
        public string FILENAME { get; set; }
        public string CREATEDBY { get; set; }
        public string REMARK { get; set; }
    }
    public class AuditExternalFollowUpList : AuditExternalFollowUp
    {
        [JsonProperty("ID_ASSIGMENT_RECOMENDATION")]
        public long ID_ASSIGMENT_RECOMENDATION { get; set; }

        [JsonProperty("ID_AUDIT_EXTERNAL_DATA_RECOMENDATION")]
        public long ID_AUDIT_EXTERNAL_DATA_RECOMENDATION { get; set; }

        [JsonProperty("DUE_DATE")]
        public DateTime DUE_DATE { get; set; }

        [JsonProperty("USERID")]
        public string USERID { get; set; }

        [JsonProperty("FUNCTION_STR")]
        public string FUNCTION_STR { get; set; }

        [JsonProperty("PIC_STR")]
        public string PIC_STR { get; set; }

        [JsonProperty("REKOMENDASI")]
        public string REKOMENDASI { get; set; }

        [JsonProperty("ASPEK")]
        public string ASPEK { get; set; }

        [JsonProperty("ID_STATUS")]
        public string ID_STATUS { get; set; }

        [JsonProperty("STATUS_NAME")]
        public string STATUS_NAME { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("ID_AUDIT_EXTERNAL")]
        public long ID_AUDIT_EXTERNAL { get; set; }

        [JsonProperty("TOTAL_SCORE")]
        public decimal TOTAL_SCORE { get; set; }

        [JsonProperty("DATE")]
        public DateTime DATE { get; set; }

        public string AUDITOR_NAME { get; set; }

        
    }
}
