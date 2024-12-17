namespace InventoryIT.Areas.Master.Models
{
    public class CostCenter
    {
        public int Client { get; set; }
        public string? FinancialManagementArea { get; set; }
        public string? FundsCenter { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CompanyCode { get; set; }
        public DateTime EXEC_DATE { get; set; }
    }

    public class CostCenterDD
    {
        public int Client { get; set; }
        public string? FundsCenter { get; set; }
        public string? Name { get; set; }
    }
}