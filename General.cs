using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Modules;

namespace DiscordBot.General
{
    internal class GeneralModule:IModule
    {
        public void Install(ModuleManager manager)
        {
            manager.CreateCommands("", cgb =>
            {
                cgb.CreateCommand("channel")
                    .Description("Channel details")
                    .Parameter("channel",ParameterType.Optional)
                    .Do(async e =>
                    {
                        var chanText = e.GetArg("channel")?.Trim();
                        // default to current ch or get first instance of channel
                        var channel = string.IsNullOrWhiteSpace(chanText) ? e.Channel : e.Server.FindChannels(chanText).FirstOrDefault();
                        // not found
                        if (channel == null)
                            return;
                        var channelString = new StringBuilder();
                        channelString.AppendLine($"```xl\n [Channel Name]:{channel.Name}");
                        channelString.AppendLine($" [Channel Id]:{channel.Id}");
                        channelString.AppendLine($" [Channel Type]:{channel.Type}```");
                        await e.Channel.SendMessage(channelString.ToString());
                    });

                cgb.CreateCommand("server")
                    .Description("Server details")
                    .Do(async e =>
                    {
                        var server = e.Server;
                        var serverString = new StringBuilder();
                        serverString.AppendLine($"```xl\n [Server Name]:{server.Name}");
                        serverString.AppendLine($" [Server Id]:{server.Id}");
                        serverString.AppendLine($" [Server Owner]:{server.Owner}");
                        serverString.AppendLine($" [Text Channels]:{server.TextChannels.Count()}");
                        serverString.AppendLine($" [Voice Channels]:{server.VoiceChannels.Count()}");
                        serverString.Append($" [User Count]:{server.UserCount}");
                        serverString.AppendLine($" [Users Online]:{ server.Users.Count(u => u.Status == UserStatus.Online)}");
                        serverString.AppendLine($" [Role Count]:{server.Roles.Count()}");
                        serverString.AppendLine($" [Server Region]:{server.Region.Name} \n```");
                        await e.Channel.SendMessage(serverString.ToString());
                    });

                cgb.CreateCommand("info")
                    .Description("User info details")
                    .Parameter("user", ParameterType.Optional)
                    .Do(async e =>
                    {
                        var userText = e.GetArg("user")?.Trim();
                        // default to current user or get first instance of username
                        var user = string.IsNullOrWhiteSpace(userText) ? e.User : e.Server.FindUsers(userText).FirstOrDefault();
                        // not found
                        if (user == null)
                            return;
                        var userString = new StringBuilder();
                        userString.AppendLine($"```xl\n [Name]: {user.Name}#{user.Discriminator}");
                        userString.AppendLine($" [Id]: #{user.Id} ");
                        userString.AppendLine($" [Status]: {user.Status}");
                        userString.AppendLine($" [Join Date]: {user.JoinedAt}");
                        userString.AppendLine($" [Last Online]: {user.LastOnlineAt:HH:mm:ss} \n```");
                        await e.Channel.SendMessage(userString.ToString());
                    });

                cgb.CreateCommand("coinflip")
                    .Description("Flips a coin")
                    .Do(async e =>
                    {
                        Random rng = new Random();
                        var result = rng.Next(0, 2) == 0 ? "Heads" : "Tails" ;
                        await e.Channel.SendMessage($"You flipped {result}");
                    });

                cgb.CreateCommand("dice")
                    .Description("Rolls a die or multiple dice")
                    .Parameter("dice",ParameterType.Unparsed)
                    .Do(async e =>
                    {
                        var diceText = e.GetArg("dice");
                        Random rng = new Random();

                        if (string.IsNullOrWhiteSpace(diceText))
                        {
                            await e.Channel.SendMessage("You rolled a die. Result: \n``` Rolled a "+ rng.Next(1, 7) + "```");
                            return;
                        }

                        int numDice, numSides;
                        var dieCountSides = diceText.Split('d');

                        if (int.TryParse(diceText, out numDice) && numDice >= 1 && numDice <= 15)
                        {
                            var output = new StringBuilder();
                            output.AppendLine($"You rolled {numDice} dice. Results: \n```");
                            for (int i = 1; i <= numDice; i++)
                            {
                                output.AppendLine($"Die number {i}: Rolled a {rng.Next(1, 7)}");
                            }
                            await e.Channel.SendMessage(output.ToString() + "```");
                            return;
                        }

                        if (dieCountSides.Length == 2)
                        {
                            //parse the input (format xdy: x dice with y sides each)
                            if (int.TryParse(dieCountSides[0], out numDice) && numDice >= 1 && numDice <= 15 && int.TryParse(dieCountSides[1], out numSides) && numSides >= 1)
                            {
                                var output = new StringBuilder();
                                output.AppendLine($"You rolled {numDice} dice with {numSides} sides each. Results: \n```");
                                for (int i = 1; i <= numDice; i++)
                                {
                                    output.AppendLine($"Die number {i}: Rolled a {rng.Next(1, numSides + 1)}");
                                }
                                await e.Channel.SendMessage(output.ToString()+"```");
                                return;
                            }
                            
                        }
                        await e.Channel.SendMessage("Valid dice format: ``` ^dice to roll one die \n ^dice x to roll x dice \n ^dice xdy to roll x dice with y sides each.```");
                    });

                cgb.CreateCommand("rps")
                    .Description("Plays rock paper scissors")
                    .Parameter("move", ParameterType.Unparsed)
                    .Do(async e =>
                    {
                        var userMove = e.GetArg("move")?.Trim();
                        userMove = userMove.ToLower();

                        if (string.IsNullOrWhiteSpace(userMove))
                        {
                            await e.Channel.SendMessage("Please enter a valid move: `^rps rock/paper/scissors`");
                            return;
                        }

                        Random rng = new Random();
                        int cpuM = rng.Next(0, 3);
                        string cpuMove;
                        if (cpuM == 0)
                            cpuMove = "rock";
                        else if (cpuM == 1)
                            cpuMove = "paper";
                        else
                            cpuMove = "scissors";

                        if (userMove == "rock" && cpuMove == "paper")
                        {
                            await e.Channel.SendMessage($"I chose {cpuMove}; {cpuMove} beats {userMove}. I win!");
                        }
                        else if (userMove == "rock" && cpuMove == "scissors")
                        {
                            await e.Channel.SendMessage($"I chose {cpuMove}; {userMove} beats {cpuMove}. You win!");
                        }
                        else if (userMove == "paper" && cpuMove == "rock")
                        {
                            await e.Channel.SendMessage($"I chose {cpuMove}; {userMove} beats {cpuMove}. You win!");
                        }
                        else if (userMove == "paper" && cpuMove == "scissors")
                        {
                            await e.Channel.SendMessage($"I chose {cpuMove}; {cpuMove} beats {userMove}. I win!");
                        }
                        else if (userMove == "scissors" && cpuMove == "rock")
                        {
                            await e.Channel.SendMessage($"I chose {cpuMove}; {cpuMove} beats {userMove}. I win!");
                        }
                        else if (userMove == "scissors" && cpuMove == "paper")
                        {
                            await e.Channel.SendMessage($"I chose {cpuMove}; {userMove} beats {cpuMove}. You win!");
                        }
                        else if (userMove.Equals(cpuMove))
                        {
                            await e.Channel.SendMessage($"I chose {cpuMove}. Both of us chose {userMove}, it's a tie!");
                        }
                        else
                            await e.Channel.SendMessage("Please enter a valid move: `^rps rock/paper/scissors`");
                    });

                cgb.CreateCommand("choose")
                    .Description("Chooses from a list separated by spaces")
                    .Parameter("list",ParameterType.Unparsed)
                    .Do(async e =>
                    {
                        var args = e.GetArg("list");
                        if (string.IsNullOrWhiteSpace(args))
                            return;
                        var choices = args.Split(' ');

                        if (choices.Count() <= 1)
                            return;
                        Random rng = new Random();
                        await e.Channel.SendMessage("I think this is a good idea: " + choices[rng.Next(0, choices.Length)]);
                    });

                cgb.CreateCommand("8ball")
                    .Description("Gives an 8ball-esque answer to a question")
                    .Parameter("question", ParameterType.Unparsed)
                    .Do(async e =>
                    {
                        string[] answers = new string[] {
                            "It is certain",
                            "It is decidedly so",
                            "Without a doubt",
                            "Yes, definitely",
                            "You may rely on it",
                            "As I see it, yes",
                            "Most likely",
                            "Outlook good",
                            "Yes",
                            "Signs point to yes",
                            "Reply hazy try again",
                            "Ask again later",
                            "Better not tell you now",
                            "Cannot predict now",
                            "Concentrate and ask again",
                            "Don't count on it",
                            "My reply is no",
                            "My sources say no",
                            "Outlook not so good",
                            "Very doubtful"};
                        var question = e.GetArg("question")?.Trim();
                        if (string.IsNullOrWhiteSpace(question))
                            return;
                        Random rng = new Random();
                        await e.Channel.SendMessage($"Question: `{question}` \n Answer: `" + answers[rng.Next(0, answers.Length)] +"`");
                    });
            });
        }
    }
}
