using System;
using UnityEngine;

/// <summary>
/// Singleton with start and end of turn events
/// </summary>
/// <remarks>
/// Events: StartOfRound, EndOfRound <br/>
/// Subscribing to events: RoundManager.Instance.(name of event) += (name of function that is called when event is Triggered) <para/>
/// Functions: RoundStart(), RoundEnd() <br/>
/// Triggering the Events: RoundManager.Instance.Round[Start/End]()
/// </remarks>
public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance {get; private set;}

    public Action StartOfRound, EndOfRound;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void RoundStart()
    {
        StartOfRound?.Invoke();
    }

    public void RoundEnd()
    {
        EndOfRound?.Invoke();
    }
    
    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}
