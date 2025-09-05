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
using Gathering_the_Magic.DeckEdit.Data;

namespace Gathering_the_Magic.DeckEdit.UI
{
    /// <summary>
    /// Interaktionslogik für ConfigDialog.xaml
    /// </summary>
    public partial class ConfigDialog
    {
        public ConfigDialog()
        {
            InitializeComponent();
        }

        private void configDialog_Loaded(object _sender, RoutedEventArgs _e)
        {
            repositoryFolderHeader.FolderPath = Data.Config.Current.RepositoryFolderPath;
        }

        private void cancelButton_Click(object _sender, RoutedEventArgs _e)
        {
            Close();
        }

        private void okButton_Click(object _sender, RoutedEventArgs _e)
        {
            Config.Current.RepositoryFolderPath = repositoryFolderHeader.FolderPath;
            Config.Save();
            Close();
        }
    }
}
