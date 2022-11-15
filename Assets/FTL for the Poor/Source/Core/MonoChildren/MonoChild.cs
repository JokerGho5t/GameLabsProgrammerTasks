using UnityEngine;

namespace Ships
{
    public abstract class MonoChild : MonoBehaviour
    {
        protected SignalBus signalBus;
        protected DataBase dataBase;

        public virtual void OnStart(SignalBus signalBus, DataBase dataBase)
        {
            this.signalBus = signalBus;
            this.dataBase = dataBase;
        }
        
        public virtual void OnUpdate() { }
    }
}