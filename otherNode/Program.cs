// See https://aka.ms/new-console-template for more information
using System;
using System.Threading;

using commLibs;

public class Node : Messaging
{
    public Node(string topic, string servers, string group) : base(topic, servers, group)
    {
    }

    public override void ProcessMessage(string value)
    {
        Console.WriteLine($"Consumed message '{value}'");
        if (value == "orange")
        {
            SendMessage("grape");
        }
    }
       public override void SentMessage(string value, string topicPartitionOffset)
       {
            Console.WriteLine($"Delivered '{value}'");
       }
}

class Program
{
    public static void Main(string[] args)
    {
        Node m = new Node("quickstart", "127.0.0.1:9092","2");
        m.ConsumeMessage();
    }
}
