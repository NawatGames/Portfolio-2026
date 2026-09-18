using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Reach _reach;

    private GameObject _gameObject;

    private void Start(){
        if (_reach == null){
            try{_reach = GetComponent<Reach>();}
            catch{Debug.LogError("Unable to find Reach script");}
        }
        _gameObject = gameObject;
    }
    public void MoveTroop(Vector3 pos){
        if (_reach.IsInReach(pos)){
            _gameObject.transform.position = pos;
            Debug.Log("Moving");
        }
    }
}
