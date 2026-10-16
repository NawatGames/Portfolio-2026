using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SelectedEntity : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Sprite _idle;
    [SerializeField] private Sprite _idleSelected; 
    private SpriteRenderer _spriteRenderer;
    private SelectableEntity _selectableEntity;
    private bool _isSelected;
    void Awake()
    {
        
    }
    void Start()
    {
        if (_idle == null)
        {
            Debug.LogError("Idle sprite not field. Please fill!");
        }
        if (_idleSelected == null)
        {
            Debug.LogError("Idle Selected sprite not field. Please fill!");
        }
        // variavel por causa de erros de localizar Input manager
        if (_inputManager == null){Debug.LogError("InputManger not added");}

        _selectableEntity = GetComponent<SelectableEntity>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isSelected = false;
    }

    private void OnEnable(){
        _inputManager.OnLeftClick += HandleLeftClick;
    }

    private void OnDestroy(){
        if (_inputManager != null){
            _inputManager.OnLeftClick -= HandleLeftClick; // InputManager.Instance.OnLeftClick 
        }
    }

    private void HandleLeftClick()
    {
        if (_selectableEntity.IsSelected)
        {
            if(_isSelected)
            {
                _isSelected = false;
                _spriteRenderer.sprite = _idle;
            }
            else
            {
                _isSelected = true;
                _spriteRenderer.sprite = _idleSelected;
            }
        }
        else if (_isSelected)
        {
            _isSelected = false;
            _spriteRenderer.sprite = _idle;
        }
        else
        {
            _isSelected = false;
            _spriteRenderer.sprite = _idle;          
        }
    }
}
