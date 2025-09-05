using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Gathering_the_Magic.DeckEdit.UI;
using WinCopies.Util;

namespace Gathering_the_Magic.DeckEdit.Data
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    sealed public class Bridge
    {
        public SaveResult SaveDeck()
        {
            string filePath = MainWindow.Current.SaveDeck();
            if (string.IsNullOrEmpty(filePath)) return null;
            return new SaveResult(filePath);
        }

        public LoadResult LoadDeck()
        {
            string filePath = MainWindow.Current.LoadDeck();
            if (string.IsNullOrEmpty(filePath)) return null;
            return new LoadResult(filePath);
        }

        public LoadResult[] LoadCollections()
        {
            IEnumerable<string> filePaths = MainWindow.Current.LoadCollections();
            if (filePaths == null) return null;
            return filePaths.Select(filePath => new LoadResult(filePath)).ToArray();
        }

        public LoadResult[] LoadDefaultCollections()
        {
            string collectionsFolderPath = Path.Combine(Config.Current.RepositoryFolderPath, "Collections");
            if (!Directory.Exists(collectionsFolderPath)) return null;

            IEnumerable<string> filePaths = Directory.GetFiles(collectionsFolderPath, "csv");
            return filePaths.Select(filePath => new LoadResult(filePath)).ToArray();
        }

        private string configFilePath = Path.Combine(Directory.Current, "web.config.user");
        public string LoadConfig()
        {
            return File.Exists(configFilePath) ? File.ReadAllText(configFilePath) : null;
        }

        public void SaveConfig(string _text)
        {
            File.WriteAllText(configFilePath, _text);
        }
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class SaveResult
    {
        public SaveResult(string _filePath)
        {
            filePath = _filePath;
            Name = Path.GetFileNameWithoutExtension(filePath);
            Type = Path.GetExtension(filePath).TrimStart(".");
            Exists = File.Exists(filePath);
        }

        private string filePath;
        public string Name { get; private set; }
        public string Type { get; private set; }
        public bool Exists { get; private set; }

        public void Save(string _text)
        {
            File.WriteAllText(filePath, _text);
        }
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class LoadResult
    {
        public LoadResult(string _filePath)
        {
            filePath = _filePath;
            Name = Path.GetFileNameWithoutExtension(filePath);
            Type = Path.GetExtension(filePath).TrimStart(".").ToLower();
            LastModified = File.GetInfo(filePath).LastWriteTimeUtc.ToString("o");
        }

        private string filePath;
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string LastModified { get; private set; }

        public string Load()
        {
            return File.ReadAllText(filePath);
        }
    }
}