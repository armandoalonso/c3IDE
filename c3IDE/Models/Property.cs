using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Models
{
    public class Property
    {
        public Guid Key { get; set; } = Guid.NewGuid();
        public string Type { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool HasMinMax { get; set; }
        public bool HasDragSpeed { get; set; }
        public bool ForEachInstance { get; set;}

        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public string DragSpeedValue { get; set; }
        public List<string> Items { get; set; }
    }
}
