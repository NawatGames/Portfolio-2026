using UnityEngine;
[System.Serializable] 
public struct ResourceAmount
{
    public Resource resource;
    public int amount;
}
[CreateAssetMenu(fileName = "Novo Recurso", menuName = "Scriptable Objects/Resource")]
public class Resource : ScriptableObject
{
    public string resourceName;
    public Sprite icon;
    public int resourseId;
    public float ResourceAmount;

}
