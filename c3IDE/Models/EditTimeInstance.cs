using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Models
{
    public class EditTimeInstance
    {
        public string Constructor { get; set; } = @"constructor(sdkType, inst)
          {
               super(sdkType, inst);
          }";

        public string Release { get; set; } = @"Release()
          {
          }";

        public string OnCreate { get; set; } = @"OnCreate()
          {
          }";

        public string OnPropertyChanged { get; set; } = @"OnPropertyChanged(id, value)
          {
          }";

        public string LoadC2Property { get; set; } = @"LoadC2Property(name, valueString)
          {
                return false;     // not handled
          }";

        public List<string> Functions { get; set; } = new List<string>();
    }
}
