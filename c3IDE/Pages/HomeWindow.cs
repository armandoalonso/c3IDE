using System;
using System.Windows.Forms;
using c3IDE.EventCore;

namespace c3IDE.Pages
{
    public partial class HomeWindow : UserControl
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void NewSingleGlobalPluginButton_Click(object sender, EventArgs e)
        {
            EventSystem.Insatnce.Hub.Publish(new NewPluginEvents(this, PluginTypeEnum.SingleGlobal));
        }
    }
}
