namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CoreShoe
    {
        public int ShoeId { get; set; }

        public double? RightSize { get; set; }

        public double? LeftSize { get; set; }

        public DateTime DatePosted { get; set; }

        public bool IsSold { get; set; } = false;

        public int ModelId { get; set; }
        public CoreModel Model { get; set; }

        public int? SoldToId { get; set; } = null;
        public CoreUser SoldToUser { get; set; }
    }
}
