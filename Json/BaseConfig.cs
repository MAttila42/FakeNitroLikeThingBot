using System.IO;
using System.Text.Json;

namespace FNLTB.Json
{
    /// <summary>
    /// Necessary config for the Bot. Without these, it won't run.
    /// <para>Token, BotID, BotTerminals, Backups</para>
    /// </summary>
    class BaseConfig
    {
        public string Token { get; set; }
        public ulong BotID { get; set; }
        public ulong OwnerID { get; set; }
        public ulong[] Terminals { get; set; }
        public ulong[] Backups { get; set; }

        /// <summary>
        /// Get the values.
        /// </summary>
        /// <returns></returns>
        public static BaseConfig GetConfig()
        {
            return JsonSerializer.Deserialize<BaseConfig>(File.ReadAllText("BaseConfig.json"));
        }
    }
}
