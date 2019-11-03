using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace ClusterSender.Actors
{
    public class AMediatorSender: ReceiveActor
    {
        public AMediatorSender()
        {
            Receive<string>(s =>
            {
                var mediator = DistributedPubSub.Get(Context.System).Mediator;
                mediator.Tell(new Put(Self));
                try
                {
                    DistributedPubSub.Get(Context.System).Mediator.Tell(new Send("/user/mediatedListener/", s));
                    Context.ActorSelection("akka.tcp://ClusterSys@localhost:4051/user/mediatedListener")
                        .Tell($"direct message to actor ref: {s}");
                    mediator.Tell(new Send("/user/mediatedSender", s));
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