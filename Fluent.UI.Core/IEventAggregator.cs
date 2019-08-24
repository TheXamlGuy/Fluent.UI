namespace Fluent.UI.Core
{
    internal interface IEventAggregator
    {
        void Publish(IEvent @event);

        void Subscribe<TEvent>(EventAggregatorHandler<TEvent> handler) where TEvent : IEvent;
    }
}