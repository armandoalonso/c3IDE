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
using Syncfusion.Windows.Forms.Edit.Dialogs;
using Syncfusion.Windows.Forms.Edit.Enums;
using Syncfusion.Windows.Forms.Edit.Interfaces;

namespace c3IDE
{
    public partial class PluginWindow : UserControl
    {
        private C3Plugin PluginData { get; set; }

        public PluginWindow()
        {
            InitializeComponent();

            //TODO: check if sync fusion has EMCA5 version of js syntax highlighting
            //configure syntax highlighting
            editTimeEditor.ApplyConfiguration(KnownLanguages.JScript);
            runTimeEditor.ApplyConfiguration(KnownLanguages.JScript);
            //TODO: might need new syntax for templates 
            editTimeTemplateEditor.ApplyConfiguration(KnownLanguages.JScript);
            runTimeTemplateEditor.ApplyConfiguration(KnownLanguages.JScript);

            //subscribe to plugin events
            EventSystem.Insatnce.Hub.Subscribe<UpdatePluginEvents>(UpdatePluginEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<SavePluginEvents>(SavePluginEventHandler);
        }

        private void SavePluginEventHandler(SavePluginEvents obj)
        {
            //validate input
            if(!ValidateInput()) return; //TODO: have better feedback for error states

            //modify text
            PluginData.Plugin.Name = pluginNameTextbox.Text;
            PluginData.Plugin.Company = pluginCompanyTextBox.Text;
            PluginData.Plugin.Author = pluginAuthorTextBox.Text;
            PluginData.Plugin.Version = pluginVersionTextBox.Text;
            PluginData.Plugin.Description = pluginDescriptionTextBox.Text;
            PluginData.Plugin.Category = pluginCategoryDropDown.Text;

            PluginData.Plugin.EditTimeFile = editTimeEditor.Text;
            PluginData.Plugin.RunTimeFile = runTimeEditor.Text;
            PluginData.Plugin.EditTimeTemplate = editTimeTemplateEditor.Text;
            PluginData.Plugin.RunTimeTemplate = runTimeTemplateEditor.Text;

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

            editTimeTemplateEditor.Text = PluginData.Plugin.EditTimeTemplate;
            runTimeTemplateEditor.Text = PluginData.Plugin.RunTimeTemplate;

            //compile templates with data
            editTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(PluginData.Plugin.EditTimeTemplate, PluginData);
            runTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(PluginData.Plugin.RunTimeTemplate, PluginData);

            //format text TODO: might not need to beautify the code just just template space
            //editTimeEditor.Text = new JSBeautify(editTimeEditor.Text, new JSBeautifyOptions()).GetResult();
            //runTimeEditor.Text = new JSBeautify(runTimeEditor.Text, new JSBeautifyOptions()).GetResult();
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

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(pluginNameTextbox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginCompanyTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginAuthorTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginVersionTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginDescriptionTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginCategoryDropDown.Text);
        }
    }
}
