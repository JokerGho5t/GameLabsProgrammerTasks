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

        public override void OnStart(SignalBus signalBus, DataBase dataBase)
        {
            base.OnStart(signalBus, dataBase);

            winnerText.text = "";
            
            logger.Init(signalBus);
            shipViewA.Init(signalBus, dataBase.ShipDataA);
            shipViewB.Init(signalBus, dataBase.ShipDataB);
            modulesView.Init(signalBus, dataBase.Weapons, dataBase.Modules);

            startFight.interactable = true;
            stopFight.interactable = false;
            
            startFight.onClick.AddListener(StartFight);
            stopFight.onClick.AddListener(StopFight);
            
            signalBus.Subscribe<SignalWinner>(Winner);
        }

        private void Winner(SignalWinner signal)
        {
            winnerText.text = $"{signal.WinnerName} WINNER!";
            StopFight();
        }

        private void StartFight()
        {
            winnerText.text = "";
            
            logger.Clear();
            
            startFight.interactable = false;
            stopFight.interactable = true;
            
            shipViewA.SetInteract(false);
            shipViewB.SetInteract(false);
            
            signalBus.Fire<SignalStartFight>();
        }
        
        private void StopFight()
        {
            stopFight.interactable = false;
            startFight.interactable = true;
            
            shipViewA.SetInteract(true);
            shipViewB.SetInteract(true);
            
            signalBus.Fire<SignalEndFight>();
        }
    }
}