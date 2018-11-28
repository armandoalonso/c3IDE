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
using c3IDE.PluginModels;

namespace c3IDE
{
    public partial class PluginWindow : UserControl
    {
        private C3Plugin PluginData { get; set; }

        public PluginWindow()
        {
            InitializeComponent();

            //subscribe to plugin events
            EventSystem.Insatnce.Hub.Subscribe<PluginInitEvents>(PluginInitEventHandler);
        }

        private void PluginInitEventHandler(PluginInitEvents obj)
        {
            //set data from event
            PluginData = obj.PluginData;

            //populate controls
            pluginNameTextbox.Text = PluginData.Plugin.Name;
            pluginCompanyTextBox.Text = PluginData.Plugin.Name;
            pluginAuthorTextBox.Text = PluginData.Plugin.Name;
            pluginVersionTextBox.Text = PluginData.Plugin.Name;
            pluginDescriptionTextBox.Text = PluginData.Plugin.Description;
            pluginCategoryDropDown.Text = PluginData.Plugin.Category;

            //compile templates with data
            editTimeEditor.Text =  TextCompiler.Insatnce.CompileTemplates(PluginData.Plugin.EditTimeTemplate, PluginData.Plugin.GetPropertyDictionary());
            runTimeEditor.Text = TextCompiler.Insatnce.CompileTemplates(PluginData.Plugin.RunTimeTemplate, PluginData.Plugin.GetPropertyDictionary());
        }

        private void addPropertyButton_Click(object sender, EventArgs e)
        {

        }

        private void editPropertyButton_Click(object sender, EventArgs e)
        {

        }

        private void deltePropertyButton_Click(object sender, EventArgs e)
        {

        }
    }
}
