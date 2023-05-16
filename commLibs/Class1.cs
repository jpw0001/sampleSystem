using System;
using System.Threading;
using Confluent.Kafka;

namespace commLibs
{
    public class Messaging
    {
        public IProducer<Null, string> pb;
        protected string _servers { get; set; }
        protected string _topic { get; set; }
        protected string _consumerGroup { get; set; } = string.Empty;
        public Messaging(string topic, string servers, string consumerGroup)
        {
            this._servers = servers;
            this._topic = topic;
            this._consumerGroup = consumerGroup;
            pb = new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = _servers }).Build();
        }
        public void SendMessage(string value) => SendMessage(value, _topic);

        public virtual void SentMessage(string value, string topicPartitionOffset)
        {
            Console.WriteLine($"Delivered '{value}' to '{topicPartitionOffset}'");
        }

        public async void SendMessage( string value, string topic )
        {
            try
            {
                var dr = await pb.ProduceAsync(topic, new Message<Null, string> { Value=value });
                SentMessage(value, dr.TopicPartitionOffset.ToString());
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }

        public virtual void ProcessMessage( string value )
        {
            Console.WriteLine($"Consumed message '{value}'");
        }

        public void ConsumeMessage()
        {
            var conf = new ConsumerConfig
            {
                GroupId = _consumerGroup,
                BootstrapServers = _servers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(_topic);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            string value = cr.Message.Value;
                            ProcessMessage(value);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }
        }
    }
}