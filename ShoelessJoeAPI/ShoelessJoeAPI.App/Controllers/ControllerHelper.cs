using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoelessJoeAPI.App.ApiModels;
using ShoelessJoeAPI.App.FileIO;
using System.IdentityModel.Tokens.Jwt;

namespace ShoelessJoeAPI.App.Controllers
{
    public class ControllerHelper : ControllerBase
    {
        protected string ErrorMessage { get; } = "An error has occured";

        protected string UnAuthMessage { get; } = "You do not have access to this resource";

        protected int UserId { get; set; }

        protected bool IsAdmin { get; set; }

        protected JwtSecurityToken _token;

        protected readonly string _location;


        public ControllerHelper(string location)
        {
            _location = location;
        }

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

        protected ActionResult InternalError(Exception exception)
        {
            FileWriter.WriteError(exception, _location);

            return StatusCode(500, ErrorMessage);
        }

        protected bool UserIdDoesMatch(int userId)
        {
            return UserId == userId;
        }

        protected List<string> DisplaysModelStateErrors()
        {
            var errorList = new List<string>();

            var errors = ModelState.Select(e => e.Value.Errors)
                .Where(w => w.Count > 0)
                .ToList();

            for (int i = 0; i < errors.Count; i++)
            {
                var errorCollection = errors[i];

                for (int a = 0; a < errorCollection.Count; a++)
                {
                    errorList.Add(errorCollection[a].ErrorMessage);
                }
            }

            return errorList;
        }
    }
}
