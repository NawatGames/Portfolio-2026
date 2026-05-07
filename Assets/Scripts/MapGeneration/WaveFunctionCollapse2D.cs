using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wave Function Collapse 2D: cada célula mantém um conjunto de tipos possíveis; escolhe-se
/// a célula de menor entropia, colapsa-se para um tipo e propaga-se restrições aos vizinhos.
/// </summary>
public static class WaveFunctionCollapse2D
{
    public const int MaxRetryDefault = 80;

    /// <summary>
    /// Gera uma grade width×height. Em contradição (domínio vazio), tenta novo seed interno
    /// </summary>
    public static bool TryGenerate(
        int width,
        int height,
        int seed,
        int maxRetries,
        TileTerrainType[,] outGrid,
        out int usedSeed)
    {
        var allTypes = (TileTerrainType[])Enum.GetValues(typeof(TileTerrainType));
        var allowed = BuildDefaultAllowedNeighbors();

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            int runSeed = seed + attempt;
            var rng = new System.Random(runSeed);

            var domains = new HashSet<TileTerrainType>[width, height];
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                domains[x, y] = new HashSet<TileTerrainType>(allTypes);
            }

            bool success = SolveWithBacktracking(width, height, domains, allowed, rng);
            if (!success)
                continue;

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                outGrid[x, y] = FirstOrDefault(domains[x, y]);
            }

            usedSeed = runSeed;
            return true;
        }

        usedSeed = seed;
        return false;
    }

    private static TileTerrainType FirstOrDefault(HashSet<TileTerrainType> set)
    {
        foreach (var t in set)
            return t;
        return default;
    }

    private static bool SolveWithBacktracking(
        int width,
        int height,
        HashSet<TileTerrainType>[,] domains,
        Dictionary<(TileTerrainType, CardinalDirection), HashSet<TileTerrainType>> allowed,
        System.Random rng)
    {
        bool hasContradiction =
            HasAnyEmptyDomain(width, height, domains);
        if (hasContradiction)
            return false;

        if (!TrySelectCellWithMinEntropy(width, height, domains, rng, out int x, out int y))
            return true;

        var options = new List<TileTerrainType>(domains[x, y]);
        Shuffle(options, rng);

        foreach (var chosen in options)
        {
            var branch = CloneDomains(width, height, domains);
            branch[x, y].Clear();
            branch[x, y].Add(chosen);

            if (!PropagateFrom(x, y, width, height, branch, allowed))
                continue;

            if (SolveWithBacktracking(width, height, branch, allowed, rng))
            {
                CopyDomains(width, height, branch, domains);
                return true;
            }
        }

        return false;
    }

    private static bool TrySelectCellWithMinEntropy(
        int width,
        int height,
        HashSet<TileTerrainType>[,] domains,
        System.Random rng,
        out int selectedX,
        out int selectedY)
    {
        selectedX = -1;
        selectedY = -1;
        int bestEntropy = int.MaxValue;
        var ties = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            int e = domains[x, y].Count;
            if (e <= 1)
                continue;

            if (e < bestEntropy)
            {
                bestEntropy = e;
                ties.Clear();
                ties.Add(new Vector2Int(x, y));
            }
            else if (e == bestEntropy)
            {
                ties.Add(new Vector2Int(x, y));
            }
        }

        if (ties.Count == 0)
            return false;

        var pick = ties[rng.Next(ties.Count)];
        selectedX = pick.x;
        selectedY = pick.y;
        return true;
    }

    private static bool HasAnyEmptyDomain(
        int width,
        int height,
        HashSet<TileTerrainType>[,] domains)
    {
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            if (domains[x, y].Count == 0)
                return true;
        }

        return false;
    }

    private static HashSet<TileTerrainType>[,] CloneDomains(
        int width,
        int height,
        HashSet<TileTerrainType>[,] source)
    {
        var clone = new HashSet<TileTerrainType>[width, height];
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
            clone[x, y] = new HashSet<TileTerrainType>(source[x, y]);
        return clone;
    }

    private static void CopyDomains(
        int width,
        int height,
        HashSet<TileTerrainType>[,] from,
        HashSet<TileTerrainType>[,] to)
    {
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            to[x, y].Clear();
            foreach (var v in from[x, y])
                to[x, y].Add(v);
        }
    }

    private static void Shuffle<T>(List<T> list, System.Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private static TileTerrainType PickWeighted(HashSet<TileTerrainType> domain, System.Random rng)
    {
        // Pesos simples
        float total = 0f;
        foreach (var t in domain)
            total += WeightFor(t);

        float r = (float)rng.NextDouble() * total;
        foreach (var t in domain)
        {
            r -= WeightFor(t);
            if (r <= 0f)
                return t;
        }

        foreach (var t in domain)
            return t;
        return TileTerrainType.Grass;
    }

    private static float WeightFor(TileTerrainType t)
    {
        return t switch
        {
            TileTerrainType.Water => 0.35f,
            TileTerrainType.Forest => 0.75f,
            TileTerrainType.Rocky => 0.6f,
            TileTerrainType.Sand => 0.9f,
            TileTerrainType.Dirt => 1f,
            TileTerrainType.Grass => 1.2f,
            _ => 1f
        };
    }

    private static bool PropagateFrom(
        int startX,
        int startY,
        int width,
        int height,
        HashSet<TileTerrainType>[,] domains,
        Dictionary<(TileTerrainType, CardinalDirection), HashSet<TileTerrainType>> allowed)
    {
        var queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));

        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            int x = cur.x;
            int y = cur.y;

            foreach (CardinalDirection dir in Enum.GetValues(typeof(CardinalDirection)))
            {
                var off = dir.ToOffset();
                int nx = x + off.x;
                int ny = y + off.y;
                if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                    continue;

                if (!ConstrainNeighbor(
                        domains[x, y],
                        domains[nx, ny],
                        dir,
                        allowed,
                        out bool changed))
                    return false;

                if (changed)
                    queue.Enqueue(new Vector2Int(nx, ny));
            }
        }

        return true;
    }

    /// <summary>
    /// Reduz o domínio do vizinho ao que é compatível com qualquer opção ainda possível na
    /// célula atual, na direção dada.
    /// </summary>
    private static bool ConstrainNeighbor(
        HashSet<TileTerrainType> fromDomain,
        HashSet<TileTerrainType> neighborDomain,
        CardinalDirection dirFromCurrentToNeighbor,
        Dictionary<(TileTerrainType, CardinalDirection), HashSet<TileTerrainType>> allowed,
        out bool domainChanged)
    {
        domainChanged = false;
        var union = new HashSet<TileTerrainType>();
        foreach (var fromTile in fromDomain)
        {
            if (allowed.TryGetValue((fromTile, dirFromCurrentToNeighbor), out var opts))
            {
                foreach (var o in opts)
                    union.Add(o);
            }
        }

        if (union.Count == 0)
        {
            if (neighborDomain.Count == 0)
                return true;
            domainChanged = true;
            neighborDomain.Clear();
            return false;
        }

        var remove = new List<TileTerrainType>();
        foreach (var n in neighborDomain)
        {
            if (!union.Contains(n))
                remove.Add(n);
        }

        if (remove.Count == 0)
            return true;

        domainChanged = true;
        foreach (var r in remove)
            neighborDomain.Remove(r);

        return neighborDomain.Count > 0;
    }

    /// <summary>
    /// Regras de borda: o que pode ficar ao norte/leste/sul/oeste de cada tipo.
    /// </summary>
    public static Dictionary<(TileTerrainType, CardinalDirection), HashSet<TileTerrainType>>
        BuildDefaultAllowedNeighbors()
    {
        void Add(
            Dictionary<(TileTerrainType, CardinalDirection), HashSet<TileTerrainType>> map,
            TileTerrainType a,
            CardinalDirection d,
            TileTerrainType b)
        {
            var key = (a, d);
            if (!map.TryGetValue(key, out var set))
            {
                set = new HashSet<TileTerrainType>();
                map[key] = set;
            }

            set.Add(b);
        }

        var m =
            new Dictionary<(TileTerrainType, CardinalDirection), HashSet<TileTerrainType>>();

        // Conectividade "ilha + interior": água liga água e areia; areia faz transição;
        // vegetação e rochas ligam terra firme.
        foreach (var dir in new[] { CardinalDirection.North, CardinalDirection.East,
                     CardinalDirection.South, CardinalDirection.West })
        {
            Add(m, TileTerrainType.Water, dir, TileTerrainType.Water);
            Add(m, TileTerrainType.Water, dir, TileTerrainType.Sand);

            Add(m, TileTerrainType.Sand, dir, TileTerrainType.Sand);
            Add(m, TileTerrainType.Sand, dir, TileTerrainType.Water);
            Add(m, TileTerrainType.Sand, dir, TileTerrainType.Grass);
            Add(m, TileTerrainType.Sand, dir, TileTerrainType.Dirt);

            Add(m, TileTerrainType.Grass, dir, TileTerrainType.Grass);
            Add(m, TileTerrainType.Grass, dir, TileTerrainType.Dirt);
            Add(m, TileTerrainType.Grass, dir, TileTerrainType.Sand);
            Add(m, TileTerrainType.Grass, dir, TileTerrainType.Forest);

            Add(m, TileTerrainType.Dirt, dir, TileTerrainType.Dirt);
            Add(m, TileTerrainType.Dirt, dir, TileTerrainType.Grass);
            Add(m, TileTerrainType.Dirt, dir, TileTerrainType.Forest);
            Add(m, TileTerrainType.Dirt, dir, TileTerrainType.Rocky);

            Add(m, TileTerrainType.Forest, dir, TileTerrainType.Forest);
            Add(m, TileTerrainType.Forest, dir, TileTerrainType.Grass);
            Add(m, TileTerrainType.Forest, dir, TileTerrainType.Dirt);

            Add(m, TileTerrainType.Rocky, dir, TileTerrainType.Rocky);
            Add(m, TileTerrainType.Rocky, dir, TileTerrainType.Dirt);
        }

        return m;
    }
}