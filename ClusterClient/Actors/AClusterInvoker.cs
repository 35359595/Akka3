using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Event;

namespace ClusterClient.Actors
{
    public class AClusterInvoker: ReceiveActor
    {
	    private readonly ILoggingAdapter _logger = Context.GetLogger();
        public AClusterInvoker()
        {
            DistributedPubSub.Get(Context.System).Mediator.Tell(new Put(Self));
            Receive<string>(s =>
            {
                _logger.Info($"passing back {s}");
                Context.System.ActorSelection("/user/singletonProxy").Tell(s);
            });
        }
    }
}