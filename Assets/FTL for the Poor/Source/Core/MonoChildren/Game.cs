using Ships.Signals;

namespace Ships
{
    public class Game : MonoChild
    {
        private Ship shipA = new Ship();
        private Ship shipB = new Ship();

        private bool m_IsFight;

        public override void OnStart(SignalBus signalBus, DataBase dataBase)
        {
            base.OnStart(signalBus, dataBase);
            
            m_IsFight = false;

            Subscribes();

            shipA.Init(signalBus, dataBase.ShipDataA);
            shipB.Init(signalBus, dataBase.ShipDataB);
        }

        private void Subscribes()
        {
            signalBus.Subscribe<SignalWeaponFire>(OnWeaponFire);
            signalBus.Subscribe<SignalAddWeaponForShip>(AddWeapon);
            signalBus.Subscribe<SignalAddModuleForShip>(AddModule);
            signalBus.Subscribe<SignalRemoveAllModules>(RemoveAllModules);
            signalBus.Subscribe<SignalStartFight>(_ => m_IsFight = true);
            signalBus.Subscribe<SignalEndFight>(OnEndFight);
            signalBus.Subscribe<SignalShipDeath>(Loser);
        }

        private void Loser(SignalShipDeath signal)
        {
            signalBus.Fire(signal.Ship == shipA
                ? new SignalWinner(shipA.Data.Name)
                : new SignalWinner(shipB.Data.Name));
        }

        private void OnEndFight(SignalEndFight signal)
        {
            m_IsFight = false;
            shipA.Recovery();
            shipB.Recovery();
        }

        private void OnWeaponFire(SignalWeaponFire signal)
        {
            if (shipA == signal.Ship)
                shipB.TakeDamage(signal.Value);
            else
                shipA.TakeDamage(signal.Value);
        }
        
        private void AddWeapon(SignalAddWeaponForShip signal)
        {
            if (shipA.Data == signal.OnShip)
                shipA.SetWeapon(signal.IdxSlot, signal.Weapon);
            else
                shipB.SetWeapon(signal.IdxSlot, signal.Weapon);
        }
        
        private void AddModule(SignalAddModuleForShip signal)
        {
            if (shipA.Data == signal.OnShip)
                shipA.SetModule(signal.IdxSlot, signal.Module);
            else
                shipB.SetModule(signal.IdxSlot, signal.Module);
        }
        
        private void RemoveAllModules(SignalRemoveAllModules signal)
        {
            if (shipA.Data == signal.OnShip)
                shipA.ClearAllSlots();
            else
                shipB.ClearAllSlots();
        }

        public override void OnUpdate()
        {
            if (m_IsFight)
                UpdateShips();
        }

        private void UpdateShips()
        {
            shipA.Update();
            shipB.Update();
        }
    }
}