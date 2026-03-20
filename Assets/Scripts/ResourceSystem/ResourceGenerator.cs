using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private string _resource;
    [SerializeField] private int _quantityProduced;

    private void OnEnable()
    {
        RoundManager.Instance.StartOfRound += RoundStarted;
        ResourceManager.Instance.NewResource(_resource);
    }

        private void OnDisable()
    {
        RoundManager.Instance.StartOfRound -= RoundStarted;
    }

    private void RoundStarted()
    {
        ResourceManager.Instance.IncreaseResource(_resource,_quantityProduced);
    }
}
