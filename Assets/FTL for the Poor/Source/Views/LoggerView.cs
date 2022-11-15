using Ships.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
    public class LoggerView : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;
        [SerializeField] public Text log;

        public void Init()
        {
            Clear();
        }

        public void NewMessage(string message)
        {
            log.text += $"{message}\n";
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)log.transform);
            
            scroll.verticalNormalizedPosition = 0;
        }

        public void Clear()
        {
            log.text = "";
        }
    }
}