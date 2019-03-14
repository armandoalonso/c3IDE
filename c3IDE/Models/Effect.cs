using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Models
{
    public class Effect
    {
        public bool BlendsBackground { get; set; } = false;
        public bool CrossSampling { get; set; } = false;
        public bool PreservesOpaqueness { get; set; } = false;
        public bool Animated { get; set; } = false;
        public bool MustPredraw { get; set; } = false;
        public int ExtendBoxHorizontal { get; set; } = 0;
        public int ExtendBoxVertical { get; set; } = 0;
    }
}
