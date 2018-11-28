using c3IDE.EventCore;
using c3IDE.PluginTemplates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using c3IDE.PluginModels;

namespace c3IDE
{
    public partial class MainForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public MainForm()
        {
            InitializeComponent();

            //initialize event system and subscribe to events
            EventSystem.Insatnce.Hub.Subscribe<NewPluginEvents>(NewPluginEventHandler);

            //initialize the application state
            ActivePanel.Height = HomeButton.Height;
            ActivePanel.Top = HomeButton.Top;
            homeWindow.BringToFront();
        }

        //handles the creation of a new plugin
        private void NewPluginEventHandler(NewPluginEvents obj)
        {
            //create plugin data
            var type = (PluginTypeEnum)obj.Content;
            var pluginTemplate = TemplateFactory.Insatnce.CreateTemplate(type);
            var pluginData = C3Plugin.CreatePlugin(pluginTemplate);

            //link plugin data with forms
            EventSystem.Insatnce.Hub.Publish(new PluginInitEvents(this, pluginData));

            //enable all other tabs
            PluginButton.Enabled = true;
            PluginButton.ForeColor = Color.White;
            TypeButton.Enabled = true;
            TypeButton.ForeColor = Color.White;
            InstanceButton.Enabled = true;
            InstanceButton.ForeColor = Color.White;
            ActionButton.Enabled = true;
            ActionButton.ForeColor = Color.White;
            ConditionButton.Enabled = true;
            ConditionButton.ForeColor = Color.White;
            ExpressionButton.Enabled = true;
            ExpressionButton.ForeColor = Color.White;
            TestButton.Enabled = true;
            TestButton.ForeColor = Color.White;
            ExportButton.Enabled = true;
            ExportButton.ForeColor = Color.White;
        }

        //trick form into thinking title bar is being pressed, to pan window
        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        //exits the application
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //bring up the home page
        private void HomeButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = HomeButton.Height;
            ActivePanel.Top = HomeButton.Top;
            homeWindow.BringToFront();
        }

        //bring up the plugins page
        private void PluginButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = PluginButton.Height;
            ActivePanel.Top = PluginButton.Top;
            pluginWindow.BringToFront();
        }

        //bring up the type page
        private void TypeButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = TypeButton.Height;
            ActivePanel.Top = TypeButton.Top;
            typeWindow.BringToFront();
        }

        //bring up the instance page
        private void InstanceButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = InstanceButton.Height;
            ActivePanel.Top = InstanceButton.Top;
            instanceWindow.BringToFront();
        }

        //bring up the actions page
        private void ActionButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = ActionButton.Height;
            ActivePanel.Top = ActionButton.Top;
            actionsWindow.BringToFront();
        }

        //bring up the condition page
        private void ConditionButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = ConditionButton.Height;
            ActivePanel.Top = ConditionButton.Top;
            conditionsWindow.BringToFront();
        }

        //bring up the expressions page
        private void ExpressionButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = ExpressionButton.Height;
            ActivePanel.Top = ExpressionButton.Top;
            expressionsWindow.BringToFront();
        }

        //bring up the test page
        private void TestButton_Click(object sender, EventArgs e)
        {
            //TODO: check out edge.js to run Node Processes in .NET (https://github.com/tjanczuk/edge)
            ActivePanel.Height = TestButton.Height;
            ActivePanel.Top = TestButton.Top;
            testWindow.BringToFront();
        }

        //bring up the export page
        private void ExportButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = ExportButton.Height;
            ActivePanel.Top = ExportButton.Top;
            exportWindow.BringToFront();
        }

        //maximize window
        private void MaximizeButton_Click(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.WindowState = FormWindowState.Maximized;
        }

        //minimize window
        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //restore window to default
        private void RestoreButton_Click(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.CenterToScreen();
        }
    }
}
