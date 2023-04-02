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

        public DateTime? DateSoldDate { get; set; } = null;
        public string DateSold { get; set; }

        public List<CoreComment> Comments { get; set; }

        public CorePotentialBuy()
        {
            if (DateSoldDate.HasValue)
            {
                DateSold = DateSoldDate.Value.ToString("F");
            }
            else
            {
                DateSold = "N/A";
            }
        }
    }
}
