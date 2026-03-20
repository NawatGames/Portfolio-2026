using UnityEngine;
using System.Collections;

public class Subscriber : MonoBehaviour
{
    private int _round = 0;
    private void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            RoundManager.Instance.RoundStart();
            yield return new WaitForSeconds(2);
            RoundManager.Instance.RoundEnd();
            yield return new WaitForSeconds(1);
        }
    }
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
        Debug.Log("Round "+_round+" começou");
    }
    private void RoundEnded()
    {
        Debug.Log("Round "+_round+" terminou");
        _round++;
    }
}
