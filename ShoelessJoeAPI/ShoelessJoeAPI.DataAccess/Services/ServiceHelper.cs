namespace ShoelessJoeAPI.DataAccess.Services
{
    public class ServiceHelper
    {
        protected int _index;

        public void ConfigureIndex(int? index)
        {
            if (index is null || (int)index == 1)
            {
                _index = 0;
            }
            else
            {
                _index = (int)index;
            }
        }
    }
}
