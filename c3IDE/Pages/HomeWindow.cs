using System;
using System.Collections.Generic;
using System.Windows.Forms;
using c3IDE.EventCore;
using c3IDE.PluginModels;

namespace c3IDE.Pages
{
    public partial class HomeWindow : UserControl
    {
        public List<C3Plugin> PluginList;
        private List<HomeButton> _buttonList;

        public HomeWindow()
        {
            InitializeComponent();
            PluginList = new List<C3Plugin>();
            _buttonList = new List<HomeButton>();

            newSingleGlobalButton.BindClick(NewSingleGlobalPluginButton);

            EventSystem.Insatnce.Hub.Subscribe<UpdatePluginEvents>(UpdatePluginEventHandler);
            EventSystem.Insatnce.Hub.Subscribe<PluginListLoadedEvents>(PluginListLoadedEventHandler);
        }

        private void PluginListLoadedEventHandler(PluginListLoadedEvents obj)
        {
            PluginList = obj.PluginList;
            _buttonList = new List<HomeButton>();
            PopulateUI();
        }
            
        private void UpdatePluginEventHandler(UpdatePluginEvents obj)
        {
            if (PluginList.Contains(obj.PluginData))
            {
                //update existing plugin
                var index = PluginList.IndexOf(obj.PluginData);
                PluginList[index] = obj.PluginData;
                PopulateUI();
            }
            else
            {
                //add new plugin
                PluginList.Add(obj.PluginData);
                PopulateUI();
            }
        }

        private void NewSingleGlobalPluginButton()
        {
            EventSystem.Insatnce.Hub.Publish(new NewPluginEvents(this, PluginTypeEnum.SingleGlobal));
        }

        private void PopulateUI()
        {
            //remove old button
            foreach (var button in _buttonList)
            {
                homePluginContainer.Controls.RemoveByKey(button.Name);
            }

            //create new buttonList
            _buttonList.Clear();
            foreach (var c3Plugin in PluginList)
            {
                var button = new HomeButton();
                button.InitButton(c3Plugin);
                _buttonList.Add(button);
            }

            var index = 0;
            //add new buttons
            foreach (var button in _buttonList)
            {
                button.Tag = index;
                homePluginContainer.Controls.Add(button);
                index++;
            }
        }
    }
}
    