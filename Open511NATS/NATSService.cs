using NATS.Client;
using System.Runtime.Serialization;
using System.Text;
using Options = NATS.Client.Options;

[DataContract]
public class NATSService :IDisposable
{
    private readonly NATSNotifierService notifier;
    private readonly ConnectionFactory cf;
    private readonly Options opts;
    private IConnection c;
    private readonly IConfiguration Configuration;



    public NATSService(NATSNotifierService notifier, IConfiguration configuration)
    {
        this.notifier = notifier;

        this.cf = new ConnectionFactory();
        
        this.opts = ConnectionFactory.GetDefaultOptions();
        Configuration = configuration;

        opts.Url = Configuration["NATSClient"];

        
        //opts.Url = Environment.GetEnvironmentVariable("NATS_SERVER_CLIENT");
        //opts.Url = "nats://host.docker.internal:4222";
        //opts.Url = "nats://my-nats:4222";


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
            notifier.Update("RoadEvents", str);
        };
        IAsyncSubscription s = c.SubscribeAsync("roadevents", h);

    }
    }
    public void Dispose()
    {
        c?.Dispose();
    }
}