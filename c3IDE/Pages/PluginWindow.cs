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
        private C3Plugin _pluginData;

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
            EventSystem.Insatnce.Hub.Subscribe<NewPropertyPluginEvents>(NewPropertyEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<UpdatePropertyPluginEvents>(UpdatePropertyEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<CompilePluginEvents>(CompilePluginEventHandler);
        }

        private void CompilePluginEventHandler(CompilePluginEvents obj)
        {
            //save and compile the plugin window
            SavePluginEventHandler(new SavePluginEvents(this));
            UpdatePluginEventHandler(new UpdatePluginEvents(this, _pluginData));
        }

        private void UpdatePropertyEventHandler(UpdatePropertyPluginEvents obj)
        {   
            //update an existing property 
            var propIndex = _pluginData.Plugin.Properties.IndexOf(obj.OldProperty);
            if (propIndex != -1)
            {
                _pluginData.Plugin.Properties[propIndex] = obj.NewProperty;
            }

            propertiesListBox.DataSource = null;
            propertiesListBox.DataSource = _pluginData.Plugin.Properties;
            propertiesListBox.DisplayMember = "Id";
        }

        private void NewPropertyEventHandler(NewPropertyPluginEvents obj)
        {
            //add new property
            _pluginData.Plugin.Properties.Add(obj.Property);
            propertiesListBox.DataSource = null;
            propertiesListBox.DataSource = _pluginData.Plugin.Properties;
            propertiesListBox.DisplayMember = "Id";
        }

        private void SavePluginEventHandler(SavePluginEvents obj)
        {
            //validate input
            if(!ValidateInput()) return; //TODO: have better feedback for error states

            //modify plugin property
            _pluginData.Plugin.Name = pluginNameTextbox.Text;
            _pluginData.Plugin.Company = pluginCompanyTextBox.Text;
            _pluginData.Plugin.Author = pluginAuthorTextBox.Text;
            _pluginData.Plugin.Version = pluginVersionTextBox.Text;
            _pluginData.Plugin.Description = pluginDescriptionTextBox.Text;
            _pluginData.Plugin.Category = pluginCategoryDropDown.Text;
            _pluginData.Plugin.Properties = propertiesListBox.Items.Cast<Property>().ToList() ?? new List<Property>();
            _pluginData.Plugin.Icon = iconImage.Image;

            //modify template/source property
            _pluginData.Plugin.EditTimeTemplate = editTimeTemplateEditor.Text;
            _pluginData.Plugin.RunTimeTemplate = runTimeTemplateEditor.Text;

            //compile 
            editTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(_pluginData.Plugin.EditTimeTemplate, _pluginData);
            runTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(_pluginData.Plugin.RunTimeTemplate, _pluginData);

            //modify based on compilation
            _pluginData.Plugin.EditTimeFile = editTimeEditor.Text;
            _pluginData.Plugin.RunTimeFile = runTimeEditor.Text;

            //save shared gloabl plugin 
            Global.Insatnce.CurrentPlugin.Plugin = _pluginData.Plugin;
        }

        private void UpdatePluginEventHandler(UpdatePluginEvents obj)
        {
            //set data from event
            _pluginData = obj.PluginData;

            //populate controls
            pluginNameTextbox.Text = _pluginData.Plugin.Name;
            pluginCompanyTextBox.Text = _pluginData.Plugin.Company;
            pluginAuthorTextBox.Text = _pluginData.Plugin.Author;
            pluginVersionTextBox.Text = _pluginData.Plugin.Version;
            pluginDescriptionTextBox.Text = _pluginData.Plugin.Description;
            pluginCategoryDropDown.Text = _pluginData.Plugin.Category;
            iconImage.Image = _pluginData.Plugin.Icon;
            propertiesListBox.DataSource = null;
            propertiesListBox.DataSource = _pluginData.Plugin.Properties;
            propertiesListBox.DisplayMember = "Id";
            editTimeTemplateEditor.Text = _pluginData.Plugin.EditTimeTemplate;
            runTimeTemplateEditor.Text = _pluginData.Plugin.RunTimeTemplate;

            //compile templates with data
            editTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(_pluginData.Plugin.EditTimeTemplate, _pluginData);
            runTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(_pluginData.Plugin.RunTimeTemplate, _pluginData);

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
