using c3IDE.DataAccess;
using c3IDE.Utilities;

namespace c3IDE.DataAccess
{
    public class DataAccessFacade : Singleton<DataAccessFacade>
    {
        public OptionsRepository Options = new OptionsRepository();
    }
}
