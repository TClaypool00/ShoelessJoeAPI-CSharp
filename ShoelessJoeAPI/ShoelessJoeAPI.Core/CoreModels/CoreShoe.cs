namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CoreShoe
    {
        public int ShoeId { get; set; }

        public double? RightSize { get; set; }

        public double? LeftSize { get; set; }

        public DateTime DatePosted { get; set; }

        public int ModelId { get; set; }
        public CoreModel Model { get; set; }

        public CoreShoeImage ShoeImage { get; set; }

        public List<CorePotentialBuy> PotentialBuys { get; set; }
    }
}
