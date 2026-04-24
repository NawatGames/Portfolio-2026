using UnityEngine;

namespace DefaultNamespace.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private State initialState;
        
        private State currentState;

        public void SetState(State newState)
        {
            if (currentState != null)
            {
                currentState.SetEnabled(false);
            }
            
            currentState = newState;
            currentState.SetEnabled(true);
        }
    }
}