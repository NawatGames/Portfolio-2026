using UnityEngine;

[CreateAssetMenu(fileName = "Nova Construção", menuName = "Sistema/Construção")]
public class Building : ScriptableObject
{
    public string buildingName;
    public GameObject prefab;
    public BuildingRequirements requirements;
    
}
