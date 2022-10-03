using Microsoft.AspNetCore.Cors;
using NATS.Client;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.Json.Nodes;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Text;
using System.Collections;
using Options = NATS.Client.Options;
using Newtonsoft.Json.Linq;

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

        opts.Url = Environment.GetEnvironmentVariable("NATS_SERVER");
    }
    public async Task Start()
    {
        if (this.c == null)
        { 
        this.c = cf.CreateConnection(opts);

        EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
        {
            Console.WriteLine($"worker received {args.Message}");
            string str = Encoding.UTF8.GetString(args.Message.Data, 0, args.Message.Data.Length);
        };
        IAsyncSubscription s = c.SubscribeAsync("roadevents", h);

    }
    }
    public void Dispose()
    {
        c?.Dispose();
    }
}