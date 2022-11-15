using Ships.Signals;

namespace Ships
{
    public class Game : MonoChild
    {
        private Ship shipA = new Ship();
        private Ship shipB = new Ship();

        private SignalBus m_SignalBus;
        private bool m_IsFight;

        public override void OnStart()
        {
            m_SignalBus = SingletonContainer.Instance.Get<SignalBus>();
            var dataBase = SingletonContainer.Instance.Get<DataBase>();

            m_IsFight = false;

            Subscribes();

            shipA.Init(dataBase.ShipDataA, 
                OnSendMessage, 
                () => Loser(true), 
                shipB.TakeDamage, 
                OnChangeHealthOrShield);
            
            shipB.Init(dataBase.ShipDataB, 
                OnSendMessage, 
                () => Loser(false), 
                shipA.TakeDamage,
                OnChangeHealthOrShield);
        }

        private void Subscribes()
        {
            m_SignalBus.Subscribe<SignalAddWeaponForShip>(AddWeapon);
            m_SignalBus.Subscribe<SignalAddModuleForShip>(AddModule);
            m_SignalBus.Subscribe<SignalRemoveAllModules>(RemoveAllModules);
            m_SignalBus.Subscribe<SignalStartFight>(_ => m_IsFight = true);
            m_SignalBus.Subscribe<SignalEndFight>(OnEndFight);
        }
        
        private void OnChangeHealthOrShield(Ship ship)
        {
            m_SignalBus.Fire(new SignalChangeShipShieldAndHealth(ship));
        }

        private void OnSendMessage(string message)
        {
            m_SignalBus.Fire(new SignalMessage(message));
        }

        private void Loser(bool isShipA)
        {
            m_SignalBus.Fire(isShipA
                ? new SignalWinner(shipA.Data.Name)
                : new SignalWinner(shipB.Data.Name));
        }

        private void OnEndFight(SignalEndFight signal)
        {
            m_IsFight = false;
            shipA.Recovery();
            m_SignalBus.Fire(new SignalUpdateShipInfo(shipA));
            shipB.Recovery();
            m_SignalBus.Fire(new SignalUpdateShipInfo(shipB));
        }
        
        private void AddWeapon(SignalAddWeaponForShip signal)
        {
            if (shipA.Data == signal.OnShip)
            {
                shipA.SetWeapon(signal.IdxSlot, signal.Weapon);
                m_SignalBus.Fire(new SignalUpdateShipInfo(shipA));
            }
            else
            {
                shipB.SetWeapon(signal.IdxSlot, signal.Weapon);
                m_SignalBus.Fire(new SignalUpdateShipInfo(shipB));
            }
        }
        
        private void AddModule(SignalAddModuleForShip signal)
        {
            if (shipA.Data == signal.OnShip)
            {
                shipA.SetModule(signal.IdxSlot, signal.Module);
                m_SignalBus.Fire(new SignalUpdateShipInfo(shipA));
            }
            else
            {
                shipB.SetModule(signal.IdxSlot, signal.Module);
                m_SignalBus.Fire(new SignalUpdateShipInfo(shipB));
            }
        }
        
        private void RemoveAllModules(SignalRemoveAllModules signal)
        {
            if (shipA.Data == signal.OnShip)
            {
                shipA.ClearAllSlots();
                m_SignalBus.Fire(new SignalUpdateShipInfo(shipA));
            }
            else
            {
                shipB.ClearAllSlots();
                m_SignalBus.Fire(new SignalUpdateShipInfo(shipB));
            }
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