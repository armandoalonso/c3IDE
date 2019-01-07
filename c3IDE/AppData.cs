using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;

namespace c3IDE
{
    public class AppData : Singleton<AppData>
    {
        public List<C3Addon> AddonList = new List<C3Addon>();
        public C3Addon CurrentAddon = new C3Addon();
    }
}
