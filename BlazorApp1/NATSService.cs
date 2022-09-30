using Microsoft.AspNetCore.Cors;
using NATS.Client;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.Json.Nodes;
using System.Text.Json;
using BlazorApp1.Data;
using Microsoft.Extensions.Options;
using System.Text;
using System.Collections;
using Options = NATS.Client.Options;

[DataContract]
public class NATSService :IDisposable
{
    private readonly NATSNotifierService notifier;
    private readonly ConnectionFactory cf;
    private readonly Options opts;
    private IConnection c;


    public NATSService(NATSNotifierService notifier)
    {
        this.notifier = notifier;

        this.cf = new ConnectionFactory();
        
        this.opts = ConnectionFactory.GetDefaultOptions();
        opts.Url = "nats://host.docker.internal:4222";
    }
    public async Task Start()
    {
        if (this.c == null)
        { 
        this.c = cf.CreateConnection(opts);



        //IConnection c = cf.CreateConnection(opts);
        EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
        {
            Console.WriteLine($"worker received {args.Message}");
            string str = Encoding.UTF8.GetString(args.Message.Data, 0, args.Message.Data.Length);
            //notifier.Update("elapsedCount", args.Message.Data.ToString());
            notifier.Update("elapsedCount", str);
        };
        IAsyncSubscription s = c.SubscribeAsync("test", h);

    }
    }
    public void Dispose()
    {
        c?.Dispose();
    }
}