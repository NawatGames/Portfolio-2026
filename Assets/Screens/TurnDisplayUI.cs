using UnityEngine;
using TMPro;

public class TurnDisplayUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private TextMeshProUGUI _phaseText;

    private void Start()
    {
        UpdateTurnUI();
    }

    private void OnEnable()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.StartOfRound += UpdateTurnUI;
        }
    }

    private void OnDisable()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.StartOfRound -= UpdateTurnUI;
        }
    }

    private void UpdateTurnUI()
    {
        if (RoundManager.Instance == null) return;

        if (_turnText != null)
        {
            _turnText.text = $"TURNO {RoundManager.Instance.CurrentRound}";
        }

        if (_phaseText != null)
        {
            _phaseText.text = RoundManager.Instance.CurrentPhase.ToString().ToUpper();
        }
    }
}