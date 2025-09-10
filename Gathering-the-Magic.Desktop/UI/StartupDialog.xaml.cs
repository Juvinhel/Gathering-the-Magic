using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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
            Github.Init();
        }

        private Version localVersion;
        private ReleaseInfo latestRelease;

        private async void startupDialog_Loaded(object _sender, RoutedEventArgs _e)
        {
            repositoryFolderHeader.FolderPath = Config.Current.RepositoryFolderPath;

            if (File.Exists(StartUp.VersionFilePath))
                localVersion = Version.Parse(File.ReadAllText(StartUp.VersionFilePath));

            latestRelease = await Github.GetLatestRelease();

            oldVersionTextBlock.Text = localVersion == null ? "Not Installed" : $"Installed Version: v{localVersion}";
            newVersionTextBlock.Text = $"Online Version: v{latestRelease.Version}";

            if (localVersion != null)
                startAppGrid.Visibility = Visibility.Visible;

            if (localVersion == null)
                startUpdateTextBlock.Text = "Install App";

            if (localVersion == latestRelease.Version)
                startUpdateTextBlock.Text = "Repair App";
        }

        private void startAllButton_Click(object _sender, RoutedEventArgs _e)
        {
            StartUp.UI = UIMode.All;
            MainWindow.Current.Start();
            Close();
        }

        private void startWorkbenchButton_Click(object _sender, RoutedEventArgs _e)
        {
            StartUp.UI = UIMode.Workbench;
            MainWindow.Current.Start();
            Close();
        }

        private void startLibraryButton_Click(object _sender, RoutedEventArgs _e)
        {
            StartUp.UI = UIMode.Library;
            MainWindow.Current.Start();
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

        private void startUpdateHyperLink_Click(object _sender, RoutedEventArgs _e)
        {
            Close();
            UpdateSplash updateSplash = new UpdateSplash(localVersion, latestRelease);
            updateSplash.Show();
        }
    }
}
