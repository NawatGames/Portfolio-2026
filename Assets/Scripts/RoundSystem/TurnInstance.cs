using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class TurnInstance : MonoBehaviour
{
    public event Action OnTurnStart, OnTurnEnd;

    public void StartTurn()
    {
        Debug.Log($"{name} turn started");
        OnTurnStart?.Invoke();
    }
        public void EndTurn()
    {
        Debug.Log($"{name} turn ended");
        OnTurnEnd?.Invoke();
    }
}
