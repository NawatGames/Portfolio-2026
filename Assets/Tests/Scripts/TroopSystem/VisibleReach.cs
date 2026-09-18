using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VisibleReach : MonoBehaviour
{
    [SerializeField] private Reach _reach;
    [SerializeField] private Movement _movement;

    // variavel por causa de erros de localizar Input manager
    [SerializeField] private InputManager _inputManager;
    private int _segments = 4; // quadrado

    private LineRenderer _line;

    private bool _isSelected;
    private SelectableEntity _selectableEntity;
    private GameObject _gameObject;
    private float _yPosition;


    // highlight
    public Color highlightColor = Color.yellow;
    private Color originalColor;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        
    }
    void Start()
    {
        if (_reach == null){
            try{_reach = GetComponent<Reach>();}
            catch{Debug.LogError("Unable to find Reach script");}
        }
        if (_movement == null){
            try{_movement = GetComponent<Movement>();}
            catch{Debug.LogError("Unable to find Movement script");}
        }

         // variavel por causa de erros de localizar Input manager
         if (_inputManager == null){Debug.LogError("InputManger not added");}

        _gameObject = gameObject;
        _isSelected = false;
        _selectableEntity = GetComponent<SelectableEntity>();
        _yPosition = _gameObject.transform.position.y;

        _line = GetComponent<LineRenderer>();
        _line.positionCount = _segments;
        _line.enabled = false; // desativa a linha para não aparecer na camera enquanto o personagem não for selecionado
        // highlight
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    private void OnEnable(){
        _inputManager.OnLeftClick += HandleLeftClick;
    }

    private void OnDestroy(){
        if (_inputManager != null){
            _inputManager.OnLeftClick -= HandleLeftClick; // InputManager.Instance.OnLeftClick 
        }
    }

    private void DrawSquare(){
        float halfWidth = _line.startWidth/2f;
        float range = _reach.radius + halfWidth;
        Vector3 center = transform.position;
        float x = 1;
        float z = 1;

        for (int i = 0; i < _segments; i++){
            _line.SetPosition(i, center + new Vector3(x*range, 0,z*range));
            z = x;
            x *= i%2 == 0? -1: 1;
            //     x    z 
            // 0:  1    1
            // 1: -1    1
            // 2: -1   -1
            // 3:  1   -1
        }

        _line.loop = true;
        _line.useWorldSpace = true;
        _line.startColor = Color.green;
        _line.endColor = Color.green;
        _line.enabled = false;
    }


    private void HandleLeftClick(){
        if(_selectableEntity.IsSelected){
            _isSelected = true;
            DrawSquare();
            _line.enabled = true;
            meshRenderer.material.color = highlightColor;
        }
        else if (_isSelected) {
            // pegando posicao do mouse
            Vector2 mousePos = _inputManager.MousePosition; //InputManager.Instance.Mouseposition
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit)){
                // movimentação
                Vector3 worldPos = hit.point;
                Debug.Log("Mouse is at: " + worldPos);
                worldPos.y = _yPosition;
                _movement.MoveTroop(worldPos);
            }
            _isSelected = false;
            _line.enabled = false;
            meshRenderer.material.color = originalColor;
        }
        else {
            _isSelected = false;
            meshRenderer.material.color = originalColor;
        }
    }

}
