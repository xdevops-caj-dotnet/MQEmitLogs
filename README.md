[TOC]

# RabbitMQ Pub/sub in .NET

## Install RabbitMQ locally

Install RabbitMQ via [Rabbit docker images](https://hub.docker.com/_/rabbitmq) through [podman](https://podman.io/getting-started/installation) command:

```bash
podman run -d --hostname my-rabbit --name local-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

Check logs:
```bash
podman logs -f local-rabbit
```

Access RabbitMQ dashboard <http://localhost:15672/> with default username / password `guest/guest`.


## Producer

Add below NuGet packages:
- `RabbitMQ.Client`
- `Newtonsoft.Json` used to serialize object to JSON string (bytes)

The producer connect to `amqp://guest:guest@localhost:5672`, and publish messages to  *fanout* exchange `logs` repeatly. 


## Consumer

Add below NuGet packages:
- `RabbitMQ.Client`

The consumer connect to `amqp://guest:guest@localhost:5672`, and consume messages from a new empty message queue with randome queue name binding with `logs` exchange and print the received message to the console. 

When the consumer disconnected the queue is automatically deleted.

In the .NET client, when we supply no parameters to QueueDeclare() we create a non-durable, exclusive, autodelete queue with a generated name:

```c#
var queueName = channel.QueueDeclare().QueueName;
```

The consumer code: [MQReceiveLogs](https://github.com/xdevops-caj-dotnet/MQReceiveLogs).

## Testing
Launch a few consumers:
```bash
# run in the MQReceiveLogs consumer *.csproj root folder in each terminal
dotnet run
```

Launch producer:
```bash
# run in the MQEmitLogs producer *.csproj root folder in another terminal
dotnet run
```

Check the logs of the consumer and producer.
On Rabbit dashboard, open "Exhange" page, and check `logs` exchange details and its binding queues.

## References
- [RabbitMQ tutorial - Publish/Subscribe](https://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html)

## More RabbitMQ examples
- [RabbitMQ Tutorials](https://www.rabbitmq.com/getstarted.html)
