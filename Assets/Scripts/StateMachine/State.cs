using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.StateMachine
{
    public class State : MonoBehaviour
    {
        public UnityEvent onEnabled;
        public UnityEvent onDisabled;
        
        public void SetEnabled(bool enabled)
        {
            if (enabled)
            {
                onEnabled.Invoke();
            }
            else
            {
                onDisabled.Invoke();
            }
        }
    }
}