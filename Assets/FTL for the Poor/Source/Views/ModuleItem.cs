using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ships
{
    [RequireComponent(typeof(Button))]
    public class ModuleItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Text labelText;
        [SerializeField] private GameObject infoBox;
        [SerializeField] private Text infoText;
        
        private Button m_Button;
        private Action m_Onclick;

        public void Init(string moduleName, string moduleInfo, Action onClick)
        {
            m_Button = GetComponent<Button>();
            m_Button.onClick.AddListener(OnClick);

            m_Onclick = onClick;
            
            infoBox.SetActive(false);

            labelText.text = moduleName;
            infoText.text = moduleInfo;
        }

        private void OnClick()
        {
            infoBox.SetActive(false);
            m_Onclick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            infoBox.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            infoBox.SetActive(false);
        }
    }
}