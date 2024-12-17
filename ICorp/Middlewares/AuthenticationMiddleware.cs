using Dapper;
using InventoryIT.Areas.Master.Models;
using InventoryIT.Data;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace InventoryIT.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext httpContext,
            ConnectionDB connectionDB,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IAntiforgery antiforgery)
        {
            var path = httpContext.Request.Path;
            if (path.HasValue && path.Value.StartsWith("/account") == false)
            {
                if (httpContext.Session.GetString("userId") == null)
                {
                    if (signInManager.IsSignedIn(httpContext.User))
                    {
                        this.getUserLogin(httpContext, connectionDB, httpContext.User.Identity.Name);
                    }
                }
            }

            await _next(httpContext);
        }

        private void getUserLogin(HttpContext httpContext, ConnectionDB _connectionDB, string userName)
        {
            UserDetail userDetail = new UserDetail();
            try
            {
                if (userName != null)
                {
                    using (IDbConnection conn = _connectionDB.Connection)
                    {
                        conn.Open();
                        userDetail = conn.QueryFirst<UserDetail>("usp_Get_User_Detail", new { userName = userName }, commandType: CommandType.StoredProcedure);
                        conn.Close();

                    }

                    if (!String.IsNullOrEmpty(userDetail.UserId))
                    {
                        httpContext.Session.SetString("userId", userDetail.UserId);
                        httpContext.Session.SetString("email", userDetail.Email);
                        httpContext.Session.SetString("fullname", userDetail.FullName);
                        httpContext.Session.SetString("username", userDetail.UserName);
                        httpContext.Session.SetString("role", userDetail.Role);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Console.Write(ex.Message);
                throw;
            }
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddlewareCustom(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
