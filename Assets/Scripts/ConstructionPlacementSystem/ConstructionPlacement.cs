using UnityEngine;

/// <summary>
/// Responsável por instanciar e posicionar construções no mapa usando o <see cref="GridManager"/>.
/// </summary>
public class ConstructionPlacement : MonoBehaviour
{
    [SerializeField]
    private GridManager gridManager;

    [SerializeField]
    [Tooltip("Opcional: pai dos prefabs instanciados.")]
    private Transform constructionRoot;

    private void Awake()
    {
        if (gridManager == null)
            gridManager = GetComponent<GridManager>();

        if (gridManager == null)
            gridManager = FindFirstObjectByType<GridManager>();
    }

    /// <summary>
    /// Instancia a construção na célula indicada, se a célula estiver livre.
    /// </summary>
    /// <returns>Falso se a célula já estiver ocupada ou referências forem inválidas.</returns>
    public bool TryPlaceConstruction(ConstructionBase prefab, Vector3Int cellPosition, out ConstructionBase instance)
    {
        instance = null;

        if (prefab == null || gridManager == null)
            return false;

        if (gridManager.GetObject(cellPosition) != null)
            return false;

        ConstructionBase created = Instantiate(prefab, constructionRoot != null ? constructionRoot : transform);
        gridManager.PlaceObject(created.gameObject, cellPosition);
        instance = created;
        return true;
    }

    /// <summary>
    /// Atalho: converte mundo → célula e usa a mesma lógica de <see cref="TryPlaceConstruction"/>.
    /// </summary>
    public bool TryPlaceConstructionAtWorld(ConstructionBase prefab, Vector3 worldPosition, out ConstructionBase instance)
    {
        Vector3Int cell = gridManager.WorldToCell(worldPosition);
        return TryPlaceConstruction(prefab, cell, out instance);
    }
}
