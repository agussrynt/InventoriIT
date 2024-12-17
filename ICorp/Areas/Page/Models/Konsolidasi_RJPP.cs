namespace InventoryIT.Areas.Page.Models
{
    public class Class
    {
        public int ID { get; set; }
        public int Tahun { get; set; }
        public int Jml_PUsaha_Rev_RJPP {  get; set; }
        public int Jml_BUsaha_HPP_RJPP { get; set; }
        public int Jml_Beban_Umum {  get; set; }
        public string? created_by { get; set; }
        public DateTime created_time { get; set; }

    }
}
