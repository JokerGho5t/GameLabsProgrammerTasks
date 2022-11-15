using System.Collections.Generic;
using Ships.Signals;
using UnityEngine;

namespace Ships
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private DataBase dataBase;
        [SerializeField] private List<MonoChild> monoChildren;
        
        private SingletonContainer m_Container;
        private SignalBus m_SignalBus;

        private void Start()
        {
            m_Container = SingletonContainer.Instance;
            m_Container.Add<SignalBus>(new SignalBus());
            m_Container.Add<DataBase>(dataBase);

            m_SignalBus = m_Container.Get<SignalBus>();
            
            DeclareSignals();
            Init();
        }

        private void OnDestroy()
        {
            m_Container.Remove<SignalBus>();
            m_Container.Remove<DataBase>();
        }

        private void DeclareSignals()
        {
            m_SignalBus.DeclareSignal<SignalAddModuleForShip>();
            m_SignalBus.DeclareSignal<SignalAddWeaponForShip>();
            m_SignalBus.DeclareSignal<SignalClickModuleSlot>();
            m_SignalBus.DeclareSignal<SignalClickWeaponSlot>();
            m_SignalBus.DeclareSignal<SignalEndFight>();
            m_SignalBus.DeclareSignal<SignalRemoveAllModules>();
            m_SignalBus.DeclareSignal<SignalShipDeath>();
            m_SignalBus.DeclareSignal<SignalChangeShipShieldAndHealth>();
            m_SignalBus.DeclareSignal<SignalStartFight>();
            m_SignalBus.DeclareSignal<SignalUpdateShipInfo>();
            m_SignalBus.DeclareSignal<SignalWeaponFire>();
            m_SignalBus.DeclareSignal<SignalWinner>();
            m_SignalBus.DeclareSignal<SignalMessage>();
        }

        private void Init()
        {
            foreach (var child in monoChildren)
            {
                child.OnStart();
            }
        }

        private void Update()
        {
            foreach (var child in monoChildren)
            {
                child.OnUpdate();
            }
        }
    }
}