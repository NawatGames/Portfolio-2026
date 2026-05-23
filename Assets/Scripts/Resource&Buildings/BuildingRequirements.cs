using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Novos Requisitos", menuName = "Sistema/Requisitos de Construção")]
public class BuildingRequirements : ScriptableObject
{

    public List<ResourceAmount> cost;
    public float timeToBuild; 
}
