using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AutoCompletionGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("input.txt");
            var count = lines.Length;
            var curLine = 0;

            var list = new List<AutoCompletionPoco>();

            while (curLine < count)
            {
                var text = lines[curLine];
                var type = lines[curLine + 1];
                var container = lines[curLine + 2];
                var desc = lines[curLine + 3];

                var obj = new AutoCompletionPoco
                {
                    Text = text,
                    Type = int.Parse(type),
                    DescriptionText = desc,
                    Container = container,
                };

                list.Add(obj);

                curLine = curLine+4;
            }

            System.IO.File.WriteAllText("output.json", JsonConvert.SerializeObject(list, Formatting.Indented));
            Process.Start("output.json");
        }
    }
}
