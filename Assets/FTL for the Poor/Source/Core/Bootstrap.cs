using System.Collections.Generic;
using Ships.Signals;
using UnityEngine;

namespace Ships
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private DataBase dataBase;
        [SerializeField] private List<MonoChild> monoChildren;

        private SignalBus signalBus = new SignalBus();

        private void Start()
        {
            DeclareSignals();
            Init();
        }
        
        private void DeclareSignals()
        {
            signalBus.DeclareSignal<SignalAddModuleForShip>();
            signalBus.DeclareSignal<SignalAddWeaponForShip>();
            signalBus.DeclareSignal<SignalClickModuleSlot>();
            signalBus.DeclareSignal<SignalClickWeaponSlot>();
            signalBus.DeclareSignal<SignalEndFight>();
            signalBus.DeclareSignal<SignalRemoveAllModules>();
            signalBus.DeclareSignal<SignalShipDeath>();
            signalBus.DeclareSignal<SignalChangeShipShieldAndHealth>();
            signalBus.DeclareSignal<SignalStartFight>();
            signalBus.DeclareSignal<SignalUpdateShipInfo>();
            signalBus.DeclareSignal<SignalWeaponFire>();
            signalBus.DeclareSignal<SignalWinner>();
            signalBus.DeclareSignal<SignalMessage>();
        }

        private void Init()
        {
            foreach (var child in monoChildren)
            {
                child.OnStart(signalBus, dataBase);
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