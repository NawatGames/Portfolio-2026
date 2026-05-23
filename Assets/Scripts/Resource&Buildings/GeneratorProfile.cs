using UnityEngine;
using UnityEngine.Animations;

[CreateAssetMenu(fileName = "GeneratorProfile", menuName = "Scriptable Objects/GeneratorProfile")]
public class GeneratorProfile : ScriptableObject
{
    public float maxCapacity;
    public float amountPerSecond;
    public Resource resourceGenerated;

}
