using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Discord.Webhook;

namespace FNLTB.Events
{
    class Emojify
    {
        public async static void DoEvent()
        {
            var message = Recieved.Message;
            string[] m = message.Content.Split(':');

            if (m.Length < 3)
                return;

            string output = message.Content;
            bool containsEmote = false;
            Emote emote;
            ulong emoteId = 0;
            var emotes = new List<KeyValuePair<string, Emote>>();
            foreach (var i in m)
            {
                try
                {
                    if (emoteId != 0 && ulong.Parse(i.Substring(0, 18)) != 0)
                        return;
                }
                catch (Exception) { }
                emote = Program._client.Guilds.SelectMany(x => x.Emotes).FirstOrDefault(x => x.Name == i);
                try { emoteId = emote.Id; }
                catch (Exception) { emoteId = 0; }
                if (emote != null)
                {
                    if (emotes.Count(x => x.Key == i) < 1)
                        emotes.Add(new KeyValuePair<string, Emote>(i, emote));
                    containsEmote = true;
                }
            }

            if (containsEmote)
            {
                foreach (var i in emotes)
                    output = output.Replace($":{i.Key}:", i.Value.ToString());
                var webhooks = await ((ITextChannel)message.Channel).GetWebhooksAsync();
                IWebhook webhook;
                try { webhook = webhooks.First(); }
                catch (Exception) { webhook = await ((ITextChannel)message.Channel).CreateWebhookAsync("emoji"); }
                try
                {
                    var webhookClient = new DiscordWebhookClient(webhook);
                    await webhookClient.SendMessageAsync(output, username: ((SocketGuildUser)message.Author).Nickname == null ? message.Author.Username : ((SocketGuildUser)message.Author).Nickname, avatarUrl: message.Author.GetAvatarUrl() == null ? message.Author.GetDefaultAvatarUrl() : message.Author.GetAvatarUrl(), allowedMentions: AllowedMentions.None);
                    await message.Channel.DeleteMessageAsync(message);
                }
                catch (Exception) { }
            }
        }
    }
}
