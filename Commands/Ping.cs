using System;
using Discord;

namespace FNLTB.Commands
{
    class Ping
    {
        public static string[] Aliases =
        {
            "ping",
            "latency"
        };
        public static string Description = "Calculates the bot's ping.";
        public static string[] Usages = { "ping" };
        public static string Permission = "Anyone can use it.";

        public async static void DoCommand(bool isResponse)
        {
            var message = Recieved.Message;
            if (message.Content.Split().Length > 1)
                return;

            if (isResponse)
            {
                var latency = DateTime.Now - Recieved.PingTime;
                await ((IUserMessage)message).ModifyAsync(m => m.Content = $"Pong! `{latency.TotalMilliseconds:f0}ms`");
            }
            else
            {
                await Program.Log();
                Recieved.PingTime = DateTime.Now;
                try { await message.Channel.SendMessageAsync($"Pinging..."); }
                catch (Exception) { }
            }
        }
    }
}
