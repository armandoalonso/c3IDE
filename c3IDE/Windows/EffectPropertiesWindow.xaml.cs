using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for EffectProperties.xaml
    /// </summary>
    public partial class EffectPropertiesWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Properties";

        /// <summary>
        /// effect property constructor
        /// </summary>
        public EffectPropertiesWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// handles when the effect property window get focus
        /// </summary>
        public void OnEnter()
        {
            if (AddonManager.CurrentAddon != null && AddonManager.CurrentAddon.Effect != null)
            {
                blendsBackground.Text = AddonManager.CurrentAddon.Effect.BlendsBackground ? "true" : "false";
                crossSampling.Text = AddonManager.CurrentAddon.Effect.CrossSampling ? "true" : "false";
                preservesOpaqueness.Text = AddonManager.CurrentAddon.Effect.PreservesOpaqueness ? "true" : "false";
                animated.Text = AddonManager.CurrentAddon.Effect.Animated ? "true" : "false";
                mustPreDraw.Text = AddonManager.CurrentAddon.Effect.MustPredraw ? "true" : "false";
                extendBoxH.Text = AddonManager.CurrentAddon.Effect.ExtendBoxHorizontal.ToString();
                extendBoxV.Text = AddonManager.CurrentAddon.Effect.ExtendBoxVertical.ToString();
                category.Text = AddonManager.CurrentAddon.AddonCategory;
            }
        }

        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null && AddonManager.CurrentAddon.Effect != null)
            {
                int.TryParse(extendBoxH.Text.Trim(), out var extH);
                int.TryParse(extendBoxV.Text.Trim(), out var extV);

                var effect = new Effect
                {
                    Animated = animated.Text == "true",
                    BlendsBackground = blendsBackground.Text == "true",
                    CrossSampling = crossSampling.Text == "true",
                    ExtendBoxVertical = extH,
                    ExtendBoxHorizontal = extV,
                    MustPredraw = mustPreDraw.Text == "true",
                    PreservesOpaqueness = preservesOpaqueness.Text == "true",
                    Parameters = AddonManager.CurrentAddon.Effect.Parameters,
                    Code = AddonManager.CurrentAddon.Effect.Code
                };

                AddonManager.CurrentAddon.Effect = effect;
                AddonManager.CurrentAddon.AddonCategory = category.Text;
                AddonManager.SaveCurrentAddon();
            }
        }

        public void Clear()
        {
            blendsBackground.Text = "false";
            crossSampling.Text = "false";
            preservesOpaqueness.Text = "false";
            animated.Text ="false";
            mustPreDraw.Text = "false";
            extendBoxH.Text = "0";
            extendBoxV.Text = "0";
        }

        public void ChangeTab(string tab, int lineNum)
        {

        }
    }
}
