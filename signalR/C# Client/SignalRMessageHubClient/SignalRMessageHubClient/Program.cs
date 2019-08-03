using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace SignalRMessageHubClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Use your signalr app url here
            var connection = new HubConnectionBuilder()
                            .WithUrl("https://localhost:44391/messagehub")
                            .Build();

            connection.On<string, string>("SRMessage", (sender, message) => {
                Console.WriteLine($"{sender} says {message}");
            });
            await connection.StartAsync();
            await connection.InvokeAsync("SendMessage", $"C# Client", "C# Client Started..").ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine(task.Status);
                }
            });
            Console.Read();
            await connection.StopAsync();
        }
    }
}
