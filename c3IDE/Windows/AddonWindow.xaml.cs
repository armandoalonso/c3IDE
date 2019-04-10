using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.Managers;
using c3IDE.Templates;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for AddonWindow.xaml
    /// </summary>
    public partial class AddonWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Addon";
        private CompletionWindow completionWindow;
        //private Dictionary<string, ThirdPartyFile> _files;
        public ThirdPartyFile _selectedFile { get; set; }
        private ObservableCollection<ThirdPartyFile> _files;
 
        /// <summary>
        /// addon widnow constructor, setup event handeling for auto completion, and setup properties for the editors 
        /// </summary>
        public AddonWindow()
        {
            InitializeComponent();

            AddonTextEditor.TextArea.TextEntering += AddonTextEditor_TextEntering;
            AddonTextEditor.TextArea.TextEntered += AddonTextEditor_TextEntered;

            AddonTextEditor.Options.EnableEmailHyperlinks = false;
            AddonTextEditor.Options.EnableHyperlinks = false;
            FileTextEditor.Options.EnableEmailHyperlinks = false;
            FileTextEditor.Options.EnableHyperlinks = false;
        }

        /// <summary>
        /// handles when the addon widnow becomes the main window
        /// </summary>
        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(AddonTextEditor, Syntax.Json);
            ThemeManager.SetupTextEditor(FileTextEditor, Syntax.Javascript);

            if (AddonManager.CurrentAddon != null)
            {
                AddonTextEditor.Text = AddonManager.CurrentAddon.AddonJson;
                _files = new ObservableCollection<ThirdPartyFile>(AddonManager.CurrentAddon.ThirdPartyFiles.Values);
                FileListBox.ItemsSource = _files;

                if (_files.Any())
                {
                    _selectedFile = FileListBox.SelectedItem as ThirdPartyFile;
                    if (_selectedFile != null)
                    {
                        FileTextEditor.Text = _selectedFile.Content;
                    }
                }
            }
            else
            {
                AddonTextEditor.Text = string.Empty;
                FileListBox.ItemsSource = null;
                FileTextEditor.Text = string.Empty;
            }     
        }

        /// <summary>
        /// handles when the addon window is no longer the main window
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                if (_selectedFile != null)
                {
                    //only update content for readable text files
                    switch (_selectedFile.Extention.ToLower())
                    {
                        case ".js":
                        case ".html":
                        case ".css":
                        case ".txt":
                        case ".json":
                        case ".xml":
                            _selectedFile.Content = FileTextEditor.Text;
                            break;
                    }
                    _selectedFile.C2Folder = C2RuntimeFolder.IsChecked != null && C2RuntimeFolder.IsChecked.Value;
                    _selectedFile.C3Folder = C3RuntimeFolder.IsChecked != null && C3RuntimeFolder.IsChecked.Value;
                    _selectedFile.Rootfolder = RootFolder.IsChecked != null && RootFolder.IsChecked.Value;
                    _selectedFile.Bytes = Encoding.ASCII.GetBytes(FileTextEditor.Text);
                    _selectedFile.PluginTemplate = TemplateHelper.ThirdPartyFile(_selectedFile);
                }

                AddonManager.CurrentAddon.ThirdPartyFiles = new Dictionary<string, ThirdPartyFile>();
                foreach (var thirdPartyFile in _files)
                {
                    AddonManager.CurrentAddon.ThirdPartyFiles.Add(thirdPartyFile.FileName, thirdPartyFile);
                }
                
                AddonManager.CurrentAddon.AddonJson = AddonTextEditor.Text;
                AddonManager.SaveCurrentAddon();
            }
        }

        /// <summary>
        /// clears all the input on the page
        /// </summary>
        public void Clear()
        {
            _files = new ObservableCollection<ThirdPartyFile>();
            _selectedFile = null;
            FileListBox.ItemsSource = null;
            FileTextEditor.Text = string.Empty;
            AddonTextEditor.Text = string.Empty;
        }

        /// <summary>
        /// handles the text entered event for addon.json window, 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddonTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJsonDocument(AddonTextEditor.Text);

            if (!TextEditorHelper.Insatnce.MatchSymbol(AddonTextEditor, e.Text))
            {
                //figure out word segment
                var segment = AddonTextEditor.TextArea.GetCurrentWord();
                if (segment == null) return;

                //get string from segment
                var text = AddonTextEditor.Document.GetText(segment);
                if (string.IsNullOrWhiteSpace(text)) return;

                //filter completion list by string
                var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, "addon_json").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList(); ;
                if (data.Any())
                {
                    ShowCompletion(AddonTextEditor.TextArea, data);
                }
            }
        }

        /// <summary>
        /// this handles when to insert the value being used by the autocompletion window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// this handles the text editor shortcuts to insert data with tab, and to handel the find and replace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && completionWindow != null && completionWindow.CompletionList.SelectedItem == null)
            {
                e.Handled = true;
                completionWindow.CompletionList.ListBox.SelectedIndex = 0;
                completionWindow.CompletionList.RequestInsertion(EventArgs.Empty);
            }
            else if (e.Key == Key.F1)
            {
                Searcher.Insatnce.UpdateFileIndex("addon.json", AddonTextEditor.Text, ApplicationWindows.AddonWindow);
                var editor = ((TextEditor)sender);
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
        }

        /// <summary>
        /// this builds the autocompletion window and displays it
        /// </summary>
        /// <param name="textArea"></param>
        /// <param name="completionList"></param>
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

        /// <summary>
        /// adds a new third party file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddFile_OnClick(object sender, RoutedEventArgs e)
        {
            var filename = await WindowManager.ShowInputDialog("New File Name?", "please enter the name for the new javascript file", "javascriptFile.js");
            if (string.IsNullOrWhiteSpace(filename)) return;

            var tpf = new ThirdPartyFile
            {
                Content = string.Empty,
                FileName = filename,
                C3Folder = true,
                C2Folder = false,
                Rootfolder = false,
                FileType = "inline-script"
            };

            tpf.PluginTemplate = TemplateHelper.ThirdPartyFile(tpf);

            tpf.Extention = Path.GetExtension(tpf.FileName);
            if (string.IsNullOrWhiteSpace(tpf.Extention))
            {
                tpf.Extention = ".txt";
                tpf.FileName = tpf.FileName + tpf.Extention;
            }

            _files.Add(tpf);

            try
            {
                var addon = JObject.Parse(AddonTextEditor.Text);
                var fileList = JArray.Parse(addon["file-list"].ToString());

                if (tpf.C3Folder) fileList.Add("c3runtime/" + tpf.FileName);
                if (tpf.C2Folder) fileList.Add("c2runtime/" + tpf.FileName);
                if (tpf.Rootfolder) fileList.Add(tpf.FileName);

                addon["file-list"] = fileList;
                AddonTextEditor.Text = addon.ToString(Formatting.Indented);
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"error parseing json, addon.json not updated => {ex.Message}");
            }
           
            //AddonManager.CurrentAddon.ThirdPartyFiles = _files;
            //FileListBox.ItemsSource = _files;
            //FileListBox.Items.Refresh();
        }

        /// <summary>
        /// removes the selected file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (FileListBox.SelectedIndex == -1)
            {
                NotificationManager.PublishErrorNotification("failed to remove file, no file selected");
                return;
            }

            _selectedFile = FileListBox.SelectedItem as ThirdPartyFile;
            if (_selectedFile != null)
            {
                var file = _selectedFile;
                _files.Remove(_selectedFile);
                FileListBox.ItemsSource = _files;
                FileListBox.Items.Refresh();

                //AddonManager.CurrentAddon.ThirdPartyFiles = _files;
                //AddonManager.SaveCurrentAddon();
                //AddonManager.LoadAllAddons();

                //clear editors
                FileTextEditor.Text = string.Empty;

                try
                {
                    var addon = JObject.Parse(AddonTextEditor.Text);
                    var fileList = JArray.Parse(addon["file-list"].ToString());

                    //remove all checks
                    foreach (var item in fileList.Children().ToList())
                    {
                        if (item.ToString().Equals("c3runtime/" + file.FileName) ||
                            item.ToString().Equals("c2runtime/" + file.FileName) ||
                            item.ToString().Equals(file.FileName))
                        {
                            fileList.Remove(item);
                        }
                    }

                    addon["file-list"] = fileList;
                    AddonTextEditor.Text = addon.ToString(Formatting.Indented);
                }
                catch (Exception ex)
                {
                    LogManager.AddErrorLog(ex);
                    NotificationManager.PublishErrorNotification($"error parseing json, addon.json not updated => {ex.Message}");
                }

                _selectedFile = null;
                C3RuntimeFolder.IsChecked = false;
                C2RuntimeFolder.IsChecked = false;
                RootFolder.IsChecked = false;
            }
            else
            {
                NotificationManager.PublishErrorNotification("failed to remove action, no 3rd party files selected");
            }
        }

        /// <summary>
        /// create file copy effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListBox_OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// imports a thrird party file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListBox_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                var file = ((string[])e.Data.GetData(DataFormats.FileDrop))?.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(file))
                {
                    var info = new FileInfo(file);
                    var filename = info.Name;
                    var content = File.ReadAllText(file);
                    var bytes = File.ReadAllBytes(file);

                    switch (info.Extension)
                    {
                        case ".js":
                            content = FormatHelper.Insatnce.Javascript(content);
                            break;
                    }

                    var tpf = new ThirdPartyFile
                    {
                        Content = content,
                        FileName = filename,
                        Bytes = bytes,
                        Extention = info.Extension.ToLower(),
                        FileType = "inline-script"
                    };

                    tpf.PluginTemplate = TemplateHelper.ThirdPartyFile(tpf);
                    _files.Add(tpf);

                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens(filename, content);
                    FileListBox.ItemsSource = _files;
                    FileListBox.Items.Refresh();
                }
            }
            catch (Exception exception)
            {
                LogManager.AddErrorLog(exception);
                NotificationManager.PublishErrorNotification($"error adding third party file, {exception.Message}");
            }
        }

        /// <summary>
        /// changes the active thrird party file and displays the content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ////save current selection
            if (_selectedFile != null)
            {
                _selectedFile.Content = FileTextEditor.Text;
                _selectedFile.C2Folder = C2RuntimeFolder.IsChecked != null && C2RuntimeFolder.IsChecked.Value;
                _selectedFile.C3Folder = C3RuntimeFolder.IsChecked != null && C3RuntimeFolder.IsChecked.Value;
                _selectedFile.Rootfolder = RootFolder.IsChecked != null && RootFolder.IsChecked.Value;
                _selectedFile.Bytes = Encoding.ASCII.GetBytes(FileTextEditor.Text);
                _selectedFile.FileType = FileTypeDropDown.Text;
                _selectedFile.PluginTemplate = TemplateHelper.ThirdPartyFile(_selectedFile);
            }

            //load new selection
            _selectedFile = FileListBox.SelectedItem as ThirdPartyFile;
            if (_selectedFile == null) return;
            C2RuntimeFolder.IsChecked = _selectedFile.C2Folder;
            C3RuntimeFolder.IsChecked = _selectedFile.C3Folder;
            RootFolder.IsChecked = _selectedFile.Rootfolder;
            FileTypeDropDown.Text = _selectedFile.FileType;

            switch (_selectedFile.Extention?.ToLower() ?? ".txt")
            {
                case ".js":
                case ".html":
                case ".css":
                case ".txt":
                case ".json":
                case ".xml":
                    FileTextEditor.Text = FormatHelper.Insatnce.Javascript(_selectedFile.Content);
                    break;   
                default:
                    FileTextEditor.Text = $"BINARY FILE => {_selectedFile.FileName}";
                    break;
            }
            
        }

        /// <summary>
        /// apply json formatting to addon.json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJsonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
            AddonTextEditor.Text = AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text);
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"failed to format json => {ex.Message}");
            }
        }

        //todo: should we allow formatting thrid party files? if so we need to do it by extention and have different formatting strageties
        /// <summary>
        /// format thrid party file as javascript
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJavascriptMenu_OnClick(object sender, RoutedEventArgs e)
        {
            FileTextEditor.Text = FormatHelper.Insatnce.Json(FileTextEditor.Text);
        }

        private void Tab_ChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            if (AddonJsTab.IsSelected)
            {
                ////save current selection
                if (_selectedFile != null)
                {
                    //only update content for readable text files
                    switch (_selectedFile.Extention?.ToLower() ?? ".txt")
                    {
                        case ".js":
                        case ".html":
                        case ".css":
                        case ".txt":
                        case ".json":
                        case ".xml":
                            _selectedFile.Content = FileTextEditor.Text;
                            break;
                    }
                    _selectedFile.C2Folder = C2RuntimeFolder.IsChecked != null && C2RuntimeFolder.IsChecked.Value;
                    _selectedFile.C3Folder = C3RuntimeFolder.IsChecked != null && C3RuntimeFolder.IsChecked.Value;
                    _selectedFile.Rootfolder = RootFolder.IsChecked != null && RootFolder.IsChecked.Value;
                    _selectedFile.Bytes = Encoding.ASCII.GetBytes(FileTextEditor.Text);
                    _selectedFile.FileType = FileTypeDropDown.Text;
                    _selectedFile.PluginTemplate = TemplateHelper.ThirdPartyFile(_selectedFile);
                }

                //update addon.json
                try
                {
                    var addon = JObject.Parse(AddonTextEditor.Text);
                    var fileList = JArray.Parse(addon["file-list"].ToString());

                    foreach (var thirdPartyFile in _files)
                    {
                        //remove all checks
                        foreach (var item in fileList.Children().ToList())
                        {
                            if (item.ToString().Equals("c3runtime/" + thirdPartyFile.FileName.Replace("\\", "/")) ||
                                item.ToString().Equals("c2runtime/" + thirdPartyFile.FileName.Replace("\\", "/")) ||
                                item.ToString().Equals(thirdPartyFile.FileName.Replace("\\", "/")))
                            {
                                fileList.Remove(item);
                            }
                        }

                        if (thirdPartyFile.C3Folder) fileList.Add("c3runtime/" + thirdPartyFile.FileName.Replace("\\", "/"));
                        if (thirdPartyFile.C2Folder) fileList.Add("c2runtime/" + thirdPartyFile.FileName.Replace("\\", "/"));
                        if (thirdPartyFile.Rootfolder) fileList.Add(thirdPartyFile.FileName.Replace("\\", "/"));

                        addon["file-list"] = fileList;
                    }
 
                    AddonTextEditor.Text = addon.ToString(Formatting.Indented);
                }
                catch (Exception ex)
                {
                    LogManager.AddErrorLog(ex);
                    NotificationManager.PublishErrorNotification($"error parseing json, addon.json not updated => {ex.Message}");
                }
            }
        }
    }
}