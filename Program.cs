using System;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using FNLTB.Commands;
using FNLTB.Events;
using FNLTB.Json;

namespace FNLTB
{
    public class Recieved
    {
        public static SocketMessage Message;
        public static DateTime PingTime;
    }

    class Program
    {
        public static DiscordSocketClient _client;
        static void Main() => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += MessageHandler;
            _client.Log += ClientLog;
            _client.Ready += Ready;
            var token = BaseConfig.GetConfig().Token;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private Task ClientLog(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task MessageHandler(SocketMessage message)
        {
            var settings = Settings.PullData();
            string prefix;
            try { prefix = settings[settings.IndexOf(settings.Find(x => x.ServerID == ((SocketGuildChannel)message.Channel).Guild.Id))].Prefix; }
            catch (Exception)
            {
                settings.Add(new Settings(((SocketGuildChannel)message.Channel).Guild.Id));
                prefix = settings[settings.IndexOf(settings.Find(x => x.ServerID == ((SocketGuildChannel)message.Channel).Guild.Id))].Prefix;
                Settings.PushData(settings);
            }
            string firstWord = message.Content.Split()[0];
            bool pong = message.Author.Id == BaseConfig.GetConfig().BotID && firstWord == "Pinging...";

            if (pong || (!message.Author.IsBot && !message.Author.IsWebhook))
                Recieved.Message = message;
            else
                return Task.CompletedTask;

            // Events
            BotMention.DoEvent();
            Emojify.DoEvent();

            if (pong)
                Ping.DoCommand(true);
            if (!message.Content.StartsWith(prefix) || message.Author.IsBot)
                return Task.CompletedTask;

            string command = firstWord.Substring(prefix.Length, firstWord.Length - prefix.Length).ToLower();

            // Commands
            if (Commands.Commands.Aliases.Contains(command) && BotChannel())
                Commands.Commands.DoCommand();
            if (EmojiList.Aliases.Contains(command) && BotChannel())
                EmojiList.DoCommand();
            if (Ping.Aliases.Contains(command) && BotChannel())
                Ping.DoCommand(false);
            if (Restart.Aliases.Contains(command) && BotChannel() && IsAdmin(true))
                Restart.DoCommand();
            if (Test.Aliases.Contains(command) && BotChannel() && IsAdmin(true))
                Test.DoCommand();

            return Task.CompletedTask;
        }

        private Task Ready()
        {
            HourlyEvents();
            return Task.CompletedTask;
        }

        private async static void HourlyEvents()
        {
            while (true)
            {
                Backup.DoEvent();
                await Task.Delay(3600000);
            }
        }

        /// <summary>
        /// Command run log in the console and specified channels.
        /// </summary>
        /// <returns></returns>
        public async static Task Log()
        {
            var message = Recieved.Message;
            Console.Write(DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss") + " ");
            string output = $"Command run - {message.Author.Username}#{message.Author.Discriminator} in #{message.Channel}: {message.Content}";
            foreach (var id in BaseConfig.GetConfig().Terminals)
                try { await ((IMessageChannel)_client.GetChannel(id)).SendMessageAsync(output, allowedMentions: AllowedMentions.None); }
                catch (Exception) { }
            Console.WriteLine(output);
        }
        /// <summary>
        /// Event log in the console and specified channels.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async static Task Log(string text)
        {
            var message = Recieved.Message;
            Console.Write(DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss") + " ");
            string output = $"Event - {text}";
            foreach (var id in BaseConfig.GetConfig().Terminals)
                try { await ((IMessageChannel)_client.GetChannel(id)).SendMessageAsync(output, allowedMentions: AllowedMentions.None); }
                catch (Exception) { }
            Console.WriteLine(output);
        }
        /// <summary>
        /// Checks if the user is an Admin.
        /// </summary>
        /// <param name="isOwner">Admin helyett tulajdonost ellenőriz.</param>
        /// <returns></returns>
        public static bool IsAdmin(bool isOwner)
        {
            foreach (var role in (Recieved.Message.Author as SocketGuildUser).Roles)
                if ((role.Permissions.Administrator && !isOwner) ||
                    BaseConfig.GetConfig().OwnerID == Recieved.Message.Author.Id)
                    return true;
            return false;
        }
        /// <summary>
        /// Checks if the command was sent in a bot channel.
        /// </summary>
        /// <returns></returns>
        public static bool BotChannel()
        {
            var message = Recieved.Message;
            var settings = Settings.PullData();
            var botChannels = settings[settings.IndexOf(settings.Find(x => x.ServerID == ((SocketGuildChannel)message.Channel).Guild.Id))].BotChannels;
            if (botChannels.Contains(message.Channel.Id) || botChannels.Count == 0)
                return true;
            return false;
        }
    }
}
