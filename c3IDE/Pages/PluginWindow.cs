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
using Syncfusion.Windows.Forms.Edit;
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

            //clear syntax editor shortcut keys
            //editTimeEditor.Commands.Clear();
            //runTimeEditor.Commands.Clear();
            //editTimeTemplateEditor.Commands.Clear();
            //runTimeTemplateEditor.Commands.Clear();

            //subscribe to plugin events
            EventSystem.Insatnce.Hub.Subscribe<UpdatePluginEvents>(UpdatePluginEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<SavePluginEvents>(SavePluginEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<NewPropertyPluginEvents>(NewPropertyEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<UpdatePropertyPluginEvents>(UpdatePropertyEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<CompilePluginEvents>(CompilePluginEventHandler);
        }

        private void CompilePluginEventHandler(CompilePluginEvents obj)
        {
            //save and compile the plugin window
            SavePluginEventHandler(new SavePluginEvents(this));
            UpdatePluginEventHandler(new UpdatePluginEvents(this, PluginData));
        }

        private void UpdatePropertyEventHandler(UpdatePropertyPluginEvents obj)
        {   
            //update an existing property 
            var propIndex = PluginData.Plugin.Properties.IndexOf(obj.OldProperty);
            if (propIndex != -1)
            {
                PluginData.Plugin.Properties[propIndex] = obj.NewProperty;
            }

            propertiesListBox.DataSource = null;
            propertiesListBox.DataSource = PluginData.Plugin.Properties;
            propertiesListBox.DisplayMember = "Id";
        }

        private void NewPropertyEventHandler(NewPropertyPluginEvents obj)
        {
            //add new property
            PluginData.Plugin.Properties.Add(obj.Property);
            propertiesListBox.DataSource = null;
            propertiesListBox.DataSource = PluginData.Plugin.Properties;
            propertiesListBox.DisplayMember = "Id";
        }

        private void SavePluginEventHandler(SavePluginEvents obj)
        {
            //validate input
            if(!ValidateInput()) return; //TODO: have better feedback for error states

            //modify plugin property
            PluginData.Plugin.Name = pluginNameTextbox.Text;
            PluginData.Plugin.Company = pluginCompanyTextBox.Text;
            PluginData.Plugin.Author = pluginAuthorTextBox.Text;
            PluginData.Plugin.Version = pluginVersionTextBox.Text;
            PluginData.Plugin.Description = pluginDescriptionTextBox.Text;
            PluginData.Plugin.Category = pluginCategoryDropDown.Text;
            PluginData.Plugin.Properties = propertiesListBox.Items.Cast<Property>().ToList() ?? new List<Property>();
            PluginData.Plugin.Icon = iconImage.Image;

            //modify template/source property
            PluginData.Plugin.EditTimeTemplate = editTimeTemplateEditor.Text;
            PluginData.Plugin.RunTimeTemplate = runTimeTemplateEditor.Text;

            //compile 
            editTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(PluginData.Plugin.EditTimeTemplate, PluginData);
            runTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(PluginData.Plugin.RunTimeTemplate, PluginData);

            //modify based on compilation
            PluginData.Plugin.EditTimeFile = editTimeEditor.Text;
            PluginData.Plugin.RunTimeFile = runTimeEditor.Text;

            //save shared gloabl plugin 
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
            iconImage.Image = PluginData.Plugin.Icon;
            propertiesListBox.DataSource = null;
            propertiesListBox.DataSource = PluginData.Plugin.Properties;
            propertiesListBox.DisplayMember = "Id";
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
            //open property window when adding new property
            AddPropertyWindow window = new AddPropertyWindow();
            window.NewProperty();
            window.ShowDialog();
        }

        private void editPropertyButton_Click(object sender, EventArgs e)
        {
            //open property window when editing an existing property
            var selectedProperty = propertiesListBox.SelectedItem as Property;
            if (selectedProperty == null) return; //TODO: add feedback for this error when propety is null

            AddPropertyWindow window = new AddPropertyWindow();
            window.EditProperty(selectedProperty);
            window.ShowDialog();
        }

        private void deletePropertyButton_Click(object sender, EventArgs e)
        {
            //TODO: implement deleting property
        }

        private bool ValidateInput()
        {
            //validates none of the require input is empty or null
            return !string.IsNullOrWhiteSpace(pluginNameTextbox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginCompanyTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginAuthorTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginVersionTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginDescriptionTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(pluginCategoryDropDown.Text);
        }

        //bind all editors to overwrite ctrl s for saving
        private void editor_RegisteringDefaultKeyBindings(object sender, EventArgs e)
        {
            ((EditControl)sender).KeyBinder.BindToCommand(Keys.Control | Keys.S, "None");
            //TODO: implement custom save method here
        }

        //allow editing properties by double clicking on the property
        private void propertiesListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedProperty = propertiesListBox.SelectedItem as Property;
            if (selectedProperty == null) return; //TODO: add feedback for this error when propety is null

            AddPropertyWindow window = new AddPropertyWindow();
            window.EditProperty(selectedProperty);
            window.ShowDialog();
        }

        //open up image dialog box
        private void iconImage_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = @"Open Icon Image";
                dlg.Filter = @"img files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(dlg.FileName);
                    iconImage.Image = img;
                }
            }
        }
    }
}
