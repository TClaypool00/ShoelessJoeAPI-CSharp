using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.DataAccess.Services
{
    public class ServiceHelper
    {
        protected int _index;

        protected string _idErrorMessage = "Id cannot be 0";

        protected void ConfigureIndex(int? index)
        {
            if (index is null || (int)index == 1)
            {
                _index = 0;
            }
            else
            {
                _index = (int)index *10;
            }
        }
    }
}
