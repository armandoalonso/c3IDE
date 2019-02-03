using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using c3IDE.Utilities.Helpers;

namespace c3IDE.Utilities.CodeCompletion
{
    //This class resolves the icon type for the completion item
    public class CompletionTypeFactory : Singleton<CompletionTypeFactory>
    {

        private Dictionary<CompletionType, ImageSource> iconCache = new Dictionary<CompletionType, ImageSource>();

        public ImageSource GetIcon(CompletionType type)
        {
            if (iconCache.ContainsKey(type))
            {
                return iconCache[type];
            }

            string base64;
            switch (type)
            {
                case CompletionType.Methods:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.methods.png");
                    break;
                case CompletionType.Variables:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.variables.png");
                    break;
                case CompletionType.Classes:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.classes.png");
                    break;
                case CompletionType.Interfaces:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.interfaces.png");
                    break;
                case CompletionType.Modules:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.modules.png");
                    break;
                case CompletionType.Properties:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.properties.png");
                    break;
                case CompletionType.Values:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.values.png");
                    break;
                case CompletionType.References:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.references.png");
                    break;
                case CompletionType.Keywords:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.keywords.png");
                    break;
                case CompletionType.Globals:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.globals.png");
                    break;
                case CompletionType.Colors:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.colors.png");
                    break;
                case CompletionType.Units:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.units.png");
                    break;
                case CompletionType.Snippets:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.snippets.png");
                    break;
                case CompletionType.Words:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.words.png");
                    break;
                case CompletionType.Misc:
                    base64 = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Utilities.CodeCompletion.Icons.misc.png");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var img = ImageHelper.Insatnce.Base64ToBitmap(base64);
            iconCache.Add(type, img);
            return img;
        }
    }

    public enum CompletionType
    {
        Methods,
        Variables,
        Classes,
        Interfaces,
        Modules,
        Properties,
        Values,
        References,
        Keywords,
        Globals,
        Colors,
        Units,
        Snippets,
        Words,
        Misc
    }
}
