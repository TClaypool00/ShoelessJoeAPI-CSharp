using ShoelessJoeAPI.Core.CoreModels.PartialModels;

namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CoreModel : CoreModelDropDown
    {
        public int ManufacterId { get; set; }
        public CoreManufacter Manufacter { get; set; }

        public List<CoreShoe> Shoes { get; set; } = new List<CoreShoe>();
    }
}
