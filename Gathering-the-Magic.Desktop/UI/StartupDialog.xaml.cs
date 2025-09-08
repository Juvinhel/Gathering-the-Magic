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
    public partial class StartupDialog
    {
        public StartupDialog()
        {
            InitializeComponent();
        }

        private void startupDialog_Loaded(object _sender, RoutedEventArgs _e)
        {
            repositoryFolderHeader.FolderPath = Data.Config.Current.RepositoryFolderPath;
        }

        private void startAllButton_Click(object _sender, RoutedEventArgs _e)
        {
            StartUp.UI = UIMode.All;
            Close();
        }

        private void startWorkbenchButton_Click(object _sender, RoutedEventArgs _e)
        {
            StartUp.UI = UIMode.Workbench;

            Close();
        }

        private void startLibraryButton_Click(object _sender, RoutedEventArgs _e)
        {
            StartUp.UI = UIMode.Library;

            Close();
        }

        private void startupDialog_Closing(object _sender, RoutedEventArgs _e)
        {
            if (Config.Current.RepositoryFolderPath != repositoryFolderHeader.FolderPath)
            {
                Config.Current.RepositoryFolderPath = repositoryFolderHeader.FolderPath;
                Config.Save();
            }
        }
    }
}
