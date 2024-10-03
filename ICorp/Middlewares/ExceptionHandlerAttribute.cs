using PlanCorp.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using PlanCorp.Models;
using Dapper;
using System.Data;

namespace PlanCorp.Middlewares
{
    public class ExceptionHandlerAttribute : IExceptionFilter
    {
        private readonly PlanCorpDbContext _context;
        private readonly ConnectionDB _connectionDB;

        public ExceptionHandlerAttribute(PlanCorpDbContext context, ConnectionDB connectionDB)
        {
            _context = context;
            _connectionDB = connectionDB;
        }

        public void OnException(ExceptionContext context)
        {
            var userId = "";
            var path = context.HttpContext.Request.Path.ToString();
            if (!context.ExceptionHandled)
            {
                ExceptionLogger logger = new ExceptionLogger()
                {
                    UserId = userId,
                    ControllerName = context.RouteData.Values["controller"].ToString(),
                    ExceptionMessage = context.Exception.Message,
                    ExceptionType = context.GetType().Name.ToString(),
                    ExceptionURL = path,
                    ExceptionStackTrace = context.Exception.StackTrace,
                    LogTime = DateTime.Now
                };

                //_context.ExceptionLoggers.Add(logger);
                //_context.SaveChanges();

                var parameters = new DynamicParameters();
                //parameters.Add("@paramsId", paramsId);

                //using (IDbConnection conn = _connectionDB.Connection)
                //{
                //    conn.Open();
                //    picList = (List<PIC>)conn.Query<PIC>("usp_Get_User_PIC_DropDown", parameters, commandType: CommandType.StoredProcedure);
                //    conn.Close();

                //    return picList;
                //}

                context.ExceptionHandled = true;
            }
        }


    }
}

