using System;
using UnityEngine;

namespace Ships
{
    public class WeaponSlot
    {
        private WeaponData data;
        
        public string Name => data.Name;
        public float Damage => data.Damage;
        public float ReloadTime => data.ReloadTime * m_FactorReloadTime;

        private float m_FactorReloadTime;
        private readonly Action<float> m_OnFire;
        private float timerReload;

        public WeaponSlot(WeaponData data, Action<float> onFire)
        {
            this.data = data;
            m_OnFire = onFire;
            
            m_FactorReloadTime = 1;
            
            Recovery();
        }

        public void Recovery()
        {
            timerReload = 0;
        }

        public void Update()
        {
            if (timerReload < ReloadTime) timerReload += Time.deltaTime;
            else Fire();
        }

        private void Fire()
        {
            timerReload = 0;
            m_OnFire?.Invoke(Damage);
        }

        public void ChangeFactorReloadTime(float value)
        {
            m_FactorReloadTime += value;
        }
        
        public void ResetFactorReloadTime()
        {
            m_FactorReloadTime = 1;
        }

        public override bool Equals(object obj)
        {
            if (obj is WeaponData weaponData)
                return data == weaponData;
            
            return base.Equals(obj);
        }
    }
}