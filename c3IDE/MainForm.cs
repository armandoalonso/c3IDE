using c3IDE.EventCore;
using c3IDE.PluginTemplates;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using c3IDE.DataAccess;
using c3IDE.Framework;
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

        public IRepository<C3Plugin> PluginRepository = new PluginRepository();
        public Window CurrenWindow = Window.Home;

        public MainForm()
        {
            InitializeComponent();

            //initialize event system and subscribe to events
            EventSystem.Insatnce.Hub.Subscribe<NewPluginEvents>(NewPluginEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<LoadPluginEvents>(LoadPluginEventHandler);

            //initialize the application state
            ActivePanel.Height = HomeButton.Height;
            ActivePanel.Top = HomeButton.Top;
            homeWindow.BringToFront();

            //load plugin data
            var pluginList = PluginRepository.GetAll().ToList();
            EventSystem.Insatnce.Hub.Publish(new PluginListLoadedEvents(this, pluginList));
        }

        private void LoadPluginEventHandler(LoadPluginEvents obj)
        {
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
            LanguageButton.Enabled = true;
            LanguageButton.ForeColor = Color.White;
            TestButton.Enabled = true;
            TestButton.ForeColor = Color.White;
            ExportButton.Enabled = true;
            ExportButton.ForeColor = Color.White;

            Global.Insatnce.CurrentPlugin = obj.PluginData;
            EventSystem.Insatnce.Hub.Publish(new UpdatePluginEvents(this, Global.Insatnce.CurrentPlugin));

            //switch to the plugin page
            ActivePanel.Height = PluginButton.Height;
            ActivePanel.Top = PluginButton.Top;
            pluginWindow.BringToFront();
            CurrenWindow = Window.Plugin;
        }

        //handles the creation of a new plugin
        private void NewPluginEventHandler(NewPluginEvents obj)
        {
            //create plugin data
            var type = (PluginTypeEnum)obj.Content;
            var pluginTemplate = TemplateFactory.Insatnce.CreateTemplate(type);
            var pluginData = C3Plugin.CreatePlugin(pluginTemplate);
            pluginData.Id = Guid.NewGuid();

            //link plugin data with forms
            EventSystem.Insatnce.Hub.Publish(new UpdatePluginEvents(this, pluginData));

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
            LanguageButton.Enabled = true;
            LanguageButton.ForeColor = Color.White;
            TestButton.Enabled = true;
            TestButton.ForeColor = Color.White;
            ExportButton.Enabled = true;
            ExportButton.ForeColor = Color.White;

            Global.Insatnce.CurrentPlugin = pluginData;

            //switch to the plugin page
            ActivePanel.Height = PluginButton.Height;
            ActivePanel.Top = PluginButton.Top;
            pluginWindow.BringToFront();
            CurrenWindow = Window.Plugin;
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
            CurrenWindow = Window.Home;
        }

        //bring up the plugins page
        private void PluginButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = PluginButton.Height;
            ActivePanel.Top = PluginButton.Top;
            pluginWindow.BringToFront();
            CurrenWindow = Window.Plugin;
        }

        //bring up the type page
        private void TypeButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = TypeButton.Height;
            ActivePanel.Top = TypeButton.Top;
            typeWindow.BringToFront();
            CurrenWindow = Window.Type;
        }

        //bring up the instance page
        private void InstanceButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = InstanceButton.Height;
            ActivePanel.Top = InstanceButton.Top;
            instanceWindow.BringToFront();
            CurrenWindow = Window.Instance;
        }

        //bring up the actions page
        private void ActionButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = ActionButton.Height;
            ActivePanel.Top = ActionButton.Top;
            actionsWindow.BringToFront();
            CurrenWindow = Window.Action;
        }

        //bring up the condition page
        private void ConditionButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = ConditionButton.Height;
            ActivePanel.Top = ConditionButton.Top;
            conditionsWindow.BringToFront();
            CurrenWindow = Window.Condition;
        }

        //bring up the expressions page
        private void ExpressionButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = ExpressionButton.Height;
            ActivePanel.Top = ExpressionButton.Top;
            expressionsWindow.BringToFront();
            CurrenWindow = Window.Expression;
        }

        //brings up the language page
        private void LanguageButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = LanguageButton.Height;
            ActivePanel.Top = LanguageButton.Top;
            languageWindow.BringToFront();
            CurrenWindow = Window.Language;
        }

        //bring up the test page
        private void TestButton_Click(object sender, EventArgs e)
        {
            //TODO: check out edge.js to run Node Processes in .NET (https://github.com/tjanczuk/edge)
            ActivePanel.Height = TestButton.Height;
            ActivePanel.Top = TestButton.Top;
            testWindow.BringToFront();
            CurrenWindow = Window.Test;
        }

        //bring up the export page
        private void ExportButton_Click(object sender, EventArgs e)
        {
            ActivePanel.Height = ExportButton.Height;
            ActivePanel.Top = ExportButton.Top;
            exportWindow.BringToFront();
            CurrenWindow = Window.Export;
        }

        //save button saves plugin data
        private void SaveButton_Click(object sender, EventArgs e)
        {
            //publish save event
            EventSystem.Insatnce.Hub.Publish(new SavePluginEvents(this));
            EventSystem.Insatnce.Hub.Publish(new UpdatePluginEvents(this, Global.Insatnce.CurrentPlugin));

            //save current plugin
            PluginRepository.Upsert(Global.Insatnce.CurrentPlugin);
        }

        //compiles all templates for the plugin
        private void CompileButton_Click(object sender, EventArgs e)
        {
            //publish save event
            EventSystem.Insatnce.Hub.Publish(new CompilePluginEvents(this, CurrenWindow));
        }
    }
}
