using System;
using UnityEngine;

/// <summary>
/// Hub de eventos de início e fim de round por fase (player/sistema).
/// </summary>
public class RoundEventManager : MonoBehaviour
{
    public static RoundEventManager Instance { get; private set; }

    public event Action<RoundPhaseKind> OnRoundStarted;
    public event Action<RoundPhaseKind> OnRoundEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void NotifyRoundStarted(RoundPhaseKind phase)
    {
        OnRoundStarted?.Invoke(phase);
    }

    public void NotifyRoundEnded(RoundPhaseKind phase)
    {
        OnRoundEnded?.Invoke(phase);
    }
}
