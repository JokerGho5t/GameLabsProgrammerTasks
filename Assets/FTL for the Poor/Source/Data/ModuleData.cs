using UnityEngine;

namespace Ships
{
    [CreateAssetMenu(fileName = "new Module", menuName = "Ships/Create Module")]
    public class ModuleData : ScriptableObject
    {
        [SerializeField] private new string name;
        public string Name => name;
        
        [SerializeField] private float addMaxHealth;
        public float AddMaxHealth => addMaxHealth;
        
        [SerializeField] private float addMaxShield;
        public float AddMaxShield => addMaxShield;
        
        [SerializeField] private float changeReloadTimePercent;
        public float ChangeReloadTimePercent => changeReloadTimePercent / 100;
        
        [SerializeField] private float changeRegenerationShieldPercent;
        public float ChangeRegenerationShieldPercent => changeRegenerationShieldPercent / 100;
        
        public string TextInfo => $"{name}\n" +
                                  $"Add Max Health: {addMaxHealth}\n" +
                                  $"Add Max Shield: {addMaxShield}\n" +
                                  $"Change Reload Time: {changeReloadTimePercent}%\n" +
                                  $"Change Regeneration Shield: {changeRegenerationShieldPercent}%\n";
    }
}