namespace PlanCorp.Models
{
    public class Setting
    {
        public string LDAPConnection { get; set; }
        public string Token { get; set; }
        public bool IsProduction { get; set; }
        public string baseURL_SIHP { get; set; }
        public string ActiveSSO { get; set; }
        public string PDSI_Auth_Login { get; set; }
        public string PDSI_Send_Mail { get; set; }
    }
}
