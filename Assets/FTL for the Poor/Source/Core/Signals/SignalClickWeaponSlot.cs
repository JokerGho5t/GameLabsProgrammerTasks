using UnityEngine;

namespace Ships.Signals
{
    public readonly struct SignalClickWeaponSlot
    {
        public readonly ShipData OnShip;
        public readonly int IdxSlot;
        public readonly RectTransform RectSlot;

        public SignalClickWeaponSlot(ShipData onShip, int idxSlot, RectTransform rectSlot)
        {
            OnShip = onShip;
            IdxSlot = idxSlot;
            RectSlot = rectSlot;
        }
    }
}