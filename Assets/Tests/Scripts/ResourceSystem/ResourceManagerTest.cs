using UnityEngine;

public class ResourceManagerTest : MonoBehaviour
{
    public static string key = "Pedra";
    void Start()
    {
        Debug.Log("Recurso "+key + "Ainda não adicionado");
        Debug.Log(ResourceManager.Instance.GetResource(key));
        ResourceManager.Instance.NewResource(key);
        Debug.Log("Recurso "+key + "adicionado");
        ResourceManager.Instance.NewResource(key);
        Debug.Log("Teste de adicionar denovo");
        Debug.Log("Pegando valor em "+ key);
        Debug.Log(ResourceManager.Instance.GetResource(key));
        Debug.Log("Adicionando 5");
        ResourceManager.Instance.IncreaseResource(key,5);
        Debug.Log(ResourceManager.Instance.GetResource(key));
        Debug.Log("Retirando 2: experado retornar 3");
        ResourceManager.Instance.DecreaseResource(key,2);
        Debug.Log(ResourceManager.Instance.GetResource(key));
    }


}
