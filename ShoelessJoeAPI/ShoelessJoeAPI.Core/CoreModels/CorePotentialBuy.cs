namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CorePotentialBuy
    {
        public int PotentialBuyId { get; set; }

        public int ShoeId { get; set; }
        public CoreShoe Shoe { get; set; }


        public int PotentialBuyerUserId { get; set; }
        public CoreUser PotentialBuyer { get; set; }

        public bool IsSold { get; set; }

        public DateTime? DateSold { get; set; } = null;

        public List<CoreComment> Comments { get; set; }
    }
}
