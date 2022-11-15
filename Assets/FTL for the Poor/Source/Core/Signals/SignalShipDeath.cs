namespace Ships.Signals
{
    public readonly struct SignalShipDeath
    {
        public readonly Ship Ship;

        public SignalShipDeath(Ship ship)
        {
            Ship = ship;
        }
    }
}