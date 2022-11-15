using System;
using System.Collections.Generic;

namespace Ships
{
    internal class SubscribersList
    {
        private bool m_NeedsCleanUp = false;

        public bool Executing;

        private List<Subscriber> m_Subscribers = new List<Subscriber>();

        
        public List<Subscriber> Subscribers => m_Subscribers;
        public bool Contains(Subscriber subscriber) => m_Subscribers.Contains(subscriber);

        public void Add(Subscriber subscriber)
        {
            m_Subscribers.Add(subscriber);
        }

        public void Remove(Subscriber subscriber)
        {
            if (Executing)
            {
                var i = m_Subscribers.IndexOf(subscriber);
                if (i < 0) return;
                
                m_NeedsCleanUp = true;
                m_Subscribers[i].SetClean();
            }
            else
            {
                m_Subscribers.Remove(subscriber);
            }
        }

        public void Execute(object signal)
        {
            Executing = true;
            
            foreach (var subscriber in m_Subscribers)
            {
                try
                {
                    subscriber.Invoke(signal);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            Executing = false;
            Cleanup();
        }

        private void Cleanup()
        {
            if (!m_NeedsCleanUp)
            {
                return;
            }

            m_Subscribers.RemoveAll(s => s.NeedClean);
            m_NeedsCleanUp = false;
        }
    }
}