using Esprima;
using Newtonsoft.Json;

namespace c3IDE.Managers
{
    public static class LintingManager
    {
        public static void Lint(string source)
        {
            var parser = new JavaScriptParser(AddonManager.CurrentAddon.InstanceRunTime);
            var program = parser.ParseProgram(true);
            var json = JsonConvert.SerializeObject(program);
        }
    }
}
