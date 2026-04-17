using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    public Grid UnityGrid { get; private set; }

    private readonly Dictionary<Vector3Int, GameObject> _objectsOnCells =
        new Dictionary<Vector3Int, GameObject>();

    private readonly Dictionary<Vector3Int, GridTile> _tiles =
        new Dictionary<Vector3Int, GridTile>();

    private void Awake()
    {
        UnityGrid = GetComponent<Grid>();
        if (UnityGrid == null)
        {
            Debug.LogError("GridManager requer um componente Grid no mesmo GameObject.");
        }
    }

    #region Conversões de coordenadas (sistema interno)

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return UnityGrid.WorldToCell(worldPosition);
    }

    public Vector3 CellToWorldCenter(Vector3Int cellPosition)
    {
        // Posição da "origem" da célula
        Vector3 cellWorld = UnityGrid.CellToWorld(cellPosition);

        Vector3 offset = UnityGrid.cellSize * 0.5f;
        return cellWorld + offset;
    }

    #endregion

    #region Gestão de Tiles


    public void RegisterTile(GridTile tile, Vector3Int cellPosition)
    {
        if (tile == null) return;

        tile.CellPosition = cellPosition;
        tile.transform.position = CellToWorldCenter(cellPosition);

        _tiles[cellPosition] = tile;
    }


    public bool TryGetTile(Vector3Int cellPosition, out GridTile tile)
    {
        return _tiles.TryGetValue(cellPosition, out tile);
    }

    #endregion

    #region Posicionar / Obter objetos na grade


    public void PlaceObject(GameObject obj, int x, int y)
    {
        if (obj == null)
        {
            Debug.LogWarning("PlaceObject recebeu um objeto nulo.");
            return;
        }

        Vector3Int cellPos = new Vector3Int(x, y, 0);
        PlaceObject(obj, cellPos);
    }


    public void PlaceObject(GameObject obj, Vector3Int cellPosition)
    {
        if (obj == null)
        {
            Debug.LogWarning("PlaceObject recebeu um objeto nulo.");
            return;
        }

        Vector3 worldPos = CellToWorldCenter(cellPosition);
        obj.transform.position = worldPos;

        _objectsOnCells[cellPosition] = obj;
    }

    public GameObject GetObject(int x, int y)
    {
        Vector3Int cellPos = new Vector3Int(x, y, 0);
        return GetObject(cellPos);
    }

    public GameObject GetObject(Vector3Int cellPosition)
    {
        _objectsOnCells.TryGetValue(cellPosition, out var obj);
        return obj;
    }


    public void ClearCell(int x, int y)
    {
        Vector3Int cellPos = new Vector3Int(x, y, 0);
        _objectsOnCells.Remove(cellPos);
    }

    #endregion
}