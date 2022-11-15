using UnityEngine;

namespace Ships
{
    [CreateAssetMenu(fileName = "new Ship", menuName = "Ships/Create Ship")]
    public class ShipData : ScriptableObject
    {
        [SerializeField] private new string name;
        public string Name => name;
        
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
        
        [SerializeField] private Color colorMessage;
        public Color ColorMessage => colorMessage;

        [SerializeField, Min(1)] private float maxHealth = 1;
        public float MaxHealth => maxHealth;

        [SerializeField] private float maxShield;
        public float MaxShield => maxShield;

        [SerializeField] private float shieldRegeneration;
        public float ShieldRegeneration => shieldRegeneration;

        [SerializeField, Min(1)] private int weaponSlot = 1;
        public int WeaponSlot => weaponSlot;

        [SerializeField, Min(0)] private int moduleSlot;
        public int ModuleSlot => moduleSlot;
    }
}
