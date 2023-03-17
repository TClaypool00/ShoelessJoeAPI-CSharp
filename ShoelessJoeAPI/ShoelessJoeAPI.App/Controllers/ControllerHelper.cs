using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RTools_NTS.Util;
using System.IdentityModel.Tokens.Jwt;

namespace ShoelessJoeAPI.App.Controllers
{
    public class ControllerHelper : ControllerBase
    {
        protected string ErrorMessage { get; } = "An error has corrected";

        protected string UnAuthMessage { get; } = "You do not have access to this resource";

        protected int UserId { get; set; }

        protected bool IsAdmin { get; set; }

        protected JwtSecurityToken _token;


        protected void ExtractToken()
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                string authHeader = Request.Headers["Authorization"];
                authHeader = authHeader.Replace("Bearer ", "");

                _token = handler.ReadToken(authHeader) as JwtSecurityToken;

                if (_token is not null)
                {
                    UserId = int.Parse(_token.Claims.First(c => c.Type == "UserId").Value);

                    IsAdmin = bool.Parse(_token.Claims.First(c => c.Type == "IsAdmin").Value);
                }

            } catch (NullReferenceException)
            {
                _token = null;
            }
        }
    }
}
