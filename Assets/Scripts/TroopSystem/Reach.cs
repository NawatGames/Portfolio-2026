using Unity.VisualScripting;
using UnityEngine;

public class Reach : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    public float radius 
    {
        get => _radius;
        private set => _radius = value;
    }

    public bool IsInReach(Vector3 pos){
        Vector3 center = gameObject.transform.position;
        
        float minusX = center.x - _radius;
        float greaterX = center.x + _radius;

        float minusZ = center.z - _radius;
        float greaterZ = center.z + _radius;

        float minusY = center.y - _radius;
        float greaterY = center.y + _radius;

        return (
            pos.x >= minusX &&
            pos.x <= greaterX 
            &&
            pos.z >= minusZ &&
            pos.z <= greaterZ 
            &&
            pos.y >= minusY &&
            pos.y <= greaterY
            );
    }
}
