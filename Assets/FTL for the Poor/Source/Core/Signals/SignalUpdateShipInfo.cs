namespace Ships.Signals
{
    public struct SignalUpdateShipInfo
    {
        public readonly Ship Ship;

        public SignalUpdateShipInfo(Ship ship)
        {
            Ship = ship;
        }
    }
}