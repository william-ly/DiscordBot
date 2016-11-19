# DiscordBot
A bot for Discord written in C#.
Requires Discord.Net v0.9.4

## Commands
Below are the commands for basic functionality of the bot (make sure the bot is online!)  
### Basic
| Command  | Description |
| ------------- | ------------- |
| `!help`  | Describes all of the functions of the bot    |
| `!uptime`  |Returns the uptime of the bot in (Days:Hours:Minutes:Seconds)    |

### General Module
| First Header  | Second Header |
| ------------- | ------------- |
| `^server`  | Server details  |
| `^channel [name]`  |  Channel details for channel [name]; default channel if none specified    |
| `^info [user]` | Details for named [user]; details of command user if none specified    |
| `^coinflip`  | Flips a coin (heads/tails)  |
| `^dice [x]/[xdy]`  | Rolls x 6-sided dice | Rolls x dice with y sides each | Rolls a die if no input specified  |
| `^rps [rock/paper/scissors]` | Plays rock-paper-scissors with the bot  |
| `^choose \[choice 1] \[choice 2] \[...] `  | Bot chooses from a list separated by spaces  |
| `^8ball [question]`  |  8ball answer to a question  |
