namespace CommandApi.MessageBus.EventProcessor.Abstractions
{
    public interface IEventProcessor
    {
        void HandleEvent(string message);
    }
}