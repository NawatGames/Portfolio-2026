using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourceDisplayUI : MonoBehaviour
{
    [SerializeField] private string _resourceKey;
    [SerializeField] private TextMeshProUGUI _valueText;

    private void Start()
    {
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
            
            int initialVal = ResourceManager.Instance.GetResource(_resourceKey);
            UpdateDisplay(initialVal >= 0 ? initialVal : 0);
        }
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
        }
    }

    private void HandleResourceChanged(string key, int newValue)
    {
        if (key == _resourceKey)
        {
            UpdateDisplay(newValue);
        }
    }

    private void UpdateDisplay(int amount)
    {
        if (_valueText != null)
        {
            _valueText.text = amount.ToString();
        }
    }
}