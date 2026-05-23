using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    
    public float currentAmount;
    public GeneratorProfile profileInfo;
    public GeneretorDescription desciptionInfo;
    void Update()
    {
        if (currentAmount < profileInfo.maxCapacity)
        {
            currentAmount += profileInfo.amountPerSecond * Time.deltaTime;
            if (currentAmount > profileInfo.maxCapacity)
            {
                currentAmount = profileInfo.maxCapacity;
            }
        }
    }
    public int CollectResource()
    {
        int amountToGive = Mathf.FloorToInt(currentAmount);
        currentAmount -= amountToGive;
        return amountToGive;
    }
}
