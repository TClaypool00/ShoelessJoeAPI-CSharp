namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CoreModel
    {
        public int ModelId { get; set; }

        public string ModelName { get; set; }

        public int ManufacterId { get; set; }
        public CoreManufacter Manufacter { get; set; }

        public List<CoreShoe> Shoes { get; set; } = new List<CoreShoe>();
    }
}
