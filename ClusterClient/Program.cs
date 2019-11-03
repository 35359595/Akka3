using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using ClusterClient.Actors;

namespace ClusterClient
{
    internal static class Program
    {
        internal static async Task Main()
        {
            Console.WriteLine("Starting Akka System...");
            var system = ActorSystem.Create("ClusterSys",
                ConfigurationFactory.ParseString(File.ReadAllText("Akka.hocon")));
            system.ActorOf(Props.Create<AMediatorReceiver>(),
                "ping");
            Console.WriteLine("System started. Press Enter to terminate...");
            Console.ReadLine();
            await system.Terminate();
            Console.WriteLine("Cluster is terminated. Bye...");
        }
    }
}