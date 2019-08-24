using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fluent.UI.Core
{
    internal class EventAggregator : IEventAggregator
    {
        private readonly object _lock = new object();

        private static readonly Lazy<EventAggregator> _lazyAggregator = new Lazy<EventAggregator>(() => new EventAggregator());
        private readonly IDictionary<Type, IList<EventAggregatorHandler<IEvent>>> _handlers = new Dictionary<Type, IList<EventAggregatorHandler<IEvent>>>();

        private EventAggregator()
        {
        }

        public static EventAggregator Current => _lazyAggregator.Value;

        public void Publish(IEvent @event)
        {
            if (_handlers.TryGetValue(@event.GetType(), out IList<EventAggregatorHandler<IEvent>> handlerList))
            {
                foreach(var handler in handlerList.ToList())
                {
                    handler.Invoke(@event);
                }
            }
        }

        public void Subscribe<TEvent>(EventAggregatorHandler<TEvent> handler) where TEvent : IEvent
        {
            if (!_handlers.ContainsKey(typeof(TEvent)))
            {
                _handlers[typeof(TEvent)] = new List<EventAggregatorHandler<IEvent>>();
            }

            var handlerList = _handlers[typeof(TEvent)];
            handlerList.Add(evt => handler((TEvent)evt));
        }
    }
}