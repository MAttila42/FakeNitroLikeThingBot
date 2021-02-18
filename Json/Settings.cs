using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FNLTB.Json
{
    /// <summary>
    /// Main databes for the settings of the servers.
    /// <para>ServerID, BotChannels</para>
    /// </summary>
    class Settings
    {
        public ulong ServerID { get; set; }
        public string Prefix { get; set; }
        public List<ulong> BotChannels { get; set; }

        /// <summary>
        /// Pulling server settings from the JSON.
        /// </summary>
        /// <returns></returns>
        public static List<Settings> PullData()
        {
            try { return JsonSerializer.Deserialize<List<Settings>>(File.ReadAllText("Settings.json")); }
            catch (Exception) { File.WriteAllText("Settings.json", "[]"); }
            return JsonSerializer.Deserialize<List<Settings>>(File.ReadAllText("Settings.json"));
        }
        /// <summary>
        /// Pushing server settings to the JSON.
        /// </summary>
        /// <param name="list"></param>
        public static void PushData(List<Settings> list)
        {
            File.WriteAllText("Settings.json", JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
        }

        /// <summary>
        /// Use the other constructor!
        /// </summary>
        public Settings() { }

        /// <summary>
        /// Basic constructor that only needs the server ID.
        /// </summary>
        /// <param name="id"></param>
        public Settings(ulong id)
        {
            ServerID = id;
            Prefix = "e!";
            BotChannels = new List<ulong>();
        }
    }
}
