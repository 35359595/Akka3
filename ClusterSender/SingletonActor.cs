using Akka.Actor;
using Akka.Event;

namespace ClusterSender
{
    public class SingletonActor: ReceiveActor
    {
	    private readonly ILoggingAdapter _logger = Context.GetLogger();
        public SingletonActor()
        {
            Receive<string>(s => _logger.Info(s));
        }
    }
}