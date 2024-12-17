namespace InventoryIT.Areas.Page.Models
{
    public class BebanUmum_Administrasi_RJPP
    {
        public int ID { get; set; }
        public int Tahun {  get; set; }
        public int PDSI_BebanUmum { get; set; }
        public int PDC_BebanUmum { get; set; }
        public int MA_BebanUmum { get; set; }
        public int Total_BebanUmum { get; set; }
        public string? created_by { get; set; }
        public DateTime? created_time { get; set; }
    }
}
