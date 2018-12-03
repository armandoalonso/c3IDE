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
            propertySaveButton.Enabled = false;
        }

        public void NewProperty()
        {
            //create a new property
            PropertySaveType = PropertySaveType.NewProperty;
            Prop = new Property
            {
                Id = "new-prop",
                Value = "\"Property Value\"",
                Type = "text",
                Description = "This is a new property",
                Name = "New property",
                Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\",  {{value}})",
                MinMax = false,
                DragSpeed = false
            };

            Prop.Text = TextCompiler.Insatnce.CompileTemplates(Prop.Template, Prop);
            PopulateGui(Prop);
        }

        public void EditProperty(Property oldprop)
        {
            //edits an existing property
            OldProp = new Property
            {
                Key = oldprop.Key
            };

            //populate controls based on exisitng data
            Prop = new Property
            {
                Id = oldprop.Id,
                Value = oldprop.Value,
                Type = oldprop.Type,
                Description = oldprop.Description,
                Name = oldprop.Name,
                Template = oldprop.Template,
                Text = oldprop.Text,
                MinMax = oldprop.MinMax,
                DragSpeed = oldprop.DragSpeed,
                Key = oldprop.Key
            };

            PropertySaveType = PropertySaveType.EditProperty;
            PopulateGui(Prop);
        }

        private void PopulateGui(Property prop)
        {
            //populate ui controls
            propertyIdTextBox.Text = prop.Id;
            propertyValueTextBox.Text = prop.Value;
            propertyTypeDropDown.Text = prop.Type;
            propertyNameTextBox.Text = prop.Name;
            propertyDescriptionTextBox.Text = prop.Description;
            propertyTemplateEditor.Text = prop.Template;
            propertySourceEditor.Text = prop.Text;
            minMaxCheckBox.Checked = prop.MinMax;
            dragSpeedCheckBox.Checked = prop.DragSpeed;
        }

        //ignore ctrl-s default save keys
        private void editor_RegisteringDefaultKeyBindings(object sender, EventArgs e)
        {
            ((EditControl)sender).KeyBinder.BindToCommand(Keys.Control | Keys.S, "None");
            //TODO: implement custom save method here
        }

        private void propertySaveButton_Click(object sender, EventArgs e)
        {
            //save the property object
            Prop.Id = propertyIdTextBox.Text;
            Prop.Value = propertyValueTextBox.Text;
            Prop.Type = propertyTypeDropDown.Text;
            Prop.Description = propertyDescriptionTextBox.Text;
            Prop.Name = propertyNameTextBox.Text;
            Prop.Template = propertyTemplateEditor.Text;
            Prop.Text = propertySourceEditor.Text;
            Prop.MinMax = minMaxCheckBox.Checked;
            Prop.DragSpeed = dragSpeedCheckBox.Checked;

            //emit save event
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
            //close window and don't save
            this.Close();
        }

        private void propertyCompileButton_Click(object sender, EventArgs e)
        {
            //save property object based on controls
            Prop.Id = propertyIdTextBox.Text;
            Prop.Value = propertyValueTextBox.Text;
            Prop.Type = propertyTypeDropDown.Text;
            Prop.Description = propertyDescriptionTextBox.Text;
            Prop.Name = propertyNameTextBox.Text;
            Prop.Template = propertyTemplateEditor.Text;
            Prop.Text = TextCompiler.Insatnce.CompileTemplates(Prop.Template, Prop);
            Prop.MinMax = minMaxCheckBox.Checked;
            Prop.DragSpeed = dragSpeedCheckBox.Checked;

            //repopulate gui
            PopulateGui(Prop);

            //enable save button
            propertySaveButton.Enabled = true;
        }

        private void propertyTypeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            //change the property template based on the type
            var propType = propertyTypeDropDown.SelectedItem.ToString();
            switch (propType)
            {
                case "combo":
                    Prop.Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\", { \"initialValue\":{{value}}, \"items\": [\"item1\", \"item2\"] })";
                    Prop.Value = "\"item1\"";
                    break;
                case "color":
                    Prop.Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\", { \"initialValue\":[{{value}}] })";
                    Prop.Value = "1,0,0";
                    break;
                case "link":
                    //TODO: add label for other callbackType (once-for-type)
                    Prop.Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\", { \"linkCallback\":\"{{value}}\", \"callbackType\":\"for-each-instance\" })";
                    Prop.Value = "Link Callback Function";
                    break;
                case "info":
                    Prop.Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\", { \"infoCallback\":\"{{value}}\"})";
                    Prop.Value = "Info Callback Function";
                    break;
                case "integer":
                case "float":
                case "percent":
                    var minMax = minMaxCheckBox.Checked ? ", \"min\":0, \"max\":100" : string.Empty;
                    var drag = dragSpeedCheckBox.Checked ? ", \"dragSpeedMultiplier\": 0.5" : string.Empty;
                    Prop.Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\", { \"initialValue\":{{value}}"+ minMax + drag + " })";
                    Prop.Value = "10";
                    break;
                default:
                    Prop.Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\",  {{value}})";
                    break;
            }

            //compile template
            Prop.Text = TextCompiler.Insatnce.CompileTemplates(Prop.Template, Prop);
            propertyValueTextBox.Text = Prop.Value;
            propertyTemplateEditor.Text = Prop.Template;
            propertySourceEditor.Text = Prop.Text;
            propertySaveButton.Enabled = false;
        }

        private void propCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //add extra to options based on selection
            var propType = propertyTypeDropDown.SelectedItem.ToString();
            if (propType == "integer" || propType == "float" || propType == "percent")
            {
                var minMax = minMaxCheckBox.Checked ? ", \"min\":0, \"max\":100" : string.Empty;
                var drag = dragSpeedCheckBox.Checked ? ", \"dragSpeedMultiplier\": 0.5" : string.Empty;
                Prop.Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\", { \"initialValue\":{{value}}" + minMax + drag + " })";

                Prop.Text = TextCompiler.Insatnce.CompileTemplates(Prop.Template, Prop);
                propertyValueTextBox.Text = Prop.Value;
                propertyTemplateEditor.Text = Prop.Template;
                propertySourceEditor.Text = Prop.Text;
            }

            //if no extras use default template
            if (!minMaxCheckBox.Checked && !dragSpeedCheckBox.Checked)
            {
                Prop.Template = "new SDK.PluginProperty(\"{{type}}\", \"{{id}}\",  {{value}})";

                Prop.Text = TextCompiler.Insatnce.CompileTemplates(Prop.Template, Prop);
                propertyValueTextBox.Text = Prop.Value;
                propertyTemplateEditor.Text = Prop.Template;
                propertySourceEditor.Text = Prop.Text;
            }
        }

        private void propertyModel_Changed(object sender, EventArgs e)
        {
            //when we change the model disable save 
            propertySaveButton.Enabled = false;
        }
    }
}
