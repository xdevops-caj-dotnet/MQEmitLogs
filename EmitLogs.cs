using System;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

class EmitLogs
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            Console.WriteLine("EmitLogs producer started");
            Publish(channel);
        }
    }

    private static void Publish(IModel channel)
    {
        channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
        var count = 0;
        while (true)
        {
            var message = new { Name = "Logs", Message = $"Hello! Count: {count}" };
            Console.WriteLine(">> {0}", message);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish(exchange: "logs",
                             routingKey: "",
                             basicProperties: null,
                             body: body);
            count++;
            Thread.Sleep(1000);
        }
    }
}