using System.Net.Http;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Gathering_the_Magic.DeckEdit.UI
{
    /// <summary>
    /// Interaktionslogik für UpdateSplash.xaml
    /// </summary>
    public partial class UpdateSplash
    {
        public UpdateSplash()
        {
            InitializeComponent();
        }

        private HttpClient webClient = new HttpClient();
        private async void updateSplash_Loaded(object _sender, RoutedEventArgs _e)
        {
            try
            {
                titleTextBlock.Text = "Update Info";

                Version localVersion = null;
                DateTime localBuildDate = default;
                string manifestFilePath = Path.Combine(StartUp.WebFolderPath, "manifest.json");
                if (File.Exists(manifestFilePath))
                {
                    string text = File.ReadAllText(manifestFilePath);
                    JObject manifest = JObject.Parse(text);
                    localVersion = Version.Parse(manifest.Property("version").Value.Value<string>());
                    localBuildDate = manifest.Property("build-date").Value.Value<DateTime>().ToLocalTime();
                }
                Version webVersion;
                DateTime webBuildDate;
                {
                    string webManifestUrl = StartUp.WebUrl.Append("manifest.json");
                    string text = await webClient.GetStringAsync(webManifestUrl);
                    JObject manifest = JObject.Parse(text);
                    webVersion = Version.Parse(manifest.Property("version").Value.Value<string>());
                    webBuildDate = manifest.Property("build-date").Value.Value<DateTime>().ToLocalTime();
                }

                oldVersionTextBlock.Text = localVersion == null ? "Not Installed" : $"Installed Version: v{localVersion} - {localBuildDate}";
                newVersionTextBlock.Text = $"Online Version: v{webVersion} - {webBuildDate}";

                if (localVersion == webVersion && localBuildDate == webBuildDate)
                {
                    Close();
                    return;
                }

                beginUpdateButton.Visibility = Visibility.Visible;
                if (localVersion != null) closeButton.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
                App.Current.Shutdown();
            }
        }

        private async void beginUpdateButton_Click(object _sender, RoutedEventArgs _e)
        {
            try
            {
                beginUpdateButton.Visibility = Visibility.Hidden;
                closeButton.Visibility = Visibility.Hidden;
                progressBar.Visibility = Visibility.Visible;
                progressTextBlock.Visibility = Visibility.Visible;

                Directory.Clear(StartUp.WebFolderPath);
                string sw = await webClient.GetStringAsync(StartUp.WebUrl.Append("sw.js"));
                string precacheAndRoute = "[" + Regex.Match(sw, "precacheAndRoute\\(\\[(?<manifest>.*?)\\]").Groups["manifest"].Value + "]";
                JArray routes = JArray.Parse(precacheAndRoute);

                List<string> fileUrls = routes.Select(x => (x as JObject).Property("url").Value.Value<string>()).ToList();
                titleTextBlock.Text = $"Downloading";
                progressBar.Value = 0;
                progressBar.Maximum = fileUrls.Count;
                progressTextBlock.Text = "0 %";

                foreach (string fileUrl in fileUrls)
                {
                    string relativePath = fileUrl.Replace("/", "\\");
                    string filePath = Path.Combine(StartUp.WebFolderPath, relativePath);
                    string url = StartUp.WebUrl.Append(fileUrl);
                    using Stream webStream = await webClient.GetStreamAsync(url);
                    using Stream fileStream = File.OpenCreate(filePath);
                    webStream.CopyTo(fileStream);

                    ++progressBar.Value;
                    progressTextBlock.Text = (progressBar.Value / progressBar.Maximum).ToString("0.00 %");
                }

                titleTextBlock.Text = "Update Done";
                closeButton.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
                App.Current.Shutdown();
            }
        }

        private void closeButton_Click(object _sender, RoutedEventArgs _e)
        {
            Close();
        }
    }
}