using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ReachOutline : MonoBehaviour
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
    private float _zPosition;

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
        _zPosition = _gameObject.transform.position.z;

        _line = GetComponent<LineRenderer>();
        _line.positionCount = _segments;
        _line.enabled = false; // desativa a linha para não aparecer na camera enquanto o personagem não for selecionado
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
        float y = 1;

        for (int i = 0; i < _segments; i++){
            _line.SetPosition(i, center + new Vector3(x*range, y*range,0));
            y = x;
            x *= i%2 == 0? -1: 1;
            //     x    y 
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

    private void HandleLeftClick()
    {
        // quando clica no personagem
        if(_selectableEntity.IsSelected){
            // se já estava selecionado -> desseleciona
            if (_isSelected)
            {
                _isSelected = false;
                _line.enabled = false;
            }
            // se não estiver selecionado ainda
            else
            {   
                _isSelected = true;
                DrawSquare();
                _line.enabled = true;
            }
        }
        // após clicar no personagem
        // e a váriavel interna está como ativa
        else if (_isSelected) {
            // pegando posicao do mouse
            Vector2 mousePos = _inputManager.MousePosition; //InputManager.Instance.Mouseposition
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit)){
                // movimentação
                Vector3 worldPos = hit.point;
                Debug.Log("Mouse is at: " + worldPos);
                worldPos.z = _zPosition;
                _movement.MoveTroop(worldPos);
            }
            _isSelected = false;
            _line.enabled = false;
        }
        // se clicar fora da área e tiver clicado no personagem antes, desseleciona no código para não contar mais como ativo
        else {
            _isSelected = false;
        }
    }
}
