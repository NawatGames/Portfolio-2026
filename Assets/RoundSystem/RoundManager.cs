using System;
using UnityEngine;

public enum DayNightCycle
{
    Dia,
    Noite
}

[DefaultExecutionOrder(-1000)]
public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    public Action StartOfRound, EndOfRound;

    [field: SerializeField] public int CurrentRound { get; private set; } = 1;
    [field: SerializeField] public DayNightCycle CurrentPhase { get; private set; } = DayNightCycle.Dia;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Inicia uma nova rodada incrementando o contador, mudando o ciclo de Dia/Noite e disparando o evento StartOfRound.
    /// </summary>
    public void RoundStart()
    {
        if (CurrentPhase == DayNightCycle.Dia)
        {
            CurrentPhase = DayNightCycle.Noite;
        }
        else
        {
            CurrentPhase = DayNightCycle.Dia;
            CurrentRound++;
        }

        StartOfRound?.Invoke();
    }

    /// <summary>
    /// Finaliza a rodada atual.
    /// </summary>
    public void RoundEnd()
    {
        EndOfRound?.Invoke();
    }

    /// <summary>
    /// Alterna entre Dia e Noite (se aplicável ao ciclo do jogo)
    /// </summary>
    public void TogglePhase()
    {
        CurrentPhase = (CurrentPhase == DayNightCycle.Dia) ? DayNightCycle.Noite : DayNightCycle.Dia;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}