using Ships.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
    public class LoggerView : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private Text log;

        public void Init(SignalBus signalBus)
        {
            signalBus.Subscribe<SignalMessage>(NewMessage);
            Clear();
        }

        private void NewMessage(SignalMessage signal)
        {
            log.text += $"{signal.Message}\n";
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)log.transform);
            
            scroll.verticalNormalizedPosition = 0;
        }

        public void Clear()
        {
            log.text = "";
        }
    }
}