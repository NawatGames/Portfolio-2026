using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraControl : MonoBehaviour
    {
        public float dragSpeed = 1f;
        private Vector2 _lastMousePos;
        private bool _isDragging = false;
        
        public float zoomSpeed = 0.5f;
        public float minZoom = 2f;
        public float maxZoom = 15f;
    
        private UnityEngine.Camera _cam;

        private void Awake()
        {
            _cam = GetComponent<UnityEngine.Camera>();

            if (_cam == null)
            {
                Debug.LogError("CameraControl requires a Camera component on the same GameObject.", this);
                enabled = false;
                return;
            }

            ValidateZoomRange();
            
            if (_cam.orthographic)
                _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, minZoom, maxZoom);
        }

        private void OnValidate()
        {
            ValidateZoomRange();
        }

        private void ValidateZoomRange()
        {
            if (minZoom < 0f)
                minZoom = 0f;

            if (maxZoom < minZoom)
                maxZoom = minZoom;
        }

        private void Update()
        {
            if (!_cam)
                return;

            HandlePan();
            HandleZoom();
        }

        private void HandlePan()
        {
            var mouse = Mouse.current;
            if (mouse == null)
                return;

            if (mouse.leftButton.wasPressedThisFrame)
            {
                if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
                    return;

                _lastMousePos = mouse.position.ReadValue();
                _isDragging = true;
            }
            
            if (mouse.leftButton.wasReleasedThisFrame)
            {
                _isDragging = false;
            }

            if (!_isDragging) return;
            var currentMousePos = mouse.position.ReadValue();

            var lastWorldPos = _cam.ScreenToWorldPoint(new Vector3(_lastMousePos.x, _lastMousePos.y, _cam.nearClipPlane));
            var currentWorldPos = _cam.ScreenToWorldPoint(new Vector3(currentMousePos.x, currentMousePos.y, _cam.nearClipPlane));

            var direction = lastWorldPos - currentWorldPos;

            transform.position += direction * dragSpeed;
            _lastMousePos = currentMousePos;
        }

        private void HandleZoom()
        {
            var mouse = Mouse.current;
            if (mouse == null)
                return;

            var scrollValue = mouse.scroll.ReadValue();

            if (scrollValue.y == 0) return;
            float scrollDelta = scrollValue.y > 0 ? 1 : -1;

            if (!_cam.orthographic) return;
            _cam.orthographicSize -= scrollDelta * zoomSpeed;
            _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, minZoom, maxZoom);
        }
    }
}