using Discord.WebSocket;
using FNLTB.Json;

namespace FNLTB.Commands
{
    class Prefix
    {
        public static string[] Aliases =
        {
            "prefix",
            "changeprefix",
            "change-prefix",
            "setprefix",
            "set-prefix"
        };
        public static string Description = "Changes the prefix of the bot.";
        public static string[] Usages = { "prefix <new prefix>" };
        public static string Permission = "Only server admins can use it.";
        public async static void DoCommand()
        {
            await Program.Log();
            var message = Recieved.Message;
            string[] m = message.Content.Split();
            if (m.Length != 2)
                return;
            if (m[1].Length > 20)
                return;
            var settings = Settings.PullData();
            settings[settings.IndexOf(settings.Find(x => x.ServerID == ((SocketGuildChannel)message.Channel).Guild.Id))].Prefix = m[1];
            Settings.PushData(settings);
            await message.Channel.SendMessageAsync($"Prefix changed to `{m[1]}`.");
        }
    }
}
