using UnityEngine;

namespace Ships.Signals
{
    public readonly struct SignalClickModuleSlot
    {
        public readonly ShipData OnShip;
        public readonly int Idx;
        public readonly RectTransform RectSlot;

        public SignalClickModuleSlot(ShipData onShip, int idx, RectTransform rectSlot)
        {
            OnShip = onShip;
            Idx = idx;
            RectSlot = rectSlot;
        }
    }
}