using Newtonsoft.Json;

namespace FakeAPI_PDSI.Model
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            IsSuccess = false;
            Message = "";
        }
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }
}
