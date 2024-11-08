namespace PlanCorp.Areas.Master.Models
{
	public class Projects
	{
		public int ID { get; set; }
		public string? NamaProject { get; set; }
		public int Segmen { get; set; }
		public int Asset { get; set; }
		public int Customer { get; set; }
		public int Contract { get; set; }
		public string? Probability { get; set; }
		public string? Sumur { get; set; }
		public string? ControlProject { get; set; }
		public int Pekerjaan { get; set; }
		public int SBT { get; set; }
		public DateTime CreatedTime { get; set; }
		public string? CreatedBy { get; set; }
	}

	public class TipeContractsDD
	{
		public int ID { get; set; }
		public string? Contract { get; set; }
	}

	public class SegmenDD
	{
		public int ID { get; set; }
		public string? Segmen { get; set; }
	}

	public class PekerjaanDD
	{
		public int ID { get; set; }
		public string? Pekerjaan { get; set; }
	}

	public class AssetDD
	{
		public int ID { get; set; }
		public string? Asset { get; set; }
		public string? CostCenter { get; set; }
	}

	public class CustomerDD
	{
		public int ID { get; set; }
		public string? Customer { get; set; }
		public string? Regional { get; set; }
	}

	public class SBTDD
	{
		public int ID { get; set; }
		public string? SbtIndex { get; set; }
		public string? Level1 { get; set; }
		public string? Level2 { get; set; }
		public string? Level3 { get; set; }
		public string? Penjelasan { get; set; }
	}

	public class ProjectsView
	{
		public int ID { get; set; }
		public string? NamaProject { get; set; }
		public string? Segmen { get; set; }
		public int IDSegmen { get; set; }
		public string? Asset { get; set; }
		public int IDAsset { get; set; }
		public string? CostCenter { get; set; }
		public string? Customer { get; set; }
		public int IDCustomer { get; set; }
		public string? Regional { get; set; }
		public string? Contract { get; set; }
		public int IDContract { get; set; }
		public string? Probability { get; set; }
		public string? Sumur { get; set; }
		public string? ControlProject { get; set; }
		public int IDPekerjaan { get; set; }
		public string? Pekerjaan { get; set; }
		public int IDSBT { get; set; }
		public string? SbtIndex { get; set; }
	}
}