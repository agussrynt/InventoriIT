using System.ComponentModel.DataAnnotations;

namespace InventoryIT.Areas.Master.Models
{
    public class TOKLANG_PEJABATFPP
    {      
        [Key]
        public int ID { get; set; }
        public string? CostCenter { get; set; }
        public string? Jabatan { get; set; }
        public string? Nopek { get; set; }
        public string? Pejabat { get; set; }
        public string? Email { get; set; }
        public string? UsernameAD { get; set; }
    }
}
