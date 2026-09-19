using UnityEngine;

public class TurnButtonController : MonoBehaviour
{
    public void FinalizarTurno()
    {
        if (RoundManager.Instance == null)
        {
            Debug.LogWarning("RoundManager não encontrado na cena!");
            return;
        }
        
        RoundManager.Instance.RoundEnd();

        RoundManager.Instance.RoundStart();
    }
}