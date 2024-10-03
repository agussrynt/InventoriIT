using Dapper;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Data;

namespace PlanCorp.Areas.Page.Services
{
    public class MasterKertasKerjaService : IMasterKertasKerjaService
    {
        private readonly ConnectionDB _connectionDB;

        public MasterKertasKerjaService(ConnectionDB connectionDB)
        {
            _connectionDB = connectionDB;
        }
        public ResponseJson StoreMasterKertasKerja(StoreModel storeModel)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                // Store year master
                var storeMasterYear = this.StoreMasterYear(storeModel.Year, storeModel.UserName);
                if (storeMasterYear.Success)
                {
                    bool isSuccess = true;
                    int yearId = Int32.Parse(storeMasterYear.Message);
                    string userName = storeModel.UserName;
                    // Store each header parameter for indicator
                    foreach (var header in storeModel.ParameterHeader)
                    {
                        if (!isSuccess) break;
                        var storeMasterIndikator = this.StoreMasterIndicator(header, yearId, userName);
                        if (!storeMasterIndikator.Success)
                        {
                            isSuccess = false;
                            responseJson.Message = "Failed store to master indikator!";
                            break;
                        }
                        int indikatorId = Int32.Parse(storeMasterIndikator.Message);
                        // Store each content parameter for parameter
                        var contentList = this.FilterContent(storeModel.ParameterContent, header.Id);
                        foreach (var content in contentList)
                        {
                            var storeMasterParameter = this.StoreMasterParameter(content, indikatorId, userName);
                            if (!storeMasterParameter.Success)
                            {
                                isSuccess = false;
                                responseJson.Message = "Failed store to master parameter!";
                                break;
                            }
                            int parameterId = Int32.Parse(storeMasterParameter.Message);
                            // Store each fuk detail
                            var fukList = this.FilterFUK(storeModel.ParameterFUK, content.Id);
                            foreach (var fuk in fukList)
                            {
                                var storeMasterFUK = this.StoreMasterFUK(fuk, parameterId, userName);
                                if (!storeMasterFUK.Success)
                                {
                                    isSuccess = false;
                                    responseJson.Message = "Failed store to master fuk!";
                                    break;
                                }
                                int fukId = Int32.Parse(storeMasterFUK.Message);
                                // Store each UP detail
                                var upList = this.FilterUP(storeModel.ParameterUP, fuk.Id);
                                foreach (var up in upList)
                                {
                                    var storeMasterUP = this.StoreMasterUP(up, fukId, userName);
                                    if (!storeMasterFUK.Success)
                                    {
                                        isSuccess = false;
                                        responseJson.Message = "Failed store to master up!";
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    responseJson.Success = isSuccess;
                    if(isSuccess) responseJson.Message = "Successfully create template!";
                    //responseJson.Message = isSuccess ? "Successfully create template!" : "Failed create template!";
                } 
                else
                {
                    responseJson = storeMasterYear;
                }

                return responseJson;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = "0";

                return responseJson;
            }
        }

        private ParameterContent[] FilterContent(ParameterContent[] parameterContents, int headerId)
        {
            return parameterContents.Where(el => el.HeaderId == headerId).ToArray();
        }

        private ParameterFUK[] FilterFUK(ParameterFUK[] parameterFUK, int contentId)
        {
            return parameterFUK.Where(el => el.ContentId == contentId).ToArray();
        }

        private ParameterUP[] FilterUP(ParameterUP[] parameterUP, int fukId)
        {
            return parameterUP.Where(el => el.FUKId == fukId).ToArray();
        }

        private ResponseJson StoreMasterYear(string year, string userName)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Master_Year_MKK", new
                    {
                        year = year,
                        username = userName
                    }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return responseJson;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = ex.Message;

                return responseJson;
            }
        }

        private ResponseJson StoreMasterIndicator(ParameterHeader parameterHeader, int yearId, string userName)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Master_Header_Indicator", new
                    {
                        yearId = yearId,
                        seq = parameterHeader.Sequence,
                        title = parameterHeader.Aspek,
                        username = userName
                    }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return responseJson;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = ex.Message;

                return responseJson;
            }
        }

        private List<ParameterHeader> GetMasterIndicator(int year)
        {
            List<ParameterHeader> result = new List<ParameterHeader>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                result = (List<ParameterHeader>)conn.Query<ParameterHeader>("usp_Get_Indicator_By_Year", new
                {
                    Year = year,
                }, commandType: CommandType.StoredProcedure);
                conn.Close();

                return result;
            }
        }

        private ResponseJson StoreMasterParameter(ParameterContent parameterContent, int indicatorId, string userName)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Master_Parameter", new
                    {
                        indicatorId = indicatorId,
                        seq = parameterContent.Sequence,
                        desc = parameterContent.Description,
                        bobot = parameterContent.Bobot,
                        username = userName
                    }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return responseJson;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = ex.Message;

                return responseJson;
            }
        }

        private List<ParameterContent> GetMasterParameter(int idIndicator)
        {
            List<ParameterContent> result = new List<ParameterContent>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                result = (List<ParameterContent>)conn.Query<ParameterContent>("usp_Get_Parameter_By_Indicator", new
                {
                    IdIndicator = idIndicator,
                }, commandType: CommandType.StoredProcedure);
                conn.Close();

                return result;
            }
        }

        private ResponseJson StoreMasterFUK(ParameterFUK parameterFUK, int parameterId, string userName)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Master_FUK", new
                    {
                        parameterId = parameterId,
                        seq = parameterFUK.Sequence,
                        parent = parameterFUK.Parent,
                        child = parameterFUK.Child,
                        desc = parameterFUK.Description,
                        username = userName
                    }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return responseJson;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = ex.Message;

                return responseJson;
            }
        }

        private List<ParameterFUK> GetMasterFUK(int parameterId)
        {
            List<ParameterFUK> result = new List<ParameterFUK>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                result = (List<ParameterFUK>)conn.Query<ParameterFUK>("usp_Get_Master_FUK", new
                {
                    IdParameter = parameterId,
                }, commandType: CommandType.StoredProcedure);
                conn.Close();

                return result;
            }
        }

        private ResponseJson StoreMasterUP(ParameterUP parameterUP, int fukId, string userName)
        {
            ResponseJson responseJson = new ResponseJson();
            try
            {
                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    responseJson = conn.QueryFirst<ResponseJson>("usp_Post_Master_UP", new
                    {
                        fukId = fukId,
                        seq = parameterUP.Sequence,
                        desc = parameterUP.Description,
                        username = userName
                    }, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return responseJson;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = ex.Message;

                return responseJson;
            }
        }

        private List<ParameterUP> GetMasterUnsurPemenuhan(int fukId)
        {
            List<ParameterUP> result = new List<ParameterUP>();
            using (IDbConnection conn = _connectionDB.Connection)
            {
                conn.Open();
                result = (List<ParameterUP>)conn.Query<ParameterUP>("usp_Get_Master_UnsurPemenuhan", new
                {
                    IdFuk = fukId,
                }, commandType: CommandType.StoredProcedure);
                conn.Close();

                return result;
            }
        }

        public BaseResponseJson GetDataMasterKertasKerja(int year)
        {
            BaseResponseJson responseJson = new BaseResponseJson();
            MasterAuditViewModel result = new MasterAuditViewModel();
            try
            {
                result.ParameterHeader = GetMasterIndicator(year);
                if (result.ParameterHeader.Count > 0)
                {
                    foreach (var itmInd in result.ParameterHeader)
                    {
                        List<ParameterContent> parameterContents = GetMasterParameter(itmInd.Id);
                        if (parameterContents.Count > 0)
                        {
                            parameterContents.ForEach(x => x.Aspek = itmInd.Aspek);
                            result.ParameterContent.AddRange(parameterContents);
                            foreach (var parameterContent in parameterContents)
                            {
                                List<ParameterFUK> parameterFUKs = GetMasterFUK(parameterContent.Id);
                                if (parameterFUKs.Count > 0)
                                {
                                    parameterFUKs.ForEach(x => x.ContentDescription = parameterContent.Description);
                                    result.ParameterFUK.AddRange(parameterFUKs);
                                    foreach (var parameterFUK in parameterFUKs)
                                    {
                                        List<ParameterUP> parameterUPs = GetMasterUnsurPemenuhan(parameterFUK.Id);
                                        if (parameterUPs.Count > 0)
                                        {
                                            parameterUPs.ForEach(x => x.FUKDescription = parameterFUK.Description);
                                            result.ParameterUP.AddRange(parameterUPs);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                responseJson.Success = true;
                responseJson.Data = result;

                return responseJson;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseJson.Success = false;
                responseJson.Message = "0";

                return responseJson;
            }
        }

        public List<Dropdown> GetDropdown(int paramsId, int option)
        {
            List<Dropdown> dropdownList = new List<Dropdown>();
            try
            {
                string query = "usp_Get_List_Dropdown";
                var parameters = new DynamicParameters();
                parameters.Add("@paramsId", paramsId);
                parameters.Add("@option", option);

                using (IDbConnection conn = _connectionDB.Connection)
                {
                    conn.Open();
                    dropdownList = (List<Dropdown>)conn.Query<Dropdown>(query, parameters, commandType: CommandType.StoredProcedure);
                    conn.Close();

                    return dropdownList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return dropdownList;
            }
        }
    }
}
