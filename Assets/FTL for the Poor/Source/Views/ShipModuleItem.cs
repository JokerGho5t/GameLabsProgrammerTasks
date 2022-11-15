using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ships
{
    [RequireComponent(typeof(Button))]
    public class ShipModuleItem : MonoBehaviour
    {
        [SerializeField] private Text labelText;
        private Button m_Button;
        private UnityAction m_OnClick;

        private void Start()
        {
            m_Button = GetComponent<Button>();
            m_Button.onClick.AddListener(m_OnClick);   
        }

        public void Init(string moduleName, UnityAction onClick)
        {
            m_OnClick = onClick;
            Repaint(moduleName);
        }

        public void Repaint(string moduleName)
        {
            labelText.text = moduleName;
        }

        public void SetInteract(bool interact)
        {
            m_Button.interactable = interact;
        }
    }
}