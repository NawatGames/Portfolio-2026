using UnityEngine;
using System.Collections;

public class ResourceGeneratorTest : MonoBehaviour
{
    private int _round = 0;
    private string key_1 = "Pedra", key_2 = "Madeira";
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
        Debug.Log("Madeireira tem: "+ResourceManager.Instance.GetResource(key_2)+ " Madeiras");
        Debug.Log("Pedreira tem: "+ResourceManager.Instance.GetResource(key_1)+ " Pedras");
    }
    private void RoundEnded()
    {
        Debug.Log("Round "+_round+" terminou");
        _round++;
    }
}

