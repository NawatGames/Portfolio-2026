using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Orquestra geração procedural por run: WFC para disposição de tiles, depois espalha POIs em
/// células passáveis.
/// </summary>
public class ProceduralMapGenerator : MonoBehaviour
{
    [Header("Grid WFC (malha lógica — movimento contínuo pode ser outra camada)")]
    [SerializeField] [Min(4)] private int mapWidth = 32;
    [SerializeField] [Min(4)] private int mapHeight = 32;
    [SerializeField] private int generationSeed = 12345;
    [SerializeField] [Min(1)] private int wfcMaxRetries = 1200;

    [Header("Pontos de interesse")]
    [SerializeField] [Min(0)] private int resourceNodes = 6;
    [SerializeField] [Min(0)] private int enemySpawns = 4;
    [SerializeField] [Min(0)] private int narrativeEvents = 2;
    [SerializeField] [Min(1)] private int minManhattanBetweenPois = 3;

    /// <summary>Último resultado válido; somente leitura para outros sistemas.</summary>
    public TileTerrainType[,] LastTerrain { get; private set; }
    public IReadOnlyList<MapPointOfInterest> LastPointsOfInterest => _lastPois;
    public int LastSeedUsed { get; private set; }

    private readonly List<MapPointOfInterest> _lastPois = new List<MapPointOfInterest>();

    private void Awake()
    {
        LastTerrain = new TileTerrainType[mapWidth, mapHeight];
    }

    /// <summary>Gera terreno + POIs. Retorna false se o WFC esgotar tentativas.</summary>
    public bool GenerateNewRun(int? seedOverride = null)
    {
        int w = mapWidth;
        int h = mapHeight;
        var grid = new TileTerrainType[w, h];
        int seed = seedOverride ?? generationSeed;

        bool ok = WaveFunctionCollapse2D.TryGenerate(
            w,
            h,
            seed,
            wfcMaxRetries,
            grid,
            out int usedSeed);

        if (!ok)
        {
            Debug.LogWarning(
                $"ProceduralMapGenerator: WFC falhou após {wfcMaxRetries} tentativas; " +
                "reduza mapWidth/mapHeight, aumente wfcMaxRetries ou relaxe regras de adjacência.");
            return false;
        }

        LastTerrain = grid;
        LastSeedUsed = usedSeed;
        generationSeed = usedSeed + 1;

        _lastPois.Clear();
        PlacePois(resourceNodes, MapPointOfInterestKind.ResourceNode, ref _poiVariant);
        PlacePois(enemySpawns, MapPointOfInterestKind.EnemySpawn, ref _poiVariant);
        PlacePois(narrativeEvents, MapPointOfInterestKind.NarrativeEvent, ref _poiVariant);

        return true;
    }

    private int _poiVariant;

    private void PlacePois(int count, MapPointOfInterestKind kind, ref int variantCounter)
    {
        if (LastTerrain == null || count <= 0)
            return;

        int w = LastTerrain.GetLength(0);
        int h = LastTerrain.GetLength(1);
        var rng = new System.Random(LastSeedUsed + 4096 + (int)kind * 997);

        int attempts = 0;
        int placed = 0;
        while (placed < count && attempts < count * 200)
        {
            attempts++;
            int x = rng.Next(0, w);
            int y = rng.Next(0, h);
            if (!IsPassableCandidate(LastTerrain[x, y]))
                continue;
            var cell = new Vector2Int(x, y);
            if (!HasMinDistance(cell, _lastPois, minManhattanBetweenPois))
                continue;

            _lastPois.Add(new MapPointOfInterest
            {
                Cell = cell,
                Kind = kind,
                VariantId = variantCounter++
            });
            placed++;
        }
    }

    private static bool IsPassableCandidate(TileTerrainType t)
    {
        return t != TileTerrainType.Water && t != TileTerrainType.Rocky;
    }

    private static bool HasMinDistance(
        Vector2Int candidate,
        List<MapPointOfInterest> existing,
        int minDist)
    {
        foreach (var p in existing)
        {
            int md = Mathf.Abs(candidate.x - p.Cell.x) + Mathf.Abs(candidate.y - p.Cell.y);
            if (md < minDist)
                return false;
        }

        return true;
    }

#if UNITY_EDITOR
    [ContextMenu("Debug: Generate map now")]
    private void Editor_GenerateNow()
    {
        GenerateNewRun(null);
        Debug.Log(
            $"[ProceduralMapGenerator] Seed {LastSeedUsed}, POIs: {_lastPois.Count}, grid {mapWidth}x{mapHeight}");
    }
#endif
}