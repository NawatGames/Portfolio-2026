using UnityEngine;

/// <summary>
/// Base para construções no mapa que reagem a início/fim de round.
/// </summary>
public class ConstructionBase : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Define se esta construção reage ao round do jogador ou ao round do sistema.")]
    private RoundPhaseKind roundSubscription = RoundPhaseKind.Player;

    protected RoundPhaseKind RoundSubscription => roundSubscription;

    protected virtual void OnEnable()
    {
        if (RoundEventManager.Instance == null)
            return;

        RoundEventManager.Instance.OnRoundStarted += HandleRoundStarted;
        RoundEventManager.Instance.OnRoundEnded += HandleRoundEnded;
    }

    protected virtual void OnDisable()
    {
        if (RoundEventManager.Instance == null)
            return;

        RoundEventManager.Instance.OnRoundStarted -= HandleRoundStarted;
        RoundEventManager.Instance.OnRoundEnded -= HandleRoundEnded;
    }

    private void HandleRoundStarted(RoundPhaseKind phase)
    {
        if (phase != roundSubscription)
            return;

        OnRoundStarted();
    }

    private void HandleRoundEnded(RoundPhaseKind phase)
    {
        if (phase != roundSubscription)
            return;

        OnRoundEnded();
    }

    /// <summary>Override nas subclasses para lógica no início do round.</summary>
    protected virtual void OnRoundStarted() { }

    /// <summary>Override nas subclasses para lógica no fim do round.</summary>
    protected virtual void OnRoundEnded() { }
}
