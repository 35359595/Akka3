using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Event;

namespace ClusterSender.Actors
{
	public class AMediatorSender : ReceiveActor
	{
		public AMediatorSender()
		{
			Receive<string>(s =>
			{
				var mediator = DistributedPubSub.Get(Context.System).Mediator;
				mediator.Tell(new Put(Self));
				try
				{
					Context.GetLogger().Info("SEND");

					mediator.Tell(new Send("/user/ping", s));

					Sender.Tell(true);
				}
				catch
				{
					Sender.Tell(false);
				}
			});
		}
	}
}