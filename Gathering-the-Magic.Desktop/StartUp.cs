using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Gathering_the_Magic.DeckEdit.Data;
using Lemon;

namespace Gathering_the_Magic.DeckEdit
{
    static public class StartUp
    {
        [STAThread]
        static public void Main(string[] _args)
        {
            try
            {
                WebFolderPath = Path.MakeRooted(Path.Combine(Program.MyFolderPath, "web"));
                Directory.Create(WebFolderPath);
                VersionFilePath = Path.Combine(WebFolderPath, "version.txt");
                ReleaseNotesFilePath = Path.Combine(WebFolderPath, "release-notes.txt");

                Config.Load();

                App app = new App();
                app.InitializeComponent();
                app.Run();

                Config.Save();
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached) throw;
                else MessageBox.Show(ex.Message, ex.GetType().Name);
            }
        }

        static public string WebFolderPath;
        static public string VersionFilePath;
        static public string ReleaseNotesFilePath;
        static public Url WebUrl = "https://juvinhel.github.io/Gathering-the-Magic";
    }
}
