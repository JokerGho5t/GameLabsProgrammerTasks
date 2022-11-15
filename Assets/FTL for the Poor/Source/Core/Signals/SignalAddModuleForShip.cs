namespace Ships.Signals
{
    public readonly struct SignalAddModuleForShip
    {
        public readonly ShipData OnShip;
        public readonly int IdxSlot;
        public readonly ModuleData Module;

        public SignalAddModuleForShip(ShipData onShip, int idxSlot, ModuleData module)
        {
            OnShip = onShip;
            IdxSlot = idxSlot;
            Module = module;
        }
    }
}