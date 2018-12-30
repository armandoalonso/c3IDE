using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Models
{
    public class Aces
    {
        public Dictionary<string, string> Categories { get; set; } = new Dictionary<string, string>();

        public List<Action> Actions { get; set; } = new List<Action>();

        public List<Condition> Conditions { get; set; } = new List<Condition>();

        public List<Expression> Expressions{ get; set; } = new List<Expression>();
    }
}
