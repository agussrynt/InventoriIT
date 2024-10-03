namespace PlanCorp.Areas.Master.Models
{
    public class Response_UserFPP
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public REST_UserFPP Data { get; set; }

    }
    public class REST_UserFPP
    {
        public bool value { get; set; }
        public string EmpNumber { get; set; }
        public string NamaLengkap { get; set; }
        public string Email { get; set; }
        public string PosID { get; set; }
        public string PosText { get; set; }
        public string DirID { get; set; }
        public string DirText { get; set; }
        public string DivID { get; set; }
        public string DivText { get; set; }
        public string DepID { get; set; }
        public string DepText { get; set; }
        public bool IsMitra { get; set; }
        public bool IsPDSI { get; set; }
    }
}
