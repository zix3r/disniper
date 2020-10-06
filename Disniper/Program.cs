using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web.ModelBinding;
using Discord;
using Discord.Gateway;
using Console = Colorful.Console;

namespace MessageLogger
{
    class Program
    {

        public static DiscordSocketClient mainacc = new DiscordSocketClient();
        static void Main(string[] args)
        {
            DiscordSocketClient client = new DiscordSocketClient();
            client.OnMessageReceived += OnMessageReceived;

            string path = AppDomain.CurrentDomain.BaseDirectory + "tokens.txt";

            if (File.Exists(path))
            {
                string[] tokens = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "tokens.txt");

                if (tokens == null || tokens.Length < 1)
                {
                    Console.WriteLine("Please enter the tokens in tokens.txt", Color.Cyan);
                    Console.WriteLine("Press enter to exit...");
                    Console.ReadLine();
                }
                else
                {
                    try
                    {
                        client.Login(tokens[1]);
                        mainacc.Login(tokens[0]);
                    }
                    catch
                    {
                        Console.WriteLine("Please enter valid tokens in tokens.txt", Color.Cyan);
                        Console.WriteLine("Press enter to exit...");
                        Console.ReadLine();
                    }

                    Console.Write(@"████████▄   ▄█     ▄████████ ███▄▄▄▄    ▄█     ▄███████▄    ▄████████    ▄████████ 
███   ▀███ ███    ███    ███ ███▀▀▀██▄ ███    ███    ███   ███    ███   ███    ███ 
███    ███ ███▌   ███    █▀  ███   ███ ███▌   ███    ███   ███    █▀    ███    ███ 
███    ███ ███▌   ███        ███   ███ ███▌   ███    ███  ▄███▄▄▄      ▄███▄▄▄▄██▀ 
███    ███ ███▌ ▀███████████ ███   ███ ███▌ ▀█████████▀  ▀▀███▀▀▀     ▀▀███▀▀▀▀▀   
███    ███ ███           ███ ███   ███ ███    ███          ███    █▄  ▀███████████ 
███   ▄███ ███     ▄█    ███ ███   ███ ███    ███          ███    ███   ███    ███ 
████████▀  █▀    ▄████████▀   ▀█   █▀  █▀    ▄████▀        ██████████   ███    ███ 
                                                                        ███    ███ ", Color.Red);
                    Console.WriteLine();

                    Thread.Sleep(-1);
                }
            } else
            {
                File.Create(path);
                Console.WriteLine("Please enter the tokens in tokens.txt", Color.Cyan);
                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
            }
        } 
        private static string redeemcode(string result, Stopwatch timer)
        {
            timer.Start();
            if (result.Contains("https://discord.gift/"))
            {
                result = result.Replace("https://discord.gift/", "");

            }
            else if (result.Contains("https://discord.com/gifts/"))
            {
                result = result.Replace("https://discord.com/gifts/", "");
            }
            string rstatus;
            try
            {
                mainacc.RedeemGift(result);
                timer.Stop();

                rstatus = "REDEEMED";
            }
            catch
            {
                try
                {
                    if (mainacc.GetNitroGift(result).Consumed == true)
                    {
                        rstatus = "ALREADY REDEEMED";
                    }
                    else
                    {
                        rstatus = "ERROR REDEEMING";
                    }
                }
                catch
                {
                    rstatus = "UNKNOWN ERROR";
                }
            }
            return rstatus;
        }
        private static void OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            Stopwatch timer = new Stopwatch();
            string message = args.Message.Content.ToString();
            if (message.Contains("https://discord.gift/") || message.Contains("https://discord.com/gifts/"))
            {
                string status = redeemcode(message, timer);
                string time = "0." + timer.ElapsedMilliseconds.ToString();

                Console.Write(@"[", Color.Cyan);
                Console.Write(time, Color.Yellow);
                Console.Write(@"] ", Color.Cyan);
                Console.Write(@"Link: ", Color.Orange);
                Console.Write(message);
                Console.Write(" [", Color.Cyan);
                Console.Write(status, Color.Yellow);
                Console.WriteLine("]", Color.Cyan);
            }
        }
    }
}