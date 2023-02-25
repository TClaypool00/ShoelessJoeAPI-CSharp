using ShoelessJoeAPI.App.ApiModels.PostModels;

namespace ShoelessJoeAPI.App.ApiModels
{
    public class ApiManufacter : PostManufacter
    {
        public int ManufacterId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
