using UnityEngine;
using System;
using System.Collections.Generic;

[DefaultExecutionOrder(-1000)]
public class ResourceManager : MonoBehaviour
{
    private Dictionary<string, int> Resources = new Dictionary<string, int>();
    public static ResourceManager Instance { get; private set; }

    // Evento disparado passando (resourceKey, novoValor)
    public event Action<string, int> OnResourceChanged;

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

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public int GetResource(string key) => Resources.ContainsKey(key) ? Resources[key] : -1;

    public void NewResource(string key)
    {
        if (!Resources.ContainsKey(key))
        {
            Resources.Add(key, 0);
            OnResourceChanged?.Invoke(key, 0);
        }
    }

    public void IncreaseResource(string key, int quantity)
    {
        if (Resources.ContainsKey(key))
        {
            Resources[key] += quantity;
            OnResourceChanged?.Invoke(key, Resources[key]);
        }
    }

    public void DecreaseResource(string key, int quantity)
    {
        if (Resources.ContainsKey(key))
        {
            Resources[key] -= quantity;
            OnResourceChanged?.Invoke(key, Resources[key]);
        }
    }
}