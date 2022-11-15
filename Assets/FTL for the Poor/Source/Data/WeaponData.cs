using UnityEngine;

namespace Ships
{
    [CreateAssetMenu(fileName = "new Weapon", menuName = "Ships/Create Weapon")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private new string name;
        public string Name => name;
        
        [SerializeField, Min(0)] private float damage;
        public float Damage => damage;
        
        [SerializeField, Min(0)] private float reloadTime;
        public float ReloadTime => reloadTime;

        public string TextInfo => $"{name}\n" +
                                  $"Damage: {damage}\n" +
                                  $"Reload Time: {reloadTime}\n";
    }
}