using System;
using System.Collections.Generic;

namespace Ships
{
    public class SignalBus
    {
        private Dictionary<Type, SubscribersList> _subscribers
            = new Dictionary<Type, SubscribersList>();

        public void DeclareSignal<T>()
        {
            _subscribers.Add(typeof(T), new SubscribersList());
        }
        
        public void Fire<TSignal>()
        {
            FireInternal(typeof(TSignal), null);
        }

        public void Fire<TSignal>(TSignal signal)
        {
            FireInternal(typeof(TSignal), signal);
        }

        private void FireInternal(Type type, object signal)
        {
            if (!_subscribers.ContainsKey(type))
                throw new Exception($"Fired undeclared signal '{type}'!");

            if(signal == null)
                signal = Activator.CreateInstance(type);

            _subscribers[type].Execute(signal);
        }

        public void Subscribe<TSignal>(Action<TSignal> callback)
        {
            Action<object> callbackWrapper = args => callback((TSignal)args);
            SubscribeInternal(new Subscriber(typeof(TSignal), callback, callbackWrapper));
        }


        private void SubscribeInternal(Subscriber subscriber)
        {
            if (!_subscribers.ContainsKey(subscriber.Type))
                throw new Exception($"Subscribe undeclared signal '{subscriber.Type}'!");

            if (_subscribers[subscriber.Type].Contains(subscriber))
                throw new Exception($"Tried subscribing to the same signal with the same callback!");

            _subscribers[subscriber.Type].Add(subscriber);
        }

        public void Unsubscribe<TSignal>(Action<TSignal> callback)
        {
            UnsubscribeInternal(new Subscriber(typeof(TSignal), callback, null));
        }

        private void UnsubscribeInternal(Subscriber subscriber)
        {
            if (!_subscribers.ContainsKey(subscriber.Type))
                throw new Exception($"Unsubscribe undeclared signal '{subscriber.Type}'!");
            
            if (!_subscribers[subscriber.Type].Contains(subscriber))
                throw new Exception($"Called unsubscribe for signal '{subscriber.Type}' but could not find corresponding subscribe!");
        }
    }
}