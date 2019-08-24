namespace Fluent.UI.Core
{
    internal delegate void EventAggregatorHandler<TEvent>(TEvent @event) where TEvent : IEvent;
}
