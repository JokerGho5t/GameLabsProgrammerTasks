namespace Ships.Signals
{
    public readonly struct SignalWeaponFire
    {
        public readonly Ship Ship;
        public readonly float Value;

        public SignalWeaponFire(Ship ship, float value)
        {
            Ship = ship;
            Value = value;
        }
    }
}