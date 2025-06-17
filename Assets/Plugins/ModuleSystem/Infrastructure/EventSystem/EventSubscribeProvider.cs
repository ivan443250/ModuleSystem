using System;

namespace MVCSample.Infrastructure.EventSystem
{
    public class EventSubscribeProvider<T> where T : IEventData
    {
        private EventContainer _eventContainer;

        public EventSubscribeProvider(EventContainer eventContainer)
        {
            _eventContainer = eventContainer;
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return _eventContainer.Subscribe(action);
        }
    }
}
