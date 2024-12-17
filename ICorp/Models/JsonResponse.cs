using Newtonsoft.Json;

namespace InventoryIT.Models
{
    public class JsonResponse
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public ResponseLDAP Data { get; set; }
    }

    public class Dropdown
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ResponseLDAP
    {
        public bool value { get; set; }
        public string EmpNumber { get; set; }
        public string NamaLengkap { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public bool IsMitra { get; set; }
        public bool IsPDSI { get; set; }
        public string UserName { get; set; }
    }

    public class LoginAPIResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public ResponseLDAP data { get; set; }
        public LoginAPIResponse()
        {
            status = "F";
            message = "Invalid Login";
            data.value = false;
        }
    }

    public class LoginAPIData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RememberMe { get; set; }
        public string method { get; set; }
    }

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
