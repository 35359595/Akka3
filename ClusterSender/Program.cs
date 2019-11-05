using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Cluster.Tools.Singleton;

namespace ClusterSender
{
	internal static class Program
	{
		private static async Task Main()
		{
			Console.WriteLine("Starting sender system...");
			var system = ActorSystem.Create("ClusterSys", ConfigurationFactory.ParseString(File.ReadAllText("Akka.hocon")));
			Console.ReadKey();

			system.ActorOf(ClusterSingletonManager.Props(
				Props.Create<SingletonActor>(),
				PoisonPill.Instance,
				ClusterSingletonManagerSettings.Create(system).WithRole("b")
			), "single");

			system.ActorOf(ClusterSingletonProxy.Props("/user/single",
					ClusterSingletonProxySettings.Create(system).WithRole("b")),
				"singleProxy").Tell("Hello to singletone!");

			var message = "initial message";
			var mediator = DistributedPubSub.Get(system).Mediator;
			
			mediator.Tell(new Send("/user/invoker", "Remote hello to singleton!"));

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