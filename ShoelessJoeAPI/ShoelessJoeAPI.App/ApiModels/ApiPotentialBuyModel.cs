namespace ShoelessJoeAPI.App.ApiModels
{
    public class ApiPotentialBuyModel
    {
        public int PotentialBuyId { get; set; }

        public int ShoeImageId { get; set; }

        public string RightShoeImage1 { get; set; }

        public string RightShoeImage2 { get; set; }

        public string LeftShoeImage1 { get; set; }

        public string LeftShoeImage2 { get; set; }

        public int ShoeId { get; set; }
        
        public double? LeftSize { get; set; }
        
        public double? RightSize { get; set; }

        public bool IsSold { get; set; }

        public int ModelId { get; set; }

        public string ModelName { get; set; }

        public int ManufacterId { get; set; }

        public string ManufacterName { get; set; }

        public int BuyerId { get; set; }

        public string BuyerFirstName { get; set; }

        public string BuyerLastName { get; set; }

        public int OwnerId { get; set; }

        public string OwnerFirstName { get; set; }

        public string OwnerLastName { get; set; }

        public bool OwnsShoe { get; set; }

        public List<ApiCommentModel> Comments { get; set; }

        public void UserOwnsShoe(int userId)
        {
            OwnsShoe = OwnerId == userId;
        }
    }
}
