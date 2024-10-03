using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PlanCorp_API.Models
{
    public class DashboardModel
    {

    }

    public class JumlahTemuanChartModel
    {
        [JsonPropertyName("Tahun")]
        public int Tahun { get; set; }

        [JsonPropertyName("Jumlah_Temuan")]
        public int JumlahTemuan { get; set; }
    }

    public class FollowUpComparerItem
    {
        [JsonPropertyName("Non_FollowUp")]
        public int NonFollowUp { get; set; }

        [JsonPropertyName("FollowUp")]
        public int FollowUp { get; set; }

        [JsonPropertyName("Tahun")]
        public int Tahun { get; set; }
    }

    public class FollowUpComparer
    {
        public int Year { get; set; }
        public FollowUpComparerItem Data { get; set; }
        public FollowUpComparer()
        {
            Data = new FollowUpComparerItem(); 
        }
    }
}
