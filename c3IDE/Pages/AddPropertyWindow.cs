using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using c3IDE.EventCore;
using c3IDE.Framework;
using c3IDE.PluginModels;
using Syncfusion.Windows.Forms.Edit;

namespace c3IDE.Pages
{
    public partial class AddPropertyWindow : Form
    {
        public Property Prop { get; set; }
        public Property OldProp { get; set; }
        public PropertySaveType PropertySaveType { get; set; }

        public AddPropertyWindow()
        {
            InitializeComponent();
        }

        public void NewProperty()
        {
            PropertySaveType = PropertySaveType.NewProperty;
            Prop = new Property
            {
                Id = "new-prop",
                Value = "\"Property Value\"",
                Type = "string",
                Description = "This is a new property",
                Name = "New property",
                Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\",  {{value}})"
            };

            Prop.Text = TextCompiler.Insatnce.CompileTemplates(Prop.Template, Prop);
            PopulateGui(Prop);
        }

        public void EditProperty(Property oldprop)
        {
            OldProp = new Property
            {
                Key = oldprop.Key
            };

            Prop = new Property
            {
                Id = oldprop.Id,
                Value = oldprop.Value,
                Type = oldprop.Type,
                Description = oldprop.Description,
                Name = oldprop.Name,
                Template = oldprop.Template,
                Text = oldprop.Text,
                Key = oldprop.Key
            };

            PropertySaveType = PropertySaveType.EditProperty;
            PopulateGui(Prop);
        }

        private void PopulateGui(Property prop)
        {
            propertyIdTextBox.Text = prop.Id;
            propertyValueTextBox.Text = prop.Value;
            propertyTypeDropDown.Text = prop.Type;
            propertyNameTextBox.Text = prop.Name;
            propertyDescriptionTextBox.Text = prop.Description;
            propertyTemplateEditor.Text = prop.Template;
            propertySourceEditor.Text = prop.Text;
        }

        private void editor_RegisteringDefaultKeyBindings(object sender, EventArgs e)
        {
            ((EditControl)sender).KeyBinder.BindToCommand(Keys.Control | Keys.S, "None");
            //TODO: implement custom save method here
        }

        private void propertySaveButton_Click(object sender, EventArgs e)
        {
            Prop.Id = propertyIdTextBox.Text;
            Prop.Value = propertyValueTextBox.Text;
            Prop.Type = propertyTypeDropDown.Text;
            Prop.Description = propertyDescriptionTextBox.Text;
            Prop.Name = propertyNameTextBox.Text;
            Prop.Template = propertyTemplateEditor.Text;
            Prop.Text = propertySourceEditor.Text;

            switch (PropertySaveType)
            {
                case PropertySaveType.NewProperty:
                    EventSystem.Insatnce.Hub.Publish(new NewPropertyPluginEvents(this, Prop));
                    break;
                case PropertySaveType.EditProperty:
                    EventSystem.Insatnce.Hub.Publish(new UpdatePropertyPluginEvents(this, Prop, OldProp));
                    break;
            }

            this.Close();
        }

        private void propertyCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void propertyCompileButton_Click(object sender, EventArgs e)
        {

            Prop.Id = propertyIdTextBox.Text;
            Prop.Value = propertyValueTextBox.Text;
            Prop.Type = propertyTypeDropDown.Text;
            Prop.Description = propertyDescriptionTextBox.Text;
            Prop.Name = propertyNameTextBox.Text;
            Prop.Template = propertyTemplateEditor.Text;
            Prop.Text = TextCompiler.Insatnce.CompileTemplates(Prop.Template, Prop);

            PopulateGui(Prop);
        }
    }
}
