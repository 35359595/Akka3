using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Event;

namespace ClusterClient.Actors
{
    public class AMediatorReceiver: ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public AMediatorReceiver()
        {
            var mediator = DistributedPubSub.Get(Context.System).Mediator;
            _logger.Info("Registering mediated receiver to mediator.");
            mediator.Tell(new Put(Self));
            _logger.Info("Receiver registered with mediator.");
            Receive<string>(message =>
            {
                _logger.Info($"Received {message}");
                mediator.Tell(new Send("/user/mediatedListener", message));
            });
        }
    }
}