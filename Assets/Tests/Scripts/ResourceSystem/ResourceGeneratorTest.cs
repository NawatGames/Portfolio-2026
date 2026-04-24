using UnityEngine;
using System.Collections;
using System;

public class ResourceGeneratorTest : MonoBehaviour
{
    private string key_1 = "Pedra", key_2 = "Madeira";
    private void Start()
    {
        RoundManager.Instance.StartNewTurn();
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
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
            RoundManager.Instance.OnRoundChanged += RoundChange;
            RoundManager.Instance.SubscribeToStart(RoundTurn.player,RoundStarted);
            RoundManager.Instance.SubscribeToEnd(RoundTurn.player,RoundEnded);
        }
    }

        private void OnDisable()
    {
        if(RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundChanged -= RoundChange;
            RoundManager.Instance.UnsubscribeToStart(RoundTurn.player,RoundStarted);
            RoundManager.Instance.UnsubscribeToEnd(RoundTurn.player,RoundEnded);
        }
    }

    private void RoundChange(int round)
    {
        Debug.Log("/////////////////////////\n"
        +"Round: "+round+"\t turno: "+RoundManager.Instance.RoundState+
        "\n/////////////////////////");
    }
    private void RoundStarted()
    {
        Debug.Log("Madeireira tem: "+ResourceManager.Instance.GetResource(key_2)+ " Madeiras");
        Debug.Log("Pedreira tem: "+ResourceManager.Instance.GetResource(key_1)+ " Pedras");
    }
    private void RoundEnded()
    {
        Debug.Log("Round: "+RoundManager.Instance.CurrentRound+"\t turno: "+RoundManager.Instance.RoundState+" terminou");
    }
}

