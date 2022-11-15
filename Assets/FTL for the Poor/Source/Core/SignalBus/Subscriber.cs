using System;

namespace Ships
{
    public struct Subscriber : IEquatable<Subscriber>
    {
        public Type Type { get; }
        public object Action { get; }

        public bool NeedClean { get; private set; }

        private readonly Action<object> m_Wrapper;

        public Subscriber(Type type, object action, Action<object> wrapper)
        {
            NeedClean = false;
            Type = type;
            Action = action;
            m_Wrapper = wrapper;
        }

        public void SetClean()
        {
            NeedClean = true;
        }

        public void Invoke(object obj)
        {
            m_Wrapper(obj);
        }
        
        public override bool Equals(object that)
        {
            if (that is Subscriber thatSignal)
            {
                return Type == thatSignal.Type && Action == thatSignal.Action;
            }

            return false;
        }

        public bool Equals(Subscriber that)
        {
            return Type == that.Type && Action.Equals(that.Action);
        }

        public static bool operator == (Subscriber left, Subscriber right)
        {
            return left.Equals(right);
        }

        public static bool operator != (Subscriber left, Subscriber right)
        {
            return !left.Equals(right);
        }
    }
}