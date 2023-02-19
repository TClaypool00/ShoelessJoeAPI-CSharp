using Microsoft.AspNetCore.Mvc;

namespace ShoelessJoeAPI.App.Controllers
{
    public class ControllerHelper : ControllerBase
    {
        protected string ErrorMessage { get; } = "An error has corrected";
    }
}
