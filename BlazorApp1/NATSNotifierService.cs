public class NATSNotifierService
{
    public async Task Update(string key, string value)
    {
        if (Notify != null)
        {
            await Notify.Invoke(key, value);
        }
    }

    public event Func<string, string, Task>? Notify;

}