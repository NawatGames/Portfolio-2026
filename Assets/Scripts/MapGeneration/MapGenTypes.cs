using System;
using UnityEngine;

/// <summary>Quatro vizinhos na malha usada pelo WFC e por utilitários de mapa.</summary>
public enum CardinalDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

public static class CardinalDirectionExtensions
{
    public static CardinalDirection Opposite(this CardinalDirection d)
    {
        return (CardinalDirection)(((int)d + 2) % 4);
    }

    public static Vector2Int ToOffset(this CardinalDirection d)
    {
        return d switch
        {
            CardinalDirection.North => new Vector2Int(0, 1),
            CardinalDirection.East => new Vector2Int(1, 0),
            CardinalDirection.South => new Vector2Int(0, -1),
            CardinalDirection.West => new Vector2Int(-1, 0),
            _ => Vector2Int.zero
        };
    }
}

/// <summary>Tipos de terreno colapsados pelo WFC.</summary>
public enum TileTerrainType : byte
{
    Grass = 0,
    Dirt,
    Sand,
    Water,
    Forest,
    Rocky
}

/// <summary>Categorias de pontos de interesse após a geração do terreno.</summary>
public enum MapPointOfInterestKind
{
    ResourceNode,
    EnemySpawn,
    NarrativeEvent
}

[Serializable]
public struct MapPointOfInterest
{
    public Vector2Int Cell;
    public MapPointOfInterestKind Kind;
    public int VariantId;
}