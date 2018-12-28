using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities;

namespace c3IDE.DaataAccess
{
    public class DataAccessFacade : Singleton<DataAccessFacade>
    {
        public OptionsRepository Options = new OptionsRepository();
    }
}
