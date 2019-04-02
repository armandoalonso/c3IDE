using c3IDE.Utilities;

namespace c3IDE.DataAccess
{
    public class DataAccessFacade : Singleton<DataAccessFacade>
    {
        public AddonRepository AddonData;
        public OptionRepository OptionData;

        public DataAccessFacade()
        {
            AddonData = new AddonRepository();
            OptionData = new OptionRepository();
        }
    }
}
