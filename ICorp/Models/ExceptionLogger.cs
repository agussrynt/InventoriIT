namespace InventoryIT.Models
{
    public class ExceptionLogger
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ControllerName { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionURL { get; set; }
        public string ExceptionStackTrace { get; set; }
        public DateTime LogTime { get; set; } = DateTime.Now;
    }
}
