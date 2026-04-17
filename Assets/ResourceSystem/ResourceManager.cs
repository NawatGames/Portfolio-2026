using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-1000)]
public class ResourceManager : MonoBehaviour
{
    private Dictionary<string, int> Resources = new Dictionary<string,int>();
    public static ResourceManager Instance {get; private set;}

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

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

/// <summary>
/// returns the value of the current resource stored. If it doesn't exists, returns -1
/// </summary>
/// <param name="key"></param>
/// <returns></returns>
    public int GetResource(string key)
    {
        return Resources.ContainsKey(key)? Resources[key] : -1;
    }

    /// <summary>
    /// Adds a new resource to the resources dictionary
    /// </summary>
    /// <param name="key"></param>
    public void NewResource(string key)
    {
        if (!Resources.ContainsKey(key))
        {
            Resources.Add(key,0);
        }
    }

    public void IncreaseResource(string key, int quantity)
    {
        if (Resources.ContainsKey(key))
        {
            Resources[key] += quantity;
        }
    }

    public void DecreaseResource(string key, int quantity)
    {
        if (Resources.ContainsKey(key))
        {
            Resources[key] -= quantity;
        }
    }
}
