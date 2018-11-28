using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using c3IDE.EventCore;
using c3IDE.Framework;
using c3IDE.Pages;
using c3IDE.PluginModels;

namespace c3IDE
{
    public partial class PluginWindow : UserControl
    {
        private C3Plugin PluginData { get; set; }

        public PluginWindow()
        {
            InitializeComponent();

            //configure syntax highlighting
            editTimeEditor.ApplyConfiguration(Syncfusion.Windows.Forms.Edit.Enums.KnownLanguages.JScript);
            //TODO: check if sync fusion has EMCA5 version of js syntax highlighting
            runTimeEditor.ApplyConfiguration(Syncfusion.Windows.Forms.Edit.Enums.KnownLanguages.JScript);

            //subscribe to plugin events
            EventSystem.Insatnce.Hub.Subscribe<UpdatePluginEvents>(UpdatePluginEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<SavePluginEvents>(SavePluginEventHandler);
        }

        private void SavePluginEventHandler(SavePluginEvents obj)
        {
            PluginData.Plugin.Name = pluginNameTextbox.Text;
            PluginData.Plugin.Company = pluginCompanyTextBox.Text;
            PluginData.Plugin.Author = pluginAuthorTextBox.Text;
            PluginData.Plugin.Version = pluginVersionTextBox.Text;
            PluginData.Plugin.Description = pluginDescriptionTextBox.Text;
            PluginData.Plugin.Category = pluginCategoryDropDown.Text;

            Global.Insatnce.CurrentPlugin.Plugin = PluginData.Plugin;
        }

        private void UpdatePluginEventHandler(UpdatePluginEvents obj)
        {
            //set data from event
            PluginData = obj.PluginData;

            //populate controls
            pluginNameTextbox.Text = PluginData.Plugin.Name;
            pluginCompanyTextBox.Text = PluginData.Plugin.Company;
            pluginAuthorTextBox.Text = PluginData.Plugin.Author;
            pluginVersionTextBox.Text = PluginData.Plugin.Version;
            pluginDescriptionTextBox.Text = PluginData.Plugin.Description;
            pluginCategoryDropDown.Text = PluginData.Plugin.Category;

            //compile templates with data
            editTimeEditor.Text =  TextCompiler.Insatnce.CompileTemplates(PluginData.Plugin.EditTimeTemplate, PluginData.Plugin.GetPropertyDictionary());
            runTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(PluginData.Plugin.RunTimeTemplate, PluginData.Plugin.GetPropertyDictionary());

            //format text
            editTimeEditor.Text = new JSBeautify(editTimeEditor.Text, new JSBeautifyOptions()).GetResult();
            runTimeEditor.Text = new JSBeautify(runTimeEditor.Text, new JSBeautifyOptions()).GetResult();
        }

        private void addPropertyButton_Click(object sender, EventArgs e)
        {
            //TODO: customize the add property dialog have syntax editor 
            AddPropertyWindow window = new AddPropertyWindow();
            window.ShowDialog();
        }

        private void editPropertyButton_Click(object sender, EventArgs e)
        {

        }

        private void deltePropertyButton_Click(object sender, EventArgs e)
        {

        }
    }
}
