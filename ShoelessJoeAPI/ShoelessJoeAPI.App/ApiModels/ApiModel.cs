using ShoelessJoeAPI.App.ApiModels.PostModels;

namespace ShoelessJoeAPI.App.ApiModels
{
    public class ApiModel : PostModel
    {
        public int ModelId { get; set; }
        public string ManufacterName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
