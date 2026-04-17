using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    public event Action<List<SelectableEntity>> OnSelectionChanged;

    private List<SelectableEntity> selectedEntities = new();

    private Vector2 dragStartPosition;
    private bool isDragging;

    private void Start()
    {
        InputManager.Instance.OnLeftClick += HandleClick;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnLeftClick -= HandleClick;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            if (!isDragging)
            {
                dragStartPosition = InputManager.Instance.MousePosition;
                isDragging = true;
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (isDragging)
            {
                SelectInArea(dragStartPosition, InputManager.Instance.MousePosition);
                isDragging = false;
            }
        }
    }

    private void HandleClick()
    {
        Vector2 mousePos = InputManager.Instance.MousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            SelectableEntity entity = hit.collider.GetComponent<SelectableEntity>();

            if (entity != null)
            {
                ClearSelection();

                entity.Select();
                selectedEntities.Add(entity);

                OnSelectionChanged?.Invoke(selectedEntities);
            }
        }
    }

    private void SelectInArea(Vector2 start, Vector2 end)
    {
        ClearSelection();

        SelectableEntity[] allEntities = FindObjectsOfType<SelectableEntity>();

        Rect selectionRect = new Rect(
            Mathf.Min(start.x, end.x),
            Mathf.Min(start.y, end.y),
            Mathf.Abs(start.x - end.x),
            Mathf.Abs(start.y - end.y)
        );

        foreach (var entity in allEntities)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(entity.transform.position);

            if (selectionRect.Contains(screenPos))
            {
                entity.Select();
                selectedEntities.Add(entity);
            }
        }

        OnSelectionChanged?.Invoke(selectedEntities);
    }

    private void ClearSelection()
    {
        foreach (var entity in selectedEntities)
            entity.Deselect();

        selectedEntities.Clear();
    }
}
