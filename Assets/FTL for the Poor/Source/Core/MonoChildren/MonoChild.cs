using UnityEngine;

namespace Ships
{
    public abstract class MonoChild : MonoBehaviour
    {

        public virtual void OnStart() { }
        
        public virtual void OnUpdate() { }
    }
}