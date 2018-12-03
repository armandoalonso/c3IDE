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
using c3IDE.PluginModels;

namespace c3IDE.Pages
{
    public partial class HomeButton : UserControl
    {
        private Action click { get; set; }
        private C3Plugin plugin { get; set; }

        public HomeButton()
        {
            InitializeComponent();
        }

        public void InitButton(C3Plugin data)
        {
            plugin = data;
            pluginButton.Image = null;
            pluginButton.BackgroundImageLayout = ImageLayout.Zoom;
            pluginButton.BackgroundImage = data.Plugin.Icon;
            pluginNameLabel.Text = data.Plugin.Name;
        }

        public void BindClick(Action newSingleGlobalPluginButtonClick)
        {
            click = newSingleGlobalPluginButtonClick;
        }

        public void ClearImage()
        {
            pluginButton.Image = null;
        }

        private void pluginButton_Click(object sender, EventArgs e)
        {
            if (click != null)
            {
                click();
            }
            else
            {
                EventSystem.Insatnce.Hub.Publish(new LoadPluginEvents(this, plugin));
            }
        }
    }
}
