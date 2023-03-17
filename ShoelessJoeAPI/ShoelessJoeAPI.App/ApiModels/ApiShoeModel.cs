using ShoelessJoeAPI.App.ApiModels.PostModels;

namespace ShoelessJoeAPI.App.ApiModels
{
    public class ApiShoeModel : PostShoeModel
    {
        public int ShoeId { get; set; }

        public bool IsSold { get; set; }

        public string ModelName { get; set; }

        public int ManufacterId { get; set; }
        
        public string MaufacterName { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? SoldToUserId { get; set; }

        public string SoldToFirstName { get; set; }

        public string SoldToLastName { get; set; }
    }
}
