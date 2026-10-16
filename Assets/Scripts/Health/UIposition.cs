using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(VidaUI))]
public class UIposition : MonoBehaviour
{
    [SerializeField] private VidaUI _ui;
    [SerializeField] private Vector3 _offset = new Vector3(0,1f,0);
    [SerializeField] private GameObject _targetObject;
    private Transform _objectTransform;
    private RectTransform _uielements;
    private Movement _playerMovement;
    private Coroutine _movementUpdate;

    void Start()
    {
        _objectTransform = _targetObject.transform;
        if(_objectTransform == null)
        {
            Debug.LogError("The transform of the target object could not be assigned.");
        }
        _uielements = GetComponentInChildren<RectTransform>();
        if(_uielements == null)
        {
            Debug.LogError("Ui canvas has no child elements or script was unable to retrive them.");
        }
        _movementUpdate = null;
        UpdateUi(); // atualiza a ui para a ajustar a posição inicial do personagem
    }

    private void OnEnable(){
        if(_targetObject == null)
        {
            Debug.LogError("No target object assigned for the ui to adjust to.");
        }
        _playerMovement = _targetObject.GetComponent<Movement>();
        if(_playerMovement == null)
        {
            Debug.LogError("Target object doesn't have a movement script attached.");
        }
        _playerMovement.MovementStart += StartMoving;
        _playerMovement.MovementEnd += StopMoving;
    }

    private void OnDestroy(){
        if (_playerMovement != null)
        {
            _playerMovement.MovementStart -= StartMoving;
            _playerMovement.MovementEnd -= StopMoving;
        }
    }

    private void StartMoving()
    {
        _movementUpdate = StartCoroutine(MovementTrack());
    }

    private void StopMoving()
    {
        if(_movementUpdate != null)
        {
            StopCoroutine(_movementUpdate);
            _movementUpdate = null;
            UpdateUi(); // chamado no final para evitar erro de atualização caso o personagem se mova muito rapido
        }
    }


    /// <summary>
    /// Coroutine para ficar atualizando a posição enquanto o personagem esta se movendo ao inves de o tempo todo com Update()
    /// </summary>
    /// <returns></returns>
    private IEnumerator MovementTrack()
    {
        while (true)
        {
            UpdateUi();
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Função para só atulizar posição da ui quando o personagem estiver se movendo
    /// </summary>
    /// <returns></returns>
    private void UpdateUi()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_objectTransform.position + new Vector3(0,_objectTransform.lossyScale.y/2f+0) + _offset);
        foreach (RectTransform rect in _uielements)
        {   
            rect.position = screenPosition;
        }
    }
}
