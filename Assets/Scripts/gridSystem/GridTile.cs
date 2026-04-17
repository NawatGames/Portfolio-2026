using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Vector3Int CellPosition { get; internal set; }
    public bool walkable = true;
}