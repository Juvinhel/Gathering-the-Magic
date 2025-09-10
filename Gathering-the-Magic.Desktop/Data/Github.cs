﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gathering_the_Magic.DeckEdit.Data
{
    static public class Github
    {
        static public void Init()
        {
            if (Client == null)
            {
                Client = new HttpClient();
                Client.DefaultRequestHeaders.Add("User-Agent", "Gathering-the-Magic.Desktop.Updater");
                Client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            }
        }

        static private string githubUser = "Juvinhel";
        static private string githubRepo = "Gathering-the-Magic.Web";
        static public HttpClient Client { get; set; }

        static public async Task<ReleaseInfo> GetLatestRelease()
        {
            string url = $"https://api.github.com/repos/{githubUser}/{githubRepo}/releases/latest";
            using HttpResponseMessage response = await Client.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            return ReleaseInfo.Parse(content);
        }
    }
}