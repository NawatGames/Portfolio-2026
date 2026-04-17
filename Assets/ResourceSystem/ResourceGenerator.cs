using UnityEngine;

[DefaultExecutionOrder(-500)]
public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private string _resource;
    [SerializeField] private int _quantityProduced;

    private void Start()
    {
        ResourceManager.Instance.NewResource(_resource);
    }
    private void OnEnable()
    {
        RoundManager.Instance.StartOfRound += RoundStarted;
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
