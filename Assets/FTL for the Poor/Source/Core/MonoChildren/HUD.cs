using Ships.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
    public class HUD : MonoChild
    {
        [SerializeField] private LoggerView logger;
        [SerializeField] private Text winnerText;
        [SerializeField] private Button startFight; 
        [SerializeField] private Button stopFight; 
        [SerializeField] private ShipView shipViewA;
        [SerializeField] private ShipView shipViewB;
        [SerializeField] private ListModulesView modulesView;

        private SignalBus m_SignalBus;
        
        public override void OnStart()
        {
            m_SignalBus = SingletonContainer.Instance.Get<SignalBus>();
            var dataBase = SingletonContainer.Instance.Get<DataBase>();
            
            winnerText.text = "";
            
            logger.Init();
            shipViewA.Init(dataBase.ShipDataA, OnClickWeaponSlot, OnClickModuleSlot);
            shipViewB.Init(dataBase.ShipDataB, OnClickWeaponSlot, OnClickModuleSlot);
            
            modulesView.Init(dataBase.Weapons, dataBase.Modules, OnAddWeapon, OnAddModule);

            startFight.interactable = true;
            stopFight.interactable = false;
            
            startFight.onClick.AddListener(StartFight);
            stopFight.onClick.AddListener(StopFight);

            Subscribes();
        }

        private void Subscribes()
        {
            m_SignalBus.Subscribe<SignalWinner>(Winner);
            m_SignalBus.Subscribe<SignalMessage>(OnMessage);
            m_SignalBus.Subscribe<SignalUpdateShipInfo>(UpdateShipInfo);
            m_SignalBus.Subscribe<SignalChangeShipShieldAndHealth>(RepaintShipShield);
        }
        
        private void Winner(SignalWinner signal)
        {
            winnerText.text = $"{signal.WinnerName} WINNER!";
            StopFight();
        }

        private void OnMessage(SignalMessage signal)
        {
            logger.NewMessage(signal.Message);
        }
        
        private void UpdateShipInfo(SignalUpdateShipInfo signal)
        {
            if(signal.Ship.Data == shipViewA.ShipData)
                shipViewA.Repaint(signal.Ship);
            else
                shipViewB.Repaint(signal.Ship);
        }
        
        private void RepaintShipShield(SignalChangeShipShieldAndHealth signal)
        {
            if(signal.Ship.Data == shipViewA.ShipData)
                shipViewA.RepaintShield(signal.Ship);
            else
                shipViewB.RepaintShield(signal.Ship);
        }

        private void OnClickModuleSlot(SignalClickModuleSlot signal)
        {
            modulesView.SelectModuleSlot(signal);
        }

        private void OnClickWeaponSlot(SignalClickWeaponSlot signal)
        {
            modulesView.SelectWeaponSlot(signal);
        }
        
        private void OnAddModule(SignalAddModuleForShip signal)
        {
            m_SignalBus.Fire(signal);
        }

        private void OnAddWeapon(SignalAddWeaponForShip signal)
        {
            m_SignalBus.Fire(signal);
        }

        private void StartFight()
        {
            winnerText.text = "";
            
            logger.Clear();
            
            startFight.interactable = false;
            stopFight.interactable = true;
            
            shipViewA.SetInteract(false);
            shipViewB.SetInteract(false);
            
            m_SignalBus.Fire<SignalStartFight>();
        }
        
        private void StopFight()
        {
            stopFight.interactable = false;
            startFight.interactable = true;
            
            shipViewA.SetInteract(true);
            shipViewB.SetInteract(true);
            
            m_SignalBus.Fire<SignalEndFight>();
        }
    }
}