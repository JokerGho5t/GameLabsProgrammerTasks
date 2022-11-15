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
        

        private SignalBus m_SignalBus;
        private bool isDead => Health <= 0;

        public void Init(SignalBus signalBus, ShipData data)
        {
            m_SignalBus = signalBus;
            this.data = data;

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
            
            m_SignalBus.Fire(new SignalUpdateShipInfo(this));
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
            m_SignalBus.Fire(new SignalChangeShipShieldAndHealth(this));
        }
        
        private void OnWeaponFire(WeaponSlot weapon, float damage)
        {
            m_SignalBus.Fire(new SignalMessage(CreateMessage($"Shot from <color=orange>{weapon.Name}</color>")));
            m_SignalBus.Fire(new SignalMessage(
                CreateMessage($"The gun will be reloaded after <color=green>{weapon.ReloadTime}</color>")));
            m_SignalBus.Fire(new SignalWeaponFire(this, damage));
        }

        private string CreateMessage(string message)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(data.ColorMessage)}>{data.Name}</color>: " + message;
        }

        public void TakeDamage(float damage)
        {
            if(isDead) return;
            
            if (Shield > 0)
            {
                if (damage <= Shield)
                {
                    Shield -= damage;
                    m_SignalBus.Fire(
                        new SignalMessage(CreateMessage($"Shields withheld <color=blue>{damage}</color> damage")));
                }
                else
                {
                    Health = Mathf.Clamp(Health - (damage - Shield), 0, MaxHealth);
                    m_SignalBus.Fire(new SignalMessage(CreateMessage(
                        $"The shields were destroyed. The ship received <color=red>{damage - Shield}</color> damage")));
                    
                    Shield = 0;
                }
            }
            else
            {
                Health = Mathf.Clamp(Health - damage, 0, MaxHealth);
                m_SignalBus.Fire(
                    new SignalMessage(CreateMessage($"The ship received <color=red>{damage}</color> damage")));
            }

            m_SignalBus.Fire(new SignalChangeShipShieldAndHealth(this));
            
            if (!isDead) return;

            m_SignalBus.Fire(new SignalMessage(CreateMessage("I was defeated!")));
            m_SignalBus.Fire(new SignalShipDeath(this));
        }

        public void SetWeapon(int idxSlot, WeaponData weaponData)
        {
            Weapons[idxSlot] = new WeaponSlot(weaponData, x => OnWeaponFire(Weapons[idxSlot], x));
            m_SignalBus.Fire(new SignalUpdateShipInfo(this));
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

            m_SignalBus.Fire(new SignalUpdateShipInfo(this));
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
            
            m_SignalBus.Fire(new SignalUpdateShipInfo(this));
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
            
            m_SignalBus.Fire(new SignalUpdateShipInfo(this));
        }

        public void ClearAllSlots()
        {
            ClearWeapons();
            ClearModules();
        }
    }
}