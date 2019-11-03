using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ClusterSender
{
	internal static class Program
	{
		private static async Task Main()
		{
			Console.WriteLine("Starting sender system...");
			var system = ActorSystem.Create("ClusterSys", ConfigurationFactory.ParseString(File.ReadAllText("Akka.hocon")));
			Console.ReadKey();

			var message = "initial message";
			var mediator = DistributedPubSub.Get(system).Mediator;

			while (!message.Equals("Stop"))
			{
				Console.WriteLine("New message:");
				message = Console.ReadLine();

				mediator.Tell(new Send("/user/ping", message, false));
			}

			Console.WriteLine("Terminating system.");
			await system.Terminate();
			Console.WriteLine("Bye...");
		}
	}
}