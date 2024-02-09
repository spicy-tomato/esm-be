using ESM.Application.Common.Interfaces;
using PusherServer;

namespace ESM.Presentation.Services;

public class PusherService : IPusherService
{
    private readonly IConfiguration _configuration;
    private Pusher? _pusher;

    private Pusher Pusher
    {
        get
        {
            if (_pusher != null)
            {
                return _pusher;
            }

            var options = new PusherOptions
            {
                Cluster = _configuration["Pusher:Cluster"],
                Encrypted = true,
            };

            _pusher = new Pusher(_configuration["Pusher:AppId"], _configuration["Pusher:Key"],
                _configuration["Pusher:Secret"], options);

            return _pusher;
        }
    }

    public PusherService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Publish(string[] channels, string @event, object data, ITriggerOptions? options = null)
    {
        await Pusher.TriggerAsync(channels, @event, data, options);
    }

    public async Task SendMessageImportExamination(int percentage)
    {
        await Publish(new[] { "admin" }, "ImportExamination", percentage);
    }
}