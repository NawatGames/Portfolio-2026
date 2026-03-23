using System.Collections;
using UnityEngine;

public class SecondListener : MonoBehaviour
{
        private void OnEnable()
    {
        if(RoundManager.Instance != null)
        {
            RoundManager.Instance.SubscribeToStart(RoundTurn.player,PlayerTurnStart);
            RoundManager.Instance.SubscribeToEnd(RoundTurn.player,PlayerTurnEnd);
            RoundManager.Instance.SubscribeToStart(RoundTurn.system,SystemTurnStart);
            RoundManager.Instance.SubscribeToEnd(RoundTurn.system,SystemTurnEnd);
        }
    }

        private void OnDisable()
    {
        if(RoundManager.Instance != null)
        {
            RoundManager.Instance.UnsubscribeToStart(RoundTurn.player,PlayerTurnStart);
            RoundManager.Instance.UnsubscribeToEnd(RoundTurn.player,PlayerTurnEnd);
            RoundManager.Instance.UnsubscribeToStart(RoundTurn.system,SystemTurnStart);
            RoundManager.Instance.UnsubscribeToEnd(RoundTurn.system,SystemTurnEnd);
        }
    }

    private void PlayerTurnStart()
    {
        Debug.Log("Ola player");
    }

    private void PlayerTurnEnd()
    {
        Debug.Log("Tchau player");
    }

    private void SystemTurnStart()
    {
        Debug.Log("Ola Sistema");
    }

    private void SystemTurnEnd()
    {
        Debug.Log("Tchau Sistema");
    }
}
