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
            commands.Add(new Command("Restart", Restart.Aliases, Restart.Description, Restart.Usages, Restart.Permission));
            commands.Add(new Command("Test", Test.Aliases, Test.Description, Test.Usages, Test.Permission));

            var settings = Settings.PullData();
            string content = "";
            switch (m.Length)
            {
                case 1:
                    content += $"For more information use `{settings[settings.IndexOf(settings.Find(x => x.ServerID == ((SocketGuildChannel)message.Channel).Guild.Id))].Prefix}commands <command>`.";
                    foreach (var command in commands)
                        content += $"\n\n**{command.Name}**\n*{command.Description}*";
                    content += "\n\nSource code on [Github](https://github.com/ExAtom/FakeNitroLikeThingBot).";

                    var embed = new EmbedBuilder()
                        .WithAuthor(author =>
                        {
                            author
                                .WithName("Commands")
                                .WithIconUrl("https://cdn.discordapp.com/attachments/782305154342322226/801852346350174208/noun_Information_405516.png"); // Information by Viktor Ostrovsky from the Noun Project
                        })
                        .WithDescription(content)
                        .WithFooter(((SocketGuildChannel)message.Channel).Guild.Name)
                        .WithColor(new Color(0x7289DA)).Build();
                    try { await message.Channel.SendMessageAsync(null, embed: embed); }
                    catch (Exception) { }
                    break;
                case 2:
                    var foundCommands = commands.Where(x => x.Aliases.Contains(m[1].ToLower())).ToList();

                    foreach (var command in foundCommands)
                    {
                        content = $"{command.Description}\n" +
                            $"{command.Permission}\n\n" +
                            $"Usage{(command.Usages.Length > 1 ? "s:\n" : ": ")}";
                        foreach (var usage in command.Usages)
                            content += $"`{usage}`\n";
                        content += $"\nAliases: ";
                        for (int j = 0; j < command.Aliases.Count(); j++)
                            content += $"`{command.Aliases[j]}`{(j < command.Aliases.Count() - 1 ? ", " : "\n\n")}";

                        var embed2 = new EmbedBuilder()
                            .WithAuthor(author =>
                            {
                                author
                                    .WithName(command.Name)
                                    .WithIconUrl("https://cdn.discordapp.com/attachments/782305154342322226/801852346350174208/noun_Information_405516.png"); // Information by Viktor Ostrovsky from the Noun Project
                            })
                            .WithDescription(content)
                            .WithFooter(((SocketGuildChannel)message.Channel).Guild.Name)
                            .WithColor(new Color(0x7289DA)).Build();
                        try { await message.Channel.SendMessageAsync(null, embed: embed2); }
                        catch (Exception) { }
                    }
                    break;

                default:
                    await message.Channel.SendMessageAsync("❌ Too many parameters!");
                    return;
            }
        }
    }
}
