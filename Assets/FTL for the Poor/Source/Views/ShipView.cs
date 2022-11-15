using Ships.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
    public class ShipView : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private Image icon;
        [SerializeField] private new Text name;
        [SerializeField] private Text shieldText;
        [SerializeField] private Slider shield;
        [SerializeField] private Text healthText;
        [SerializeField] private Slider health;

        [Header("Modules")] 
        [SerializeField] private RectTransform weaponsContainer;
        [SerializeField] private ShipModuleItem itemWeaponPrefab;
        [SerializeField] private RectTransform modulesContainer;
        [SerializeField] private ShipModuleItem itemModulePrefab;

        private ShipModuleItem[] m_Modules;
        private ShipModuleItem[] m_Weapons;
        private ShipData m_ShipData;
        
        public void Init(SignalBus signalBus, ShipData shipData)
        {
            m_ShipData = shipData;

            icon.sprite = shipData.Icon;
            name.text = shipData.Name;
            shield.value = shield.maxValue = shipData.MaxShield;
            shieldText.text = $"{shipData.MaxShield} / {shipData.MaxShield}";
            health.value = health.maxValue = shipData.MaxHealth;
            healthText.text = $"{shipData.MaxHealth} / {shipData.MaxHealth}";

            InitSlots(signalBus, shipData);

            signalBus.Subscribe<SignalUpdateShipInfo>(OnCallUpdate);
            signalBus.Subscribe<SignalChangeShipShieldAndHealth>(OnRepaintShield);
        }
        
        public void Repaint(Ship ship)
        {
            shield.value = shield.maxValue = ship.MaxShield;
            shieldText.text = $"{ship.MaxShield} / {ship.MaxShield}";
            
            health.value = health.maxValue = ship.MaxHealth;
            healthText.text = $"{ship.MaxHealth} / {ship.MaxHealth}";

            for (var i = 0; i < m_Weapons.Length; i++)
            {
                var weaponSlot = m_Weapons[i];
                weaponSlot.Repaint(ship.Weapons[i] == null ? "" : ship.Weapons[i].Name);
            }
            
            for (var i = 0; i < m_Modules.Length; i++)
            {
                var moduleSlot = m_Modules[i];
                moduleSlot.Repaint(ship.Modules[i] == null ? "" : ship.Modules[i].Name);
            }
        }

        public void SetInteract(bool interact)
        {
            foreach (var weapon in m_Weapons)
                weapon.SetInteract(interact);

            foreach (var module in m_Modules)
                module.SetInteract(interact);
        }

        private void InitSlots(SignalBus signalBus, ShipData shipData)
        {
            m_Weapons = new ShipModuleItem[shipData.WeaponSlot];
            for (int i = 0; i < shipData.WeaponSlot; i++)
            {
                var idx = i;
                m_Weapons[i] = Instantiate(itemWeaponPrefab, weaponsContainer);
                m_Weapons[i].Init("",
                    () => signalBus.Fire(new SignalClickWeaponSlot(shipData, idx,
                        (RectTransform)m_Weapons[idx].transform)));
            }

            m_Modules = new ShipModuleItem[shipData.ModuleSlot];
            for (int i = 0; i < shipData.ModuleSlot; i++)
            {
                var idx = i;
                m_Modules[i] = Instantiate(itemModulePrefab, modulesContainer);
                m_Modules[i].Init("",
                    () => signalBus.Fire(new SignalClickModuleSlot(shipData, idx,
                        (RectTransform)m_Modules[idx].transform)));
            }
        }

        private void OnCallUpdate(SignalUpdateShipInfo signal)
        {
            if (signal.Ship.Data != m_ShipData) return;
            
            Repaint(signal.Ship);
        }
        
        private void OnRepaintShield(SignalChangeShipShieldAndHealth signalChange)
        {
            if (signalChange.Ship.Data != m_ShipData) return;
            
            shield.value = signalChange.Ship.Shield;
            shieldText.text = $"{signalChange.Ship.Shield} / {signalChange.Ship.MaxShield}";
            
            health.value = signalChange.Ship.Health;
            healthText.text = $"{signalChange.Ship.Health} / {signalChange.Ship.MaxHealth}";
        }
    }
}