using System;
using Discord;
using FNLTB.Json;

namespace FNLTB.Events
{
    class Backup
    {
        public async static void DoEvent()
        {
            foreach (var id in BaseConfig.GetConfig().Backups)
                try
                {
                    await ((IMessageChannel)Program._client.GetChannel(id)).SendFileAsync(@"Settings.json");
                    await Program.Log($"Made a backup in <#{id}>");
                }
                catch (Exception) { }
        }
    }
}
