using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace AltenBlog.Common.Infrastructore;

public static class QueueFactory // ismi Factory ama build patterne daha yakin bir klass olacak
{
    public static void SendMessageToExchange(string exchangeName, string exchangeType, string queueName, object obj)//obj nin kendisi,evente olabilir,
                                                                                                                    //stringde olabilir,json la generik edip gönderecegiz
    {
        //RabbitMq ya mesaj gönderebilmek icin ConnectionFactory Klassina ihtiyacimiz var,RabbitMQ nun kendi implementationu bu
        // daha sonra bu connectionu kullanarak channel olusturacagiz bu channelida kullanarak consumor evet basic consumor

        var channel = CreateBasicConsumer()
                        .EnsureExchage(exchangeName, exchangeType)
                        .EnsureQueue(queueName, exchangeName)
                        .Model;
        var queBody = JsonSerializer.Serialize(obj);
        var body = Encoding.UTF8.GetBytes(queBody);

        channel.BasicPublish(exchange: exchangeType, routingKey: queueName, basicProperties: null, body: body);
    }

    public static EventingBasicConsumer CreateBasicConsumer()
    {
        var factory = new ConnectionFactory() { HostName = SozlukConstans.RabbitMQHost };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        return new EventingBasicConsumer(channel);
    }

    //Burasi tamda Builder Patternde oldugugi, objenin kendisi extention methoda gönderilir ve sonrasinda bu geri ayni type gönderilir,
    //bunu cagiranlar yine ayni type sahip olur
    public static EventingBasicConsumer EnsureExchage(this EventingBasicConsumer consumer, string exchangeName, string exchangeType = SozlukConstans.DefaultExchangeType)
    {
        consumer.Model.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: false, autoDelete: false);
        return consumer;
    }

    public static EventingBasicConsumer EnsureQueue(this EventingBasicConsumer consumer, string queueName, string exchangeName)
    {
        consumer.Model.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, null);
        consumer.Model.QueueBind(queue: queueName, exchange: exchangeName, queueName); //Bindig islemi yapiliyor

        return consumer;
    }
}