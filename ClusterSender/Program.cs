using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using Akka.Routing;
using ClusterSender.Actors;

namespace ClusterSender
{
    internal static class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("Starting sender system...");
            var system = ActorSystem.Create("ClusterSys",
                ConfigurationFactory.ParseString(File.ReadAllText("Akka.hocon")));
            var sender = system.ActorOf(Props.Create<AMediatorSender>().WithRouter(FromConfig.Instance), 
                "mediatedSender");
            Console.ReadKey();
            var message = "initial message";
            while (!message.Equals("Stop"))
            {
                Console.WriteLine("New message:");
                message = Console.ReadLine();
//                Console.WriteLine(await sender.Ask<bool>(message) switch
//                    {
//                        true => "Sent ok",
//                        false => "Send failed"
//                    });
                DistributedPubSub.Get(system).Mediator.Tell(new Send("/user/ping", message, true));
            }
            Console.WriteLine("Terminating system.");
            await system.Terminate();
            Console.WriteLine("Bye...");
        }
    }
}