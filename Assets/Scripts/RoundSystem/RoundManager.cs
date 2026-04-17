using System;
using System.Dynamic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Singleton class that controls rounds and turns <para/>
/// This class allows others to subscribe to the player and system round start and end of turn. <br/>
/// It controls the switch between the end of one and start of another, having public calls to change the turn to the other instance. <br/>
/// When a full cycle happens Player-> System -> Player a new round is automatically registered, with the class allowing other to register an action when a new round happens.
/// </summary>
/// <remarks>
/// Event: OnRoundChanged, <br/>
/// Subscribing to event: RoundManager.Instance.(name of event) += (name of function that is called when event is Triggered) <para/>
/// Public Functions: <br/>
/// -> SubscribeToStart, SubscribeToEnd, <br/>
/// -> UnsubscribeToStart, UnsubscribeToEnd, <br/>
/// -> StartNewTurn, EndTurn() <para/>
/// 
/// Subscribing to Instances Events: SubscribeTo[Start/End](RoundTurn.[player/system], action that will listen) <para/>
/// 
/// Start the RoundManager internal system: RoundManager.Instance.StartNewTurn() <br/>
/// Cycling the round and trigerring the events: RoundManager.Instance.EndTurn()
/// </remarks>
[DefaultExecutionOrder(-1000)]
public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance {get; private set;}

    [Header("Turn Instances")]
    [SerializeField] private TurnInstance playerTurn;
    [SerializeField] private TurnInstance systemTurn;

    public RoundTurn RoundState {get; private set;} = RoundTurn.player;
    public int CurrentRound {get;private set;} = 1;

    private bool started = false;

    public event Action<int> OnRoundChanged;

    private void Awake()
    {
        if(playerTurn == null)
        {
            UnityEngine.Debug.LogError("playerTurn instance unattributed");
            return;
        }
        if(systemTurn == null)
        {
            UnityEngine.Debug.LogError("systemTurn instance unattributed");
            return;
        }
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    
    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    private void EndPlayerTurn()
    {
        playerTurn.EndTurn();
        StartSystemTurn();
    }

    private void StartSystemTurn()
    {
        systemTurn.StartTurn();
        RoundState = RoundTurn.system;
    }

    private void EndSystemTurn()
    {
        systemTurn.EndTurn();

        CurrentRound++;
        OnRoundChanged?.Invoke(CurrentRound);

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        playerTurn.StartTurn();
        RoundState = RoundTurn.player;
    }

    // functions that can be called by others

    // subcription
    public void SubscribeToStart(RoundTurn option, Action listener)
    {
         switch (option)
        {
            case RoundTurn.player: 
                playerTurn.OnTurnStart += listener;
                break;
            case RoundTurn.system: 
                systemTurn.OnTurnStart += listener;
                break;
        }
    }

    public void SubscribeToEnd(RoundTurn option, Action listener)
    {
         switch (option)
        {
            case RoundTurn.player: 
                playerTurn.OnTurnEnd += listener;
                break;
            case RoundTurn.system: 
                systemTurn.OnTurnEnd += listener;
                break;
        }
    }

    public void UnsubscribeToStart(RoundTurn option, Action listener)
    {
         switch (option)
        {
            case RoundTurn.player: 
                playerTurn.OnTurnStart -= listener;
                break;
            case RoundTurn.system: 
                systemTurn.OnTurnStart -= listener;
                break;
        }
    }

    public void UnsubscribeToEnd(RoundTurn option, Action listener)
    {
         switch (option)
        {
            case RoundTurn.player: 
                playerTurn.OnTurnEnd -= listener;
                break;
            case RoundTurn.system: 
                systemTurn.OnTurnEnd -= listener;
                break;
        }
    }

    // calls

/// <summary>
/// Function called only to start the round manager cycle
/// </summary>
    public void StartNewTurn()
    {
        if (!started)
        {
            StartPlayerTurn();
            started = true;
        }
    }

/// <summary>
/// Function used to cycle turns and rounds
/// </summary>
    public void EndTurn()
    {
        switch (RoundState)
        {
            case RoundTurn.player: 
                EndPlayerTurn();
                break;
            case RoundTurn.system: 
                EndSystemTurn();
                break;
        }
    }
}
