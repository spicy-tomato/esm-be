using PusherServer;

namespace ESM.Application.Common.Interfaces;

public interface IPusherService
{
    public Task Publish(string[] channels, string @event, object data, ITriggerOptions? options);

    public Task SendMessageImportExamination(int percentage);
}