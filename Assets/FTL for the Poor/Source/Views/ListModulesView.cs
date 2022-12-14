using System;
using System.Collections.Generic;
using Ships.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
    public class ListModulesView : MonoBehaviour
    {
        [SerializeField] private RectTransform containerListWeapons;
        [SerializeField] private RectTransform containerListModules;
        [SerializeField] private Button closeButton;
        [SerializeField] private ModuleItem prefab;

        private ShipData m_TargetShip;
        private int m_SlotIdx;

        public void Init(List<WeaponData> weapons, List<ModuleData> modules, Action<SignalAddWeaponForShip> onAddWeapon,
            Action<SignalAddModuleForShip> onAddModule)
        {
            for (var i = 0; i < weapons.Count; i++)
            {
                var btn = Instantiate(prefab, containerListWeapons);
                var idx = i;
                btn.Init(weapons[i].Name, weapons[i].TextInfo,
                    () =>
                    {
                        onAddWeapon(new SignalAddWeaponForShip(m_TargetShip, m_SlotIdx, weapons[idx]));
                        OnClose();
                    });
            }

            for (var i = 0; i < modules.Count; i++)
            {
                var btn = Instantiate(prefab, containerListModules);
                var idx = i;
                btn.Init(modules[i].Name, modules[i].TextInfo,
                    () =>
                    {
                        onAddModule(new SignalAddModuleForShip(m_TargetShip, m_SlotIdx, modules[idx]));
                        OnClose();
                    });
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(containerListWeapons);
            LayoutRebuilder.ForceRebuildLayoutImmediate(containerListModules);

            closeButton.onClick.AddListener(OnClose);

            OnClose();
        }

        private void OnClose()
        {
            gameObject.SetActive(false);
            containerListModules.gameObject.SetActive(false);
            containerListWeapons.gameObject.SetActive(false);
        }
        
        public void SelectWeaponSlot(SignalClickWeaponSlot signal)
        {
            m_TargetShip = signal.OnShip;
            m_SlotIdx = signal.IdxSlot;
            
            gameObject.SetActive(true);
            
            SetRectPosition(signal.RectSlot, containerListWeapons);
            containerListWeapons.gameObject.SetActive(true);
        }

        public void SelectModuleSlot(SignalClickModuleSlot signal)
        {
            m_TargetShip = signal.OnShip;
            m_SlotIdx = signal.Idx;
            
            gameObject.SetActive(true);

            SetRectPosition(signal.RectSlot, containerListModules);
            containerListModules.gameObject.SetActive(true);
        }
        
        private void SetRectPosition(RectTransform target, RectTransform rect)
        {
            var position = target.position;
            var pivot = new Vector2(0, 0.5f);
            if (position.x + containerListWeapons.rect.width > 1920)
            {
                pivot.x = 1;
                position.x += target.rect.xMin - 10;
            }
            else
            {
                position.x += target.rect.xMax + 10;
            }

            rect.position = position;
            rect.pivot = pivot;
        }
    }
}