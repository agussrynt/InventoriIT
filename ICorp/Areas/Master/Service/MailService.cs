using Dapper;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Data;
using PlanCorp.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;

namespace PlanCorp.Areas.Master.Service
{
    public class MailService : IMailService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IOptions<Setting> _setting;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly ConnectionDB _connectionDB;
        public MailService(IOptions<Setting> setting, IHttpContextAccessor httpContextAccessor, ConnectionDB connectionDB)
        {
            _setting = setting;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
            _connectionDB = connectionDB;
        }
        public async Task<MailResultModel> SendMail(MailModel mail)
        {
            MailResultModel apiresponse = new MailResultModel();
            LoginAPIData dataLogin = new LoginAPIData();

            var data = new[]
            {
                new KeyValuePair<string, string>("recipient", mail.recipient),
                new KeyValuePair<string, string>("subject", mail.subject),
                new KeyValuePair<string, string>("body", mail.body)
            };

            try
            {
                if (_setting.Value.IsProduction)
                {
                    string token = _session.GetString("tokenapi");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Token", token);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var content = new FormUrlEncodedContent(data);
                    using (var response = await client.PostAsync(_setting.Value.PDSI_Send_Mail, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        // If hit API Login is success
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            apiresponse = JsonConvert.DeserializeObject<MailResultModel>(apiResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                apiresponse.Message = ex.Message;
            }
            return apiresponse;
        }

        public List<MailAccModel> GetEmailAcc(string paramsId)
        {
            List<MailAccModel> picList = new List<MailAccModel>();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", paramsId);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    picList = (List<MailAccModel>)conn.Query<MailAccModel>("usp_Get_EmailUser", parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return picList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return picList;
            }
        }
    }
}
