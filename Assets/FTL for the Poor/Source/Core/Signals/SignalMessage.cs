using UnityEngine;

namespace Ships.Signals
{
    public readonly struct SignalMessage
    {
        public readonly string Message;

        public SignalMessage(string message)
        {
            Message = message;
        }
    }
}