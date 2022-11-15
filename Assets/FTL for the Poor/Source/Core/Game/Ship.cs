using System;
using Ships.Signals;
using UnityEngine;

namespace Ships
{
    public class Ship
    {
        private ShipData data;
        public ShipData Data => data;

        public float MaxHealth => data.MaxHealth + m_AddMaxHealth;
        private float m_AddMaxHealth;
        public float Health { get; private set; }

        public float MaxShield => data.MaxShield + m_AddMaxShield;
        private float m_AddMaxShield;
        public float Shield { get; private set; }
        public float ShieldRegeneration => data.ShieldRegeneration * m_FactorRegenerationShield;
        private float m_FactorRegenerationShield;
        private float timerShield;
        
        public WeaponSlot[] Weapons { get; private set; }
        public ModuleData[] Modules { get; private set; }
        

        private bool isDead => Health <= 0;

        private Action<string> m_OnMessage;
        private Action m_OnDeath;
        private Action<float> m_OnWeaponFire;
        private Action<Ship> m_ChangeHealthOrShield;

        public void Init(ShipData data, Action<string> onMessage, Action onDeath, Action<float> onWeaponFire,
            Action<Ship> changeHealthOrShield)
        {
            this.data = data;

            m_OnMessage = onMessage;
            m_OnDeath = onDeath;
            m_OnWeaponFire = onWeaponFire;
            m_ChangeHealthOrShield = changeHealthOrShield;

            m_AddMaxHealth = 0;
            m_AddMaxShield = 0;
            m_FactorRegenerationShield = 1;
            timerShield = 0;

            Weapons = new WeaponSlot[data.WeaponSlot];
            Modules = new ModuleData[data.ModuleSlot];

            Recovery();
        }

        public void Recovery()
        {
            Health = MaxHealth;
            Shield = MaxShield;
            
            foreach (var weapon in Weapons)
            {
                weapon?.Recovery();
            }
        }

        public void Update()
        {
            if (isDead) return;

            foreach (var weapon in Weapons)
            {
                weapon?.Update();
            }

            UpdateShield();
        }

        private void UpdateShield()
        {
            if (timerShield < ShieldRegeneration)
            {
                timerShield += Time.deltaTime;
                return;
            }

            timerShield = 0;
            Shield = Mathf.Clamp(++Shield, 0, MaxShield);
            m_ChangeHealthOrShield(this);
        }

        private void OnWeaponFire(WeaponSlot weapon, float damage)
        {
            SendMessage(
                $"Shot from <color=orange>{weapon.Name}</color>. " +
                $"The gun will be reloaded after <color=green>{weapon.ReloadTime}</color>");
            m_OnWeaponFire(damage);
        }

        private void SendMessage(string message)
        {
            m_OnMessage(
                $"<color=#{ColorUtility.ToHtmlStringRGB(data.ColorMessage)}>{data.Name}</color>: " + message);
        }

        public void TakeDamage(float damage)
        {
            if(isDead) return;
            
            if (Shield > 0)
            {
                if (damage <= Shield)
                {
                    Shield -= damage;
                    SendMessage($"Shields withheld <color=blue>{damage}</color> damage");
                }
                else
                {
                    Health = Mathf.Clamp(Health - (damage - Shield), 0, MaxHealth);
                    SendMessage(
                        $"The shields were destroyed. " +
                        $"The ship received <color=red>{damage - Shield}</color> damage");
                    
                    Shield = 0;
                }
            }
            else
            {
                Health = Mathf.Clamp(Health - damage, 0, MaxHealth);
                SendMessage($"The ship received <color=red>{damage}</color> damage");
            }

            m_ChangeHealthOrShield(this);
            
            if (!isDead) return;

            SendMessage("I was defeated!");
            m_OnDeath();
        }

        public void SetWeapon(int idxSlot, WeaponData weaponData)
        {
            Weapons[idxSlot] = new WeaponSlot(weaponData, x => OnWeaponFire(Weapons[idxSlot], x));

            foreach (var module in Modules)
            {
                if(module == null || module.ChangeReloadTimePercent == 0) continue;
                Weapons[idxSlot].ChangeFactorReloadTime(module.ChangeReloadTimePercent);
            }
        }

        public void SetModule(int idxSlot, ModuleData moduleData)
        {
            ClearModule(idxSlot);
            
            Modules[idxSlot] = moduleData;
            
            m_AddMaxHealth += moduleData.AddMaxHealth;
            m_AddMaxShield += moduleData.AddMaxShield;
            m_FactorRegenerationShield += moduleData.ChangeRegenerationShieldPercent;

            if (moduleData.ChangeReloadTimePercent != 0)
            {
                foreach (var weapon in Weapons)
                {
                    weapon?.ChangeFactorReloadTime(moduleData.ChangeReloadTimePercent);
                }
            }
        }
        
        private void ClearModule(int idx)
        {
            if(Modules[idx] == null) return;

            m_AddMaxHealth -= Modules[idx].AddMaxHealth;
            m_AddMaxShield -= Modules[idx].AddMaxShield;
            m_FactorRegenerationShield -= Modules[idx].ChangeRegenerationShieldPercent;
            
            if(Modules[idx].ChangeReloadTimePercent == 0) return;
            
            foreach (var weapon in Weapons)
            {
                weapon?.ChangeFactorReloadTime(-Modules[idx].ChangeReloadTimePercent);
            }
        }

        public void ClearWeapons()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i] = null;
            }
        }

        public void ClearModules()
        {
            m_AddMaxHealth = 0;
            m_AddMaxShield = 0;
            m_FactorRegenerationShield = 1;
            
            foreach (var weapon in Weapons)
            {
                weapon.ResetFactorReloadTime();
            }
            
            for (int i = 0; i < Modules.Length; i++)
            {
                Modules[i] = null;
            }
        }

        public void ClearAllSlots()
        {
            ClearWeapons();
            ClearModules();
        }
    }
}