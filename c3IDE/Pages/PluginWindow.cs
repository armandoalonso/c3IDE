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

namespace c3IDE
{
    public partial class PluginWindow : UserControl
    {
        public PluginWindow()
        {
            InitializeComponent();

            //subscribe to plugin events
            EventSystem.Insatnce.Hub.Subscribe<NewPluginEvents>(NewPluginEventHandler);
        }

        private void NewPluginEventHandler(EventMessageBase obj)
        {
            
        }
    }
}
