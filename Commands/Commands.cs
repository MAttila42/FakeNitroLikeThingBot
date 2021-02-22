using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using FNLTB.Json;

namespace FNLTB.Commands
{
    class Command
    {
        public string Name { get; set; }
        public string[] Aliases { get; set; }
        public string Description { get; set; }
        public string[] Usages { get; set; }
        public string Permission { get; set; }

        public Command(string name, string[] aliases, string desc, string[] usages, string perm)
        {
            Name = name;
            Aliases = aliases;
            Description = desc;
            Usages = usages;
            Permission = perm;
        }
    }

    class Commands
    {
        public static string[] Aliases =
        {
            "commands",
            "command",
            "parancsok",
            "parancs",
            "help",
            "segítség",
            "segitseg"
        };
        public static string Description = "Shows the list of commands or information about the one asked.";
        public static string[] Usages = { "commands [command]" };
        public static string Permission = "Anyone can use it.";

        public async static void DoCommand()
        {
            await Program.Log();

            var message = Recieved.Message;
            string[] m = message.Content.Split();

            var commands = new List<Command>();
            commands.Add(new Command("Commands", Commands.Aliases, Commands.Description, Commands.Usages, Commands.Permission));
            commands.Add(new Command("EmojiList", EmojiList.Aliases, EmojiList.Description, EmojiList.Usages, EmojiList.Permission));
            commands.Add(new Command("Ping", Ping.Aliases, Ping.Description, Ping.Usages, Ping.Permission));
            commands.Add(new Command("Prefix", Prefix.Aliases, Prefix.Description, Prefix.Usages, Prefix.Permission));
            commands.Add(new Command("Restart", Restart.Aliases, Restart.Description, Restart.Usages, Restart.Permission));
            commands.Add(new Command("Test", Test.Aliases, Test.Description, Test.Usages, Test.Permission));

            var settings = Settings.PullData();
            var embed = new EmbedBuilder()
                .WithAuthor(author => { author.WithIconUrl("https://cdn.discordapp.com/attachments/782305154342322226/801852346350174208/noun_Information_405516.png"); }) // Information by Viktor Ostrovsky from the Noun Project
                .WithFooter("Made by ExAtom")
                .WithColor(new Color(0x7289DA));
            switch (m.Length)
            {
                case 1:
                    embed.WithAuthor(author => { author.WithName("Commands"); });
                    embed.WithDescription($"For more information use `{settings[settings.IndexOf(settings.Find(x => x.ServerID == ((SocketGuildChannel)message.Channel).Guild.Id))].Prefix}commands <command>`.\nSource code on [Github](https://github.com/ExAtom/FakeNitroLikeThingBot).\nPing the bot for other help.");
                    foreach (var command in commands)
                        embed.AddField(command.Name, $"*{command.Description}*");
                    break;
                case 2:
                    var foundCommands = commands.Where(x => x.Aliases.Contains(m[1].ToLower())).ToList();
                    if (foundCommands.Count == 0)
                        return;

                    foreach (var command in foundCommands)
                    {
                        string content = $"{command.Description}\n" +
                            $"{command.Permission}\n\n" +
                            $"Usage{(command.Usages.Length > 1 ? "s:\n" : ": ")}";
                        foreach (var usage in command.Usages)
                            content += $"`{settings[settings.IndexOf(settings.Find(x => x.ServerID == ((SocketGuildChannel)message.Channel).Guild.Id))].Prefix}{usage}`\n";
                        content += $"\nAliases: ";
                        for (int j = 0; j < command.Aliases.Count(); j++)
                            content += $"`{command.Aliases[j]}`{(j < command.Aliases.Count() - 1 ? ", " : "\n\n")}";

                        embed.WithAuthor(author => { author.WithName(command.Name); })
                            .WithDescription(content);
                    }
                    break;

                default:
                    await message.Channel.SendMessageAsync("❌ Too many parameters!");
                    return;
            }
            try { await message.Channel.SendMessageAsync(null, embed: embed.Build()); }
            catch (Exception) { }
        }
    }
}
