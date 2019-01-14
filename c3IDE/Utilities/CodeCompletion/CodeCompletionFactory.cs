using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities.CodeCompletion.Bindings;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace c3IDE.Utilities.CodeCompletion
{
    public class CodeCompletionFactory : Singleton<CodeCompletionFactory>
    {
        private readonly Dictionary<CodeType, ICompletionBindings> _bindingCache = new Dictionary<CodeType, ICompletionBindings>();

        public IList<ICompletionData> GetCompletionData(CodeType type)
        {
            if (_bindingCache.ContainsKey(type))
            {
                return _bindingCache[type].Completions;
            }

            //todo: might break down binding by type of files plugin.js, actions.js, aces.json etc 
            switch (type)
            {
                case CodeType.Json:
                    _bindingCache.Add(type, new JsonBindings());
                    return _bindingCache[type].Completions;
                case CodeType.Javascript:
                    _bindingCache.Add(type, new JavascriptBinding());
                    return _bindingCache[type].Completions;
                default:
                    return null;
            }
        }

    }

    public enum CodeType
    {
        Json,
        Javascript
    }
}
