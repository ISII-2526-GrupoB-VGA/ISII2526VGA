using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Composition;
using System.Text;
using System.Threading.Channels;

namespace AppForSEII2526.API.Logging.LogViewer
{
    public class Subscriber
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IBasicProperties _properties;

        private readonly string _exchangeName = "logs";

        public Subscriber(string _hostname = "localhost", int _port = 5672, string _username = "guest", string _password = "guest")
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
                Port = _port
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // 3
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout, durable: true);
        }

        public void Start() //aquí hace toda la lógica
        {
            //4
            var tempQueue = _channel.QueueDeclare(); //Esto no se muy bien pq está
            var queueName = tempQueue.QueueName; //Necesitamos recuperar el nombre que rabbit asignó de la cola temporal

            //5. Notificamos al broker de que los mensajes de este suscriptor deben ir a la cola temporal
            _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

            Console.WriteLine($"[Subscriber] Escuchando en exchange '{_exchangeName}', cola '{queueName}'...");

            //6. Creamos el consumidor
            var consumer = new EventingBasicConsumer(_channel);

            //7. Configurar callback para manejar mensajes entrantes

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray(); //contenido del mensaje (array de bytes)
                var message = Encoding.UTF8.GetString(body); //se convierte de vuelta a string
                Console.WriteLine($"Pedido recibido: {message}");
                //ProcessOrder(message); // Aquí va la lógica de procesamiento

                try
                {
                    var obj = JsonConvert.DeserializeObject<object>(message);
                    var pretty = JsonConvert.SerializeObject(obj, Formatting.Indented);
                    Console.WriteLine("Mensaje recibido:\n" + pretty);
                }
                catch
                {
                    Console.WriteLine("Mensaje recibido (no JSON): " + message);
                }

            };


            //8. Empezar a consumir mensajes
            _channel.BasicConsume(queue: queueName, autoAck:true, consumer: consumer);

        }

        public void Dispose() // Limpiamos recursos y cerramos conexiones
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
