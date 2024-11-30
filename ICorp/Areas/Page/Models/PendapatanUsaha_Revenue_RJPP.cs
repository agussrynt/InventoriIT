namespace PlanCorp.Areas.Page.Models
{
	public class PendapatanUsaha_Revenue_RJPP
	{
        public int ID { get; set; }
        public int id_Revenue_RJPP { get; set; }
        public int PDSI_PendapatanUsaha { get; set; }
        public int PDC_PendapatanUsaha { get; set; }
        public int PDC_Eliminasi { get; set; }
        public int MA_PendapatanUsaha { get; set; }
        public int MA_Eliminasi { get; set; }
        public int Tahun {  get; set; }
        public int Total_PendapatanUsaha { get; set; }
        public string? created_by { get; set; }
        public DateTime? created_time { get; set; }
        public string? updated_by { get; set; }
        public DateTime? updated_time { get; set; }

    }
}
