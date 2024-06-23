using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.RabbitMQClientService;

using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class RabbitMQClientManager:IRabbitMQClientService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQClientManager(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"],
            Port = int.Parse(configuration["RabbitMQ:Port"]),
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"]
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public async Task<CounterDetailsDto> GetCounterDetailsAsync(Guid counterId)
    {
        var taskCompletionSource = new TaskCompletionSource<CounterDetailsDto>();
        var correlationId = Guid.NewGuid().ToString();

        var replyQueueName = _channel.QueueDeclare().QueueName;
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                var counterDetails = JsonSerializer.Deserialize<CounterDetailsDto>(response);
                taskCompletionSource.SetResult(counterDetails);
            }
        };

        _channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);

        var message = Encoding.UTF8.GetBytes(counterId.ToString());
        var props = _channel.CreateBasicProperties();
        props.ReplyTo = replyQueueName;
        props.CorrelationId = correlationId;

        _channel.BasicPublish(
            exchange: "",
            routingKey: "counterQueue",
            basicProperties: props,
            body: message);

        return await taskCompletionSource.Task;
    }
}

