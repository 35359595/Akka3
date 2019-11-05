## Example implementation of distributed Send of [Akka.net](https://getakka.net/)

* This is a basic implementation where `"/user/ping"` actor is been accessed from remote system directly.

* Cluster Singleton residing in nodes with one role is been accessed from node of different role.

### Known issues / missunderstandings
* Actors deployed to cluster from configuration cannot be accessed through mediator's `Send()` message.
This won't work:
```
akka.actor.deployment {
    /ping {
        router = round-robin-pull
        cluster {
            enabled = on
            ...other config...
        }
    }
}
```
with the error:
`INFO][11/03/2019 08:19:28][Thread 0018][akka://ClusterSys/system/distributedPubSubMediator] Message String from akka://*/user/mediatedSender/c4 to akka://*/system/distributedPubSubMediator was not delivered. 1 dead letters encountered.`
