using UnityEngine;
using System.Collections;

public class Subscriber : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        Debug.Log("\tRound "+RoundManager.Instance.CurrentRound+" começou");
        RoundManager.Instance.StartNewTurn();
        yield return new WaitForSeconds(2);
        while (true)
        {
            RoundManager.Instance.EndTurn();
            yield return new WaitForSeconds(2);
        }
    }
    private void OnEnable()
    {
        if(RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundChanged += RoundChanged;
            RoundManager.Instance.SubscribeToEnd(RoundTurn.player,TurnEnd);
            RoundManager.Instance.SubscribeToEnd(RoundTurn.system,TurnEnd);
        }
    }

        private void OnDisable()
    {
        if(RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundChanged -= RoundChanged;
            RoundManager.Instance.UnsubscribeToEnd(RoundTurn.player,TurnEnd);
            RoundManager.Instance.UnsubscribeToEnd(RoundTurn.system,TurnEnd);
        }
    }

    private void TurnEnd()
    {
        Debug.Log("\nMudando de turno\n");
    }
    private void RoundChanged(int round)
    {
        int previous = round - 1;
        Debug.Log("\tRound "+previous+" terminou");
        Debug.Log("\tRound "+round+" começou");
    }

}
