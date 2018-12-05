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
using Syncfusion.Windows.Forms.Edit;
using Syncfusion.Windows.Forms.Edit.Enums;

namespace c3IDE
{
    public partial class TypeWindow : UserControl
    {
        public TypeWindow()
        {
            InitializeComponent();

            //TODO: check if sync fusion has EMCA5 version of js syntax highlighting
            //configure syntax highlighting
            editTimeEditor.ApplyConfiguration(KnownLanguages.JScript);
            runTimeEditor.ApplyConfiguration(KnownLanguages.JScript);
            //TODO: might need new syntax for templates 
            editTimeTemplateEditor.ApplyConfiguration(KnownLanguages.JScript);
            runTimeTemplateEditor.ApplyConfiguration(KnownLanguages.JScript);

            EventSystem.Insatnce.Hub.Subscribe<UpdatePluginEvents>(UpdatePluginEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<SavePluginEvents>(SavePluginEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<CompilePluginEvents>(CompilePluginEventHandler);
        }

        private void CompilePluginEventHandler(CompilePluginEvents obj)
        {
            throw new NotImplementedException();
        }

        private void SavePluginEventHandler(SavePluginEvents obj)
        {
            throw new NotImplementedException();
        }

        private void UpdatePluginEventHandler(UpdatePluginEvents obj)
        {
            throw new NotImplementedException();
        }

        //bind all editors to overwrite ctrl s for saving
        private void editor_RegisteringDefaultKeyBindings(object sender, EventArgs e)
        {
            ((EditControl)sender).KeyBinder.BindToCommand(Keys.Control | Keys.S, "None");
            //TODO: implement custom save method here
        }
    }
}
