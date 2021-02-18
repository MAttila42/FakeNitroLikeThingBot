using System;
using Discord;
using Discord.WebSocket;
using FNLTB.Json;

namespace FNLTB.Events
{
    class BotMention
    {
        public async static void DoEvent()
        {
            var message = Recieved.Message;
            ulong id = Program._client.CurrentUser.Id;
            var settings = Settings.PullData();
            if ((message.Content == $"<@!{id}>" || message.Content == $"<@{id}>") && Program.BotChannel())
            {
                string prefix = settings[settings.IndexOf(settings.Find(x => x.ServerID == ((SocketGuildChannel)message.Channel).Guild.Id))].Prefix;
                try
                {
                    await message.Channel.SendMessageAsync(null, embed: new EmbedBuilder()
                        .WithAuthor(author =>
                        {
                            author
                                .WithName(Program._client.CurrentUser.Username)
                                .WithIconUrl("https://cdn.discordapp.com/attachments/782305154342322226/801852346350174208/noun_Information_405516.png"); // Information by Viktor Ostrovsky from the Noun Project
                        })
                        .WithDescription($"Hi, I'm a simple bot that gives poor people a Nitro-like experience by letting them send emojis that they normally can't.\nTo see all my commands type `{prefix}commands`.")
                        .AddField("How it works", "You don't need any special command or permission. Just send you message as you would normally do, put an emoji in there (Like this: `:blobParty:`) and I will replace your message with a [Webhook](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks) that has you profile picture and nickname.")
                        .AddField("Links", "[Github](https://github.com/ExAtom/FakeNitroLikeThingBot) - Here's my full source code if that's what makes you happy... NERD.\n[Dev Server](https://discord.gg/u9pmswhmbc) - Invite to ExAtom's Playground where you can find all his projects or some help with them.\n[Invite](https://discord.com/api/oauth2/authorize?client_id=811628507490811961&permissions=537259072&scope=bot) - The link to invite me to your server.")
                        .WithThumbnailUrl(Program._client.CurrentUser.GetAvatarUrl())
                        .WithFooter("Made by ExAtom")
                        .WithColor(new Color(0x7289DA)).Build());
                }
                catch (Exception) { }
            }
        }
    }
}
