using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Utilities;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Logging;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Utilities.ThemeEngine;
using ICSharpCode.AvalonEdit.Editing;


namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for AddonWindow.xaml
    /// </summary>
    public partial class AddonWindow : UserControl, IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Addon";
        private CompletionWindow completionWindow;
        private Dictionary<string, ThirdPartyFile> _files;
        private ThirdPartyFile _selectedFile;
        public  List<TabItem> Tabs { get; set; }
 
        //ctor
        public AddonWindow()
        {
            InitializeComponent();

            AddonTextEditor.TextArea.TextEntering += AddonTextEditor_TextEntering;
            AddonTextEditor.TextArea.TextEntered += AddonTextEditor_TextEntered;

            AddonTextEditor.Options.EnableEmailHyperlinks = false;
            AddonTextEditor.Options.EnableHyperlinks = false;
            FileTextEditor.Options.EnableEmailHyperlinks = false;
            FileTextEditor.Options.EnableHyperlinks = false;

            //add tabs
            Tabs = new List<TabItem> { AddonJsTab, ThirdPartyFileTab };
        }

        //editor events
        private void AddonTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJsonDocument(AddonTextEditor.Text);

            //add matching closing symbol
            switch (e.Text)
            {
                case "{":
                    AddonTextEditor.Document.Insert(AddonTextEditor.TextArea.Caret.Offset, "}");
                    AddonTextEditor.TextArea.Caret.Offset--;
                    return;

                case "\"":
                    AddonTextEditor.Document.Insert(AddonTextEditor.TextArea.Caret.Offset, "\"");
                    AddonTextEditor.TextArea.Caret.Offset--;
                    return;

                case "[":
                    AddonTextEditor.Document.Insert(AddonTextEditor.TextArea.Caret.Offset, "]");
                    AddonTextEditor.TextArea.Caret.Offset--;
                    return;

                case "(":
                    AddonTextEditor.Document.Insert(AddonTextEditor.TextArea.Caret.Offset, ")");
                    AddonTextEditor.TextArea.Caret.Offset--;
                    return;

                default:
                    //figure out word segment
                    var segment = AddonTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = AddonTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.Json, "addonjs").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList(); ;
                    if (data.Any())
                    {
                        ShowCompletion(AddonTextEditor.TextArea, data);
                    }
                    break;
            }
        }

        private void AddonTextEditor_TextEntering(object sender, TextCompositionEventArgs e)
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

        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && completionWindow != null && completionWindow.CompletionList.SelectedItem == null)
            {
                e.Handled = true;
                completionWindow.CompletionList.ListBox.SelectedIndex = 0;
                completionWindow.CompletionList.RequestInsertion(EventArgs.Empty);
            }
            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                AppData.Insatnce.GlobalSave();
            }
        }

        //completion window
        private void ShowCompletion(TextArea textArea, List<GenericCompletionItem> completionList)
        {
            //if any data matches show completion list
            completionWindow = new CompletionWindow(textArea)
            {
                //overwrite color due to global style
                Foreground = new SolidColorBrush(Colors.Black)
            };
            var completionData = completionWindow.CompletionList.CompletionData;
            CodeCompletionDecorator.Insatnce.Decorate(ref completionData, completionList); ;
            completionWindow.Width = 250;
            completionWindow.CompletionList.ListBox.Items.SortDescriptions.Add(new SortDescription("Type", ListSortDirection.Ascending));

            completionWindow.Show();
            completionWindow.Closed += delegate { completionWindow = null; };
        }

        //window states
        public void OnEnter()
        {
            AppData.Insatnce.SetupTextEditor(AddonTextEditor, Syntax.Json);
            AppData.Insatnce.SetupTextEditor(FileTextEditor, Syntax.Javascript);

            if (AppData.Insatnce.CurrentAddon != null)
            {
                AddonTextEditor.Text = AppData.Insatnce.CurrentAddon.AddonJson;
                _files = AppData.Insatnce.CurrentAddon.ThirdPartyFiles;
                FileListBox.ItemsSource = _files;

                if (_files.Any())
                {
                    FileListBox.SelectedIndex = 0;
                    _selectedFile = ((KeyValuePair<string, ThirdPartyFile>)FileListBox.SelectedItem).Value;
                    FileTextEditor.Text = _selectedFile.Content;
                }
            }
            else
            {
                AddonTextEditor.Text = string.Empty;
                FileListBox.ItemsSource = null;
                FileTextEditor.Text = string.Empty;
            }
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                if (_selectedFile != null)
                {
                    _selectedFile.Content = FileTextEditor.Text;
                    _files[_selectedFile.FileName] = _selectedFile;
                }

                AppData.Insatnce.CurrentAddon.ThirdPartyFiles = _files;
                AppData.Insatnce.CurrentAddon.AddonJson = AddonTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }
        }

        public void SetupTheme(Theme t)
        {
            
        }

        //button clicks
        private async void AddFile_OnClick(object sender, RoutedEventArgs e)
        {
            var filename = await AppData.Insatnce.ShowInputDialog("New File Name?", "please enter the name for the new javascript file", "javascriptFile.js");
            if (string.IsNullOrWhiteSpace(filename)) return;

            _files.Add(filename, new ThirdPartyFile
            {
                Content = string.Empty,
                FileName = filename,
                PluginTemplate = $@"{{
	filename: ""c3runtime/{filename}"",
	type: ""inline-script""
}}"
            });

            AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text.Replace(@"file-list"": [", $@"file-list"": [
        ""c3runtime/{filename}"","));

            //add
            FileListBox.Items.Refresh();
            FileListBox.SelectedIndex = _files.Count - 1;
            _selectedFile = ((KeyValuePair<string, ThirdPartyFile>)FileListBox.SelectedItem).Value;
            FileTextEditor.Text = _selectedFile.Content;
        }

        private void RemoveFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (FileListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("failed to remove file, no file selected");
                return;
            }

            _selectedFile = ((KeyValuePair<string, ThirdPartyFile>)FileListBox.SelectedItem).Value;
            if (_selectedFile != null)
            {
                _files.Remove(_selectedFile.FileName);
                FileListBox.ItemsSource = _files;
                FileListBox.Items.Refresh();

                AppData.Insatnce.CurrentAddon.ThirdPartyFiles = _files;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
                AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();

                //clear editors
                FileTextEditor.Text = string.Empty;
                AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text.Replace($@"""c3runtime/{_selectedFile.FileName}"",", string.Empty));
                _selectedFile = null;
            }
            else
            {
                AppData.Insatnce.ErrorMessage("failed to remove action, no 3rd party files selected");
            }
        }

        //file drop
        private void FileListBox_OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void FileListBox_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                var file = ((string[])e.Data.GetData(DataFormats.FileDrop))?.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(file))
                {
                    var info = new FileInfo(file);
                    var name = info.Name;
                    var content = File.ReadAllText(file);

                    _files.Add(name, new ThirdPartyFile
                    {
                        Content = content,
                        FileName = name,
                        PluginTemplate = $@"{{
	filename: ""c3runtime/{name}"",
	type: ""inline-script""
}}"
                    });

                    AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text.Replace(@"file-list"": [", $@"file-list"": [
        ""c3runtime/{name}"","));

                    //add
                    FileListBox.Items.Refresh();
                    FileListBox.SelectedIndex = _files.Count - 1;
                    _selectedFile = ((KeyValuePair<string, ThirdPartyFile>)FileListBox.SelectedItem).Value;

                    //todo: determine if i should display file content
                    FileTextEditor.Text = _selectedFile.Content;
                }
            }
            catch (Exception exception)
            {
                LogManager.Insatnce.Exceptions.Add(exception);
                Console.WriteLine(exception.Message);
                AppData.Insatnce.ErrorMessage($"error adding third party file, {exception.Message}");
            }
        }

        //list box events
        private void FileListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FileListBox.SelectedIndex == -1)
            {
                //ignore
                return;
            }

            //save current selection
            if (_selectedFile != null)
            {
                //todo: determine if i should display file content
                _selectedFile.Content = FileTextEditor.Text;
                _files[_selectedFile.FileName] = _selectedFile;

                //load new selection
                _selectedFile = ((KeyValuePair<string, ThirdPartyFile>)FileListBox.SelectedItem).Value;

                //todo: determine if i should display file content
                FileTextEditor.Text = _selectedFile.Content;
            }
        }

        //context menu
        private void FormatJsonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            AddonTextEditor.Text = AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text);
        }

        private async void AddFileMenu_OnClick(object sender, RoutedEventArgs e)
        {
            //todo: remove duplicate code
            var filename = await AppData.Insatnce.ShowInputDialog("New File Name?", "please enter the name for the new javascript file", "javascriptFile.js");
            if (string.IsNullOrWhiteSpace(filename)) return;

            _files.Add(filename, new ThirdPartyFile
            {
                Content = string.Empty,
                FileName = filename,
                PluginTemplate = $@"{{
	filename: ""c3runtime/{filename}"",
	type: ""inline-script""
}}"
            });

            AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text.Replace(@"file-list"": [", $@"file-list"": [
        ""c3runtime/{filename}"","));

            //add
            FileListBox.Items.Refresh();
            FileListBox.SelectedIndex = _files.Count - 1;
            _selectedFile = ((KeyValuePair<string, ThirdPartyFile>)FileListBox.SelectedItem).Value;
            FileTextEditor.Text = _selectedFile.Content;
        }

        private void RemoveFileMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (FileListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("failed to remove file, no file selected");
                return;
            }

            //todo: remove duplicate code
            _selectedFile = ((KeyValuePair<string, ThirdPartyFile>)FileListBox.SelectedItem).Value;
            if (_selectedFile != null)
            {
                _files.Remove(_selectedFile.FileName);
                FileListBox.ItemsSource = _files;
                FileListBox.Items.Refresh();

                AppData.Insatnce.CurrentAddon.ThirdPartyFiles = _files;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
                AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();

                //clear editors
                FileTextEditor.Text = string.Empty;
                AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text.Replace($@"""c3runtime/{_selectedFile.FileName}"",", string.Empty));
                _selectedFile = null;
            }
            else
            {
                AppData.Insatnce.ErrorMessage("failed to remove action, no 3rd party files selected");
            }
        }

        private void FormatJavascriptMenu_OnClick(object sender, RoutedEventArgs e)
        {
            FileTextEditor.Text = FormatHelper.Insatnce.Json(FileTextEditor.Text);
        }
    }
}