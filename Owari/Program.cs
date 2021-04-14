using Discord;
using Discord.WebSocket;
using DiscordUtils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Owari
{
    public class Program
    {
        private static DiscordSocketClient client;
        static async Task Main(string[] args)
        {
            client = client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
            });
            client.Log += Utils.Log;
            client.Ready += Ready;
            await client.LoginAsync(TokenType.Bot, File.ReadAllText("token.txt"));
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private static async Task Ready()
        {
            var guild = client.GetGuild(146701031227654144);
            var role = await guild.CreateRoleAsync("OwariArchive", null, null, false, null);
            foreach (var c in client.GetGuild(146701031227654144).Channels)
            {
                await c.ModifyAsync((x) =>
                {
                    x.PermissionOverwrites = new Overwrite[] {
                        new Overwrite(role.Id, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Allow))
                    };
                });
                await c.AddPermissionOverwriteAsync(guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            }
            Console.WriteLine("Done");
        }
    }
}
