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
using c3IDE.Utilities.Search;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for SearchAndReplaceWindow.xaml
    /// </summary>
    public partial class SearchAndReplaceWindow : UserControl
    {
        private IEnumerable<SearchResult> FoundResults;

        public SearchAndReplaceWindow()
        {
            InitializeComponent();
        }

        public Action RestoreWindow { get; set; }

        public void PopulateSearchData(IEnumerable<SearchResult> results)
        {
            FoundResults = results;
            SearchGrid.DataContext = results;
            FindText.Text = Searcher.Insatnce.LastSearchedWord;
            FindText.IsReadOnly = true;
        }

        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            var tb = (sender as TextBox);
            tb?.SelectAll();
        }

        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }

        private void ReplaceAllSelected_Click(object sender, RoutedEventArgs e)
        {
            var replaceList = new List<SearchResult>();
            //replace all text
            foreach (var searchResult in FoundResults.Where(x => x.Selected))
            {
                searchResult.Line = searchResult.Line.Replace(FindText.Text, ReplaceText.Text);
                replaceList.Add(searchResult);
            }

            Searcher.Insatnce.GlobalReplace(AddonManager.CurrentAddon, replaceList);
            RestoreWindow();
        }

        private void SearchGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (SearchGrid.SelectedItem is SearchResult item)
            //{
            //    AppData.Insatnce.NavigateToWindow(item.Window);
            //}
        }
    }
}
