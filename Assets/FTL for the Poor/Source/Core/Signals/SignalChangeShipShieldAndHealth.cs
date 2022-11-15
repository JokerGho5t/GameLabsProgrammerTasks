namespace Ships.Signals
{
    public readonly struct SignalChangeShipShieldAndHealth
    {
        public readonly Ship Ship;

        public SignalChangeShipShieldAndHealth(Ship ship)
        {
            Ship = ship;
        }
    }
}