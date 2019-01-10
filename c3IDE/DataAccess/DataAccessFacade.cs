using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities;

namespace c3IDE.DataAccess
{
    public class DataAccessFacade : Singleton<DataAccessFacade>
    {
        public AddonRepository AddonData = new AddonRepository();
        public OptionRepository OptionData = new OptionRepository();
    }
}
