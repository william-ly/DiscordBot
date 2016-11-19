using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Modules;
using DiscordBot.General;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args) => new Program().Start();
        private DiscordClient _client;
        
        public void Start()
        {
            _client = new DiscordClient();
            DateTime start = new DateTime();
            start = DateTime.Now;
            Console.WriteLine("BOTmoderator!");

            //Add command service
            _client.UsingCommands(x => {
                x.PrefixChar = '^';
                x.HelpMode = HelpMode.Public;
            });

            //Add module service
            var modules = _client.AddService<ModuleService>(new ModuleService());
            modules.Add(new GeneralModule(), "General", ModuleFilter.None);
            Console.WriteLine("Modules added");

            _client.MessageReceived += async (s, e) =>
            {
                if (e.Message.IsAuthor) return;

                Console.WriteLine(e.User.Name + " has sent a message in channel "+ e.Channel.Name + " || Content:"+ e.Message.Text);
                
                if (e.Message.Text == "!help")
                {
                    var commandList = new StringBuilder();
                    commandList.AppendLine("```");
                    commandList.AppendLine("!help: Bot help. You're using it right now!");
                    commandList.AppendLine("!uptime: How long the bot has been online");
                    commandList.AppendLine("^server: Server details");
                    commandList.AppendLine("^channel [name]: Channel details for channel [name]; default channel if none specified");
                    commandList.AppendLine("^info [user]: Details for named [user]; details of command user if none specified");
                    commandList.AppendLine("^coinflip: Flips a coin (heads/tails)");
                    commandList.AppendLine("^dice [x]/[xdy]: Rolls x 6-sided dice | Rolls x dice with y sides each | Rolls a die if no input specified");
                    commandList.AppendLine("^rps [rock/paper/scissors]: Plays rock-paper-scissors with the bot.");
                    commandList.AppendLine("^choose [choice 1] [choice 2] [...] : Bot chooses from a list separated by spaces");
                    commandList.AppendLine("^8ball [question]: 8ball answer to a question");
                    commandList.AppendLine("```");
                    await e.Channel.SendMessage(e.User.Mention + "\n" + commandList.ToString());
                }

                if (e.Message.Text == "!uptime")
                {
                    DateTime current = DateTime.Now;
                    TimeSpan elapsed = current - start;
                    await e.Channel.SendMessage("```BOTmoderator has been online for " + elapsed.Days + " Days " + elapsed.Hours + " Hours " + elapsed.Minutes + " Minutes " + elapsed.Seconds + " Seconds.```");
                }
            };

            _client.UserJoined += async (s, e) =>
            {
                await e.Server.DefaultChannel.SendMessage(e.User.Mention + " joined the server! Welcome!");
                Console.WriteLine(e.User.Name + " joined the server");
            };

            _client.UserLeft += async (s, e) =>
            {
                await e.Server.DefaultChannel.SendMessage(e.User.Mention + " left the server!");
                Console.WriteLine(e.User.Name + " left the server");
            };

            _client.ChannelCreated += async (s, e) =>
            {
                if (e.Channel.Type == ChannelType.Text)
                {
                    await e.Channel.SendMessage("New text channel has been created!");
                }
                else
                    await e.Channel.SendMessage("New voice channel has been created!");
            };

            _client.ExecuteAndWait(async () => {
                await _client.Connect("MjIyODI0MTAyNzY1OTIwMjU4.CxFZaA.idGgHMrPTgyRco37PDDSVlzPDR0", TokenType.Bot);
                _client.SetGame("!help for help", GameType.Twitch, "http://google.ca");
            });


        }
    }
}
