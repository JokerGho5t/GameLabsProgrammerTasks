namespace Ships.Signals
{
    public readonly struct SignalAddWeaponForShip
    {
        public readonly ShipData OnShip;
        public readonly int IdxSlot;
        public readonly WeaponData Weapon;

        public SignalAddWeaponForShip(ShipData onShip, int idxSlot, WeaponData weapon)
        {
            OnShip = onShip;
            IdxSlot = idxSlot;
            Weapon = weapon;
        }
    }
}