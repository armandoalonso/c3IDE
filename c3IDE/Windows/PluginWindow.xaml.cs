using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using c3IDE.DataAccess;
using c3IDE.Utilities;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for PluginWindow.xaml
    /// </summary>
    public partial class PluginWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Plugin";
        private CompletionWindow completionWindow;

        public PluginWindow()
        {
            InitializeComponent();

            EditTimePluginTextEditor.TextArea.TextEntering += EditTimePluginTextEditor_TextEntering;
            EditTimePluginTextEditor.TextArea.TextEntered += EditTimePluginTextEditor_TextEntered;
        }

        private void EditTimePluginTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            //add matching closing symbol
            switch (e.Text)
            {
                case "{":
                    EditTimePluginTextEditor.Document.Insert(EditTimePluginTextEditor.TextArea.Caret.Offset, "}");
                    EditTimePluginTextEditor.TextArea.Caret.Offset--;
                    return;
                case "\"":
                    EditTimePluginTextEditor.Document.Insert(EditTimePluginTextEditor.TextArea.Caret.Offset, "\"");
                    EditTimePluginTextEditor.TextArea.Caret.Offset--;
                    return;
                case "[":
                    EditTimePluginTextEditor.Document.Insert(EditTimePluginTextEditor.TextArea.Caret.Offset, "]");
                    EditTimePluginTextEditor.TextArea.Caret.Offset--;
                    return;
                case "(":
                    EditTimePluginTextEditor.Document.Insert(EditTimePluginTextEditor.TextArea.Caret.Offset, ")");
                    EditTimePluginTextEditor.TextArea.Caret.Offset--;
                    return;
            }

            //todo: show completeion window on dot

            //figure out word segment
            var segment = EditTimePluginTextEditor.TextArea.GetCurrentWord();
            if (segment == null) return;

            //get string from segment
            var text = EditTimePluginTextEditor.Document.GetText(segment);
            if (string.IsNullOrWhiteSpace(text)) return;

            //filter completion list by string
            var data = CodeCompletionFactory.Insatnce.GetCompletionData(CodeType.Javascript).Where(x => x.Text.ToLower().Contains(text)).ToList();
            if (data.Any())
            {
                //if any data matches show completion list
                completionWindow = new CompletionWindow(EditTimePluginTextEditor.TextArea)
                {
                    //overwrite color due to global style
                    Foreground = new SolidColorBrush(Colors.Black)
                };

                var completionData = completionWindow.CompletionList.CompletionData;
                CodeCompletionDecorator.Insatnce.Decorate(ref completionData, data); ;
                completionWindow.Show();
                completionWindow.Closed += delegate { completionWindow = null; };
            }
        }

        private void EditTimePluginTextEditor_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        public void OnEnter()
        {
            EditTimePluginTextEditor.Text = AppData.Insatnce.CurrentAddon?.PluginEditTime;
            RunTimePluginTextEditor.Text =  AppData.Insatnce.CurrentAddon?.PluginRunTime;
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.PluginEditTime = EditTimePluginTextEditor.Text;
                AppData.Insatnce.CurrentAddon.PluginRunTime = RunTimePluginTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }  
        }

        private void InsertNewProperty(object sender, RoutedEventArgs e)
        {
            NewPropertyWindow.IsOpen = true;
            PropertyIdText.Text = "test-property";
            PropertyTypeDropdown.Text = "text";
        }

        private void AddPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            var id = PropertyIdText.Text;
            var type = PropertyTypeDropdown.Text;
            string template;

            //check for duplicate property id
            var propertyRegex = new Regex(@"new SDK[.]PluginProperty\(\""(?<type>\w+)\""\W+\""(?<id>\w+[-]?\w+)\""");
            var propertyMatches = propertyRegex.Matches(EditTimePluginTextEditor.Text);
            var firstProperty = propertyMatches.Count == 0;

            foreach (Match propertyMatch in propertyMatches)
            {
                if (propertyMatch.Groups["id"].ToString() == id)
                {
                    AppData.Insatnce.ErrorMessage("cannot have duplicate property id.");
                    return;
                }
            }

            var comma = firstProperty ? string.Empty : ",";
            switch (type)
            {
                case "integer":
                case "float":
                case "percent":
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", 0){comma}";
                    break;
                case "check":
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", true){comma}";
                    break;
                case "color":
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", {{\"initialValue\": [1,0,0]}} ){comma}";
                    break;
                case "combo":
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", {{\"items\":[\"item1\", \"item2\", \"item3\"]}}, \"initialValue\": \"item1\"){comma}";
                    break;
                default:
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", \"value\"){comma}";
                    break;
            }

            EditTimePluginTextEditor.Text = EditTimePluginTextEditor.Text.Replace("this._info.SetProperties([", template);
            NewPropertyWindow.IsOpen = false;
        }

        private void GenerateFileDependency(object sender, RoutedEventArgs e)
        {
            var content = string.Join(",\n",AppData.Insatnce.CurrentAddon.ThirdPartyFiles.Values.Select(x => x.PluginTemplate));
            var template = $@"this._info.AddFileDependency({content});";

            EditTimePluginTextEditor.Text =
                FormatHelper.Insatnce.Javascript(EditTimePluginTextEditor.Text.Replace("SDK.Lang.PopContext(); // .properties", $"{template}\n\nSDK.Lang.PopContext();		// .properties"));
        }
    }
}
