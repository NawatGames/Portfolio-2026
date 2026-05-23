using UnityEngine;
using System.Collections.Generic;
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<Resource, int> inventory = new Dictionary<Resource, int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddResource(Resource resource, int amount)
    {
        if (!inventory.ContainsKey(resource))
        {
            inventory[resource] = 0;
        }
        inventory[resource] += amount;
        Debug.Log($"Adicionado {amount} {resource.resourceName}. Total: {inventory[resource]}");
    }

    public bool HasEnough(List<ResourceAmount> requirements)
    {
        foreach (var req in requirements)
        {
            if (!inventory.ContainsKey(req.resource) || inventory[req.resource] < req.amount)
            {
                return false; 
            }
        }
        return true;
    }

    public void ConsumeResources(List<ResourceAmount> requirements)
    {
        if (HasEnough(requirements))
        {
            foreach (var req in requirements)
            {
                inventory[req.resource] -= req.amount;
            }
            Debug.Log("Recursos consumidos para construção!");
        }
    }
}
