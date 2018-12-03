using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.PluginModels
{
    public class Property : IEquatable<Property>
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }
        public string Text { get; set; }
        public bool MinMax { get; set; }
        public bool DragSpeed { get; set; }
        public Guid Key { get; set; } = Guid.NewGuid();

        public bool Equals(Property other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Property)obj);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
