using System.Collections.Generic;
using UnityEngine;

namespace Ships
{
    [CreateAssetMenu(fileName = "new DataBase", menuName = "Ships/Create DataBase", order = 0)]
    public class DataBase : ScriptableObject
    {
        [SerializeField] private ShipData shipDataA;
        public ShipData ShipDataA => shipDataA;
        
        [SerializeField] private ShipData shipDataB;
        public ShipData ShipDataB => shipDataB;

        [SerializeField] private List<WeaponData> weapons;
        public List<WeaponData> Weapons => weapons;

        [SerializeField] private List<ModuleData> modules;
        public List<ModuleData> Modules => modules;
    }
}