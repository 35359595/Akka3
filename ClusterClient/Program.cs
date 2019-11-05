using Akka.Actor;
using Akka.Configuration;
using ClusterClient.Actors;
using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Cluster.Tools.Singleton;
using Akka.Routing;

namespace ClusterClient
{
	internal static class Program
	{
		internal static async Task Main()
		{
			Console.WriteLine("Starting Akka System...");
			var system = ActorSystem.Create("ClusterSys", ConfigurationFactory.ParseString(File.ReadAllText("Akka.hocon")));

			system.ActorOf(Props.Create<AMediatorReceiver>(), "ping");
			system.ActorOf(Props.Create<AClusterInvoker>(), "invoker");
			
			// singleton proxy
			system.ActorOf(ClusterSingletonProxy.Props("/user/single",
				ClusterSingletonProxySettings.Create(system).WithRole("b")),
				"singletonProxy");

			Console.WriteLine("System started. Press Enter to terminate...");
			Console.ReadLine();

			await system.Terminate();

			Console.WriteLine("Cluster is terminated. Bye...");
		}
	}
}