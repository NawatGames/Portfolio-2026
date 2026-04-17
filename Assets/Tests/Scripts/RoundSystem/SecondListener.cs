using UnityEngine;

public class SecondListener : MonoBehaviour
{
        private void OnEnable()
    {
        if(RoundManager.Instance != null)
        {
            RoundManager.Instance.StartOfRound += RoundStarted;
            RoundManager.Instance.EndOfRound += RoundEnded;
        }
    }

        private void OnDisable()
    {
        if(RoundManager.Instance != null)
        {
            RoundManager.Instance.StartOfRound -= RoundStarted;
            RoundManager.Instance.EndOfRound -= RoundEnded;
        }
    }

    private void RoundStarted()
    {
        Debug.Log("\t Me chamaram no inicio");
    }
    private void RoundEnded()
    {
        Debug.Log("\t Me chamaram no fim");
    }
}
