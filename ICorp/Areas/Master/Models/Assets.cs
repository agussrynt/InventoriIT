namespace PlanCorp.Areas.Master.Models
{
    public class Assets
    {
        public int ID { get; set; }
        public string? Asset { get; set; }
        public string? Keterangan { get; set; }
        public int TipeAsset { get; set; }
        public string? CostCenter { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class TipeAssets
    {
        public int ID { get; set; }
        public string? TipeAsset { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class AssetView
    {
        public int ID { get; set; }
        public string? Asset { get; set; }
        public string? Keterangan { get; set; }
        public int IdAssetType { get; set; }
        public string? TipeAsset { get; set; }
        public string? CostCenter { get; set; }
        public string? CreatedBy { get; set; }
    }
}