using System;
using System.Diagnostics;

namespace FNLTB.Commands
{
    class Restart
    {
        public static string[] Aliases = { "restart" };
        public static string Description = "Restart or shutdown the bot. Updates from the Github repository.";
        public static string[] Usages = { "restart [option]" };
        public static string Permission = "Only the Bot's owner can use it.";

        public async static void DoCommand()
        {
            await Program.Log();
            var message = Recieved.Message;
            try
            {
                if (message.Content.Split()[1] == "exit")
                {
                    try { await message.Channel.SendMessageAsync("Shutting down..."); }
                    catch (Exception) { }
                    Environment.Exit(0);
                }
            }
            catch (Exception) { }
            try
            {
                try { await message.Channel.SendMessageAsync("Restarting bot... (This may take a few moments)"); }
                catch (Exception) { }
                string commands =
                    "cd ..\n" +
                    "git pull\n" +
                    "dotnet build -o build\n" +
                    "cd build\n" +
                    "dotnet FakeNitroLikeThingBot.dll";
                var process = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{commands}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                Process.Start(process);
                Environment.Exit(0);
            }
            catch (Exception)
            {
                try { await message.Channel.SendMessageAsync("❌ Can't find bash!"); }
                catch (Exception) { }
            }
        }
    }
}
