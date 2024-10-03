namespace PlanCorp.Models
{
    public class ParameterData
    {
        public int ParamsId { get; set; }
    }

    public class UploadFile
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
    }
}
