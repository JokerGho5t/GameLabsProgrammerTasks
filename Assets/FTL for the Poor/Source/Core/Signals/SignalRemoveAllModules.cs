namespace Ships.Signals
{
    public readonly struct SignalRemoveAllModules
    {
        public readonly ShipData OnShip;

        public SignalRemoveAllModules(ShipData onShip)
        {
            OnShip = onShip;
        }
    }
}